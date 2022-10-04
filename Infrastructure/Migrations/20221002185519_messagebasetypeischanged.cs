using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class messagebasetypeischanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "UpdatedTime",
                table: "Message",
                newName: "ReadTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Message",
                newName: "ReadDate");

            migrationBuilder.AlterColumn<decimal>(
                name: "BirthDate",
                table: "User",
                type: "numeric(8)",
                precision: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,0)",
                oldPrecision: 8);

            migrationBuilder.AlterColumn<Guid>(
                name: "ToId",
                table: "Message",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 11)
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<decimal>(
                name: "MessageStatus",
                table: "Message",
                type: "numeric(8)",
                precision: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,0)",
                oldPrecision: 8);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Message",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 12)
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<Guid>(
                name: "FromId",
                table: "Message",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 10)
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "ReadTime",
                table: "Message",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 9)
                .OldAnnotation("Relational:ColumnOrder", 15);

            migrationBuilder.AlterColumn<decimal>(
                name: "ReadDate",
                table: "Message",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 8)
                .OldAnnotation("Relational:ColumnOrder", 14);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReadTime",
                table: "Message",
                newName: "UpdatedTime");

            migrationBuilder.RenameColumn(
                name: "ReadDate",
                table: "Message",
                newName: "UpdatedDate");

            migrationBuilder.AlterColumn<decimal>(
                name: "BirthDate",
                table: "User",
                type: "numeric(8,0)",
                precision: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(8)",
                oldPrecision: 8);

            migrationBuilder.AlterColumn<Guid>(
                name: "ToId",
                table: "Message",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 9)
                .OldAnnotation("Relational:ColumnOrder", 11);

            migrationBuilder.AlterColumn<decimal>(
                name: "MessageStatus",
                table: "Message",
                type: "numeric(8,0)",
                precision: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(8)",
                oldPrecision: 8);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Message",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 10)
                .OldAnnotation("Relational:ColumnOrder", 12);

            migrationBuilder.AlterColumn<Guid>(
                name: "FromId",
                table: "Message",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 8)
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "UpdatedTime",
                table: "Message",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 15)
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<decimal>(
                name: "UpdatedDate",
                table: "Message",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 14)
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Message",
                type: "varchar",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 13);

            migrationBuilder.AddColumn<decimal>(
                name: "CreatedDate",
                table: "Message",
                type: "numeric",
                nullable: false,
                defaultValue: 0m)
                .Annotation("Relational:ColumnOrder", 11);

            migrationBuilder.AddColumn<decimal>(
                name: "CreatedTime",
                table: "Message",
                type: "numeric",
                nullable: false,
                defaultValue: 0m)
                .Annotation("Relational:ColumnOrder", 12);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Message",
                type: "varchar",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 16);
        }
    }
}
