using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppleAcademy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO "Status" ("Guid", "Description", "Active")
                SELECT '9b587d0a-0063-4dd1-abed-ff200799ba2b', 'Pendente', true
                WHERE NOT EXISTS (SELECT 1 FROM "Status" WHERE lower("Description") = lower('Pendente'))
                ON CONFLICT ("Guid") DO NOTHING;

                INSERT INTO "Status" ("Guid", "Description", "Active")
                SELECT 'de6dd154-d5f0-4be4-94db-39ee6edd096f', 'Concluído', true
                WHERE NOT EXISTS (SELECT 1 FROM "Status" WHERE lower("Description") = lower('Concluído'))
                ON CONFLICT ("Guid") DO NOTHING;

                INSERT INTO "Status" ("Guid", "Description", "Active")
                SELECT '7e37f08a-ca9c-4954-9721-0dc09f4c8aa2', 'Negado', true
                WHERE NOT EXISTS (SELECT 1 FROM "Status" WHERE lower("Description") = lower('Negado'))
                ON CONFLICT ("Guid") DO NOTHING;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Preserva os status existentes ao reverter a migration.
        }
    }
}
