using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace _17_StandardCornellBox
{
    public class XComparator : IComparer<HitTable>
    {
        public int Compare([AllowNull] HitTable x, [AllowNull] HitTable y)
        {
            return Helpers.Box_compare(x, y, 0);
        }
    }

    public class YComparator : IComparer<HitTable>
    {
        public int Compare([AllowNull] HitTable x, [AllowNull] HitTable y)
        {
            return Helpers.Box_compare(x, y, 1);
        }
    }

    public class ZComparator : IComparer<HitTable>
    {
        public int Compare([AllowNull] HitTable x, [AllowNull] HitTable y)
        {
            return Helpers.Box_compare(x, y, 2);
        }
    }
}
