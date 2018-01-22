using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BW.Lennier.PluginModel;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal sealed class OptionsFormBuilder : IDisposable
	{
		private List<UiOptionsCreator> _optionCreators;
		private List<Button>           _buttons;
		private OptionsBaloonForm      _optionsForm;
		private int _minSpaceLeft;
		private int _minSpaceRight;

		internal OptionsFormBuilder()
		{
			_optionCreators = new List<UiOptionsCreator>();
		}

		internal void AddOptionCreator([NotNull] UiOptionsCreator creator)
		{
			if(creator == null) throw new ArgumentNullException();

			_optionCreators.Add(creator);
			if (_minSpaceLeft < creator.MinSpaceLeft)
				_minSpaceLeft = creator.MinSpaceLeft;

			if (_minSpaceRight < creator.MinSpaceRight)
				_minSpaceRight = creator.MinSpaceRight;
		}

		[NotNull]
		private List<Button> CreateButtons()
		{
			return _buttons 
				?? (_buttons = _optionCreators
					.SelectMany(cr => cr.CreateOptionButtons())
					.ToList());
		} 

		internal void BuildForm()
		{
			if (_optionsForm != null)
				return;
			var allButtons = CreateButtons();
			var position   = Cursor.Position;
			var screen     = Screen.PrimaryScreen.Bounds;
			position.X     = position.X < _minSpaceLeft      ? _minSpaceLeft
				: position.X > screen.Width - _minSpaceRight ? screen.Width - _minSpaceRight
				: position.X ;

			_optionsForm = new OptionsBaloonForm(allButtons, allButtons.Count, position);
			_optionsForm.Closed += (sender, args) => _optionsForm = null;
		}

		internal void RemoveOptionsFrom([NotNull] UiOptionsCreator creator)
		{
			if(creator == null)
				throw new ArgumentNullException();

			var firstBtnId = creator.ButtonNames.FirstOrDefault();
			if (firstBtnId == null)
				return;
			var firstIndex  = _buttons.FindIndex(b => firstBtnId.Equals(b.Name));
			var buttonCount = creator.ButtonNames.Count;

			if (_optionsForm != null)
			{
				_optionsForm.Close();
			}

			for (int i = firstIndex; i < firstIndex + buttonCount; i++)
			{
				_buttons[i].Dispose();
			}
			_buttons.RemoveRange(firstIndex, buttonCount);

			_optionCreators.Remove(creator);
		}

		public void Dispose()
		{
			_optionsForm?.Dispose();
		}
	}
}
