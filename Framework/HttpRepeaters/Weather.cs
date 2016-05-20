using System;
using System.Drawing;
using System.Threading;
using Newtonsoft.Json.Linq;
using TrueRED.Framework.HttpCore;

namespace TrueRED.Framework.HttpRepeaters
{
	/// <summary>
	/// OpenWeatherMap API
	///	http://openweathermap.org/
	/// </summary>
	public class Weather : HttpRepeater
	{
		private const string HOST = "http://api.openweathermap.org/data/2.5/weather";
		private const string APIKEY = "acb233fcbbd0633a6fcab3b58d93683a";

		/// <summary>
		/// 대상 지역의 날씨를 얻어옵니다.
		/// </summary>
		/// <param name="city">도시</param>
		/// <param name="callback"></param>
		public static void getWeather( string city, Action<WeatherResult> callback )
		{
			var parameters = new HttpParameters();
			parameters.Add( "APPID", APIKEY );
			parameters.Add( "q", city );
			HttpCommunicator.Get( HOST, null, parameters,
                (response) => {
				    JObject js = JObject.Parse(response);

				    var temp = js["main"]["temp"].ToString();

				    var obj = new WeatherResult();
				    obj.tempreture = K2C( float.Parse( temp ) ).ToString( "0.##" );
                    if (callback != null)
                        callback.Invoke(obj);
			    },
                (e, retry ) => {
				    Log.Error( "Weather", "ERROR {0} : retry after 3sec", e );
                    Thread.Sleep(3000);
                    if (retry != null)
                        retry.Invoke();
			    } );
		}

		public static void getWeather( double Latitude, double Longitude, Action<WeatherResult> callback )
		{
			var parameters = new HttpParameters();
			parameters.Add( "APPID", APIKEY );
			parameters.Add( "lat", Latitude );
			parameters.Add( "lon", Longitude );
			HttpCommunicator.Get( HOST, null, parameters,
                ( response ) => {
                    JObject js = JObject.Parse(response);

                    var temp = js["main"]["temp"].ToString();
                    var weat = js["weather"][0]["main"].ToString();

                    var obj = new WeatherResult();
                    obj.tempreture = K2C(float.Parse(temp)).ToString("0.##");
                    obj.weather = weat;
                    if (callback != null)
                        callback.Invoke(obj);
                },
                (e, retry) => {
                    Log.Error("Weather", "ERROR {0} : retry after 3sec", e);
                    Thread.Sleep(3000);
                    if (retry != null)
                        retry.Invoke();
                });
		}

		public struct WeatherResult
		{

			public Point Coord { get; set; } // 좌표
			public string tempreture { get; set; }
			public string weather { get; set; }
			internal string weatherKr
			{
				get
				{
					switch ( weather )
					{
						case "Thunderstorm": //code 1x
							return "폭풍우";
						case "Drizzle": //code 1x
							return "이슬비";
						case "Rain": //code 1x
							return "비";
						case "Snow": //code 1x
							return "눈";
						case "Atmosphere": //code 1x
							return "안개";
						case "Clear": //code 1x
							return "맑음";
						case "Clouds": //code 1x
							return "구름";
						case "Extreme": //code 1x
							return "Extreme";
						case "Additional": //code 1x
							return "Additional";
						default:
							return "undefined";
					}
				}
			}
		}

		/// <summary>
		/// 켈빈 온도를 섭씨 온도로 변환합니다.
		/// </summary>
		/// <param name="k">켈빈 온도</param>
		/// <returns></returns>
		public static float K2C( float k )
		{
			//°C = K − 273.15

			var c = k - 273.15f;
			return c;
		}

		/// <summary>
		/// 섭씨 온도를 켈빈 온도로 변환합니다.
		/// </summary>
		/// <param name="c">섭씨 온도</param>
		/// <returns></returns>
		public static float C2K( float c )
		{
			//K = °C + 273.15

			var k = c + 273.15f;
			return k;
		}
	}
}
