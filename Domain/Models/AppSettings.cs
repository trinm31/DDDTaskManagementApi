﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AppSettings
    {
        public static string ConnectionString { get; private set; }
        public static string Secret { get; set; }
    }
}
