using System;
using BW.Lennier.PluginModel;

namespace FailingTestPlugin
{
	public class ConstructorFailComponent : IMainComponent
	{
		public ConstructorFailComponent()
		{
			throw new AccessViolationException();
		}

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
