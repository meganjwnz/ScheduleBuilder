using System;

namespace ScheduleManager.Model
{
    /// <summary>
    /// These are the values of the person class
    /// all users of Schedule Manager will require these values
    /// 
    /// SSN is optional
    /// 
    /// </summary>
    public class Person
    {
        public int Id {get; set;}
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char? Ssn { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string StreetAddress { get; set; }
        public string Zipcode { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public int RoleId { get; set; }
        public int StatusId { get; set; }
    }
}
