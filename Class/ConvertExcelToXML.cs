using Gestecnica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using toDoClassLibrary;
using toDoClassLibrary47;
using toDoList.Models;
using toDoList.ViewModels;

namespace toDoList.Class
{
    public class ConvertExcelToXML
    {
        private List<ExcelFileLine> data;
        private List<GeneralLedger> accounts;
        private List<Customer> customers;
        private List<Supplier> suppliers;
        private List<SageVAT> sageVats;
        private List<SageCashFlow> sageCashFlows;
        private List<SageBudgetLine> sageBudgetLines;
        private List<SageDefrayal> sageDefrayals;
        private List<SageSector> sageSectors;
        private List<SageCostCenter> sageCostCenters;
        private List<SageJournal> sageJournals;
        private List<SageDocumentType> sageDocumentTypes;
        private List<Document> documents;
        private string DataLanc;
        private string ExcelFilePath;
        private string XMLFilePath;
        private int NIF;
        private int Year;
        private int Month;
     //   private string LocalCon;
        private string CodEmpSage;
        private string SedeFiscal;
        private string NomeEmpresa;
        private string Morada;
        private string CodPostal;
        private string Localidade;
        private string EmpSageCon;
        private string CodDiario;
        private string TipoLanc;
        private string TipoDoc;
        private bool LancUnico;

        public List<ErrorLine> errors;
        private string baseDados;
        private DadosEmpresaImportada dadosEmpresaImportada;

        // private readonly StreamReader streamReader;
        private readonly ExcelLabViewModel model;
        //private readonly string month;
        //private readonly string db;
        private readonly DadosEmpresaImportada dadosEmpresaView;
        private readonly ConConfigViewModel conConfigViewModel;
        private readonly EmpresasViewModel empresaViewModel;
        private readonly Empr empr;

        public ConvertExcelToXML(ExcelLabViewModel _model, string baseDados, DadosEmpresaImportada dadosEmpresaImportada, ConConfigViewModel conConfigViewModel, EmpresasViewModel empresaViewModel, Empr _empr)
        {
            this.model = _model;
            this.baseDados = baseDados;
            this.dadosEmpresaImportada = dadosEmpresaImportada;
            this.conConfigViewModel = conConfigViewModel;
            this.empresaViewModel = empresaViewModel;
        
            //this.streamReader = _streamReader;
    
            Month = (int)Enum.Parse(typeof(MonthEnum), _model.MesLancamento.ToString());
    
            this.dadosEmpresaView = dadosEmpresaImportada;
            this.conConfigViewModel = conConfigViewModel;
            this.empresaViewModel = empresaViewModel;
            this.empr = _empr;

            data = new List<ExcelFileLine>();
            accounts = new List<GeneralLedger>();
            customers = new List<Customer>();
            suppliers = new List<Supplier>();
            sageVats = new List<SageVAT>();
            sageCashFlows = new List<SageCashFlow>();
            sageBudgetLines = new List<SageBudgetLine>();
            sageDefrayals = new List<SageDefrayal>();
            sageSectors = new List<SageSector>();
            sageCostCenters = new List<SageCostCenter>();
            sageJournals = new List<SageJournal>();
            sageDocumentTypes = new List<SageDocumentType>();
            documents = new List<Document>();
            this.DataLanc = model.DataLancamento.ToString("yyyy-MM-dd");
            ExcelFilePath = model.InputFilePath;// ExcelPath;
            XMLFilePath = model.OutputFilePath;
            string[] ExcelPathArray = ExcelFilePath.Split('\\');
            string fileName = ExcelPathArray[ExcelPathArray.Length - 1];
            string[] fileNameArray = fileName.Split('.');
            string fileNameNoExt = fileNameArray[0];
            int index = fileNameNoExt.IndexOf(" (");
            string fileNameNoExtRep = index >= 0 ? fileNameNoExt.Remove(index) : fileNameNoExt;
            string[] fileNameNoExtArray = fileNameNoExtRep.Split('_');
            NIF = Convert.ToInt32(fileNameNoExtArray[0]);
            Year = Convert.ToInt32(fileNameNoExtArray[1]);
           // string[] monthArray = fileNameNoExtArray[3];
            //Month = int.Parse(fileNameNoExtArray[3]);
            this.CodEmpSage = dadosEmpresaView.CodeEmpresa;
            SedeFiscal = empr.Sede;
            NomeEmpresa =empr.Nome;
            Morada =empr.Morada;
            CodPostal = empr.CodPostal;
            Localidade = empr.Localidade;

            this.CodDiario = model.DiarioLancamentoStr;
            this.TipoLanc = model.TipoLancamentoStr;
            this.TipoDoc = model.TipoLancamentoStr;
            this.LancUnico = model.LancamentoUnico;

            EncryptionHelper encryptionHelper = new EncryptionHelper();

            this.EmpSageCon = "Data Source=" + conConfigViewModel.NomeServidor +
                              ";Initial Catalog=" + baseDados +
                              ";Persist Security Info=True" +
                              ";User ID=" + conConfigViewModel.Utilizador +
                              ";Password=" + encryptionHelper.Decrypt(conConfigViewModel.Password) + ";";
            this.errors = new List<ErrorLine>();

        }

       

