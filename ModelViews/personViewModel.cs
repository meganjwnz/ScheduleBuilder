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
    /// SSN and Phonenumber are checked to insure only numeral input
    /// 
    /// </summary>
    public class PersonViewModel
    {
        //Lastname
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name required")]
        public string LastName { get; set; }

        //Firstname
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name required")]
        public string FirstName { get; set; }

        //DOB
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date of Birth required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        //SSN
        [Display(Name = "SSN")]
        [RegularExpression(@"^\d{9}|\d{3}-\d{2}-\d{4}$", ErrorMessage = "Invalid Social Security Number")]
        public string Ssn { get; set; }


        //Gender
        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender required")]
        public string Gender { get; set; }


        //Phone
        [Display(Name = "Phone number \n##########")]
        [Required(ErrorMessage = "Phone number required")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone# Only enter numbers")]
        public string Phone { get; set; }


        //Address
        [Display(Name = "Street address")]
        [Required(ErrorMessage = "Address required")]
        public string StreetAddress { get; set; }


        //Zipcode
        [Display(Name = "Zipcode")]
        [Required(ErrorMessage = "Zipcode required")]
        [StringLength(5, MinimumLength = 5)]
        public string Zipcode { get; set; }

        //Email
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //Confirming Email
        [Display(Name = "Confirm Email address")]
        [Compare("Email", ErrorMessage = "Email and Confirm Email must match")]
        public string CompareEmail { get; set; }

    }
}
