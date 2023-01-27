using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Weather
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string  TemperatureC { get; set; }
        public DateTime Date { get; set; }

        
    }
}
