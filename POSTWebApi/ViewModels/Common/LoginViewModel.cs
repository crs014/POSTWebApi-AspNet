using System.ComponentModel.DataAnnotations;

namespace POSTWebApi.ViewModels.Common
{
    public class LoginViewModel
    {
        [EmailAddress]
        [MaxLength(255)]
        [Required]
        public string Email { get; set; }


        [MaxLength(45)]
        [MinLength(6)]
        [Required]
        public string Password { get; set; }

        [Required]
        public string DeviceNumber { get; set; }
    }
}