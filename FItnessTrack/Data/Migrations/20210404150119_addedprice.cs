using Microsoft.EntityFrameworkCore.Migrations;

namespace FItnessTrack.Data.Migrations
{
    public partial class addedprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Carts");

            migrationBuilder.AddColumn<double>(
                name: "Charge",
                table: "Services",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Charge",
                table: "Carts",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Charge",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Charge",
                table: "Carts");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Services",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Carts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
