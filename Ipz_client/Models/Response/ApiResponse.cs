namespace Ipz_client.Models.Response
{
    public class ApiResponse
    {
        public bool Success { get; set; } = true;
        public List<string> Errors { get; set; } = [];
        public object Data { get; set; }
    }
}
