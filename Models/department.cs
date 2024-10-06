using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;

namespace Task_Manager.Models
{
    public class department
    {
        public department() { } 
        public department(int id, string name, int headID)
        {
            Id = id;
            Name = name;
            this.headID = headID;
        }

        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        public int headID { get; set; }
        public IEnumerable<SelectListItem>? EmployeeIdOptions { get; set; } = new List<SelectListItem>();

    }
}