        public void ConvertFile()
        {
            try
            {    
                string diarioDesc = model.DiarioLancamentoStr;// //selectDiario_command.ExecuteScalar().ToString();
                sageJournals.Add(new SageJournal { SageJournalID = CodDiario, SageDescription = diarioDesc, SageNumeration = "MENS", SageOrigin = "MC", SageType = "NOR" });

                    
                string docDesc = model.TipoDocumentoStr; //selectDoc_command.ExecuteScalar().ToString();
                sageDocumentTypes.Add(new SageDocumentType { SageDocumentTypeID = TipoDoc, SageDescription = docDesc });

                clsExcelReader _excelReader = new clsExcelReader(ExcelFilePath);
                DataTable _excelData = _excelReader.ReturnExcelDataTable();

                bool blankCells = false;            // = true, se o ficheiro tiver células em branco (à excepção do centro de custo)
                bool invalidAccountsLines = false;  // = true, se ambas as contas a creditar e a debitar forem contas de cliente ou fornecedor (se começarem por 21 ou 22)
                bool repeatedDocNumbers = false;

                int rowNumber = 1;
                int uniqueDocLineNumber = 1;

                Document uniqueDoc = new Document(DataLanc + " DocumentoUnico");

                foreach (DataRow dr in _excelData.Rows)
                {
                    if (dr[0].ToString().Replace(" ", "") == ""
                        && dr[1].ToString().Replace(" ", "") == ""
                        && dr[2].ToString().Replace(" ", "") == ""
                        && dr[3].ToString().Replace(" ", "") == ""
                        && dr[4].ToString().Replace(" ", "") == ""
                        && dr[5].ToString().Replace(" ", "") == ""
                        && dr[6].ToString().Replace(" ", "") == "")
                    {
                        break;  // fim do ficheiro Excel
                    }

                    if (dr[0].ToString().Replace(" ", "") == ""
                        || dr[1].ToString().Replace(" ", "") == ""
                        //|| dr[2].ToString().Replace(" ", "") == ""
                        || dr[4].ToString().Replace(" ", "") == ""
                        || dr[5].ToString().Replace(" ", "") == ""
                        || dr[6].ToString().Replace(" ", "") == "")
                    {
                        blankCells = true;
                        break;
                    }

                    if ((dr[0].ToString().StartsWith("21") || dr[0].ToString().StartsWith("22"))
                        && (dr[1].ToString().StartsWith("21") || dr[1].ToString().StartsWith("22")))
                    {
                        invalidAccountsLines = true;
                        break;
                    }

                    ExcelFileLine line = new ExcelFileLine();
                    line.Conta_Creditar = dr[0].ToString();
                    line.Conta_Debitar = dr[1].ToString();
                    line.NIF = dr[2].ToString();
                    line.Centro_Custo = dr[3].ToString();
                    line.Data_Lanc = Convert.ToDateTime(dr[4].ToString()).ToString("dd-MM-yyyy");
                    line.Num_Doc = dr[5].ToString();
                    line.Valor = dr[6].ToString();
                    data.Add(line);

                    if (line.Conta_Creditar != "")
                    {
                        if (!accounts.Exists(x => x.AccountID.Equals(line.Conta_Creditar)))
                        {
                            bool contaExiste;
                            string Descr = "";
                            string EdeMov = "False";
                            string ContaIVA = "";

                            using (SqlConnection con = new SqlConnection(EmpSageCon))
                            {
                                con.Open();

                                string selectAccount_query = "select * from Conta where CConta = '" + line.Conta_Creditar + "'";
                                SqlCommand selectAccount_command = new SqlCommand(selectAccount_query, con);
                                var readerConta = selectAccount_command.ExecuteReader();
                                contaExiste = readerConta.HasRows;
                                if (contaExiste)
                                {
                                    while (readerConta.Read())
                                    {
                                        Descr = readerConta["Descr"].ToString();
                                        EdeMov = readerConta["EdeMov"].ToString();
                                        ContaIVA = readerConta["CCIva"].ToString();
                                    }
                                    if (EdeMov == "False")
                                    {
                                        errors.Add(new ErrorLine { AccountID = line.Conta_Creditar, ErrorMsg = "Conta não é de movimento" });
                                    }
                                }
                                else
                                {
                                    errors.Add(new ErrorLine { AccountID = line.Conta_Creditar, ErrorMsg = "Conta não existe" });
                                }
                                readerConta.Close();

                                if (ContaIVA != "")
                                {
                                    if (!accounts.Exists(x => x.AccountID.Equals(ContaIVA)))
                                    {
                                        string IVAEdeMov = "False";
                                        string DescrIVA = "";
                                        string Regime = "";
                                        string Taxa = "0";

                                        string selectContaIVA_query = "select * from Conta where CConta = '" + ContaIVA + "'";
                                        SqlCommand selectContaIVA_command = new SqlCommand(selectContaIVA_query, con);
                                        var readerIVA = selectContaIVA_command.ExecuteReader();
                                        if (readerIVA.HasRows)
                                        {
                                            while (readerIVA.Read())
                                            {
                                                DescrIVA = readerIVA["Descr"].ToString();
                                                Regime = readerIVA["IvaRg"].ToString();
                                                IVAEdeMov = readerIVA["EdeMov"].ToString();
                                            }
                                        }
                                        readerIVA.Close();

                                        if (IVAEdeMov == "True")
                                        {
                                            switch (SedeFiscal)
                                            {
                                                case "C":
                                                    switch (Regime)
                                                    {
                                                        case "N":
                                                            Taxa = "23";
                                                            break;
                                                        case "I":
                                                            Taxa = "13";
                                                            break;
                                                        case "R":
                                                            Taxa = "6";
                                                            break;
                                                    }
                                                    break;

                                                case "M":
                                                    switch (Regime)
                                                    {
                                                        case "N":
                                                            Taxa = "22";
                                                            break;
                                                        case "I":
                                                            Taxa = "12";
                                                            break;
                                                        case "R":
                                                            Taxa = "5";
                                                            break;
                                                    }
                                                    break;

                                                case "A":
                                                    switch (Regime)
                                                    {
                                                        case "N":
                                                            Taxa = "18";
                                                            break;
                                                        case "I":
                                                            Taxa = "9";
                                                            break;
                                                        case "R":
                                                            Taxa = "4";
                                                            break;
                                                    }
                                                    break;
                                            }

                                            accounts.Add(new GeneralLedger { AccountID = ContaIVA, AccountDescription = DescrIVA, SageAuxType = "NA", ContaIVA = "" });
                                            sageVats.Add(new SageVAT
                                            {
                                                SageVatID = ContaIVA.Length > 7 ? ContaIVA.Substring(3, 4) : ContaIVA.Substring(3),
                                                SageDescription = DescrIVA,
                                                SageTaxPayableAccount = ContaIVA,
                                                SageRecapitulative = "True",
                                                SageNotTaxSupported = "0",
                                                SageVatType = "DED",
                                                SageMarket = "EL0",
                                                SageVatPercentage = Taxa
                                            });
                                            var vat = sageVats.Find(x => x.SageTaxPayableAccount.Equals(ContaIVA));
                                            if (vat.SageVatID.StartsWith("21")) vat.SageTypeOfGood = "MERC";
                                            else if (vat.SageVatID.StartsWith("22")) vat.SageTypeOfGood = "IMOB";
                                            else if (vat.SageVatID.StartsWith("23")) vat.SageTypeOfGood = "OBS";
                                            else vat.SageTypeOfGood = "OUTROS";
                                        }
                                        else
                                        {
                                            errors.Add(new ErrorLine { AccountID = line.Conta_Creditar, ErrorMsg = "Conta tem uma conta de IVA que não é de movimento" });
                                        }
                                    }
                                }

                                con.Close();
                                con.Dispose();
                            }

                            if (line.Conta_Creditar.StartsWith("21"))
                            {
                                accounts.Add(new GeneralLedger { AccountID = line.Conta_Creditar, AccountDescription = Descr, SageAuxType = "CLI", ContaIVA = ContaIVA });

                                //string NIFCliente = line.NIF;
                                //string Nome = "(sem nome)";

                                //using (SqlConnection con = new SqlConnection(EmpSageCon))
                                //{
                                //    con.Open();

                                //    string selectCustomer_query = "select * from Terce where CConC = '" + line.Conta_Creditar + "'";
                                //    SqlCommand selectCustomer_command = new SqlCommand(selectCustomer_query, con);
                                //    var reader = selectCustomer_command.ExecuteReader();
                                //    if (reader.HasRows)
                                //    {
                                //        while (reader.Read())
                                //        {
                                //            NIFCliente = reader["CIFis"].ToString();
                                //            Nome = reader["Nome"].ToString();
                                //        }
                                //    }
                                //    else
                                //    {
                                //        errors.Add(new ErrorLine { AccountID = line.Conta_Creditar, ErrorMsg = "Cliente não existe" });
                                //    }
                                //    reader.Close();

                                //    con.Close();
                                //    con.Dispose();
                                //}

                                //if (NIFCliente != line.NIF)
                                //{
                                //    errors.Add(new ErrorLine { AccountID = line.Conta_Creditar, ErrorMsg = "NIF incorreto" });
                                //}

                                // APENAS PARA EFEITOS DE TESTE
                                //customers.Add(new Customer { AccountID = line.Conta_Creditar, CustomerID = line.Conta_Creditar.Substring(line.Conta_Creditar.Length - 3), CustomerTaxID = line.NIF, CompanyName = Nome });
                            }
                            else if (line.Conta_Creditar.StartsWith("22"))
                            {
                                accounts.Add(new GeneralLedger { AccountID = line.Conta_Creditar, AccountDescription = Descr, SageAuxType = "FOR", ContaIVA = ContaIVA });

                                //string NIFFornecedor = line.NIF;
                                //string Nome = "(sem nome)";

                                //using (SqlConnection con = new SqlConnection(EmpSageCon))
                                //{
                                //    con.Open();

                                //    string selectSupplier_query = "select * from Terce where CConF = '" + line.Conta_Creditar + "'";
                                //    SqlCommand selectSupplier_command = new SqlCommand(selectSupplier_query, con);
                                //    var reader = selectSupplier_command.ExecuteReader();
                                //    if (reader.HasRows)
                                //    {
                                //        while (reader.Read())
                                //        {
                                //            NIFFornecedor = reader["CIFis"].ToString();
                                //            Nome = reader["Nome"].ToString();
                                //        }
                                //    }
                                //    else
                                //    {
                                //        errors.Add(new ErrorLine { AccountID = line.Conta_Creditar, ErrorMsg = "Fornecedor não existe" });
                                //    }
                                //    reader.Close();

                                //    con.Close();
                                //    con.Dispose();
                                //}

                                //if (NIFFornecedor != line.NIF)
                                //{
                                //    errors.Add(new ErrorLine { AccountID = line.Conta_Creditar, ErrorMsg = "NIF incorreto" });
                                //}

                                // APENAS PARA EFEITOS DE TESTE
                                //suppliers.Add(new Supplier { AccountID = line.Conta_Creditar, SupplierID = line.Conta_Creditar.Substring(line.Conta_Creditar.Length - 3), SupplierTaxID = line.NIF, CompanyName = Nome });
                            }
                            else if (line.Conta_Creditar.StartsWith("12"))
                            {
                                accounts.Add(new GeneralLedger { AccountID = line.Conta_Creditar, AccountDescription = Descr, SageAuxType = "TES", ContaIVA = ContaIVA });
                            }
                            else
                            {
                                if (line.Conta_Creditar.StartsWith("243"))
                                {
                                    errors.Add(new ErrorLine { AccountID = line.Conta_Creditar, ErrorMsg = "Esta conta é de IVA, como tal não pode ser inserida no ficheiro" });
                                }
                                else
                                {
                                    accounts.Add(new GeneralLedger { AccountID = line.Conta_Creditar, AccountDescription = Descr, SageAuxType = "NA", ContaIVA = ContaIVA });
                                }
                            }
                        }

                        if (line.NIF != "" && !LancUnico)
                        {
                            string CIFis = new clsCalculo().ReturnStr("select CIFis from Terce where CIFis = '" + line.NIF + "'", "CIFis", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                            bool nifExists = CIFis != "";

                            string nome = new clsCalculo().ReturnStr("select Nome from Terce where CIFis = '" + line.NIF + "'", "Nome", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));

                            if (line.Conta_Creditar.StartsWith("21") && !customers.Exists(x => x.AccountID == line.Conta_Creditar && x.CustomerTaxID == line.NIF))
                            {
                                string CConC = new clsCalculo().ReturnStr("select CConC from Terce where CIFis = '" + line.NIF + "'", "CConC", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));

                                if (CConC == "")
                                {
                                    if (nifExists)
                                    {
                                        clsGesSQLCommands.RunManualCommand("update Terce set CConC = '" + CConC + "' where CIFis = '" + line.NIF + "'", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                                        customers.Add(new Customer { AccountID = line.Conta_Creditar, CompanyName = nome, CustomerID = rowNumber.ToString(), CustomerTaxID = line.NIF });
                                    }
                                    else
                                    {
                                        customers.Add(new Customer { AccountID = line.Conta_Creditar, CompanyName = "(sem nome)", CustomerID = rowNumber.ToString(), CustomerTaxID = line.NIF });
                                    }
                                }
                                else
                                {
                                    if (CConC == line.Conta_Creditar)
                                        customers.Add(new Customer { AccountID = line.Conta_Creditar, CompanyName = nome, CustomerID = rowNumber.ToString(), CustomerTaxID = line.NIF });
                                    else
                                        errors.Add(new ErrorLine { AccountID = line.NIF, ErrorMsg = "NIF já está associado a outro cliente" });
                                }
                            }
                            else if (line.Conta_Creditar.StartsWith("22") && !suppliers.Exists(x => x.AccountID == line.Conta_Creditar && x.SupplierTaxID == line.NIF))
                            {
                                string CConF = new clsCalculo().ReturnStr("select CConF from Terce where CIFis = '" + line.NIF + "'", "CConF", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));

                                if (CConF == "")
                                {
                                    if (nifExists)
                                    {
                                        clsGesSQLCommands.RunManualCommand("update Terce set CConF = '" + CConF + "' where CIFis = '" + line.NIF + "'", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                                        suppliers.Add(new Supplier { AccountID = line.Conta_Creditar, CompanyName = nome, SupplierID = rowNumber.ToString(), SupplierTaxID = line.NIF });
                                    }
                                    else
                                    {
                                        suppliers.Add(new Supplier { AccountID = line.Conta_Creditar, CompanyName = "(sem nome)", SupplierID = rowNumber.ToString(), SupplierTaxID = line.NIF });
                                    }
                                }
                                else
                                {
                                    if (CConF == line.Conta_Creditar)
                                        suppliers.Add(new Supplier { AccountID = line.Conta_Creditar, CompanyName = nome, SupplierID = rowNumber.ToString(), SupplierTaxID = line.NIF });
                                    else
                                        errors.Add(new ErrorLine { AccountID = line.NIF, ErrorMsg = "NIF já está associado a outro fornecedor" });
                                }
                            }
                        }

                    }

                    if (line.Conta_Debitar != "")
                    {
                        if (!accounts.Exists(x => x.AccountID.Equals(line.Conta_Debitar)))
                        {
                            bool contaExiste;
                            string Descr = "";
                            string EdeMov = "False";
                            string ContaIVA = "";

                            using (SqlConnection con = new SqlConnection(EmpSageCon))
                            {
                                con.Open();

                                string selectAccount_query = "select * from Conta where CConta = '" + line.Conta_Debitar + "'";
                                SqlCommand selectAccount_command = new SqlCommand(selectAccount_query, con);
                                var readerConta = selectAccount_command.ExecuteReader();
                                contaExiste = readerConta.HasRows;
                                if (contaExiste)
                                {
                                    while (readerConta.Read())
                                    {
                                        Descr = EncodeStringToXML(readerConta["Descr"].ToString());
                                        EdeMov = readerConta["EdeMov"].ToString();
                                        ContaIVA = readerConta["CCIva"].ToString();
                                    }
                                    if (EdeMov == "False")
                                    {
                                        errors.Add(new ErrorLine { AccountID = line.Conta_Debitar, ErrorMsg = "Conta não é de movimento" });
                                    }
                                }
                                else
                                {
                                    errors.Add(new ErrorLine { AccountID = line.Conta_Debitar, ErrorMsg = "Conta não existe" });
                                }
                                readerConta.Close();

                                if (ContaIVA != "")
                                {
                                    if (!accounts.Exists(x => x.AccountID.Equals(ContaIVA)))
                                    {
                                        string IVAEdeMov = "False";
                                        string DescrIVA = "";
                                        string Regime = "";
                                        string Taxa = "0";

                                        string selectContaIVA_query = "select * from Conta where CConta = '" + ContaIVA + "'";
                                        SqlCommand selectContaIVA_command = new SqlCommand(selectContaIVA_query, con);
                                        var readerIVA = selectContaIVA_command.ExecuteReader();
                                        if (readerIVA.HasRows)
                                        {
                                            while (readerIVA.Read())
                                            {
                                                DescrIVA = readerIVA["Descr"].ToString();
                                                Regime = readerIVA["IvaRg"].ToString();
                                                IVAEdeMov = readerIVA["EdeMov"].ToString();
                                            }
                                        }
                                        readerIVA.Close();

                                        if (IVAEdeMov == "True")
                                        {
                                            switch (SedeFiscal)
                                            {
                                                case "C":
                                                    switch (Regime)
                                                    {
                                                        case "N":
                                                            Taxa = "23";
                                                            break;
                                                        case "I":
                                                            Taxa = "13";
                                                            break;
                                                        case "R":
                                                            Taxa = "6";
                                                            break;
                                                    }
                                                    break;

                                                case "M":
                                                    switch (Regime)
                                                    {
                                                        case "N":
                                                            Taxa = "22";
                                                            break;
                                                        case "I":
                                                            Taxa = "12";
                                                            break;
                                                        case "R":
                                                            Taxa = "5";
                                                            break;
                                                    }
                                                    break;

                                                case "A":
                                                    switch (Regime)
                                                    {
                                                        case "N":
                                                            Taxa = "18";
                                                            break;
                                                        case "I":
                                                            Taxa = "9";
                                                            break;
                                                        case "R":
                                                            Taxa = "4";
                                                            break;
                                                    }
                                                    break;
                                            }

                                            accounts.Add(new GeneralLedger { AccountID = ContaIVA, AccountDescription = DescrIVA, SageAuxType = "NA", ContaIVA = "" });
                                            sageVats.Add(new SageVAT
                                            {
                                                SageVatID = ContaIVA.Length > 7 ? ContaIVA.Substring(3, 4) : ContaIVA.Substring(3),
                                                SageDescription = DescrIVA,
                                                SageTaxPayableAccount = ContaIVA,
                                                SageRecapitulative = "True",
                                                SageNotTaxSupported = "0",
                                                SageVatType = "DED",
                                                SageMarket = "EL0",
                                                SageVatPercentage = Taxa
                                            });
                                            var vat = sageVats.Find(x => x.SageTaxPayableAccount.Equals(ContaIVA));
                                            if (vat.SageVatID.StartsWith("21")) vat.SageTypeOfGood = "MERC";
                                            else if (vat.SageVatID.StartsWith("22")) vat.SageTypeOfGood = "IMOB";
                                            else if (vat.SageVatID.StartsWith("23")) vat.SageTypeOfGood = "OBS";
                                            else vat.SageTypeOfGood = "OUTROS";
                                        }
                                        else
                                        {
                                            errors.Add(new ErrorLine { AccountID = line.Conta_Creditar, ErrorMsg = "Conta tem uma conta de IVA que não é de movimento" });
                                        }
                                    }
                                }

                                con.Close();
                                con.Dispose();
                            }

                            if (line.Conta_Debitar.StartsWith("21"))
                            {
                                accounts.Add(new GeneralLedger { AccountID = line.Conta_Debitar, AccountDescription = Descr, SageAuxType = "CLI", ContaIVA = ContaIVA });

                                //string NIFCliente = line.NIF;
                                //string Nome = "(sem nome)";

                                //using (SqlConnection con = new SqlConnection(EmpSageCon))
                                //{
                                //    con.Open();

                                //    string selectCustomer_query = "select * from Terce where CConC = '" + line.Conta_Debitar + "'";
                                //    SqlCommand selectCustomer_command = new SqlCommand(selectCustomer_query, con);
                                //    var reader = selectCustomer_command.ExecuteReader();
                                //    if (reader.HasRows)
                                //    {
                                //        while (reader.Read())
                                //        {
                                //            NIFCliente = reader["CIFis"].ToString();
                                //            Nome = reader["Nome"].ToString();
                                //        }
                                //    }
                                //    else
                                //    {
                                //        errors.Add(new ErrorLine { AccountID = line.Conta_Debitar, ErrorMsg = "Cliente não existe" });
                                //    }
                                //    reader.Close();

                                //    con.Close();
                                //    con.Dispose();
                                //}

                                //if (NIFCliente != line.NIF)
                                //{
                                //    errors.Add(new ErrorLine { AccountID = line.Conta_Debitar, ErrorMsg = "NIF incorreto" });
                                //}

                                // APENAS PARA EFEITOS DE TESTE
                                //customers.Add(new Customer { AccountID = line.Conta_Debitar, CustomerID = line.Conta_Debitar.Substring(line.Conta_Debitar.Length - 3), CustomerTaxID = line.NIF, CompanyName = Nome });
                            }
                            else if (line.Conta_Debitar.StartsWith("22"))
                            {
                                accounts.Add(new GeneralLedger { AccountID = line.Conta_Debitar, AccountDescription = Descr, SageAuxType = "FOR", ContaIVA = ContaIVA });

                                //string NIFFornecedor = line.NIF;
                                //string Nome = "(sem nome)";

                                //using (SqlConnection con = new SqlConnection(EmpSageCon))
                                //{
                                //    con.Open();

                                //    string selectSupplier_query = "select * from Terce where CConF = '" + line.Conta_Debitar + "'";
                                //    SqlCommand selectSupplier_command = new SqlCommand(selectSupplier_query, con);
                                //    var reader = selectSupplier_command.ExecuteReader();
                                //    if (reader.HasRows)
                                //    {
                                //        while (reader.Read())
                                //        {
                                //            NIFFornecedor = reader["CIFis"].ToString();
                                //            Nome = reader["Nome"].ToString();
                                //        }
                                //    }
                                //    else
                                //    {
                                //        errors.Add(new ErrorLine { AccountID = line.Conta_Debitar, ErrorMsg = "Fornecedor não existe" });
                                //    }
                                //    reader.Close();

                                //    con.Close();
                                //    con.Dispose();
                                //}

                                //if (NIFFornecedor != line.NIF)
                                //{
                                //    errors.Add(new ErrorLine { AccountID = line.Conta_Debitar, ErrorMsg = "NIF incorreto" });
                                //}

                                // APENAS PARA EFEITOS DE TESTE
                                //suppliers.Add(new Supplier { AccountID = line.Conta_Debitar, SupplierID = line.Conta_Debitar.Substring(line.Conta_Debitar.Length - 3), SupplierTaxID = line.NIF, CompanyName = Nome });
                            }
                            else if (line.Conta_Debitar.StartsWith("12"))
                            {
                                accounts.Add(new GeneralLedger { AccountID = line.Conta_Debitar, AccountDescription = Descr, SageAuxType = "TES", ContaIVA = ContaIVA });
                            }
                            else
                            {
                                if (line.Conta_Debitar.StartsWith("243"))
                                {
                                    errors.Add(new ErrorLine { AccountID = line.Conta_Debitar, ErrorMsg = "Esta conta é de IVA, como tal não pode ser inserida no ficheiro" });
                                }
                                else
                                {
                                    accounts.Add(new GeneralLedger { AccountID = line.Conta_Debitar, AccountDescription = Descr, SageAuxType = "NA", ContaIVA = ContaIVA });
                                }
                            }
                        }

                        if (line.NIF != "" && !LancUnico)
                        {
                            string CIFis = new clsCalculo().ReturnStr("select CIFis from Terce where CIFis = '" + line.NIF + "'", "CIFis", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                            bool nifExists = CIFis != "";

                            string nome = EncodeStringToXML(new clsCalculo().ReturnStr("select Nome from Terce where CIFis = '" + line.NIF + "'", "Nome", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon)));

                            if (line.Conta_Debitar.StartsWith("21") && !customers.Exists(x => x.AccountID == line.Conta_Debitar && x.CustomerTaxID == line.NIF))
                            {
                                string CConC = new clsCalculo().ReturnStr("select CConC from Terce where CIFis = '" + line.NIF + "'", "CConC", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));

                                if (CConC == "")
                                {
                                    if (nifExists)
                                    {
                                        clsGesSQLCommands.RunManualCommand("update Terce set CConC = '" + CConC + "' where CIFis = '" + line.NIF + "'", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                                        customers.Add(new Customer { AccountID = line.Conta_Debitar, CompanyName = nome, CustomerID = rowNumber.ToString(), CustomerTaxID = line.NIF });
                                    }
                                    else
                                    {
                                        customers.Add(new Customer { AccountID = line.Conta_Debitar, CompanyName = "(sem nome)", CustomerID = rowNumber.ToString(), CustomerTaxID = line.NIF });
                                    }
                                }
                                else
                                {
                                    if (CConC == line.Conta_Debitar)
                                        customers.Add(new Customer { AccountID = line.Conta_Debitar, CompanyName = nome, CustomerID = rowNumber.ToString(), CustomerTaxID = line.NIF });
                                    else
                                        errors.Add(new ErrorLine { AccountID = line.NIF, ErrorMsg = "NIF já está associado a outro cliente" });
                                }
                            }
                            else if (line.Conta_Debitar.StartsWith("22") && !suppliers.Exists(x => x.AccountID == line.Conta_Debitar && x.SupplierTaxID == line.NIF))
                            {
                                string CConF = new clsCalculo().ReturnStr("select CConF from Terce where CIFis = '" + line.NIF + "'", "CConF", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));

                                if (CConF == "")
                                {
                                    if (nifExists)
                                    {
                                        clsGesSQLCommands.RunManualCommand("update Terce set CConF = '" + CConF + "' where CIFis = '" + line.NIF + "'", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                                        suppliers.Add(new Supplier { AccountID = line.Conta_Debitar, CompanyName = nome, SupplierID = rowNumber.ToString(), SupplierTaxID = line.NIF });
                                    }
                                    else
                                    {
                                        suppliers.Add(new Supplier { AccountID = line.Conta_Debitar, CompanyName = "(sem nome)", SupplierID = rowNumber.ToString(), SupplierTaxID = line.NIF });
                                    }
                                }
                                else
                                {
                                    if (CConF == line.Conta_Debitar)
                                        suppliers.Add(new Supplier { AccountID = line.Conta_Debitar, CompanyName = nome, SupplierID = rowNumber.ToString(), SupplierTaxID = line.NIF });
                                    else
                                        errors.Add(new ErrorLine { AccountID = line.NIF, ErrorMsg = "NIF já está associado a outro fornecedor" });
                                }
                            }
                        }
                    }

                    if (line.Centro_Custo.Replace(" ", "") != "" && !sageCostCenters.Exists(x => x.SageCostCenterID.Equals(line.Centro_Custo.ToUpper())))
                    {
                        string CCeCu = new clsCalculo().ReturnStr("select CCeCu from CenCu where CCeCu = '" + line.Centro_Custo + "'", "CCeCu", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                        string Descr = new clsCalculo().ReturnStr("select Descr from CenCu where CCeCu = '" + line.Centro_Custo + "'", "Descr", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                        if (CCeCu != "")
                            sageCostCenters.Add(new SageCostCenter { SageCostCenterID = CCeCu, SageDescription = Descr });
                        else
                        {
                            if (!errors.Exists(x => x.AccountID == CCeCu && x.ErrorMsg == "Centro de custo não existe"))
                                errors.Add(new ErrorLine { AccountID = CCeCu, ErrorMsg = "Centro de custo não existe" });
                        }
                    }

                    if (!documents.Exists(x => x.Doc_Numero.Equals(line.Num_Doc)))
                    {
                        string specifier = "F";
                        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-CA");

                        string valorString = line.Valor;

                        string entityID = "";

                        //if (line.Conta_Creditar.StartsWith("21") || line.Conta_Creditar.StartsWith("22"))
                        //{
                        //    entityID = line.Conta_Creditar.Substring(line.Conta_Creditar.Length - 3);
                        //}
                        //else if (line.Conta_Debitar.StartsWith("21") || line.Conta_Debitar.StartsWith("22"))
                        //{
                        //    entityID = line.Conta_Debitar.Substring(line.Conta_Debitar.Length - 3);
                        //}

                        if (line.Conta_Creditar.StartsWith("21"))
                        {
                            var docCustomer = customers.Find(x => x.AccountID == line.Conta_Creditar && x.CustomerTaxID == line.NIF);
                            if (docCustomer != null)
                                entityID = docCustomer.CustomerID;
                            else
                            {
                                if (!LancUnico)
                                    continue;
                            }
                        }
                        else if (line.Conta_Debitar.StartsWith("21"))
                        {
                            var docCustomer = customers.Find(x => x.AccountID == line.Conta_Debitar && x.CustomerTaxID == line.NIF);
                            if (docCustomer != null)
                                entityID = docCustomer.CustomerID;
                            else
                            {
                                if (!LancUnico)
                                    continue;
                            }
                        }
                        else if (line.Conta_Creditar.StartsWith("22"))
                        {
                            var docSupplier = suppliers.Find(x => x.AccountID == line.Conta_Creditar && x.SupplierTaxID == line.NIF);
                            if (docSupplier != null)
                                entityID = docSupplier.SupplierID;
                            else
                            {
                                if (!LancUnico)
                                    continue;
                            }
                        }
                        else if (line.Conta_Debitar.StartsWith("22"))
                        {
                            var docSupplier = suppliers.Find(x => x.AccountID == line.Conta_Debitar && x.SupplierTaxID == line.NIF);
                            if (docSupplier != null)
                                entityID = docSupplier.SupplierID;
                            else
                            {
                                if (!LancUnico)
                                    continue;
                            }
                        }

                        var doc = new Document(line.Num_Doc);
                        doc.Data_Lanc = line.Data_Lanc;
                        doc.EntityID = entityID;

                        var acc1 = accounts.Find(x => x.AccountID.Equals(line.Conta_Creditar));
                        var acc2 = accounts.Find(x => x.AccountID.Equals(line.Conta_Debitar));

                        var docLine1 = new Transaction { Cta_Conta = line.Conta_Creditar, Cta_NrCCust = "", Data_Lanc = doc.Data_Lanc, Doc_Numero = doc.Doc_Numero, Doc_Ordem = LancUnico ? uniqueDocLineNumber++.ToString() : "1", Val_Valor = "-" + Convert.ToDouble(valorString).ToString(specifier, culture), Lan_Refer = acc1.AccountDescription, SageAuxType = acc1.SageAuxType, SageControl = "OUT" };
                        var docLine2 = new Transaction { Cta_Conta = line.Conta_Debitar, Cta_NrCCust = sageCostCenters.Exists(x => x.SageCostCenterID == line.Centro_Custo) ? line.Centro_Custo : "", Data_Lanc = doc.Data_Lanc, Doc_Numero = doc.Doc_Numero, Doc_Ordem = LancUnico ? uniqueDocLineNumber++.ToString() : "2", Val_Valor = Convert.ToDouble(valorString).ToString(specifier, culture), Lan_Refer = acc2.AccountDescription, SageAuxType = acc2.SageAuxType, SageControl = "OUT" };
                        Transaction docLine3 = null;

                        if (acc1.ContaIVA != "")
                        {
                            var sageVat = sageVats.Find(x => x.SageTaxPayableAccount.Equals(acc1.ContaIVA));
                            if (sageVat != null)
                            {
                                var valor = Convert.ToDouble(valorString);
                                var taxa = Convert.ToDouble(sageVat.SageVatPercentage) / 100;
                                var valorPercTaxa = valor * taxa;

                                docLine3 = new Transaction { Cta_Conta = acc1.ContaIVA, Cta_NrCCust = "", Data_Lanc = doc.Data_Lanc, Doc_Numero = doc.Doc_Numero, Doc_Ordem = LancUnico ? uniqueDocLineNumber++.ToString() : "3", Val_Valor = "-" + valorPercTaxa.ToString(specifier, culture), Lan_Refer = sageVat.SageDescription, SageAuxType = "NA", SageControl = "VLRIVA", SageVatID = sageVat.SageVatID };
                                docLine2.Val_Valor = (valor + valorPercTaxa).ToString(specifier, culture);
                                docLine1.SageControl = "BASIVA";
                                docLine1.SageVatID = sageVat.SageVatID;
                            }
                        }
                        else if (acc2.ContaIVA != "")
                        {
                            var sageVat = sageVats.Find(x => x.SageTaxPayableAccount.Equals(acc2.ContaIVA));
                            if (sageVat != null)
                            {
                                var valor = Convert.ToDouble(valorString);
                                var taxa = Convert.ToDouble(sageVat.SageVatPercentage) / 100;
                                var valorPercTaxa = valor * taxa;

                                docLine3 = new Transaction { Cta_Conta = acc2.ContaIVA, Cta_NrCCust = "", Data_Lanc = doc.Data_Lanc, Doc_Numero = doc.Doc_Numero, Doc_Ordem = LancUnico ? uniqueDocLineNumber++.ToString() : "3", Val_Valor = valorPercTaxa.ToString(specifier, culture), Lan_Refer = sageVat.SageDescription, SageAuxType = "NA", SageControl = "VLRIVA", SageVatID = sageVat.SageVatID };
                                docLine1.Val_Valor = "-" + (valor + valorPercTaxa).ToString(specifier, culture);
                                docLine2.SageControl = "BASIVA";
                                docLine2.SageVatID = sageVat.SageVatID;
                            }
                        }

                        documents.Add(doc);
                        doc.AddTransaction(docLine1);
                        doc.AddTransaction(docLine2);
                        if (docLine3 != null)
                            doc.AddTransaction(docLine3);

                        uniqueDoc.AddTransaction(docLine1);
                        uniqueDoc.AddTransaction(docLine2);
                        if (docLine3 != null)
                            uniqueDoc.AddTransaction(docLine3);
                    }
                    else
                    {
                        repeatedDocNumbers = true;
                        break;
                    }

                    rowNumber++;
                }

                FileInfo uploadedFile = new FileInfo(ExcelFilePath);
                uploadedFile.Delete();

                if (blankCells)
                {
                    errors.RemoveAll(x => x.ErrorMsg != "");
                    errors.Add(new ErrorLine { AccountID = "", ErrorMsg = "O ficheiro Excel contém células obrigatórias em branco" });
                    return;
                }

                if (invalidAccountsLines)
                {
                    errors.RemoveAll(x => x.ErrorMsg != "");
                    errors.Add(new ErrorLine { AccountID = "", ErrorMsg = "O ficheiro Excel contém linhas em que as contas a creditar e a debitar são ambas de cliente ou fornecedor" });
                    return;
                }

                if (repeatedDocNumbers)
                {
                    errors.RemoveAll(x => x.ErrorMsg != "");
                    errors.Add(new ErrorLine { AccountID = "", ErrorMsg = "O ficheiro Excel contém números de documento repetidos" });
                    return;
                }

                accounts.Sort(delegate (GeneralLedger x, GeneralLedger y)
                {
                    if (x.AccountID == null && y.AccountID == null) return 0;
                    else if (x.AccountID == null) return -1;
                    else if (y.AccountID == null) return 1;
                    else return x.AccountID.CompareTo(y.AccountID);
                });

                errors.Sort(delegate (ErrorLine x, ErrorLine y)
                {
                    if (x.AccountID == null && y.AccountID == null) return 0;
                    else if (x.AccountID == null) return -1;
                    else if (y.AccountID == null) return 1;
                    else return x.AccountID.CompareTo(y.AccountID);
                });



                uniqueDoc.Doc_Tipo = TipoDoc;
                uniqueDoc.Diar_Mes = Month.ToString();
                uniqueDoc.Diar_Numero = CodDiario;
                uniqueDoc.Data_Lanc = DataLanc;
                uniqueDoc.EntityType = "";
                uniqueDoc.EntityID = "";
                uniqueDoc.Doc_Categoria = "";

                if (LancUnico)
                {
                    documents.Clear();
                    documents.Add(uniqueDoc);
                }



                int monthDays = 0;
                switch (Month)
                {
                    case 1:
                        monthDays = 31;
                        break;
                    case 2:
                        if (DateTime.IsLeapYear(Year))
                        {
                            monthDays = 29;
                        }
                        else
                        {
                            monthDays = 28;
                        }
                        break;
                    case 3:
                        monthDays = 31;
                        break;
                    case 4:
                        monthDays = 30;
                        break;
                    case 5:
                        monthDays = 31;
                        break;
                    case 6:
                        monthDays = 30;
                        break;
                    case 7:
                        monthDays = 31;
                        break;
                    case 8:
                        monthDays = 31;
                        break;
                    case 9:
                        monthDays = 30;
                        break;
                    case 10:
                        monthDays = 31;
                        break;
                    case 11:
                        monthDays = 30;
                        break;
                    case 12:
                        monthDays = 31;
                        break;
                }

                DateTime startDate = new DateTime(Year, Month, 1);
                DateTime endDate = new DateTime(Year, Month, monthDays);
                FileInfo XMLfile = new FileInfo(XMLFilePath + "\\SAGE_ACCOUNTING_EXCEL_" + startDate.ToString("yyyyMMdd") + "_" + endDate.ToString("yyyyMMdd") + "_" + NIF + ".xml");

                using (StreamWriter sw = new StreamWriter(XMLfile.FullName, false, Encoding.GetEncoding("iso-8859-1")))
                {
                    // INÍCIO DO XML
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"Windows-1252\" standalone=\"no\"?>");
                    sw.WriteLine("<AuditFile xmlns=\"urn:OECD:StandardAuditFile-Tax:PT_1.03_01\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");



                    // Header
                    sw.WriteLine("\t<Header xmlns=\"urn:OECD:StandardAuditFile-Tax:PT_1.03_01\">");
                    sw.WriteLine("\t\t<AuditFileVersion>1.03_01</AuditFileVersion>");
                    sw.WriteLine("\t\t<CompanyID>" + CodEmpSage + "</CompanyID>");
                    sw.WriteLine("\t\t<TaxRegistrationNumber>" + NIF + "</TaxRegistrationNumber>");
                    sw.WriteLine("\t\t<TaxAccountingBasis>" + SedeFiscal + "</TaxAccountingBasis>");
                    sw.WriteLine("\t\t<CompanyName>" + NomeEmpresa + "</CompanyName>");
                    sw.WriteLine("\t\t<BusinessName>" + NomeEmpresa + "</BusinessName>");
                    sw.WriteLine("\t\t<CompanyAdress>");
                    sw.WriteLine("\t\t\t<AddressDetail>" + Morada + "</AddressDetail>");
                    sw.WriteLine("\t\t\t<City>" + Localidade + "</City>");
                    sw.WriteLine("\t\t\t<PostalCode>" + CodPostal + "</PostalCode>");
                    sw.WriteLine("\t\t</CompanyAdress>");
                    sw.WriteLine("\t\t<FiscalYear>" + Year + "</FiscalYear>");
                    sw.WriteLine("\t\t<StartDate>" + startDate.ToString("dd-MM-yyyy") + "</StartDate>");
                    sw.WriteLine("\t\t<EndDate>" + endDate.ToString("dd-MM-yyyy") + "</EndDate>");
                    sw.WriteLine("\t\t<CurrencyCode>EUR</CurrencyCode>");
                    sw.WriteLine("\t\t<DateCreated>" + DateTime.Now.ToString("yyyy-MM-dd") + "</DateCreated>");
                    sw.WriteLine("\t\t<SageExtendedData>");
                    sw.WriteLine("\t\t\t<SageOriginatorApp>CLAB</SageOriginatorApp>");
                    sw.WriteLine("\t\t\t<SageExportDate>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "</SageExportDate>");
                    sw.WriteLine("\t\t\t<SageVersion>003</SageVersion>");
                    sw.WriteLine("\t\t</SageExtendedData>");
                    sw.WriteLine("\t</Header>\n");

                    // MasterFiles (informação sobre todos os números de conta)
                    sw.WriteLine("\t<MasterFiles xmlns=\"urn:OECD:StandardAuditFile-Tax:PT_1.03_01\">");
                    sw.WriteLine("\t\t<!-- Plano de Contas -->");

                    // GeneralLedger
                    // Bancos
                    foreach (var acc in accounts)
                    {
                        if (acc.SageAuxType == "TES")
                        {
                            sw.WriteLine("\t\t<GeneralLedger>");

                            sw.WriteLine("\t\t\t<AccountID>" + acc.AccountID + "</AccountID>");
                            sw.WriteLine("\t\t\t<AccountDescription>" + acc.AccountDescription + "</AccountDescription>");
                            sw.WriteLine("\t\t\t<SageExtendedData>");
                            sw.WriteLine("\t\t\t\t<SageAuxType>" + acc.SageAuxType + "</SageAuxType>");
                            sw.WriteLine("\t\t\t\t<SageCashFlowID/>");
                            sw.WriteLine("\t\t\t\t<SageBudgetLineID/>");
                            sw.WriteLine("\t\t\t\t<SageDefrayalID/>");
                            sw.WriteLine("\t\t\t\t<SageCheckingAccount>0</SageCheckingAccount>");
                            sw.WriteLine("\t\t\t</SageExtendedData>");

                            sw.WriteLine("\t\t</GeneralLedger>");
                        }
                    }

                    // Clientes
                    foreach (var acc in accounts)
                    {
                        if (acc.SageAuxType == "CLI")
                        {
                            sw.WriteLine("\t\t<GeneralLedger>");

                            sw.WriteLine("\t\t\t<AccountID>" + acc.AccountID + "</AccountID>");
                            sw.WriteLine("\t\t\t<AccountDescription>" + acc.AccountDescription + "</AccountDescription>");
                            sw.WriteLine("\t\t\t<SageExtendedData>");
                            sw.WriteLine("\t\t\t\t<SageAuxType>" + acc.SageAuxType + "</SageAuxType>");
                            sw.WriteLine("\t\t\t\t<SageCashFlowID/>");
                            sw.WriteLine("\t\t\t\t<SageBudgetLineID/>");
                            sw.WriteLine("\t\t\t\t<SageDefrayalID/>");
                            sw.WriteLine("\t\t\t\t<SageCheckingAccount>0</SageCheckingAccount>");
                            sw.WriteLine("\t\t\t</SageExtendedData>");

                            sw.WriteLine("\t\t</GeneralLedger>");
                        }
                    }

                    // Fornecedores
                    foreach (var acc in accounts)
                    {
                        if (acc.SageAuxType == "FOR")
                        {
                            sw.WriteLine("\t\t<GeneralLedger>");

                            sw.WriteLine("\t\t\t<AccountID>" + acc.AccountID + "</AccountID>");
                            sw.WriteLine("\t\t\t<AccountDescription>" + acc.AccountDescription + "</AccountDescription>");
                            sw.WriteLine("\t\t\t<SageExtendedData>");
                            sw.WriteLine("\t\t\t\t<SageAuxType>" + acc.SageAuxType + "</SageAuxType>");
                            sw.WriteLine("\t\t\t\t<SageCashFlowID/>");
                            sw.WriteLine("\t\t\t\t<SageBudgetLineID/>");
                            sw.WriteLine("\t\t\t\t<SageDefrayalID/>");
                            sw.WriteLine("\t\t\t\t<SageCheckingAccount>0</SageCheckingAccount>");
                            sw.WriteLine("\t\t\t</SageExtendedData>");

                            sw.WriteLine("\t\t</GeneralLedger>");
                        }
                    }

                    // Outros
                    foreach (var acc in accounts)
                    {
                        if (acc.SageAuxType == "NA")
                        {
                            sw.WriteLine("\t\t<GeneralLedger>");

                            sw.WriteLine("\t\t\t<AccountID>" + acc.AccountID + "</AccountID>");
                            sw.WriteLine("\t\t\t<AccountDescription>" + acc.AccountDescription + "</AccountDescription>");
                            sw.WriteLine("\t\t\t<SageExtendedData>");
                            sw.WriteLine("\t\t\t\t<SageAuxType>" + acc.SageAuxType + "</SageAuxType>");
                            sw.WriteLine("\t\t\t\t<SageCashFlowID/>");
                            sw.WriteLine("\t\t\t\t<SageBudgetLineID/>");
                            sw.WriteLine("\t\t\t\t<SageDefrayalID/>");
                            sw.WriteLine("\t\t\t\t<SageCheckingAccount>0</SageCheckingAccount>");
                            sw.WriteLine("\t\t\t</SageExtendedData>");

                            sw.WriteLine("\t\t</GeneralLedger>");
                        }
                    }

                    // Customer
                    sw.WriteLine("\t\t<!-- Clientes -->");
                    foreach (var c in customers)
                    {
                        sw.WriteLine("\t\t<Customer>");
                        sw.WriteLine("\t\t\t<CustomerID>" + c.CustomerID + "</CustomerID>");
                        sw.WriteLine("\t\t\t<AccountID>" + c.AccountID + "</AccountID>");
                        sw.WriteLine("\t\t\t<CustomerTaxID>" + c.CustomerTaxID + "</CustomerTaxID>");
                        sw.WriteLine("\t\t\t<CompanyName>" + c.CompanyName + "</CompanyName>");
                        sw.WriteLine("\t\t</Customer>");
                    }

                    // Supplier
                    sw.WriteLine("\t\t<!-- Fornecedores -->");
                    foreach (var s in suppliers)
                    {
                        sw.WriteLine("\t\t<Supplier>");
                        sw.WriteLine("\t\t\t<SupplierID>" + s.SupplierID + "</SupplierID>");
                        sw.WriteLine("\t\t\t<AccountID>" + s.AccountID + "</AccountID>");
                        sw.WriteLine("\t\t\t<SupplierTaxID>" + s.SupplierTaxID + "</SupplierTaxID>");
                        sw.WriteLine("\t\t\t<CompanyName>" + s.CompanyName + "</CompanyName>");
                        sw.WriteLine("\t\t</Supplier>");
                    }

                    // SageVAT
                    sw.WriteLine("\t\t<!-- Códigos de IVA da Gestão Comercial -->");
                    foreach (var vat in sageVats)
                    {
                        sw.WriteLine("\t\t<SageVAT>");
                        sw.WriteLine("\t\t\t<SageVatID>" + vat.SageVatID + "</SageVatID>");
                        sw.WriteLine("\t\t\t<SageDescription>" + vat.SageDescription + "</SageDescription>");
                        sw.WriteLine("\t\t\t<SageTaxPayableAccount>" + vat.SageTaxPayableAccount + "</SageTaxPayableAccount>");
                        sw.WriteLine("\t\t\t<SageRecapitulative>" + vat.SageRecapitulative + "</SageRecapitulative>");
                        sw.WriteLine("\t\t\t<SageReverseChange>" + vat.SageReverseCharge + "</SageReverseChange>");
                        sw.WriteLine("\t\t\t<SageNotTaxSupported>" + vat.SageNotTaxSupported + "</SageNotTaxSupported>");
                        sw.WriteLine("\t\t\t<SageVatType>" + vat.SageVatType + "</SageVatType>");
                        sw.WriteLine("\t\t\t<SageMarket>" + vat.SageMarket + "</SageMarket>");
                        sw.WriteLine("\t\t\t<SageTypeOfGood>" + vat.SageTypeOfGood + "</SageTypeOfGood>");
                        sw.WriteLine("\t\t\t<SageVatPercentage>" + vat.SageVatPercentage + "</SageVatPercentage>");
                        sw.WriteLine("\t\t</SageVAT>");
                    }

                    // SageCostCenter
                    sw.WriteLine("\t\t<!-- Centros de Custo de Gestão Comercial -->");
                    foreach (var cc in sageCostCenters)
                    {
                        sw.WriteLine("\t\t<SageCostCenter>");
                        sw.WriteLine("\t\t\t<SageCostCenterID>" + cc.SageCostCenterID + "</SageCostCenterID>");
                        sw.WriteLine("\t\t\t<SageDescription>" + cc.SageDescription + "</SageDescription>");
                        sw.WriteLine("\t\t</SageCostCenter>");
                    }

                    // SageJournal
                    sw.WriteLine("\t\t<!-- Diários de Gestão Comercial -->");
                    foreach (var jrn in sageJournals)
                    {
                        sw.WriteLine("\t\t<SageJournal>");
                        sw.WriteLine("\t\t\t<SageJournalID>" + jrn.SageJournalID + "</SageJournalID>");
                        sw.WriteLine("\t\t\t<SageDescription>" + jrn.SageDescription + "</SageDescription>");
                        sw.WriteLine("\t\t\t<SageNumeration>" + jrn.SageNumeration + "</SageNumeration>");
                        sw.WriteLine("\t\t\t<SageOrigin>" + jrn.SageOrigin + "</SageOrigin>");
                        sw.WriteLine("\t\t\t<SageType>" + jrn.SageType + "</SageType>");
                        sw.WriteLine("\t\t</SageJournal>");
                    }

                    // SageDocumentType
                    sw.WriteLine("\t\t<!-- Tipos de Documento -->");
                    foreach (var type in sageDocumentTypes)
                    {
                        sw.WriteLine("\t\t<SageDocumentType>");
                        sw.WriteLine("\t\t\t<SageDocumentTypeID>" + type.SageDocumentTypeID + "</SageDocumentTypeID>");
                        sw.WriteLine("\t\t\t<SageDescription>" + type.SageDescription + "</SageDescription>");
                        sw.WriteLine("\t\t</SageDocumentType>");
                    }

                    sw.WriteLine("\t</MasterFiles>");



                    // GeneralLedgerEntries (informação sobre cada documento no ficheiro input)
                    sw.WriteLine("\t<GeneralLedgerEntries xmlns=\"urn:OECD:StandardAuditFile-Tax:PT_1.03_01\">");

                    foreach (var j in sageJournals)
                    {
                        sw.WriteLine("\t\t<Journal>");
                        sw.WriteLine("\t\t\t<JournalID>" + j.SageJournalID + "</JournalID>");
                        sw.WriteLine("\t\t\t<Description>" + j.SageDescription + "</Description>");

                        int numDiario = 1;

                        foreach (var doc in documents)
                        {
                            sw.WriteLine("\t\t\t<Transaction>");

                            sw.WriteLine("\t\t\t\t<TransactionID>" + doc.Data_Lanc + " " + j.SageJournalID + " " + numDiario + "</TransactionID>");
                            sw.WriteLine("\t\t\t\t<TransactionDate>" + doc.Data_Lanc + "</TransactionDate>");
                            sw.WriteLine("\t\t\t\t<DocArchivalNumber>" + numDiario++ + "</DocArchivalNumber>");
                            sw.WriteLine("\t\t\t\t<TransactionType>N</TransactionType>");
                            sw.WriteLine("\t\t\t\t<GLPostingDate>" + this.DataLanc + "</GLPostingDate>");

                            if (doc.EntityType == "S")
                            {
                                sw.WriteLine("\t\t\t\t<SupplierID>" + doc.EntityID + "</SupplierID>");
                            }
                            else if (doc.EntityType == "C")
                            {
                                sw.WriteLine("\t\t\t\t<CustomerID>" + doc.EntityID + "</CustomerID>");
                            }

                            sw.WriteLine("\t\t\t\t<SageExtendedData>");
                            sw.WriteLine("\t\t\t\t\t<SageDocumentTypeID>" + TipoDoc + "</SageDocumentTypeID>");
                            sw.WriteLine("\t\t\t\t\t<SageDocumentSeries>1</SageDocumentSeries>");
                            sw.WriteLine("\t\t\t\t\t<SageDocumentNumber>" + doc.Doc_Numero + "</SageDocumentNumber>");
                            sw.WriteLine("\t\t\t\t\t<SageTransactionType>NOR</SageTransactionType>");
                            sw.WriteLine("\t\t\t\t\t<SageTransactionStatus>NOR</SageTransactionStatus>");
                            sw.WriteLine("\t\t\t\t</SageExtendedData>");

                            foreach (var trans in doc.docTransactions)
                            {
                                sw.WriteLine("\t\t\t\t<Line>");

                                sw.WriteLine("\t\t\t\t\t<RecordID>" + trans.Doc_Ordem + "</RecordID>");
                                sw.WriteLine("\t\t\t\t\t<AccountID>" + trans.Cta_Conta + "</AccountID>");
                                sw.WriteLine("\t\t\t\t\t<SystemEntryDate>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "</SystemEntryDate>");
                                sw.WriteLine("\t\t\t\t\t<Description>" + trans.Doc_Numero + "</Description>");
                                if (trans.Val_Valor.StartsWith("-"))
                                {
                                    sw.WriteLine("\t\t\t\t\t<CreditAmount>" + trans.Val_Valor.Substring(1) + "</CreditAmount>");
                                }
                                else
                                {
                                    sw.WriteLine("\t\t\t\t\t<DebitAmount>" + trans.Val_Valor + "</DebitAmount>");
                                }
                                sw.WriteLine("\t\t\t\t\t<SageExtendedData>");
                                sw.WriteLine("\t\t\t\t\t\t<SageAuxType>" + trans.SageAuxType + "</SageAuxType>");
                                sw.WriteLine("\t\t\t\t\t\t<SageControl>" + trans.SageControl + "</SageControl>");
                                if (trans.SageVatID != "")
                                {
                                    sw.WriteLine("\t\t\t\t\t\t<SageVatID>" + trans.SageVatID + "</SageVatID>");
                                }
                                if (trans.Cta_NrCCust != "")
                                {
                                    sw.WriteLine("\t\t\t\t\t\t<SageCostCenter>");
                                    sw.WriteLine("\t\t\t\t\t\t\t<SageCostCenterID>" + trans.Cta_NrCCust + "</SageCostCenterID>");
                                    sw.WriteLine("\t\t\t\t\t\t\t<SageCostCenterAmount>" + (trans.Val_Valor.StartsWith("-") ? trans.Val_Valor.Substring(1) : trans.Val_Valor) + "</SageCostCenterAmount>");
                                    sw.WriteLine("\t\t\t\t\t\t</SageCostCenter>");
                                }
                                sw.WriteLine("\t\t\t\t\t</SageExtendedData>");

                                sw.WriteLine("\t\t\t\t</Line>");
                            }

                            sw.WriteLine("\t\t\t</Transaction>");
                        }

                        sw.WriteLine("\t\t</Journal>");
                    }

                    sw.WriteLine("\t</GeneralLedgerEntries>");



                    sw.WriteLine("</AuditFile>");
                    // FIM DO XML

                    sw.Close();
                }
            }
            catch (Exception ex)
            {

                //ExceptionUtility.WriteErrorToLogFile(ex);
                errors.RemoveAll(x => x.ErrorMsg != "");
                errors.Add(new ErrorLine { AccountID = "", ErrorMsg = "O ficheiro Excel contém células com informação inválida" });
                throw ex;
            }
        }

        private string EncodeStringToXML(string str)
        {
            return str.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("\'", "&apos;");
        }
    }
}
