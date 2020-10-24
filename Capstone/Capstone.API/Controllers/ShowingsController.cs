using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Clusters;
using System;
using System.Collections.Generic;

namespace Capstone.API.Controllers
{
    [Route("api/showings")]
    [ApiController]
    public class ShowingsController : ControllerBase
    {
        private readonly ShowingService _showingService;
        private readonly IMapper _mapper;

        public ShowingsController(ShowingService showingService,
            IMapper mapper)
        {
            _showingService = showingService ??
                throw new ArgumentNullException(nameof(showingService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
    }
}
