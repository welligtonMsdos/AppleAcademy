using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppleAcademy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO "Perfil" ("Guid", "Name", "Active")
                SELECT '9ddf8627-87fa-4f38-86e1-9c5f5530ed98', 'Admin', true
                WHERE NOT EXISTS (SELECT 1 FROM "Perfil" WHERE lower("Name") = lower('Admin'))
                ON CONFLICT ("Guid") DO NOTHING;

                INSERT INTO "Perfil" ("Guid", "Name", "Active")
                SELECT 'dee0f9e9-becc-47a4-8aa6-db09ac1c394d', 'Aluno', true
                WHERE NOT EXISTS (SELECT 1 FROM "Perfil" WHERE lower("Name") = lower('Aluno'))
                ON CONFLICT ("Guid") DO NOTHING;

                INSERT INTO "Perfil" ("Guid", "Name", "Active")
                SELECT '8ba1ccbe-85a5-4eea-b6f0-51006fd1091f', 'Professor', true
                WHERE NOT EXISTS (SELECT 1 FROM "Perfil" WHERE lower("Name") = lower('Professor'))
                ON CONFLICT ("Guid") DO NOTHING;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Preserva perfis e usuários existentes ao reverter a migration.
        }
    }
}
