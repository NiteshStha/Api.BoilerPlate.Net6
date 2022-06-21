using System.ComponentModel.DataAnnotations;

namespace Api.BoilerPlate.Net6.Models.Users
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}