using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace birthday.Controllers
{
    [ApiController]
    [Route("/aniversario")]
    public class BirthdayController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<BirthdayController> _logger;

        public BirthdayController(ILogger<BirthdayController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Retorna o aniversário do Plebeu Alef
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBirthday()
        {
            var response = await CalculateBirthday();

            return Ok(response);
        }

        private async Task<string> CalculateBirthday()
        {
            var monthsToAlefBirthday = 0;
            var daysToAlefBirthday = 0;

            var alefBirthday = Convert.ToDateTime("10/11/1993");
            var today = Convert.ToDateTime("02/10/1993"); //DateTime.Now;

            if (today.Day > 10)
            {
                monthsToAlefBirthday = alefBirthday.Month - today.Month - 1;
                var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
                var restDays = daysInMonth - today.Day;
                daysToAlefBirthday = alefBirthday.Day + restDays;
            }
            else
            {
                monthsToAlefBirthday = alefBirthday.Month - today.Month;
                daysToAlefBirthday = alefBirthday.Day - today.Day;
            }

            var message = "";
            switch (monthsToAlefBirthday)
            {
                case 1:
                    switch (daysToAlefBirthday)
                    {
                        case 1:
                            message = $"FALTAM {monthsToAlefBirthday} MÊS E {daysToAlefBirthday} DIAS PARA O ANIVERÁRIO DO PLEBEU ALEF";
                            break;
                    }

                    message = $"FALTAM {monthsToAlefBirthday} MÊS E {daysToAlefBirthday} DIAS PARA O ANIVERÁRIO DO PLEBEU ALEF";
                    break;
                case > 1:
                    switch (daysToAlefBirthday)
                    {
                        case 1:
                            message = $"FALTAM {monthsToAlefBirthday} MÊS E {daysToAlefBirthday} DIAS PARA O ANIVERÁRIO DO PLEBEU ALEF";
                            break;
                    }
                    message = $"FALTAM {monthsToAlefBirthday} MESES E {daysToAlefBirthday} DIAS PARA O ANIVERÁRIO DO PLEBEU ALEF";
                    break;
                default:
                    switch (daysToAlefBirthday)
                    {
                        case 1:
                            message = $"FALTAM {daysToAlefBirthday} DIA PARA O ANIVERÁRIO DO PLEBEU ALEF";
                            break;
                    }
                    message = $"FALTAM {daysToAlefBirthday} DIAS PARA O ANIVERÁRIO DO PLEBEU ALEF";
                    break;
            }

            return message;
        }
    }
}
