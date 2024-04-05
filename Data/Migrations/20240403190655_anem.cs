using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courier_Tracking_and_Delivery_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class anem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PackageStatuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PackageStatuses");
        }
    }
}
