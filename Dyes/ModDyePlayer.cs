using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using ShaderLib.Dyes;

namespace ShaderLib.Dyes
{
	public class ModDyePlayer : ModPlayer
	{
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

		private int GetShaderID(Item i){
			if(i.GetModInfo<MetaDyeInfo>(mod).fakeItemID < 0) {
				return GameShaders.Armor.GetShaderIdFromItemId(i.GetModInfo<MetaDyeInfo>(mod).fakeItemID);
			}
			return GameShaders.Armor.GetShaderIdFromItemId(i.type);
		}
	}
}