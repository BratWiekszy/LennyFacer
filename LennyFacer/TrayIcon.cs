using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BW.Lennier.PluginModel;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal class TrayIcon : IDisposable
	{
		private const string IconPath       = "LennyFacer.ico";
		private const string DefaultTooltip = "Feeds your conversations with Lenny vibe. Ctrl+L to activate";
		private const string ExitMenuText   = "I ain't need you";

		private Icon       _lennyIcon;
		private NotifyIcon _icon;
		private Container  _container;
		private int        _specialMenuItems = 0;
		private bool       _disposed = false;
		

		internal TrayIcon()
		{
			_container    = new Container();
			_lennyIcon    = Icon.ExtractAssociatedIcon(IconPath);
			_icon         = new NotifyIcon(_container)
			{
				Icon    = _lennyIcon,
				Text    = DefaultTooltip,
				Visible = true
			};
			_icon.ContextMenuStrip = new ContextMenuStrip(_container);
			_icon.ContextMenu      = new ContextMenu();
		}

		public void Dispose()
		{
			if (_disposed)
				return;
			_disposed = true;
			_lennyIcon.Dispose();
			_icon.Dispose();
			_container.Dispose();

			_lennyIcon = null;
			_icon      = null;
			_container = null;
		}

		/// <summary>
		/// Add menu items like exit or add face.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="callback"></param>
		/// <param name="exit"></param>
		public void AddSpecialMenuItem([CanBeNull] string text, EventHandler callback, 
		bool exit = false)
		{
			if (string.IsNullOrEmpty(text) || exit)
				text = ExitMenuText;
			var menuItems = _icon.ContextMenu.MenuItems;
			menuItems.Add(menuItems.Count, new MenuItem(text, callback));
			_specialMenuItems++;
		}

		/// <summary>
		/// Add menu items like add face.
		/// </summary>
		public void AddSpecialMenuItem([NotNull] ActionDefinition action)
		{
			if(action == null) throw new ArgumentNullException();

			var menuItems = _icon.ContextMenu.MenuItems;
			menuItems.Add(menuItems.Count, new MenuItem(action.Name, action.Callback));
			_specialMenuItems++;
		}

		public void AddMenuItem([NotNull] MenuItem item)
		{
			var menuItems = _icon.ContextMenu.MenuItems;
			var count = menuItems.Count;
			_icon.ContextMenu.MenuItems.Add(count - _specialMenuItems, item);
		}

		/// <summary>
		/// Starts context menu as fresh collection of menu items.
		/// </summary>
		/// <param name="items"></param>
		public void SetMenuItems([NotNull] IEnumerable<MenuItem> items)
		{
			var menuItems = _icon.ContextMenu.MenuItems;
			menuItems.Clear();
			foreach (var item in items)
				menuItems.Add(item);
		}
	}
}
