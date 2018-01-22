using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal class StackTraceObfuscator
	{
		private Regex _regex;

		internal StackTraceObfuscator()
		{
			_regex = new Regex(@"(BW\.Lennier\..*)");
		}

		[NotNull]
		internal string Obfuscate([NotNull] string stackTrace)
		{
			return _regex.Replace(stackTrace, "[Internal Code]");
		}
	}
}
