using RentCars.Data;
using RentCars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCars
{
    public class Seeder
    {
        public static void Seed(ApplicationDbContext dbContext)
        {
            if (!dbContext.Cars.Any())
            {
                var rental = GetCars();
                dbContext.AddRange(rental);
                dbContext.SaveChanges();
            }
        }
        private static IEnumerable<Car> GetCars()
        {
            var cars = new List<Car>()
            {
                new Car()
                {
                    Brand = "BMW",
                    Model = "M2",
                    MaxSpeed = 240,
                    Year = 2020,
                    Milage = 30000,
                    NumberOfSeats = 4,
                    Price = 150,
                    Transmisson = "Automat",
                    CarPlate ="PO 24151",
                    Vin = "42151252622",
                    FilePath="M2.jpg"
                },
                new Car()
                {
                    Brand = "Audi",
                    Model = "A8",
                    MaxSpeed = 230,
                    Year = 2021,
                    Milage = 10000,
                    NumberOfSeats = 5,
                    Price = 130,
                    Transmisson = "Automat",
                     CarPlate ="PO 53212",
                     Vin = "42151252622",
                     FilePath="A8.jpg"
                },
                new Car()
                {
                    Brand = "Mazda",
                    Model = "6",
                    MaxSpeed = 200,
                    Year = 2019,
                    Milage = 45000,
                    NumberOfSeats = 5,
                    Price = 110,
                    Transmisson = "Manual",
                     CarPlate ="PO 21342",
                     Vin = "42151252622",
                     FilePath="6.jpg"
                },

            };
            return cars;
        }
    }
}
