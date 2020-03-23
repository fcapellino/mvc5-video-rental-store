namespace VideoRentalStore.Web.CustomResult
{
    public class HttpResult
    {
        public bool Success { get; set; }
        public object Resources { get; set; }

        public HttpResult(bool success, object resources = null)
        {
            Success = success;
            Resources = resources;
        }
    }
}
