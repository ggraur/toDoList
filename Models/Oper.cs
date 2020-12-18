using System;
using System.Collections.Generic;

#nullable disable

namespace toDoList.Models
{
    public partial class Oper
    {
        public string Coper { get; set; }
        public string Nome { get; set; }
        public byte Nivel { get; set; }
        public string PassE { get; set; }
        public string Email { get; set; }
        public bool EpassForte { get; set; }
        public bool Inact { get; set; }
    }
}
