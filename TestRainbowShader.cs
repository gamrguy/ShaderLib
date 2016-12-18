using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using ShaderLib.Shaders;

namespace ShaderLib
{
	/// <summary>
	/// A test class, designed to cycle through the color spectrum.
	/// </summary>
	public class TestRainbowShader : ModArmorShaderData
	{
		public TestRainbowShader(Ref<Effect> shader, string passName) : base(shader, passName){
			saturation = 1.2f;
			UpdatePrimary = RainbowColor;
		}

		private Color RainbowColor(Entity e, DrawData? drawData){
			return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
		}
	}
}

