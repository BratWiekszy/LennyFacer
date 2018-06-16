using BW.Lennier.PluginModel;
using Xaml._2Style;

namespace Xaml2Style
{
	public class MainComponent : BW.Lennier.PluginModel.IMainComponent
	{
		private ClipboardExtractor _xamlStyleConverter;

		public void Dispose()
		{
			
		}

		public void Initialize()
		{
			_xamlStyleConverter = new ClipboardExtractor(new XamlStyleParser());
		}

		public IDefineHotkeys GetHotkeyDefinition()
		{
			return null;
		}

		public UiOptionsCreator GetComponentOptionsCreator()
		{
			var action = new ActionDefinition("Convert Xaml to Style", 
											  (s, a) => _xamlStyleConverter.ClipboardXamlToStyle());

			var option = new UiOptionsCreator(200, 200, new []{action}, 1);
			return option;
		}
	}
}
