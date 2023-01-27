using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace Weather_Presentation.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IRepository _repository;

        public WeatherController(IRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [Authorize(Policy ="AppUser")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _repository.GetWeathers();
            return Ok(result);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetWeather(int Id)
        {
            var weather = await _repository.GetWeather(Id);
            return Ok(weather);

        }
        [HttpPost]
        [Authorize(Policy ="AppUser")]
        public async Task<IActionResult> AddWeather(Weather weather)
        {
            var result = await _repository.AddWeather(weather);
            return Ok(result);
        }
        [HttpDelete("{Id}")]
        [Authorize(Policy = "Admin")]
        public  async Task<IActionResult>DeleteWeather(int Id)
        {
            var deletedWeather = await _repository.DeleteWeather(Id);
            return Ok(deletedWeather);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult>UpdateWeather(int Id, Weather weather)
        {
            if(Id != weather.Id)
            { return BadRequest("Invalid Id"); }
            var result = await _repository.UpdateWeather(weather);
            return Ok(result);

        }
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchByDescription(string description)
        { return Ok(await _repository.SearchByDescription(description)); }
    }
}
