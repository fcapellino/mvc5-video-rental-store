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
    public class ClientsController : Controller
    {
        private readonly IDataAccess<Client> _clientsDataAccess;

        public ClientsController(IDataAccess<Client> clientDataAccess)
        {
            _clientsDataAccess = clientDataAccess;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetClientsList(string query = null)
        {
            try
            {
                var clientsList = (await _clientsDataAccess.GetAllAsync())
                    .Select(Mapper.Map<Client, ClientDataTableViewModel>)
                    .ToList();

                if (!string.IsNullOrWhiteSpace(query))
                {
                    clientsList = clientsList.Where(c => c.FullName.ToLower().Contains(query.Trim().ToLower())).ToList();
                }

                return new JsonCamelCaseResult(new JsonData(success: true, resources: clientsList), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new JsonData(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            try
            {
                var createEditClientViewModel = Mapper.Map<Client, CreateEditClientViewModel>(new Client());
                return PartialView("_Create", createEditClientViewModel);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateEditClientViewModel createEditClientViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("_Create", createEditClientViewModel);
                }

                var client = Mapper.Map<CreateEditClientViewModel, Client>(createEditClientViewModel);
                await _clientsDataAccess.AddAsync(client);
                return new JsonCamelCaseResult(new JsonData(success: true), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new JsonData(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var client = await _clientsDataAccess.GetByIdAsync(id);
                if (client == null)
                {
                    return HttpNotFound();
                }

                var createEditClientViewModel = Mapper.Map<Client, CreateEditClientViewModel>(client);
                return PartialView("_Edit", createEditClientViewModel);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CreateEditClientViewModel createEditClientViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("_Edit", createEditClientViewModel);
                }

                var client = Mapper.Map<CreateEditClientViewModel, Client>(createEditClientViewModel);
                await _clientsDataAccess.UpdateAsync(client);
                return new JsonCamelCaseResult(new JsonData(success: true), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new JsonData(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var client = await _clientsDataAccess.GetByIdAsync(id);
                await _clientsDataAccess.DeleteAsync(client);
                return new JsonCamelCaseResult(new JsonData(success: true), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new JsonData(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _clientsDataAccess?.Dispose();
            base.Dispose(disposing);
        }
    }
}
