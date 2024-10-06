using Microsoft.AspNetCore.Mvc;
using Task_Manager.Models;

namespace Task_Manager.Controllers
{
    [SessionAuthorize]
    public class DepartmentsController : Controller
    {
        private readonly TaskManagerDAL _DAL;
        public DepartmentsController(TaskManagerDAL dal)
        {
            _DAL = dal;
        }

        public IActionResult Index()
        {
            var model = new departments();


            model.departmentList = _DAL.GetDepartmentList();
            model.employees = _DAL.GetEmployeeIdList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new department();
            model.EmployeeIdOptions = _DAL.GetEmployeeIdList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(department dep)
        {
            _DAL.AddDepartment(dep);

            return RedirectToAction("index", "Tasks");
        }
        [HttpPost]
        public IActionResult UpdateDepartmentHead(department dep)
        {
            _DAL.UpdateDepartment(dep.Id, dep.headID);
            return RedirectToAction("index", "Departments");
        }
    }
}
