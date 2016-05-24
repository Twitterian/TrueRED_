using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TrueRED.Framework.HttpCore
{
    public class HttpHeaders : Dictionary<HttpRequestHeader, string>
    {
    }

    public class HttpParameters : Dictionary<string, object>
    {
        public void Add(string str)
        {            
            if (!string.IsNullOrWhiteSpace(str) || str.Length > 1)
            {
                int read = 0;
                int find = 0;

                if (str[0] == '?')
                    read = 1;

                string key, val;

                while (read < str.Length)
                {
                    find = str.IndexOf('=', read);
                    key = str.Substring(read, find - read);
                    read = find + 1;

                    find = str.IndexOf('&', read);
                    if (find > 0)
                    {
                        val = (find - read == 1) ? null : str.Substring(read, find - read);
                        read = find + 1;
                    }
                    else
                    {
                        val = str.Substring(read);
                        read = str.Length;
                    }

                    this[key] = val;
                }
            }
        }

        public override string ToString()
        {
            if (this.Count == 0) return null;

            StringBuilder raw = null;
            foreach (var st in this)
                if (!string.IsNullOrWhiteSpace(st.Key))
                    raw.AppendFormat("{0}={1}&", st.Key, Convert.ToString(st.Value));

            raw.Remove(raw.Length - 1, 1);
            return raw.ToString();
        }
    }
}
