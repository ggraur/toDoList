using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace toDoList.Models
{
    public class SQL_IEmpresaAGes : IEmpresaAGes
    {
        private readonly AGesContext context;
        private readonly ILogger<SQL_IEmpresaAGes> logger;

        public SQL_IEmpresaAGes(AGesContext _context, ILogger<SQL_IEmpresaAGes> _logger)
        {
            this.context = _context;
            this.logger = _logger;

        }

        public IEnumerable<AGesEmpresasUtilizadores> GetUtilizadores(string NIF, string NomeEmpresa)
        {
            return context.AGesEmpresasUtilizadores.Where(x=>x.NIF==NIF && x.NomeEmpresa==NomeEmpresa);
        }
    }
}
