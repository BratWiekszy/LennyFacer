using System;
using BW.Lennier.PluginModel;

namespace FailingTestPlugin
{
	public abstract class AbstractComponent : IMainComponent
	{
		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public void Initialize()
		{
			throw new NotImplementedException();
		}

		public IDefineHotkeys GetHotkeyDefinition()
		{
			throw new NotImplementedException();
		}

		public UiOptionsCreator GetComponentOptionsCreator()
		{
			throw new NotImplementedException();
		}
	}
}
