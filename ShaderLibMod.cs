using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using ShaderLib.System;

namespace ShaderLib
{
	public class ShaderLibMod : Mod
	{
		public static ShaderLibMod instance;

		//These store the vanilla shader lists, so that they can be reset when the mod is unloaded.
		private List<ArmorShaderData> vanillaShaders;
		private Dictionary<int, int> vanillaBindings;

		/// <summary>
		/// The highest shader ID in the vanilla game.
		/// </summary>
		public const short maxVanillaID = 116;

		//Provides easy access to this object now needed instead of pixelShader directly.
		public static Ref<Effect> shaderRef = new Ref<Effect>(Main.pixelShader);

		public ShaderLibMod() {
			Properties = new ModProperties() {
				Autoload = true,
			};
		}

		public override void Load() {
			Logger.InfoFormat("{0}: ", Name);

			//Save the state of the vanilla shader listings.
			vanillaShaders = new List<ArmorShaderData>(ShaderReflections.GetShaderList());
			vanillaBindings = new Dictionary<int, int>(ShaderReflections.GetShaderBindings());
			
			ShaderLoader.RegisterMod(this);

			instance = this;
		}

		public override void PostSetupContent() {
			ShaderLoader.SetupContent();
		}

		public override void Unload() {
			//Reset the shader listings to the vanilla state.
			ShaderReflections.SetShaderList(vanillaShaders);
			ShaderReflections.SetShaderBindings(vanillaBindings);
			ShaderReflections.SetShaderCount(vanillaShaders.Count);

			instance = null;
			shaderRef = null;
			VariantHandler.variants = null;
			ShaderLoader.Unload();
		}
	}
}