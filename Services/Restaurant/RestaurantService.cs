using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantRaterMVC.Data;
using RestaurantRaterMVC.Models.Restaurant;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterMVC.Models;

namespace RestaurantRaterMVC.Services.Restaurant
{
    public class RestaurantService : IRestaurantService
    {
        private RestaurantDbContext _context;
        public RestaurantService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateRestaurantAsync(RestaurantCreate model)
        {
            RestaurantEntity restaurant = new RestaurantEntity()
            {
                Name = model.Name,
                Location = model.Location,
            };

            _context.Restaurants.Add(restaurant);

            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<bool> DeleteRestaurant(int id)
        {
            RestaurantEntity restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return false;

            _context.Restaurants.Remove(restaurant);

            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<List<RestaurantListItem>> GetAllRestaurantsAsync()
        {
            List<RestaurantListItem> restaurants = await _context.Restaurants
            .Include(r => r.Ratings)
            .Select(r => new RestaurantListItem()
            {
                Id = r.Id,
                Name = r.Name,
                Score = r.Score
            }).ToListAsync();
            return restaurants;
        }

        public async Task<RestaurantDetail> GetRestaurantByIdAsync(int id)
        {
            RestaurantEntity restaurant = await _context.Restaurants
                .Include(r => r.Ratings)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (restaurant == null) return null;

            RestaurantDetail restaurantDetail = new RestaurantDetail()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Location = restaurant.Location,
                AverageFoodScore = restaurant.AverageFoodScore,
                AverageCleanlinessScore = restaurant.AverageCleanlinessScore,
                AverageAtmosphereScore = restaurant.AverageAtmosphereScore,
                Score = restaurant.Score,
            };

            return restaurantDetail;
        }

        public async Task<bool> UpdateRestaurantAsync(RestaurantEdit model)
        {
            RestaurantEntity restaurant = await _context.Restaurants.FindAsync(model.Id);
                if (restaurant == null) return false;

            restaurant.Location = model.Location;
            restaurant.Name = model.Name;

            return await _context.SaveChangesAsync() == 1;
        }
    }
}