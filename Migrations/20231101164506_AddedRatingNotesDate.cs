using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrewBuddy.Migrations
{
    /// <inheritdoc />
    public partial class AddedRatingNotesDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beer_ToBeerStyles",
                table: "Beers");

            migrationBuilder.DropForeignKey(
                name: "FK_Beer_ToBreweries",
                table: "Beers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Breweries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(25)",
                oldFixedLength: true,
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BeerStyles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(12)",
                oldFixedLength: true,
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<int>(
                name: "StyleId",
                table: "Beers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BreweryId",
                table: "Beers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Beer_ToBeerStyle",
                table: "Beers",
                column: "StyleId",
                principalTable: "BeerStyles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Beer_ToBrewery",
                table: "Beers",
                column: "BreweryId",
                principalTable: "Breweries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beer_ToBeerStyle",
                table: "Beers");

            migrationBuilder.DropForeignKey(
                name: "FK_Beer_ToBrewery",
                table: "Beers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Breweries",
                type: "nchar(25)",
                fixedLength: true,
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BeerStyles",
                type: "nchar(12)",
                fixedLength: true,
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "StyleId",
                table: "Beers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BreweryId",
                table: "Beers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Beer_ToBeerStyle",
                table: "Beers",
                column: "StyleId",
                principalTable: "BeerStyles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Beer_ToBrewery",
                table: "Beers",
                column: "BreweryId",
                principalTable: "Breweries",
                principalColumn: "Id");
        }
    }
}
