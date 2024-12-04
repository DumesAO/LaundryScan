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
    public partial class SelectCareSymbolsVM : ObservableObject
    {
        public SelectCareSymbolsVM(SelectCareSymbolsPage page, ClothingItem clothingItem)
        {
            this.Page = page;
            this.clothingItem = clothingItem;
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
        private ClothingItem clothingItem;
        public SelectCareSymbolsPage Page { get;}
        public ObservableCollection<CareSymbolGroup> CareSymbolGroups { get; } = [];
        [RelayCommand]
        public async Task SelectSymbols()
        {
            var selection=CareSymbolGroups.SelectMany(x => x.Selection).Cast<CareSymbol>().ToList();
            if (selection.Count == 0) return;
            await Page.Navigation.PushAsync(new AddClothPage(selection,clothingItem));
            foreach(var group in CareSymbolGroups)
            {
                group.Selection.Clear();
            }
        }
    }
}
