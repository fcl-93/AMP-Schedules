﻿using System;
using System.Collections.Generic;
using AMPSystem.Classes;

namespace AMPSystem.Interfaces
{
    public interface  ITimeTableItem
    {
        int ItemID { get; set; }
        string Name { get; set; }
        string Color { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        ICollection<Room> Rooms { get; set; }
    }
}