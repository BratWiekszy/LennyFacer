using System;
using System.Collections.Generic;
using System.ComponentModel;
using BW.Lennier.PluginModel;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal sealed class PluginSandbox
	{
		private List<ComponentContainer> _containers;
		private PluginFailReactPolicy    _failReactPolicy;
		private IPluginLogger            _logger;

		internal PluginSandbox([NotNull] IPluginLogger logger,
							   [NotNull] List<ComponentContainer> containers,
							   [NotNull] OptionsFormBuilder builder,
							   [NotNull] HotkeyManager hotkeyManager
		)
		{
			_logger          = logger;
			_containers      = containers;
			_failReactPolicy = new PluginFailReactPolicy(builder, hotkeyManager);
		}

		/// <summary>
		/// Sandboxes exceptions in hotkey actions executed by plugin. 
		/// </summary>
		/// <param name="hotkeyManager"></param>
		internal void SandboxHotkeys([NotNull] HotkeyManager hotkeyManager)
		{
			foreach (var container in _containers)
			{
				SandboxHotkey(container, hotkeyManager);
			}
		}

		private void SandboxHotkey([NotNull] ComponentContainer container,
								   [NotNull] HotkeyManager hotkeyManager)
		{
			try
			{
				var builder        = new ProxyBuilder(this, container);
				var hotkeyBindings = container.CreateHotkeyBindings(builder.BuildHotkeyProxy);
				hotkeyManager.AddHotkeys(hotkeyBindings);
			}
			catch (Exception e)
			{
				var ex = new HotkeyException(e);
				_logger.LogHotkeyException(ex);
			}
		}

		/// <summary>
		/// Sandboxes actions executed from options. 
		/// Current policy is to remove plugins that fail.
		/// </summary>
		/// <param name="builder"></param>
		internal void SandboxOptions([NotNull] OptionsFormBuilder builder)
		{
			foreach (var container in _containers)
			{
				SandboxOption(container, builder);
			}

		}

		private void SandboxOption([NotNull] ComponentContainer container,
								   [NotNull] OptionsFormBuilder optionsBuilder)
		{
			try
			{
				var builder = new ProxyBuilder(this, container);
				var creator = container.CreateOptionCreator(builder.BuildOptionProxy);
				if (creator == null)
					return;
				optionsBuilder.AddOptionCreator(creator);
			}
			catch (Exception e)
			{
				_logger.LogException(e);
			}
		}

		private void LogHotkeyException([NotNull] HotkeyException e,
										[NotNull] ComponentContainer container)
		{
			_logger.LogHotkeyException(e);
			if (_failReactPolicy.RemoveOnHotkeyFail(container))
			{
				_containers.Remove(container);
			}
		}

		private void LogOptionException([NotNull] Exception e,
										[NotNull] ComponentContainer container)
		{
			_logger.LogException(e);
			if (_failReactPolicy.RemoveOnOptionsFail(container))
			{
				_containers.Remove(container);
			}
		}


		private sealed class HotkeyProxy : IHotkeyProxy
		{
			private HotkeyEventHandler _action;
			private PluginSandbox      _sandbox;
			private ComponentContainer _container;

			public HotkeyProxy(PluginSandbox sandbox, ComponentContainer container)
			{
				_sandbox = sandbox;
				_container = container;
			}

			public HotkeyEventHandler Proxy 
			{
				get { return _action; } 
				set { _action = value; }
			}

			[NotNull]
			public HandledEventHandler SourceHandler {get { return Invoke; }}

			private void Invoke(object sender, HandledEventArgs args)
			{
				var key = ((Hotkey)sender).Key;
				try
				{
					_action.Invoke(key);
				}
				catch (Exception e)
				{
					var hotkeyEx = new HotkeyException(e, key);
					_sandbox.LogHotkeyException(hotkeyEx, _container);
				}
				args.Handled = true;
			}
		}

		private sealed class OptionProxy : IOptionProxy
		{
			public  EventHandler       Proxy {get; set;}
			private PluginSandbox      _sandbox;
			private ComponentContainer _container;

			public EventHandler SourceHandler { get { return Invoke; }}

			public OptionProxy(PluginSandbox sandbox, ComponentContainer container)
			{
				_sandbox = sandbox;
				_container = container;
			}

			private void Invoke(object sender, EventArgs args)
			{
				try
				{
					Proxy.Invoke(sender, args);
				}
				catch(Exception e)
				{
					_sandbox.LogOptionException(e, _container);
				}
			}
		}

		private sealed class ProxyBuilder
		{
			private ComponentContainer _container;
			private PluginSandbox      _sandbox;

			public ProxyBuilder([NotNull] PluginSandbox sandbox,
								[NotNull] ComponentContainer container)
			{
				_sandbox   = sandbox;
				_container = container;
			}

			[NotNull]
			public IOptionProxy BuildOptionProxy() => new OptionProxy(_sandbox, _container);

			[NotNull]
			public IHotkeyProxy BuildHotkeyProxy() => new HotkeyProxy(_sandbox, _container);
		}
	}
}
