using System;
using System.Collections.Generic;
using System.ComponentModel;
using BW.Lennier.PluginModel;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal delegate void HotkeyChanged([NotNull] HotkeyKey key);

	internal sealed class HotkeyManager : IDisposable
	{
		internal event HotkeyChanged HotkeyAdded;
		internal event HotkeyChanged HotkeyRemoved;

		private Dictionary<HotkeyKey, Hotkey> _hotkeys;
		private AppHandleProvider _handleProvider;
		private bool _disposed = false;

		internal HotkeyManager([NotNull] AppHandleProvider handleProvider)
		{
			_handleProvider = handleProvider ?? throw new ArgumentNullException();
			_hotkeys = new Dictionary<HotkeyKey, Hotkey>();
		}

		internal void AddHotkey([NotNull] HotkeyKey key, HandledEventHandler callback)
		{
			if (!_hotkeys.TryGetValue(key, out Hotkey hotkey))
			{
				hotkey = new Hotkey(key);
				_hotkeys.Add(key, hotkey);
				hotkey.Pressed += callback;
				hotkey.Register(_handleProvider.Handle);
			}
			else
				hotkey.Pressed += callback;
			HotkeyAdded?.Invoke(key);
		}

		[NotNull]
		internal static HandledEventHandler HotkeyHandlerMapping(HotkeyEventHandler callback)
		{
			return (s, args) => callback(((Hotkey)s).Key);
		}

		public void AddHotkeys(
		[NotNull] IEnumerable<Pair<HotkeyKey, HotkeyEventHandler>> hotkeys)
		{
			foreach (var hotkey in hotkeys)
			{
				AddHotkey(hotkey.First, HotkeyHandlerMapping(hotkey.Second));
			}
		}

		public void AddHotkeys(
			[NotNull] IEnumerable<Pair<HotkeyKey, HandledEventHandler>> hotkeys)
		{
			foreach(var hotkey in hotkeys)
			{
				AddHotkey(hotkey.First, hotkey.Second);
			}
		}

		public void RemoveHotkey([NotNull] HotkeyKey key, HandledEventHandler handler)
		{
			try
			{
				var hotkey = _hotkeys[key];
				hotkey.Pressed -= handler;
				if (hotkey.AnySubscribtions)
					return;
				hotkey.Dispose();
				_hotkeys.Remove(key);
				HotkeyRemoved?.Invoke(key);
			}
			catch (KeyNotFoundException e)
			{
				throw new InvalidOperationException("removing not existing key", e);
			}
		}


		public void Dispose()
		{
			if(_disposed == true)
				throw new InvalidOperationException();
			_disposed = true;
			foreach (var hotkey in _hotkeys)
			{
				if(hotkey.Value.Registered)
					hotkey.Value.Dispose();
			}
			_handleProvider.DestroyHandle();
		}
	}
}
