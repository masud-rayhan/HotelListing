using HotelListing.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models
{
    public class CountryDTO : UpdateCountryDTO
    {
        public int Id { get; set; }
        
    }

    public class CreateCountryDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Country Name Is Too Long")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "ShortCountry Name Is Too Long")]
        public string ShortName { get; set; }
        
    }

    public class UpdateCountryDTO : CreateCountryDTO
    {
        public IList<CountryDTO> Countries { get; set; }
    }
}
