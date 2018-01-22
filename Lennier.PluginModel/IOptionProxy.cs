using System;

namespace BW.Lennier.PluginModel
{
	internal interface IOptionProxy
	{
		EventHandler Proxy {get; set;}

		EventHandler SourceHandler { get; }
	}
}
