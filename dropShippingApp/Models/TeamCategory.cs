﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dropShippingApp.Models
{
    public class TeamCategory : Category
    {
        [Key]
        public int TeamCategoryID { get; set; }
    }
}
