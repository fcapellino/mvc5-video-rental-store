using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoRentalStore.Domain.Entities
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public int GenreId { get; set; }
        public MovieGenre Genre { get; set; }

        [Required]
        [StringLength(900)]
        public string Description { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Range(1, 20)]
        public short TotalUnits { get; set; }

        [Required]
        [Range(0, 20)]
        public short AvailableUnits { get; set; }

        [Required]
        public string Base64PosterImage { get; set; }
    }
}
