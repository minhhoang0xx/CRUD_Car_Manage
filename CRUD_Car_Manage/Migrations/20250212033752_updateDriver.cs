using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_Car_Manage.Migrations
{
    /// <inheritdoc />
    public partial class updateDriver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "carId",
                table: "Drivers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "carId",
                table: "Drivers",
                type: "int",
                nullable: true);
        }
    }
}
