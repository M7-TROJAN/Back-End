﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _02_Using_DataAnnotations_with_Scaffold_DbContext;

public partial class Speaker
{
    [Key]
    public int Id { get; set; }

    [StringLength(25)]
    public string FirstName { get; set; } = null!;

    [StringLength(25)]
    public string LastName { get; set; } = null!;

    [InverseProperty("Speaker")]
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
