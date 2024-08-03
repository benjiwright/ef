using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dometrain.EFCore.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeImdbRatingToInternetRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ImdbRating",
                table: "Films",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
            
            migrationBuilder.RenameColumn(
                name: "ImdbRating",
                table: "Films",
                newName: "InternetRating");

            // BAD AUTO GENERATED CODE
            // migrationBuilder.DropColumn(
            //     name: "ImdbRating",
            //     table: "Films");
            //
            // migrationBuilder.AddColumn<decimal>(
            //     name: "InternetRating",
            //     table: "Films",
            //     type: "decimal(18,2)",
            //     nullable: false,
            //     defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "InternetRating",
                table: "Films",
                type: "int",
                nullable: false,
                defaultValue: 0m);
            
            migrationBuilder.RenameColumn(
                name: "InternetRating",
                table: "Films",
                newName: "ImdbRating");
            
            
            // migrationBuilder.DropColumn(
            //     name: "InternetRating",
            //     table: "Films");
            //
            // migrationBuilder.AddColumn<int>(
            //     name: "ImdbRating",
            //     table: "Films",
            //     type: "int",
            //     nullable: false,
            //     defaultValue: 0);
        }
    }
}
