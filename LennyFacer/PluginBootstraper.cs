using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BW.Lennier.PluginModel;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal delegate void ComponentChanged([NotNull] ComponentContainer container);

	/// <summary>
	/// Creates sandboxed plugin loader.
	/// </summary>
	internal sealed class PluginBootstraper
	{
		internal event ComponentChanged ComponentAdded;

		private const string PluginDirectory = @"Plugins";
		private List<ComponentContainer> _containers;
		private IPluginLogger            _logger;
		private PluginSandbox            _sandbox;

		internal PluginBootstraper()
		{
			_logger = new PluginLogger(PluginDirectory+"/log.txt",
									   new StackTraceObfuscator().Obfuscate);

			_containers = new List<ComponentContainer>();
			if (! Directory.Exists(PluginDirectory))
			{
				Directory.CreateDirectory(PluginDirectory);
				return;
			}

			_logger.LogPluginLoadStart();
			var loadedPlugins = LoadPlugins();

			_logger.LogPluginLoadEnd(loadedPlugins, _containers.Count);
		}

		internal void Bootstrap([NotNull] OptionsFormBuilder builder,
								[NotNull] HotkeyManager hotkeyManager)
		{
			_sandbox = new PluginSandbox(_logger, _containers, builder, hotkeyManager);
			foreach (var container in Containers)
			{
				container.Component.Initialize();
			}

			_sandbox.SandboxOptions(builder);
			_sandbox.SandboxHotkeys(hotkeyManager);
		}

		private int LoadPlugins()
		{
			var files = Directory.EnumerateFiles(PluginDirectory)
				.Where(s => string.Equals(Path.GetExtension(s), ".dll"))
				.Select(s => Path.GetFullPath(s));
			return files.Count(dllFile => LoadedAssembly(dllFile));
		}

		private bool LoadedAssembly([NotNull] string fullPath)
		{
			try
			{
				var assembly = Assembly.LoadFile(fullPath);
				var mainComponentTypes = assembly.ExportedTypes
					.Where(t => t.IsClass && typeof(IMainComponent).IsAssignableFrom(t))
					.ToList();
				if (mainComponentTypes.Count == 0)
				{
					_logger.LogInvalidPlugin(fullPath, PluginErrors.NoMainComponent);
					return false;
				}

				var assemblyName = Path.GetFileName(fullPath);
				foreach (var type in mainComponentTypes)
				{
					CreateComponent(type, assemblyName);
				}
				return true;
			}
			catch (BadImageFormatException)
			{
				_logger.LogInvalidPlugin(fullPath, PluginErrors.InvalidPluginAssembly);
			}
			catch (FileLoadException)
			{
				_logger.LogInvalidPlugin(fullPath, PluginErrors.CantLoadPluginAssembly);
			}
			catch (Exception)
			{
				_logger.LogInvalidPlugin(fullPath);
			}
			return false;
		}

		private void CreateComponent([NotNull] Type type, string assemblyName)
		{
			try
			{
				var component = (IMainComponent)Activator.CreateInstance(type);

				var container = new ComponentContainer(assemblyName, component);
				_containers.Add(container);
				ComponentAdded?.Invoke(container);
			}
			catch(MissingMethodException)
			{
				_logger.LogInvalidPlugin(assemblyName, type.FullName, PluginErrors.NoComponentConstructor);
			}
			catch(MethodAccessException)
			{
				_logger.LogInvalidPlugin(assemblyName, type.FullName, PluginErrors.NoComponentConstructor);
			}
			catch(Exception)
			{
				_logger.LogInvalidPlugin(assemblyName, type.FullName, PluginErrors.UnidentifiedError);
			}
		}

		internal IEnumerable<ComponentContainer> Containers {get { return _containers; }}
	}
}
