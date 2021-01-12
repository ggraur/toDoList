using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models.Interfaces;

namespace toDoList.Models.SQL
{
    public class Sql_IDiarioLancamento : IDiarioLancamento
    {
        private readonly DbContext context;

        public Sql_IDiarioLancamento(DbContext customDbContext)
        {
            this.context = customDbContext;
        }

        public List<SelectListItem> GetDiarioLancemento()
        {
            List<SelectListItem> tmp = new List<SelectListItem>();
            using (context)
            {
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select DR, CONCAT(DR, ' - ', Descr) as 'Descr' from DrLan order by DR";
                    context.Database.OpenConnection();
                    using (DbDataReader dbDataReader = command.ExecuteReader())
                    {
                        while (dbDataReader.Read())
                        {
                            SelectListItem listItem = new SelectListItem()
                            {
                                Value = dbDataReader["DR"].ToString(),
                                Text = dbDataReader["Descr"].ToString()
                            };
                            tmp.Add(listItem);
                        }
                    }
                    context.Database.CloseConnection();
                }
                return tmp;
            }
        }

        public List<SelectListItem> GetTipoDocumento()
        {
            List<SelectListItem> tmp = new List<SelectListItem>();
            using (context)
            {
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select TDoc, CONCAT(TDoc, ' - ', Descr) as 'Descr' from TpDoc order by TDoc";
                    context.Database.OpenConnection();
                    using (DbDataReader dbDataReader = command.ExecuteReader())
                    {
                        while (dbDataReader.Read())
                        {
                            SelectListItem listItem = new SelectListItem()
                            {
                                Value = dbDataReader["TDoc"].ToString(),
                                Text = dbDataReader["Descr"].ToString()
                            };
                            tmp.Add(listItem);
                        }
                    }
                    context.Database.CloseConnection();
                }
                return tmp;
            }
        }

        public List<SelectListItem> GetTipoLancamento()
        {
            List<SelectListItem> tmp = new List<SelectListItem>();
            using (context)
            {
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select TLan, Descr from TpLan order by TLan";
                    context.Database.OpenConnection();
                    using (DbDataReader dbDataReader = command.ExecuteReader())
                    {
                        while (dbDataReader.Read())
                        {
                            SelectListItem listItem = new SelectListItem()
                            {
                                Value = dbDataReader["TLan"].ToString(),
                                Text = dbDataReader["Descr"].ToString()
                            };
                            tmp.Add(listItem);
                        }
                    }
                    context.Database.CloseConnection();
                }
                return tmp;
            }
        }
    }
}
