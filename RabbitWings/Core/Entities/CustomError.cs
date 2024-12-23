namespace RabbitWings.Core.Entities
{
    /// <summary>
    /// Represents a standardized error response.
    /// </summary>
    public class CustomError
    {
        public string StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
