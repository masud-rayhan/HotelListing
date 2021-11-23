using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWrork;
        private readonly ILogger<HotelController> _logger;
        private readonly IMapper _mapper;

        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper )
        {
            _unitOfWrork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }



        [HttpGet]
        
        
        public async Task<IActionResult> GetHostels()
        {
            try
            {
                var hostels = await _unitOfWrork.Hotels.GetAll();
                var results = _mapper.Map<IList<HotelDTO>>(hostels);
                return Ok(results);
                 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in tha {nameof(GetHostels)}");
                return StatusCode(500, "Internal Server Error. Please Try again later");
            }
        }




        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetHostel(int id)
        {
            try
            {
                var hostels = await _unitOfWrork.Hotels.Get(q=>q.Id==id, new List<string> {"Country"});
                var results = _mapper.Map<HotelDTO>(hostels);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in tha {nameof(GetHostels)}");
                return StatusCode(500, "Internal Server Error. Please Try again later");
            }
        }


    }
}
