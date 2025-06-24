using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsurenceManagementSystemWebApi.Migrations
{
    /// <inheritdoc />
    public partial class updatingNavigatingProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policies_Agents_AgentId",
                table: "Policies");

            migrationBuilder.AlterColumn<int>(
                name: "AgentId",
                table: "Policies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_Agents_AgentId",
                table: "Policies",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "AgentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policies_Agents_AgentId",
                table: "Policies");

            migrationBuilder.AlterColumn<int>(
                name: "AgentId",
                table: "Policies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_Agents_AgentId",
                table: "Policies",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "AgentId");
        }
    }
}
