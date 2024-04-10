﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollegeWebApplication.Models;

public partial class Student
{
    public int IdStudent { get; set; }
    [Display(Name = "Name")]
    public string NameStudent { get; set; }
    [Display(Name = "Surname")]
    public string SurnameStudent { get; set; }
    [Display(Name = "Year of study")]
    [Range(1,4)]
    public int? YearOfStudy { get; set; }
    public int? IdGroup { get; set; }
    [Display(Name = "Group")]
    public virtual GroupCollege IdGroupNavigation { get; set; }
}