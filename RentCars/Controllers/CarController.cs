using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentCars.ModelsDto;
using RentCars.Services;

namespace RentCars.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        /// <summary>
        /// Pobiera tabele Cars z bazy danych
        /// </summary>
        /// <returns>zwraca widok dla metody</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _carService.GetCars();
       
            return View(cars);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetails(int id)
        {
            var details = await _carService.GetById(id);

            return View(details);
        }
        /// <summary>
        /// Pobiera tabele Cars z bazy ale z innymi funkcjami w widoku
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCarsAdmin()
        {
            var cars = await _carService.GetCars();

            return View(cars);
        }

        /// <summary>
        /// Widok tworzenia auta
        /// </summary>
        /// <returns>Zwraca widok tworzenia nowego auta</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// metoda ktora dodaje auto do tabeli
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>przekierowanie do akcji create</returns>
        [HttpPost]
        public IActionResult CreatePost(CreateCarDto dto)
        {
            _carService.Create(dto);

            return RedirectToAction("Create");
        }

        /// <summary>
        /// Szczegóły samochodu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var car = await _carService.GetById(id);

            return View(car);
        }

        /// <summary>
        /// Zwraca widok usuwania wraz z informacjami o wybranym aucie 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _carService.GetById(id);

            return View(car);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _carService.Delete(id);
            return RedirectToAction("GetCarsAdmin");
        }
        /// <summary>
        /// Zwraca widok edytowania wraz z informacjami o wybranym aucie 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _carService.GetById(id);

            return View(car);
        }
        [HttpPost]
        public async Task<IActionResult> EditPost(CarDto dto)
        {
            await _carService.Edit(dto);

            return RedirectToAction("GetCarsAdmin");
        }
    }
}
