using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentCars.Data;
using RentCars.Models;
using RentCars.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCars.Services
{
    public interface IRentService
    {
        Task<IEnumerable<CarDto>> GetAvailableCars(string city, DateTime dayOfRent, DateTime dayOfReturn);
        decimal CountPrice(int id, DateTime dayRent, DateTime dayReturn);
        Task<string> Rent(ReservationDto dto);
    }
    public class RentService : IRentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;


        public RentService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CarDto>> GetAvailableCars(string city, DateTime dayOfRent, DateTime dayOfReturn)
        {      
            var cars = await _dbContext.Cars
            .Include(c => c.RentalHistory)
            .Where(c => !c.RentalHistory.Any(r => r.DateOfRent <= dayOfReturn && r.DateOfReturn >= dayOfRent))
            .ToListAsync();

            var carsDto = _mapper.Map<List<CarDto>>(cars);

            foreach (var car in carsDto)
            {
                car.City = city;
                car.DateOfRent = dayOfRent.ToString();
                car.DateOfReturn = dayOfReturn.ToString();
            }
            return carsDto;
        }

        public decimal CountPrice(int id, DateTime dayRent, DateTime dayReturn)
        {
            var days = dayReturn - dayRent;

            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == id);

            if (car == null)
                throw new Exception();

            decimal price = car.Price;

            if (days.Days == 0)
            {
                return price * 1;
            }

            return price * Convert.ToInt32(days.Days);
        }

        public async Task<string>  Rent(ReservationDto dto)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(r => r.Id == dto.CarId);
            var user = await _dbContext.Users.FirstOrDefaultAsync(r => r.Id == dto.UserId);
            int id = dto.CarId;
            
                //mapper
                Reservation reservation = new Reservation();

                reservation.Email = user.Email;
                reservation.FirstName = dto.FirstName;
                reservation.LastName = dto.LastName;
                reservation.City = dto.City;
                reservation.FullPrice = dto.Price;
                reservation.UserId = dto.UserId;
                _dbContext.Reservations.Add(reservation);
                _dbContext.SaveChanges();


                RentalHistory date = new RentalHistory();
                date.DateOfRent = dto.DateOfRent;
                date.DateOfReturn = dto.DateOfReturn;
                date.CarId = id;
                date.ReservationId = reservation.Id;
                _dbContext.RentalHistory.Add(date);
                _dbContext.SaveChanges();


            return $"Rezerwacja  auta:{car.Brand} przebiegła pomyślnie ";
        }
    }
}
