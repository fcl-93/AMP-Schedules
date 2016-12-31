﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using Microsoft.Graph;
using Newtonsoft.Json.Linq;
using Resources;
using Room = AMPSystem.Models.Room;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class AddAlertController : TemplateController
    {
        // GET: AddAlert
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                return await TemplateMethod();
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction($"Index", $"Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }

        public override ActionResult Hook(TimeTableManager manager)
        {
            var keys = Request.QueryString.AllKeys;
            for (var i = 0 ; i < keys.Length - 2; i = i + 5)
            {
                var name = Request.QueryString[i];
                var startTime = Convert.ToDateTime(Request.QueryString[i + 1]);
                var endTime = Convert.ToDateTime(Request.QueryString[i + 2]);

                var time = int.Parse(Request.QueryString[i + 3]);
                var units = Request.QueryString[i + 4];
                
                Debug.WriteLine(name + " " + startTime + " " + endTime + " " + time + " " + units);

                var item = ((List<ITimeTableItem>) manager.TimeTable.ItemList).Find(
                    it =>
                        it.Name == name &&
                        it.StartTime == startTime);

                TimeSpan timeSpan;
                switch (units)
                {
                    case "Minutes":
                        timeSpan = new TimeSpan(0, time, 0);
                        break;
                    case "Hours":
                        timeSpan = new TimeSpan(time, 0, 0);
                        break;
                    case "Days":
                        timeSpan = new TimeSpan(time, 0, 0, 0);
                        break;
                    case "Weeks":
                        timeSpan = new TimeSpan(time * 7, 0, 0, 0);
                        break;
                    default:
                        timeSpan = new TimeSpan(0, 0, 0);
                        break;
                }
                var alertTime = item.StartTime - timeSpan;
                var alert = new Alert(alertTime, item);
                //TODO Add into BD
                if (item is Lesson)
                {
                    var mUser = DbManager.Instance.CreateUserIfNotExists(CurrentUser.Email);
                    var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(item.Rooms.First().Building.Name);
                    var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding,item.Rooms.First().Floor, item.Rooms.First().Name);
                    var mLesson = DbManager.Instance.CreateLessonIfNotExists(item.Name, mRoom, mUser, item.Color,
                        item.StartTime, item.EndTime);
                    DbManager.Instance.AddAlertToLesson(alertTime, mLesson);
                    DbManager.Instance.SaveChanges();
                }
                else if (item is EvaluationMoment)
                {
                    var mUser = DbManager.Instance.CreateUserIfNotExists(CurrentUser.Email);
                    var mRooms = new List<Room>();
                    foreach (var room in item.Rooms)
                    {
                        var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                        var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name);
                        mRooms.Add(mRoom);
                    }
                    var mEvMoment = DbManager.Instance.CreateEvaluationMomentIfNotExists(item.Name, mRooms, mUser, item.Color,
                        item.StartTime, item.EndTime, item.Description);
                    DbManager.Instance.AddAlertToEvaluation(alertTime, mEvMoment);
                    DbManager.Instance.SaveChanges();
                }
                else if (item is OfficeHours)
                {
                    var mUser = DbManager.Instance.CreateUserIfNotExists(CurrentUser.Email);
                    var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(item.Rooms.First().Building.Name);
                    var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, item.Rooms.First().Floor, item.Rooms.First().Name);
                    var mOfficeHour = DbManager.Instance.CreateOfficeHourIfNotExists(item.Name, mRoom, mUser, item.Color,
                        item.StartTime, item.EndTime);
                    DbManager.Instance.AddAlertToOfficeH(alertTime, mOfficeHour);
                    DbManager.Instance.SaveChanges();
                }
                item.Alerts.Add(alert);
            }
            return base.Hook(manager);
        }
    }
}