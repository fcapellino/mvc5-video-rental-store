namespace VideoRentalStore.Web.ViewModels
{
    public class MovieDataTableViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string ReleaseDate { get; set; }
        public short TotalUnits { get; set; }
        public short AvailableUnits { get; set; }
        public string Base64Poster { get; set; }
    }
}
