using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShaderLib.System
{
	/// <summary>
	/// Holds some utility stuff
	/// </summary>
	public static class SpriteBatchUtils
	{
		public static void Restart(this SpriteBatch sb, Matrix scaleMatrix, bool forShader = true) {
			Rectangle scissor = sb.GraphicsDevice.ScissorRectangle;
			
			sb.End();
			sb.GraphicsDevice.ScissorRectangle = scissor;
			sb.Begin(
				forShader ? SpriteSortMode.Immediate : SpriteSortMode.Deferred, 
				BlendState.AlphaBlend, 
				SamplerState.LinearClamp, 
				sb.GraphicsDevice.DepthStencilState,
				sb.GraphicsDevice.RasterizerState, 
				null,
				scaleMatrix
			);
		}
	}
}