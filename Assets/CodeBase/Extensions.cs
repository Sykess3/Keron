using System.Collections.Generic;

namespace CodeBase
{
    public static class Extensions
    {
        public static List<T> AddRangeOfTwoCollection<T>(this List<T> resultList, IList<T> firstList, IList<T> secondList)
        {
            if (firstList != null) 
                resultList.AddRange(firstList);
            if (secondList != null) 
                resultList.AddRange(secondList);

            return resultList;
        } 
    }
}