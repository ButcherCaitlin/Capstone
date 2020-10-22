using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Capstone.API.Models;
using Capstone.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;

namespace Capstone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly PropertyService propertyService;

        public PropertiesController(PropertyService propertyService)
        {
            this.propertyService = propertyService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Property>> Get()
        {
            return Ok(propertyService.Get());
        }

        [HttpGet("{id:length(24)}", Name = "GetProperty")]
        public ActionResult<Property> Get(string id)
        {
            var property = propertyService.Get(id);
            if (property == null) return NotFound();
            return property;
        }

        [HttpPost]
        public ActionResult<Property> Create(Property property)
        {
            propertyService.Create(property);
            return CreatedAtRoute("GetProperty", new { id = property.Id.ToString() }, property);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Property propertyIn)
        {
            var property = propertyService.Get(id);
            if (property == null) return NotFound();
            propertyService.Update(id, propertyIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var property = propertyService.Get(id);
            if (property == null) return NotFound();
            propertyService.Remove(property.Id);
            return NoContent();
        }
    }
}
