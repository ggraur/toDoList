using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using toDoClassLibrary47;
using toDoList.Models;
using toDoList.ViewModels;

namespace toDoList
{
   public class ConverterASCtoXML
    {
        private List<ASCFileLine> data;
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
        private string ASCFilePath;
        private string XMLFilePath;
        private int NIF;
        private int Year;
        private int Month;
       
        private string CodEmpSage;
        private string EmpSageCon;
        private Empr empr;
        public List<ErrorLine> Errors = new List<ErrorLine>();
        private StreamReader streamReader;
        public ConverterASCtoXML(StreamReader _streamReader,
                                CLabViewModel model, 
                                string _Month,
                                string db,
                                DadosEmpresaImportada dadosEmpresaView, 
                                ConConfigViewModel conConfigViewModel, 
                                EmpresasViewModel empresaViewModel, 
                                Empr _empr)
        {
            streamReader = _streamReader;
            data = new List<ASCFileLine>();
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
            ASCFilePath = model.strInputFilePath;
            XMLFilePath = model.OutputFilePAth;//XMLpath;
            //string[] ASCpathArray = ASCpath.Split('\\');
            //string fileName = ASCpathArray[ASCpathArray.Length - 1];
            //string[] fileNameArray = fileName.Split('.');
            //string fileNameNoExt = fileNameArray[0];
            //int index = fileNameNoExt.IndexOf(" (");
            //string fileNameNoExtRep = index >= 0 ? fileNameNoExt.Remove(index) : fileNameNoExt;
            //string[] fileNameNoExtArray = fileNameNoExtRep.Split('_');
            NIF = Int32.Parse(empresaViewModel.NIF); // Convert.ToInt32(fileNameNoExtArray[0]);
            Year =model.Ano;//Convert.ToInt32(fileNameNoExtArray[fileNameNoExtArray.Length - 2]);
            Month = Convert.ToInt32(_Month);
            //this.LocalCon = LocalCon;
            this.CodEmpSage = dadosEmpresaView.CodeAplicacao ;//CodEmpSage;

            EncryptionHelper encryptionHelper = new EncryptionHelper();
            //clsCheckValidData
            this.EmpSageCon = "Data Source=" + conConfigViewModel.NomeServidor +
                                      ";Initial Catalog=" + db +
                                      ";Persist Security Info=True" +
                                      ";User ID=" + conConfigViewModel.Utilizador +
                                      ";Password=" + encryptionHelper.Decrypt(conConfigViewModel.Password) + ";"; 
            
            this.Errors = new List<ErrorLine>();
            empr = _empr;
        }

