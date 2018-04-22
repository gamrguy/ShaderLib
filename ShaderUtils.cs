using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ShaderLib
{
	/// <summary>
	/// Holds some utility stuff
	/// </summary>
	public static class ShaderUtils
	{
		public static void Restart(this SpriteBatch sb, Matrix scaleMatrix, SpriteSortMode sortMode = SpriteSortMode.Immediate, bool useDefaultSamp = false) {
			Rectangle scissor = sb.GraphicsDevice.ScissorRectangle;
			RasterizerState rast = sb.GraphicsDevice.RasterizerState.CloneRast();
			BlendState blend = sb.GraphicsDevice.BlendState;
			DepthStencilState stencil = sb.GraphicsDevice.DepthStencilState;

			rast.ScissorTestEnable = true;

			sb.End();
			sb.GraphicsDevice.ScissorRectangle = scissor;
			sb.Begin(sortMode, blend, useDefaultSamp ? Main.DefaultSamplerState : SamplerState.AnisotropicClamp, stencil, rast, null, scaleMatrix);
		}

		private static RasterizerState CloneRast(this RasterizerState rast) {
			RasterizerState clone = new RasterizerState();
			clone.CullMode = rast.CullMode;
			clone.DepthBias = rast.DepthBias;
			clone.FillMode = rast.FillMode;
			clone.MultiSampleAntiAlias = rast.MultiSampleAntiAlias;
			clone.Name = rast.Name;
			clone.ScissorTestEnable = rast.ScissorTestEnable;
			clone.SlopeScaleDepthBias = rast.SlopeScaleDepthBias;
			clone.Tag = rast.Tag;
			return clone;
		}
	}
}