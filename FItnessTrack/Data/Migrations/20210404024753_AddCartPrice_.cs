using Microsoft.EntityFrameworkCore.Migrations;

namespace FItnessTrack.Data.Migrations
{
    public partial class AddCartPrice_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_TrainingId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_TrainingId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "TrainingId",
                table: "Carts");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Services",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Carts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Carts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ServiceId",
                table: "Carts",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_TrainingId",
                table: "Carts",
                column: "ServiceId",
                principalTable: "Trainings",
                principalColumn: "TrainingId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_TrainingId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_ServiceId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Carts");

            migrationBuilder.AddColumn<int>(
                name: "TrainingId",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_TrainingId",
                table: "Carts",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_TrainingId",
                table: "Carts",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "TrainingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
