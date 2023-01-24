using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentCars.Services;

namespace RentCars.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly UserManager<IdentityUser> _userManager;

        public ReservationController(IReservationService reservationService, UserManager<IdentityUser> userManager)
        {
            _reservationService = reservationService;
            _userManager = userManager;
        }

        private async Task<IdentityUser> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return user;
        }


        [HttpGet]
        public async Task<IActionResult> Reseravations()
        {
            var currentUser = await GetCurrentUser();

            var userReservations = await _reservationService.GetReservations(currentUser.Id);


            return View(userReservations);
        }
    }
}
