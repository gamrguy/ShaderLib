using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ShaderLib
{
	public abstract class GlobalShader
	{
		public Mod Mod { get; internal set; }
		public virtual string Name => GetType().Name;
		
		public virtual bool Autoload() => true;
		
		public virtual int ItemInventoryShader(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) => 0;
		public virtual int ItemWorldShader(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) => 0;
		public virtual int ProjectileShader(Projectile projectile, SpriteBatch spriteBatch, Color lightColor) => 0;
		public virtual int NPCShader(NPC npc, SpriteBatch spriteBatch, Color drawColor) => 0;
		public virtual void PlayerShader(ref PlayerShaderData data, PlayerDrawInfo drawInfo) { }
		public virtual void HeldItemShader(ref int shaderID, Item item, PlayerDrawInfo drawInfo) { }
	}
}
