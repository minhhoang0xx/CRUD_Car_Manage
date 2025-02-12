using CRUD_Car_Manage.Data;
using CRUD_Car_Manage.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Driver = CRUD_Car_Manage.Model.Driver;

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
			// kiem tra xe co ton tai khong
			var car = _context.Cars.Find(dc.CarId);
			if ( car == null)
			{
				return BadRequest("not Exist");
			}
			//them tai xe moi
			var newDriver = new Driver
			{
				username = dc.Driver.username,
				D_Thoi_Gian_Tao = dc.Driver.D_Thoi_Gian_Tao,
				D_Trang_Thai = dc.Driver.D_Trang_Thai
			};
			_context.Drivers.Add(newDriver);
			_context.SaveChanges();
			// them luon vao bang DriverCar
			var driverCar = new DriverCar
			{
				DriverId = newDriver.ID,
				CarId = dc.CarId
			};
			_context.DriverCars.Add(driverCar);
			_context.SaveChanges();

			return CreatedAtAction(nameof(GetDriver), new { id = newDriver.ID }, newDriver);
		}
		[HttpDelete("deleteDriver/{id}")]
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
		[HttpPut("updateDriver/{id}")]
		public ActionResult UpdateDriver(int id, [FromBody] Driver driver)
		{
			var updateDriver = _context.Drivers.Find(id);
			if (updateDriver == null)
			{
				return BadRequest("No Driver exist");
			}
			updateDriver.username = driver.username;
			updateDriver.D_Thoi_Gian_Tao = driver.D_Thoi_Gian_Tao;
			updateDriver.D_Trang_Thai = driver.D_Trang_Thai;
			_context.Drivers.Update(updateDriver);
			_context.SaveChanges();
			return Ok(updateDriver);
		}
		[HttpPut("updateCarOfDriver/{id}")]
		public ActionResult updateCarOfDriver(int id, [FromBody] List<int> carIDs)
		{
			var driver = _context.Drivers
											.Include(d => d.DriverCars)
											.FirstOrDefault(d => d.ID == id);
			if (driver == null)
			{
				return BadRequest("No Driver exist");
			}
			// xoa het danh sach xe
			_context.DriverCars.RemoveRange(driver.DriverCars);
			// Thêm danh sách xe mới
			foreach (var carID in carIDs)
			{
				var car = _context.Cars.Find(carID);
				if (car != null)
				{
					_context.DriverCars.Add(new DriverCar { DriverId = id, CarId = carID });
				}
			}

			_context.SaveChanges();
			return Ok("Driver's cars updated!!!");

		}
	}
}
