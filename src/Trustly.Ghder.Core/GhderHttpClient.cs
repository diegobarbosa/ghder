using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Trustly.Ghder.Core
{
    public class GhderHttpClient
    {
        public static HttpClient Instance { get; set; } = new HttpClient();
    }
}
