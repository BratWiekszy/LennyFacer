using System;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal enum PluginErrors
	{
		UnidentifiedError,
		NoMainComponent,
		InvalidPluginAssembly,
		CantLoadPluginAssembly,
		NoComponentConstructor
	}

	internal interface ILogger
	{
		void LogException([NotNull] Exception e, Func<string> injectDesc = null);
	}


	internal interface IPluginLogger : ILogger
	{
		void LogPluginLoadStart();

		void LogPluginLoadEnd(int loadedPluginCount, int loadedComponentCount);

		void LogInvalidPlugin([NotNull] string fullPath, PluginErrors error);
		void LogInvalidPlugin([CanBeNull] string assemblyName, [NotNull] string typeName, PluginErrors error);

		void LogInvalidPlugin([NotNull] string fullPath);

		void LogHotkeyException([NotNull] HotkeyException e);
	}
}