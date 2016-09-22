using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ID;
using ShaderLib.Shaders;

namespace ShaderLib
{
	public class ShaderLibMod : Mod
	{
		/// <summary>
		/// Which items are not directly associated with a shader ID.
		/// </summary>
		public List<int> unLinkedItems = new List<int>();

		//These store the vanilla shader lists, so that they can be reset when the mod is unloaded.
		public List<ArmorShaderData> vanillaShaders;
		public Dictionary<int, int> vanillaBindings;

		/// <summary>
		/// The highest shader ID in the vanilla game.
		/// </summary>
		public const short maxVanillaID = 116;

		public ShaderLibMod()
		{
			Properties = new ModProperties() {
				Autoload = true,
			};
		}

		public override void Load()
		{
			//Save the state of the vanilla shader listings.
			vanillaShaders = new List<ArmorShaderData>(ShaderReflections.GetShaderList());
			vanillaBindings = new Dictionary<int, int>(ShaderReflections.GetShaderBindings());
			ProjectileShader.hooks = new List<Func<int, int>>();

			unLinkedItems.Add(ItemType("TestMetaDye"));

			ShaderReflections.BindArmorShaderWithID<TestRainbowShader>(ItemType("TestRainbowDye"), new TestRainbowShader(Main.pixelShader, "ArmorColored"));
		}

		public override void Unload()
		{
			//Reset the shader listings to the vanilla state.
			ShaderReflections.SetShaderList(vanillaShaders);
			ShaderReflections.SetShaderBindings(vanillaBindings);
			ShaderReflections.customShaders = 0;
		}
	}
}