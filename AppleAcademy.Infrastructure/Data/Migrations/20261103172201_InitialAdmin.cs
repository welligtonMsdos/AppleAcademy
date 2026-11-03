using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppleAcademy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Bancos criados anteriormente com EnsureCreated não têm histórico de migrations.
            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS "Perfil" (
                    "Guid" uuid NOT NULL CONSTRAINT "PK_Perfil" PRIMARY KEY,
                    "Name" character varying(50) NOT NULL,
                    "Active" boolean NOT NULL
                );
                CREATE TABLE IF NOT EXISTS "Status" (
                    "Guid" uuid NOT NULL CONSTRAINT "PK_Status" PRIMARY KEY,
                    "Description" character varying(50) NOT NULL,
                    "Active" boolean NOT NULL
                );
                CREATE TABLE IF NOT EXISTS "User" (
                    "Guid" uuid NOT NULL CONSTRAINT "PK_User" PRIMARY KEY,
                    "Name" character varying(150) NOT NULL,
                    "Email" character varying(150) NOT NULL,
                    "Password" character varying(100) NOT NULL,
                    "Active" boolean NOT NULL,
                    "PerfilGuid" uuid NOT NULL,
                    CONSTRAINT "FK_User_Perfil_PerfilGuid" FOREIGN KEY ("PerfilGuid")
                        REFERENCES "Perfil" ("Guid") ON DELETE CASCADE
                );
                CREATE INDEX IF NOT EXISTS "IX_User_PerfilGuid" ON "User" ("PerfilGuid");
                INSERT INTO "Perfil" ("Guid", "Name", "Active")
                VALUES ('9ddf8627-87fa-4f38-86e1-9c5f5530ed98', 'Admin', true)
                ON CONFLICT ("Guid") DO NOTHING;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Perfil");
        }
    }
}
