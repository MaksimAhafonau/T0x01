using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceDC.Data.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "resources",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Type = table.Column<int>(type: "integer", nullable: false),
                Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Capacity = table.Column<int>(type: "integer", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_resources", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                PasswordHash = table.Column<string>(type: "text", nullable: false),
                Role = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "resource_schedules",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_resource_schedules", x => x.Id);
                table.ForeignKey(
                    name: "FK_resource_schedules_resources_ResourceId",
                    column: x => x.ResourceId,
                    principalTable: "resources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "bookings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                StartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                EndTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_bookings", x => x.Id);
                table.ForeignKey(
                    name: "FK_bookings_resources_ResourceId",
                    column: x => x.ResourceId,
                    principalTable: "resources",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_bookings_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_bookings_ResourceId_StartTime_EndTime",
            table: "bookings",
            columns: new[] { "ResourceId", "StartTime", "EndTime" });

        migrationBuilder.CreateIndex(
            name: "IX_bookings_UserId",
            table: "bookings",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_resource_schedules_ResourceId_DayOfWeek",
            table: "resource_schedules",
            columns: new[] { "ResourceId", "DayOfWeek" });

        migrationBuilder.CreateIndex(
            name: "IX_users_Email",
            table: "users",
            column: "Email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "bookings");

        migrationBuilder.DropTable(
            name: "resource_schedules");

        migrationBuilder.DropTable(
            name: "users");

        migrationBuilder.DropTable(
            name: "resources");
    }
}
