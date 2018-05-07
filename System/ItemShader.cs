using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ShaderLib.System
{
    public class ItemShader : GlobalItem
	{
		public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			ShaderLoader.ItemInventoryShader(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
			return true;
		}

		public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.Restart(Main.UIScaleMatrix, false);
        }

		public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			ShaderLoader.ItemWorldShader(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
			return true;
		}

		public override void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.Restart(Main.GameViewMatrix.TransformationMatrix, false);
		}
	}
}

