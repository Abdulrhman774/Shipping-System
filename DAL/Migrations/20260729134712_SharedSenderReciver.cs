using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class SharedSenderReciver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderName",
                table: "TbUserSender",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ReceiverName",
                table: "TbUserReceiver",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TbUserSender",
                newName: "SenderName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TbUserReceiver",
                newName: "ReceiverName");
        }
    }
}
