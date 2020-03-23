using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using VideoRentalStore.DataAccess.Common;
using VideoRentalStore.Domain.Entities;
using VideoRentalStore.Web.CustomResult;
using VideoRentalStore.Web.ViewModels;

namespace VideoRentalStore.Web.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IDataAccess<Movie> _moviesDataAccess;
        private readonly IDataAccess<MovieGenre> _movieGenresDataAccess;
        private readonly IDataAccess<Rental> _rentalsDataAccess;

        public MoviesController(IDataAccess<Movie> moviesDataAccess, IDataAccess<MovieGenre> movieGenresDataAccess, IDataAccess<Rental> rentalsDataAccess)
        {
            _moviesDataAccess = moviesDataAccess;
            _movieGenresDataAccess = movieGenresDataAccess;
            _rentalsDataAccess = rentalsDataAccess;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetMoviesList()
        {
            try
            {
                var moviesList = (await _moviesDataAccess.GetAllAsync())
                    .Select(Mapper.Map<Movie, MovieDataTableViewModel>)
                    .ToList();

                return new JsonCamelCaseResult(new HttpResult(success: true, resources: moviesList), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new HttpResult(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAvailableMoviesList(string query = null)
        {
            try
            {
                var moviesList = (await _moviesDataAccess.GetAllAsync())
                    .Where(p => p.AvailableUnits > 0)
                    .Select(Mapper.Map<Movie, MovieDataTableViewModel>)
                    .ToList();

                if (!string.IsNullOrWhiteSpace(query))
                {
                    moviesList = moviesList.Where(p => p.Name.ToLower().Contains(query.ToLower())).ToList();
                }

                return new JsonCamelCaseResult(new HttpResult(success: true, resources: moviesList), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new HttpResult(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Create()
        {
            try
            {
                var createEditMovieViewModel = new CreateEditMovieViewModel()
                {
                    TotalUnits = 1
                };

                createEditMovieViewModel.Genres = (await _movieGenresDataAccess.GetAllAsync())
                    .OrderBy(x => x.Name);
                return PartialView("_Create", createEditMovieViewModel);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Create(CreateEditMovieViewModel createEditMovieViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    createEditMovieViewModel.Genres = (await _movieGenresDataAccess.GetAllAsync())
                        .OrderBy(x => x.Name);
                    return PartialView("_Create", createEditMovieViewModel);
                }

                var movie = Mapper.Map<CreateEditMovieViewModel, Movie>(createEditMovieViewModel);
                await _moviesDataAccess.AddAsync(movie);
                return new JsonCamelCaseResult(new HttpResult(success: true), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new HttpResult(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var movie = await _moviesDataAccess.GetByIdAsync(id);
                if (movie == null)
                {
                    return HttpNotFound();
                }

                var createEditMovieViewModel = Mapper.Map<Movie, CreateEditMovieViewModel>(movie);
                createEditMovieViewModel.Genres = (await _movieGenresDataAccess.GetAllAsync())
                        .OrderBy(x => x.Name);

                return PartialView("_Edit", createEditMovieViewModel);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(CreateEditMovieViewModel createEditMovieViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    createEditMovieViewModel.Genres = (await _movieGenresDataAccess.GetAllAsync())
                        .OrderBy(x => x.Name);
                    return PartialView("_Edit", createEditMovieViewModel);
                }

                var movie = Mapper.Map<CreateEditMovieViewModel, Movie>(createEditMovieViewModel);
                var rentalsCount = (await _rentalsDataAccess.GetAllAsync())
                    .Count(a => a.Id == createEditMovieViewModel.Id);

                if (movie.TotalUnits >= rentalsCount)
                {
                    movie.AvailableUnits = (short)(movie.TotalUnits - rentalsCount);
                }
                else
                {
                    ModelState.AddModelError("errorTotalUnits", "Must enter a valid quantity.");
                }

                await _moviesDataAccess.UpdateAsync(movie);
                return new JsonCamelCaseResult(new HttpResult(success: true), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new HttpResult(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var movie = await _moviesDataAccess.GetByIdAsync(id);
                await _moviesDataAccess.DeleteAsync(movie);
                return new JsonCamelCaseResult(new HttpResult(success: true), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new HttpResult(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _moviesDataAccess?.Dispose();
            _movieGenresDataAccess?.Dispose();
            _rentalsDataAccess?.Dispose();
            base.Dispose(disposing);
        }
    }
}
