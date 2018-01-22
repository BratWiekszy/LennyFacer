using System;
using System.IO;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal class Logger : ILogger
	{
		protected Func<string, string> _obfuscator;
		protected string _logFile;
		protected bool   _started;

		internal Logger([NotNull] string logFilePath,
						[CanBeNull] Func<string, string> obfuscator = null)
		{
			_logFile    = logFilePath;
			_obfuscator = obfuscator;
			if (obfuscator == null)
				_obfuscator = (s) => s;
		}

		[NotNull]
		protected StreamWriter CreateLog()
		{
			var writer = File.Exists(_logFile) == false
					   ? File.CreateText(_logFile) : File.AppendText(_logFile);
			if(!_started)
			{
				writer.WriteLine();
				writer.WriteLine($"*** [{DateTime.Now}] ***");
				WriteHeader(writer);
				_started = true;
			}
			else
				writer.WriteLine($"> [{DateTime.Now}]: ");
			return writer;
		}

		protected virtual void WriteHeader([NotNull] StreamWriter writer)
		{
			writer.WriteLine("Application started...");
		}

		public void LogException([NotNull] Exception e,
		[CanBeNull] Func<string> injectDesc = null)
		{
			var writer = CreateLog();
			writer.WriteLine(injectDesc?.Invoke());
			writer.WriteLine($"An exception occurred: {e.Message} in {e.Source}");
			writer.WriteLine(_obfuscator(e.StackTrace));
			writer.Dispose();
		}
	}
}
