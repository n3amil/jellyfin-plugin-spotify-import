using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities.Audio;

namespace Viperinius.Plugin.SpotifyImport.Matchers
{
    internal class TrackComparison
    {
        private static readonly DefaultMatcher<string> _defaultStringMatcher = new DefaultMatcher<string>();
        private static readonly CaseInsensitiveMatcher _caseInsensitiveMatcher = new CaseInsensitiveMatcher();
        private static readonly IgnorePunctuationMatcher _punctuationMatcher = new IgnorePunctuationMatcher();

        private static bool Equal(string? jellyfinName, string? providerName, ItemMatchLevel matchLevel)
        {
            var result = false;
            if (string.IsNullOrEmpty(jellyfinName) || string.IsNullOrEmpty(providerName))
            {
                return result;
            }

            if (!result && matchLevel == ItemMatchLevel.Default)
            {
                result = _defaultStringMatcher.Matches(jellyfinName, providerName);
            }

            if (!result && matchLevel == ItemMatchLevel.IgnoreCase)
            {
                result = _caseInsensitiveMatcher.Matches(jellyfinName, providerName);
            }

            if (!result && matchLevel == ItemMatchLevel.IgnorePunctuationAndCase)
            {
                result = _punctuationMatcher.Matches(jellyfinName, providerName);
            }

            return result;
        }

        private static bool ListContains(IReadOnlyList<string>? jellyfinList, string? providerName, ItemMatchLevel matchLevel)
        {
            if (jellyfinList == null)
            {
                return false;
            }

            return jellyfinList.Where(j => Equal(j, providerName, matchLevel)).Any();
        }

        public static bool TrackNameEqual(Audio jfItem, ProviderTrackInfo providerItem, ItemMatchLevel matchLevel)
        {
            return Equal(jfItem.Name, providerItem.Name, matchLevel);
        }

        public static bool AlbumNameEqual(Audio jfItem, ProviderTrackInfo providerItem, ItemMatchLevel matchLevel)
        {
            return Equal(jfItem.AlbumEntity?.Name, providerItem.AlbumName, matchLevel);
        }

        public static bool AlbumArtistContained(Audio jfItem, ProviderTrackInfo providerItem, ItemMatchLevel matchLevel)
        {
            return ListContains(jfItem.AlbumEntity?.Artists, providerItem.AlbumArtistName, matchLevel);
        }

        public static bool ArtistContained(Audio jfItem, ProviderTrackInfo providerItem, ItemMatchLevel matchLevel)
        {
            return ListContains(jfItem.Artists, providerItem.ArtistName, matchLevel);
        }
    }
}
