using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Manager.Common.Tokens
{
    [DebuggerDisplay("Code = {Code}")]
    [DataContract]
    public abstract class Token : TokenBase
    {
        [DataMember(Order = 1)]
        public string Code { get; }

        [Obsolete("this is used for serialize")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Token() { }

        public Token(string code, ILookup<string, string> extraData)
        {
            Precondition.NotNull(code);

            this.Code = code;
            base.ExtraData = extraData;
        }
    }
}
