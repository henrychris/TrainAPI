namespace TrainAPI.Domain.Constants;

public static class DomainConstants
{
    /// <summary>
    /// Use globally to enforce ID lengths
    /// </summary>
    public const int MAX_ID_LENGTH = 450;

    public const int MIN_NAME_LENGTH = 3;
    public const int MAX_NAME_LENGTH = 50;

    public const int MIN_EMAIL_ADDRESS_LENGTH = 5;
    public const int MAX_EMAIL_ADDRESS_LENGTH = 50;

    public const int MAX_JSON_LENGTH = 450;
    public const int MAX_ENUM_LENGTH = 20;

    public const int MIN_CODE_LENGTH = 1;
    public const int MAX_CODE_LENGTH = 3;
}