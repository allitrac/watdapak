using Android.Util;
using Android.Webkit;
using System;
using Android.Graphics;
using Android.Content;
using Android.Preferences;

namespace watdapak
{
    internal class HelloWebViewClient : WebViewClient
    {
        private MainActivity mainActivity;
        string rtFa = "", FedAuth = "";
        bool isFed = false, isRtFa = false;

        public HelloWebViewClient(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            return base.ShouldOverrideUrlLoading(view, url);
        }


        public override void OnPageFinished(WebView view, string url)
        {

                CookieManager cookieManager = CookieManager.Instance;
                string generateToken = cookieManager.GetCookie("https://sharepointevo.sharepoint.com/SitePages/home.aspx?AjaxDelta=1");

                String[] token = generateToken.Split(new char[] { ';' });
                for (int i = 0; i < token.Length; i++)
                {
                    if (token[i].Contains("rtFa"))
                    {
                        rtFa = token[i].Replace("rtFa=", "");
                        isRtFa = true;
                    }
                    if (token[i].Contains("FedAuth"))
                    {
                        FedAuth = token[i].Replace("FedAuth=", "");
                        isFed = true;
                    }
                }

                if (isRtFa && isFed)
                {
                    
                    ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences((mainActivity));
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString("rtFa", rtFa);
                    editor.PutString("FedAuth", FedAuth);
                    editor.Apply();
                    Log.Info("credentials", "rtFa = " + rtFa + "\nFedAuth = " + FedAuth);
                    mainActivity.setCredentialsAsync(rtFa, FedAuth);

                }
            
        }

        //public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        //{
        //    CookieManager cookieManager = CookieManager.Instance;
        //    string generateToken = cookieManager.GetCookie("https://sharepointevo.sharepoint.com/SitePages/home.aspx?AjaxDelta=1");

        //    String[] token = generateToken.Split(new char[] { ';' });
        //    for (int i = 0; i < token.Length; i++)
        //    {
        //        if (token[i].Contains("rtFa"))
        //        {
        //            rtFa = token[i].Replace("rtFa=", "");
        //            isRtFa = true;
        //        }
        //        if (token[i].Contains("FedAuth"))
        //        {
        //            FedAuth = token[i].Replace("FedAuth=", "");
        //            isFed = true;
        //        }
        //    }

        //    if (isRtFa && isFed)
        //    {
        //        Log.Info("credentials", "rtFa = " + rtFa + "\nFedAuth = " + FedAuth);
        //        mainActivity.setCredentialsAsync(rtFa, FedAuth);

        //    }
        //}

    }
}