﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Manager.Common
{
    public interface IUserAuthorizationViewer
    {
        Uri AuthorizeUrl { set; }
        IUserConsentHandler AuthController { set; }
    }
}
