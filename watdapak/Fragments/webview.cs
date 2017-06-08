using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Webkit;

namespace watdapak.Fragments
{
    public class webview : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.weview, container, false);


            WebView webViewz = rootView.FindViewById<WebView>(Resource.Id.webView);
            webViewz.Settings.JavaScriptEnabled = true;
            webViewz.Settings.DomStorageEnabled = true;
            webViewz.ClearCache(true);
            CookieManager.Instance.RemoveSessionCookie();
            webViewz.LoadUrl("https://sharepointevo.sharepoint.com");
            webViewz.SetWebViewClient(new HelloWebViewClient((Activity as MainActivity)));



            return rootView;

        }
    }
}