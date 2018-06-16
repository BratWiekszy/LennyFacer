using System.Windows.Forms;
using JetBrains.Annotations;

namespace Xaml2Style
{
	internal sealed class ClipboardExtractor
	{
		[NotNull] private readonly IXamlConverter _converter;

		public ClipboardExtractor([NotNull] IXamlConverter converter)
		{
			_converter = converter;
		}

		public void ClipboardXamlToStyle()
		{
			if (Clipboard.ContainsText() == false)
				return;
			var xaml = Clipboard.GetText();
			var str = _converter.ConvertString(xaml);
			if (str == null)
				return;
			Clipboard.SetText(str);
		}

	}

}
