using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace ShaderLib.System
{
	public class ModDyePlayer : ModPlayer
	{
		/*public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo) {
			//Don't ask. This is the vanilla code that applies dye to the appropriate equip types.
			//Yes, it's modified a bit to work with the modded dyes, of course.
			if(player.dye[0] != null) {
				drawInfo.headArmorShader = ShaderLoader.GetShaderIDNum(player.dye[0]);
			}
			if(player.dye[1] != null) {
				drawInfo.bodyArmorShader = ShaderLoader.GetShaderIDNum(player.dye[1]);
			}
			if(player.dye[2] != null) {
				drawInfo.legArmorShader = ShaderLoader.GetShaderIDNum(player.dye[2]);
				player.cLegs = ShaderLoader.GetShaderIDNum(player.dye[2]);
			}
			if(player.wearsRobe) {
				drawInfo.legArmorShader = drawInfo.bodyArmorShader;
			}
			for (int x = 0; x < 20; x++)
			{
				int num = x % 10;
				if (player.dye[num] != null && player.armor[x].type > 0 && player.armor[x].stack > 0 && (x / 10 >= 1 || !player.hideVisual[num] || player.armor[x].wingSlot > 0 || player.armor[x].type == 934))
				{
					if (player.armor[x].handOnSlot > 0 && player.armor[x].handOnSlot < 19)
					{
						drawInfo.handOnShader = ShaderLoader.GetShaderIDNum(player.dye[num]);
					}
					if (player.armor[x].handOffSlot > 0 && player.armor[x].handOffSlot < 12)
					{
						drawInfo.handOffShader = ShaderLoader.GetShaderIDNum(player.dye[num]);
					}
					if (player.armor[x].backSlot > 0 && player.armor[x].backSlot < 11)
					{
						drawInfo.backShader = ShaderLoader.GetShaderIDNum(player.dye[num]);
					}
					if (player.armor[x].frontSlot > 0 && player.armor[x].frontSlot < 5)
					{
						drawInfo.frontShader = ShaderLoader.GetShaderIDNum(player.dye[num]);
					}
					if (player.armor[x].shoeSlot > 0 && player.armor[x].shoeSlot < 18)
					{
						drawInfo.shoeShader = ShaderLoader.GetShaderIDNum(player.dye[num]);
					}
					if (player.armor[x].waistSlot > 0 && player.armor[x].waistSlot < 12)
					{
						drawInfo.waistShader = ShaderLoader.GetShaderIDNum(player.dye[num]);
					}
					if (player.armor[x].shieldSlot > 0 && player.armor[x].shieldSlot < 6)
					{
						drawInfo.shieldShader = ShaderLoader.GetShaderIDNum(player.dye[num]);
					}
					if (player.armor[x].neckSlot > 0 && player.armor[x].neckSlot < 9)
					{
						drawInfo.neckShader = ShaderLoader.GetShaderIDNum(player.dye[num]);
					}
					if (player.armor[x].faceSlot > 0 && player.armor[x].faceSlot < 9)
					{
						drawInfo.faceShader = ShaderLoader.GetShaderIDNum(player.dye[num]);
					}
					if (player.armor[x].balloonSlot > 0 && player.armor[x].balloonSlot < 16)
					{
						drawInfo.balloonShader = ShaderLoader.GetShaderIDNum(player.dye[num]);
					}
					if (player.armor[x].wingSlot > 0 && player.armor[x].wingSlot < 37)
					{
						drawInfo.wingShader = ShaderLoader.GetShaderIDNum(player.dye[num]);
					}
					if (player.armor[x].type == 934)
					{
						drawInfo.carpetShader = ShaderLoader.GetShaderIDNum(player.dye[num]);
					}
				}
			}

		}*/

		public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
		{
			ApplyArmorShaders(ref drawInfo);
			ApplyAccessoryShaders(ref drawInfo);
		}

		public override void PreUpdateBuffs()
		{
			ApplyArmorShaders();
			ApplyAccessoryShaders();
			ApplyMiscShaders();
		}

		private void ApplyArmorShaders(ref PlayerDrawInfo drawInfo)
		{
			if (!player.HeadDye().IsAir)
			{
				drawInfo.headArmorShader = ShaderLoader.GetShaderIDNum(player.HeadDye());
			}
			if (!player.BodyDye().IsAir)
			{
				drawInfo.bodyArmorShader = ShaderLoader.GetShaderIDNum(player.BodyDye());
			}
			if (!player.LegsDye().IsAir)
			{
				drawInfo.legArmorShader = ShaderLoader.GetShaderIDNum(player.LegsDye());
				player.cLegs = ShaderLoader.GetShaderIDNum(player.LegsDye());
			}
			if (player.wearsRobe)
			{
				drawInfo.legArmorShader = drawInfo.bodyArmorShader;
			}
		}
		private void ApplyArmorShaders()
		{
			if (player.HeadDye() != null)
			{
				player.cHead = ShaderLoader.GetShaderIDNum(player.HeadDye());
			}
			if (player.BodyDye() != null)
			{
				player.cBody = ShaderLoader.GetShaderIDNum(player.BodyDye());
			}
			if (player.LegsDye() != null)
			{
				player.cLegs = ShaderLoader.GetShaderIDNum(player.LegsDye());
			}
			if (player.wearsRobe)
			{
				player.cLegs = player.cBody;
			}
		}
		private void ApplyAccessoryShaders(ref PlayerDrawInfo drawInfo)
		{
			int shaderId;

			void ApplyShader(ref int output, int slot, int max)
			{
				if (slot > 0 && slot < max)
				{
					output = shaderId;
				}
			}

			for (int x = 0; x < 20; x++)
			{
				int num = x % 10;
				shaderId = ShaderLoader.GetShaderIDNum(player.dye[num]);
				Item accessory = player.armor[x];

				if (player.dye[num] != null && accessory.type != ItemID.None && accessory.stack > 0 
					&& (x / 10 >= 1 || !player.hideVisual[num] || accessory.wingSlot > 0 || accessory.type == ItemID.FlyingCarpet))
				{
					ApplyShader(ref drawInfo.handOnShader, accessory.handOnSlot, 19);
					ApplyShader(ref drawInfo.handOffShader, accessory.handOffSlot, 12);
					ApplyShader(ref drawInfo.backShader, accessory.backSlot, 11);
					ApplyShader(ref drawInfo.frontShader, accessory.frontSlot, 5);
					ApplyShader(ref drawInfo.shoeShader, accessory.shoeSlot, 18);
					ApplyShader(ref drawInfo.waistShader, accessory.waistSlot, 12);
					ApplyShader(ref drawInfo.shieldShader, accessory.shieldSlot, 6);
					ApplyShader(ref drawInfo.neckShader, accessory.neckSlot, 9);
					ApplyShader(ref drawInfo.faceShader, accessory.faceSlot, 9);
					ApplyShader(ref drawInfo.balloonShader, accessory.balloonSlot, 16);
					ApplyShader(ref drawInfo.wingShader, accessory.wingSlot, 37);

					if (accessory.type == ItemID.FlyingCarpet)
					{
						drawInfo.carpetShader = shaderId;
					}
				}
			}
		}
		private void ApplyAccessoryShaders()
		{
			int shaderId;

			void ApplyShader(ref int output, int slot, int max)
			{
				if (slot > 0 && slot < max)
				{
					output = shaderId;
				}
			}

			for (int x = 0; x < 20; x++)
			{
				int num = x % 10;
				shaderId = ShaderLoader.GetShaderIDNum(player.dye[num]);
				Item accessory = player.armor[x];

				if (player.dye[num] != null && accessory.type != ItemID.None && accessory.stack > 0
					&& (x / 10 >= 1 || !player.hideVisual[num] || accessory.wingSlot > 0 || accessory.type == ItemID.FlyingCarpet))
				{
					ApplyShader(ref player.cHandOn, accessory.handOnSlot, 19);
					ApplyShader(ref player.cHandOff, accessory.handOffSlot, 12);
					ApplyShader(ref player.cBack, accessory.backSlot, 11);
					ApplyShader(ref player.cFront, accessory.frontSlot, 5);
					ApplyShader(ref player.cShoe, accessory.shoeSlot, 18);
					ApplyShader(ref player.cWaist, accessory.waistSlot, 12);
					ApplyShader(ref player.cShield, accessory.shieldSlot, 6);
					ApplyShader(ref player.cNeck, accessory.neckSlot, 9);
					ApplyShader(ref player.cFace, accessory.faceSlot, 9);
					ApplyShader(ref player.cBalloon, accessory.balloonSlot, 16);
					ApplyShader(ref player.cWings, accessory.wingSlot, 37);

					if (accessory.type == ItemID.FlyingCarpet)
					{
						player.cCarpet = shaderId;
					}
				}
			}
		}
		private void ApplyMiscShaders()
		{
			if (player.PetDye() != null)
			{
				player.cPet = ShaderLoader.GetShaderIDNum(player.PetDye());
			}
			if (player.LightDye() != null)
			{
				player.cLight = ShaderLoader.GetShaderIDNum(player.LightDye());
			}
			if (player.MinecartDye() != null)
			{
				player.cMinecart = ShaderLoader.GetShaderIDNum(player.MinecartDye());
			}
			if (player.MountDye() != null)
			{
				player.cMount = ShaderLoader.GetShaderIDNum(player.MountDye());
			}
			if (player.GrappleDye() != null)
			{
				player.cGrapple = ShaderLoader.GetShaderIDNum(player.GrappleDye());
			}
			player.cYorai = player.cPet;
		}

		private static PlayerShaderData data;
		private static int tracker;
		private static void Track() { tracker = Main.playerDrawData.Count; }

		private static List<int> shirts = new List<int>();

		private PlayerLayer trackerLayer = new PlayerLayer("ShaderLib", "Tracker", d => Track());

		private PlayerLayer heldItemLayer = new PlayerLayer("ShaderLib", "HeldItemShader", (PlayerDrawInfo drawInfo) => {
			Player player = drawInfo.drawPlayer;

			//Selects the held mouse item if it's the client-side player AND is holding one; selected inventory item otherwise
			Item heldItem = (Main.myPlayer == player.whoAmI) && Main.mouseItem.active && !Main.mouseItem.IsAir && Main.mouseItem.type != ItemID.None ? Main.mouseItem : player.inventory[player.selectedItem];

			//Don't cause crashes :-)
			if(heldItem == null || !heldItem.active || heldItem.IsAir) return;

			//Don't bother with items that aren't showing (e.g. Arkhalis or... most items, really)
			if(heldItem.noUseGraphic || (heldItem.useStyle == 0 && heldItem.holdStyle == 0)) return;

			int index = Main.playerDrawData.Count - 1;

			//Handling for applying shaders to flame textures. TODO: Figure out why this affects armor shadow effects
			if(heldItem.flame) {
				// Too unstable for now
				index -= 7;
				/*int flameShaderID = 0;
				heldItemFlameHooks.ForEach((hook) => hook(ref flameShaderID, heldItem, player));

				//Apply to all 7 layers of flame textures, because that's how it is for some bizarre reason
				for (int i = 0; i < 1; i++)
				{
					if ((player.itemAnimation > 0 || heldItem.holdStyle > 0) && !player.pulley && flameShaderID > 0)
					{
						DrawData data = Main.playerDrawData[index];
						data.texture = Main.itemFlameTexture[heldItem.type];
						data.shader = flameShaderID;
						Main.playerDrawData[index] = data;
					}

					index -= 1;
				}*/
			}

			//Handling for applying shaders to the held item itself
			int shaderID = 0;
			ShaderLoader.HeldItemShader(out shaderID, heldItem, drawInfo);

			//Only use shader when player is using/holding an item
			if(player.itemAnimation > 0 && !player.pulley) {
				EditData(shaderID, index);
			}
		});

		private PlayerLayer skinLayer = new PlayerLayer("ShaderLib", "BodyLegsShader", (PlayerDrawInfo drawInfo) => {
			data = ShaderLoader.PlayerShader(drawInfo);
			EditData(data.bodySkinShader, Main.playerDrawData.Count - 2);
			EditData(data.legsSkinShader, Main.playerDrawData.Count - 1);
		});

		private PlayerLayer faceLayer = new PlayerLayer("ShaderLib", "FaceShader", (PlayerDrawInfo drawInfo) => {
			EditData(data.faceShader, Main.playerDrawData.Count - 3);
		});

		private PlayerLayer legsLayer = new PlayerLayer("ShaderLib", "LegsShader", (PlayerDrawInfo drawInfo) => {
			Player player = drawInfo.drawPlayer;

			if(player.legs <= 0 && !player.wearsRobe) {
				EditData(data.shoesShader, Main.playerDrawData.Count - 1);
				EditData(data.pantsShader, Main.playerDrawData.Count - 2);
			}
		});

		private PlayerLayer bodyLayer = new PlayerLayer("ShaderLib", "ShirtShader", (PlayerDrawInfo drawInfo) => {
			Player player = drawInfo.drawPlayer;

			if(player.body <= 0)
				for(int i = tracker; i < Main.playerDrawData.Count - 1; i++)
					shirts.Add(i);

			if(player.body <= 0 || drawInfo.drawHands)
				EditData(data.bodySkinShader, Main.playerDrawData.Count - 1);
		});

		private PlayerLayer armsLayer = new PlayerLayer("ShaderLib", "ArmsShader", (PlayerDrawInfo drawInfo) => {
			Player player = drawInfo.drawPlayer;

			if(player.body <= 0) {
				for(int i = tracker + 1; i < Main.playerDrawData.Count; i++)
					shirts.Add(i);

				VariantHandler.ApplyVariant(player.skinVariant, data.shirtShader, data.underShirtShader, shirts);
			}

			if(player.body <= 0 || drawInfo.drawArms)
				EditData(data.bodySkinShader, tracker);

			if(player.body > 0 && drawInfo.drawHands)
				EditData(data.bodySkinShader, tracker + 1);
		});

		private PlayerLayer hairLayer = new PlayerLayer("ShaderLib", "HairShader", (PlayerDrawInfo drawInfo) => {
			Player player = drawInfo.drawPlayer;

			if(player.head <= 0 || drawInfo.drawHair || drawInfo.drawAltHair) {
				int idx = drawInfo.drawAltHair ? 2 : 1;
				EditData(data.hairShader, Main.playerDrawData.Count - idx);
			}
		});

		private PlayerLayer weaponOut = new PlayerLayer("ShaderLib", "WeaponOutSupport", (PlayerDrawInfo drawInfo) => {
			Player player = drawInfo.drawPlayer;

			Item heldItem = player.inventory[player.selectedItem];

			if (heldItem == null || heldItem.IsAir || heldItem.holdStyle != 0) return;

			int shaderID = 0;
			ShaderLoader.HeldItemShader(out shaderID, heldItem, drawInfo);
			for (int i = tracker; i < Main.playerDrawData.Count; i++)
				EditData(shaderID, i);
		});

		/*private static int x = 25;
		private static int y = 25;
		private static int count = 0;

		private static void DebugStuff(string name)
		{
			x += 55;
			Utils.DrawBorderString(Main.spriteBatch, name, new Vector2(x, y - 25), Color.White);
			for (; count < Main.playerDrawData.Count; count++)
			{
				var data = Main.playerDrawData[count];
				Main.spriteBatch.Draw(data.texture, new Rectangle(x, y, data.sourceRect.Value.Width, data.sourceRect.Value.Height), data.sourceRect, Color.White);
				y += data.sourceRect.Value.Height;
			}
		}

		public PlayerLayer debugLayer(string name) => new PlayerLayer("ShaderLib", name, drawInfo =>
		{
			DebugStuff(name);
		});*/

		public override void ModifyDrawLayers(List<PlayerLayer> layers) {
			shirts.Clear();

			if(!Main.gameMenu) {
				layers.Insert(layers.IndexOf(PlayerLayer.HeldItem) + 1, heldItemLayer);
			}

			layers.Insert(layers.IndexOf(PlayerLayer.Skin) + 1, skinLayer);
			layers.Insert(layers.IndexOf(PlayerLayer.Face) + 1, faceLayer);
			layers.Insert(layers.IndexOf(PlayerLayer.Legs) + 1, legsLayer);
			layers.Insert(layers.IndexOf(PlayerLayer.Body), trackerLayer);
			layers.Insert(layers.IndexOf(PlayerLayer.Body) + 1, bodyLayer);
			layers.Insert(layers.IndexOf(PlayerLayer.Arms), trackerLayer);
			layers.Insert(layers.IndexOf(PlayerLayer.Arms) + 1, armsLayer);
			layers.Insert(layers.IndexOf(PlayerLayer.Head) + 1, hairLayer);

			// WeaponOut support
			if (ModLoader.GetMod("WeaponOutLite") != null || ModLoader.GetMod("WeaponOut") != null)
			{
				int idx_item = layers.FindIndex((layer) => { return layer.mod == "WeaponOut" && layer.Name == "HeldItem"; });
				int idx_back = layers.FindIndex((layer) => { return layer.mod == "WeaponOut" && layer.Name == "HairBack"; });
				if (idx_item > -1)
				{
					layers.Insert(idx_item, trackerLayer);
					layers.Insert(idx_item + 2, weaponOut);
				}
				if (idx_back > -1)
				{
					layers.Insert(idx_back, trackerLayer);
					layers.Insert(idx_back + 2, weaponOut);
				}
			}
			#region debug
			/*
			x = 25;
			y = 25;
			count = 0;

			layers.Insert(layers.IndexOf(PlayerLayer.Body) + 1, debugLayer("body"));
			layers.Insert(layers.IndexOf(PlayerLayer.Face) + 1, debugLayer("face"));
			layers.Insert(layers.IndexOf(PlayerLayer.Skin) + 1, debugLayer("skin"));
			layers.Insert(layers.IndexOf(PlayerLayer.Arms) + 1, debugLayer("arms"));
			layers.Insert(layers.IndexOf(PlayerLayer.BackAcc) + 1, debugLayer("back"));
			layers.Insert(layers.IndexOf(PlayerLayer.BalloonAcc) + 1, debugLayer("balloon"));
			layers.Insert(layers.IndexOf(PlayerLayer.FaceAcc) + 1, debugLayer("face"));
			layers.Insert(layers.IndexOf(PlayerLayer.FrontAcc) + 1, debugLayer("front"));
			layers.Insert(layers.IndexOf(PlayerLayer.Hair) + 1, debugLayer("hair"));
			layers.Insert(layers.IndexOf(PlayerLayer.HairBack) + 1, debugLayer("hairBack"));
			layers.Insert(layers.IndexOf(PlayerLayer.HandOffAcc) + 1, debugLayer("handOff"));
			layers.Insert(layers.IndexOf(PlayerLayer.HandOnAcc) + 1, debugLayer("handOn"));
			layers.Insert(layers.IndexOf(PlayerLayer.Head) + 1, debugLayer("head"));
			layers.Insert(layers.IndexOf(PlayerLayer.HeldItem) + 1, debugLayer("item"));
			layers.Insert(layers.IndexOf(PlayerLayer.HeldProjBack) + 1, debugLayer("projBack"));
			layers.Insert(layers.IndexOf(PlayerLayer.HeldProjFront) + 1, debugLayer("projFront"));
			layers.Insert(layers.IndexOf(PlayerLayer.Legs) + 1, debugLayer("legs"));
			layers.Insert(layers.IndexOf(PlayerLayer.MiscEffectsBack) + 1, debugLayer("miscBack"));
			layers.Insert(layers.IndexOf(PlayerLayer.MiscEffectsFront) + 1, debugLayer("miscFront"));
			layers.Insert(layers.IndexOf(PlayerLayer.MountBack) + 1, debugLayer("mBack"));
			layers.Insert(layers.IndexOf(PlayerLayer.MountFront) + 1, debugLayer("mFront"));
			layers.Insert(layers.IndexOf(PlayerLayer.NeckAcc) + 1, debugLayer("neck"));
			layers.Insert(layers.IndexOf(PlayerLayer.ShieldAcc) + 1, debugLayer("shield"));
			layers.Insert(layers.IndexOf(PlayerLayer.ShoeAcc) + 1, debugLayer("shoe"));
			layers.Insert(layers.IndexOf(PlayerLayer.SolarShield) + 1, debugLayer("solarShield"));
			layers.Insert(layers.IndexOf(PlayerLayer.WaistAcc) + 1, debugLayer("waist"));
			layers.Insert(layers.IndexOf(PlayerLayer.Wings) + 1, debugLayer("wings"));*/
			#endregion
		}

		public override void ModifyDrawHeadLayers(List<PlayerHeadLayer> layers)
		{
			// TODO: Implement shaders on the map head textures
		}

		internal static void EditData(int id, int idx) {
			if(id > 0) {
				var data = Main.playerDrawData[idx];
				data.shader = id;
				Main.playerDrawData[idx] = data;
			}
		}
	}
}