using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopAPI.Migrations
{
    public partial class checkout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CheckoutId",
                table: "Carts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Checkouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payment = table.Column<float>(type: "real", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Checkouts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CheckoutId",
                table: "Carts",
                column: "CheckoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_CustomerId",
                table: "Checkouts",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Checkouts_CheckoutId",
                table: "Carts",
                column: "CheckoutId",
                principalTable: "Checkouts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Checkouts_CheckoutId",
                table: "Carts");

            migrationBuilder.DropTable(
                name: "Checkouts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CheckoutId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CheckoutId",
                table: "Carts");
        }
    }
}
