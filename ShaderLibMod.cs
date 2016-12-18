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
		/// This is currently unused, as the meta dyes system is unfinished.
		/// ...It'll probably be moved to a different mod entirely.
		/// </summary>
		//public List<int> unLinkedItems = new List<int>();

		//These store the vanilla shader lists, so that they can be reset when the mod is unloaded.
		private List<ArmorShaderData> vanillaShaders;
		private Dictionary<int, int> vanillaBindings;

		/// <summary>
		/// The highest shader ID in the vanilla game.
		/// </summary>
		public const short maxVanillaID = 116;

		//Provides easy access to this object now needed instead of pixelShader directly.
		public static Ref<Effect> shaderRef = new Ref<Effect>(Main.pixelShader);

		public ShaderLibMod()
		{
			Properties = new ModProperties() {
				Autoload = true,
			};
		}

		public override void Load()
		{
			//Save the state of the vanilla shader listings.
			if(Main.netMode != 2) {
				vanillaShaders = new List<ArmorShaderData>(ShaderReflections.GetShaderList());
				vanillaBindings = new Dictionary<int, int>(ShaderReflections.GetShaderBindings());
			}
			ProjectileShader.hooks = new List<Func<int, Projectile, int>>();
			ItemShader.preDrawInv = new List<Func<int, Item, int>>();
			ItemShader.preDrawWorld = new List<Func<int, Item, int>>();
			NPCShader.hooks = new List<Func<int, NPC, int>>();
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