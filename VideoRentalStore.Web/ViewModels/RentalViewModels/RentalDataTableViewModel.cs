namespace VideoRentalStore.Web.ViewModels
{
    public class RentalDataTableViewModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string MovieName { get; set; }
        public string Date { get; set; }
        public int DaysLate { get; set; }
    }
}
