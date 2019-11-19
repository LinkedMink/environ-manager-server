using System.Reflection;

namespace LinkedMink.Data.Base
{
    public class SortCriteria
    {
        public enum SortOrder
        {
            None = 0,
            Ascending = 1,
            Descending = 2
        }

        public SortOrder Order { get; set; }
        public PropertyInfo Property { get; set; }
    }
}
