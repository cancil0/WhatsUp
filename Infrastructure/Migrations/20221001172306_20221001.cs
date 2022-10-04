using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class _20221001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    MobilePhone = table.Column<string>(type: "varchar", nullable: false),
                    BirthDate = table.Column<decimal>(type: "numeric(8)", precision: 8, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedTime = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar", nullable: true),
                    UpdatedDate = table.Column<decimal>(type: "numeric", nullable: true),
                    UpdatedTime = table.Column<decimal>(type: "numeric", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Text = table.Column<string>(type: "text", maxLength: 4000, nullable: true),
                    MessageStatus = table.Column<decimal>(type: "numeric(8)", precision: 8, nullable: false),
                    SendDate = table.Column<decimal>(type: "numeric", nullable: false),
                    SendTime = table.Column<decimal>(type: "numeric", nullable: false),
                    ReceivedDate = table.Column<decimal>(type: "numeric", nullable: false),
                    ReceivedTime = table.Column<decimal>(type: "numeric", nullable: false),
                    FromId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedTime = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar", nullable: true),
                    UpdatedDate = table.Column<decimal>(type: "numeric", nullable: true),
                    UpdatedTime = table.Column<decimal>(type: "numeric", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_User_FromId",
                        column: x => x.FromId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_User_ToId",
                        column: x => x.ToId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_FromId",
                table: "Message",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ToId",
                table: "Message",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_User_MobilePhone",
                table: "User",
                column: "MobilePhone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
