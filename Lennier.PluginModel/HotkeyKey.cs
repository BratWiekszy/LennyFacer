using System;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace BW.Lennier.PluginModel
{
	public delegate void HotkeyEventHandler([NotNull] HotkeyKey key);

	[Flags]
	public enum KeyModifiers
	{
		Alt     = 1,
		Control = 2,
		Shift   = 4
	}

	/// <summary>
	/// Defines key/keys combination that can be used to invoke actions.
	/// </summary>
	public sealed class HotkeyKey : IEquatable<HotkeyKey>
	{
		public HotkeyKey(Keys key, [NotNull] string hotkeyDescription,
						 KeyModifiers modifiers)
		{
			if(string.IsNullOrWhiteSpace(hotkeyDescription))
				throw new ArgumentNullException(nameof(hotkeyDescription));

			HotkeyDescription = hotkeyDescription;
			Key     = key;
			Alt     = (modifiers & KeyModifiers.Alt)     != 0;
			Control = (modifiers & KeyModifiers.Control) != 0;
			Shift   = (modifiers & KeyModifiers.Shift)   != 0;
			if(modifiers == 0)
				throw new ArgumentException("You must specify at least one modifier!");
		}

		public string HotkeyDescription { get; }

		public Keys Key     { get; }
		public bool Alt     { get; }
		public bool Control { get; }
		public bool Shift   { get; }

		public bool AnyModifier {get {return Alt || Control || Shift;}}

		public override bool Equals(object obj)
		{
			return Equals(obj as HotkeyKey);
		}

		public bool Equals(HotkeyKey other)
		{
			return other   != null &&
				   Key     == other.Key &&
				   Alt     == other.Alt &&
				   Control == other.Control &&
				   Shift   == other.Shift;
		}

		public override int GetHashCode()
		{
			var hashCode = -1938637507;
			hashCode = hashCode * -1521134295 + Key.GetHashCode();
			hashCode = hashCode * -1521134295 + Alt.GetHashCode();
			hashCode = hashCode * -1521134295 + Control.GetHashCode();
			hashCode = hashCode * -1521134295 + Shift.GetHashCode();
			return hashCode;
		}

		public override string ToString()
		{
			var mod = Control? "Ctrl + " : "";
			mod    += Alt    ? "Alt + "  : "";
			mod    += Shift  ? "Shift + ": "";
			return mod + Key;
		}
	}
}
