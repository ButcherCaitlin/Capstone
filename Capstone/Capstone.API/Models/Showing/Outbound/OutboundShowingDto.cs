﻿using System;

namespace Capstone.API.Models
{
    public abstract class OutboundShowingDto
    {
        public string Id { get; set; }
        public string PropertyID { get; set; }
        public DateTimeOffset Start { get; set; }
        public TimeSpan Duration { get; set; }
    }
}