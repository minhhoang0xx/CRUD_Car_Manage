using Microsoft.EntityFrameworkCore;
using CRUD_Car_Manage.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CarContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("CarContext") ?? throw new InvalidOperationException("Connection' not found.")));

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: MyAllowSpecificOrigins,
		policy =>
		{
			policy.WithOrigins("http://localhost:3000") 
				  .AllowAnyHeader()
				  .AllowAnyMethod();
		});
});


var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Car}/{action=GetAllCar}/{id?}");

app.Run();
