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
        [RegularExpression(@"^\d{9}|\d{3}-\d{2}-\d{4}$", ErrorMessage = "Invalid Social Security Number")]
        public string Ssn { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender required")]
        public string Gender { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Phone number required")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string Phone { get; set; }

        [Display(Name = "Street address")]
        [Required(ErrorMessage = "Address required")]
        public string StreetAddress { get; set; }

        [Display(Name = "Zipcode")]
        [Required(ErrorMessage = "Zipcode required")]
        [StringLength(5, MinimumLength = 5)]
        public string Zipcode { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Confirm Email address")]
        [Compare("Email", ErrorMessage = "Email and Confirm Email must match")]
        public string CompareEmail { get; set; }

    }
}
