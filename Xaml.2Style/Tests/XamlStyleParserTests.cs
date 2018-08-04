using System;
using Xaml._2Style;
using Xunit;

namespace Tests
{
	public class XamlStyleParserTests
	{
		[Xunit.Fact]
		public void DebugTest()
		{
			var text =
				"<TextBox FontSize=\"14\" Padding=\"12,14,0,0\" BorderThickness=\"1 1 1 2\" x:Name=\"input\"\r\n\t\t\t\t\t\t\tBackground=\"WhiteSmoke\" TextWrapping=\"{TemplateBinding TextWrapping}\" \r\n\t\t\t\t\t\t\tHeight=\"{TemplateBinding Height}\" AcceptsReturn=\"{TemplateBinding AcceptsReturn}\"\r\n\t\t\t\t\t\t\tText=\"{Binding RelativeSource={RelativeSource TemplatedParent}, Path=Text}\" \r\n\t\t\t\t\t\t\tMaxLength=\"{TemplateBinding MaxLength}\" Width=\"{TemplateBinding ActualWidth}\"\r\n\t\t\t\t\t\t\tStyle=\"{StaticResource TextValidationStyle}\" BorderBrush=\"{Binding RelativeSource={RelativeSource TemplatedParent}, Mode=OneWay, Path=BorderBrush}\"/>\r\n";
			var parser = new XamlStyleParser();
			var r = parser.ConvertString(text);
			Console.WriteLine(r);
			Assert.Equal(true, r.StartsWith("<Style TargetType=\"TextBox\""));
			Assert.Equal(true, r.EndsWith("</Style>"));
		}
	}
}
