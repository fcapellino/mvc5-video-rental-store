using System;
using System.ComponentModel.DataAnnotations;

namespace VideoRentalStore.Web.ViewModels
{
    public class CreateEditClientViewModel
    {
        public int Id { get; set; }

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
        public DateTime AddedDate { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
