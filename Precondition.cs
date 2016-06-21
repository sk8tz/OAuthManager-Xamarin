using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Manager
{
    public static class Precondition
    {
        public static void NotNull(object obj, string parameterName = "", string message = "")
        {
            if (obj == null) throw new ArgumentNullException(parameterName, message);
        }

        public static void NotNullOrEmpty(string value, string parameterName = "", string message = "")
        {
            switch (value)
            {
                case null:
                    throw new ArgumentNullException(parameterName, message);
                case "":
                    throw new ArgumentException(message, parameterName);
            }
        }

        public static void Requires(bool condition, string parameterName = "", string message = "")
        {
            if (!condition) throw new ArgumentException(message, parameterName);
        }
    }
}
