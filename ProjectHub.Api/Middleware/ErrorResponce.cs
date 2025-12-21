namespace ProjectHub.Api.Middleware
{
    public class ErrorResponce
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = "Something went wrong.";
        public string TraceId { get; set; } = "";
        public string? Detail { get; set; } //only for Development
    }
}
