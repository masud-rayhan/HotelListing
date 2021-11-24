using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hostels = await _unitOfWrork.Hotels.GetAll();
                var results = _mapper.Map<IList<HotelDTO>>(hostels);
                return Ok(results);
                 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in tha {nameof(GetHotels)}");
                return StatusCode(500, "Internal Server Error. Please Try again later");
            }
        }




        [HttpGet("{id:int}", Name ="GetHotel")]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hostels = await _unitOfWrork.Hotels.Get(q=>q.Id==id, new List<string> {"Country"});
                var results = _mapper.Map<HotelDTO>(hostels);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in tha {nameof(GetHotels)}");
                return StatusCode(500, "Internal Server Error. Please Try again later");
            }
        }

        //[Authorize(Roles="Administrator")]
        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO )
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post Attemp in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDTO);
                await _unitOfWrork.Hotels.Insert(hotel);
                await _unitOfWrork.Save();

                return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotel);

            }catch(Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in tha {nameof(CreateHotel)}");
                return StatusCode(500, "Internal Server Error. Please Try again later");
            }
        }




        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO hotelDTO)
        {
            if(!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid Update attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = await _unitOfWrork.Hotels.Get(q => q.Id == id);
                if(hotel == null)
                {
                    _logger.LogError($"Invaid Update attempt in {nameof(UpdateHotel)}");
                    return BadRequest("Submitted Data is Invalid");
                }

                _mapper.Map(hotelDTO,hotel);
                _unitOfWrork.Hotels.Update(hotel);
                await _unitOfWrork.Save();
                
                return NoContent();

            }catch(Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in tha {nameof(UpdateHotel)}");
                return StatusCode(500, "Internal Server Error. Please Try again later");
            }
        }

    }
}
