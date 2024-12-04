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
    public partial class OutfitDetailVM : ObservableObject
    {
        public OutfitDetailVM(OutfitDetailPage page, Outfit fit)
        {
            this.Page = page;
            Fit = fit;
        }
        public OutfitDetailPage Page { get; }
        [ObservableProperty] private Outfit fit;
        [RelayCommand]
        private async Task RedactOutfit()
        {
            await Page.Navigation.PushAsync(new RedactOutfitPage(this.Fit));
        }
    }
}
