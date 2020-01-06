using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mebel
{
    public static class HttpCookieCollectionExtensions
    {
       public static string Language(this HttpCookieCollection cookie)
        {
            return cookie["samir"]?.Value;
        } 
    }
}