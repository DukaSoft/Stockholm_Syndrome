using Microsoft.EntityFrameworkCore.Migrations;

namespace Stockholm_Syndrome_Web.Migrations
{
    public partial class FcRoleAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"SET IDENTITY_INSERT [dbo].[AspNetRoles] ON
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [Description]) VALUES (9, N'FC', N'FC', N'766f0ff5-61f7-451c-b08e-a427e1fdc647', N'Fleet Commander')
SET IDENTITY_INSERT [dbo].[AspNetRoles] OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
