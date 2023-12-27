using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Company");

            migrationBuilder.EnsureSchema(
                name: "Base");

            migrationBuilder.EnsureSchema(
                name: "Config");

            migrationBuilder.CreateTable(
                name: "CommercialSegment",
                schema: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommercialSegment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyIds",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyIds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentType",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LegalIdentifier = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Hostname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AuthorizedAgent_Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AuthorizedAgent_Surname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AuthorizedAgent_Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AuthorizedAgent_Identity_DocumentType = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    AuthorizedAgent_Identity_LegalIdentifier = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CommercialSegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_CommercialSegment_CommercialSegmentId",
                        column: x => x.CommercialSegmentId,
                        principalSchema: "Company",
                        principalTable: "CommercialSegment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommercialSegment_Name",
                schema: "Company",
                table: "CommercialSegment",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_AuthorizedAgent_Identity_LegalIdentifier",
                schema: "Company",
                table: "Company",
                column: "AuthorizedAgent_Identity_LegalIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_CommercialSegmentId",
                schema: "Company",
                table: "Company",
                column: "CommercialSegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Hostname",
                schema: "Company",
                table: "Company",
                column: "Hostname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_LegalIdentifier",
                schema: "Company",
                table: "Company",
                column: "LegalIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_Name",
                schema: "Company",
                table: "Company",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentType_Code",
                schema: "Config",
                table: "DocumentType",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company",
                schema: "Company");

            migrationBuilder.DropTable(
                name: "CompanyIds",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "DocumentType",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "CommercialSegment",
                schema: "Company");
        }
    }
}
