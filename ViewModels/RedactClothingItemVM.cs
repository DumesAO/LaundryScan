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
    public partial class RedactClothingItemVM : ObservableObject
    {
        public RedactClothingItemVM( RedactClothingItemPage page, ClothingItem clothingItem) 
        {
            this.Page = page;
            this.ClothiItem = clothingItem;
            GetData();
            Name = ClothiItem.Name;
            
        }
        [ObservableProperty] private string name;
        [ObservableProperty] private ObservableCollection<CareSymbol> careSymbols = [];
        [ObservableProperty] private Material material;
        [ObservableProperty] private Category category;
        [ObservableProperty] private ClothingItem clothiItem;
        RedactClothingItemPage Page { get; }
        public ObservableCollection<Category> Categories { get; } = new();
        public ObservableCollection<Material> Materials { get; } = new();
        void GetData()
        {
            var categories = App.Database.GetCategories();
            var materials = App.Database.GetMaterials();
            CareSymbols= new ObservableCollection<CareSymbol>(ClothiItem.CareSymbols);

            Categories.Clear();
            Materials.Clear();
            foreach (var category in categories) { 

                Categories.Add(category);
                if (category.ID == ClothiItem.Category.ID)
                {
                    this.Category=Categories.Last();
                }
            }
                
            foreach (var material in materials)
            {
                Materials.Add(material);
                if (material.ID == ClothiItem.Material.ID)
                {
                    this.Material = Materials.Last();
                }
            }
        }

        [RelayCommand]
        private async Task AddNewLabel()
        {
            await Page.Navigation.PushAsync(new SelectNewLabelPage(ClothiItem));
        }
        [RelayCommand]
        private void RemoveCareSymbol(CareSymbol item)
        {
            if (CareSymbols.Contains(item))
            {
                CareSymbols.Remove(item);
            }
        }
        [RelayCommand]
        private async Task UpdateCloth()
        {
            if (!string.IsNullOrEmpty(Name) && Material!=null && Category!=null)
            {
                ClothiItem.Name = Name;
                ClothiItem.CategoryID = Category.ID;
                ClothiItem.MaterialID = Material.ID;
                ClothiItem.CareSymbols = CareSymbols.ToList();
                App.Database.SaveClothingItem(ClothiItem);
                await Page.DisplayAlert("Успіх", "Дані оновлено!", "OK");
                await Page.Navigation.PushAsync(new ClothingItemDetailPage(ClothiItem));
            }
        }

    }
}
