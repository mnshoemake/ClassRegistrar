using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ClassRegistrar.Models
{
    public class LoginRequest
    {
        private int _LoginID;
        private string _Name;
        private string _EmailAddress;
        private string _LoginName;
        private string _NewOrReactivate;
        private string _ReasonForAccess;
        private DateTime _DateRequiredBy;

        public LoginRequest()
        {
            LoginID = 1000;
            Name = "ERROR";
            EmailAddress = "ERROR";
            LoginName = "ERROR";
            NewOrReactivate = "ERROR";
            ReasonForAccess = "ERROR";
            DateRequiredBy = DateTime.Now;
        }

        public LoginRequest(int loginID
            , string name
            , string emailAddress
            , string loginName
            , string newOrReactivate
            , string reasonForAccess
            , DateTime dateRequiredBy)
        {
            this.LoginID = loginID;
            this.Name = name;
            this.EmailAddress = emailAddress;
            this.LoginName = loginName;
            this.NewOrReactivate = newOrReactivate;
            this.ReasonForAccess = reasonForAccess;
            this.DateRequiredBy = dateRequiredBy;
        }

        public int LoginID { get => _LoginID; set => _LoginID = value; }
        [Required]
        public string Name { get => _Name; set => _Name = value; }
        [EmailAddress, Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get => _EmailAddress; set => _EmailAddress = value; }
        [Required]
        [Display(Name = "User Name")]
        public string LoginName { get => _LoginName; set => _LoginName = value; }
        [Required]
        [Display(Name = "New Or Reactivate")]
        public string NewOrReactivate { get => _NewOrReactivate; set => _NewOrReactivate = value; }
        [Required]
        [Display(Name = "Reason For Access")]
        public string ReasonForAccess { get => _ReasonForAccess; set => _ReasonForAccess = value; }
        [Required]
        [Display(Name = "Date Required By")]
        public DateTime DateRequiredBy { get => _DateRequiredBy; set => _DateRequiredBy = value; }


    }
}