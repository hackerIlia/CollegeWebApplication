﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollegeWebApplication.Models;

public partial class Teacher
{
    public int IdTeacher { get; set; }

    [Display(Name = "Name")]
    public string NameTeahcer { get; set; }

    [Display(Name = "Surname")]
    public string SurnameTeacher { get; set; }

    public string Degree { get; set; }

    public virtual ICollection<GroupCollege> GroupColleges { get; set; } = new List<GroupCollege>();
}