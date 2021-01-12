using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace toDoList.Class
{
    public class clsCheckValidData
    {
        private ConConfigViewModel ConConfigViewModel;
        private string DataBase;
        private string connectionString;
        //private   SqlConnection sqlConnection;
        public clsCheckValidData(ConConfigViewModel _conConfigViewModel, string dataBase)
        {

            string _connectionString = "Data Source=" + _conConfigViewModel.NomeServidor +
                                      ";Initial Catalog=" + dataBase +
                                      ";Persist Security Info=True" +
                                      ";User ID=" + _conConfigViewModel.Utilizador +
                                      ";Password=" + _conConfigViewModel.Password +
                                      ";TransparentNetworkIPResolution=False;max pool size=32700";
            this.DataBase = dataBase;
            this.ConConfigViewModel = _conConfigViewModel;
            this.connectionString = _connectionString;
        }

        public bool ValidDbAndServer()
        {
            bool bResult = false;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        bResult = true;
                    }
                }
                catch (DbException ex)
                {
                    if (ex.ErrorCode == 500)
                    {
                        bResult = false;
                    }
                }
            }
            return bResult;
        }
    }
}
