using CommunityToolkit.Mvvm.ComponentModel;
using LaundryScan.Models;
using System.Collections.ObjectModel;
using LaundryScan.Views;
using CommunityToolkit.Mvvm.Input;
namespace LaundryScan.ViewModels
{
    public partial class CareSymbolsVM : ObservableObject
    {
        public CareSymbolsVM(CareSymbolsPage page)
        {
            this.Page = page;
            GetData();
        }
        void GetData()
        {
            var careSymbolGroups = App.Database.GetCareSymbolsGroups();
            foreach (CareSymbolGroup careSymbolGroup in careSymbolGroups)
            {
                CareSymbolGroups.Add(careSymbolGroup);
            }
        }
        public CareSymbolsPage Page { get; }
        [RelayCommand]
        private async Task ShowDetails()
        {
            CareSymbol symbol = null;
            foreach (CareSymbolGroup careSymbolGroup in CareSymbolGroups)
            {
                if (careSymbolGroup.Selection.Count == 1)
                {
                    symbol = (CareSymbol)careSymbolGroup.Selection[0];
                    careSymbolGroup.Selection.Clear();
                }
            }
            if (symbol == null) return;
            await Page.Navigation.PushAsync(new CareSymbolDetailPage(symbol));

            
            
        }
        
        public ObservableCollection<CareSymbolGroup> CareSymbolGroups { get; } = [];


    }
}
