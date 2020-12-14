using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public class SQL_IEmpresaUtilizadores : IEmpresaUtilizadores
    {   
        private readonly AppDbContext context;
        public SQL_IEmpresaUtilizadores(AppDbContext context)
        {
            this.context = context;
        }
        
        public bool DeleteUserEmpresa(EmpresaUtilizadoresViewModel _model)
        {
            var bResult = false;
            EmpresaUtilizadoresViewModel _tmpModel = context.EmpresaUtilizadores.Where(x => x.Id == _model.Id).ToList()[0];
            if (_tmpModel != null)
            {
                context.EmpresaUtilizadores.Remove(_model);
                context.SaveChanges();
            }
            bResult = true;
            return bResult;
        }

        public bool InsertUserEmpresa(EmpresaUtilizadoresViewModel _model)
        {
            var bResult = false;
            context.EmpresaUtilizadores.Add(_model);
            context.SaveChanges();
            bResult = true;
            return bResult;
        }

        public bool UpdateUserEmpresa(EmpresaUtilizadoresViewModel _model)
        {
            var bResult = false;
            var _tmp = context.EmpresaUtilizadores.Attach(_model);
            _tmp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            bResult = true;
            return bResult;
        }
    }
}
