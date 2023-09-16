using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeSizeColorTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shirts_Colors_ColorId",
                table: "Shirts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shirts_Sizes_SizeId",
                table: "Shirts");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Shirts_ColorId",
                table: "Shirts");

            migrationBuilder.DropIndex(
                name: "IX_Shirts_SizeId",
                table: "Shirts");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Shirts");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "Shirts");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Shirts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Shirts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Shirts");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Shirts");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "Shirts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "Shirts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shirts_ColorId",
                table: "Shirts",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Shirts_SizeId",
                table: "Shirts",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shirts_Colors_ColorId",
                table: "Shirts",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shirts_Sizes_SizeId",
                table: "Shirts",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
