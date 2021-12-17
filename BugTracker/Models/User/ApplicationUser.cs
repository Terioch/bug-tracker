﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Column(TypeName = "varchar(100)")]
    public string? FirstName { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? LastName { get; set; }

    public static implicit operator string(ApplicationUser v)
    {
        throw new NotImplementedException();
    }
}

