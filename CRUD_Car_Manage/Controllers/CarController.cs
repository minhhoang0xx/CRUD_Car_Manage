using CRUD_Car_Manage.Data;
using CRUD_Car_Manage.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace CRUD_Car_Manage.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CarController : ControllerBase
	{
		public readonly CarContext _context;
		public CarController( CarContext context)
		{
			_context = context;

		}
		[HttpGet("getAllCar")]
		public ActionResult<List<Car>> GetAllCars()// dùng ActionResult sẽ giúp cho biết trả về kết quả rõ ràng hơn 200, 404, 400
		{
			return _context.Cars.ToList();
		}
		[HttpGet("getCar/{id}")]        // có thể thực hiện ngay như này không phải lặp lại công việc route
		public ActionResult<Car> GetCar(int id)
		{
			var car =_context.Cars.Find(id);
			if (car == null) 
			{
				return NotFound(); 
			}
			else
			{
				return car;
			}
		
		}
		[HttpPost("addCart")]
		public ActionResult<Car> addCar([FromBody]Car car) 
		{
			if (car == null)
			{
				return NotFound();
			}
			_context.Cars.Add(car);
			_context.SaveChanges();
			return CreatedAtAction(nameof(GetCar), new { id = car.ID }, car);
			// dùng để có thể trả lại url xe mới tạo thông qua việc gán id vào url
		}


	}
}
