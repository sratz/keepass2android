﻿using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using KeePassLib.Serialization;
using keepass2android;
using keepass2android.Io;

namespace Kp2aUnitTests
{
	/// <summary>
	/// Very simple implementation of the Kp2aApp interface to be used in tests
	/// </summary>
	internal class TestKp2aApp : IKp2aApp
	{
		internal enum YesNoCancelResult
		{
			Yes, No, Cancel
		}

		private Database _db;
		private YesNoCancelResult _yesNoCancelResult = YesNoCancelResult.Yes;
		private Dictionary<PreferenceKey, bool> _preferences = new Dictionary<PreferenceKey, bool>();

		public void SetShutdown()
		{
			
		}

		public Database GetDb()
		{
			return _db;
		}

		public void StoreOpenedFileAsRecent(IOConnectionInfo ioc, string keyfile)
		{
			
		}

		public Database CreateNewDatabase()
		{
			TestDrawableFactory testDrawableFactory = new TestDrawableFactory();
			_db = new Database(testDrawableFactory, new TestKp2aApp());
			return _db;

		}

		public string GetResourceString(UiStringKey stringKey)
		{
			return stringKey.ToString();
		}

		public bool GetBooleanPreference(PreferenceKey key)
		{
			if (_preferences.ContainsKey(key))
				return _preferences[key];
			return true;
		}

		public UiStringKey? LastYesNoCancelQuestionTitle { get; set; }


		public void AskYesNoCancel(UiStringKey titleKey, UiStringKey messageKey,
			EventHandler<DialogClickEventArgs> yesHandler,
			EventHandler<DialogClickEventArgs> noHandler,
			EventHandler<DialogClickEventArgs> cancelHandler,
			Context ctx)
		{
			AskYesNoCancel(titleKey, messageKey, UiStringKey.yes, UiStringKey.no,
				yesHandler, noHandler, cancelHandler, ctx);
		}

		public void AskYesNoCancel(UiStringKey titleKey, UiStringKey messageKey,
			UiStringKey yesString, UiStringKey noString,
			EventHandler<DialogClickEventArgs> yesHandler,
			EventHandler<DialogClickEventArgs> noHandler,
			EventHandler<DialogClickEventArgs> cancelHandler,
			Context ctx)
		{
			LastYesNoCancelQuestionTitle = titleKey;
			switch (_yesNoCancelResult)
			{
				case YesNoCancelResult.Yes:
					yesHandler(null, null);
					break;
				case YesNoCancelResult.No:
					noHandler(null, null);
					break;
				case YesNoCancelResult.Cancel:
					cancelHandler(null, null);
					break;
				default:
					throw new Exception("unexpected case!");
			}
			
		}

		public Handler UiThreadHandler {
			get { return null; } //ensure everything runs in the same thread. Otherwise the OnFinish-callback would run after the test has already finished (with failure)
		}
		public IProgressDialog CreateProgressDialog(Context ctx)
		{
			return new ProgressDialogStub();
		}

		public IFileStorage GetFileStorage(IOConnectionInfo iocInfo)
		{
			return new BuiltInFileStorage();
		}

		public void SetYesNoCancelResult(YesNoCancelResult yesNoCancelResult)
		{
			_yesNoCancelResult = yesNoCancelResult;
		}

		public void SetPreference(PreferenceKey key, bool value)
		{
			_preferences[key] = value;
		}
	}
}