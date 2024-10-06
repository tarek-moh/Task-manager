using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Task_Manager.Models;
using BC = BCrypt.Net.BCrypt;

namespace Task_Manager.Controllers
{
    public class AccountsController : Controller
    {
        private readonly TaskManagerDAL _DAL;
        public AccountsController(TaskManagerDAL dal)
        {
            _DAL = dal;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Task_Manager.Models.account account)
        {
            string hash = await _DAL.GetHashedPassword(account.email);
            if(hash != null && BC.Verify(account.password, hash))
            {
                Task_Manager.Models.Employee employee = await _DAL.GetEmployeeByEmail(account.email);

                string isManger;
                if (employee.DepartmentId == null)
                {
                    isManger = "false";
                }
                else
                {
                    isManger = _DAL.isManager(employee.Id, employee.DepartmentId).ToString().ToLower();
                }

                //save the rest of employee Data in the future

                HttpContext.Session.SetString("authenticated", "valid");
                HttpContext.Session.SetString("myId", employee.Id.ToString());
                HttpContext.Session.SetString("departmentId", employee.DepartmentId.ToString());
                HttpContext.Session.SetString("isManager", isManger);
                HttpContext.Session.SetString("fName", employee.FirstName);
                HttpContext.Session.SetString("lName", employee.LastName);


                TempData["valid"] = "true";
                TempData["myId"]=employee.Id.ToString();
                TempData["isManager"] = isManger;
                TempData["fName"] = employee.FirstName;
                TempData["lName"] = employee.LastName;

                bool isAdmin = _DAL.isAdmin(account.email);
                if (isAdmin)
                {
                    HttpContext.Session.SetString("isAdmin", isAdmin.ToString().ToLower());
                    TempData["isAdmin"] = isAdmin.ToString().ToLower();

                }
                else
                {
                    HttpContext.Session.SetString("isAdmin", isAdmin.ToString().ToLower());
                    TempData["isAdmin"] = isAdmin.ToString().ToLower();

                }

                return RedirectToAction("Index", "Tasks");
            }
            else
            {
                TempData["valid"] = "false";
                HttpContext.Session.SetString("authenticated", "inValid");
                return View(account);
            }
        }

        public ActionResult Add()
        {
            var departments = _DAL.GetDepartmentList();
            ViewBag.Departments = new SelectList(departments, "Id", "Name"); 
            return View();
        }

        [HttpPost]
        public ActionResult Add(Task_Manager.Models.Employee employee)
        {
            if (ModelState.IsValid)
            {

                if (_DAL.isEmployeeExist(employee.Email))
                { 
                    ModelState.AddModelError("Email", "An employee with this email already exists.");
                }
                else
                {
                    _DAL.AddEmployee(employee);
                    return RedirectToAction("List", "Employees");
                }

            }
            var departments = _DAL.GetDepartmentList();
            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            return View(employee);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData.Clear();
            return RedirectToAction("Login", "Accounts");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        async public Task<ActionResult> ChangePassword(Task_Manager.Models.ChangePassword acc)
        {
            string? hash = await _DAL.GetHashedPassword(acc.email);
            if (hash != null && BC.Verify(acc.password, hash))
            {
                if(acc.confirmPassword == acc.newPassword)
                {
                    string newHash = BC.HashPassword(acc.newPassword);
                    _DAL.ChangePassword(acc.email, newHash);
                    return RedirectToAction("Index", "Tasks");
                }
                else
                {
                    ModelState.AddModelError("confirmPassword", "confirmation password doesn't match your new password");
                    return View(acc);
                }
            }
            else
            {
                ModelState.AddModelError("email", "Incorrect email or password");
                return View(acc);
            }

        }
    }
}
