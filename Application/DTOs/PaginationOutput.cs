using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class PaginationOutput<T>  where T : class
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
