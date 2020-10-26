﻿using Capstone.API.Models.Showing;
using System;

namespace Capstone.API.Models
{
    public abstract class CustomOutboundShowingDto : OutboundShowingDto
    {

        public string CounterpartID { get; set; }
        public bool Host { get; set; }
    }
}
