using System.Collections.Generic;
using JetBrains.Annotations;

namespace BW.Lennier.PluginModel
{
	public interface IDefineHotkeys
	{
		[NotNull]
		IEnumerable<Pair<HotkeyKey, HotkeyEventHandler>> GetHotkeysToActions();
	}
}
