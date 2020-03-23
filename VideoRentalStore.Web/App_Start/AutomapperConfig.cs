using System;
using System.Globalization;
using AutoMapper;
using VideoRentalStore.Domain.Entities;
using VideoRentalStore.Web.ViewModels;

namespace VideoRentalStore.Web
{
    public static class AutomapperConfig
    {
        public static void SetMappingProfiles()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ApplicationUser, UserDataTableViewModel>()
                    .ForMember
                        (dst => dst.Id, src => src.MapFrom(s => s.Id))
                    .ForMember
                        (dst => dst.FullName, src => src.MapFrom(s => s.FullName))
                    .ForMember
                        (dst => dst.Telephone, src => src.MapFrom(s => s.PhoneNumber))
                    .ForMember
                        (dst => dst.Address, src => src.MapFrom(s => s.Address))
                    .ForMember
                        (dst => dst.Email, src => src.MapFrom(s => s.Email))
                    .ForMember
                        (dst => dst.AddedDate, src => src.MapFrom(s => s.AddedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                    .ForMember
                        (dst => dst.Type, opt => opt.Ignore());

                cfg.CreateMap<CreateUserViewModel, ApplicationUser>()
                    .ForMember
                        (dst => dst.FullName, src => src.MapFrom(s => s.FullName))
                    .ForMember
                        (dst => dst.PhoneNumber, src => src.MapFrom(s => s.Telephone))
                    .ForMember
                        (dst => dst.Address, src => src.MapFrom(s => s.Address))
                    .ForMember
                        (dst => dst.UserName, src => src.MapFrom(s => s.Email))
                    .ForMember
                        (dst => dst.Email, src => src.MapFrom(s => s.Email))
                    .ForAllOtherMembers(opt => opt.Ignore());

                cfg.CreateMap<Movie, MovieDataTableViewModel>()
                    .ForMember
                        (dst => dst.Id, src => src.MapFrom(s => s.Id))
                    .ForMember
                        (dst => dst.Name, src => src.MapFrom(s => s.Name))
                    .ForMember
                        (dst => dst.Genre, src => src.MapFrom(s => s.Genre.Name))
                    .ForMember
                        (dst => dst.Description, src => src.MapFrom(s => s.Description))
                    .ForMember
                        (dst => dst.ReleaseDate, src => src.MapFrom(s => s.ReleaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                    .ForMember
                        (dst => dst.TotalUnits, src => src.MapFrom(s => s.TotalUnits))
                    .ForMember
                        (dst => dst.AvailableUnits, src => src.MapFrom(s => s.AvailableUnits))
                    .ForMember
                        (dst => dst.Base64Poster, src => src.MapFrom(s => s.Base64PosterImage))
                    .ForAllOtherMembers(opt => opt.Ignore());

                cfg.CreateMap<Movie, CreateEditMovieViewModel>()
                    .ForMember
                        (dst => dst.Id, src => src.MapFrom(s => s.Id))
                    .ForMember
                        (dst => dst.Name, src => src.MapFrom(s => s.Name))
                    .ForMember
                        (dst => dst.GenreId, src => src.MapFrom(s => s.GenreId))
                    .ForMember
                        (dst => dst.Description, src => src.MapFrom(s => s.Description))
                    .ForMember
                        (dst => dst.ReleaseDate, src => src.MapFrom(s => s.ReleaseDate))
                    .ForMember
                        (dst => dst.TotalUnits, src => src.MapFrom(s => s.TotalUnits))
                    .ForMember
                        (dst => dst.Base64Poster, src => src.MapFrom(s => s.Base64PosterImage))
                    .ForAllOtherMembers(opt => opt.Ignore());

                cfg.CreateMap<CreateEditMovieViewModel, Movie>()
                    .ForMember
                        (dst => dst.Id, src => src.MapFrom(s => s.Id))
                    .ForMember
                        (dst => dst.Name, src => src.MapFrom(s => s.Name))
                    .ForMember
                        (dst => dst.GenreId, src => src.MapFrom(s => s.GenreId))
                    .ForMember
                        (dst => dst.Description, src => src.MapFrom(s => s.Description))
                    .ForMember
                        (dst => dst.ReleaseDate, src => src.MapFrom(s => s.ReleaseDate))
                    .ForMember
                        (dst => dst.TotalUnits, src => src.MapFrom(s => s.TotalUnits))
                    .ForMember
                        (dst => dst.AvailableUnits, src => src.MapFrom(s => s.TotalUnits))
                    .ForMember
                        (dst => dst.Base64PosterImage, src => src.MapFrom(s => s.Base64Poster))
                    .ForAllOtherMembers(opt => opt.Ignore());

                cfg.CreateMap<Client, ClientDataTableViewModel>()
                    .ForMember
                        (dst => dst.Id, src => src.MapFrom(s => s.Id))
                    .ForMember
                        (dst => dst.FullName, src => src.MapFrom(s => s.FullName))
                    .ForMember
                        (dst => dst.Telephone, src => src.MapFrom(s => s.Telephone))
                    .ForMember
                        (dst => dst.Adress, src => src.MapFrom(s => s.Address))
                    .ForMember
                        (dst => dst.Email, src => src.MapFrom(s => s.Email))
                    .ForMember
                        (dst => dst.AddedDate, src => src.MapFrom(s => s.AddedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                    .ForAllOtherMembers(opt => opt.Ignore());

                cfg.CreateMap<Client, CreateEditClientViewModel>()
                    .ForMember
                        (dst => dst.Id, src => src.MapFrom(s => s.Id))
                    .ForMember
                        (dst => dst.FullName, src => src.MapFrom(s => s.FullName))
                    .ForMember
                        (dst => dst.Telephone, src => src.MapFrom(s => s.Telephone))
                    .ForMember
                        (dst => dst.Address, src => src.MapFrom(s => s.Address))
                    .ForMember
                        (dst => dst.AddedDate, src => src.MapFrom(s => s.AddedDate))
                    .ForMember
                        (dst => dst.Email, src => src.MapFrom(s => s.Email))
                    .ForAllOtherMembers(opt => opt.Ignore());

                cfg.CreateMap<CreateEditClientViewModel, Client>()
                    .ForMember
                        (dst => dst.Id, src => src.MapFrom(s => s.Id))
                    .ForMember
                        (dst => dst.FullName, src => src.MapFrom(s => s.FullName))
                    .ForMember
                        (dst => dst.Telephone, src => src.MapFrom(s => s.Telephone))
                    .ForMember
                        (dst => dst.Address, src => src.MapFrom(s => s.Address))
                    .ForMember
                        (dst => dst.AddedDate, src => src.MapFrom(s => s.AddedDate))
                    .ForMember
                        (dst => dst.Email, src => src.MapFrom(s => s.Email))
                    .ForAllOtherMembers(opt => opt.Ignore());

                cfg.CreateMap<Rental, RentalDataTableViewModel>()
                    .ForMember
                        (dst => dst.Id, src => src.MapFrom(s => s.Id))
                    .ForMember
                        (dst => dst.ClientId, src => src.MapFrom(s => s.ClientId))
                    .ForMember
                        (dst => dst.ClientName, src => src.MapFrom(s => s.Client.FullName))
                    .ForMember
                        (dst => dst.MovieName, src => src.MapFrom(s => s.Movie.Name))
                    .ForMember
                        (dst => dst.Date, src => src.MapFrom(s => s.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                    .ForMember
                        (dst => dst.DaysLate, src => src.MapFrom(s => (int)(DateTime.Now - s.Date).TotalDays))
                    .ForAllOtherMembers(opt => opt.Ignore());
            });
        }
    }
}
