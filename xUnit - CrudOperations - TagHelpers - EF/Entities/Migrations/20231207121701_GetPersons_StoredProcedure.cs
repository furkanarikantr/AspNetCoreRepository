using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class GetPersons_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPerons = @"
                CREATE PROCEDURE [dbo].[GetAllPersons] 
                AS BEGIN
                    SELECT PersonId, PersonName, Email, DateOfBirth, Gender, CountryId, Address, ReceiveNewsLetters FROM [dbo].[Persons]
                END
            ";
            migrationBuilder.Sql(sp_GetAllPerons);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPerons = @"
                DROP PROCEDURE [dbo].[GetAllPersons] 
            ";
            migrationBuilder.Sql(sp_GetAllPerons);
        }
    }
}
