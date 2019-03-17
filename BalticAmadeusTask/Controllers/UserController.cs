using BalticAmadeusTask.Helpers;
using BalticAmadeusTask.Models;
using BalticAmadeusTask.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BalticAmadeusTask.Controllers
{
    public class UserController : Controller
    {
        private readonly IRegisteredUserCRUDApiService _registeredUserCrudApiService;

        public UserController(IRegisteredUserCRUDApiService registeredUserCrudApiService, IOptions<CRUDApiConfig> config)
        {
            _registeredUserCrudApiService = registeredUserCrudApiService;
            _registeredUserCrudApiService.SetAPIBaseURL(config.Value.apiBaseUrl);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        // GET: RegisteredUser
        public async Task<ActionResult> Index(string searchName, string searchEmail, int? maxElements)
        {
            var response = await _registeredUserCrudApiService.GetAll(searchName, searchEmail, maxElements);
            return View(await response.Content.ReadAsAsync<List<RegisteredUser>>());
        }

        // GET: RegisteredUser/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            var response = await _registeredUserCrudApiService.Get(id);
            return View(await response.Content.ReadAsAsync<RegisteredUser>());
        }

        // GET: Movies/Edit/5
        public IActionResult Create()
        {
            return View(new RegisteredUser());
        }

        // POST: RegisteredUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisteredUser registeredUser)
        {
            if (ModelState.IsValid)
            {
                var response = await _registeredUserCrudApiService.Create(registeredUser);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Details), await HTTPHelper.Convert<RegisteredUser>(response));
                }
                else
                {
                    var error = await HTTPHelper.Convert<DataValidationError>(response);
                    ModelStateToValidationError.AddModelStateError(ModelState, error);
                    return View(registeredUser);
                }
            }
            return View(registeredUser);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _registeredUserCrudApiService.Get(id);
            var registeredUser = await HTTPHelper.Convert<RegisteredUser>(response);

            if (registeredUser == null)
            {
                return NotFound();
            }
            return View(registeredUser);
        }

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, RegisteredUser registeredUser)
        {
            var invalidEmail = false; //await _userRepository.IsEmailAlreadyExist(registeredUser.Email, registeredUser.Id);

            if (invalidEmail)
            {
                ModelState.AddModelError("Email", "Email already exists");
            }
            //
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RegisteredUserWithoutPassword, RegisteredUser>();
            });
            IMapper iMapper = config.CreateMapper();
            var registeredUser = iMapper.Map<RegisteredUserWithoutPassword, RegisteredUser>(registeredUserWithoutPassword);
            

            if (ModelState.IsValid)
            {
                await _registeredUserCrudApiService.Edit(id, registeredUser);
                return RedirectToAction(nameof(Details), registeredUser);
            }
            return View(registeredUser);
        }

        // POST: RegisteredUser/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        */
    }
}