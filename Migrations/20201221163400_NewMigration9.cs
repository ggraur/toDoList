using Microsoft.EntityFrameworkCore.Migrations;

namespace toDoList.Migrations
{
    public partial class NewMigration9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(" declare @strSql nvarchar(max)='';	  " +
							   	 " if exists(select 1 from sys.views where name = 'db_vw_UtilizadoresEmpresa' and type = 'v') " +
								 "		DROP VIEW[dbo].[db_vw_UtilizadoresEmpresa]; " +
								 " " +
								 "				Begin " +
								 "						select @strSql = '  CREATE VIEW [dbo].[db_vw_UtilizadoresEmpresa] " +
								 "                      AS " +
								 "						SELECT        dbo.AspNetUsers.Id, dbo.AspNetUsers.AppUserAddressUserAddressId, dbo.AspNetUsers.UserName, dbo.AspNetUsers.NormalizedUserName, dbo.AspNetUsers.Email, dbo.AspNetUsers.NormalizedEmail, " +   
								 "								dbo.AspNetUsers.EmailConfirmed, dbo.AspNetUsers.PasswordHash, dbo.AspNetUsers.SecurityStamp, dbo.AspNetUsers.ConcurrencyStamp, dbo.AspNetUsers.PhoneNumber, dbo.AspNetUsers.PhoneNumberConfirmed, " +   
								 "								dbo.AspNetUsers.TwoFactorEnabled, dbo.AspNetUsers.LockoutEnd, dbo.AspNetUsers.LockoutEnabled, dbo.AspNetUsers.AccessFailedCount " +
								 "								FROM dbo.AspNetUsers RIGHT OUTER JOIN dbo.EmpresaUtilizadores ON dbo.AspNetUsers.Id = dbo.EmpresaUtilizadores.UserID; ' " +
								 "		EXEC sp_executesql @strSql " +
								 "	end " +
								 " " +
								 "	 if exists(select 1 from sys.views where name = 'db_vw_GabContabilidade' and type = 'v') " +
							     " " +
								 "						 DROP VIEW[dbo].[db_vw_GabContabilidade]; " +
								 "				Begin												 " +
								 "					select @strSql = '  CREATE VIEW [dbo].[db_vw_GabContabilidade]    " +
								 "				AS																		 " +
								 "				SELECT  EmpresaID, Nome, NIF, Licenca, NrPostos, NrEmpresas, DataExpiracao, DataCriacao, Ativo													   " +
								 "				FROM            dbo.EmpresasViewModel																																									   " +
								 "				WHERE(isCabContabilidade = 1); ' " +
								 "				EXEC sp_executesql @strSql		 " +
								 "	end													   " +
								 "" +
								 "	if exists(select 1 from sys.views where name = 'db_vw_EmpresasGabContabilidadeAno' and type = 'v') " +
								 " " +
								"						 DROP VIEW[dbo].[db_vw_EmpresasGabContabilidadeAno]; " +
							"					Begin																																																							   " +
							"						select @strSql = 'CREATE VIEW[dbo].[db_vw_EmpresasGabContabilidadeAno]    																		   " +
							"							  AS																	" +
							"							  SELECT DISTINCT EmpresaID, AnoFi FROM dbo.DadosEmpresaImportada; ' " +
							"			EXEC sp_executesql @strSql " +
							"		End " +
							"" +
							"	 if exists(select 1 from sys.views where name = 'db_vw_EmpresasGabContabilidade' and type = 'v')" +
							" " +
							"						 DROP VIEW[dbo].[db_vw_EmpresasGabContabilidade];																																																																																							" +
							"				begin																																																	   " +
							"					select @strSql = 'CREATE VIEW[dbo].[db_vw_EmpresasGabContabilidade]     															  " +
							"						  AS																														   " +
							"						  SELECT        EmpresaID, Nome, NIF, Licenca, NrPostos, NrEmpresas, DataExpiracao, DataCriacao, Ativo, IdCabContabilidade " +
							"						  FROM            dbo.EmpresasViewModel	   " +
							"						  WHERE(isCabContabilidade = 0); ' " +
							"		EXEC sp_executesql @strSql " +
							"	end");   

		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
