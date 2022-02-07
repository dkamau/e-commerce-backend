namespace ECommerceBackend.Web.Endpoints.OrderEndpoints
{
    public class ListOrderRequest
    {
        public int? Page { get; set; }
        public int? RecordsPerPage { get; set; }
        public bool Paginate { get; set; } = true;
    }
}
