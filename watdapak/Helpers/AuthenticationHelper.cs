using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace watdapak.Helpers
{
    class AuthenticationHelper
    {
        public const string Authority = "https://login.windows.net/common";
        public static Uri returnUri = new Uri("http://com.refractored.navdrawer.samplecompat/");
        public static string clientId = "14deed28-5837-4572-85dc-05ebd73b8ae8";
        public static string SharePointUrl = "https://sharepointevo.sharepoint.com/";
        public static AuthenticationContext authContext = null;

        public static async Task<AuthenticationResult> GetAccessToken(string serviceResourceId, PlatformParameters param)
        {

            authContext = new AuthenticationContext(Authority);
            if (authContext.TokenCache.ReadItems().Any())
                authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);
            var authResult = await authContext.AcquireTokenAsync(serviceResourceId, clientId, returnUri, param);

            return authResult;
        }

    }
}