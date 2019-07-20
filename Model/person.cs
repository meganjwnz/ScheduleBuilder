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
        //PersonID
        public int Id { get; set; }

        //Person's Lastname
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name required")]
        public string LastName { get; set; }

        //Person Firstname
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name required")]
        public string FirstName { get; set; }

        //Person DateOfBirth
        [Required(ErrorMessage = "Date of Birth required")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateOfBirth { get; set; }

        //Person SSN
        [Display(Name = "SSN")]
        [StringLength(9, MinimumLength = 9)]
        public string Ssn { get; set; }

        //Person Gender
        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender required")]
        public string Gender { get; set; }

        //Person Name
        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Phone number required")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10)]
        public string Phone { get; set; }

        //Person's Address
        [Display(Name = "Street address")]
        [Required(ErrorMessage = "Address required")]
        public string StreetAddress { get; set; }

        //Person's Zipcode
        [Display(Name = "Zipcode")]
        [Required(ErrorMessage = "Zipcode required")]
        [StringLength(5, MinimumLength = 5)]
        public string Zipcode { get; set; }


        //Person Username
        [Display(Name = "Username")]
        [Required(ErrorMessage = "User name required")]
        public string Username { get; set; }

        //Person's password
        public string Password { get; set; }

        //person's roleID
        public int RoleId { get; set; }

        //Person StatusId
        public int StatusId { get; set; }

        //Person's email
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //Returns the full name of the person
        //$ style is much better data usage 
        public string GetFullName()
        {
            return ($"{ this.FirstName } { this.LastName}");
        }
    }
}
