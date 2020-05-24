using Microsoft.EntityFrameworkCore.Migrations;

namespace Stockholm_Syndrome_Web.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"SET IDENTITY_INSERT [dbo].[AspNetRoles] ON
                INSERT INTO[dbo].[AspNetRoles]([Id], [Name], [NormalizedName], [ConcurrencyStamp], [Description]) VALUES(1, N'Admin', N'ADMIN', N'52065857-fbab-44e5-aaaf-053b15f2c3c6', N'Site Administrator')
                INSERT INTO[dbo].[AspNetRoles]
                        ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [Description]) VALUES(2, N'Director', N'DIRECTOR', N'42bda49d-7a0b-4c3c-9c98-dd37dcdff9ad', N'Site Director')
                INSERT INTO[dbo].[AspNetRoles]
                        ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [Description]) VALUES(3, N'RecruitmentManager', N'RECRUITMENTMANAGER', N'074defc5-fcc8-41cc-b98f-b9d8cd2faf3e', N'Recruitment Manager can approve or reject Applications')
                INSERT INTO[dbo].[AspNetRoles]
                        ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [Description]) VALUES(4, N'FuelAdmin', N'FUELADMIN', N'316a2982-181a-4aff-b847-7703accb7ff8', N'Fuel Administrator can see ALL infrastructure, and Fuel Depot')
                INSERT INTO[dbo].[AspNetRoles]
                        ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [Description]) VALUES(5, N'FuelManager', N'FUELMANAGER', N'f9c6ca72-a857-4103-9db6-f35b95cca5d9', N'Fuel Manager can see NON critical infrastructure')
                INSERT INTO[dbo].[AspNetRoles]
                        ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [Description]) VALUES(6, N'MemberOfAlliance', N'MEMBEROFALLIANCE', N'b5faf98e-1955-4fd8-a814-93794aad4c0e', N'Member of Alliance')
                INSERT INTO [dbo].[AspNetRoles] 
                        ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [Description]) VALUES (7, N'OpsCreate', N'OPSCREATE', N'0d2d8c0f-fe58-4357-80ef-8475e60f456e', N'Able to Create Ops')
                INSERT INTO [dbo].[AspNetRoles] 
                        ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [Description]) VALUES (8, N'OpsManager', N'OPSMANAGER', N'b75843c9-fb09-4382-b277-a73993aa85d2', N'Able to Edit/Delete Ops')
                SET IDENTITY_INSERT[dbo].[AspNetRoles] OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
