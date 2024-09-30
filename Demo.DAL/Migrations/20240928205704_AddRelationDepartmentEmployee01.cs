using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo.DAL.Migrations
{
    public partial class AddRelationDepartmentEmployee01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeptID",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DeptID",
                table: "Employees",
                column: "DeptID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DeptID",
                table: "Employees",
                column: "DeptID",
                principalTable: "Departments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DeptID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DeptID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DeptID",
                table: "Employees");
        }
    }
}
