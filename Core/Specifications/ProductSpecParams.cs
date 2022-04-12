namespace Core.Specifications
{
    public class ProductSpecParams
    {
        private const int maxPageSize = 50;
        private int _pageSize = 6;
        private string _search;
        private string _category;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public int PageIndex { get; set; } = 1;

        public string Sort { get; set; }

        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }

        public string Category
        {
            get => _category;
            set => _category = value.ToLower();
        }
    }
}
