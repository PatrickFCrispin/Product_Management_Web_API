namespace ProductManagement.Domain.Responses
{
    public class Response<T> : GenericResponse
    {
        public T? Value { get; set; }
    }

    public class CollectionResponse<T> : GenericResponse
    {
        public IEnumerable<T>? Values { get; set; }
    }

    public class CollectionPaginatedResponse<T> : GenericResponse
    {
        public IEnumerable<T>? Values { get; set; }
        public int Count { get; set; }
        public int Limit { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }
    }

    public class GenericResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}