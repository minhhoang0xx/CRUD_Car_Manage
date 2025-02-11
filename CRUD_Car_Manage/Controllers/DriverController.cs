using CRUD_Car_Manage.Data;
using CRUD_Car_Manage.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
		//[HttpGet("getAllDriver")]
		//public ActionResult GetAllCar(Driver driver)
		//{
		//	var allDriver = _context.Drivers.ToList();
		//	return Ok(allDriver);
		//}
		[HttpGet("getDriver/{id}")]
		public ActionResult GetDriver(int id)
		{
			var obj = _context.Drivers						   //Include & ThenInclude để nạp(lấy) thông tin
									.Include(d=>d.DriverCars)  // lấy tất cả danh sách DriverCar(dc) liên quan với Driver(d)
									.ThenInclude(dc => dc.Car) // lấy thông tin Car liên quan với DriverCar
									.FirstOrDefault(d =>  d.ID == id); // lấy cái Driver đầu tien có id trùng
			


			if (obj == null) 
			{ 
				return NotFound(); 
			}
			return Ok(obj);	
		}
		[HttpPost("addDriver")]
		public ActionResult AddDriver([FromBody]DriverCar dc)
		{
			if (dc == null)
			{
				return BadRequest("Input Invalid!");
			}
			var driver = _context.Drivers.Find(dc.DriverId);
			var car = _context.Cars.Find(dc.CarId);
			if (driver == null || car == null)
			{
				return BadRequest("not Exist");
			}
			_context.DriverCars.Add(dc);
			_context.SaveChanges();
			return CreatedAtAction(nameof(AddDriver),new { id = dc.DriverId }, dc);
		}
		[HttpPost("deleteDriver")]
		public ActionResult DeleteDriver(int id)
		{
			var deleteDriver = _context.Drivers
											.Include(d => d.DriverCars)
											.ThenInclude(dc => dc.Car)
											.FirstOrDefault(d => d.ID == id);

			if (deleteDriver == null)
			{
				return BadRequest("Input Invalid!");
			}
			// Delete all danh sách trong bảng DriverCar liên quan đến driver này
			_context.DriverCars.RemoveRange(deleteDriver.DriverCars); // Remove xóa 1, RemoveRange xóa nhiều
			// Sau khi delete mqh, delete khỏi driver
			_context.Drivers.Remove(deleteDriver);

			_context.SaveChanges();
			return Ok("Delete Success!!!");
		}
		[HttpPut("updateDriver")]
		public ActionResult UpdateDriver(int id, [FromBody] Driver driver)
		{
			var updateDriver = _context.Drivers.Find(id);
			if(updateDriver == null)
			{
				return BadRequest("No Driver exist");
			}
			updateDriver.username = driver.username;
			updateDriver.carId = driver.carId;
			updateDriver.D_Thoi_Gian_Tao = driver.D_Thoi_Gian_Tao;
			updateDriver.D_Trang_Thai = driver.D_Trang_Thai;
			_context.Drivers.Update(updateDriver);
			_context.SaveChanges();
			return Ok(updateDriver);
		}
	}
}
