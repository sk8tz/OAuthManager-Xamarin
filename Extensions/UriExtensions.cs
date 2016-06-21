using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Manager.Extensions
{
    public static class UriExtensions
    {
        public static Uri ProcessIEUrlErrors(this Uri current)
        {
            return current.Authority.Equals("ieframe.dll", StringComparison.CurrentCultureIgnoreCase) ? new Uri(current.Fragment.Substring(1)) : current;
        }
    }
}
