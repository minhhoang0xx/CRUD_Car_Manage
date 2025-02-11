using CRUD_Car_Manage.Data;
using CRUD_Car_Manage.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;


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
			var cars = new List<Car>();
			using (var Connection = _context.Database.GetDbConnection())
			{
				Connection.Open();
				using (var command = Connection.CreateCommand())
				{
					command.CommandText = "GetAllCar";// kết nối với stored procedure ở SQL Server
					command.CommandType = CommandType.StoredProcedure;
					using (var reader = command.ExecuteReader()) // chỉ dùng để đọc dữ liệu, không hỗ trợ tương tác
					{
						while (reader.Read()) // duyệt từng dòng dữ liệu
						{
							var car = new Car
							{
								ID = reader.GetInt32(reader.GetOrdinal("ID")),
								Bien_So_Xe = reader.GetString(reader.GetOrdinal("Bien_So_Xe")),
								Loai_Xe = reader.GetString(reader.GetOrdinal("Loai_Xe")),
								Ngay_Tao = reader.GetDateTime(reader.GetOrdinal("Ngay_Tao")),
								Trang_Thai = reader.GetString(reader.GetOrdinal("Trang_Thai"))
							};
							cars.Add(car);
						}
					}
				}
			}
			return Ok(cars);
		}
		[HttpGet("getCar/{id}")]        // có thể thực hiện ngay như này không phải lặp lại công việc route
		public ActionResult<Car> GetCar(int id)
		{
			Car car = null;

			using (var connection = _context.Database.GetDbConnection())
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "GetCar";
					command.CommandType = CommandType.StoredProcedure;

					var idParameter = new SqlParameter("@id", id); // sqlParameter dùng để truyền giá trị vào câu lệnh SQL
																   // đảm bảo dữ liệu được xử lý đùnsg kiểu khi truyền vào
					command.Parameters.Add(idParameter);

					using (var reader = command.ExecuteReader())
					{ 
						if (reader.Read())
						{
							car = new Car
							{
								ID = reader.GetInt32(reader.GetOrdinal("ID")),
								Bien_So_Xe = reader.GetString(reader.GetOrdinal("Bien_So_Xe")),
								Loai_Xe = reader.GetString(reader.GetOrdinal("Loai_Xe")),
								Ngay_Tao = reader.GetDateTime(reader.GetOrdinal("Ngay_Tao")),
								Trang_Thai = reader.GetString(reader.GetOrdinal("Trang_Thai"))
							};
						}
					}
				}

			}
			if (car == null) return NotFound();
			return Ok(car);
		}
		[HttpPost("addCar")]
		//public ActionResult<Car> addCar([FromBody]Car car) 
		//{
		//	if (car == null)
		//	{
		//		return NotFound();
		//	}
		//	_context.Cars.Add(car);
		//	_context.SaveChanges();
		//	return CreatedAtAction(nameof(GetCar), new { id = car.ID }, car);
		//	// dùng để có thể trả lại url xe mới tạo thông qua việc gán id vào url
		//}
		public ActionResult<Car> addCar([FromBody]Car car) // Frombody khi dữ liệu đến từ request body (JSON/XML)
														   // FromQuerry khi dữ liệu từ URL(String)
														   // FromRoute -- route
														   // FromHeader -- HTTP Header
		{
			if(car == null)
			{
				return BadRequest("input is not validated");
			}
			using (var connection = _context.Database.GetDbConnection())
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "AddCar";
					command.CommandType = CommandType.StoredProcedure;
					// Add thông qua Stored Procedure
					command.Parameters.Add(new SqlParameter("@Bien_So_Xe", car.Bien_So_Xe));
					command.Parameters.Add(new SqlParameter("@Loai_Xe", car.Loai_Xe));
					command.Parameters.Add(new SqlParameter("@Ngay_Tao", car.Ngay_Tao));
					command.Parameters.Add(new SqlParameter("@Trang_Thai", car.Trang_Thai));

					command.ExecuteNonQuery(); // thuc hien stored procedure
				}
			}
			return CreatedAtAction(nameof(GetCar), new {id = car.ID},car );
			// dùng để có thể trả lại url xe mới tạo thông qua việc gán id vào url
		}

		[HttpDelete("deletaCar/{id}")]
		//public ActionResult DeleteCar(int id) 
		//{
		//	if(_context.Cars.Find(id) == null)
		//	{
		//		return NotFound(); // 404
		//	}
		//	_context.Cars.Remove(_context.Cars.Find(id));
		//	_context.SaveChanges();
		//	return Ok("delete successfully!");
		//}

		public ActionResult DeleteCar(int id)
		{
			if (_context.Cars.Find(id) == null)
			{
				return NotFound();
			}
			using (var connection  = _context.Database.GetDbConnection())
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "DeleteCar";
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.Add(new SqlParameter("@id",id));
					command.ExecuteNonQuery ();
				}
			}
			return Ok("Delete Successfully!");
		}
		[HttpPut("updateCar/{id}")]
		//public ActionResult UpdateCar(int id, [FromBody]Car car)
		//{
		//	var obj = _context.Cars.Find(id);
		//	if (obj == null) 
		//	{
		//		return NotFound();
		//	}
		//	obj.Bien_So_Xe = car.Bien_So_Xe;
		//	obj.Loai_Xe = car.Loai_Xe;
		//	obj.Ngay_Tao = car.Ngay_Tao;
		//	obj.Trang_Thai = car.Trang_Thai;

		//	_context.Cars.Update(obj);
		//	_context.SaveChanges();
		//	return Ok(obj);
		//}

		public ActionResult UpdateCar(int id, [FromBody] Car car)
		{
			if (car == null) return BadRequest("Car is not existing");
			using (var connection = _context.Database.GetDbConnection())
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "UpdateCar";
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.Add(new SqlParameter("@id", id));
					command.Parameters.Add(new SqlParameter("@Bien_So_Xe", car.Bien_So_Xe));
					command.Parameters.Add(new SqlParameter("@Loai_Xe", car.Loai_Xe));
					command.Parameters.Add(new SqlParameter("@Ngay_Tao", car.Ngay_Tao));
					command.Parameters.Add(new SqlParameter("@Trang_Thai", car.Trang_Thai));

					command.ExecuteNonQuery();
				}
			}
			return Ok("Update Successfully");
		} 







	}
}
