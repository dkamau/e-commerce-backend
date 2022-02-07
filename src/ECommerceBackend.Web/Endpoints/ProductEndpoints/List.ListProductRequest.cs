namespace ECommerceBackend.Web.Endpoints.ProductEndpoints
{
    public class ListProductRequest
    {
        public int? Page { get; set; }
        public int? RecordsPerPage { get; set; }
        public bool Paginate { get; set; } = true;
    }
}
