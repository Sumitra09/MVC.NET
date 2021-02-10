using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPBank.Model
{
    public class FilterSortModel
    {
        public FilterSortModel()
        {
            Filter = new FilterModel();
            sort = new List<SortModel>();
        }

        public FilterModel Filter { get; set; }

        public int take { get; set; }
        public int skip { get; set; }
        public List<SortModel> sort { get; set; }
        public string columnname { get; set; }
        public string searchtext { get; set; }
    }
    public class FilterModel
    {
        public FilterModel()
        {
            Filters = new List<FilterParameterModel>();
        }
        public List<FilterParameterModel> Filters { get; set; }
        public string Logic { get; set; }
    }
    public class SortModel
    {
        public string Field { get; set; }
        public string Dir { get; set; }

    }
    public class FilterParameterModel
    {
        public string Operator { get; set; }
        public string Value { get; set; }
        public string Field { get; set; }
        public string Logic { get; set; }
        public IEnumerable<FilterParameterModel> Filters { get; set; }
    }
}
