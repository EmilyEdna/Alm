using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Text;

namespace AlmCore.ThreadDowner
{
    public class HttpCore
    {
        public static HttpWebResponse RangeDown(string URL, long From, long To)
        {
            HttpWebRequest request = null;
            if (URL.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, certificate, chain, errors) => true);
                request = WebRequest.Create(URL) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
            }
            else
            {
                request = WebRequest.Create(URL) as HttpWebRequest;
            }
            request.AddRange(From, To);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response;
        }
    }
}
