using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ShaderLib.Dyes;

namespace ShaderLib
{
	public class TestMetaDye : ModItem
	{
		public override bool Autoload(ref string name, ref string texture, System.Collections.Generic.IList<EquipType> equips)
		{
			texture = "Terraria/Item_" + ItemID.TwilightHairDye;
			return base.Autoload(ref name, ref texture, equips);
		}

		public override void SetDefaults()
		{
			item.name = "Test Meta Dye";
			item.toolTip = "Instructions:\nprimary in slot 1\nsecondary in slot 2\neffect in slot 3";
			item.maxStack = 1;
			item.dye = 1; //Allows dye to be equipped
			item.useStyle = 1;
			item.useAnimation = 5;
			item.useTime = 5;
		}

		public override bool UseItem(Player player)
		{
			MetaDyeInfo meta = item.GetModInfo<MetaDyeInfo>(mod);
			meta.components.Clear();
			meta.SetEffect(player.inventory[0], MetaDyeInfo.DyeEffects.PRIMARY);
			meta.SetEffect(player.inventory[1], MetaDyeInfo.DyeEffects.SECONDARY);
			meta.SetEffect(player.inventory[2], MetaDyeInfo.DyeEffects.TYPE | MetaDyeInfo.DyeEffects.SATURATION);
			meta.fakeItemID = MetaDyeLoader.FindPreexistingShader(meta);
			if(meta.fakeItemID != -1) {
				Main.NewText(meta.fakeItemID.ToString());
				return true;
			}
			meta.fakeItemID = ShaderReflections.BindArmorShaderNoID<ModArmorShaderData>(meta.GetDataFromComponents());
			Main.NewText(meta.fakeItemID.ToString());

			return true;
		}
	}
}

