using IL.Terraria.Graphics.Shaders;
using IL.Terraria.UI.Chat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
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
			=> spriteBatch.Restart(Main.UIScaleMatrix, false, false);

		public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			ShaderLoader.ItemWorldShader(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
			return true;
		}

		public override void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
			=> spriteBatch.Restart(Main.GameViewMatrix.TransformationMatrix, false);

		/*public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
		{
			Main.spriteBatch.Restart(Main.UIScaleMatrix, worldDraw: false);
			var strSize = Terraria.UI.Chat.ChatManager.GetStringSize(Main.fontMouseText, lines[0].text, Vector2.One);//Main.fontMouseText.MeasureString(lines[0].text);

			DrawData data = new DrawData
			{
				position = Main.MouseWorld + new Vector2(14, 14),
				scale = new Vector2(Main.UIScale, Main.UIScale),
				sourceRect = new Rectangle(0, 0, 128, 128),
				texture = new Texture2D(Main.spriteBatch.GraphicsDevice, Main.basiliskMountTexture.Width, Main.basiliskMountTexture.Height / 4)
			};
			//Terraria.Graphics.Shaders.GameShaders.Armor.ApplySecondary(110, Main.player[item.owner], data);
			Terraria.Graphics.Shaders.GameShaders.Armor.ApplySecondary(ShaderLoader.GetShaderIDNum(item), Main.player[item.owner], data);
			Main.spriteBatch.Draw(Main.basiliskMountTexture, new Vector2(100, 100), Color.White);
			//Terraria.Graphics.Shaders.GameShaders.Armor.ApplySecondary(114, Main.clientPlayer);
			return true;
		}

		public override void PostDrawTooltipLine(Item item, DrawableTooltipLine line)
		{
			if(line.Name.Equals("ItemName")) {
				Main.spriteBatch.Restart(Main.GameViewMatrix.TransformationMatrix, false);
			//Terraria.Graphics.Shaders.GameShaders.Armor.Apply(114, Main.clientPlayer);
				//Terraria.Graphics.Shaders.GameShaders.Armor.ApplySecondary(114, Main.clientPlayer);
			}
			//base.PostDrawTooltipLine(item, line);
		}*/
	}
}

