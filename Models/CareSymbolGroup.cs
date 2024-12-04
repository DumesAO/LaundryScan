using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryScan.Models
{
    public class CareSymbolGroup
    {
        
        

        [PrimaryKey, AutoIncrement]
        [Display(AutoGenerateField = false)]
        public int ID { get; set; }
        [NotNull]
        public string Name { get; set; }

        [Ignore]
        public List<CareSymbol> CareSymbols { get; set; }

        [Ignore]
        public ObservableCollection<object> Selection { get; set; } = [];
    }
}
