using Newtonsoft.Json;

namespace Entity.Abstract
{
    public class SuccessResponse
    {
        public bool IsSuccessful { get; set; } = true;
        public int StatusCode { get; set; }
        public string CorrelationId { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
