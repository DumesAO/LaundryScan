using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public partial class SelectNewLabelVM : ObservableObject
    {
        public SelectNewLabelVM(SelectNewLabelPage page, ClothingItem clothingItem)
        {
            this.Page = page;
            this.ClothingItem = clothingItem;
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
        public SelectNewLabelPage Page { get; }
        public ClothingItem ClothingItem { get; }
        [RelayCommand]
        private async Task AddSymbol()
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
            if(ClothingItem.CareSymbols.Contains(symbol)) await Page.Navigation.PushAsync(new RedactClothingItemPage(ClothingItem));
            else
            {
                ClothingItem.CareSymbols.Add(symbol);
                await Page.Navigation.PushAsync(new RedactClothingItemPage(ClothingItem));
            }
        }

        public ObservableCollection<CareSymbolGroup> CareSymbolGroups { get; } = [];
    }
}
