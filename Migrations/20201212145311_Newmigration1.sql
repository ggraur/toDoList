if exists(select 1 from sys.views where name='db_vw_UtilizadoresEmpresa' and type='v')
   drop view [dbo].[db_vw_UtilizadoresEmpresa];
go

CREATE VIEW [dbo].[db_vw_UtilizadoresEmpresa]
AS
SELECT        dbo.AspNetUsers.Id, dbo.AspNetUsers.AppUserAddressUserAddressId, dbo.AspNetUsers.UserName, dbo.AspNetUsers.NormalizedUserName, dbo.AspNetUsers.Email, dbo.AspNetUsers.NormalizedEmail, 
                         dbo.AspNetUsers.EmailConfirmed, dbo.AspNetUsers.PasswordHash, dbo.AspNetUsers.SecurityStamp, dbo.AspNetUsers.ConcurrencyStamp, dbo.AspNetUsers.PhoneNumber, dbo.AspNetUsers.PhoneNumberConfirmed, 
                         dbo.AspNetUsers.TwoFactorEnabled, dbo.AspNetUsers.LockoutEnd, dbo.AspNetUsers.LockoutEnabled, dbo.AspNetUsers.AccessFailedCount, dbo.EmpresaUtilizadores.EmpresaId
FROM            dbo.AspNetUsers RIGHT OUTER JOIN
                         dbo.EmpresaUtilizadores ON dbo.AspNetUsers.Id = dbo.EmpresaUtilizadores.UserID
GO
