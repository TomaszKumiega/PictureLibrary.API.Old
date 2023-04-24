namespace PictureLibrary.Tools.ContentRangeValidator
{
    public interface IContentRangeValidator
    {
        bool IsContentRangeValid(string contentRange);
        bool IsContentRangeContinuationOfOldContentRange(string oldContentRange,  string newContentRange, int bytesRead);
        bool IsUploadComplete(string contentRange);
    }
}
