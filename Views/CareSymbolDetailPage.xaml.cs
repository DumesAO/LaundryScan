using LaundryScan.Models;
namespace LaundryScan.Views
{
    public partial class CareSymbolDetailPage : ContentPage
    {
        public CareSymbolDetailPage(CareSymbol careSymbol)
        {
            InitializeComponent();
            BindingContext = careSymbol;
        }
    }
}