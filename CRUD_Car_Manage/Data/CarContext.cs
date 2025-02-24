using CRUD_Car_Manage.Model;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Car_Manage.Data
{
	public class CarContext : DbContext
	{
		public CarContext(DbContextOptions<CarContext> options) : base(options) { }
		public DbSet<Car> Cars { get; set; }
		public DbSet<Driver> Drivers { get; set; }

		public DbSet<DriverCar> DriverCars { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		// OnModelCreating la noi cau hinh cac mqh
		// modelBuilder cung cap mot API dung de cau hinh

		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<DriverCar>()
				.HasKey(dc => new { dc.DriverId, dc.CarId }); // cap khoa chinh cua DriverCar ket hop cua DriverId va CarId

			modelBuilder.Entity<DriverCar>()
				.HasOne(dc => dc.Driver)
				.WithMany(d => d.DriverCars)
				.HasForeignKey(dc => dc.DriverId) // DriverId trong DriverCar la khoa ngoai. tham chieu den Model Driver
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<DriverCar>()
				.HasOne(dc => dc.Car)
				.WithMany(c => c.DriverCars)
				.HasForeignKey(dc => dc.CarId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}

