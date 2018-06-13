using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GovindEventReminderService.Areas.Admin.Models
{
    public class Admin
    {
        [Required(ErrorMessage ="The Old Password Field is required.")]
        [Display(Name ="OldPassword")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "The New Password Field is required.")]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "The Confirm Password Field is required.")]
        [Display(Name = "ConfirmPassword")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "The CountryId is required.")]
        [Display(Name = "CountryId")]
        public int CountryId
        {
            get;set;
        }
        [Required(ErrorMessage ="Country Name Required")]
        [Display(Name = "CountryName")]
        public string CountryName
        {
            get; set;
        }
        [Required(ErrorMessage = "isActive Required")]
        [Display(Name = "isActive")]
        public bool isActive
        {
            get; set;
        }
        public int StateId
        {
            get; set;
        }
        [Required(ErrorMessage = "Country Name Required")]
        public string StateName
        {
            get; set;
        }
       
       

    }
}