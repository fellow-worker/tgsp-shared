namespace TGSP.Shared.Security
{
    /// <summary>
    /// A enum that defines the possible results of validation of token
    /// </summary>
    public enum TokenValidationResult
    {
        NoResult, Success, FormatError, Expired, SignatureError, ContextError
    }
}