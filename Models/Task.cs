using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Task_Manager.Models
{
	public class Task
	{
		public Task() {  }
		public Task(int id, string name, string? description, DateTime dueDate, string status, int priority, int employeeId)
		{
			Id = id;
			Name = name;
			Description = description;
			DueDate = dueDate;
			Status = status;
			Priority = priority;
			EmployeeId = employeeId;
		}
        public Task( string name, string? description, DateTime dueDate, string status, int priority, int employeeId)
        {
            Name = name;
            Description = description;
            DueDate = dueDate;
            Status = status;
            Priority = priority;
            EmployeeId = employeeId;

        }

        public int Id { get; set; }
		[Required(ErrorMessage = "Name is required!")]
		[MaxLength(30)]
		public string Name { get; set; }
		public string? Description { get; set; }

		[Required(ErrorMessage = "Description is required!")]
		[Display(Name = "Due Date")]
		[DataType(DataType.Date)]
		public DateTime DueDate { get; set; }

		[Required(ErrorMessage ="Select Status!")]
		public string? Status { get; set; }
        [Required(ErrorMessage = "Select priority!")]

        public int? Priority { get; set; }

		[Required(ErrorMessage ="Select employee ID!")]
		[Display(Name="Employee ID")]
		public int EmployeeId	{ get; set; }
		public List<Task_Manager.Models.Comment>? comments { get; set; }
        public string? NewCommentText { get; set; }

		public IEnumerable<SelectListItem>? StatusOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem>? PriorityOptions { get; set; } = new List<SelectListItem>();
		public IEnumerable<SelectListItem>? EmployeeIdOptions { get; set; } = new List<SelectListItem>();
    }
}
