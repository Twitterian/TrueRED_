using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TrueRED.Framework.HttpCore
{
	public static class HttpCommunicator
	{
		public static void Post( string url, HttpHeaders headers, HttpParameters parameters, Action<string> callback, Action<Exception, Action> errorCallback )
		{
			Excute( url, RequestType.POST, headers, parameters, callback, errorCallback );
		}

		public static void Get( string url, HttpHeaders headers, HttpParameters parameters, Action<string> callback, Action<Exception, Action> errorCallback )
		{
			Excute( url, RequestType.GET, headers, parameters, callback, errorCallback );
		}

		public enum RequestType { POST, GET }
		public static void Excute( string url, RequestType method, HttpHeaders headers, HttpParameters parameters, Action<string> callback, Action<Exception, Action> errorCallback )
		{
			Task.Factory.StartNew( delegate
			{
				string response = null;
				string rawParameters = null;
				string methodStr = null;

				try
				{
                    Uri uri = new Uri(url);
                    if (method == RequestType.GET && parameters != null)
                        uri = CombineQuery(uri, parameters);

                    var req = HttpWebRequest.Create(uri) as HttpWebRequest;
                    req.Method = method == RequestType.GET ? "GET" : "POST";
                    req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                    if (method == RequestType.POST && parameters != null)
                    {
                        req.ContentType = "application/x-www-form-urlencoded";

                        var buff = Encoding.UTF8.GetBytes(parameters.ToString());
                        req.GetRequestStream().Write(buff, 0, buff.Length);
                    }

                    if (headers != null)
                    {
                        foreach (var st in headers)
                        {
                            if (st.Key == HttpRequestHeader.ContentType)
                                req.ContentType = st.Value;
                            if (st.Key == HttpRequestHeader.UserAgent)
                                req.UserAgent = st.Value;

                            req.Headers[st.Key] = st.Value;
                        }
                    }

                    using (var res = req.GetResponse())
                    {
                        using (var stream = res.GetResponseStream())
                        {
                            var reader = new StreamReader(stream);
                            response = reader.ReadToEnd();
                        }
                    }

					Log.Http( "HttpCommon", "HTTP {0} : {1}?{2}", methodStr, url, rawParameters );

					// excute callback
					if ( callback != null )
                        callback.Invoke( response );
				}
				catch ( Exception e )
				{
                    if (errorCallback != null)
					    errorCallback.Invoke( e, () => Post( url, headers, parameters, callback, errorCallback ) );
				}
			} );
		}

		public static void GetImage( string url, Action<Image> callback, Action<Exception, Action> errorCallback )
		{
			Task.Factory.StartNew( delegate
			{
				try
				{
                    var req = WebRequest.Create(url);
                    using (var res = req.GetResponse())
                        using (var stream = res.GetResponseStream())
						    if ( callback != null )
                                callback.Invoke( Bitmap.FromStream( stream ) );
				}
				catch ( Exception e )
				{
                    if (errorCallback != null)
					    errorCallback.Invoke( e, () => GetImage( url, callback, errorCallback ));
				}
			} );
		}

        private static Uri CombineQuery(Uri uri, HttpParameters collection)
        {
            var dic = new HttpParameters();
            dic.Add(uri.Query);

            foreach (var st in collection)
                dic[st.Key] = st.Value;

            return new UriBuilder(uri) { Query = "?" + dic.ToString() }.Uri;
        }
	}
}