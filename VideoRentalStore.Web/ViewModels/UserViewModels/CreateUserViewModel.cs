using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoRentalStore.Web.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required]
        [Phone]
        public string Telephone { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Type { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
