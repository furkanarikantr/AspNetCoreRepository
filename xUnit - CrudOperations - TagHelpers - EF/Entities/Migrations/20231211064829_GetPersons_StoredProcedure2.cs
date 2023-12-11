using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class GetPersons_StoredProcedure2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string setTinValues = @"
            UPDATE [Persons] SET [TIN] = '11111';
            ";

            migrationBuilder.Sql(setTinValues);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string setTinValues = @"
            UPDATE [Persons] SET [TIN] = NULL;
        ";

            migrationBuilder.Sql(setTinValues);
        }
    }
}
