using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrueRED.Modules;
using Tweetinvi.Core.Interfaces.Streaminvi;

namespace TrueRED.Framework
{
	class ModuleManager
	{
		public static ModuleList<Module> Modules { get; private set; } = new ModuleList<Module>( ); // 어플리케이션에 로딩된 모듈 목록
		public static string ModulePath;
		private static IUserStream MainStream { get; set; }
		public static bool StreamState { get; private set; }
		private static bool StreamPaused { get; set; }
		public static string LogHeader { get; private set; } = "ModuleManager";

		public static bool LoadAllModules( string modulePath )
		{
			if ( !Directory.Exists( modulePath ) ) return false;
			ModuleManager.ModulePath = modulePath;

			var module_settings = Directory.GetFiles( ModulePath );
			SuspendStream( );
			foreach ( var item in module_settings )
			{
				if ( item.ToLower( ).EndsWith( ".ini" ) )
				{
					var module = LoadModule( item );
				}
			}
			ResumeStream( );
			RefrashStream( );

			return true;
		}
		public static bool LoadAllModules( )
		{
			return LoadAllModules( ModulePath );
		}

		private static Module LoadModule( string path )
		{
			var parser = new INIParser(path);
			var module = Module.Create( parser );
			if ( module == null )
			{
				Log.Error( LogHeader,
				string.Format( "모듈 이름 [{0}]를 로드할 수 없었습니다", module.Name ) );
			}
			if ( Modules.Find( item => item.Name == module.Name ) != null )
			{
				Log.Error( LogHeader,
				string.Format( "모듈 이름 [{0}]가 이미 존재합니다. 뒤따른 모듈은 로드되지 않습니다.", module.Name ) );
			}
			else
			{
				Modules.Add( module );
			}
			return module;
		}
		private static void UnloadModule( Module module )
		{
			Modules.Remove( module );
		}
		public static void UnloadModule( string moduleName )
		{
			var module = Modules.Find( item => item.Name == moduleName );
			UnloadModule( module );
		}

		public static void UnloadAllModule( )
		{
			Modules.Clear( );
		}
		public static void RemoveModule( string moduleName )
		{
			var module_settings = Directory.GetFiles( ModulePath );
			foreach ( var item in module_settings )
			{
				if ( item.ToLower( ).EndsWith( ".ini" ) )
				{
					if ( item.Contains( moduleName ) )
					{
						File.Delete( item );
						Log.Debug( LogHeader, string.Format( "Deleted file [{0}]", item ) );
					}
				}
			}
		}

		internal static void Initialize( bool isUseStream = true )
		{
			StreamState = isUseStream;
			MainStream = CreateStream( );

			Modules.OnModuleAttachLiestner.Add( delegate ( Module module )
			{
				if ( module is ITimeTask )
				{
					var timetask = (ITimeTask)module;
					Task.Factory.StartNew( ( ) => timetask.Run( ) );
				}
			} );
			Modules.OnModuleDetachLiestner.Add( delegate ( Module module )
			 {
				 module.Dispose( );
			 } );
			Modules.OnModuleAttachLiestner.Add( OnModuleAdd_AttachStream );
			Modules.OnModuleDetachLiestner.Add( OnModuleAdd_DetachStream );
		}
		internal static void ReloadStream()
		{
			if ( MainStream != null )
			{
				SuspendStream( );
				if ( MainStream.StreamState == Tweetinvi.Core.Enum.StreamState.Running ) MainStream.StopStream( );
				ResumeStream( );
				MainStream = CreateStream( );
			}
		}

