namespace ECommerceBackend.Web.Endpoints.OrderEndpoints
{
    public class SearchOrderRequest
    {
        public string Search { get; set; }
        public string OrderNumber { get; set; }
        public int? Page { get; set; }
        public int? RecordsPerPage { get; set; }
        public bool Paginate { get; set; } = true;
    }
}
