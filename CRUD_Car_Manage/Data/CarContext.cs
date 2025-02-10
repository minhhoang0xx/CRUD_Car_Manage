using CRUD_Car_Manage.Model;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Car_Manage.Data
{
	public class CarContext : DbContext
	{
		public CarContext(DbContextOptions<CarContext> options) : base(options) { }
		public DbSet<Car> Cars { get; set; }
	}
}
