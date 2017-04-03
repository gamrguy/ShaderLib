using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace ShaderLib.Dyes
{
	public class ModDyePlayer : ModPlayer
	{	
		internal static List<Func<int, Item, Player, int>> heldItemHooks;
		internal static List<Func<int, Item, Player, int>> heldItemFlameHooks;

		public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
		{
			//Don't ask. This is the vanilla code that applies dye to the appropriate equip types.
			//Yes, it's modified a bit to work with the modded dyes, of course.
			if (player.dye[0] != null && player.dye[0].modItem != null)
			{
				drawInfo.headArmorShader = GetShaderID(player.dye[0]);
			}
			if (player.dye[1] != null && player.dye[1].modItem != null)
			{
				drawInfo.bodyArmorShader = GetShaderID(player.dye[1]);
			}
			if (player.dye[2] != null && player.dye[2].modItem != null)
			{
				drawInfo.legArmorShader = GetShaderID(player.dye[2]);
				player.cLegs = GetShaderID(player.dye[2]);
			}
			if (player.wearsRobe)
			{
				drawInfo.legArmorShader = drawInfo.bodyArmorShader;
			}
			for (int x = 0; x < 20; x++)
			{
				int num = x % 10;
				if (player.dye[num] != null && player.armor[x].type > 0 && player.armor[x].stack > 0 && (x / 10 >= 1 || !player.hideVisual[num] || player.armor[x].wingSlot > 0 || player.armor[x].type == 934) && player.dye[num].modItem != null)
				{
					if (player.armor[x].handOnSlot > 0 && player.armor[x].handOnSlot < 19)
					{
						drawInfo.handOnShader = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].handOffSlot > 0 && player.armor[x].handOffSlot < 12)
					{
						drawInfo.handOffShader = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].backSlot > 0 && player.armor[x].backSlot < 11)
					{
						drawInfo.backShader = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].frontSlot > 0 && player.armor[x].frontSlot < 5)
					{
						drawInfo.frontShader = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].shoeSlot > 0 && player.armor[x].shoeSlot < 18)
					{
						drawInfo.shoeShader = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].waistSlot > 0 && player.armor[x].waistSlot < 12)
					{
						drawInfo.waistShader = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].shieldSlot > 0 && player.armor[x].shieldSlot < 6)
					{
						drawInfo.shieldShader = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].neckSlot > 0 && player.armor[x].neckSlot < 9)
					{
						drawInfo.neckShader = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].faceSlot > 0 && player.armor[x].faceSlot < 9)
					{
						drawInfo.faceShader = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].balloonSlot > 0 && player.armor[x].balloonSlot < 16)
					{
						drawInfo.balloonShader = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].wingSlot > 0 && player.armor[x].wingSlot < 37)
					{
						drawInfo.wingShader = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].type == 934)
					{
						drawInfo.carpetShader = GetShaderID(player.dye[num]);
					}
				}
			}

		}

		public override void PreUpdateBuffs()
		{
			//Hey! Look! More modified vanilla code!

			//Data update for armor shaders (helps with armor-created dusts e.g. Jungle set)
			if (player.dye[0] != null && player.dye[0].modItem != null)
			{
				player.cHead = GetShaderID(player.dye[0]);
			}
			if (player.dye[1] != null && player.dye[1].modItem != null)
			{
				player.cBody = GetShaderID(player.dye[1]);
			}
			if (player.dye[2] != null && player.dye[2].modItem != null)
			{
				player.cLegs = GetShaderID(player.dye[2]);
			}
			if (player.wearsRobe)
			{
				player.cLegs = player.cBody;
			}

			//Data update for accessory shaders (helps with accessory-created dusts e.g. Hermes and related boots)
			for (int x = 0; x < 20; x++)
			{
				int num = x % 10;
				if (player.dye[num] != null && player.armor[x].type > 0 && player.armor[x].stack > 0 && (x / 10 >= 1 || !player.hideVisual[num] || player.armor[x].wingSlot > 0 || player.armor[x].type == 934) && player.dye[num].modItem != null)
				{
					if (player.armor[x].handOnSlot > 0 && player.armor[x].handOnSlot < 19)
					{
						player.cHandOn = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].handOffSlot > 0 && player.armor[x].handOffSlot < 12)
					{
						player.cHandOff = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].backSlot > 0 && player.armor[x].backSlot < 11)
					{
						player.cBack = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].frontSlot > 0 && player.armor[x].frontSlot < 5)
					{
						player.cFront = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].shoeSlot > 0 && player.armor[x].shoeSlot < 18)
					{
						player.cShoe = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].waistSlot > 0 && player.armor[x].waistSlot < 12)
					{
						player.cWaist = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].shieldSlot > 0 && player.armor[x].shieldSlot < 6)
					{
						player.cShield = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].neckSlot > 0 && player.armor[x].neckSlot < 9)
					{
						player.cNeck = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].faceSlot > 0 && player.armor[x].faceSlot < 9)
					{
						player.cFace = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].balloonSlot > 0 && player.armor[x].balloonSlot < 16)
					{
						player.cBalloon = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].wingSlot > 0 && player.armor[x].wingSlot < 37)
					{
						player.cWings = GetShaderID(player.dye[num]);
					}
					if (player.armor[x].type == 934)
					{
						player.cCarpet = GetShaderID(player.dye[num]);
					}
				}
			}

			if (player.miscDyes[0] != null && player.miscDyes[0].modItem != null)
			{
				player.cPet = GetShaderID(player.miscDyes[0]);
			}
			if (player.miscDyes[1] != null && player.miscDyes[1].modItem != null)
			{
				player.cLight = GetShaderID(player.miscDyes[1]);
			}
			if (player.miscDyes[2] != null && player.miscDyes[2].modItem != null)
			{
				player.cMinecart = GetShaderID(player.miscDyes[2]);
			}
			if (player.miscDyes[3] != null && player.miscDyes[3].modItem != null)
			{
				player.cMount = GetShaderID(player.miscDyes[3]);
			}
			if (player.miscDyes[4] != null && player.miscDyes[4].modItem != null)
			{
				player.cGrapple = GetShaderID(player.miscDyes[4]);
			}
			player.cYorai = player.cPet;
		}

		public PlayerLayer heldItemLayer = new PlayerLayer("ShaderLib", "HeldItemShader", (PlayerDrawInfo drawInfo) => {
			Player player = drawInfo.drawPlayer;

			//Selects the held mouse item if it's the client-side player AND is holding one; selected inventory item otherwise
			Item heldItem = (Main.myPlayer == player.whoAmI) && Main.mouseItem.active && !Main.mouseItem.IsAir && Main.mouseItem.type > 0 ? Main.mouseItem : player.inventory[player.selectedItem];

			//Don't cause crashes :-)
			if(heldItem == null || !heldItem.active || heldItem.IsAir) return;
			int index = Main.playerDrawData.Count - 1;

			//Don't bother with items that aren't showing (e.g. Arkhalis)
			if(heldItem.noUseGraphic) return;

			//Handling for applying shaders to flame textures
			if(heldItem.flame) {
				int flameShaderID = 0;
				heldItemHooks.ForEach((hook) => { flameShaderID = hook(flameShaderID, heldItem, player); });

				if(player.itemAnimation > 0 && !player.pulley && flameShaderID > 0) {
					DrawData data = Main.playerDrawData[index];
					data.shader = flameShaderID;
					Main.playerDrawData[index] = data;
				}

				index -= 1;
			}

			//Handling for applying shaders to the held item itself
			int shaderID = 0;
			heldItemHooks.ForEach((hook) => { shaderID = hook(shaderID, heldItem, player); });

			//Only use shader when player is using/holding an item
			if(player.itemAnimation > 0 && !player.pulley && shaderID > 0) {
				DrawData data = Main.playerDrawData[index];
				data.shader = shaderID;
				Main.playerDrawData[index] = data;
			}
		});

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			if(!Main.gameMenu) {
				layers.Insert(layers.IndexOf(PlayerLayer.HeldItem) + 1, heldItemLayer);
			}
		}

		private int GetShaderID(Item i){
			/*if(i.GetModInfo<MetaDyeInfo>(mod).fakeItemID < 0) {
				return GameShaders.Armor.GetShaderIdFromItemId(i.GetModInfo<MetaDyeInfo>(mod).fakeItemID);
			}*/
			return GameShaders.Armor.GetShaderIdFromItemId(i.type);
		}
	}
}