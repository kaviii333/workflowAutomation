using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkflowAutomation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExecutionSummaryFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalDurationSeconds",
                table: "Executions",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDurationSeconds",
                table: "Executions");
        }
    }
}
