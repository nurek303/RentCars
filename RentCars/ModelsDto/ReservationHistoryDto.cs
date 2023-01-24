using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCars.ModelsDto
{
    public class ReservationHistoryDto
    {
        public string Brand { get; set; }

        public string Model { get; set; }

        public string? FilePath { get; set; }

        public DateTime? DateOfRent { get; set; }

        public DateTime? DateOfReturn { get; set; }

        public string City { get; set; }

        public decimal FullPrice { get; set; }
    }
}
