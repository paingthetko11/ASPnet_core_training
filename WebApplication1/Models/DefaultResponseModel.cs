namespace WebApplication1.Models
{
    public class DefaultResponseModel
    {
        public bool Success { get; set; }

        public int Statuscode { get; set; }

        public string? Message {  get; set; }
        
        public dynamic? Data { get; set; }
    }
}
