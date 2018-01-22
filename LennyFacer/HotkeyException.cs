using System;
using BW.Lennier.PluginModel;

namespace BW.Lennier
{
	internal sealed class HotkeyException : Exception
	{
		/// <summary>
		/// Specifies exception thrown during executing hotkey action.
		/// </summary>
		/// <param name="e"></param>
		/// <param name="key"></param>
		internal HotkeyException(Exception e, HotkeyKey key)
			: base($"Action invoked by hot key {key} caused an exception.", e)
		{
			
		}

		/// <summary>
		/// Specifies exception thrown before successful hotkey binding.
		/// </summary>
		/// <param name="e"></param>
		internal HotkeyException(Exception e)
			: base($"Plugin failed to create hot key definition or binding.", e)
		{

		}
	}
}
