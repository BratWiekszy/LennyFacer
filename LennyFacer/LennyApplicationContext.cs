using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using BW.Lennier.PluginModel;
using JetBrains.Annotations;

namespace BW.Lennier
{
	public sealed class LennyApplicationContext : ApplicationContext
	{
		private LennyFaceComponent _lennyComponent;
		private OptionsFormBuilder _optionsBuilder;
		private HotkeyDisplay      _hotkeyDisplay;
		private TrayIcon           _trayIcon;
		private HotkeyManager      _hotkeys;
		private PluginBootstraper  _pluginBootstraper;
		private Logger             _logger;

		public LennyApplicationContext()
		{
			_logger = new Logger("log.txt");
			InitUiElements();
			var handle     = new AppHandleProvider();
			_hotkeys       = new HotkeyManager(handle);
			_hotkeys.HotkeyRemoved += _hotkeyDisplay.OnHotkeyRemoved;

			RegisterHotkey();

			_pluginBootstraper = new PluginBootstraper();
			_pluginBootstraper.Bootstrap(_optionsBuilder, _hotkeys);

			_hotkeyDisplay.AddHotkeys(_pluginBootstraper.Containers);
			ThreadExit += Exit;
		}

		private void InitUiElements()
		{
			_trayIcon       = new TrayIcon();
			_lennyComponent = new LennyFaceComponent();
			_hotkeyDisplay  = new HotkeyDisplay();

			_lennyComponent.Initialize();
			_lennyComponent.AddedFace += factory => _trayIcon.AddMenuItem(factory());

			_trayIcon.SetMenuItems(_lennyComponent.MenuLennys);
			_trayIcon.AddSpecialMenuItem("Show hotkey actions",
										 (s, args) => _hotkeyDisplay.DisplayHotkey());
			_trayIcon.AddSpecialMenuItem(null, Exit, exit: true);

			_optionsBuilder = new OptionsFormBuilder();
			_optionsBuilder.AddOptionCreator(_lennyComponent.GetComponentOptionsCreator()
											 ?? throw new ArgumentNullException());
		}

		private void RegisterHotkey()
		{
			const string assembly = "Lenny Core";
			var key = new HotkeyKey(Keys.L, "Invokes Options Pane", KeyModifiers.Control);
			_hotkeyDisplay.AddHotkeyDefinition(key, assembly);

			_hotkeys.AddHotkey(key, (o, args) =>
			{
				_optionsBuilder.BuildForm();
				args.Handled = true;
			});

			var hotkeyDefinitions = 
				_lennyComponent.GetHotkeyDefinition().GetHotkeysToActions().ToList();
			_hotkeys.AddHotkeys(hotkeyDefinitions);
			foreach (var hotkey in hotkeyDefinitions.Select(h => h.First))
			{
				_hotkeyDisplay.AddHotkeyDefinition(hotkey, assembly);
			}
		}

		/// <summary>
		/// Log unhandled exception, thrown likely by plugin code. If our class threw this exception, we assume there's flaw in application and app shuts down.
		/// </summary>
		/// <param name="args"></param>
		public void HandleException([NotNull] ThreadExceptionEventArgs args)
		{
			var ex = args.Exception;
			_logger.LogException(ex);

			var declaringType = ex.TargetSite.DeclaringType;
			if (declaringType != null 
			 && declaringType.Assembly == Assembly.GetExecutingAssembly())
				Exit(this, null);
		}

		private void Exit(object sender, EventArgs args)
		{
			_hotkeys.Dispose();
			_trayIcon.Dispose();
			_lennyComponent.Dispose();
			_optionsBuilder.Dispose();

			ThreadExit -= Exit;
			ExitThread();
		}
	}
}
