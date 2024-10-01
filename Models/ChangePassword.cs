using System.ComponentModel.DataAnnotations;

namespace Task_Manager.Models
{
    public class ChangePassword
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [MinLength(6)]
        public string password { get; set; }
        [Required]
        [MinLength(6)]
        [Display(Name = "New password")]
        public string newPassword { get; set; }
        [Required]
        [MinLength(6)]
        [Display(Name = "Confirm password")]
        public string confirmPassword { get; set; }
    }
}
