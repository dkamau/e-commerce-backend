namespace ECommerceBackend.Web.AbstractClasses
{
    public abstract class Pagination
    {
        public int? PreviousPage { get; set; }
        public int Page { get; set; }
        public int? NextPage { get; set; }
        public int TotalPages { get; set; }
        public int RecordsPerPage { get; set; }
        public int RecordsInPage { get; set; }
        public int TotalRecords { get; set; }
    }
}
