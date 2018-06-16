using System.Text;
using System.Text.RegularExpressions;
using Xaml2Style;

namespace Xaml._2Style
{
	public class XamlStyleParser : IXamlConverter
	{
		/// <summary>
		/// captures &lt;'TextBox' or ^TextBox or /'TextBox' 
		/// </summary>
		private Regex _controlTypeMatch;
		/// <summary>
		/// captures 'BorderThickness'="'1 1 1 2'" 'Background'="'WhiteSmoke'" 'BorderBrush'="'#ddff00'"
		/// </summary>
		private Regex _propertyMatch;

		private StringBuilder _builder;
		private int _indentCount = 0;
		private const string StyleCloseTag = "\n</Style>";
		private const string StyleStartTagTemplate = "<Style TargetType=\"{0}\" x:Key=\" \">\n";
		private const string SetterTemplate = "<Setter Property=\"{0}\" Value=\"{1}\"/>\n";

		public XamlStyleParser()
		{
			_builder = new StringBuilder(100);
			_controlTypeMatch = new Regex(@"(?<=\<)([A-Z])\w+|^([A-Z])\w+(?= )|(?<=\/)([A-Z])\w+");
			_propertyMatch = new Regex("(?<= )([A-Z]\\w+)=\"([#\\dA-Z][ ,\\d\\w+]+)\"");
		}

		public string ConvertString(string source)
		{
			_builder.Clear();
			_indentCount = 0;
			ConvertStyleTags(source);
			return _builder.ToString();
		}

		private void ConvertStyleTags(string input)
		{
			var matches = _controlTypeMatch.Matches(input);
			if (matches.Count == 0)
			{
				ConvertProperties(input);
				return;
			}
			var t = matches[0].Value;
			_indentCount = 1;
			_builder.AppendFormat(StyleStartTagTemplate, t);
			ConvertProperties(input);
			_builder.Append(StyleCloseTag);
		}

		private void ConvertProperties(string input)
		{
			var matches = _propertyMatch.Matches(input);
			foreach (Match match in matches)
			{
				ConvertSetter(match);
			}
		}

		private void ConvertSetter(Match match)
		{
			var groups = match.Groups;
			if (groups.Count != 3)
				return;
			_builder.Append('\t', _indentCount);
			_builder.AppendFormat(SetterTemplate, groups[1], groups[2]);
		}
	}
}
