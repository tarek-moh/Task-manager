using Microsoft.AspNetCore.Mvc;
using Task_Manager.Models;

namespace Task_Manager.Controllers
{
    [SessionAuthorize]

    public class EmployeesController : Controller
    {
        private readonly TaskManagerDAL _DAL;
        public EmployeesController(TaskManagerDAL dal)
        {
            _DAL = dal;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> List()
        {
            var model = await _DAL.GetDepartmentEmployeeList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _DAL.DeleteEmployeeAndUpdateTasksAsync(id);

            if (result)
            {
                return RedirectToAction("List"); 
            }

            return BadRequest("Failed to delete employee.");
        }

    }
}
