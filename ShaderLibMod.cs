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

		public static Ref<Effect> shaderRef = new Ref<Effect>(Main.pixelShader);

		public ShaderLibMod()
		{
			Properties = new ModProperties() {
				Autoload = true,
			};
		}

		public override void Load()
		{
			/*foreach(var pass in Main.pixelShader.CurrentTechnique.Passes) {
				ErrorLogger.Log(pass.Name);
			}*/

			//Save the state of the vanilla shader listings.
			if(Main.netMode != 2) {
				vanillaShaders = new List<ArmorShaderData>(ShaderReflections.GetShaderList());
				vanillaBindings = new Dictionary<int, int>(ShaderReflections.GetShaderBindings());
			}
			ProjectileShader.hooks = new List<Func<int, Projectile, int>>();
			ItemShader.preDrawInv = new List<Func<int, Item, int>>();
			ItemShader.preDrawWorld = new List<Func<int, Item, int>>();
			NPCShader.hooks = new List<Func<int, NPC, int>>();

			unLinkedItems.Add(ItemType("TestMetaDye"));

			if(Main.netMode != 2) {
				ShaderReflections.BindArmorShaderWithID<TestRainbowShader>(ItemType("TestRainbowDye"), new TestRainbowShader(shaderRef, "ArmorColored"));
				ShaderReflections.BindArmorShaderWithID<TestRainbowShader>(ItemType("TestPlaidDye"), new TestRainbowShader(shaderRef, "ArmorVortex"));
			}
		}

		public override void Unload()
		{
			if(Main.netMode != 2) {
				//Reset the shader listings to the vanilla state.
				ShaderReflections.SetShaderList(vanillaShaders);
				ShaderReflections.SetShaderBindings(vanillaBindings);
				ShaderReflections.customShaders = 0;
			}
		}
	}
}