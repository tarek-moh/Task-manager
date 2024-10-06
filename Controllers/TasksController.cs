using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace Task_Manager.Controllers
{
    [SessionAuthorize]
    public class TasksController : Controller
    {
        private readonly TaskManagerDAL _DAL;
        public TasksController(TaskManagerDAL dal)
        {
            _DAL = dal;
        }
        public ActionResult Index(int priority = 0, string status = "All")
        {
            List<Task_Manager.Models.Task> tasks;

            if (HttpContext.Session.GetString("isAdmin") == "true")
            {
                tasks = _DAL.getTasksList();
            }
            else
            {
                string myIdString = HttpContext.Session.GetString("myId");
                string depIdString = HttpContext.Session.GetString("departmentId");

                if (int.TryParse(myIdString, out int myId) && int.TryParse(depIdString, out int depId))
                {
                    tasks = _DAL.GetMyTasks(myId,depId);
                }
                else
                {
                    return RedirectToAction("Error with user ID"); 
                }
            }

            if (priority != 0)
            {
                tasks = tasks.Where(t => t.Priority == priority).ToList();
            }
            if (status != "All")
            {
                tasks = tasks.Where(t => t.Status == status).ToList();
            }
            return View(tasks);
        }

        public ActionResult Add()
        {
            var model = new Task_Manager.Models.Task()
            {
                DueDate = DateTime.Now // Set default to current date
            };


            // Populate other properties if needed
            model.StatusOptions = GetStatusList();
            model.PriorityOptions = GetPriorityList();
            model.EmployeeIdOptions = getTaskIdList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Task_Manager.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _DAL.addTask(task);
                return RedirectToAction("Index");
            }

            task.DueDate = DateTime.Now;
            task.StatusOptions = GetStatusList();
            task.PriorityOptions = GetPriorityList();
            task.EmployeeIdOptions = getTaskIdList();

            return View(task);
        }

        private List<SelectListItem> GetStatusList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Pending", Text = "Pending" },
                new SelectListItem { Value = "In Progress", Text = "In Progress" },
                new SelectListItem { Value = "Completed", Text = "Completed" }
            };
        }

        private List<SelectListItem> GetPriorityList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1" , Text = "High"},
                new SelectListItem { Value = "2", Text = "Medium" },
                new SelectListItem { Value = "3", Text = "Low" }
            };
        }

        private List<SelectListItem> getTaskIdList()
        {
            return _DAL.GetEmployeeIdList();
        }
        public async Task<ActionResult> Delete(int id)
        {
            var task = await _DAL.GetTaskById(id);

            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _DAL.DeleteTask (id);

                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View("Error");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            Task_Manager.Models.Task task = await _DAL.GetTaskById(id);

            return View(task);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Task_Manager.Models.Task task)
        {
            try
            {
                _DAL.editTask(task);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View("Error");
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            Task_Manager.Models.Task task = await _DAL.GetTaskById(id);
            return View(task);
        }

        [HttpPost]
        public async Task<ActionResult> AddComment(Task_Manager.Models.Task task)
        {
            // Ensure the NewComment is not null (for safety)
            if (string.IsNullOrEmpty(task.NewCommentText))
            {
                Console.WriteLine("comment cannot be empty!");
                //show user the error
            }

            // Create a new comment based on the new comment field in the task 

            Task_Manager.Models.Comment comment = new Task_Manager.Models.Comment();

            comment.taskId = task.Id;
            comment.commentText = task.NewCommentText;
            comment.createdDate = DateTime.Now; 

            // Add the comment to the database
             _DAL.AddComment(comment);

            // Optionally: Add the new comment to the task's comment collection if it's being displayed immediately
            task.comments.Add(comment);

            // Redirect back to the Details view of the task
            return RedirectToAction("Details", new { id = task.Id });
        }

        public async Task<ActionResult> Search(int id)
        {
            List<Task_Manager.Models.Task> tasks = _DAL.getTasksListByEmployeeId(id);
            return View("Index", tasks);
        }
    }
}
