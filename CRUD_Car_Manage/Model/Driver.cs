﻿using Newtonsoft.Json;

namespace CRUD_Car_Manage.Model
{
	public class Driver
	{
		public int ID { get; set; }
		public string username {  get; set; }
		public DateTime D_Thoi_Gian_Tao { get; set; }
		public string D_Trang_Thai {  get; set; }

		// Mqh n`- n`
		[JsonIgnore]
		public ICollection<DriverCar> DriverCars { get; set; }
	}
}
