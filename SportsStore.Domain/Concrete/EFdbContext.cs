﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SportsStore.Domain.Concrete
{
    public class EFdbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}
