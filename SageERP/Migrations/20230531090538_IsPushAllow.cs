using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShampanERP.Migrations
{
    public partial class IsPushAllow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPushAllow",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPushAllow",
                table: "AspNetUsers");
        }
    }
}
