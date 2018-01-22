using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BW.Lennier.PluginModel;

namespace FailingTestPlugin
{
	public class OptionsFail2Component : IMainComponent, IDefineHotkeys
	{
		public void Dispose()
		{
		}

		public void Initialize()
		{
		}

		public IDefineHotkeys GetHotkeyDefinition()
		{
			return this;
		}

		public UiOptionsCreator GetComponentOptionsCreator()
		{
			var creator = new UiOptionsCreator(0, 0, new ActionDefinition[]
			{
				new ActionDefinition("click fail", (s, a) => Invoke())
			}, 1);
			return creator;
		}

		private void Invoke()
		{
			throw new NotSupportedException();
		}

		private void Invoke(HotkeyKey key)
		{
			throw new ApplicationException();
		}

		public IEnumerable<Pair<HotkeyKey, HotkeyEventHandler>> GetHotkeysToActions()
		{
			yield return
				new Pair<HotkeyKey, HotkeyEventHandler>(
					new HotkeyKey(Keys.M, "fails", KeyModifiers.Alt), Invoke);
		}
	}
}
