using JunsBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JunsBlog.Test.Helper
{
    public static class ExtensionMethods
    {
        public static T ReplaceOne<T>(this List<T> items, T item) where T : EntityBase
        {
            var index = items.FindIndex(x => x.Id == item.Id);
            if (index == -1)
                items.Add(item);
            else
                items[index] = item;
            return item;
        }
    }
}
