namespace CRUD_Car_Manage.Model
{
	public class AddDriver
	{
		public string username { get; set; }
		public DateTime D_Thoi_Gian_Tao { get; set; }
		public string D_Trang_Thai { get; set; }
		public List<int> ListCarId { get; set; }
	}
}
