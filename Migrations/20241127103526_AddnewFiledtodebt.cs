using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taavoni.Migrations
{
    /// <inheritdoc />
    public partial class AddnewFiledtodebt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AmountWithPenaltyRate",
                table: "Debts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountWithPenaltyRate",
                table: "Debts");
        }
    }
}
