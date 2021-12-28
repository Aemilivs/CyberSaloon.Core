using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSaloon.Core.Repo.Common
{
    public class Pagination<T>
    {
        public IEnumerable<T> Entities { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        public Pagination(IEnumerable<T> list, int pageSize, int pageNumber)
        {
            var count = list.Count();
            
            Entities =
                list
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize);

            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int) Math.Ceiling(count/(double)PageSize);
        }
    }
}