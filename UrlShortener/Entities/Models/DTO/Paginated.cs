using System;
using System.Collections.Generic;

namespace Entities.Models.DTO
{
    public class Paginated<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public T[] Items { get; set; }

        public Paginated(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items.ToArray();
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
    }
}
