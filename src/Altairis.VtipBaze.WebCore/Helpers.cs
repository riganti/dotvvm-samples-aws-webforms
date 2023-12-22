using DotVVM.Framework.Hosting;
using System;

namespace Altairis.VtipBaze.WebCore
{
    public static class Helpers
    {

        public static Uri GetApplicationBaseUri(this IDotvvmRequestContext context)
        {
            var uriBuilder = new UriBuilder(context.HttpContext.Request.Url);
            uriBuilder.Path = context.HttpContext.Request.PathBase.Value;
            uriBuilder.Query = string.Empty;
            uriBuilder.Fragment = string.Empty;
            if (!uriBuilder.Path.EndsWith("/"))
            {
                uriBuilder.Path += "/";
            }

            return uriBuilder.Uri;
        }

    }
}