using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.API.Models
{
    public class UpdateShowingDto : ManipulateShowingDto
    {
        public string Id { get; set; }
    }
}
