using FinCore.ViewModels;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace FinCore
{
    public abstract class MainWindowBase : ReactiveWindow<AppViewModel> { }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MainWindowBase
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new AppViewModel();

            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(
                    ViewModel,
                    vm => vm.IsAvailable,
                    v => v.listBoxSymbols.Visibility
                ).DisposeWith(disposableRegistration);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.Symbols,
                    v => v.listBoxSymbols.ItemsSource
                ).DisposeWith(disposableRegistration);

                Observable.Range(0, 1)
                    .Select(_ => Unit.Default)
                    .InvokeCommand(ViewModel.LoadAllSymbols)
                    .DisposeWith(disposableRegistration);
            });
        }
    }
}
