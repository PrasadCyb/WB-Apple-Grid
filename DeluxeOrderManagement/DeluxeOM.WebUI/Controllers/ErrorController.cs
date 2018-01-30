using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeluxeOM.WebUI.Models;

namespace DeluxeOM.WebUI.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: /Error/AccessDenied
        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult SessionTimeout()
        {
            return View();
        }
    }
}