using System.Collections.Generic;
using System.Windows.Forms;
using BW.Lennier.PluginModel;

namespace FailingTestPlugin
{
	public class OptionFailComponent : IMainComponent, IDefineHotkeys
	{
		private FailingOptionForm _form;

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
			throw new System.NotImplementedException();
		}

		public IEnumerable<Pair<HotkeyKey, HotkeyEventHandler>> GetHotkeysToActions()
		{
			yield return 
				new Pair<HotkeyKey, HotkeyEventHandler>(
					new HotkeyKey(Keys.K, "displays failing form", KeyModifiers.Alt), Invoke);
		}

		private void Invoke(HotkeyKey key)
		{
			if (key == null)
				return;
			if (key.Key == Keys.K && key.Alt == true)
			{
				_form = new FailingOptionForm();
				_form.Visible = true;
			}
		}
	}
}
