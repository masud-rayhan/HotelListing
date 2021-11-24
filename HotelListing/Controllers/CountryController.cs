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
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWrork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;


        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger,IMapper mapper)
        {
            _unitOfWrork = unitOfWork; ;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCounties()
        {
            try
            {
                var countries =await _unitOfWrork.Countries.GetAll();
                var results = _mapper.Map<IList<CountryDTO>>(countries);
                return Ok(results);

            }catch(Exception ex)
            {
                _logger.LogError(ex,$"Something Went Wrong in tha {nameof(GetCounties)}");
                return StatusCode(500,"Internal Server Error. Please Try again later");
            }
        }




        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWrork.Countries.Get(q=>q.Id==id, new List<string> { "Hotels" });
                var results = _mapper.Map<CountryDTO>(country);
                return Ok(results);

            }catch(Exception ex)
            {
                _logger.LogError(ex,$"Something Went Wrong in tha {nameof(GetCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try again later");
            }
        }




        [HttpPut]
        public async Task<IActionResult> UpdateCountry(int id,[FromBody] UpdateCountryDTO countryDTO)
        {
            if(!ModelState.IsValid || id< 1)
            {
                _logger.LogError($"Invalid Update Attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = await _unitOfWrork.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                    _logger.LogError($"Invaid Update attempt in {nameof(UpdateCountry)}");
                    return BadRequest("Submitted Data is Invalid");
                }

                _mapper.Map(countryDTO, country);

                _unitOfWrork.Countries.Update(country);
                await _unitOfWrork.Save();

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in tha {nameof(UpdateCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try again later");
            }

        }
    }
}
