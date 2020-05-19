using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace ShaderLib.System
{
	/// <summary>
	/// Holds some utility stuff
	/// </summary>
	public static class SpriteBatchUtils
	{
		public static void Restart(this SpriteBatch sb, Matrix scaleMatrix, bool forShader = true, bool worldDraw = true)
		{
			Rectangle scissor = sb.GraphicsDevice.ScissorRectangle;

			sb.End();
			sb.GraphicsDevice.ScissorRectangle = scissor;
			sb.Begin(
				forShader ? SpriteSortMode.Immediate : SpriteSortMode.Deferred,
				BlendState.AlphaBlend,
				worldDraw ? SamplerState.PointClamp : SamplerState.LinearClamp,
				sb.GraphicsDevice.DepthStencilState,
				sb.GraphicsDevice.RasterizerState,
				null,
				scaleMatrix
			);
		}
	}

	public static class PlayerExtensions
	{
		public static Item HeadDye(this Player player) => player.dye[0];
		public static Item BodyDye(this Player player) => player.dye[1];
		public static Item LegsDye(this Player player) => player.dye[2];

		public static Item PetDye(this Player player) => player.miscDyes[0];
		public static Item LightDye(this Player player) => player.miscDyes[1];
		public static Item MinecartDye(this Player player) => player.miscDyes[2];
		public static Item MountDye(this Player player) => player.miscDyes[3];
		public static Item GrappleDye(this Player player) => player.miscDyes[4];
	}
}