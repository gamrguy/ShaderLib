using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace ShaderLib.Shaders
{
	public class NPCShader : GlobalNPC
	{
		public static List<Func<int, NPC, int>> hooks;
		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			int shaderID = 0;

			foreach(var hook in hooks) {
				shaderID = hook(shaderID, npc);
			}

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationZ(0f) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0f)));

			DrawData data = new DrawData();
			data.origin = npc.Center;
			data.position = npc.position - Main.screenPosition;
			data.scale = new Vector2(npc.scale, npc.scale);
			data.texture = Main.npcTexture[npc.type];
			data.sourceRect = npc.frame;//data.texture.Frame(1, Main.npcFrameCount[npc.type], 0, npc.frame);
			GameShaders.Armor.ApplySecondary(shaderID, npc, data);

			return true;
		}

		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationZ(0f) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0f)));
		}
	}
}

