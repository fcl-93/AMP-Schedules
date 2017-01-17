﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.Interfaces;
using Newtonsoft.Json;

namespace AMPSchedules.Controllers
{
    public class OldContactsController : OldTemplateController
    {
        // GET: ContactsController
        public ActionResult Index()
        {
            return View($"Contacts");
        }

        public async Task<ActionResult> ReturnOfficeHours()
        {
            return await TemplateMethod();
        }


        public override ActionResult Hook()
        {
            ICollection<ITimeTableItem> items = new List<ITimeTableItem>();
            foreach (var officeHour in TimeTableManager.Instance.TimeTable.ItemList)
            {
                Debug.Write(officeHour);
                if (officeHour is OfficeHours)
                {
                //officeHour.Teacher
                //officeHour.Name;
                //officeHour.Rooms;
                items.Add(officeHour);
                }
            }
            return
                Content(
                    JsonConvert.SerializeObject(items.ToArray(),
                        new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                    "application/json");
        }

        //    IDataReader dataReader = new FileData();
        //{


        //public ActionResult Teachers()
        //    Repository loadData = new Repository();
        //    loadData.DataReader = dataReader;
        //    loadData.GetCourses();
        //    loadData.GetRooms();
        //    loadData.GetTeachers();
        //    loadData.GetUserCourses("2054313");
        //    loadData.GetSchedule("2054313");

        //    var startDateTime = Convert.ToDateTime(Request.QueryString["start"]);
        //    var endDateTime = Convert.ToDateTime(Request.QueryString["end"]);

        //    //The manager will start the timetableitem list with the data read from the repo
        //    TimeTableManager manager = new TimeTableManager(loadData, startDateTime, endDateTime);

        //    ICollection<ITimeTableItem> items = new List<ITimeTableItem>();


        //    return Content(JsonConvert.SerializeObject(manager.Repository.Teachers.ToArray()), "application/json");
        //}
    }
}