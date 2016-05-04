namespace TrueRED.Framework
{
	class Singleton<T> where T : class, new()
	{
		private static readonly object _padlock = new object();
		private static T _instance = default(T);
		public static T Instance
		{
			get
			{
				lock ( _padlock )
				{
					if ( _instance == null ) _instance = new T( );
				}
				return _instance;
			}
		}
	}
}
