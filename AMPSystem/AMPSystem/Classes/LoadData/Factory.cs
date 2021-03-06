﻿using System;
using System.Collections.Generic;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes.LoadData
{
    public class Factory
    {
        #region Singleton
        private static Factory _instance;

        private Factory(){ }

        public static Factory Instance => _instance ?? (_instance = new Factory());
        #endregion

        // Creates Lessons
        public ITimeTableItem Create(int id, string name, DateTime startTime, DateTime endTime, ICollection<Room> rooms,
            ICollection<Course> courses, string type, User teacher)
        {
            return new Lesson(id, startTime, endTime, rooms, courses, type, name, teacher);
        }
        public ITimeTableItem Create(int id, string name, string color, DateTime startTime, DateTime endTime,
            ICollection<Room> rooms, ICollection<Course> courses, string type, User teacher, string reminder)
        {
            return new Lesson(id, startTime, endTime, color, rooms, courses, type, name, teacher, reminder);
        }

        //Creates OfficeHours
        public ITimeTableItem Create(int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms, User teacher,
            string name)
        {
            return new OfficeHours(id, name, startTime, endTime, rooms, teacher);
        }
        public ITimeTableItem Create(int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms, User teacher,
            string name, string color, string reminder)
        {
            return new OfficeHours(id, name, startTime, endTime, rooms, teacher, color, reminder);
        }

        // Creates EvaluationMoment
        public ITimeTableItem Create(int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms,
            ICollection<Course> courses, string name)
        {
            return new EvaluationMoment(id, startTime, endTime, rooms, courses, name);
        }
        public ITimeTableItem Create(int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms,
            ICollection<Course> courses, string name, string color, string reminder)
        {
            return new EvaluationMoment(id, startTime, endTime, rooms, color, courses, name, reminder);
        }
        public ITimeTableItem Create(DateTime startTime, DateTime endTime, ICollection<Room> rooms,
            ICollection<Course> courses, string name, string color, string description, bool editable, string reminder)
        {
            return new EvaluationMoment(startTime, endTime, rooms, courses, name, color, description, editable, reminder);
        }

        public Room CreateRoom(int id, string name, int floor)
        {
            return new Room(id, name, floor);
        }

        public Course CreateCourse(int id, string name, ICollection<int> years)
        {
            return new Course(id, name, years);
        }

        public Building CreateBuilding(int id, string name, string address, ICollection<Room> rooms)
        {
            return new Building(id, name, address, rooms);
        }

        // Creates User.
        public User CreateUser(int id, string name, string email, ICollection<string> roles, ICollection<Course> courses)
        {
            return new User(id, name, email, roles, courses);
        }
        public User CreateUser(string name, string email, ICollection<string> roles, ICollection<Course> courses)
        {
            return new User(name, email, roles, courses);
        }
    }
}