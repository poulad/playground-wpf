using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using ReactiveUI;
using PackageSource = NuGet.Configuration.PackageSource;

namespace NugetBrowser.ViewModels
{
    public class AppViewModel : ReactiveObject
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
            var providers = new List<Lazy<INuGetResourceProvider>>();
            providers.AddRange(Repository.Provider.GetCoreV3()); // Add v3 API support
            var packageSource = new PackageSource("https://api.nuget.org/v3/index.json");
            var source = new SourceRepository(packageSource, providers);

            var filter = new SearchFilter(false);
            var resource = await source.GetResourceAsync<PackageSearchResource>().ConfigureAwait(false);
            var metadata = await resource.SearchAsync(term, filter, 0, 10, null, cancellationToken).ConfigureAwait(false);
            return metadata.Select(x => new NugetDetailsViewModel(x));
        }
    }
}
