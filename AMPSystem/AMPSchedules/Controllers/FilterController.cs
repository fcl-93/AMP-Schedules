﻿using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Classes.Filters;
using AMPSystem.Interfaces;
using Resources;

namespace AMPSchedules.Controllers
{
    public class FilterController : TemplateController
    {
        public override ActionResult Hook()
        {
            var filters = new AndCompositeFilter();
            foreach (var filter in Request.QueryString)
                if (Request.QueryString[(string) filter] == "ClassName")
                {
                    IFilter nameFilter = new Name((string) filter);
                    filters.Add(nameFilter);
                }
                else if (Request.QueryString[(string) filter] == "Type")
                {
                    IFilter typeFilter = new TypeF((string) filter);
                    filters.Add(typeFilter);
                }
            filters.ApplyFilter();
            return base.Hook();
        }

        //Handles every request that was made by a user to filter activities.
        [HttpGet]
        public async Task<ActionResult> AddFilter()
        {
            try
            {
                return await TemplateMethod();
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error",
                    new {message = Resource.Error_Message + Request.RawUrl + ": " + e.Message});
            }
        }
    }
}