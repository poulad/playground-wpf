using IEXTrading.IEXTradingREFERENCEDATA_SYMBOLS;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace FinCore.ViewModels
{
    public class AppViewModel : ReactiveObject
    {
        public IEnumerable<Symbols_REFERENCEDATA_SYMBOLS> Symbols => _symbols.Value;

        public bool IsAvailable => _isAvailable.Value;

        public ReactiveCommand<Unit, IEnumerable<Symbols_REFERENCEDATA_SYMBOLS>> LoadAllSymbols { get; }

        private readonly ObservableAsPropertyHelper<IEnumerable<Symbols_REFERENCEDATA_SYMBOLS>> _symbols;

        private readonly ObservableAsPropertyHelper<bool> _isAvailable;

        public AppViewModel()
        {
            LoadAllSymbols = ReactiveCommand.CreateFromTask(QuerySymbolsAsync);

            _symbols = LoadAllSymbols
                .ToProperty(this, nameof(Symbols), scheduler: RxApp.MainThreadScheduler);

            _isAvailable = this
                .WhenAnyValue(x => x.Symbols)
                .Select(symbols => symbols?.Any() == true)
                .ToProperty(this, nameof(IsAvailable));
        }

        private async Task<IEnumerable<Symbols_REFERENCEDATA_SYMBOLS>> QuerySymbolsAsync(CancellationToken cancellationToken = default)
        {
            var connection = IEXTrading.IEXTradingConnection.Instance;
            var result = await connection.GetQueryObject_REFERENCEDATA_SYMBOLS().QueryAsync()
                .ConfigureAwait(false);
            return result.Data.Symbols;
        }
    }
}
