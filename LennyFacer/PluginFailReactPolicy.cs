using JetBrains.Annotations;

namespace BW.Lennier
{
	/// <summary>
	/// Determines reactions on plugin errors.
	/// Current policy suggest component removal. 
	/// </summary>
	internal sealed class PluginFailReactPolicy
	{
		[NotNull] private readonly OptionsFormBuilder _optionsBuilder;
		[NotNull] private readonly HotkeyManager      _hotkeyManager;

		internal PluginFailReactPolicy([NotNull] OptionsFormBuilder optionsBuilder,
									   [NotNull] HotkeyManager hotkeyManager)
		{
			_optionsBuilder = optionsBuilder;
			_hotkeyManager  = hotkeyManager;
		}

		internal bool RemoveOnOptionsFail([NotNull] ComponentContainer component)
		{
			var creator = component.OptionCreator;
			if (creator != null)
			{
				_optionsBuilder.RemoveOptionsFrom(creator);
			}
			component.OptionCreator = null;
			return ! AnyConnection(component);
		}

		internal bool RemoveOnHotkeyFail([NotNull] ComponentContainer component)
		{
			var bindings = component.HotkeyBindings;
			foreach (var binding in bindings)
			{
				_hotkeyManager.RemoveHotkey(binding.First, binding.Second.SourceHandler);
			}
			component.HotkeyBindings = null;
			return ! AnyConnection(component);
		}

		[Pure]
		private static bool AnyConnection([NotNull] ComponentContainer component)
		{
			return component.OptionCreator != null || component.HotkeyBindings != null;
		}
	}
}
