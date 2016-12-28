﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Interfaces;
using Microsoft.Ajax.Utilities;
using Microsoft.Graph;
using Resources;

namespace AMPSchedules.Controllers
{
    public class AddEventController : TemplateController
    {
        // GET: AddEvent
        [HttpGet]
        public async Task<ActionResult> AddEvent()
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

        public override ActionResult hook(TimeTableManager manager)
        {
            //Get the room
            ICollection<Room> rooms = new List<Room>();
            foreach (var building in manager.Repository.Buildings)
            {
                try
                {
                    var room =
                        ((List<Room>) (building.Rooms)).Find(r => r.Id == Int32.Parse(Request.QueryString["room"]));
                    rooms.Add(room);
                }
                catch (ArgumentNullException e)
                {
                    Debug.Write(e);
                }
            }
            
            //Get The course
            ICollection<Course> courses = new List<Course>();
            foreach (var course in manager.Repository.Courses)
            {
                if (course.Name == Request.QueryString["course"])
                {
                    courses.Add(course);
                }
            }

            //Get the course
            ITimeTableItem newEvent = new EvaluationMoment(
                Convert.ToDateTime(Request.QueryString["beginsAt"]),
                Convert.ToDateTime(Request.QueryString["endsAt"]),
                rooms,
                courses,
                Request.QueryString["title"], Request.QueryString["description"]);
            
            //Add new Event
            manager.TimeTable.AddTimetableItem(newEvent);
            return base.hook(manager);
        }
    }
}