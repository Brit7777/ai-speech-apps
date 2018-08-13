using AISpeechExplorer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace AISpeechExplorer
{
    public static class ControlExtensions
    {
        public static int RemoveAll<T>(this IList<T> list, Predicate<T> predicate)
        {
            int returnCount = 0;

            for (int i = list.Count - 1; i >= 0; --i)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                    returnCount++;
                }
            }

            return returnCount;
        }

        public static CourseType AsCourseType(this object args)
        {
            return (CourseType)((((args as SelectionChangedEventArgs).AddedItems.FirstOrDefault() as PivotItem).Parent as Pivot).SelectedIndex);
        }
    }
}
