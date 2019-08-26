using proms.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proms.utils
{
    public class Utils
    {
        public static object serializedArrayToObject(object[] data)
        {
            return data[0];
        }
        public static KeyValue[] pairRequestQuery(string requestQuery)
        {
            string[] pairs = requestQuery.Split('&');
            KeyValue[] pairModel = new KeyValue[pairs.Length];
            int i = 0;
            foreach (string pair in pairs)
            {
                string[] paxes = pair.Split('=');
                pairModel[i] = new KeyValue(paxes[0], paxes[1]);
                i ++;
            }
            return pairModel;
        }
    }
}