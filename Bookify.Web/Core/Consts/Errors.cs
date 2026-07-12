namespace Bookify.Web.Core.Consts
{
    public static class Errors
    {
        public const string MaxLengthError = "The field {0} must be a string with a maximum length of {1}.";
        public const string MinLengthError = "The field {0} must be a string with a minimum length of {1}.";
        public const string MinMaxError = "The {0} must be at least {2} and at max {1} characters long.";

        public const string RequiredError = "The field {0} is required.";

        public const string DublicatedError = "Another Record With the same {0} is already exists";

        public const string NotAllowedExtension = "This file extension is not allowed.";
        public const string MaxSize = "The file size exceeds the maximum allowed size.";

        public const string DublicatedBookWithTheSameAuthor = "A book with the same title and author already exists.";
        public const string DublicatedAuthorWithTheSameBook = "An author with the same book title already exists.";
        public const string InvalidPublishingDate = "The publishing date cannot be in the future.";
        public const string InvalidRange = "{0} Must Be Between {1} and {2}.";
        
        public const string InvalidEmail = "Invalid Email";
        public const string ConfirmPasswordNotMatch = "The password and confirmation password do not match.";

        public const string InvalidUserName = "Can only contain letters and digits";  
        public const string WeakPassword = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";

        public const string OnlyEnglishLetters = "Only English letters are allowed.";
        public const string OnlyArabicLetters = "Only Arabic letters are allowed.";
        public const string OnlyNumbersAndLetters = "Only Arabic/English letters or digits are allowed.";
        public const string DenySpecialCharacters = "Special characters are not allowed.";

        public const string InvalidMobileNumber = "Invalid mobile number.";


    }
}
