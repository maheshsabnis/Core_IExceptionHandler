namespace Core_IExceptionHandler.Infrastructure
{
    public class ErrorDetails
    {
        public int Status { get; set; }
        public string? TypeName { get; set; }
        public string? ErrorTitle { get; set; }
        public string? Message { get; set; }
        public string? RequestDetails { get; set; }
    }
}
