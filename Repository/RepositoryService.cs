using Domain.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryService : IRepository
    {
        private readonly ApplicationDbContext _context;

        public RepositoryService(ApplicationDbContext context )
        {
            _context = context;
        }

        public async Task<Weather> AddWeather(Weather weather)
        {
            var result = await _context.Weathers.AddAsync(weather);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Weather> DeleteWeather(int weatherId)
        {
            var result =  await _context.Weathers.FirstOrDefaultAsync(e => e.Id == weatherId);
            if (result != null)
            {  
                _context.Weathers.Remove(result);
                await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<Weather> GetWeather(int weatherId)
        { 
            var result = await _context.Weathers
                .FirstOrDefaultAsync(e => e.Id == weatherId);
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }

        }

        public async Task<IEnumerable<Weather>> GetWeathers()
        {
            return await _context.Weathers.ToListAsync();
        }

        public async Task<Weather> UpdateWeather(Weather weather)
        {
            var result = await _context.Weathers
                .FirstOrDefaultAsync(e => e.Id == weather.Id);

            if (result != null)
            {
               result.Description = weather.Description;
               result.TemperatureC= weather.TemperatureC;
                await _context.SaveChangesAsync();

                return result;
            }

            return null;

        }
        public async Task<IList<Weather>>SearchByDescription(string description)
        {
            var searchText = await _context.Weathers.Where(e => e.Description.ToLower()
            .Contains(description.ToLower())).ToListAsync();
            return searchText;
        }
    }
}
