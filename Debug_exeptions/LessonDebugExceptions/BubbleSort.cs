using System.Collections.Generic;

namespace LessonDebugExceptions
{
    public static class BubbleSort
    {
        public static void Sort<TItem>(
            IList<TItem> items,
            IComparer<TItem> comparer )
        {
            for ( int i = 1; i < items.Count; i++ )
            {
                for ( int j = i + 1; j < items.Count; j++ )
                {
                    if ( comparer.Compare( items[ i ], items[ j ] ) > 0 )
                    {
                        TItem swap = items[ j ];
                        items[ j ] = items[ i ];
                        items[ i ] = swap;
                    }
                }
            }
        }
    }
}
