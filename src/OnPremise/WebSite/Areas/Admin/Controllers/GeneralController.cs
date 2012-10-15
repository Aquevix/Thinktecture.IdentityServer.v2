﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Models.Configuration;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.Configuration)]
    public class GeneralController : Controller
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public GeneralController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public GeneralController(IConfigurationRepository configuration)
        {
            ConfigurationRepository = configuration;
        }

        public ActionResult Index()
        {
            var model = ConfigurationRepository.Global;
            return View("Index", model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(GlobalConfiguration model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ConfigurationRepository.Global = model;
                    TempData["Message"] = "Update Successful";
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error updating configuration.");
                }
            }

            return View("Index", model);
        }
    }
}