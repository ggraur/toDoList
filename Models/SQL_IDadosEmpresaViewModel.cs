using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public class SQL_IDadosEmpresaViewModel : IDadosEmpresaViewModel
    {
        private readonly AppDbContext context;
        private readonly ILogger<SQL_ICLab> logger;

        public SQL_IDadosEmpresaViewModel(AppDbContext _context, ILogger<SQL_ICLab> _logger)
        {
            context = _context;
            logger = _logger;
        }
        public async Task<bool> Insert(DadosEmpresaImportada model)
        {
            bool bResult = false;
            if (model != null)
            {
                try
                {
                    context.DadosEmpresaImportada.Add(model);
                    var result = await context.SaveChangesAsync();
                    bResult = true;
                }
                catch (DbUpdateException ex)
                {
                    logger.Log(LogLevel.Warning, ex.Message);
                    bResult = false;
                }
            }
            return bResult;
        }
        public DadosEmpresaImportada ReturnModelByEmpresaAno(int EmpresaID, Int16 Ano)
        {
         
            return context.DadosEmpresaImportada.FirstOrDefault(x => x.EmpresaID == EmpresaID && x.AnoIn == Ano && x.AnoFi == Ano);
        }
        public bool ModelExist(DadosEmpresaImportada _model)
        {
            bool bResult = false;
            if (_model != null)
            {
                try
                {
                    var tt = context.DadosEmpresaImportada
                        .FirstOrDefault(x => x.EmpresaID == _model.EmpresaID && x.AnoIn == _model.AnoIn 
                        && x.AnoFi == _model.AnoFi && x.CodeAplicacao == _model.CodeAplicacao && x.CodeEmpresa == _model.CodeEmpresa);

                    if (tt != null)
                    {
                        bResult = true;
                    }
                    else
                    { bResult = false;
                    }
                }
                catch (DbUpdateException ex)
                {
                    logger.Log(LogLevel.Warning, ex.Message);
                    bResult = false;
                }
            }
            return bResult;
        }
        public Task<bool> Update(DadosEmpresaImportada model)
        {
            throw new NotImplementedException();
        }
    }
}
