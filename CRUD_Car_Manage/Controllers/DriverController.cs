using CRUD_Car_Manage.Data;
using CRUD_Car_Manage.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Car_Manage.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DriverController : ControllerBase
	{
		public readonly CarContext _context;
		public DriverController(CarContext context)
		{
			_context = context;
		}
		[HttpGet("getAllcar")]
		public IActionResult GetAllCar(Car car)
		{
			var allCar = _context.Cars.ToList();
			return Ok(allCar);
		}
		[HttpGet("takecar")]
		public IActionResult TakeCar(int id)
		{
			var obj = _context.Cars.Find(id);
			if (obj == null) 
			{ 
				return NotFound(); 
			}
			return Ok(obj);

			
		}
	}
}