		private static void OnModuleAdd_AttachStream( Module module )
		{
			if ( module is IStreamListener )
			{
				var listener = (IStreamListener)module;
				MainStream.TweetCreatedByAnyone += listener.TweetCreateByAnyone;
				MainStream.MessageSent += listener.MessageSent;
				MainStream.MessageReceived += listener.MessageReceived;
				MainStream.TweetFavouritedByAnyone += listener.TweetFavouritedByAnyone;
				MainStream.TweetUnFavouritedByAnyone += listener.TweetUnFavouritedByAnyone;
				MainStream.ListCreated += listener.ListCreated;
				MainStream.ListUpdated += listener.ListUpdated;
				MainStream.ListDestroyed += listener.ListDestroyed;
				MainStream.BlockedUser += listener.BlockedUser;
				MainStream.UnBlockedUser += listener.UnBlockedUser;
				MainStream.FollowedUser += listener.FollowedUser;
				MainStream.FollowedByUser += listener.FollowedByUser;
				MainStream.UnFollowedUser += listener.UnFollowedUser;
				MainStream.AuthenticatedUserProfileUpdated += listener.AuthenticatedUserProfileUpdated;
				MainStream.FriendIdsReceived += listener.FriendIdsReceived;
				MainStream.AccessRevoked += listener.AccessRevoked;
				RefrashStream( );
			}
		}

		private static void OnModuleAdd_DetachStream( Module module )
		{
			if ( module is IStreamListener )
			{
				var listener = (IStreamListener)module;
				MainStream.TweetCreatedByAnyone -= listener.TweetCreateByAnyone;
				MainStream.MessageSent -= listener.MessageSent;
				MainStream.MessageReceived -= listener.MessageReceived;
				MainStream.TweetFavouritedByAnyone -= listener.TweetFavouritedByAnyone;
				MainStream.TweetUnFavouritedByAnyone -= listener.TweetUnFavouritedByAnyone;
				MainStream.ListCreated -= listener.ListCreated;
				MainStream.ListUpdated -= listener.ListUpdated;
				MainStream.ListDestroyed -= listener.ListDestroyed;
				MainStream.BlockedUser -= listener.BlockedUser;
				MainStream.UnBlockedUser -= listener.UnBlockedUser;
				MainStream.FollowedUser -= listener.FollowedUser;
				MainStream.FollowedByUser -= listener.FollowedByUser;
				MainStream.UnFollowedUser -= listener.UnFollowedUser;
				MainStream.AuthenticatedUserProfileUpdated -= listener.AuthenticatedUserProfileUpdated;
				MainStream.FriendIdsReceived -= listener.FriendIdsReceived;
				MainStream.AccessRevoked -= listener.AccessRevoked;
				RefrashStream( );
			}
		}

		private static void RefrashStream( )
		{
			if ( MainStream == null || StreamPaused ) return;

			Log.Http( LogHeader, "유저 스트림을 재시작할게요" );
			SuspendStream( );
			if ( MainStream.StreamState == Tweetinvi.Core.Enum.StreamState.Running ) MainStream.StopStream( );
			if ( StreamState ) MainStream.StartStreamAsync( );
			ResumeStream( );
		}

		private static IUserStream CreateStream( )
		{
			var userStream = Tweetinvi.Stream.CreateUserStream();
			userStream.StreamStopped += UserStream_StreamStopped;
			userStream.StreamStarted += delegate
			{
				Log.Http( LogHeader, "유저 스트림이 활성화되었어요." );
			};
			RefrashStream( );
			return userStream;
		}

		private static void UserStream_StreamStopped( object sender, Tweetinvi.Core.Events.EventArguments.StreamExceptionEventArgs e )
		{
			Log.Error( LogHeader, "스트림이 정지됐어요. 다시 여는중..." );
			MainStream.StartStreamAsync( );
		}

		private static void SuspendStream( )
		{
			StreamPaused = true;
			MainStream.StreamStopped -= UserStream_StreamStopped;
		}
		private static void ResumeStream( )
		{
			StreamPaused = false;
			MainStream.StreamStopped += UserStream_StreamStopped;
		}
	}

	public class ModuleList<T> : List<T>
	{
		public List<Action<T>> OnModuleAttachLiestner { get; private set; } = new List<Action<T>>( );
		public List<Action<T>> OnModuleDetachLiestner { get; private set; } = new List<Action<T>>( );

		public new void Add( T item )
		{
			foreach ( var liestner in OnModuleAttachLiestner )
			{
				liestner( item );
			}
			base.Add( item );
		}

		public new void Remove( T item )
		{
			foreach ( var liestner in OnModuleDetachLiestner )
			{
				liestner( item );
			}
			base.Remove( item );
		}

	}
}
