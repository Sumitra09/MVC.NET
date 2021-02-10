using NPBank.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace NPBank.BusinessLogic
{
    public static class FilterSortHelper
    {

        public static string ToLinqOperator(string operators)
        {
            switch (operators.ToLower())
            {
                case "eq": return "=";
                case "neq": return "!=";
                case "gte": return ">=";
                case "gt": return ">";
                case "lte": return "<=";
                case "lt": return "<";
                case "or": return " or ";
                case "and": return " and ";
                case "contains": return " like N'%{0}%' ";
                case "startswith": return " like N'{0}%' ";
                case "endswith": return " like N'%{0}' ";
                case "doesnotcontain": return " NOT LIKE N'%{0}%' ";

                default: return null;
            }
        }

        public static string BuildWhereClause(FilterModel filters)
        {
            string whereClause="";

            if (filters != null)
            {
                var whereFilter= filters.Filters.Where(x => !x.Field.Contains("having")).ToList();
                int filterCount = whereFilter.Count();
                if (filterCount > 0)
                {
                    whereClause = " WHERE ";
                    foreach (var filter in whereFilter)
                    {

                        var dataField = filter.Field.Split('_');
						DateTime datet;
						if (DateTime.TryParse(filter.Value, out datet))
						{
							if (filter.Field.Contains("Nepali"))
							{
								if (dataField.Length > 1)
									filter.Field = dataField[0] + "." + dataField[1];
							}
							else
							{
								filter.Value = datet.ToShortDateString();
								if (dataField.Length > 1)
									filter.Field = "Convert(Date," + dataField[0] + "." + dataField[1] + ")";
							}
							
						}

						else
						{
							if (dataField.Length > 1)
                                filter.Field = dataField[0] + "." + dataField[1];
                        }
						
                        if (filter.Filters == null)
                        {
							//var dat = DateTime.ParseExact(filter.Value, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
							

							if (filter.Operator == "contains" ||
                                filter.Operator == "startswith" ||
                                filter.Operator == "endswith" ||
                                filter.Operator == "doesnotcontain"
                                )
                            {
                                whereClause += filter.Field + string.Format(ToLinqOperator(filter.Operator), filter.Value);
                            }
                            else
                            {
                                whereClause += filter.Field + ToLinqOperator(filter.Operator) + " N'" + filter.Value + "'";
                            }

                            if (filterCount > 1)
                            {

                                whereClause += ToLinqOperator(filters.Logic);
                            }
                        }
                        else
                        {
                            int innerfilterCount = whereFilter.Count();
                            foreach (var nextfilter in whereFilter)
                            {
                                whereClause += nextfilter.Field + ToLinqOperator(nextfilter.Operator) + "'" + nextfilter.Value + "'";
                                if (innerfilterCount > 1)
                                {
                                    whereClause += ToLinqOperator(filter.Logic);
                                }
                                innerfilterCount--;
                            }

                        }
                        filterCount--;
                    }

                }
            }
            return whereClause;
        
        }


        public static string HavingClause(FilterModel filters)
        {
            string havingClause = "";
            if (filters != null)
            {
                var havingfilters = filters.Filters.Where(x => x.Field.Contains("having")).ToList();
                int filterCount = havingfilters.Count();
                if (filterCount > 0)
                {
                    havingClause = " HAVING ";
                    foreach (var hFilter in havingfilters)
                    {
                        var dataField = hFilter.Field.Split('_');
                        if (dataField.Length > 1)
                        {
                            int j=2;
                            hFilter.Field = "SUM(";
                            for (int i = 1; i < dataField.Length; i++)
                            {
                                switch (dataField[i])
                            {
                                case "sub":
                                    hFilter.Field += "-";
                                    break;
                                case "having":
                                    hFilter.Field += "";
                                    break;
                                default:
                                    if (i == j)
                                    {
                                        hFilter.Field += "." + dataField[i];
                                        j = j + 3;
                                    }
                                        else
                                        hFilter.Field +=  dataField[i];

                                    break;

                            }  
                            }
                                //foreach (var f in dataField)
                                //{
                                //    switch (f)
                                //    {
                                //        case "sub":
                                //            hFilter.Field += "-";
                                //            break;
                                //        case "_":
                                //            hFilter.Field += ".";
                                //            break;
                                //        case "having":
                                //            hFilter.Field += "";
                                //            break;
                                //        default:
                                //            hFilter.Field += "." + f;
                                //            break;

                                //    }                            

                                //}
                                hFilter.Field += ")";
                        }
                        //hFilter.Field = "SUM(" + dataField[0] + "." + dataField[1] + ")";
                        else
                            hFilter.Field = "SUM(" + hFilter.Field + ")";
                        if (hFilter.Filters == null)
                        {                            
                            havingClause += hFilter.Field + ToLinqOperator(hFilter.Operator) + " '" + hFilter.Value + "'";
                            if (filterCount > 1)
                            {
                                havingClause += ToLinqOperator(filters.Logic);
                            }
                        }
                        else
                        {
                            int innerfilterCount = hFilter.Filters.Count();
                            foreach (var nextfilter in hFilter.Filters)
                            {
                                havingClause += nextfilter.Field + ToLinqOperator(nextfilter.Operator) + "'" + nextfilter.Value + "'";
                                if (innerfilterCount > 1)
                                {
                                    havingClause += ToLinqOperator(hFilter.Logic);
                                }
                                innerfilterCount--;
                            }
                        }
                        filterCount--;
                    }
                }
            }
            return havingClause;
        }

        public static string OrderField(string sort)
        {
            var sortFieldName = sort;
            var dataField = sortFieldName.Split('_');
            if (dataField.Length > 1)
            {
                if (sort.Contains("having"))
                {
                    sortFieldName = "SUM(";
                    int j=2;
                    for (int i = 1; i < dataField.Length; i++)
                    {
                        switch (dataField[i])
                        {
                            case "sub":
                                sortFieldName += "-";
                                break;
                            case "having":
                                sortFieldName += "";
                                break;
                            default:
                                if (i == j)
                                {
                                    sortFieldName += "." + dataField[i];
                                    j = j + 3;
                                }
                                else
                                    sortFieldName += dataField[i];
                                break;
                        }
                    }
                    sortFieldName += ")";
                }
                else
                    sortFieldName =dataField[0] + "."+ dataField[1];
                }
                
            return sortFieldName;
        }


    }
}