using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Manager.Common
{
    public interface IAuthProvider
    {
        bool AppendCredentials(HttpRequestMessage request);
    }
}
