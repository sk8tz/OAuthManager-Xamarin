using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Manager.Common.Tokens
{
    public class AuthToken : Token
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AuthToken() { }

        public AuthToken(string code, ILookup<string, string> extraData) : base(code, extraData) { }
    }
}
