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
    public partial class RedactOutfitVM : ObservableObject
    {
        public RedactOutfitVM(RedactOutfitPage page, Outfit outfit)
        {
            this.Page = page;
            this.Outfit = outfit;
            Name = Outfit.Name;
            GetData();
        }

        [ObservableProperty] private string name;
        [ObservableProperty] private ObservableCollection<ClothingItem> clothingItems = [];
        [ObservableProperty] private Outfit outfit;

        public void GetData()
        {
            ClothingItems.Clear();
            foreach (ClothingItem clothingItem in Outfit.ClothingItems)
            {
                ClothingItems.Add(clothingItem);
            }
        }
        public RedactOutfitPage Page { get; }

        [RelayCommand]
        private async Task AddNewItem()
        {
            Outfit.Name = Name;
            Outfit.ClothingItems = ClothingItems.ToList();
            await Page.Navigation.PushAsync(new SelectNewItemPage(Outfit));
        }
        [RelayCommand]
        private void RemoveClothingItem(ClothingItem item)
        {
            if (ClothingItems.Contains(item))
            {
                ClothingItems.Remove(item);
            }
        }
        [RelayCommand]
        private async Task UpdateOutfit()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                Outfit.Name = Name;
                Outfit.ClothingItems=ClothingItems.ToList();
                App.Database.SaveOutfit(Outfit);
                await Page.DisplayAlert("Успіх", "Дані оновлено!", "OK");
                await Page.Navigation.PushAsync(new OutfitDetailPage(Outfit));
            }
        }
    }
}
