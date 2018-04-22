using System.Collections.Generic;
using Terraria;

namespace ShaderLib.Dyes
{
	public enum ShirtType
	{
		SHIRT,
		UNDERSHIRT,
		NONE
	}

	public static class VariantHandler
	{
		public static readonly Dictionary<int, ShirtType[]> variants = new Dictionary<int, ShirtType[]>
		{
			{ 0, new ShirtType[] { ShirtType.UNDERSHIRT, ShirtType.SHIRT, ShirtType.UNDERSHIRT, ShirtType.NONE } },
			{ 1, new ShirtType[] { ShirtType.UNDERSHIRT, ShirtType.SHIRT, ShirtType.UNDERSHIRT, ShirtType.NONE } },
			{ 2, new ShirtType[] { ShirtType.UNDERSHIRT, ShirtType.SHIRT, ShirtType.NONE, ShirtType.SHIRT} },
			{ 3, new ShirtType[] { ShirtType.SHIRT, ShirtType.UNDERSHIRT, ShirtType.SHIRT, ShirtType.NONE, ShirtType.SHIRT } },
			{ 4, new ShirtType[] { ShirtType.UNDERSHIRT, ShirtType.SHIRT, ShirtType.UNDERSHIRT, ShirtType.SHIRT } },
			{ 5, new ShirtType[] { ShirtType.UNDERSHIRT, ShirtType.SHIRT, ShirtType.UNDERSHIRT, ShirtType.NONE } },
			{ 6, new ShirtType[] { ShirtType.UNDERSHIRT, ShirtType.SHIRT, ShirtType.UNDERSHIRT, ShirtType.SHIRT } },
			{ 7, new ShirtType[] { ShirtType.SHIRT, ShirtType.UNDERSHIRT, ShirtType.SHIRT, ShirtType.UNDERSHIRT, ShirtType.SHIRT} },
			{ 8, new ShirtType[] { ShirtType.SHIRT, ShirtType.UNDERSHIRT, ShirtType.SHIRT, ShirtType.NONE, ShirtType.SHIRT } },
			{ 9, new ShirtType[] { ShirtType.UNDERSHIRT, ShirtType.SHIRT, ShirtType.NONE, ShirtType.SHIRT} }
		};

		public static void ApplyVariant(int var, int shirtID, int ushirtID, List<int> dat) {
			if(dat.Count != variants[var].Length) return;
			for(int i = 0; i < variants[var].Length; i++) {
				switch(variants[var][i]) {
					case (ShirtType.SHIRT):
						ModDyePlayer.EditData(shirtID, dat[i]);
						break;
					case (ShirtType.UNDERSHIRT):
						ModDyePlayer.EditData(ushirtID, dat[i]);
						break;
					default:
						break;
				}
			}
		}
	}
}