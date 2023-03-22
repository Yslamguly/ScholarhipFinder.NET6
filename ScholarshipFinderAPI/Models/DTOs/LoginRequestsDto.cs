using System.ComponentModel.DataAnnotations;

namespace ScholarhipFinderAPI.Models.DTOs{
    public class LoginRegistrationDto{
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}