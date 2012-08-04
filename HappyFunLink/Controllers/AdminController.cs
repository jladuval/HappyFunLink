using System.Web.Mvc;

namespace HappyFunLink.Controllers
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using Domain.Services.Interfaces;

    using Entities;

    using HappyFunLink.Models.Admin;

    using WebCore.Security.Interfaces;

    public class AdminController : Controller
    {
        private readonly MembershipProviderBase _accounts;

        private readonly IAdminService _adminService;

        public AdminController(
            MembershipProviderBase accounts,
            IAdminService adminService)
        {
            _accounts = accounts;
            _adminService = adminService;
        }

        public ActionResult Index()
        {
            return View(CreateAdminModel());
        }

        public ActionResult AddAdministrator(AdminModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _accounts.CreateUser(model.NewUser.FirstName, model.NewUser.LastName, model.NewUser.Email, model.NewUser.Password, false);
                    return RedirectToAction("Index");
                }
                catch(ArgumentException e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            return View("Index", model);
        }

        private AdminModel CreateAdminModel()
        {
            return new AdminModel
                {
                    Adjectives = Mapper.Map<List<Adjective>, List<AdjectiveModel>>(_adminService.GetAllAdjectives()),
                    Nouns = Mapper.Map<List<Noun>, List<NounModel>>(_adminService.GetAllNouns())
                };
        }
    }
}
