using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal sealed class PluginLogger : Logger, IPluginLogger
	{
		private const string PluginError = "Plugin assembly {0} cannot be loaded!";

		private IDictionary<PluginErrors, string> _errors = new Dictionary<PluginErrors, string>(5)
		{	
			[PluginErrors.UnidentifiedError]
			= "Something happened. Unidentified error occurred.",
			[PluginErrors.NoMainComponent]
			= "Plugin does not contain any accessible IMainComponent class.",
			[PluginErrors.InvalidPluginAssembly]
			= "Assembly is not valid and can't be loaded!",
			[PluginErrors.CantLoadPluginAssembly]
			= "An error occurred during loading the plugin assembly.",
			[PluginErrors.NoComponentConstructor]
			= "Type does not contain accessible constructor!",
		};
		
		private int _pluginSection;

		internal PluginLogger([NotNull] string logFilePath, Func<string, string> obfuscation): base(logFilePath, obfuscation)
		{
		}
		

		public void LogPluginLoadStart()
		{
			// current policy advice to omit logging if successfull
			_pluginSection = 1;
		}

		public void LogPluginLoadEnd(int pluginCount, int componentCount)
		{
			if (_pluginSection > 0)
			{
				var writer = CreateLog();
				writer.WriteLine($"Plugin loading finished. Loaded {pluginCount} plugins with {componentCount} components.");
				writer.WriteLine();
				writer.Dispose();
			}
			_pluginSection = 0;
		}

		public void LogInvalidPlugin(string fullPath, PluginErrors error)
		{
			StreamWriter writer = null;
			try
			{
				writer = CreateLog();
				writer.WriteLine(PluginError, fullPath);
				writer.WriteLine(_errors[error]);
			}
			catch (Exception) {}
			finally
			{
				writer?.Dispose();
			}
		}

		public void LogInvalidPlugin(string assemblyName, string typeName, PluginErrors error)
		{
			StreamWriter writer = null;
			try
			{
				writer = CreateLog();
				writer.WriteLine($"There was a problem in {assemblyName}, type {typeName}");
				writer.WriteLine(_errors[error]);
			}
			catch(Exception) { }
			finally
			{
				writer?.Dispose();
			}
		}

		public void LogInvalidPlugin(string fullPath)
		{
			StreamWriter writer = null;
			try
			{
				writer = CreateLog();
				writer.WriteLine(PluginError, fullPath);
			}
			catch(Exception) { }
			finally
			{
				writer?.Dispose();
			}
		}

		protected override void WriteHeader([NotNull] StreamWriter writer)
		{
			if(_pluginSection == 1)
			{
				writer.WriteLine("Starting loading plugins...");
				_pluginSection++;
			}
		}

		public void LogHotkeyException([NotNull] HotkeyException e)
		{
			Func<string> inject = () => e.Message;
			LogException(e.InnerException, inject);
		}
	}
}
