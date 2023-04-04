using System.Collections.Generic;

namespace VideoRentalStore.Web.ViewModels
{
    public class CreateRentalViewModel
    {
        public int ClientId { get; set; }
        public List<int> MoviesIds { get; set; }
    }
}
