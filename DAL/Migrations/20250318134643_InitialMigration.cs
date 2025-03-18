using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Authors') THEN
                        CREATE TABLE ""Authors"" (
                            ""Id"" SERIAL PRIMARY KEY,
                            ""FirstName"" VARCHAR(100) NOT NULL,
                            ""LastName"" VARCHAR(100) NOT NULL,
                            ""BirthDate"" TIMESTAMPTZ NOT NULL,
                            ""Country"" TEXT NOT NULL
                        );
                    END IF;
                END
                $$;
            ");

            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Users') THEN
                        CREATE TABLE ""Users"" (
                            ""Id"" SERIAL PRIMARY KEY,
                            ""Username"" VARCHAR(50) NOT NULL,
                            ""PasswordHash"" TEXT NOT NULL,
                            ""Role"" VARCHAR(20) NOT NULL
                        );
                    END IF;
                END
                $$;
            ");

            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Books') THEN
                        CREATE TABLE ""Books"" (
                            ""Id"" SERIAL PRIMARY KEY,
                            ""ISBN"" VARCHAR(13) NOT NULL,
                            ""Title"" TEXT NOT NULL,
                            ""Genre"" TEXT NOT NULL,
                            ""Description"" TEXT NOT NULL,
                            ""AuthorId"" INTEGER NOT NULL,
                            ""BorrowedTime"" TIMESTAMPTZ NOT NULL,
                            ""ReturnTime"" TIMESTAMPTZ NOT NULL,
                            ""ImageData"" BYTEA NOT NULL,
                            CONSTRAINT ""FK_Books_Authors"" FOREIGN KEY (""AuthorId"") REFERENCES ""Authors""(""Id"") ON DELETE CASCADE
                        );
                    END IF;
                END
                $$;
            ");

            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'RefreshTokens') THEN
                        CREATE TABLE ""RefreshTokens"" (
                            ""Id"" SERIAL PRIMARY KEY,
                            ""Token"" TEXT NOT NULL,
                            ""ExpiryDate"" TIMESTAMPTZ NOT NULL,
                            ""UserId"" INTEGER NOT NULL,
                            CONSTRAINT ""FK_RefreshTokens_Users"" FOREIGN KEY (""UserId"") REFERENCES ""Users""(""Id"") ON DELETE CASCADE
                        );
                    END IF;
                END
                $$;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Books') THEN
                        DROP TABLE ""Books"";
                    END IF;
                END
                $$;
            ");

            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'RefreshTokens') THEN
                        DROP TABLE ""RefreshTokens"";
                    END IF;
                END
                $$;
            ");

            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Authors') THEN
                        DROP TABLE ""Authors"";
                    END IF;
                END
                $$;
            ");

            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Users') THEN
                        DROP TABLE ""Users"";
                    END IF;
                END
                $$;
            ");
        }
    }
}
