using AISpeechExplorer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISpeechExplorer
{
    public static class ListExtensions
    {
        public static EnrollmentStatusType AsEnrollmentStatus(this string value)
        {
            return (EnrollmentStatusType)Enum.Parse(typeof(EnrollmentStatusType), value, true);
        }
    }
}
