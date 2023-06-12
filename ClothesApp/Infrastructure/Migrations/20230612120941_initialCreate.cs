using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:order_status_type", "in_review,in_delivery,completed,cancelled")
                .Annotation("Npgsql:Enum:user_type", "admin,customer")
                .OldAnnotation("Npgsql:Enum:order_status_type", "on_hold,in_review,in_delivery,completed")
                .OldAnnotation("Npgsql:Enum:user_type", "admin,customer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:order_status_type", "on_hold,in_review,in_delivery,completed")
                .Annotation("Npgsql:Enum:user_type", "admin,customer")
                .OldAnnotation("Npgsql:Enum:order_status_type", "in_review,in_delivery,completed,cancelled")
                .OldAnnotation("Npgsql:Enum:user_type", "admin,customer");
        }
    }
}
