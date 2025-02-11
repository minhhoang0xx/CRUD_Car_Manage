namespace CRUD_Car_Manage.Model
{
	public class DriverCar
	{
		public int DriverId { get; set; }
		public Driver Driver { get; set; }

		public int CarId { get; set; }
		public Car Car { get; set; }
	}
}
