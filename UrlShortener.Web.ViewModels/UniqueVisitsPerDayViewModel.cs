﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Web.ViewModels
{
    public class UniqueVisitsPerDayViewModel
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
