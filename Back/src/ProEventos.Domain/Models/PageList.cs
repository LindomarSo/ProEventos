using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Domain.Models
{
    public class PageList<T> : List<T>
    {
        public PageList() {  }

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count; 
            PageSize = pageSize;    
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PageList<T>> CreateAsync(
                IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize) // Começa de 0 por isso  -1
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}
