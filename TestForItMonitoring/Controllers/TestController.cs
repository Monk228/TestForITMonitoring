using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;
using TestForItMonitoring.Context;
using TestForItMonitoring.DatabaseModels;
using TestForItMonitoring.Models;

namespace TestForItMonitoring.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> TestMethod(TestModel model)
        {
            int first, second, result;

            if (!Int32.TryParse(model.First, out first) || model.First.Length > 1000)      
                return BadRequest("Первое число некорректно введено, либо больше 1000 символов");
            
            if (!Int32.TryParse(model.Second, out second) || model.First.Length > 1000)
                return BadRequest("Второе число некорректно введено, либо больше 1000 символов");
            
            result = first + second;

            try
            {
                using (DataContext db = new DataContext())
                {
                    db.Requests.Add(new RequestModel()
                    {
                        ClientIp = Request.Host.Value,
                        FirstNumber = first,
                        SecondNumber = second,
                        Result = result,
                    });
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return new JsonResult(JsonSerializer.Serialize(result));
        }
    }
}