        public void ConvertFile(string outputPath)
        {
            try
            {
                StreamReader ASCfile = streamReader;
                //using (StreamReader ASCfile = new StreamReader(ASCFilePath, Encoding.GetEncoding("iso-8859-1")))
                using (ASCfile)
                {
                    int rowNumber = 1;

                    string lineString;

             //      string str = ASCfile.ReadLine();

                    ASCfile.ReadLine();

                    // ler o cabeçalho e passar para a linha seguinte
                    while ((lineString = ASCfile.ReadLine()) != null)
                    {
                        if (lineString == "")
                        {
                            break;
                        }

                        string[] lineArray = lineString.Split('\t');

                        ASCFileLine line = new ASCFileLine();
                        line.Cta_Conta = lineArray[0].ToString();
                        line.Cta_NrCCust = lineArray[1].ToString();
                        line.Diar_Mes = lineArray[2].ToString();
                        line.Diar_Numero = lineArray[3].ToString();
                        line.Data_Lanc = lineArray[4].ToString();
                        line.Doc_Tipo = lineArray[5].ToString();
                        line.Doc_Numero = lineArray[6].ToString();
                        line.Doc_Ordem = lineArray[7].ToString();
                        line.Val_Valor = lineArray[8].ToString();
                        line.Lan_Refer = EncodeStringToXML(lineArray[9].ToString());

                        if (EmpSageCon != "")
                        {
                            using (SqlConnection con = new SqlConnection(EmpSageCon))
                            {
                                con.Open();

                                string selectAccount_query = "select * from Conta where CConta = '" + line.Cta_Conta + "'";
                                SqlCommand selectAccount = new SqlCommand(selectAccount_query, con);
                                var reader = selectAccount.ExecuteReader();

                                if (!reader.HasRows)
                                {
                                    if (!Errors.Exists(x => x.AccountID.Equals(line.Cta_Conta) && x.ErrorMsg.Equals("Conta não existe")))
                                    {
                                        Errors.Add(new ErrorLine { AccountID = line.Cta_Conta, ErrorMsg = "Conta não existe" });
                                    }
                                }
                                else
                                {
                                    while (reader.Read())
                                    {
                                        string EdeMov = reader["EdeMov"].ToString();
                                        if (EdeMov == "False")
                                        {
                                            if (!Errors.Exists(x => x.AccountID.Equals(line.Cta_Conta) && x.ErrorMsg.Equals("Conta não é de movimento")))
                                            {
                                                Errors.Add(new ErrorLine { AccountID = line.Cta_Conta, ErrorMsg = "Conta não é de movimento" });
                                            }
                                        }
                                    }
                                }

                                reader.Close();

                                con.Close();
                                con.Dispose();
                            }
                        }

                        data.Add(line);

                        if (!accounts.Exists(x => x.AccountID.Equals(line.Cta_Conta)))
                        {
                            if (line.Cta_Conta.StartsWith("12"))
                            {
                                accounts.Add(new GeneralLedger { AccountID = line.Cta_Conta, AccountDescription = line.Lan_Refer, SageAuxType = "TES" });
                            }
                            else if (line.Cta_Conta.StartsWith("21"))
                            {
                                accounts.Add(new GeneralLedger { AccountID = line.Cta_Conta, AccountDescription = line.Lan_Refer, SageAuxType = "CLI" });

                                string nomeCliente = new clsCalculo().ReturnStr("select Nome from Terce where CConC = '" + line.Cta_Conta + "'", "Nome", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                                string nifCliente = new clsCalculo().ReturnStr("select CIFis from Terce where CConC = '" + line.Cta_Conta + "'", "CIFis", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));

                                if (nifCliente == "")
                                {
                                    if (!Errors.Exists(x => x.AccountID.Equals(line.Cta_Conta) && x.ErrorMsg.Equals("Cliente não existe")))
                                        Errors.Add(new ErrorLine { AccountID = line.Cta_Conta, ErrorMsg = "Cliente não existe" });
                                }

                                customers.Add(new Customer { AccountID = line.Cta_Conta, CustomerTaxID = nifCliente, CompanyName = EncodeStringToXML(nomeCliente), CustomerID = rowNumber.ToString() });
                            }
                            else if (line.Cta_Conta.StartsWith("22"))
                            {
                                accounts.Add(new GeneralLedger { AccountID = line.Cta_Conta, AccountDescription = line.Lan_Refer, SageAuxType = "FOR" });

                                string nomeFornecedor = new clsCalculo().ReturnStr("select Nome from Terce where CConF = '" + line.Cta_Conta + "'", "Nome", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                                string nifFornecedor = new clsCalculo().ReturnStr("select CIFis from Terce where CConF = '" + line.Cta_Conta + "'", "CIFis", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));

                                if (nifFornecedor == "")
                                {
                                    if (!Errors.Exists(x => x.AccountID.Equals(line.Cta_Conta) && x.ErrorMsg.Equals("Fornecedor não existe")))
                                        Errors.Add(new ErrorLine { AccountID = line.Cta_Conta, ErrorMsg = "Fornecedor não existe" });
                                }

                                suppliers.Add(new Supplier { AccountID = line.Cta_Conta, SupplierTaxID = nifFornecedor, CompanyName = EncodeStringToXML(nomeFornecedor), SupplierID = rowNumber.ToString() });
                            }
                            else
                            {
                                if (line.Cta_Conta.StartsWith("243"))
                                {
                                    if (!accounts.Exists(x => x.AccountID.Equals(line.Cta_Conta)))
                                    {
                                        accounts.Add(new GeneralLedger { AccountID = line.Cta_Conta, AccountDescription = line.Lan_Refer, SageAuxType = "NA" });

                                        using (SqlConnection con = new SqlConnection(EmpSageCon))
                                        {
                                            con.Open();

                                            string selectSede_query = "select Valor from ParEmp where CPar = 'Sede'";
                                            SqlCommand selectSede = new SqlCommand(selectSede_query, con);
                                            string sede = selectSede.ExecuteScalar().ToString();

                                            string selectContaIVA_query = "select IvaRg from Conta where CConta = '" + line.Cta_Conta + "' and EdeIva = 1 and IvaRg <> ''";
                                            SqlCommand selectContaIVA = new SqlCommand(selectContaIVA_query, con);
                                            var res = selectContaIVA.ExecuteScalar();

                                            if (res != null)
                                            {
                                                string Regime = res.ToString();
                                                string Taxa = "0";

                                                switch (sede)
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

                                                sageVats.Add(new SageVAT
                                                {
                                                    SageVatID = line.Cta_Conta.Length > 7 ? line.Cta_Conta.Substring(3, 4) : line.Cta_Conta.Substring(3),
                                                    SageDescription = line.Lan_Refer,
                                                    SageTaxPayableAccount = line.Cta_Conta,
                                                    SageRecapitulative = "True",
                                                    SageNotTaxSupported = "0",
                                                    SageVatType = "DED",
                                                    SageMarket = "EL0",
                                                    SageVatPercentage = Taxa
                                                });
                                                var vat = sageVats.Find(x => x.SageTaxPayableAccount.Equals(line.Cta_Conta));
                                                if (vat.SageVatID.StartsWith("21")) vat.SageTypeOfGood = "MERC";
                                                else if (vat.SageVatID.StartsWith("22")) vat.SageTypeOfGood = "IMOB";
                                                else if (vat.SageVatID.StartsWith("23")) vat.SageTypeOfGood = "OBS";
                                                else vat.SageTypeOfGood = "OUTROS";
                                            }
                                            else
                                            {
                                                Errors.Add(new ErrorLine { AccountID = line.Cta_Conta, ErrorMsg = "Conta não é de IVA" });
                                            }

                                            con.Close();
                                            con.Dispose();
                                        }
                                    }
                                }
                                else
                                {
                                    string sageAuxType = "NA";

                                    string nif = new clsCalculo().ReturnStr("select CIFis from Terce where CConC = '" + line.Cta_Conta + "'", "CIFis", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                                    string nome = new clsCalculo().ReturnStr("select Nome from Terce where CConC = '" + line.Cta_Conta + "'", "Nome", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));

                                    if (nif != "")
                                    {
                                        sageAuxType = "CLI";
                                        customers.Add(new Customer { AccountID = line.Cta_Conta, CustomerTaxID = nif, CompanyName = EncodeStringToXML(nome), CustomerID = rowNumber.ToString() });
                                    }

                                    nif = new clsCalculo().ReturnStr("select CIFis from Terce where CConF = '" + line.Cta_Conta + "'", "CIFis", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                                    nome = new clsCalculo().ReturnStr("select Nome from Terce where CConF = '" + line.Cta_Conta + "'", "Nome", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));

                                    if (nif != "")
                                    {
                                        sageAuxType = "FOR";
                                        suppliers.Add(new Supplier { AccountID = line.Cta_Conta, SupplierTaxID = nif, CompanyName = EncodeStringToXML(nome), SupplierID = rowNumber.ToString() });
                                    }

                                    accounts.Add(new GeneralLedger { AccountID = line.Cta_Conta, AccountDescription = line.Lan_Refer, SageAuxType = sageAuxType });
                                }
                            }
                        }

                        if (line.Cta_NrCCust != "" && !sageCostCenters.Exists(x => x.SageCostCenterID == line.Cta_NrCCust))
                        {
                            string CCeCu = new clsCalculo().ReturnStr("select CCeCu from CenCu where CCeCu = '" + line.Cta_NrCCust + "'", "CCeCu", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                            string Descr = new clsCalculo().ReturnStr("select Descr from CenCu where CCeCu = '" + line.Cta_NrCCust + "'", "Descr", clsConnectionConfig.GetConnection_CustomConnectionString(EmpSageCon));
                            if (CCeCu != "")
                                sageCostCenters.Add(new SageCostCenter { SageCostCenterID = CCeCu, SageDescription = Descr });
                            else
                            {
                                if (!Errors.Exists(x => x.AccountID == CCeCu && x.ErrorMsg == "Centro de custo não existe"))
                                    Errors.Add(new ErrorLine { AccountID = CCeCu, ErrorMsg = "Centro de custo não existe" });
                            }
                        }

                        if (!sageJournals.Exists(x => x.SageJournalID == line.Diar_Numero))
                        {
                            sageJournals.Add(new SageJournal { SageJournalID = line.Diar_Numero, SageDescription = "Diário " + line.Diar_Numero, SageNumeration = "MENS", SageOrigin = "MC", SageType = "NOR" });
                        }

                        if (!sageDocumentTypes.Exists(x => x.SageDocumentTypeID == line.Doc_Tipo))
                        {
                            sageDocumentTypes.Add(new SageDocumentType { SageDocumentTypeID = line.Doc_Tipo, SageDescription = "Documento Tipo " + line.Doc_Tipo });
                        }

                        if (!documents.Exists(x => x.Doc_Numero == line.Doc_Numero))
                        {
                            var newDoc = new Document(line.Doc_Numero);
                            newDoc.Doc_Tipo = line.Doc_Tipo;
                            newDoc.Data_Lanc = Convert.ToDateTime(line.Data_Lanc).ToString("dd-MM-yyyy");
                            newDoc.Diar_Mes = line.Diar_Mes;
                            newDoc.Diar_Numero = line.Diar_Numero;
                            documents.Add(newDoc);
                        }

                        var trans = new Transaction();
                        trans.Cta_Conta = line.Cta_Conta;
                        trans.Cta_NrCCust = sageCostCenters.Exists(x => x.SageCostCenterID == line.Cta_NrCCust) ? line.Cta_NrCCust : "";
                        trans.Diar_Mes = line.Diar_Mes;
                        trans.Diar_Numero = line.Diar_Numero;
                        trans.Data_Lanc = Convert.ToDateTime(line.Data_Lanc).ToString("dd-MM-yyyy");
                        trans.Doc_Numero = line.Doc_Numero;
                        trans.Doc_Ordem = (Convert.ToInt32(line.Doc_Ordem) + 1).ToString();
                        trans.Val_Valor = line.Val_Valor;
                        trans.Lan_Refer = line.Lan_Refer;

                        var doc = documents.Find(x => x.Doc_Numero == line.Doc_Numero);
                        if (trans.Cta_Conta.StartsWith("21"))
                        {
                            doc.EntityID = customers.Find(x => x.AccountID.Equals(trans.Cta_Conta)).CustomerID;
                        }
                        else if (trans.Cta_Conta.StartsWith("22"))
                        {
                            doc.EntityID = suppliers.Find(x => x.AccountID.Equals(trans.Cta_Conta)).SupplierID;
                        }
                        doc.AddTransaction(trans);

                        rowNumber++;
                    }
                    ASCfile.Close();
                }

                FileInfo uploadedFile = new FileInfo(ASCFilePath);
                uploadedFile.Delete();

                accounts.Sort(delegate (GeneralLedger x, GeneralLedger y)
                {
                    if (x.AccountID == null && y.AccountID == null) return 0;
                    else if (x.AccountID == null) return -1;
                    else if (y.AccountID == null) return 1;
                    else return x.AccountID.CompareTo(y.AccountID);
                });

                sageJournals.Sort(delegate (SageJournal x, SageJournal y)
                {
                    if (x.SageJournalID == null && y.SageJournalID == null) return 0;
                    else if (x.SageJournalID == null) return -1;
                    else if (y.SageJournalID == null) return 1;
                    else return x.SageJournalID.CompareTo(y.SageJournalID);
                });

                sageDocumentTypes.Sort(delegate (SageDocumentType x, SageDocumentType y)
                {
                    if (x.SageDocumentTypeID == null && y.SageDocumentTypeID == null) return 0;
                    else if (x.SageDocumentTypeID == null) return -1;
                    else if (y.SageDocumentTypeID == null) return 1;
                    else return x.SageDocumentTypeID.CompareTo(y.SageDocumentTypeID);
                });

                foreach (var doc in documents)
                {
                    doc.Set_SageAuxTypes();
                    doc.Set_SageControl();
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
                FileInfo XMLfile = new FileInfo(outputPath + "\\SAGE_ACCOUNTING_C-LAB_" + startDate.ToString("yyyyMMdd") + "_" + endDate.ToString("yyyyMMdd") + "_" + NIF + ".xml");

                using (StreamWriter sw = new StreamWriter(XMLfile.FullName, false, Encoding.GetEncoding("iso-8859-1")))
                {
                    // INÍCIO DO XML
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"Windows-1252\" standalone=\"no\"?>");
                    sw.WriteLine("<AuditFile xmlns=\"urn:OECD:StandardAuditFile-Tax:PT_1.03_01\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");

                    // Header
                    sw.WriteLine("\t<Header xmlns=\"urn:OECD:StandardAuditFile-Tax:PT_1.03_01\">");
                    sw.WriteLine("\t\t<AuditFileVersion>1.03_01</AuditFileVersion>");

                    //if (LocalCon != "" && CodEmpSage != "")
                    //{
                    //    using (SqlConnection con = new SqlConnection(LocalCon))
                    //    {
                            //con.Open();

                            //string command_string = "select * from Emprs where CEmp = '" + CodEmpSage + "' and NContrib = '" + NIF + "'";
                            //SqlCommand command = new SqlCommand(command_string, con);
                            //var reader = command.ExecuteReader();
                            //while (reader.Read())
                            //{
                                string _nome = empr.Nome ;// EncodeStringToXML(reader["Nome"].ToString());
                                string _morada = empr.Morada;//reader["Morada"].ToString();
                                string _codPostal = empr.CodPostal;//reader["CodPostal"].ToString();
                                string _localidade = empr.Localidade;//reader["Localidade"].ToString();
                                string _sede = empr.Sede;//reader["Sede"].ToString();

                                sw.WriteLine("\t\t<CompanyID>" + CodEmpSage + "</CompanyID>");
                                sw.WriteLine("\t\t<TaxRegistrationNumber>" + NIF + "</TaxRegistrationNumber>");
                                sw.WriteLine("\t\t<TaxAccountingBasis>" + _sede + "</TaxAccountingBasis>");
                                sw.WriteLine("\t\t<CompanyName>" + _nome + "</CompanyName>");
                                sw.WriteLine("\t\t<BusinessName>" + _nome + "</BusinessName>");
                                sw.WriteLine("\t\t<CompanyAdress>");
                                sw.WriteLine("\t\t\t<AddressDetail>" + _morada + "</AddressDetail>");
                                sw.WriteLine("\t\t\t<City>" + _localidade + "</City>");
                                sw.WriteLine("\t\t\t<PostalCode>" + _codPostal + "</PostalCode>");
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
                    sw.WriteLine("\t\t<!-- Clientes -->");
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
                    sw.WriteLine("\t\t<!-- Fornecedores -->");
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
                    sw.WriteLine("\t\t<!-- Outros -->");
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

                    // SageCashFlow
                    sw.WriteLine("\t\t<!-- Fluxos de Gestão Comercial -->");
                    foreach (var cf in sageCashFlows)
                    {
                        sw.WriteLine("\t\t<SageCashFlow>");

                        sw.WriteLine("\t\t\t<SageCashFlowID>" + cf.SageCashFlowID + "</SageCashFlowID>");
                        sw.WriteLine("\t\t\t<SageDescription>" + cf.SageDescription + "</SageDescription>");
                        sw.WriteLine("\t\t\t<SageNature>" + cf.SageNature + "</SageNature>");
                        sw.WriteLine("\t\t\t<SageInOut>" + cf.SageInOut + "</SageInOut>");
                        sw.WriteLine("\t\t\t<SageEntity>" + cf.SageEntity + "</SageEntity>");

                        sw.WriteLine("\t\t</SageCashFlow>");
                    }

                    // SageBudgetLine
                    sw.WriteLine("\t\t<!-- Rubricas de Gestão Comercial/de Pessoal -->");
                    foreach (var line in sageBudgetLines)
                    {
                        sw.WriteLine("\t\t<SageBudgetLine>");

                        sw.WriteLine("\t\t\t<SageBudgetLineID>" + line.SageBudgetLineID + "</SageBudgetLineID>");
                        sw.WriteLine("\t\t\t<SageDescription>" + line.SageDescription + "</SageDescription>");

                        sw.WriteLine("\t\t</SageBudgetLine>");
                    }

                    // SageDefrayal
                    sw.WriteLine("\t\t<!-- Custeios de Gestão Comercial -->");
                    foreach (var def in sageDefrayals)
                    {
                        sw.WriteLine("\t\t<SageDefrayal>");

                        sw.WriteLine("\t\t\t<SageDefrayalID>" + def.SageDefrayalID + "</SageDefrayalID>");
                        sw.WriteLine("\t\t\t<SageDescription>" + def.SageDescription + "</SageDescription>");

                        sw.WriteLine("\t\t</SageDefrayal>");
                    }

                    // SageSector
                    sw.WriteLine("\t\t<!-- Sectores de Gestão de Pessoal -->");
                    foreach (var sec in sageSectors)
                    {
                        sw.WriteLine("\t\t<SageSector>");

                        sw.WriteLine("\t\t\t<SageSectorID>" + sec.SageSectorID + "</SageSectorID>");
                        sw.WriteLine("\t\t\t<SageDescription>" + sec.SageDescription + "</SageDescription>");

                        sw.WriteLine("\t\t</SageSector>");
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

                    sw.WriteLine("\t</MasterFiles>\n\n\n");



                    // GeneralLedgerEntries (informação sobre cada documento no ficheiro input)
                    sw.WriteLine("\t<GeneralLedgerEntries xmlns=\"urn:OECD:StandardAuditFile-Tax:PT_1.03_01\">");



                    foreach (var journal in sageJournals)
                    {
                        sw.WriteLine("\t\t<Journal>");
                        sw.WriteLine("\t\t\t<JournalID>" + journal.SageJournalID + "</JournalID>");
                        sw.WriteLine("\t\t\t<Description>" + journal.SageDescription + "</Description>");

                        int numDiario = 1;

                        foreach (var doc in documents)
                        {
                            if (doc.Diar_Numero == journal.SageJournalID)
                            {
                                sw.WriteLine("\t\t\t<Transaction>");

                                sw.WriteLine("\t\t\t\t<TransactionID>" + doc.Data_Lanc + " " + journal.SageJournalID + " " + numDiario + "</TransactionID>");
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
                                sw.WriteLine("\t\t\t\t\t<SageDocumentTypeID>" + doc.Doc_Tipo + "</SageDocumentTypeID>");
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
                                    sw.WriteLine("\t\t\t\t\t<Description>" + trans.Lan_Refer + "</Description>");
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
                                    if (trans.Cta_Conta.StartsWith("243"))
                                    {
                                        sw.WriteLine("\t\t\t\t\t\t<SageVatID>" + doc.SageVatID + "</SageVatID>");
                                    }
                                    else
                                    {
                                        if (trans.Val_Valor.StartsWith("-") && doc.SageVatDC == "C")
                                        {
                                            sw.WriteLine("\t\t\t\t\t\t<SageVatID>" + doc.SageVatID + "</SageVatID>");
                                        }
                                        else if (!trans.Val_Valor.StartsWith("-") && doc.SageVatDC == "D")
                                        {
                                            sw.WriteLine("\t\t\t\t\t\t<SageVatID>" + doc.SageVatID + "</SageVatID>");
                                        }
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
                        }


                        sw.WriteLine("\t\t</Journal>");
                    }
                    

                    sw.WriteLine("\t</GeneralLedgerEntries>");



                    sw.WriteLine("</AuditFile>");
                    // FIM DO XML



                    sw.Close();

                    Errors.Sort(delegate (ErrorLine x, ErrorLine y)
                    {
                        if (x.AccountID == null && y.AccountID == null) return 0;
                        else if (x.AccountID == null) return -1;
                        else if (y.AccountID == null) return 1;
                        else return x.AccountID.CompareTo(y.AccountID);
                    });
                }

            }
            catch (Exception ex)
            {
               // ExceptionUtility.WriteErrorToLogFile(ex);
                throw ex;
            }
        }

        private string EncodeStringToXML(string str)
        {
            return str.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("\'", "&apos;");
        }
    }
}
