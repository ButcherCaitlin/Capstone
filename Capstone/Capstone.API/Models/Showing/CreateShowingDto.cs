using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.API.Models
{
    public class CreateShowingDto
    {
        [Required]
        public DateTimeOffset StartTime { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
    }
}
