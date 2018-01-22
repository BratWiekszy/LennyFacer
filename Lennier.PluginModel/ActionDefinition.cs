using System;
using JetBrains.Annotations;

namespace BW.Lennier.PluginModel
{
	[UsedImplicitly]
	public sealed class ActionDefinition
	{
		private string       _name;
		private EventHandler _callback;

		public ActionDefinition([NotNull] string actionName,
			[NotNull] EventHandler actionCallback)
		{
			_name     = actionName;
			_callback = actionCallback;
		}

		public string       Name     { get => _name;}
		public EventHandler Callback { get => _callback;}
	}
}
