namespace Ipz_server.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = [];
        public object Data { get; set; }
    }
}
