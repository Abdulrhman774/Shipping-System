using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class SetEmailAndPhoneAsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbUserSender_Email",
                table: "TbUserSender",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbUserSender_Phone",
                table: "TbUserSender",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbUserReceiver_Email",
                table: "TbUserReceiver",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbUserReceiver_Phone",
                table: "TbUserReceiver",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PhoneNumber",
                table: "AspNetUsers",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbUserSender_Email",
                table: "TbUserSender");

            migrationBuilder.DropIndex(
                name: "IX_TbUserSender_Phone",
                table: "TbUserSender");

            migrationBuilder.DropIndex(
                name: "IX_TbUserReceiver_Email",
                table: "TbUserReceiver");

            migrationBuilder.DropIndex(
                name: "IX_TbUserReceiver_Phone",
                table: "TbUserReceiver");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PhoneNumber",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
