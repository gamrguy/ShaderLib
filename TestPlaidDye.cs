using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ShaderLib.Dyes;

namespace ShaderLib
{
	public class TestPlaidDye : ModItem
	{
		public override bool Autoload(ref string name, ref string texture, System.Collections.Generic.IList<EquipType> equips)
		{
			texture = "Terraria/Item_" + ItemID.RainbowHairDye;
			return base.Autoload(ref name, ref texture, equips);
		}

		public override void SetDefaults()
		{
			item.name = "Test Plaid Dye";
			item.toolTip = "Plaid?";
			item.dye = 1; //Allows dye to be equipped
		}
	}
}

