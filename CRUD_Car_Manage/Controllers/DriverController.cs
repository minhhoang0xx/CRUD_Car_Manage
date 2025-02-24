using CRUD_Car_Manage.Data;
using CRUD_Car_Manage.Migrations;
using CRUD_Car_Manage.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
		[HttpGet("getAllDriver")]
		public ActionResult<IEnumerable<AddDriver>> GetAllDriver()
		{


			//var results = from d in _context.Drivers.AsQueryable()
			//			  group d by d.ID into g
			//			  select new AddDriver
			//			  {
			//				  DriverId = g.Key?.ToString(),
			//				  Cars = g.ToList()
			//			  };
			//var joinQuery =
			//from driver in _context.Drivers.AsQueryable()
			//join dc in _context.DriverCars.AsQueryable() on driver.ID equals dc.DriverId
			//join c in _context.Cars.AsQueryable() on dc.CarId equals c.ID
			//group driver by driver.ID into grDriver
			//select new 
			//{
			//	D_Thoi_Gian_Tao = grDriver.FirstOrDefault().D_Thoi_Gian_Tao,
			//	D_Trang_Thai = grDriver.FirstOrDefault().D_Trang_Thai,
			//	username = grDriver.FirstOrDefault().username,
			//	//CarName = grDriver.FirstOrDefault().Bien_So_Xe,
			//	ListCarId = grDriver.Select(x => x.DriverCars).ToList()
			//};


			var joinQuery =
			from driver in _context.Drivers.AsQueryable()
			join dc in _context.DriverCars.AsQueryable() on driver.ID equals dc.DriverId
			join c in _context.Cars.AsQueryable() on dc.CarId equals c.ID
			select new AddDriver
			{
				ID = driver.ID,
				D_Thoi_Gian_Tao = driver.D_Thoi_Gian_Tao,
				D_Trang_Thai = driver.D_Trang_Thai,
				username = driver.username,
				CarName= c.Bien_So_Xe
			};
			if (!joinQuery.Any())
			{
				return NotFound("No drivers found.");
			}

			return Ok(joinQuery);
		}
		[HttpGet("getDriver/{id}")]
		public ActionResult GetDriver(int id)
		{
			var driver = _context.Drivers           //Include & ThenInclude để nạp(lấy) thông tin
				.Include(d => d.DriverCars)         // lấy tất cả danh sách DriverCar(dc) liên quan với Driver(d)
				.ThenInclude(dc => dc.Car)          // lấy thông tin Car liên quan với DriverCar
				.FirstOrDefault(d => d.ID == id);   // lấy cái Driver đầu tien có id trùng

			if (driver == null)
			{
				return NotFound("Driver not found.");
			}

			var result = new AddDriver
			{
				ID = driver.ID,
				username = driver.username,
				D_Thoi_Gian_Tao = driver.D_Thoi_Gian_Tao,
				D_Trang_Thai = driver.D_Trang_Thai,
				ListCarId = driver.DriverCars.Select(dc => dc.CarId).ToList()
			};

			return Ok(result);
		}
		[HttpPost("addDriver")]
		public ActionResult AddDriver([FromBody] AddDriver aDriver)
		{
			if (aDriver.D_Trang_Thai == null || aDriver.ListCarId == null || aDriver.D_Thoi_Gian_Tao == null)
			{
				return BadRequest("Input Invalid!");
			}
			//them tai xe moi
			var newDriver = new Driver
			{
				username = aDriver.username,
				D_Thoi_Gian_Tao = aDriver.D_Thoi_Gian_Tao,
				D_Trang_Thai = aDriver.D_Trang_Thai,
			};
			_context.Drivers.Add(newDriver);
			_context.SaveChanges();

			if (aDriver.ListCarId == null)
			{
				aDriver.ListCarId = new List<int>();
			}
			// gan n` xe
			foreach (var carID in aDriver.ListCarId)
			{
				var car = _context.Cars.Find(carID);
				if (car == null)
				{
					return BadRequest($"CarID = {carID} khong exist");
				}
				// them luon vao bang DriverCar
				var driverCar = new DriverCar
				{
					DriverId = newDriver.ID,
					CarId = carID,
				};
				_context.DriverCars.Add(driverCar);
			}
			_context.SaveChanges();
			return Created("Driver added successfully.", null);
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
		public ActionResult UpdateDriver(int id, [FromBody] AddDriver driverDTO)
		{
			var updateDriver = _context.Drivers
				.Include(d => d.DriverCars)
				.FirstOrDefault(d => d.ID == id);

			if (updateDriver == null)
			{
				return BadRequest("Driver not found.");
			}
			updateDriver.username = driverDTO.username;
			updateDriver.D_Thoi_Gian_Tao = driverDTO.D_Thoi_Gian_Tao;
			updateDriver.D_Trang_Thai = driverDTO.D_Trang_Thai;
			// xoa list cu
			_context.DriverCars.RemoveRange(updateDriver.DriverCars);
			// them list moi
			if (driverDTO.ListCarId != null)
			{
				foreach (var carID in driverDTO.ListCarId)
				{
					var car = _context.Cars.Find(carID);
					if (car != null)
					{
						_context.DriverCars.Add(new DriverCar { DriverId = id, CarId = carID });
					}
				}
			}
			_context.SaveChanges();
			return Ok("Driver updated successfully!");
		}
	}
}
