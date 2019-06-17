using System;
using System.ComponentModel.DataAnnotations;

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

        public int Id { get; set; }

        [Display(Name = "Last Name")]
        [Required (ErrorMessage = "Last name required")]
        public string LastName { get; set; }


        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Last name required")]
        public string FirstName { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Last name required")]
        public DateTime DateOfBirth { get; set; }

        
        [Display(Name = "SSN")]
        public char? Ssn { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Last name required")]
        public string Gender { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Last name required")]
        public string Phone { get; set; }

        [Display(Name = "Street address")]
        [Required(ErrorMessage = "Last name required")]
        public string StreetAddress { get; set; }

        [Display(Name = "Zipcode")]
        [Required(ErrorMessage = "Last name required")]
        public string Zipcode { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Last name required")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Last name required")]
        public string Password { get; set; }

        [Display(Name = "Role Id")]
        [Required(ErrorMessage = "Last name required")]
        public int RoleId { get; set; }

        [Display(Name = "Status Id")]
        [Required(ErrorMessage = "Last name required")]
        public int StatusId { get; set; }


    }
}
