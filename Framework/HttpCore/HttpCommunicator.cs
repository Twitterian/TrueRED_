using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;

namespace TrueRED.Framework.HttpCore
{
	class HttpCommunicator
	{
		private HttpCommunicator( ) { }

		public static void Post( string url, IDictionary<string, string> headers = null, IDictionary<string, string> parameters = null, Action<string> callback = null, Action<Exception, Action> errorCallback = null )
		{
			Excute( url, RequestType.POST, headers, parameters, callback, errorCallback );
		}

		public static void Get( string url, IDictionary<string, string> headers = null, IDictionary<string, string> parameters = null, Action<string> callback = null, Action<Exception, Action> errorCallback = null )
		{
			Excute( url, RequestType.GET, headers, parameters, callback, errorCallback );
		}

		public enum RequestType { POST, GET }
		public static void Excute( string url, RequestType method, IDictionary<string, string> headers = null, IDictionary<string, string> parameters = null, Action<string> callback = null, Action<Exception, Action> errorCallback = null )
		{
			Task.Factory.StartNew( delegate
			{
				string response = null;
				string rawParameters = null;
				string methodStr = null;

				try
				{
					using ( WebClient wc = new WebClient( ) )
					{
						// Attatch headers
						if ( headers != null )
						{
							foreach ( var item in headers )
							{
								wc.Headers[item.Key] = item.Value;
							}
						}

						switch ( method )
						{
							case RequestType.POST:
								{
									rawParameters = inflateRawString( parameters );
									methodStr = method.ToString( );
									if ( string.IsNullOrEmpty( rawParameters ) )
									{
										response = wc.UploadString( url, methodStr, null );
									}
									else
									{
										response = wc.UploadString( url, methodStr, rawParameters );
									}
								}
								break;
							case RequestType.GET:
								{
									rawParameters = inflateRawString( parameters );
									methodStr = method.ToString( );
									if ( string.IsNullOrEmpty( rawParameters ) )
									{
										response = wc.DownloadString( url );
									}
									else
									{
										response = wc.DownloadString( url + "?" + rawParameters );
									}
								}
								break;
							default:
								break;
						}

						Log.Http( "HttpCommon", string.Format( "HTTP {0} : {1}?{2}", methodStr, url, rawParameters ) );

						// excute callback
						if ( callback != null ) callback( response );
					}
				}
				catch ( Exception e )
				{
					errorCallback( e, delegate
					{
						Post( url, headers, parameters, callback, errorCallback );
					} );
				}
			} );
		}

		public static void GetImage( string url, Action<Image> callback = null, Action<Exception, Action> errorCallback = null )
		{
			Task.Factory.StartNew( delegate
			{
				try
				{
					using ( var wc = new WebClient( ) )
					{
						using ( var stream = wc.OpenRead( url ) )
						{
							var image = new Bitmap( stream );
							callback( image );
						}
					}
				}
				catch ( Exception e )
				{
					errorCallback( e, delegate
					{
						GetImage( url, callback, errorCallback );
					} );
				}
			} );
		}

		private static string inflateRawString( IDictionary<string, string> collection )
		{
			string raw = null;
			if ( collection == null || collection.Count == 0 ) return raw;
			foreach ( KeyValuePair<string, string> post_arg in collection )
			{
				if ( string.IsNullOrEmpty( post_arg.Value ) ) continue;
				raw += post_arg.Key + "=" + post_arg.Value + "&";
			}
			return raw.Substring( 0, raw.Length - 1 );
		}
	}
}