using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace BW.Lennier
{
	internal class LennyFaceRepository
	{
		private const string ResFileName = "faces.txt";
		private const string Lenny = "( ͡° ͜ʖ ͡°)";

		private List<string> _lennys = null;
		private string       _last   = Lenny;

		private List<string> SeedList {
			get => new List<string> { Lenny, "(⌐■_■)", "ಠ_ಠ", @"¯\_ツ_/¯" };
		}

		internal LennyFaceRepository()
		{
			StartupManageFaces();
		}

		internal int Count {get {return _lennys.Count;}}

		/// <summary>
		/// Loads faces.txt with lenny faces
		/// </summary>
		private void StartupManageFaces()
		{
			if(File.Exists(ResFileName) == false)
			{
				_lennys = SeedList;
				CreateFileFromLennys();
			}
			else
			{
				_lennys = File.ReadLines(ResFileName)
					.Where(l => !string.IsNullOrWhiteSpace(l))
					.ToList();
			}
		}

		private void CreateFileFromLennys()
		{
			using(var writer = File.CreateText(ResFileName))
			{
				foreach(var lenny in _lennys)
					writer.WriteLine(lenny);
			}
		}

		public IEnumerable<string> Faces {get {return _lennys;}}

		public void OnFaceSelection([NotNull] string s)
		{
			Clipboard.SetText(s);
			_last = s;
		}

		public void OnMainHotkey() => Clipboard.SetText(_last);

		/// <summary>
		/// Checks if face meets condition to be saved and if so, saves it.
		/// </summary>
		/// <param name="face"></param>
		/// <returns></returns>
		public bool AddFace(string face)
		{
			face = face.Trim();
			if (face.Length < 3)
				return false;
			if(_lennys.Contains(face) == false)
			{
				_lennys.Add(face);
				
				using(var writer = File.AppendText(ResFileName))
				{
					writer.WriteLine();
					writer.Write(face);
				}
			}
			return true;
		}
	}
}
