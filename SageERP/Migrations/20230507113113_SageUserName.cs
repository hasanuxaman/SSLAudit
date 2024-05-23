using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShampanERP.Migrations
{
    public partial class SageUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SageUserName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SageUserName",
                table: "AspNetUsers");
        }
    }
}
