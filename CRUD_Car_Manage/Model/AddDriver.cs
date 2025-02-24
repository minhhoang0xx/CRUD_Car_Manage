namespace CRUD_Car_Manage.Model
{
	public class AddDriver
	{
		public int ID { get; set; }
		public string username { get; set; }
		public DateTime D_Thoi_Gian_Tao { get; set; }
		public string D_Trang_Thai { get; set; }
		public string? CarName { get; set; }
		public List<int> ListCarId { get; set; }
	}
}
