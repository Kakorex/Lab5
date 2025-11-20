using System;

namespace BLL
{
    public abstract class PersonDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Passport { get; set; }
    }

    public class StudentDto : PersonDto
    {
        public string StudentID { get; set; }
        public int Course { get; set; }
        public string MilitaryID { get; set; }
    }

    public class FootballPlayerDto : PersonDto
    {
        public string Team { get; set; }
    }

    public class LawyerDto : PersonDto
    {
        public string Company { get; set; }
    }
}