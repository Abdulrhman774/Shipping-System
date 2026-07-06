using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class MakeTrackingNumAsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TbShipment_TrackingNumber",
                table: "TbShipment",
                column: "TrackingNumber",
                unique: true,
                filter: "[TrackingNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbShipment_TrackingNumber",
                table: "TbShipment");
        }
    }
}
