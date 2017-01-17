﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AMPSystem;
using Newtonsoft.Json;

namespace AMPSchedules.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Alerts
        public ActionResult Index()
        {
            var mail = ClaimsPrincipal.Current.FindFirst("preferred_username")?.Value;
            var startDateTime = Convert.ToDateTime(Request.QueryString["start"]);
            var endDateTime = Convert.ToDateTime(Request.QueryString["end"]);
            var facade = new AMPSystemFacade(mail,startDateTime,endDateTime);

            return Content(JsonConvert.SerializeObject(facade.GetCourses(),
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    "application/json");
        }
    }
}