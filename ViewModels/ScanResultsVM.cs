using CommunityToolkit.Mvvm.ComponentModel;
using LaundryScan.Models;
using LaundryScan.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryScan.ViewModels
{
    public partial class ScanResultsVM : ObservableObject
    {
        public ScanResultsVM(ScanResultsPage page, List<string> scanResults) 
        {
            this.Page = page;
            foreach (var item in scanResults) 
            {
                CareSymbol careSymbol = App.Database.GetCareSymbolByCode(item);
                if (careSymbol != null) {
                    CareSymbols.Add(careSymbol);
                }

                }
        }
        public ScanResultsPage Page { get; set; }
        public ObservableCollection<CareSymbol> CareSymbols { get; } = [];
    }
}
