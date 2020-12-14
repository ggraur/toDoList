using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public class SQL_ConConfig : IConConfig
    {
        private readonly AppDbContext context;

        public SQL_ConConfig(AppDbContext context)
        {
            this.context = context;
        }

        //public async Task<ConConfigViewModel> GetModelByID(int id) 
        //{
        //    var taskResult = await Task.Run(() =>
        //    {
        //        return context.ConConfigViewModel.Where(x => x.EmpresaID == id);
        //    });
        //    return (ConConfigViewModel)taskResult;
        //}

        public bool DeleteConnection(ConConfigViewModel _model)
        {
            var bResult = false;
            ConConfigViewModel _tmpModel = context.ConConfigViewModel.Find(_model.ConexaoID);
            if (_tmpModel != null)
            {
                context.ConConfigViewModel.Remove(_tmpModel);
                context.SaveChanges();
            }
            bResult = true;
            return bResult;
        }

        public bool InsertConnection(ConConfigViewModel _model)
        {
            var bResult = false;
            context.ConConfigViewModel.Add(_model);
            context.SaveChanges();
            bResult = true;
            return bResult;
        }

        public bool UpdateConnection(ConConfigViewModel _model)
        {
            var bResult = false;
            var _tmp = context.ConConfigViewModel.Attach(_model);
            _tmp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            bResult = true;
            return bResult;
        }

        public IEnumerable<ConConfigViewModel> GetExistingRegistries(int EmpresaId)
        {
            return context.ConConfigViewModel.Where(x=>x.EmpresaId==EmpresaId).ToList();
        }
        public IEnumerable<ConConfigViewModel> FindById(int id)
        {

            return context.ConConfigViewModel.Where(x => x.ConexaoID == id);
        }
        public async Task<ConConfigViewModel> FindByIdAsync(int id)
        {
            var taskResult = await Task.Run(() =>
            {
                return context.ConConfigViewModel.Where(x => x.ConexaoID == id);
            });
            return (ConConfigViewModel)taskResult;
        }

        public ConConfigViewModel GetModelByID(int id)
        {
           return context.ConConfigViewModel.Where(x => x.ConexaoID == id).ToList()[0];
        }
    }


}
