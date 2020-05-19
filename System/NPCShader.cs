using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ShaderLib.System
{
	public sealed class NPCShader : GlobalNPC
	{
		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			ShaderLoader.NPCShader(npc, spriteBatch, drawColor);
			return true;
		}

		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
			=> spriteBatch.Restart(Main.GameViewMatrix.TransformationMatrix);
	}
}

