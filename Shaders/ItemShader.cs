using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace ShaderLib.Shaders
{
	public class ItemShader : GlobalItem
	{
		public static List<Func<int, Item, int>> preDrawInv;
		public static List<Func<int, Item, int>> preDrawWorld;

		public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			int shaderID = 0;

			foreach(var hook in preDrawInv) {
				shaderID = hook(shaderID, item);
			}

			//BlendState test = BlendState.Additive;
			//test.ColorBlendFunction = BlendFunction.Subtract;

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationZ(0f) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0f)));

			DrawData data = new DrawData();
			data.origin = origin;
			data.position = position - Main.screenPosition;
			data.scale = new Vector2(scale, scale);
			data.sourceRect = frame;
			data.texture = Main.itemTexture[item.type];
			GameShaders.Armor.ApplySecondary(shaderID, Main.player[item.owner], data);

			return true;
		}

		public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationZ(0f) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0f)));
		}

		public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			//CustomizerItemInfo info = item.GetModInfo<CustomizerItemInfo>(mod);

			int shaderID = 0;

			foreach(var hook in preDrawWorld) {
				shaderID = hook(shaderID, item);
			}

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationZ(0f) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0f)));

			DrawData data = new DrawData();
			data.origin = item.Center;
			data.position = item.position - Main.screenPosition;
			data.scale = new Vector2(scale, scale);
			//data.sourceRect = item.;
			data.texture = Main.itemTexture[item.type];
			data.rotation = rotation;
			GameShaders.Armor.ApplySecondary(shaderID, Main.player[item.owner], data);

			return true;
		}

		public override void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationZ(0f) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0f)));
		}
	}
}

