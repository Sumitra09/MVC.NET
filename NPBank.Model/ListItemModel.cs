using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPBank.Model
{
    public class ListItemModel
    {
        public int ListItemId { get; set; }
        public int ListItemCategoryId { get; set; }
        public string ListItemName { get; set; }
        public string ListItemDisplayName { get; set; }
    }
    public class ListItemCategoryModel
    {
        public int ListItemCategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
