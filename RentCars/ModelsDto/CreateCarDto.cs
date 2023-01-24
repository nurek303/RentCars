using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCars.ModelsDto
{
    public class CreateCarDto
    {
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int MaxSpeed { get; set; }
        [Required]
        public int Milage { get; set; }
        [Required]
        public string Transmisson { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int NumberOfSeats { get; set; }
        [Required]
        public string CarPlate { get; set; }
        [Required]
        public string Vin { get; set; }

        [Required]
        public IFormFile Image { get; set; } = null!;
    }
}
