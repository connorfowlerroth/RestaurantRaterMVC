using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestaurantRaterMVC.Models.Rating;
using RestaurantRaterMVC.Services.Rating;

namespace RestaurantRaterMVC.Controllers
{
    [Route("[controller]")]
    public class RatingController : Controller
    {
        private readonly ILogger<RatingController> _logger;
        private readonly IRatingService _service;

        public RatingController(ILogger<RatingController> logger, IRatingService service)
        {
            _logger = logger;
            _service = service;
        }

        public  async Task<IActionResult> Index()
        {
            var ratings = await _service.GetAllRatings();
            return View(ratings);
        }

        public async Task<IActionResult> Restaurant(int id)
        {
            var ratings = await _service.GetRatingsForRestaurant(id);

            return View(ratings);
        }

        public async Task<IActionResult> RateRestaurant(int id)
        {
            RatingCreate ratingCreate = new RatingCreate()
            {
                RestaurantId = id,
            };

            return View(ratingCreate);
        }

        [HttpPost]
        public async Task<IActionResult> RateRestaurant(RatingCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            bool isRated = await _service.RateRestaurant(model);

            if (!isRated) return View(model);
            return RedirectToAction(nameof(Restaurant), new { id = model.RestaurantId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}