using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BW.Lennier.PluginModel;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal sealed class HotkeyDisplay
	{
		private HotkeyDisplayForm  _displayForm;
		private List<ListViewItem> _items;
		private ListViewItem[]     _cachedItems;

		internal HotkeyDisplay()
		{
			_items = new List<ListViewItem>();
		}

		internal void AddHotkeyDefinition([NotNull] HotkeyKey key, [NotNull] string assembly)
		{
			var listItem = new ListViewItem(
				new string[] { key.ToString(), key.HotkeyDescription }, -1);
			listItem.ToolTipText = assembly;
			_items.Add(listItem);
			_cachedItems = null;
			_displayForm?.Close();
		}

		internal void AddHotkeys([NotNull] IEnumerable<ComponentContainer> containers)
		{
			foreach (var container in containers)
			{
				foreach (var key in container.HotkeyBindings.Select(b => b.First))
				{
					AddHotkeyDefinition(key, container.AssemblyId);
				}
			}
		}

		[NotNull]
		internal HotkeyChanged OnHotkeyRemoved 
		{
			get 
			{ 
				return key =>
				{
					var index = _items.FindIndex(item =>
					{
						var items = item.SubItems;
						return items[0].Text == key.ToString()
							   && items[1].Text == key.HotkeyDescription;
					});
					_items.RemoveAt(index);
					_cachedItems = null;
					_displayForm?.Close();
				};
			}
		}

		internal void DisplayHotkey()
		{
			if (_cachedItems == null) _cachedItems = _items.ToArray();
			if (_displayForm != null)
				return;
			_displayForm = new HotkeyDisplayForm(_cachedItems);
			_displayForm.Closed += (sender, args) => _displayForm = null;
		}
	}
}
