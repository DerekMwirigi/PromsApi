using proms.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace proms.utils
{
    public class RequestService
    {
        public static object post (KeyValue[] headers, string endPoint, string payLoad)
        {
            var request = (HttpWebRequest)WebRequest.Create(endPoint);
            var data = Encoding.ASCII.GetBytes(payLoad);
            if (headers != null)
            {
                foreach (KeyValue header in headers)
                {
                    request.Headers.Add(header.key, header.value);
                }
            }
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            WebResponse response = request.GetResponse();
            switch (((HttpWebResponse)response).StatusCode)
            {
                case HttpStatusCode.OK:
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    reader.Close(); dataStream.Close(); response.Close();
                    return responseFromServer;
                default:
                    return null;

            }
        }
    }
}