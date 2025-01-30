namespace Hostel_Management.Responses
{
    public class APIResponse
    {
        public bool IsSuccessful { get; set; }

        public string Message { get; set; }

        public object? Data { get; set; }

    }
}
