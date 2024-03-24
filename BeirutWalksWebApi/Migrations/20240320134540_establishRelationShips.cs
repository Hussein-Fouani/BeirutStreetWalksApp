using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeirutWalksWebApi.Migrations
{
    /// <inheritdoc />
    public partial class establishRelationShips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
