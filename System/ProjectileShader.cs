using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ShaderLib.System
{
	public class ProjectileShader : GlobalProjectile
	{
		public override bool PreDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
		{
			ShaderLoader.ProjectileShader(projectile, spriteBatch, lightColor);
			return true;
		}
		
		//Resets the SpriteBatch after drawing projectile, to prepare for next projectile
		public override void PostDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
			=> spriteBatch.Restart(Main.GameViewMatrix.TransformationMatrix);
	}
}

