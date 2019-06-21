using SendGrid.Helpers.Mail;
using System;
using System.ComponentModel.DataAnnotations;

namespace ScheduleBuilder.Model
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

        public int Id { get; set; }

        public string LastName { get; set; }
        
        public string FirstName { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateOfBirth { get; set; }

        public string Ssn { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }

        public string StreetAddress { get; set; }

        public string Zipcode { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
   
        public int RoleId { get; set; }

        public int StatusId { get; set; }

        public string Email { get; set; }
    }
}
