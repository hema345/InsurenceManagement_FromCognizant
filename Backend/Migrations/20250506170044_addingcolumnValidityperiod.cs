using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsurenceManagementSystemWebApi.Migrations
{
    /// <inheritdoc />
    public partial class addingcolumnValidityperiod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ValidityPeriod",
                table: "AvailablePolicies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidityPeriod",
                table: "AvailablePolicies");
        }
    }
}
