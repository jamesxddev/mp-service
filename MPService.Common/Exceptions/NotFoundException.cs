namespace MPService.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public string ErrorCode { get; }

        public NotFoundException(string entityName, object key)
            : base($"{entityName} with key {key} was not found.")
        {
            ErrorCode = $"{entityName.ToUpper()}_NOT_FOUND";
        }
    }
}
