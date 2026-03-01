namespace MailContainerTest.Types
{
    [Flags]
    public enum AllowedMailType
    {
        StandardLetter = 1 ,
        LargeLetter = 2,   
        SmallParcel = 4 // changed to 4 (power of 2) to make the HasFlag cehck work correctly
    }
}