using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using VideoRentalStore.DataAccess;
using VideoRentalStore.Domain.Entities;
using VideoRentalStore.Web.CustomResult;
using VideoRentalStore.Web.ViewModels;

namespace VideoRentalStore.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ApplicationUserManager _userManager;
        private readonly RoleStore<IdentityRole> _roleStore;

        public UsersController(ApplicationDbContext context)
        {
            _applicationDbContext = context;
            _userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            _roleStore = new RoleStore<IdentityRole>(_applicationDbContext);
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetUsersList()
        {
            try
            {
                var usersList = (await _applicationDbContext.Users.OrderBy(u => u.FullName).ToListAsync())
                    .Select(Mapper.Map<ApplicationUser, UserDataTableViewModel>)
                    .Select(u => { u.Type = _userManager.GetRoles(u.Id).FirstOrDefault(); return u; })
                    .ToList();

                return new JsonCamelCaseResult(new JsonData(success: true, resources: usersList), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new JsonData(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Create()
        {
            try
            {
                var createUserViewModel = new CreateUserViewModel
                {
                    Roles = await _applicationDbContext.Roles.Select(r => r.Name).OrderBy(Name => Name).ToListAsync()
                };
                return PartialView("_Create", createUserViewModel);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Create(CreateUserViewModel createUserViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    createUserViewModel.Roles = await _applicationDbContext.Roles.Select(r => r.Name).OrderBy(Name => Name).ToListAsync();
                    return PartialView("_Create", createUserViewModel);
                }

                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var section = ConfigurationManager.GetSection("newUserConfiguration") as NameValueCollection;
                    var user = Mapper.Map<CreateUserViewModel, ApplicationUser>(createUserViewModel);
                    var createResult = await _userManager.CreateAsync(user, section["DefaultPassword"]);
                    var addToRoleResult = await _userManager.AddToRoleAsync(user.Id, createUserViewModel.Type);

                    if (createResult.Succeeded && addToRoleResult.Succeeded)
                    {
                        transactionScope.Complete();
                        return new JsonCamelCaseResult(new JsonData(success: true), JsonRequestBehavior.AllowGet);
                    }
                }

                throw new InvalidOperationException();
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new JsonData(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                var applicationUser = await _userManager.FindByIdAsync(id);
                if (applicationUser == null)
                {
                    return HttpNotFound();
                }

                var editUserViewModel = new EditUserViewModel
                {
                    Id = applicationUser.Id,
                    Type = (await _userManager.GetRolesAsync(applicationUser.Id)).FirstOrDefault(),
                    Roles = await _applicationDbContext.Roles.Select(r => r.Name).OrderBy(Name => Name).ToListAsync()
                };
                return PartialView("_Edit", editUserViewModel);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(EditUserViewModel editUserViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    editUserViewModel.Roles = await _applicationDbContext.Roles.Select(r => r.Name).OrderBy(Name => Name).ToListAsync();
                    return PartialView("_Edit", editUserViewModel);
                }

                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var applicationUser = _userManager.FindById(editUserViewModel.Id);
                    var oldRole = (await _userManager.GetRolesAsync(applicationUser.Id)).FirstOrDefault();
                    var removeFromRoleResult = await _userManager.RemoveFromRoleAsync(applicationUser.Id, oldRole);
                    var addToRoleResult = await _userManager.AddToRoleAsync(applicationUser.Id, editUserViewModel.Type);

                    if (removeFromRoleResult.Succeeded && addToRoleResult.Succeeded)
                    {
                        transactionScope.Complete();
                        return new JsonCamelCaseResult(new JsonData(success: true), JsonRequestBehavior.AllowGet);
                    }
                }

                throw new InvalidOperationException();
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new JsonData(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var applicationUser = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(applicationUser);
                return new JsonCamelCaseResult(new JsonData(success: true), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new JsonCamelCaseResult(new JsonData(success: false), JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_applicationDbContext != null)
                {
                    _applicationDbContext.Dispose();
                }

                if (_userManager != null)
                {
                    _userManager.Dispose();
                }

                if (_roleStore != null)
                {
                    _roleStore.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
