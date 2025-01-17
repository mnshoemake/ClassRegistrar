﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ClassRegistrar.Models
{
    public class Class
    {

        private int _ClassID;
        private string _ClassName;
        private DateTime _ClassDate;
        private string _ClassDescription;

        //Constructor
        public Class(int ClassID, string ClassName, DateTime ClassDate, string ClassDescription)
        {
            this.ClassID = ClassID;
            this.ClassName = ClassName;
            this.ClassDate = ClassDate;
            this.ClassDescription = ClassDescription;
        }

        public int ClassID { get => _ClassID; set => _ClassID = value; }
        [Display(Name = "Class Name")]
        public string ClassName { get => _ClassName; set => _ClassName = value; }
        [Display(Name = "Start Date")]
        public DateTime ClassDate { get => _ClassDate; set => _ClassDate = value; }
        [Display(Name = "Class Description")]
        public string ClassDescription { get => _ClassDescription; set => _ClassDescription = value; }

    }
}