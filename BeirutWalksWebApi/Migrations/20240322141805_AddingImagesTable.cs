using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeirutWalksWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddingImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Walks_Difficulty_DifficultyID",
                table: "Walks");

            migrationBuilder.DropForeignKey(
                name: "FK_Walks_Regions_RegionsId",
                table: "Walks");

            migrationBuilder.DropIndex(
                name: "IX_Walks_DifficultyID",
                table: "Walks");

            migrationBuilder.DropIndex(
                name: "IX_Walks_RegionsId",
                table: "Walks");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.CreateIndex(
                name: "IX_Walks_DifficultyID",
                table: "Walks",
                column: "DifficultyID");

            migrationBuilder.CreateIndex(
                name: "IX_Walks_RegionsId",
                table: "Walks",
                column: "RegionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Walks_Difficulty_DifficultyID",
                table: "Walks",
                column: "DifficultyID",
                principalTable: "Difficulty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Walks_Regions_RegionsId",
                table: "Walks",
                column: "RegionsId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
