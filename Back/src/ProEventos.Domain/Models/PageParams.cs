namespace ProEventos.Domain.Models
{
    public class PageParams
    {
        public const int MaxPageSize = 50;
        public int PageNumber { get; set; }

        public int pageSize = 10;
        public int PageSize 
        { 
            get { return pageSize; }

            set 
            {
                pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }

        public string Term { get; set; } = string.Empty;
    }
}