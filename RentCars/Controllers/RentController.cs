using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentCars.Models;
using RentCars.ModelsDto;
using RentCars.Services;

namespace RentCars.Controllers
{
    [Authorize(Policy = "RequireAdminOrUserRole")]
    public class RentController : Controller
    {
        private readonly IRentService _rentService;
        private readonly ICarService _carService;
        private readonly UserManager<IdentityUser> _userManager;
     

        public RentController(
            IRentService rentService,
            ICarService carService,
            UserManager<IdentityUser> userManager)
        {
            _rentService = rentService;
            _carService = carService;
            _userManager = userManager;
           
        }

        private async Task<IdentityUser> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return user;
        }



        [HttpPost]
        [AllowAnonymous]
        public IActionResult FindCars(IFormCollection form)
        {
            var city = form["city"];
            var dayOfRent = DateTime.Parse(form["dayOfRent"]);
            var dayOfReturn = DateTime.Parse(form["dayOfReturn"]);

            return RedirectToAction("AvailableCars", new { city, dayOfRent, dayOfReturn });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AvailableCars(string city, DateTime dayOfRent, DateTime dayOfReturn)
        {
            var availableCars = await _rentService.GetAvailableCars(city, dayOfRent, dayOfReturn);

            return View(availableCars);
        }


        [HttpGet]
        public async Task<IActionResult> RentCar(CarDto carDto)
        {
            var car = await _carService.GetById(carDto.Id);

            ReservationDto dto = new();
            var dayOfRent = DateTime.Parse(carDto.DateOfRent);
            var dayOfReturn = DateTime.Parse(carDto.DateOfReturn);
            var currentUser = await GetCurrentUser();

            dto.City = carDto.City;
            dto.UserId = currentUser.Id;

            decimal price = _rentService.CountPrice(carDto.Id, dayOfRent, dayOfReturn);
            dto.Brand = car.Brand;
            dto.Model = car.Model;
            dto.CarId = carDto.Id;

            dto.DateOfRent = dayOfRent;
            dto.DateOfReturn = dayOfReturn;
            dto.Price = price;

            return View(dto);
        }

        public async Task<IActionResult> RentCarPost(ReservationDto dto)
        {
            await _rentService.Rent(dto);

            return RedirectToAction("Index", "Home");
        }

        //public IActionResult Summary(Reservation res)
        //{
        //    return View();
        //}

        
    }
}
