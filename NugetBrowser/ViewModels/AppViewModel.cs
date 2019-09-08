using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;

namespace NugetBrowser.ViewModels
{
    class AppViewModel : ReactiveObject
    {
        private string _searchTerm;

        public string SearchTerm
        {
            get => _searchTerm;
            set => this.RaiseAndSetIfChanged(ref _searchTerm, value);
        }

        private readonly ObservableAsPropertyHelper<IEnumerable<NugetDetailsViewModel>> _searchResults;
        public IEnumerable<NugetDetailsViewModel> SearchResults => _searchResults.Value;

        private readonly ObservableAsPropertyHelper<bool> _isAvailable;
        public bool IsAvailable => _isAvailable.Value;

        public AppViewModel()
        {
            _searchResults = this
                .WhenAnyValue(self => self.SearchTerm)
                .Throttle(TimeSpan.FromMilliseconds(800))
                .Select(query => query?.Trim())
                .DistinctUntilChanged()
                .Where(query => !string.IsNullOrWhiteSpace(query))
                .SelectMany(SearchNuGetPackages)
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, self => self.SearchResults);

            _searchResults.ThrownExceptions.Subscribe(e =>
            {
                System.Diagnostics.Debug.WriteLine(e);
            });

            _isAvailable = this
                .WhenAnyValue(self => self.SearchResults)
                .Select(results => results != null)
                .ToProperty(this, self => self.IsAvailable);
        }

        private async Task<IEnumerable<NugetDetailsViewModel>> SearchNuGetPackages(
            string term,
            CancellationToken cancellationToken
        )
        {
            return null;
        }
    }
}
