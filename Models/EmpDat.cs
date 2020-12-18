using System;
using System.Collections.Generic;

#nullable disable

namespace toDoList.Models
{
    public partial class EmpDat
    {
        public string Cemp { get; set; }
        public string Capl { get; set; }
        public short AnoIn { get; set; }
        public short AnoFi { get; set; }
        public string DataDir { get; set; }
        public string Server { get; set; }
        public string SharedDir { get; set; }
        public string Estado { get; set; }
    }
}
