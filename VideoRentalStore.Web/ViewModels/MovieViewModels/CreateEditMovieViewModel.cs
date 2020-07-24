using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VideoRentalStore.Domain.Entities;

namespace VideoRentalStore.Web.ViewModels
{
    public class CreateEditMovieViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        [StringLength(900)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        [Range(1, 20)]
        public short TotalUnits { get; set; }

        [Required(ErrorMessage = "You must attach an image.")]
        public string Base64Poster { get; set; }

        public IEnumerable<MovieGenre> Genres { get; set; }
    }
}
