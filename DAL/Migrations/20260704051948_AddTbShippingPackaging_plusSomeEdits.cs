using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTbShippingPackaging_plusSomeEdits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbCities_TbCountries",
                table: "TbCities");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserReceivers_TbCities",
                table: "TbUserReceivers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserSenders_TbCities",
                table: "TbUserSenders");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserSubscriptions_TbSubscriptionPackages",
                table: "TbUserSubscriptions");

            migrationBuilder.DropTable(
                name: "TbShippmentStatus");

            migrationBuilder.DropTable(
                name: "TbShippments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbUserSubscriptions",
                table: "TbUserSubscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbUserSenders",
                table: "TbUserSenders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbUserReceivers",
                table: "TbUserReceivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbSubscriptionPackages",
                table: "TbSubscriptionPackages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbShippingTypes",
                table: "TbShippingTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbPaymentMethods",
                table: "TbPaymentMethods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbCountries",
                table: "TbCountries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbCities",
                table: "TbCities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbCarriers",
                table: "TbCarriers");

            migrationBuilder.RenameTable(
                name: "TbUserSubscriptions",
                newName: "TbUserSubscription");

            migrationBuilder.RenameTable(
                name: "TbUserSenders",
                newName: "TbUserSender");

            migrationBuilder.RenameTable(
                name: "TbUserReceivers",
                newName: "TbUserReceiver");

            migrationBuilder.RenameTable(
                name: "TbSubscriptionPackages",
                newName: "TbSubscriptionPackage");

            migrationBuilder.RenameTable(
                name: "TbShippingTypes",
                newName: "TbShippingType");

            migrationBuilder.RenameTable(
                name: "TbPaymentMethods",
                newName: "TbPaymentMethod");

            migrationBuilder.RenameTable(
                name: "TbCountries",
                newName: "TbCountry");

            migrationBuilder.RenameTable(
                name: "TbCities",
                newName: "TbCity");

            migrationBuilder.RenameTable(
                name: "TbCarriers",
                newName: "TbCarrier");

            migrationBuilder.RenameIndex(
                name: "IX_TbUserSubscriptions_PackageId",
                table: "TbUserSubscription",
                newName: "IX_TbUserSubscription_PackageId");

            migrationBuilder.RenameIndex(
                name: "IX_TbUserSenders_CityId",
                table: "TbUserSender",
                newName: "IX_TbUserSender_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_TbUserReceivers_CityId",
                table: "TbUserReceiver",
                newName: "IX_TbUserReceiver_CityId");

            migrationBuilder.RenameColumn(
                name: "ShippimentCount",
                table: "TbSubscriptionPackage",
                newName: "ShipmentCount");

            migrationBuilder.RenameIndex(
                name: "IX_TbCities_CountryId",
                table: "TbCity",
                newName: "IX_TbCity_CountryId");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "TbUserSender",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultAddress",
                table: "TbUserSender",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtherAddress",
                table: "TbUserSender",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "TbUserSender",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "TbUserReceiver",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultAddress",
                table: "TbUserReceiver",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtherAddress",
                table: "TbUserReceiver",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "TbUserReceiver",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CityEName",
                table: "TbCity",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CityAName",
                table: "TbCity",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbUserSubscription",
                table: "TbUserSubscription",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbUserSender",
                table: "TbUserSender",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbUserReceiver",
                table: "TbUserReceiver",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbSubscriptionPackage",
                table: "TbSubscriptionPackage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbShippingType",
                table: "TbShippingType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbPaymentMethod",
                table: "TbPaymentMethod",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbCountry",
                table: "TbCountry",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbCity",
                table: "TbCity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbCarrier",
                table: "TbCarrier",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TbShippingPackaging",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ShippingPackagingAname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShippingPackagingEname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShippingPackaging", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbShipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ShippingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShippingTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShippingPackagingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    PackageValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingRate = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserSubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrackingNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShipment_TbPaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "TbPaymentMethod",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbShipment_TbShippingPackaging_ShippingPackagingId",
                        column: x => x.ShippingPackagingId,
                        principalTable: "TbShippingPackaging",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbShipment_TbShippingType_ShippingTypeId",
                        column: x => x.ShippingTypeId,
                        principalTable: "TbShippingType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbShipment_TbUserReceiver_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "TbUserReceiver",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbShipment_TbUserSender_SenderId",
                        column: x => x.SenderId,
                        principalTable: "TbUserSender",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbShipmentStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ShipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShipmentStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShipmentStatus_TbCarrier_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "TbCarrier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbShipmentStatus_TbShipment_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "TbShipment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbShipment_PaymentMethodId",
                table: "TbShipment",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShipment_ReceiverId",
                table: "TbShipment",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShipment_SenderId",
                table: "TbShipment",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShipment_ShippingPackagingId",
                table: "TbShipment",
                column: "ShippingPackagingId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShipment_ShippingTypeId",
                table: "TbShipment",
                column: "ShippingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShipmentStatus_CarrierId",
                table: "TbShipmentStatus",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShipmentStatus_ShipmentId",
                table: "TbShipmentStatus",
                column: "ShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCity_TbCountry_CountryId",
                table: "TbCity",
                column: "CountryId",
                principalTable: "TbCountry",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserReceiver_TbCity_CityId",
                table: "TbUserReceiver",
                column: "CityId",
                principalTable: "TbCity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserSender_TbCity_CityId",
                table: "TbUserSender",
                column: "CityId",
                principalTable: "TbCity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserSubscription_TbSubscriptionPackage_PackageId",
                table: "TbUserSubscription",
                column: "PackageId",
                principalTable: "TbSubscriptionPackage",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbCity_TbCountry_CountryId",
                table: "TbCity");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserReceiver_TbCity_CityId",
                table: "TbUserReceiver");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserSender_TbCity_CityId",
                table: "TbUserSender");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserSubscription_TbSubscriptionPackage_PackageId",
                table: "TbUserSubscription");

            migrationBuilder.DropTable(
                name: "TbShipmentStatus");

            migrationBuilder.DropTable(
                name: "TbShipment");

            migrationBuilder.DropTable(
                name: "TbShippingPackaging");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbUserSubscription",
                table: "TbUserSubscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbUserSender",
                table: "TbUserSender");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbUserReceiver",
                table: "TbUserReceiver");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbSubscriptionPackage",
                table: "TbSubscriptionPackage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbShippingType",
                table: "TbShippingType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbPaymentMethod",
                table: "TbPaymentMethod");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbCountry",
                table: "TbCountry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbCity",
                table: "TbCity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbCarrier",
                table: "TbCarrier");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "TbUserSender");

            migrationBuilder.DropColumn(
                name: "IsDefaultAddress",
                table: "TbUserSender");

            migrationBuilder.DropColumn(
                name: "OtherAddress",
                table: "TbUserSender");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "TbUserSender");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "TbUserReceiver");

            migrationBuilder.DropColumn(
                name: "IsDefaultAddress",
                table: "TbUserReceiver");

            migrationBuilder.DropColumn(
                name: "OtherAddress",
                table: "TbUserReceiver");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "TbUserReceiver");

            migrationBuilder.RenameTable(
                name: "TbUserSubscription",
                newName: "TbUserSubscriptions");

            migrationBuilder.RenameTable(
                name: "TbUserSender",
                newName: "TbUserSenders");

            migrationBuilder.RenameTable(
                name: "TbUserReceiver",
                newName: "TbUserReceivers");

            migrationBuilder.RenameTable(
                name: "TbSubscriptionPackage",
                newName: "TbSubscriptionPackages");

            migrationBuilder.RenameTable(
                name: "TbShippingType",
                newName: "TbShippingTypes");

            migrationBuilder.RenameTable(
                name: "TbPaymentMethod",
                newName: "TbPaymentMethods");

            migrationBuilder.RenameTable(
                name: "TbCountry",
                newName: "TbCountries");

            migrationBuilder.RenameTable(
                name: "TbCity",
                newName: "TbCities");

            migrationBuilder.RenameTable(
                name: "TbCarrier",
                newName: "TbCarriers");

            migrationBuilder.RenameIndex(
                name: "IX_TbUserSubscription_PackageId",
                table: "TbUserSubscriptions",
                newName: "IX_TbUserSubscriptions_PackageId");

            migrationBuilder.RenameIndex(
                name: "IX_TbUserSender_CityId",
                table: "TbUserSenders",
                newName: "IX_TbUserSenders_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_TbUserReceiver_CityId",
                table: "TbUserReceivers",
                newName: "IX_TbUserReceivers_CityId");

            migrationBuilder.RenameColumn(
                name: "ShipmentCount",
                table: "TbSubscriptionPackages",
                newName: "ShippimentCount");

            migrationBuilder.RenameIndex(
                name: "IX_TbCity_CountryId",
                table: "TbCities",
                newName: "IX_TbCities_CountryId");

            migrationBuilder.AlterColumn<string>(
                name: "CityEName",
                table: "TbCities",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CityAName",
                table: "TbCities",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbUserSubscriptions",
                table: "TbUserSubscriptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbUserSenders",
                table: "TbUserSenders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbUserReceivers",
                table: "TbUserReceivers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbSubscriptionPackages",
                table: "TbSubscriptionPackages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbShippingTypes",
                table: "TbShippingTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbPaymentMethods",
                table: "TbPaymentMethods",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbCountries",
                table: "TbCountries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbCities",
                table: "TbCities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbCarriers",
                table: "TbCarriers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TbShippments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShippingTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CurrentState = table.Column<byte>(type: "tinyint", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    PackageValue = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ShippingRate = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    TrackingNumber = table.Column<double>(type: "float", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserSubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShippments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShippments_TbPaymentMethods",
                        column: x => x.PaymentMethodId,
                        principalTable: "TbPaymentMethods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbShippments_TbShippingTypes",
                        column: x => x.ShippingTypeId,
                        principalTable: "TbShippingTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbShippments_TbUserReceivers",
                        column: x => x.ReceiverId,
                        principalTable: "TbUserReceivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbShippments_TbUserSenders",
                        column: x => x.SenderId,
                        principalTable: "TbUserSenders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbShippmentStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShippmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CurrentState = table.Column<byte>(type: "tinyint", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShippmentStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShippmentStatus_TbCarriers",
                        column: x => x.CarrierId,
                        principalTable: "TbCarriers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbShippmentStatus_TbShippments",
                        column: x => x.ShippmentId,
                        principalTable: "TbShippments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbShippments_PaymentMethodId",
                table: "TbShippments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippments_ReceiverId",
                table: "TbShippments",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippments_SenderId",
                table: "TbShippments",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippments_ShippingTypeId",
                table: "TbShippments",
                column: "ShippingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippmentStatus_CarrierId",
                table: "TbShippmentStatus",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippmentStatus_ShippmentId",
                table: "TbShippmentStatus",
                column: "ShippmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCities_TbCountries",
                table: "TbCities",
                column: "CountryId",
                principalTable: "TbCountries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserReceivers_TbCities",
                table: "TbUserReceivers",
                column: "CityId",
                principalTable: "TbCities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserSenders_TbCities",
                table: "TbUserSenders",
                column: "CityId",
                principalTable: "TbCities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserSubscriptions_TbSubscriptionPackages",
                table: "TbUserSubscriptions",
                column: "PackageId",
                principalTable: "TbSubscriptionPackages",
                principalColumn: "Id");
        }
    }
}
