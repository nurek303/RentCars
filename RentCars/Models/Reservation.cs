using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCars.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string City { get; set; }

        public decimal FullPrice { get; set; }

        public string UserId { get; set; }

        public ICollection<RentalHistory> RentalHistory { get; set; }

        public virtual List<IdentityUser> User { get; set; }
    }
}
