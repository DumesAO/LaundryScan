using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LaundryScan.Models;
using LaundryScan.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryScan.ViewModels
{
    public partial class CothingItemDetailVm : ObservableObject
    {
        public CothingItemDetailVm(ClothingItemDetailPage page, ClothingItem clothingItem)
        {
            this.Page = page;
            ClothingItem = clothingItem;
        }
        public ClothingItemDetailPage Page { get; }
        [ObservableProperty] private ClothingItem clothingItem;
        [RelayCommand]
        private async Task RedactItem()
        {
            await Page.Navigation.PushAsync(new RedactClothingItemPage(ClothingItem));
        }
    }
}
