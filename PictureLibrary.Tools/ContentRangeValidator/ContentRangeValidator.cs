using System.Text.RegularExpressions;

namespace PictureLibrary.Tools.ContentRangeValidator
{
    public partial class ContentRangeValidator : IContentRangeValidator
    {
        public bool IsContentRangeContinuationOfOldContentRange(string oldContentRange, string newContentRange, int bytesRead)
        {
            return TryParseContentRange(oldContentRange, out _, out int previousUploadEndIndex, out _)
                && TryParseContentRange(newContentRange, out int newUploadStartIndex, out int newUploadEndIndex, out _)
                && ++previousUploadEndIndex == newUploadStartIndex
                && (newUploadStartIndex + bytesRead) == newUploadEndIndex;
        }

        public bool IsContentRangeValid(string contentRange)
        {

            if (!TryParseContentRange(contentRange, out int startIndex, out int endIndex, out int totalLength))
            {
                return false;
            }

            if (startIndex < 0 || endIndex >= totalLength || startIndex > endIndex)
            {
                return false;
            }

            return true;
        }

        public bool IsUploadComplete(string contentRange)
        {
            return TryParseContentRange(contentRange, out _, out int endIndex, out int totalLength)
                && endIndex == totalLength;
        }

        private static bool TryParseContentRange(string contentRange, out int startIndex, out int endIndex, out int totalLength)
        {
            var regex = ContentRangeRegex();
            var match = regex.Match(contentRange);

            if (!match.Success)
            {
                startIndex = 0;
                endIndex = 0; 
                totalLength = 0;

                return false;
            }

            startIndex = int.Parse(match.Groups["start"].Value);
            endIndex = int.Parse(match.Groups["end"].Value);
            totalLength = int.Parse(match.Groups["total"].Value);

            return true;
        }

        [GeneratedRegex("^bytes (?<start>\\d+)-(?<end>\\d+)/(?<total>\\d+)$")]
        private static partial Regex ContentRangeRegex();
    }
}
