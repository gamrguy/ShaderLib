using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ID;

namespace ShaderLib
{
	public class BaseHooks : GlobalShader
	{
		// Pet/grapple dye fix
		public override int ProjectileShader(Projectile projectile, SpriteBatch spriteBatch, Color lightColor) {
			if(Main.player[projectile.owner] == null) return 0;

			int shaderID = 0;
			bool hook, pet, lightPet;
			hook = Main.projHook[projectile.type];
			pet = Main.projPet[projectile.type] && !projectile.minion && projectile.damage == 0 && !ProjectileID.Sets.LightPet[projectile.type];
			pet = pet || (projectile.type == ProjectileID.StardustGuardian);
			lightPet = !projectile.minion && projectile.damage == 0 && ProjectileID.Sets.LightPet[projectile.type];

			if(hook || pet || lightPet) {
				Player owner = Main.player[projectile.owner];

				//Return the proper shader; if it's 0, return the current shader ID (do nothing)
				if(hook) shaderID = owner.cGrapple != 0 ? owner.cGrapple : shaderID;
				else if(pet) shaderID = owner.cPet != 0 ? owner.cPet : shaderID;
				else if(lightPet) shaderID = owner.cLight != 0 ? owner.cLight : shaderID;
			}

			return shaderID;
		}

		// Enable armor shaders on familiar wig
		public override void PlayerShader(ref PlayerShaderData data, PlayerDrawInfo drawInfo) {
			Player player = drawInfo.drawPlayer;
			if(player.head == 0 && player.dye[0] != null) {
				Item dye = drawInfo.drawPlayer.dye[0];
				if(dye.modItem != null && dye.modItem as IDye != null) data.hairShader = (dye.modItem as IDye).DyeID.ID;
				else data.hairShader = GameShaders.Armor.GetShaderIdFromItemId(drawInfo.drawPlayer.dye[0].type);
			}
		}
	}
}