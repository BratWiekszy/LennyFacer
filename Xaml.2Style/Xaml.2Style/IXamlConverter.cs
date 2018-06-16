using JetBrains.Annotations;

namespace Xaml2Style
{
	internal interface IXamlConverter
	{
		[CanBeNull] string ConvertString([NotNull] string source);
	}
}