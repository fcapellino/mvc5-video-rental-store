using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using VideoRentalStore.DataAccess.Common;
using VideoRentalStore.Domain.Entities;
using VideoRentalStore.Web.CustomResult;
using VideoRentalStore.Web.ViewModels;

namespace VideoRentalStore.Web.Controllers
{
    [Authorize]
    public class RentalsController : Controller
    {
        private readonly IDataAccess<Client> _clientsDataAccess;
        private readonly IDataAccess<Movie> _moviesDataAccess;
        private readonly IDataAccess<Rental> _rentalsDataAccess;
        private readonly IIdentityMessageService _emailService;

        public RentalsController(IDataAccess<Client> clientsDataAccess, IDataAccess<Movie> moviesDataAccess, IDataAccess<Rental> rentalsDataAccess, IIdentityMessageService emailService)
        {
            _clientsDataAccess = clientsDataAccess;
            _moviesDataAccess = moviesDataAccess;
            _rentalsDataAccess = rentalsDataAccess;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetRentalsList()
        {
            try
            {
                var rentalsList = (await _rentalsDataAccess.GetAllAsync())
                    .Select(Mapper.Map<Rental, RentalDataTableViewModel>)
                    .ToList();

                return new JsonCamelCaseResult(new HttpResult(success: true, resources: rentalsList), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new HttpResult(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            try
            {
                return PartialView("_Create", new CreateRentalViewModel());
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateRentalViewModel createRentalViewModel)
        {
            try
            {
                var client = await _clientsDataAccess.GetByIdAsync(createRentalViewModel.ClientId);
                var moviesList = (await _moviesDataAccess.GetAllAsync())
                    .Where(p => createRentalViewModel.MoviesIds.Contains(p.Id)).ToList();

                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var currentMovie in moviesList)
                    {
                        currentMovie.AvailableUnits--;
                        var newRental = new Rental()
                        {
                            ClientId = client.Id,
                            MovieId = currentMovie.Id,
                            Date = DateTime.Now
                        };

                        await _moviesDataAccess.UpdateAsync(currentMovie);
                        await _rentalsDataAccess.AddAsync(newRental);
                    }

                    transactionScope.Complete();
                    return new JsonCamelCaseResult(new HttpResult(success: true), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new HttpResult(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SendEmail(int id)
        {
            try
            {
                var rentals = (await _rentalsDataAccess.GetAllAsync())
                    .Where(x => x.ClientId == id);
                string moviesList = string.Join(", ", rentals.Select(a => a.Movie.Name));

                await _emailService.SendAsync(new IdentityMessage()
                {
                    Destination = rentals.FirstOrDefault().Client.Email,
                    Subject = "Remainder",
                    Body = string.Format("<h3><i>You must return the following movies: {0}.</i></h3>", moviesList)
                });

                return new JsonCamelCaseResult(new HttpResult(success: true), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new HttpResult(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var rental = await _rentalsDataAccess.GetByIdAsync(id);
                var movie = await _moviesDataAccess.GetByIdAsync(rental.MovieId);

                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    movie.AvailableUnits++;
                    await _moviesDataAccess.UpdateAsync(movie);
                    await _rentalsDataAccess.DeleteAsync(rental);

                    transactionScope.Complete();
                    return new JsonCamelCaseResult(new HttpResult(success: true), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new HttpResult(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _clientsDataAccess?.Dispose();
            _moviesDataAccess?.Dispose();
            _rentalsDataAccess?.Dispose();
            base.Dispose(disposing);
        }
    }
}
