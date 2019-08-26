using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proms.models
{
    public class KeyValue
    {
        public string key { set; get; }
        public string value { set; get; }

        public KeyValue(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }
}