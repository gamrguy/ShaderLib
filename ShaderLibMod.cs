using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TerraUI;
using TerraUI.Utilities;

namespace ItemCustomizer
{
	public class CustomizerMod : Mod
	{
		//public static UI.Windows.CustomizerUI gui;
		public static bool guiOn;

		public CustomizerMod()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public override void Load()
		{
			UIUtils.Mod = this;
			UIUtils.Subdirectory = "TerraUI/";
		}

		public override void AddRecipeGroups()
		{
			Item item = new Item();
			item.SetDefaults(ItemID.StrangePlant1);
			RecipeGroup group = new RecipeGroup(() => Lang.misc[37] + " " + item.name, new int[]
				{
					ItemID.StrangePlant1,
					ItemID.StrangePlant2,
					ItemID.StrangePlant3,
					ItemID.StrangePlant4
				});
			RecipeGroup.RegisterGroup("ItemCustomizer:AnyStrangePlant", group);
		}

		public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
			UIPlayer player = Main.player[Main.myPlayer].GetModPlayer<UIPlayer>(this);

			if(!Main.playerInventory)
				guiOn = false;

			if(guiOn) {
				player.DrawUI(spriteBatch);
			}
		}
	}
}

