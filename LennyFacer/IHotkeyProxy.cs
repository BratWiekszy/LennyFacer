using System.ComponentModel;
using BW.Lennier.PluginModel;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal interface IHotkeyProxy
	{
		[NotNull]
		HotkeyEventHandler Proxy { get; set; }

		[NotNull]
		HandledEventHandler SourceHandler { get; }
	}
}
