using System.ComponentModel.DataAnnotations;
namespace UdemyApp.Models
{
    public class UserForRegisterDTO
    {
        [Required]
        public string Username {get; set;}
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage ="Password must be between 4 and 8 characters")]
        public string Password {get; set;}

    }
}
