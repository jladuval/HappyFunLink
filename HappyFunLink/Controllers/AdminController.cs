using System.Web.Mvc;

namespace HappyFunLink.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Security;

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

        [Authorize]
        public ActionResult Index()
        {
            return View(CreateAdminModel());
        }

        [HttpPost]
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

        [Authorize]
        [HttpPost]
        public ActionResult AddNoun(AdminModel model)
        {
            if(!String.IsNullOrEmpty(model.NewNoun))
            {
                _adminService.InsertNoun(new Noun {Word = model.NewNoun});
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddAdjective(AdminModel model)
        {
            if (!String.IsNullOrEmpty(model.NewAdjective))
            {
                _adminService.InsertAdjective(new Adjective { Word = model.NewAdjective });
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public void DeleteNoun(int nounId)
        {
            _adminService.DeleteNoun(nounId);
        }
        
        [Authorize]
        [HttpPost]
        public void DeleteAdjective(int adjId)
        {
            _adminService.DeleteAdjective(adjId);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteAdjective(AdminModel model)
        {
            if (!String.IsNullOrEmpty(model.NewNoun))
            {
                _adminService.InsertNoun(new Noun() { Word = model.NewNoun });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
           if(ModelState.IsValid)
           {
               if(_accounts.ValidateUser(model.Email, model.Password))
               {
                   _accounts.LoginUser(model.Email);
                   FormsAuthentication.SetAuthCookie(model.Email, true);
                   return RedirectToAction("Index");
               }
               ModelState.AddModelError("", "Invalid Username/Password");
           }
           return View("Login");
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
