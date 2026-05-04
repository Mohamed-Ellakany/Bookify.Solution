namespace Bookify.Web.Core.Consts
{
    public static class Errors
    {
        public const string MaxLengthError = "The field {0} must be a string with a maximum length of {1}.";
        public const string MinLengthError = "The field {0} must be a string with a minimum length of {1}.";
        public const string RequiredError = "The field {0} is required.";
        public const string DublicatedError = "This item already exists.";

        public const string NotAllowedExtension = "This file extension is not allowed.";
        public const string MaxSize = "The file size exceeds the maximum allowed size.";

        public const string DublicatedBookWithTheSameAuthor = "A book with the same title and author already exists.";
        public const string DublicatedAuthorWithTheSameBook = "An author with the same book title already exists.";
        public const string InvalidPublishingDate = "The publishing date cannot be in the future.";



    }
}
