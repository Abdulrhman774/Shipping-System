using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddShipmentStatusAndHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbShipmentStatus_TbShipment_ShipmentId",
                table: "TbShipmentStatus");

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "TbShipment",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusLastUpdatedAt",
                table: "TbShipment",
                type: "datetime",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TbShipmentStatusHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ShipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShipmentStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShipmentStatusHistory_TbShipment_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "TbShipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbShipmentStatusHistory_ShipmentId",
                table: "TbShipmentStatusHistory",
                column: "ShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShipmentStatus_TbShipment_ShipmentId",
                table: "TbShipmentStatus",
                column: "ShipmentId",
                principalTable: "TbShipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbShipmentStatus_TbShipment_ShipmentId",
                table: "TbShipmentStatus");

            migrationBuilder.DropTable(
                name: "TbShipmentStatusHistory");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TbShipment");

            migrationBuilder.DropColumn(
                name: "StatusLastUpdatedAt",
                table: "TbShipment");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShipmentStatus_TbShipment_ShipmentId",
                table: "TbShipmentStatus",
                column: "ShipmentId",
                principalTable: "TbShipment",
                principalColumn: "Id");
        }
    }
}
