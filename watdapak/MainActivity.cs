using Android.App;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using System;
using System.Threading;
using Org.Apache.Http.Impl.Client;
using Org.Apache.Http.Client.Methods;
using Org.Apache.Http;
using Android.Util;
using Newtonsoft.Json;
using Org.Apache.Http.Util;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;
using System.Net;
using System.Text;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using watdapak.Helpers;

namespace watdapak
{
    [Activity(Label = "watdapak", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        string siteUrl = "https://sharepointevo.sharepoint.com", projectDataRestUrl = "/_api/ProjectData/Projects", rtFa = null, FedAuth = null, projectServerRestUrl = "/_api/ProjectServer/Projects";
        AuthenticationResult authResult;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            WebView webViewz = FindViewById<WebView>(Resource.Id.webView);
            webViewz.Settings.JavaScriptEnabled = true;
            webViewz.Settings.DomStorageEnabled = true;
            webViewz.ClearCache(true);
            CookieManager.Instance.RemoveSessionCookie();
            webViewz.LoadUrl(siteUrl);
            webViewz.SetWebViewClient(new HelloWebViewClient(this));



        }

        public async Task<Boolean> login() {
            authResult = await AuthenticationHelper.GetAccessToken(AuthenticationHelper.SharePointUrl, new PlatformParameters(this));
            Log.Info("Login", authResult.UserInfo.DisplayableId + "");
            return true;
        }

        public void setCredentialsAsync(string rtFa, string fedAuth)
        {
            this.rtFa = rtFa;
            this.FedAuth = fedAuth;
            //GetProjects();
            //GetProjectById();
            //GetProjectByName();
            //GetFormDigest();
            //addProject2();
            //getProjectEPT();
            //checkOutProject();
        }

        public async Task<string> GetFormDigest()
        {
            string response = "", formDigest = "";

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);

            var body = "";
            var contents = new StringContent(body);
            contents.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");
            
            try
            {
                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/contextinfo", contents);
                postResult.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode) {
                     response = await postResult.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<watdapak.FormDigestModel.RootObject>(response);
                    formDigest = data.D.GetContextWebInformation.FormDigestValue;
                    Log.Info("formDigest", formDigest);
                }
                return formDigest;
            }
            catch(Exception e){
                Log.Info("formDigest", e.Message);
                return null;
            }

            
        }

        public async void GetProjects() {

            try {
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
                handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

                var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
                mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Accept.Add(mediaType);

                var result = await client.GetStringAsync(siteUrl + "/sites/mobility" + projectServerRestUrl);
                Log.Info("Projects", result);
                var data = JsonConvert.DeserializeObject<watdapak.Models.RootObject>(result);
                for (int i = 0; i < data.D.Results.Count; i++) {
                    Log.Info("Projects", data.D.Results[i].Name + " = " + data.D.Results[i].Id);
                }
            }
            catch (Exception e) {
                Log.Info("Projects error", e.Message);
            }
        }

        public async void GetProjectById() {

            try {
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
                handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

                var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
                mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Accept.Add(mediaType);


                var result = await client.GetStringAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects(guid'f4c063c9-c70e-e711-80c9-00155d0cb90e')");
                Log.Info("ProjectById", result);
                var data = JsonConvert.DeserializeObject<watdapak.ProjectById.RootObject>(result);
                Log.Info("ProjectById", data.D.Name + " " + data.D.Description);
            }
            catch (Exception e) {
                Log.Info("ProjectById", e.Message);
            }
        }

        public async void GetProjectByName() {

            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
                handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

                var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
                mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Accept.Add(mediaType);


                var result = await client.GetStringAsync(siteUrl + "/sites/mobility/_api/ProjectData/Projects?$filter=ProjectName eq 'TestTasks'");
                Log.Info("ProjectByName", result);
                var data = JsonConvert.DeserializeObject<watdapak.ProjectById.RootObject>(result);
                Log.Info("ProjectById", data.D.Name + " " + data.D.Description);
            }
            catch (Exception e) {
                Log.Info("ProjectByName", e.Message);
            }
        }

        public async void addProject() {

            var formDigest = await GetFormDigest();

            var handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));


            HttpClient client = new HttpClient(handler);
            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));
            client.DefaultRequestHeaders.Accept.Add(mediaType); ;
            client.DefaultRequestHeaders.Add("X-RequestDigest", formDigest);

            var body = "{'parameters': {'Name': 'kristian test', 'Description': 'ftwftwftw', 'EnterpriseProjectTypeId': '09fa52b4-059b-4527-926e-99f9be96437a'} }";
            var contents = new StringContent(body, Encoding.UTF8, "application/json");
            //contents.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            try {
                //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, siteUrl + "/sites/mobility" + projectServerRestUrl + "/Add");
                //request.Headers.Add("X-RequestDigest", formDigest);
                ////request.Headers.Add("content-Type","application/json;odata=verbose");
                //request.Content = contents;


                //HttpResponseMessage response = await client.SendAsync(request);
                //string responseString = await response.Content.ReadAsStringAsync();
                //var result = response.EnsureSuccessStatusCode();
                //if (response.IsSuccessStatusCode)
                //    Log.Info("Add Projects", "mana og add");

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects/Add", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("Add Projects", "mana og add");

            }
            catch (Exception e) {
                Log.Info("Add Projects", "failed kay " + e.Message);
            }

        }

        public async void addProject2() {

            var formDigest = await GetFormDigest();

            var handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));


            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
            client.DefaultRequestHeaders.Add("X-RequestDigest", formDigest);
            client.DefaultRequestHeaders.Add("X-Http-Method", "POST");


            //string parameters = "{'parameters': {'Name': 'kristian test', 'Start': '"+ DateTime.Today +"' 'Description': 'ftwftwftw', 'EnterpriseProjectTypeId': '09fa52b4-059b-4527-926e-99f9be96437a'} }";
            string parameters = "{'parameters': {'Name': 'kristian test', 'Start': '" + DateTime.Today + "' 'Description': 'ftwftwftw'} }";
            //var body = "{'parameters': {'Name': 'kristian test', 'Description': 'ftwftwftw', 'Start': '2016-01-04T08:00:00'} }";
            //var contents = new StringContent(body, Encoding.UTF8, "application/json");
            //contents.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            try
            {
                StringContent strContent = new StringContent(parameters);
                strContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");
                HttpResponseMessage responseMessage = client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects/Add", strContent).Result;

                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.IsSuccessStatusCode)
                {
                    var content = responseMessage.Content.ReadAsStringAsync();
                    Log.Info("Add Projects", content + "");
                }
                else {
                    var content = responseMessage.Content.ReadAsStringAsync();
                    Log.Info("Add Projects", content + "");
                }

            }
            catch (Exception e)
            {
                Log.Info("Add Projects", "failed kay " + e.Message);
            }

        }

        public async void addTask() {

        }

        public async void getProjectEPT() {

            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
                handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

                var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
                mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Accept.Add(mediaType);

                var result = await client.GetStringAsync(siteUrl + "/sites/mobility/_api/ProjectServer/EnterpriseProjectTypes");
                Log.Info("EnterpriseProjectTypes", result);
            }
            catch (Exception e)
            {
                Log.Info("Projects error", e.Message);
            }

        }

        public async void checkOutProject() {

            var formDigest = await GetFormDigest();

            var handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

            HttpClient client = new HttpClient(handler);
            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));
            client.DefaultRequestHeaders.Accept.Add(mediaType); ;
            client.DefaultRequestHeaders.Add("X-RequestDigest", formDigest);

            var body = "";
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try {
                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects('311d30db-5b05-e711-80c9-00155d0c4508')/CheckOut", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("checkout", await result.Content.ReadAsStringAsync());
            }
            catch (Exception e) {
                Log.Info("checkout", "failed kay " + e.Message);
            }
        }

    }
}

