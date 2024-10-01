using System.ComponentModel.DataAnnotations;

namespace Task_Manager.Models;

public class Employee
{
	public int? Id { get; set; }

    [Required(ErrorMessage = "First name is required!")]
    [Display(Name = "first name")]
    public string FirstName {get; set; }

    [Required(ErrorMessage = "Last name is required!")]
    [Display (Name = "Last name")]
    public string LastName { get; set; }
    [Range(18, 65)]
    public int ? Age { get; set; }
	public string? phone { get; set; }
    [Display(Name = "Super visor")]

    public int? SuperVisor { get; set; }
    [Required(ErrorMessage = "specify Permissions")]
    [Display(Name = "Admin")]
    public bool IsAdmin { get; set; }
    [Required (ErrorMessage = "Email is required!")]
    public string Email { get; set; }
    [Required (ErrorMessage = "Password is required!")]
    [MinLength(6)]
    [Display (Name = "Password")]
    public string PassWord {  get; set; }
    [Required(ErrorMessage = "Department ID is required!")]
    [Display(Name = "Department ID")]
    public int DepartmentId {  get; set; }
}