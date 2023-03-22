using System.ComponentModel.DataAnnotations;

namespace ScholarhipFinderAPI.Models.DTOs{
    public class UserResgistrationRequestDto{
        public string UserName { get; set; }
        [Required]
        public string  Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}