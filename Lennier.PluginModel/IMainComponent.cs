

using System;
using JetBrains.Annotations;

namespace BW.Lennier.PluginModel
{
	public interface IMainComponent : IDisposable
	{
		void Initialize();

		[NotNull]
		IDefineHotkeys GetHotkeyDefinition();

		[CanBeNull] 
		UiOptionsCreator GetComponentOptionsCreator();
	}
}
