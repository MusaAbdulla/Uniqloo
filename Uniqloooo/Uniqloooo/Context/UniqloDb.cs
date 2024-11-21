﻿using Microsoft.EntityFrameworkCore;
using Uniqloooo.Models;
namespace Uniqloooo.Context
{
    public class UniqloDb:DbContext
    {
        public DbSet<Slider> sliders { get; set; }
        public UniqloDb(DbContextOptions opt):base(opt) { }
     
    }
}