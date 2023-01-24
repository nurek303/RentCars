using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCars.Models
{
    public class RentalHistory
    {
        public int Id { get; set; }

        public DateTime? DateOfRent { get; set; }

        public DateTime? DateOfReturn { get; set; }

        public int CarId { get; set; }

        public virtual Car Car { get; set; }

        public int ReservationId { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}
