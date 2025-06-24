using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsurenceManagementSystemWebApi.Migrations
{
    /// <inheritdoc />
    public partial class DeleteBehaviourConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policies_Agents_AgentId",
                table: "Policies");

            migrationBuilder.DropForeignKey(
                name: "FK_Policies_AvailablePolicies_AvailablePolicyId",
                table: "Policies");

            migrationBuilder.DropForeignKey(
                name: "FK_Policies_Customers_CustomerId",
                table: "Policies");

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_Agents_AgentId",
                table: "Policies",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "AgentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_AvailablePolicies_AvailablePolicyId",
                table: "Policies",
                column: "AvailablePolicyId",
                principalTable: "AvailablePolicies",
                principalColumn: "AvailablePolicyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_Customers_CustomerId",
                table: "Policies",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policies_Agents_AgentId",
                table: "Policies");

            migrationBuilder.DropForeignKey(
                name: "FK_Policies_AvailablePolicies_AvailablePolicyId",
                table: "Policies");

            migrationBuilder.DropForeignKey(
                name: "FK_Policies_Customers_CustomerId",
                table: "Policies");

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_Agents_AgentId",
                table: "Policies",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "AgentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_AvailablePolicies_AvailablePolicyId",
                table: "Policies",
                column: "AvailablePolicyId",
                principalTable: "AvailablePolicies",
                principalColumn: "AvailablePolicyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_Customers_CustomerId",
                table: "Policies",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
