using System;

namespace Common
{
    public abstract class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Passport { get; set; }
        public Person() { }
        public Person(string firstName, string lastName, string passport)
        {
            FirstName = firstName;
            LastName = lastName;
            Passport = passport;
        }
    }

    [Serializable]
    public class Student : Person
    {
        public string StudentID { get; set; }
        public int Course { get; set; }
        public string MilitaryID { get; set; }
        public Student() { }
        public Student(string firstName, string lastName, string passport, string id, int course, string militaryID)
            : base(firstName, lastName, passport)
        {
            StudentID = id;
            Course = course;
            MilitaryID = militaryID;
        }
    }

    [Serializable]
    public class FootballPlayer : Person
    {
        public string Team { get; set; }
        public FootballPlayer() { }
        public FootballPlayer(string firstName, string lastName, string passport, string team)
            : base(firstName, lastName, passport)
        {
            Team = team;
        }
    }

    [Serializable]
    public class Lawyer : Person
    {
        public string Company { get; set; }
        public Lawyer() { }
        public Lawyer(string firstName, string lastName, string passport, string company)
            : base(firstName, lastName, passport)
        {
            Company = company;
        }
    }
}