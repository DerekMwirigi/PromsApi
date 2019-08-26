using Newtonsoft.Json;
using proms.controllers;
using proms.models;
using proms.models.common;
using proms.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proms.api.mpesa
{
    public partial class callback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string payLoad;
            using (var reader = new StreamReader(Request.InputStream))
                payLoad = reader.ReadToEnd();
            Response response = new PaymentController().mpesaCallback(JsonConvert.DeserializeObject<STKCallbackResponse>(payLoad), new Commoner(0, Request.QueryString["token"], null, null));

            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(JsonConvert.SerializeObject(response));
            Response.End();
        }
    }
}