using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace toDoList
{
    public class ASCFileLine
    {
        public string Cta_Conta { get; set; }
        public string Cta_NrCCust { get; set; }
        public string Diar_Mes { get; set; }
        public string Diar_Numero { get; set; }
        public string Data_Lanc { get; set; }
        public string Doc_Tipo { get; set; }
        public string Doc_Numero { get; set; }
        public string Doc_Ordem { get; set; }
        public string Val_Valor { get; set; }
        public string Lan_Refer { get; set; }
    }

    public class ExcelFileLine
    {
        public string Conta_Creditar { get; set; }
        public string Conta_Debitar { get; set; }
        public string NIF { get; set; }
        public string Centro_Custo { get; set; }
        public string Data_Lanc { get; set; }
        public string Num_Doc { get; set; }
        public string Valor { get; set; }
    }

    public class UberCSVFileLine
    {
        public string Num_Fatura { get; set; }
        public string Data_Fatura { get; set; }
        public string Exig_IVA { get; set; }
        public string Nome_Fornecedor { get; set; }
        public string NIF_Fornecedor { get; set; }
        public string Taxa_Serv1 { get; set; }
        public string GST_Taxa_Serv { get; set; }
        public string Taxa_Serv2 { get; set; }
        public string Desc_Liq { get; set; }
        public string GST_Desc { get; set; }
        public string Desc_Bruto { get; set; }
        public string Taxa_Merc_Liq { get; set; }
        public string GST_Taxa_Merc { get; set; }
        public string Taxa_Serv_Bruta { get; set; }
    }

    public class DriverCSVFileLine
    {
        public string ID_Fluxo { get; set; }
        public string Num_Fatura { get; set; }
        public string Data_Fatura { get; set; }
        public string Exig_IVA { get; set; }
        public string Nome_Fornecedor { get; set; }
        public string Morada_Fornecedor { get; set; }
        public string CodPostal_Fornecedor { get; set; }
        public string Cidade_Fornecedor { get; set; }
        public string Perc_IVA { get; set; }
        public string Desc_Artigo { get; set; }
        public string NIF_Fornecedor { get; set; }
        public string Valor_Liquido { get; set; }
        public string Valor_IVA { get; set; }
        public string Valor_Bruto { get; set; }
    }

    public class GeneralLedger
    {
        public string AccountID { get; set; }
        public string AccountDescription { get; set; }
        public string SageAuxType { get; set; }
        public string ContaIVA { get; set; }
    }

    public class Customer
    {
        public string AccountID { get; set; }
        public string CustomerID { get; set; }
        public string CustomerTaxID { get; set; }
        public string CompanyName { get; set; }
    }

    public class Supplier
    {
        public string AccountID { get; set; }
        public string SupplierID { get; set; }
        public string SupplierTaxID { get; set; }
        public string CompanyName { get; set; }
    }

    public class SageVAT
    {
        public string SageVatID { get; set; }
        public string SageDescription { get; set; }
        public string SageTaxPayableAccount { get; set; }
        public string SageRecapitulative { get; set; }
        public string SageReverseCharge { get; set; }
        public string SageNotTaxSupported { get; set; }
        public string SageVatType { get; set; }
        public string SageMarket { get; set; }
        public string SageTypeOfGood { get; set; }
        public string SageVatPercentage { get; set; }
    }

    public class SageCashFlow
    {
        public string SageCashFlowID { get; set; }
        public string SageDescription { get; set; }
        public string SageNature { get; set; }
        public string SageInOut { get; set; }
        public string SageEntity { get; set; }
    }

    public class SageBudgetLine
    {
        public string SageBudgetLineID { get; set; }
        public string SageDescription { get; set; }
    }

    public class SageDefrayal
    {
        public string SageDefrayalID { get; set; }
        public string SageDescription { get; set; }
    }

 public    class SageSector
    {
        public string SageSectorID { get; set; }
        public string SageDescription { get; set; }
    }

 public    class SageCostCenter
    {
        public string SageCostCenterID { get; set; }
        public string SageDescription { get; set; }
    }

  public   class SageJournal
    {
        public string SageJournalID { get; set; }
        public string SageDescription { get; set; }
        public string SageNumeration { get; set; }
        public string SageOrigin { get; set; }
        public string SageType { get; set; }
    }

  public   class SageDocumentType
    {
        public string SageDocumentTypeID { get; set; }
        public string SageDescription { get; set; }
    }

  public   class Document
    {
        public string Doc_Numero { get; set; }
        public string Doc_Tipo { get; set; }
        public string Data_Lanc { get; set; }
        public string Diar_Mes { get; set; }
        public string Diar_Numero { get; set; }
        public string Doc_Categoria { get; set; }  // CM - Compra;   PGT - Pagamento;   VN - Venda
        public string EntityID { get; set; }       // CustomerID ou SupplierID, dependendo da Doc_Categoria
        public string EntityType { get; set; }     // C - Customer;   S - Supplier
        public string SageVatID { get; set; }
        public string SageVatDC { get; set; }      // D - Débito;   C - Crédito

        public List<Transaction> docTransactions;

        public Document(string numero)
        {
            Doc_Numero = numero;
            Doc_Categoria = "VN";
            EntityType = "";
            EntityID = "";
            SageVatID = "";
            SageVatDC = "";
            docTransactions = new List<Transaction>();
        }

        public void AddTransaction(Transaction trans)
        {
            if (trans != null)
            {
                docTransactions.Add(trans);

                Data_Lanc = trans.Data_Lanc;

                trans.Lan_Refer = trans.Lan_Refer.Replace("&", "&amp;");

                if (trans.Cta_Conta.StartsWith("21"))
                {
                    EntityType = "C";
                }
                else if (trans.Cta_Conta.StartsWith("22"))
                {
                    EntityType = "S";
                    if (trans.Val_Valor.StartsWith("-"))
                    {
                        Doc_Categoria = "PGT";
                    }
                    else
                    {
                        Doc_Categoria = "CM";
                    }
                }

                if (trans.Cta_Conta.StartsWith("243"))
                {
                    trans.SageControl = "VLRIVA";
                    SageVatID = trans.Cta_Conta.Length > 7 ? trans.Cta_Conta.Substring(3, 4) : trans.Cta_Conta.Substring(3);
                    if (trans.Val_Valor.StartsWith("-"))
                    {
                        SageVatDC = "C";
                    }
                    else
                    {
                        SageVatDC = "D";
                    }
                }
                //else if (trans.Cta_Conta.StartsWith("31") || trans.Cta_Conta.StartsWith("71"))
                //{
                //    trans.SageControl = "BASIVA";
                //}
            }
        }

        // AVISO: apenas deve ser chamado depois de TODAS as transacções de TODOS os documentos tiverem sido adicionados!
        public void Set_SageAuxTypes()
        {
            if (docTransactions.Exists(x => x.Cta_Conta.StartsWith("12")))
            {
                var trans = docTransactions.Find(x => x.Cta_Conta.StartsWith("12"));
                trans.SageAuxType = "TES";

                if (Doc_Categoria != "PGT")
                {
                    trans.SageControl = "TES";
                }
            }
            else if (docTransactions.Exists(x => x.Cta_Conta.StartsWith("22")))
            {
                var trans = docTransactions.Find(x => x.Cta_Conta.StartsWith("22"));
                trans.SageAuxType = "FOR";
            }
            else if (docTransactions.Exists(x => x.Cta_Conta.StartsWith("21")))
            {
                var trans = docTransactions.Find(x => x.Cta_Conta.StartsWith("21"));
                trans.SageAuxType = "CLI";
            }
        }

        // AVISO: apenas deve ser chamado depois de TODAS as transacções de TODOS os documentos tiverem sido adicionados!
        public void Set_SageControl()
        {
            if (SageVatDC == "D")
            {
                foreach (var trans in docTransactions.TakeWhile(x => !x.Cta_Conta.StartsWith("243") && !x.Val_Valor.StartsWith("-")))
                {
                    trans.SageControl = "BASIVA";
                }
            }
            else if (SageVatDC == "C")
            {
                foreach (var trans in docTransactions.TakeWhile(x => !x.Cta_Conta.StartsWith("243") && x.Val_Valor.StartsWith("-")))
                {
                    trans.SageControl = "BASIVA";
                }
            }
        }
    }

  public   class Transaction
    {
        public string Cta_Conta { get; set; }
        public string Cta_NrCCust { get; set; }
        public string Doc_Numero { get; set; }
        public string Doc_Ordem { get; set; }
        public string Data_Lanc { get; set; }
        public string Diar_Mes { get; set; }
        public string Diar_Numero { get; set; }
        public string Val_Valor { get; set; }
        public string Lan_Refer { get; set; }
        public string SageAuxType { get; set; }
        public string SageControl { get; set; }
        public string SageVatID { get; set; }

        public Transaction()
        {
            SageAuxType = "NA";
            SageControl = "OUT";
            SageVatID = "";
        }
    }

   public class ErrorLine
    {
        public string AccountID { get; set; }
        public string ErrorMsg { get; set; }
    }
}