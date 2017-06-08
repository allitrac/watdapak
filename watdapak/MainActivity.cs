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
using Android.Content;
using Android.Preferences;
using watdapak.Fragments;

namespace watdapak
{
    [Activity(Label = "watdapak", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        string siteUrl = "https://sharepointevo.sharepoint.com", projectDataRestUrl = "/_api/ProjectData/Projects", rtFa = null, FedAuth = null, projectServerRestUrl = "/_api/ProjectServer/Projects";
        

        //Button getProj, getProjById, addProj, deleteProj, checkOut, checkIn, updateProj, publishProj;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            //ISharedPreferencesEditor editor = prefs.Edit();
            //editor.Clear();
            if (prefs.GetString("rtFa", null) != null && prefs.GetString("FedAuth", null) != null)
            {
                Log.Info("credentials", "credentials already available");
                setCredentialsAsync(prefs.GetString("rtFa", null), prefs.GetString("FedAuth", null));
                FragmentManager.BeginTransaction()
                    .Replace(Resource.Id.frame_container, new home())
                    .Commit();
            }
            else
            {
                FragmentManager.BeginTransaction()
                    .Replace(Resource.Id.frame_container, new webview())
                    .Commit();
            }

            
        }

        public void setCredentialsAsync(string rtFa, string fedAuth)
        {
            this.rtFa = rtFa;
            this.FedAuth = fedAuth;
            //GetProjects();
            //GetProjectById();
            //GetProjectByName();
            //GetFormDigest();
            //addProject();
            //getProjectEPT();
            //checkOutProject();
            //updateProject();
            //checkInProject();
            //publishProject();
            //getTasks();
            //deleteProject();
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
                    Log.Info("Projects", data.D.Results[i].Name + " = " + data.D.Results[i].Id + " = " + data.D.Results[i].Description);
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


                var result = await client.GetStringAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects(guid'3cf12a9f-f948-e711-80cc-00155d0cd408')");
                Log.Info("ProjectById", result);
                var data = JsonConvert.DeserializeObject<watdapak.ProjectById.RootObject>(result);
                Log.Info("ProjectById", data.D.Name + " " + data.D.Description + " = " + data.D.IsCheckedOut);
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

            var body = "{'parameters': {'Name': 'Project_test_03', 'Description': 'Project_test_03', 'EnterpriseProjectTypeId': '09fa52b4-059b-4527-926e-99f9be96437a'} }";
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects/Add", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("Add Projects", "mana og add");

            }
            catch (Exception e) {
                Log.Info("Add Projects", "failed kay " + e.Message);
            }

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
                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects('9f6bed59-604b-e711-80cd-00155d08a911')/CheckOut()", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("Projectz checkout", "checkout successful");
            }
            catch (Exception e) {
                Log.Info("Projectz checkout", "failed kay " + e.Message);
            }
        }

        public async void checkInProject()
        {

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

            try
            {
                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects('9f6bed59-604b-e711-80cd-00155d08a911')/Draft/CheckIn()", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode) { 
                    Log.Info("Projectz checkin", "checkin successful");
                }
            }
            catch (Exception e)
            {
                Log.Info("Projectz checkin", "failed kay " + e.Message);
            }
        }

        public async void updateProject() {

            var formDigest = await GetFormDigest();
            //checkOutProject();

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);
            client.DefaultRequestHeaders.Add("X-RequestDigest", formDigest);
            //client.DefaultRequestHeaders.Add("X-HTTP-METHOD", "MERGE");
            //client.DefaultRequestHeaders.Add("If-Match", "*");


            //var body = "{'Name':'Updated by Kristian Francisco'}";
            //var contents = new StringContent(body, Encoding.UTF8, "application/json");
            //var body = "{\"__metadata\":{\"type\":\"PS.test0001\"},\"Name\":\"update name\"}";

            var body = "{ \"__metadata\":{ \"type\":\"PS.DraftProject\"}, 'ProjectName':'UpdateTest'}";
            var contents = new StringContent(body);
            contents.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            try
            {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects('3cf12a9f-f948-e711-80cc-00155d0cd408')/Draft/update()", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode) { 
                    Log.Info("Projectz Update", "update successful");
                }
            }
            catch (Exception e)
            {
                Log.Info("Projectz Update", "update not successful cuz "+e.Message);
            }

        }

        public async void updateProjectCollection() {

            var formDigest = await GetFormDigest();

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);
            client.DefaultRequestHeaders.Add("X-RequestDigest", formDigest);
            //client.DefaultRequestHeaders.Add("X-HTTP-METHOD", "MERGE");
            //client.DefaultRequestHeaders.Add("If-Match", "*");


            var body = "";
            var contents = new StringContent(body);
            contents.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            try
            {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects/update()", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                {
                    Log.Info("Projectz Update", "update successful");
                }
            }
            catch (Exception e)
            {
                Log.Info("Projectz Update", "update not successful cuz " + e.Message);
            }

        }

        public async void publishProject() {

            var formDigest = await GetFormDigest();

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);
            client.DefaultRequestHeaders.Add("X-RequestDigest", formDigest);

            var body = "";
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects('9f6bed59-604b-e711-80cd-00155d08a911')/Draft/Publish(true)", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode) { 
                    Log.Info("Projectz publish", "publish successful");
                }
            }
            catch (Exception e)
            {
                Log.Info("Projectz publish", "publish not successful cuz " + e.Message);
            }
        }

        public async void getTasks() {

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);

            try
            {
                var result = await client.GetStringAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects('9f6bed59-604b-e711-80cd-00155d08a911')/Tasks");
                var data = JsonConvert.DeserializeObject<watdapak.Models.TaskModel.RootObject>(result);
                for (int i = 0; i < data.D.Results.Count; i++) {
                    Log.Info("projectz tasks", data.D.Results[i].Name + " " + data.D.Results[i].Id + " " + data.D.Results[i].IsManual);
                }
                
            }
            catch (Exception e)
            {
                Log.Info("projectz tasks error", e.Message);
            }
        }

        public async void addTask() {


            var formDigest = await GetFormDigest();
           

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);
            client.DefaultRequestHeaders.Add("X-RequestDigest", formDigest);

            var body = "{'parameters':{'Id':'" + Guid.NewGuid() + "', 'Name':'TASK_01', 'Notes':'THIS IS THE NOTES', 'Start':'"+ DateTime.Today + "', 'Duration':'5d', 'IsManual':'False' } }";
            var contents = new StringContent(body);
            contents.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            try
            {
                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects('9f6bed59-604b-e711-80cd-00155d08a911')/Draft/Tasks/Add", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode) 
                    Log.Info("projectz Add Tasks", "add task successful");
            }
            catch (Exception e)
            {
                Log.Info("projectz Add Tasks error", e.Message);
            }

        }

        public async void updateTask() {

            var formDigest = await GetFormDigest();


            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);
            client.DefaultRequestHeaders.Add("X-RequestDigest", formDigest);
            client.DefaultRequestHeaders.Add("X-HTTP-METHOD", "MERGE");

            var body = "{ \"__metadata\":{ \"type\":\"PS.DraftTask\"}, 'Name':'Updated Task', 'PercentComplete':'100', 'Duration':'1d'}";
            var contents = new StringContent(body);
            contents.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            try
            {
                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects('9f6bed59-604b-e711-80cd-00155d08a911')/Draft/Tasks('ea288d31-748e-4c98-b149-d714aacf790e')", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("projectz Update Tasks", "update task successful");
            }
            catch (Exception e)
            {
                Log.Info("projectz Update Tasks error", e.Message);
            }

        }

        public async void deleteTask() {

            var formDigest = await GetFormDigest();


            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);
            client.DefaultRequestHeaders.Add("X-RequestDigest", formDigest);
            //client.DefaultRequestHeaders.Add("X-HTTP-METHOD", "MERGE");

            var body = "";
            var contents = new StringContent(body);
            contents.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            try
            {
                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects('3cf12a9f-f948-e711-80cc-00155d0cd408')/Draft/Tasks('fdc52661-314f-4751-90a1-f64f3638e29b')/deleteObject()", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("projectz delete Tasks", "delete task successful");
            }
            catch (Exception e)
            {
                Log.Info("projectz delete Tasks error", e.Message);
            }

        }

        public async void deleteProject() {

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

            try
            {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects('ec15fd81-be3a-e711-80d3-00155d08880e')/deleteObject()", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("Delete Projects", "mana og delete");

            }
            catch (Exception e)
            {
                Log.Info("delete Projects", "failed kay " + e.Message);
            }

        }

        public async void getTimesheetPeriod() {

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);

            try
            {
                var result = await client.GetStringAsync(siteUrl + "/sites/mobility/_api/ProjectServer/timesheetperiods");
                var data = JsonConvert.DeserializeObject<watdapak.Models.TimesheetPeriod.RootObject>(result);
                for (int i = 0; i < data.D.Results.Count; i++)
                {
                    Log.Info("timesheetz period", data.D.Results[i].Name + " " + data.D.Results[i].Id + " " + data.D.Results[i].Start);
                }

            }
            catch (Exception e)
            {
                Log.Info("timesheetz period", e.Message);
            }

        }

        public async void createTimesheet() {

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

            try
            {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/TimesheetPeriods('5be36a28-c90e-e711-80d2-00155d0cbd04')/createTimesheet()", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("Timesheetz create", "mana og create");

            }
            catch (Exception e)
            {
                Log.Info("Timesheetz create", "failed kay " + e.Message);
            }

        }

        public async void submitTimesheet() {

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

            try
            {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/TimesheetPeriods('5be36a28-c90e-e711-80d2-00155d0cbd04')/Timesheet/submit('I am submitting this timesheet through my phone')", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("Timesheetz submit", "mana og submit");

            }
            catch (Exception e)
            {
                Log.Info("Timesheetz submit", "failed kay " + e.Message);
            }

        }

        public async void getTimesheetLines() {

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);

            try
            {
                var result = await client.GetStringAsync(siteUrl + "/sites/mobility/_api/ProjectServer/TimeSheetPeriods('5ce36a28-c90e-e711-80d2-00155d0cbd04')/TimeSheet/Lines");
                var data = JsonConvert.DeserializeObject<watdapak.Models.TimesheetLines.RootObject>(result);
                for (int i = 0; i < data.D.Results.Count; i++)
                {
                    Log.Info("timesheetz lines", data.D.Results[i].ProjectName + " " + data.D.Results[i].Id + " " + data.D.Results[i].TaskName + " " + data.D.Results[i].TotalWork);
                }

            }
            catch (Exception e)
            {
                Log.Info("timesheetz lines", e.Message);
            }

        }

        public async void addTimesheetLine() {

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

            var body = "{'parameters':{'TaskName':'Task created by android'} }";
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/TimeSheetPeriods('5ce36a28-c90e-e711-80d2-00155d0cbd04')/TimeSheet/Lines/Add", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("Timesheetz add line", "mana og add");

            }
            catch (Exception e)
            {
                Log.Info("Timesheetz add line", "failed kay " + e.Message);
            }

        }

        public async void deleteTimesheetLine() {

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

            try
            {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/TimeSheetPeriods('5ce36a28-c90e-e711-80d2-00155d0cbd04')/TimeSheet/Lines('a1dd0448-9297-4bae-2cb1-6a554734fdff')/deleteObject()", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("Timesheetz delete line", "mana og delete");

            }
            catch (Exception e)
            {
                Log.Info("Timesheetz delete line", "failed kay " + e.Message);
            }

        }

        public async void getTimesheetLineWork() {

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);

            try
            {
                var result = await client.GetStringAsync(siteUrl + "/sites/mobility/_api/ProjectServer/TimeSheetPeriods('5ce36a28-c90e-e711-80d2-00155d0cbd04')/TimeSheet/Lines('0b1a124b-aca4-e6a8-5095-1d77349a4aa9')/Work");
                var data = JsonConvert.DeserializeObject<watdapak.Models.TimesheetWork.RootObject>(result);
                for (int i = 0; i < data.D.Results.Count; i++)
                {
                    Log.Info("timesheetz work", data.D.Results[i].Comment + " " + data.D.Results[i].Start + " " + data.D.Results[i].ActualWork + " " + data.D.Results[i].PlannedWork);
                }

            }
            catch (Exception e)
            {
                Log.Info("timesheetz work", e.Message);
            }

        }

        public async void addTimesheetLineWork() {

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

            var body = "'parameters':{'ActualWork':'3h', 'Start':'"+ DateTime.Now +"', 'End':'"+ DateTime.Now + "', 'Comment':'Commented by KFsama', 'NonBillableOvertimeWork':'0h', 'NonBillableWork':'0h', 'OvertimeWork':'0h', 'PlannedWork':'0h' }";
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/TimeSheetPeriods('5ce36a28-c90e-e711-80d2-00155d0cbd04')/TimeSheet/Lines('48f127c8-771b-bfce-72b9-a6923390861a')/Work/Add", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("Timesheetz add work", "mana og add");

            }
            catch (Exception e)
            {
                Log.Info("Timesheetz add work", "failed kay " + e.Message);
            }
        }

        public async void getEnterpriseResources() {

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            handler.CookieContainer.Add(new Cookie("rtFa", rtFa, "/", "sharepointevo.sharepoint.com"));
            handler.CookieContainer.Add(new Cookie("FedAuth", FedAuth, "/", "sharepointevo.sharepoint.com"));

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);

            try
            {
                var result = await client.GetStringAsync(siteUrl + "/sites/mobility/_api/ProjectServer/EnterpriseResources");
                var data = JsonConvert.DeserializeObject<watdapak.Models.EnterpriseResource.RootObject>(result);
                for (int i = 0; i < data.D.Results.Count; i++)
                {
                    Log.Info("enterprise resources", data.D.Results[i].Name + " " + data.D.Results[i].Email + " " + data.D.Results[i].Id + " " + data.D.Results[i].ResourceType);
                }

            }
            catch (Exception e)
            {
                Log.Info("enterprise resources", e.Message);
            }

        }

        public async void addEnterpriseResource() {

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

            var body = "{'parameters':{ 'Id':'" + Guid.NewGuid() + "','Name':'No one', 'ResourceType':'3' } }";
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/EnterpriseResources/add", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("enterprise", "mana og add");

            }
            catch (Exception e)
            {
                Log.Info("enterprise", "failed kay " + e.Message);
            }

        }

        public async void deleteEnterpriseResource()
        {

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

            try
            {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/EnterpriseResources('ec4df8fa-9cee-4193-8d30-5630a2d1bea4')/deleteObject()", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("enterprise", "mana og delete");

            }
            catch (Exception e)
            {
                Log.Info("enterprise", "failed kay " + e.Message);
            }

        }

        public async void updateEnterpriseResource() {

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
            client.DefaultRequestHeaders.Add("X-HTTP-METHOD","MERGE");

            var body = "{ '__metadata': { 'type': 'PS.EnterpriseResource' }, 'Name':'UPDATED RESOURCE'}";
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/EnterpriseResources('4af6b103-13ff-e611-80d3-00155d0c2609')", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("enterprise", "mana og delete");

            }
            catch (Exception e)
            {
                Log.Info("enterprise", "failed kay " + e.Message);
            }


        }

        public async void addAssignmentOnTask() {

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

            //var body = "{'parameters':{ 'Id':'" + Guid.NewGuid() + "','ResourceId':'" + new Guid("ed76d01e-1403-e711-80d4-00155d085706") + "', 'TaskId':'e1e62ba1-4706-4d5b-9a48-83a1b3b301cc' } }";
            var body = "{'parameters':{ 'ResourceId':'4af6b103-13ff-e611-80d3-00155d0c2609', 'TaskId':'ea288d31-748e-4c98-b149-d714aacf790e' } }";
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {

                var postResult = await client.PostAsync(siteUrl + "/sites/mobility/_api/ProjectServer/Projects('9f6bed59-604b-e711-80cd-00155d08a911')/Draft/Assignments/add", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                    Log.Info("projectz", "mana og add");

            }
            catch (Exception e)
            {
                Log.Info("projectz", "failed kay " + e.Message);
            }
        }

        

    }
}

