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
		[HttpPost("addCar")]
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

		[HttpDelete("deletaCar/{id}")]
		public ActionResult DeleteCar(int id) 
		{
			if(_context.Cars.Find(id) == null)
			{
				return NotFound(); // 404
			}
			_context.Cars.Remove(_context.Cars.Find(id));
			_context.SaveChanges();
			return Ok("delete successfully!");
		}
		[HttpPut("updateCar/{id}")]
		public ActionResult UpdateCar(int id, [FromBody]Car car)
		{
			var obj = _context.Cars.Find(id);
			if (obj == null) 
			{
				return NotFound();
			}
			obj.Bien_So_Xe = car.Bien_So_Xe;
			obj.Loai_Xe = car.Loai_Xe;
			obj.Ngay_Tao = car.Ngay_Tao;
			obj.Trang_Thai = car.Trang_Thai;

			_context.Cars.Update(obj);
			_context.SaveChanges();
			return Ok(obj);
		}


	}
}
