using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class receiveddatenullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BirthDate",
                table: "User",
                type: "numeric(8)",
                precision: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,0)",
                oldPrecision: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "ReceivedTime",
                table: "Message",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReceivedDate",
                table: "Message",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "MessageStatus",
                table: "Message",
                type: "numeric(8)",
                precision: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,0)",
                oldPrecision: 8);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BirthDate",
                table: "User",
                type: "numeric(8,0)",
                precision: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(8)",
                oldPrecision: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "ReceivedTime",
                table: "Message",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ReceivedDate",
                table: "Message",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MessageStatus",
                table: "Message",
                type: "numeric(8,0)",
                precision: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(8)",
                oldPrecision: 8);
        }
    }
}
