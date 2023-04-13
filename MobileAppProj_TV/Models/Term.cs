using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileAppProj_TV.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]

        public int Id { get; set; }
        public String TermTitle { get; set; }
        public DateTime TermStart { get; set; }
        public DateTime TermEnd { get; set; }
    }


}

