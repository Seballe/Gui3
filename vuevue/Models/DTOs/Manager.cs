using System.ComponentModel.DataAnnotations;

namespace ModelsApi.Models.DTOs
{
    public class Manager
    {
        [MaxLength(64)]
        public string FirstName { get; set; }
        [MaxLength(32)]
        public string LastName { get; set; }
        [MaxLength(254)]
        public string Email { get; set; }
        [MaxLength(60)]
        public string Password { get; set; }
    }
}
