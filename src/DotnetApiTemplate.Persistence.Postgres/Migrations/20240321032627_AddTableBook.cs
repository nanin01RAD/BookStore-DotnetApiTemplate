using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiTemplate.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddTableBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Author = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Publisher = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    YearPublish = table.Column<int>(type: "int", nullable: true),
                    Genre = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Price = table.Column<decimal>(type: "decimal", nullable: true),
                    QtyAvailable = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.BookId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book");
        }
    }
}
