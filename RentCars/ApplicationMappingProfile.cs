using AutoMapper;
using RentCars.Models;
using RentCars.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCars
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<CarDto, Car>();

            CreateMap<Car, CarDto>();
            CreateMap<Car, CreateCarDto>();

            CreateMap<CreateCarDto, Car>()
                .ForMember(r => r.FilePath, r => r.MapFrom(dto => dto.Image.FileName.ToString()));

            CreateMap<CreateCarDto, CarDto>();

          
            CreateMap<ReservationDto, RentalHistory>()
                .ForMember(d => d.DateOfRent, c => c.MapFrom(dto => dto.DateOfRent))
                .ForMember(d => d.DateOfReturn, c => c.MapFrom(dto => dto.DateOfReturn))
                .ForMember(d => d.CarId, c => c.MapFrom(dto => dto.CarId));

            CreateMap<ReservationDto, Reservation>();


            CreateMap<RentalHistory, ReservationHistoryDto>()
                .ForMember(d => d.Brand, b => b.MapFrom(dto => dto.Car.Brand))
                .ForMember(d => d.Model, m => m.MapFrom(dto => dto.Car.Model))
                .ForMember(d => d.FilePath, m => m.MapFrom(dto => dto.Car.FilePath))
                .ForMember(d => d.FullPrice, m => m.MapFrom(dto => dto.Reservation.FullPrice))
                .ForMember(d => d.City, m => m.MapFrom(dto => dto.Reservation.City));
               

        }
    }
}
