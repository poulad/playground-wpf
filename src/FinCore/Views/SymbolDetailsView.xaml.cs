using FinCore.ViewModels;
using ReactiveUI;

namespace FinCore.Views
{
    public abstract class SymbolDetailsViewBase : ReactiveUserControl<SymbolDetailsViewModel> { }

    public partial class SymbolDetailsView : SymbolDetailsViewBase
    {
        public SymbolDetailsView()
        {
            InitializeComponent();
        }
    }
}
