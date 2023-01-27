using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository
    {
        Task<IEnumerable<Weather>> GetWeathers();
        Task<Weather> GetWeather(int weatherId);
        Task<Weather> AddWeather(Weather Weather);
        Task<Weather> UpdateWeather(Weather weather);
        Task<Weather> DeleteWeather(int weatherId);
        Task<IList<Weather>> SearchByDescription(string description);

    }
}
