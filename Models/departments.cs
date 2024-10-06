using Microsoft.AspNetCore.Mvc.Rendering;

namespace Task_Manager.Models
{
    public class departments
    {

        public List<Task_Manager.Models.department> departmentList { get; set; }
        public List<SelectListItem> employees { get; set; }


    }
}
