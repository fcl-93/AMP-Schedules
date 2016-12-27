﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Resources;
using AMPSchedules.Helpers;
using AMPSchedules.Models;
using AMPSystem.Classes;

using AMPSystem.Interfaces;
using Microsoft.Graph;
using Newtonsoft.Json;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class ColorController: TemplateController
    {
        public override void hook(TimeTableManager manager)
        {
            //Read The Color that was sent
            string color = null;
            string itemName = null;
            foreach (var eventName in Request.QueryString)
            {
                itemName = (string)eventName;
                color = Request.QueryString[itemName];
            }

            //Change the color on the items 
            foreach (var item in manager.TimeTable.ItemList)
            {
                if (item.Name == itemName)
                {
                    if (color != null)
                    {

                        item.Color = color;
                        //Debug.Write(item.Color);
                    }
                    else
                    {
                        Debug.Write("No Color was Defined");
                        break;
                    }
                }
            }
        }
        [HttpGet]
        public async Task<ActionResult> EventColor()
        {
            try
            {
                return await TemplateMethod();
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
           
        }
    }
}