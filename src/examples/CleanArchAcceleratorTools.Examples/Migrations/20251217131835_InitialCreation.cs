using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchAcceleratorTools.Examples.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_COUNTRY",
                columns: table => new
                {
                    SQ_COUNTRY = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CD_COUNTRY = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    NM_COUNTRY = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    DT_CREATION = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SQ_COUNTRY", x => x.SQ_COUNTRY);
                });

            migrationBuilder.CreateTable(
                name: "TB_INSTRUCTOR",
                columns: table => new
                {
                    SQ_INSTRUCTOR = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TX_FULL_NAME = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DT_CREATION = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SQ_INSTRUCTOR", x => x.SQ_INSTRUCTOR);
                });

            migrationBuilder.CreateTable(
                name: "TB_STATE",
                columns: table => new
                {
                    SQ_STATE = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SQ_COUNTRY = table.Column<long>(type: "bigint", nullable: true),
                    CD_STATE = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    NM_STATE = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    TX_ABBREVIATION = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    DT_CREATION = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SQ_STATE", x => x.SQ_STATE);
                    table.ForeignKey(
                        name: "FK_TB_STATE_TB_COUNTRY_SQ_COUNTRY",
                        column: x => x.SQ_COUNTRY,
                        principalTable: "TB_COUNTRY",
                        principalColumn: "SQ_COUNTRY");
                });

            migrationBuilder.CreateTable(
                name: "TB_COURSE",
                columns: table => new
                {
                    SQ_COURSE = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SQ_INSTRUCTOR = table.Column<long>(type: "bigint", nullable: true),
                    TX_TITLE = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DT_CREATION = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SQ_COURSE", x => x.SQ_COURSE);
                    table.ForeignKey(
                        name: "FK_TB_COURSE_TB_INSTRUCTOR_SQ_INSTRUCTOR",
                        column: x => x.SQ_INSTRUCTOR,
                        principalTable: "TB_INSTRUCTOR",
                        principalColumn: "SQ_INSTRUCTOR");
                });

            migrationBuilder.CreateTable(
                name: "TB_CITY",
                columns: table => new
                {
                    SQ_CITY = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SQ_STATE = table.Column<long>(type: "bigint", nullable: true),
                    CD_CITY = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    NM_CITY = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    DT_CREATION = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SQ_CITY", x => x.SQ_CITY);
                    table.ForeignKey(
                        name: "FK_TB_CITY_TB_STATE_SQ_STATE",
                        column: x => x.SQ_STATE,
                        principalTable: "TB_STATE",
                        principalColumn: "SQ_STATE");
                });

            migrationBuilder.CreateTable(
                name: "TB_MODULE",
                columns: table => new
                {
                    SQ_MODULE = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SQ_COURSE = table.Column<long>(type: "bigint", nullable: true),
                    TX_NAME = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DT_CREATION = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SQ_MODULE", x => x.SQ_MODULE);
                    table.ForeignKey(
                        name: "FK_TB_MODULE_TB_COURSE_SQ_COURSE",
                        column: x => x.SQ_COURSE,
                        principalTable: "TB_COURSE",
                        principalColumn: "SQ_COURSE");
                });

            migrationBuilder.CreateTable(
                name: "TB_ADDRESS",
                columns: table => new
                {
                    SQ_ADDRESS = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SQ_CITY = table.Column<long>(type: "bigint", nullable: true),
                    TX_STREET_AVENUE = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    TX_NUMBER = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TX_COMPLEMENT = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    TX_NEIGHBORHOOD = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    TX_POSTAL_CODE = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    DT_CREATION = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SQ_ADDRESS", x => x.SQ_ADDRESS);
                    table.ForeignKey(
                        name: "FK_TB_ADDRESS_TB_CITY_SQ_CITY",
                        column: x => x.SQ_CITY,
                        principalTable: "TB_CITY",
                        principalColumn: "SQ_CITY");
                });

            migrationBuilder.CreateTable(
                name: "TB_LESSON",
                columns: table => new
                {
                    SQ_LESSON = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SQ_MODULE = table.Column<long>(type: "bigint", nullable: true),
                    TX_TITLE = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DT_DURATION = table.Column<TimeSpan>(type: "time", nullable: true),
                    DT_CREATION = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SQ_LESSON", x => x.SQ_LESSON);
                    table.ForeignKey(
                        name: "FK_TB_LESSON_TB_MODULE_SQ_MODULE",
                        column: x => x.SQ_MODULE,
                        principalTable: "TB_MODULE",
                        principalColumn: "SQ_MODULE");
                });

            migrationBuilder.CreateTable(
                name: "TB_PROFILE",
                columns: table => new
                {
                    SQ_PROFILE = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SQ_INSTRUCTOR = table.Column<long>(type: "bigint", nullable: true),
                    SQ_ADDRESS = table.Column<long>(type: "bigint", nullable: true),
                    TX_BIO = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    TX_LINKEDIN = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    DT_CREATION = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SQ_PROFILE", x => x.SQ_PROFILE);
                    table.ForeignKey(
                        name: "FK_TB_PROFILE_TB_ADDRESS_SQ_ADDRESS",
                        column: x => x.SQ_ADDRESS,
                        principalTable: "TB_ADDRESS",
                        principalColumn: "SQ_ADDRESS");
                    table.ForeignKey(
                        name: "FK_TB_PROFILE_TB_INSTRUCTOR_SQ_INSTRUCTOR",
                        column: x => x.SQ_INSTRUCTOR,
                        principalTable: "TB_INSTRUCTOR",
                        principalColumn: "SQ_INSTRUCTOR");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_ADDRESS_SQ_CITY",
                table: "TB_ADDRESS",
                column: "SQ_CITY");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CITY_SQ_STATE",
                table: "TB_CITY",
                column: "SQ_STATE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_COURSE_SQ_INSTRUCTOR",
                table: "TB_COURSE",
                column: "SQ_INSTRUCTOR");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LESSON_SQ_MODULE",
                table: "TB_LESSON",
                column: "SQ_MODULE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MODULE_SQ_COURSE",
                table: "TB_MODULE",
                column: "SQ_COURSE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PROFILE_SQ_ADDRESS",
                table: "TB_PROFILE",
                column: "SQ_ADDRESS");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PROFILE_SQ_INSTRUCTOR",
                table: "TB_PROFILE",
                column: "SQ_INSTRUCTOR",
                unique: true,
                filter: "[SQ_INSTRUCTOR] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TB_STATE_SQ_COUNTRY",
                table: "TB_STATE",
                column: "SQ_COUNTRY");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_LESSON");

            migrationBuilder.DropTable(
                name: "TB_PROFILE");

            migrationBuilder.DropTable(
                name: "TB_MODULE");

            migrationBuilder.DropTable(
                name: "TB_ADDRESS");

            migrationBuilder.DropTable(
                name: "TB_COURSE");

            migrationBuilder.DropTable(
                name: "TB_CITY");

            migrationBuilder.DropTable(
                name: "TB_INSTRUCTOR");

            migrationBuilder.DropTable(
                name: "TB_STATE");

            migrationBuilder.DropTable(
                name: "TB_COUNTRY");
        }
    }
}
