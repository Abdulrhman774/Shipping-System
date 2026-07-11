using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class SetRelationBetweenTbShipmenttbUserSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "TbUserSubscription",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TbUserSubscription",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "UsedShipmentCount",
                table: "TbUserSubscription",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "UsedTotalDistance",
                table: "TbUserSubscription",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UsedTotalWeight",
                table: "TbUserSubscription",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbUserSender",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "TbUserSender",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbUserReceiver",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "TbUserReceiver",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationDays",
                table: "TbSubscriptionPackage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "TbSubscriptionPackage",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_TbUserSubscription_UserId",
                table: "TbUserSubscription",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserSender_ApplicationUserId",
                table: "TbUserSender",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserSender_UserId",
                table: "TbUserSender",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserReceiver_ApplicationUserId",
                table: "TbUserReceiver",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserReceiver_UserId",
                table: "TbUserReceiver",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShipment_UserSubscriptionId",
                table: "TbShipment",
                column: "UserSubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShipment_TbUserSubscription_UserSubscriptionId",
                table: "TbShipment",
                column: "UserSubscriptionId",
                principalTable: "TbUserSubscription",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserReceiver_AspNetUsers_ApplicationUserId",
                table: "TbUserReceiver",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserReceiver_AspNetUsers_UserId",
                table: "TbUserReceiver",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserSender_AspNetUsers_ApplicationUserId",
                table: "TbUserSender",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserSender_AspNetUsers_UserId",
                table: "TbUserSender",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbShipment_TbUserSubscription_UserSubscriptionId",
                table: "TbShipment");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserReceiver_AspNetUsers_ApplicationUserId",
                table: "TbUserReceiver");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserReceiver_AspNetUsers_UserId",
                table: "TbUserReceiver");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserSender_AspNetUsers_ApplicationUserId",
                table: "TbUserSender");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserSender_AspNetUsers_UserId",
                table: "TbUserSender");

            migrationBuilder.DropIndex(
                name: "IX_TbUserSubscription_UserId",
                table: "TbUserSubscription");

            migrationBuilder.DropIndex(
                name: "IX_TbUserSender_ApplicationUserId",
                table: "TbUserSender");

            migrationBuilder.DropIndex(
                name: "IX_TbUserSender_UserId",
                table: "TbUserSender");

            migrationBuilder.DropIndex(
                name: "IX_TbUserReceiver_ApplicationUserId",
                table: "TbUserReceiver");

            migrationBuilder.DropIndex(
                name: "IX_TbUserReceiver_UserId",
                table: "TbUserReceiver");

            migrationBuilder.DropIndex(
                name: "IX_TbShipment_UserSubscriptionId",
                table: "TbShipment");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "TbUserSubscription");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TbUserSubscription");

            migrationBuilder.DropColumn(
                name: "UsedShipmentCount",
                table: "TbUserSubscription");

            migrationBuilder.DropColumn(
                name: "UsedTotalDistance",
                table: "TbUserSubscription");

            migrationBuilder.DropColumn(
                name: "UsedTotalWeight",
                table: "TbUserSubscription");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "TbUserSender");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "TbUserReceiver");

            migrationBuilder.DropColumn(
                name: "DurationDays",
                table: "TbSubscriptionPackage");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TbSubscriptionPackage");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TbUserSender",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TbUserReceiver",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }
    }
}
