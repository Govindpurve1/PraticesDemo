//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GovindEventReminderService.Models
{
    using System;
    
    public partial class SP_CheckUserLogin_Result
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Nullable<int> HintQuetionId { get; set; }
        public string NewHintQuestion { get; set; }
        public string HintQuestionAnswer { get; set; }
        public Nullable<System.DateTime> DateOfRegistraion { get; set; }
        public Nullable<System.DateTime> LastAccessDateTime { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<bool> IsCancelled { get; set; }
    }
}
