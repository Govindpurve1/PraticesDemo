//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GovindEventReminderService.Areas.User.Models
{
    using System;
    
    public partial class GetBirthdayReminder
    {
        public int BirthdayReminder { get; set; }
        public int UserId { get; set; }
        public int BirthdayDetailsId { get; set; }
        public Nullable<bool> C30DaysBefore { get; set; }
        public Nullable<bool> C14DaysBefore { get; set; }
        public Nullable<bool> C7DaysBefore { get; set; }
        public Nullable<bool> C3DaysBefore { get; set; }
        public Nullable<bool> DayofEvent { get; set; }
    }
}
