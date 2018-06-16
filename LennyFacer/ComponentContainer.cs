using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BW.Lennier.PluginModel;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal sealed class ComponentContainer
	{
		private static readonly IEnumerable<Pair<HotkeyKey, HotkeyEventHandler>> EmptyHotkeys 
			= new Pair<HotkeyKey, HotkeyEventHandler>[0];
		private string _assemblyId;
		private IMainComponent _component;
		private UiOptionsCreator _optionCreator;
		private List<Pair<HotkeyKey, IHotkeyProxy>> _hotkeyBindings;

		internal ComponentContainer([NotNull] string assemblyId, 
									[NotNull] IMainComponent component
		)
		{
			_component  = component;
			_assemblyId = assemblyId;
		}

		[CanBeNull]
		internal UiOptionsCreator OptionCreator 
		{
			get { return _optionCreator; } 
			set { _optionCreator = value; }
		}

		internal List<Pair<HotkeyKey, IHotkeyProxy>> HotkeyBindings 
		{
			get { return _hotkeyBindings; }
			set { _hotkeyBindings = value; }

		}

		internal IMainComponent Component { get { return _component; } }

		internal string AssemblyId { get { return _assemblyId; } }

		internal bool IsSamePlugin([NotNull] ComponentContainer container)
		{
			return string.Equals(_assemblyId, container._assemblyId);
		}

		[CanBeNull]
		internal UiOptionsCreator CreateOptionCreator([NotNull] Func<IOptionProxy> proxyFactory)
		{
			_optionCreator = _component.GetComponentOptionsCreator();
			if (_optionCreator != null)
				_optionCreator.ProxyFactory = proxyFactory;
			return _optionCreator;
		}

		[NotNull]
		internal IEnumerable<Pair<HotkeyKey, HandledEventHandler>> CreateHotkeyBindings(
			[NotNull] Func<IHotkeyProxy> proxyFactory)
		{
			var hotkeyDefinition = _component.GetHotkeyDefinition();
			var hotkeys = hotkeyDefinition == null 
				? EmptyHotkeys 
				: hotkeyDefinition.GetHotkeysToActions();

			_hotkeyBindings = new List<Pair<HotkeyKey, IHotkeyProxy>>();

			foreach(var hotkey in hotkeys)
			{
				var key     = hotkey.First;
				var proxy   = proxyFactory.Invoke();
				proxy.Proxy = hotkey.Second;

				_hotkeyBindings.Add(new Pair<HotkeyKey, IHotkeyProxy>(key, proxy));
			}
			return _hotkeyBindings
				.Select(p => new Pair<HotkeyKey, HandledEventHandler>(
					p.First, p.Second.SourceHandler) );
		}
	}
}
