using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskQueue.DAO.Models
{
    public class Page<T>
    {
        public List<T> Items { get; protected set; }

        public int TotalItems { get; protected set; }

        public int PageSize { get; protected set; }

        public int PageCount { get; private set; }

        public int PageIndex { get; protected set; }

        public Page(IEnumerable<T> items, int pageIndex, int pageSize, int total)
        {
            Items = items.ToList();
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItems = total;

            var pages = (double)TotalItems / PageSize;
            PageCount = (int)Math.Round(pages, MidpointRounding.ToPositiveInfinity);
        }

        public Page<U> Convert<U>(Func<T, U> itemConverterFunc)
        {
            var items = Items.Select(i => itemConverterFunc(i)).ToList();
            return new Page<U>(items, PageIndex, PageSize, TotalItems);
        }
    }
}