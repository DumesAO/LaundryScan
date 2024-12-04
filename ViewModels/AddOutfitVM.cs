using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LaundryScan.Models;
using LaundryScan.Views;

namespace LaundryScan.ViewModels
{
    public partial class AddOutfitVM : ObservableObject
    {
        [ObservableProperty] private string name;
        [ObservableProperty] private ObservableCollection<ClothingItem> selectedClothingItems=[];
        public AddOutfitVM(AddOutfitPage page)
        {
            this.Page = page;
        }
        AddOutfitPage Page { get; }
        [RelayCommand]
        private async Task AddNewItem()
        {
            await Page.Navigation.PushAsync(new SelectClothingItemsPage());
        }
        [RelayCommand]
        private void RemoveClothingItem(ClothingItem item)
        {
            if (SelectedClothingItems.Contains(item))
            {
                SelectedClothingItems.Remove(item);
            }
        }
        [RelayCommand]
        private async Task CreateOutfit()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var outfit = new Outfit();
                outfit.Name = Name;
                foreach (var item in SelectedClothingItems) 
                {
                    outfit.AddClothingItem(item);
                }
                App.Database.SaveOutfit(outfit);
                await Page.DisplayAlert("Успіх", "Новий набір створено!", "OK");
                await Page.Navigation.PushAsync(new MainPage());
            }
        }
    }
}
