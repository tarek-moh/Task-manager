using System.ComponentModel.DataAnnotations;

namespace Task_Manager.Models
{
    public class account
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }

    }
}
