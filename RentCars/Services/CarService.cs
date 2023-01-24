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
    public interface ICarService
    {
        Task<IEnumerable<CarDto>> GetCars();
        void Create(CreateCarDto dto);
        Task<CarDto> GetById(int id);
        Task Delete(int id);
        Task Edit(CarDto dto);
    }

    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly string directoryPath;


        public CarService(ApplicationDbContext dbContext, IMapper mapper,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            directoryPath = configuration["AppSettings:ImageDir"];
        }

        public async Task<IEnumerable<CarDto>> GetCars()
        {
            var cars = await _dbContext.Cars.ToListAsync();

            var carsDto = _mapper.Map<List<CarDto>>(cars);

            return carsDto;
        }


        //details
        public async Task<CarDto> GetById(int id)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(x => x.Id == id);

            var carDto = _mapper.Map<CarDto>(car);

            return carDto;
        }
        public void Create(CreateCarDto dto)
        {
            CreateFile(dto, null);

            var car = _mapper.Map<Car>(dto);

            _dbContext.Add(car);
            _dbContext.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(x => x.Id == id);

            if (car != null)
            {
             
                DeleteFile(car);

                _dbContext.Cars.Remove(car);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception();
            }

        }

        public async Task Edit(CarDto dto)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (car != null && dto != null)
            {
                if (dto.Image != null)
                {
                    DeleteFile(car);
                    CreateFile(null, dto);

                    car = UpdateCar(dto, car, dto.Image.FileName);
                }
                else
                {
                    car = UpdateCar(dto, car, dto.FilePath);
                }
                _dbContext.UpdateRange(car);
                _dbContext.SaveChanges();
            }
        }
        private Car UpdateCar(CarDto dto, Car car, string filePath)
        {
            car.FilePath = filePath;
            car.Brand = dto.Brand;
            car.Model = dto.Model;
            car.Price = dto.Price;
            car.CarPlate = dto.CarPlate;
            car.Year = dto.Year;
            car.MaxSpeed = dto.MaxSpeed;
            car.Milage = dto.Milage;
            car.NumberOfSeats = dto.NumberOfSeats;
            car.Transmisson = dto.Transmisson;
            car.Vin = dto.Vin;

            return car;
        }


        private void DeleteFile(Car car)
        {
            Directory.CreateDirectory(directoryPath);
            string filePath = Path.Combine(directoryPath, car.FilePath);
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                File.Delete(filePath);
            }
        }

        private void CreateFile(CreateCarDto? createCar, CarDto? editCar)
        {
            //dla edita
            if (createCar == null && editCar != null)
            {
                if (editCar.Image != null)
                {
                    Directory.CreateDirectory(directoryPath);

                    string filePath = Path.Combine(directoryPath, editCar.Image.FileName);

                    using var stream = System.IO.File.Create(filePath);
                    editCar.Image.CopyTo(stream);
                }
            }
            //dla create
            if (editCar == null && createCar != null)
            {
                if (createCar.Image != null)
                {
                    Directory.CreateDirectory(directoryPath);

                    string filePath = Path.Combine(directoryPath, createCar.Image.FileName);

                    using var stream = System.IO.File.Create(filePath);
                    createCar.Image.CopyTo(stream);
                }
            }
        }
    }
}
