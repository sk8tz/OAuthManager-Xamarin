using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Manager.Common.Tokens
{
    public abstract class TokenBase
    {
        public ILookup<string, string> ExtraData { get; set; }
    }
}
