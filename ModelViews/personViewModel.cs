using System;
using System.ComponentModel.DataAnnotations;

namespace ScheduleBuilder.ModelViews
{
    /// <summary>
    /// These are the values of the person class
    /// all users of Schedule Manager will require these values
    /// 
    /// SSN is optional
    /// 
    /// </summary>
    public class PersonViewModel
    {

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name required")]
        public string LastName { get; set; }


        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name required")]
        public string FirstName { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date of Birth required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }


        [Display(Name = "SSN")]
        public string Ssn { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender required")]
        public string Gender { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Phone number required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Street address")]
        [Required(ErrorMessage = "Address required")]
        public string StreetAddress { get; set; }

        [Display(Name = "Zipcode")]
        [Required(ErrorMessage = "Zipcode required")]
        public string Zipcode { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "User name required")]
        public string Username { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Confirm Email address")]
        [Compare("Email", ErrorMessage = "Email and Confirm Email must match")]
        public string CompareEmail { get; set; }

    }
}
