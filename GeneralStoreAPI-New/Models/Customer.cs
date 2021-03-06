﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralStoreAPI_New.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string FullName { get; set; }
        //readonly -- remove setter! Do another migration before running
        /*{
            get
            {
                string concatenate = FirstName + LastName;
                return concatenate;
            }
        }*/
        public virtual List<Transaction> Transactions { get; set; } = new List<Transaction>(); //?
    }
}