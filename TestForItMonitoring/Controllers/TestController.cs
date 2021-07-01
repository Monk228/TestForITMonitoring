using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using TestForItMonitoring.Context;
using TestForItMonitoring.DatabaseModels;
using TestForItMonitoring.Helpers;
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
            if (!Validation.IsNumber(model.First) || model.First.Length > 1000)
            {
                return BadRequest("Первое число некорректно введено, либо больше 1000 символов");
            }
            if (!Validation.IsNumber(model.Second) || model.Second.Length > 1000)
            {
                return BadRequest("Второе число некорректно введено, либо больше 1000 символов");
            }

            BigNumber _first = new BigNumber(model.First);
            BigNumber _second = new BigNumber(model.Second);

            BigNumber _result = new BigNumber("");

            if (_first.Sign != _second.Sign)
            {
                _result = BigNumber.Substract(_first, _second);

            }
            else
            {
                _result = BigNumber.Add(_first, _second);
            }
           
            try
            {
                using (DataContext db = new DataContext())
                {
                    db.Requests.Add(new RequestModel()
                    {
                        ClientIp = Request.Host.Value,
                        FirstNumber = _first.ToString(),
                        SecondNumber = _second.ToString(),
                        Result = _result.ToString(),
                    });
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
            return new JsonResult(JsonSerializer.Serialize(_result.ToString()));
        }
    }
}
