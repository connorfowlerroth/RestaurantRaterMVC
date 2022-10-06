using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantRaterMVC.Data;
using RestaurantRaterMVC.Models.Rating;
using Microsoft.EntityFrameworkCore;

namespace RestaurantRaterMVC.Services.Rating
{
    public class RatingService : IRatingService
    {
        private readonly RestaurantDbContext _context;

        public RatingService(RestaurantDbContext context)
        {
            _context = context;
        }

        public Task<bool> DeleteRating(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RatingListItem>> GetAllRatings()
        {
            var ratings = _context.Ratings
                .Select(r => new RatingListItem()
                {
                    Id = r.Id,
                    RestaurantName = r.Restaurant.Name,
                    FoodScore = r.FoodScore,
                    AtmosphereScore = r.AtmosphereScore,
                    CleanlinessScore = r.CleanlinessScore,
                });

            return await ratings.ToListAsync();
        }

        public async Task<List<RatingListItem>> GetRatingsForRestaurant(int id)
        {
            var ratings = _context.Ratings
                .Where(r => r.RestaurantId == id)
                .Select(r => new RatingListItem()
                {
                    Id = r.Id,
                    RestaurantName = r.Restaurant.Name,
                    FoodScore = r.FoodScore,
                    CleanlinessScore = r.CleanlinessScore,
                    AtmosphereScore = r.AtmosphereScore,
                    Score = r.FoodScore,
                });

            return await ratings.ToListAsync();
        }

        public async Task<bool> RateRestaurant(RatingCreate model)
        {
            var rating = new RatingEntity()
            {
                FoodScore = model.FoodScore,
                CleanlinessScore = model.CleanlinessScore,
                AtmosphereScore = model.AtmosphereScore,
                RestaurantId = model.RestaurantId,
            };

            _context.Ratings.Add(rating);

            return await _context.SaveChangesAsync() == 1;
        }
    }
}