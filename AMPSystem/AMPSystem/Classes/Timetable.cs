﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class Timetable
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public ICollection<ITimeTableItem> ItemList { get; set; }

        public Timetable(DateTime startDateTime, DateTime endDateTime)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            ItemList = new List<ITimeTableItem>();
        }

        public void AddTimetableItem(ITimeTableItem item)
        {
            ItemList.Add(item);
        }

        public void RemoveTimetableItem(ITimeTableItem item)
        {
            ItemList.Remove(item);
        }
    }
}
