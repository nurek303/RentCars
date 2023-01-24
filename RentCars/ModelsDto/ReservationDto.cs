using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCars.ModelsDto
{
    public class ReservationDto
    {
        public string Brand { get; set; }

        public string Model { get; set; }

        public int CarId { get; set; }

        public DateTime DateOfRent { get; set; }

        public DateTime DateOfReturn { get; set; }

        public string City { get; set; }

        public decimal Price { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }
    }
}
