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
using Newtonsoft.Json;

namespace watdapak.EPTModel
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }
    public class Result
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("ProjectDetailPages")]
        public ProjectDetailPages ProjectDetailPages { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("ImageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty("IsDefault")]
        public bool IsDefault { get; set; }
        [JsonProperty("IsManaged")]
        public bool IsManaged { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Order")]
        public int Order { get; set; }
        [JsonProperty("ProjectPlanTemplateId")]
        public string ProjectPlanTemplateId { get; set; }
        [JsonProperty("WorkflowAssociationId")]
        public string WorkflowAssociationId { get; set; }
        [JsonProperty("WorkflowAssociationName")]
        public object WorkflowAssociationName { get; set; }
        [JsonProperty("WorkspaceTemplateName")]
        public string WorkspaceTemplateName { get; set; }
    }
    public class Metadata
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("uri")]
        public string Uri { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class ProjectDetailPages
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Deferred
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

}