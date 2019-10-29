using System.Collections.Generic;

namespace DpaHttpClient
{
    public class GridRequestOptions
    {
        public bool RequireTotalCount { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string SearchOperation { get; set; }
        public string DataField { get; set; }
        public object SearchValue { get; set; }
        public string[] SearchExpr { get; set; }
        public IEnumerable<GridRequestSortItem> Sort { get; set; }
        public object[] Filter { get; set; }
    }
}
