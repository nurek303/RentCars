using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentCars.Data;
using RentCars.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCars.Services
{
    public interface IReservationService
    {
        public Task<List<ReservationHistoryDto>> GetReservations(string userId);
    }

    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public ReservationService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<List<ReservationHistoryDto>> GetReservations(string userId)
        {
           var rentalHistory = await _dbContext.RentalHistory
                .Include(d=>d.Car)
                .Include(d=>d.Reservation)
                .Where(u=>u.Reservation.UserId == userId)
                .ToListAsync();

            var userReservations = _mapper.Map<List<ReservationHistoryDto>>(rentalHistory);

            return userReservations;
        }
    }
}
