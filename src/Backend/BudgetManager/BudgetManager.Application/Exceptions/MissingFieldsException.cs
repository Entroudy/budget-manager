namespace BudgetManager.Common.Exceptions;

public class MissingFieldsException : Exception
{
    public Dictionary<string, string> FieldErrors { get; set; }
    
    public MissingFieldsException(string message) : base(message) { }

    public MissingFieldsException(string message, Dictionary<string, string> fieldErrors) : base(message)
    {
        FieldErrors = fieldErrors;
    }
}
