using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BW.Lennier.PluginModel;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal delegate void AddMenu([NotNull] Func<MenuItem> item);

	internal sealed class LennyFaceComponent : IMainComponent, IDefineHotkeys
	{
		private const string ActionAddFaceText   = "Add face";
		private const string ActionShowFacesText = "Faces";

		private LennyFaceRepository _lennys;
		private ContextMenu         _menu;
		private Flyout              _addFaceFlyout;

		internal event AddMenu AddedFace;

		internal LennyFaceComponent()
		{
			_lennys    = new LennyFaceRepository();
			_menu      = new ContextMenu();
			CreateMenu();
		}

		private void CreateMenu()
		{
			foreach(var item in MenuLennys)
			{
				_menu.MenuItems.Add(item);
			}
		}

		[NotNull]
		private ActionDefinition AddFaceAction => 
			new ActionDefinition(ActionAddFaceText, (s, e) => CreateAddFaceForm());

		private void CreateAddFaceForm()
		{
			if (_addFaceFlyout != null)
				return;
			_addFaceFlyout = new Flyout(face =>
			{
				if (! _lennys.AddFace(face))
					return;
				var item = CreateMenuEntry(face);
				_menu.MenuItems.Add(item);

				AddedFace?.Invoke(() => CreateMenuEntry(face));
			});
			_addFaceFlyout.Closed += (sender, args) => _addFaceFlyout = null;
		}

		[NotNull]
		[ItemNotNull]
		internal IEnumerable<MenuItem> MenuLennys
		{
			get => _lennys.Faces.Select(face => CreateMenuEntry(face));
		}

		[NotNull]
		private MenuItem CreateMenuEntry(string face)
		{
			return new MenuItem(face, (sender, args) => _lennys.OnFaceSelection(face));
		}

		public void Initialize()
		{
			
		}

		[NotNull]
		public IDefineHotkeys GetHotkeyDefinition()
		{
			return this;
		}

		public IEnumerable<Pair<HotkeyKey, HotkeyEventHandler>> GetHotkeysToActions()
		{
			/*var key = new HotkeyKey(Keys.L, "Copies last used lenny to clipboard.",
									KeyModifiers.Control);
			yield return new Pair<HotkeyKey, HotkeyEventHandler>(key, k =>
			{
				_lennys.OnMainHotkey();
			});*/
			return new Pair<HotkeyKey, HotkeyEventHandler>[0];
		}

		[NotNull]
		public UiOptionsCreator GetComponentOptionsCreator()
		{
			var addOption   = AddFaceAction;
			var facesOption = new ActionDefinition(ActionShowFacesText, ((s, a) =>
			{
				_menu.Show((Control)s, new Point(200, -30));
			}));
			return new UiOptionsCreator(200, 400, 
				new ActionDefinition[]{ facesOption, addOption }, 2);
		}

		public void Dispose() {}
	}
}
