using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace BW.Lennier.PluginModel
{
	public sealed class UiOptionsCreator
	{
		private static int _lastSeed = 0;

		private int _minSpaceLeft;
		private int _minSpaceRight;
		private IEnumerable<ActionDefinition> _definitions;
		private List<string>                  _buttonNames;
		private Func<IOptionProxy>            _proxyFactory;
		private bool _counted = false;

		/// <summary>
		/// Responsible for building options definition with right event. 
		/// Also controls space that must be left around options form.
		/// </summary>
		/// <param name="minSpaceLeft">Minimal pixel space at left side of options</param>
		/// <param name="minSpaceRight">Minimal pixel space at right side of options</param>
		/// <param name="optionDefinitions">definitions for options</param>
		/// <param name="count">Count of options</param>
		public UiOptionsCreator(int minSpaceLeft, int minSpaceRight,
		[ItemNotNull] [NotNull] IEnumerable<ActionDefinition> optionDefinitions, int count )
		{
			_minSpaceLeft  = minSpaceLeft;
			_minSpaceRight = minSpaceRight;
			_definitions   = optionDefinitions;
			if (optionDefinitions.Any(option => option == null))
			{
				throw new ArgumentNullException("option");
			}
			_buttonNames = new List<string>(count);
		}

		internal int OptionCount
		{
			get
			{
				if(_counted == false) throw new InvalidOperationException("actual count isn't reliable, check count only after creation.");

				return _buttonNames.Count;
			}
		}

		internal int MinSpaceLeft  {get {return _minSpaceLeft;}}

		internal int MinSpaceRight {get {return _minSpaceRight;}}

		internal List<string> ButtonNames {get {return _buttonNames;}}

		/// <summary>
		/// Factory method to create proxies between options and plugin code.
		/// </summary>
		internal Func<IOptionProxy> ProxyFactory 
		{
			get {return _proxyFactory;} 
			set { _proxyFactory = value;}
		}

		[Pure,
		ItemNotNull,
		NotNull,
		UsedImplicitly]
		internal IEnumerable<Button> CreateOptionButtons()
		{
			return _definitions.Select((d, i) => CreateButton(d, i));
		}

		[Pure]
		[NotNull]
		private Button CreateButton([NotNull] ActionDefinition definition, int i)
		{
			if (_buttonNames.Count == i)
			{
				_buttonNames.Add("optBtn" + (i + _lastSeed));
				_lastSeed++;
				_counted = true;
			}
			string name = _buttonNames[i];

			var button = new Button()
			{
				Name = name,
				Text = definition.Name,
			};
			if (_proxyFactory == null)
				button.Click += definition.Callback;
			else
			{
				var proxy     = _proxyFactory.Invoke();
				button.Click += proxy.SourceHandler;
				proxy.Proxy  += definition.Callback;
			}
			return button;
		}
	}
}
