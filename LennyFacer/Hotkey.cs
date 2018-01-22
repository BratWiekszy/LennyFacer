using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BW.Lennier.PluginModel;
using JetBrains.Annotations;

namespace BW.Lennier
{
	/// <summary>
	/// This is a modified version of global hotkey class by Max Bolingbroke
	/// <see>https://bloggablea.wordpress.com/2007/05/01/global-hotkeys-with-net/</see>
	/// </summary>
	internal sealed class Hotkey : IMessageFilter, IDisposable
	{
		#region Interop
		/// <summary>
		/// 0 means error
		/// </summary>
		/// <param name="hWnd">handle</param>
		/// <param name="id"></param>
		/// <param name="fsModifiers"></param>
		/// <param name="vk"></param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError = true)]
		private static extern int RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

		/// <summary>
		/// 0 means error
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError=true)]
		private static extern int UnregisterHotKey(IntPtr hWnd, int id);

		private const uint WM_HOTKEY = 0x312;

		private const uint MOD_ALT     = 0x1;
		private const uint MOD_CONTROL = 0x2;
		private const uint MOD_SHIFT   = 0x4;

		private const uint ERROR_HOTKEY_ALREADY_REGISTERED = 1409;

		#endregion

		private static int currentID;
		private const  int maximumID = 0xBFFF;
		
		private HotkeyKey _key;
		private IntPtr    handle;
		private int       id;
		private bool      registered;

		public bool Registered
		{
			get { return registered; }
		}

		public HotkeyKey Key {get {return _key;}}

		public event HandledEventHandler Pressed;

		internal bool AnySubscribtions { get { return Pressed != null; } }

		public Hotkey([NotNull] HotkeyKey key)
		{
			_key = key ?? throw new ArgumentNullException();
			Application.AddMessageFilter(this);
		}

		~Hotkey()
		{
			Dispose();
		}

		public bool CanRegister(IntPtr handle)
		{
			// Handle any exceptions: they mean "no, you can't register" :)
			try
			{
				// Attempt to register
				if (! Register(handle))
					return false;

				Dispose();
				return true;
			}
			catch (Win32Exception)
			{ return false; }
			catch (NotSupportedException)
			{ return false; }
		}

		public bool Register(IntPtr handle)
		{
			// Check that we have not registered
			if (registered)
				throw new NotSupportedException("You cannot register a hotkey that is already registered"); 

			// Get an ID for the hotkey and increase current ID
			id = Hotkey.currentID;
			Hotkey.currentID = Hotkey.currentID + 1 % Hotkey.maximumID;

			// Translate modifier keys into unmanaged version
			uint modifiers =  (_key.Alt     ? Hotkey.MOD_ALT     : 0) 
							| (_key.Control ? Hotkey.MOD_CONTROL : 0) 
							| (_key.Shift   ? Hotkey.MOD_SHIFT   : 0);

			if (Hotkey.RegisterHotKey(handle, id, modifiers, _key.Key) == 0)
			{
				// Is the error that the hotkey is registered?
				if (Marshal.GetLastWin32Error() == ERROR_HOTKEY_ALREADY_REGISTERED)
					return false;

				throw new Win32Exception();
			}

			// Save the control reference and register state
			registered  = true;
			this.handle = handle;
			return true;
		}
		public bool PreFilterMessage(ref Message message)
		{
			// Only process WM_HOTKEY messages
			if (message.Msg != WM_HOTKEY)
				return false;

			// Check that the ID is our key and we are registerd
			if (registered && message.WParam.ToInt64() == id)
				return OnPressed();
			
			return false;
		}

		private bool OnPressed()
		{
			HandledEventArgs handledEventArgs = new HandledEventArgs(false);
			Pressed?.Invoke(this, handledEventArgs);

			// Return whether we handled the event or not
			return handledEventArgs.Handled;
		}

		public void Dispose()
		{
			if (! registered)
				throw new InvalidOperationException("You cannot unregister a hotkey that is not registered"); 
		
			// It's possible that the control itself has died: in that case, no need to unregister!
			
			// Clean up after ourselves
			if (Hotkey.UnregisterHotKey(handle, id) == 0)
				throw new Win32Exception();

			// Clear the control reference and register state
			registered = false;
			Pressed    = null;
			GC.SuppressFinalize(this);
		}
	}
}
