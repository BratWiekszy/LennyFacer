

using System;
using JetBrains.Annotations;

namespace BW.Lennier.PluginModel
{
	public interface IMainComponent : IDisposable
	{
		void Initialize();

		[CanBeNull]
		IDefineHotkeys GetHotkeyDefinition();

		[CanBeNull] 
		UiOptionsCreator GetComponentOptionsCreator();
	}
}
