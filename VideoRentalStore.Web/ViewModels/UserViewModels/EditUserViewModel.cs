using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoRentalStore.Web.ViewModels
{
    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Type { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
