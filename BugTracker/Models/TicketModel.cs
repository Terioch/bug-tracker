﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class TicketModel
    {
        public string? Id { get; set; }

        public string? ProjectId { get; set; } 

        public string? Description { get; set; }        
    }
}
