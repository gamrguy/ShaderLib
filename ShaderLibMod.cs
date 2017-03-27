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

		//Dictionary of registered modded shaders - first is shader ID key to ShaderData, second is mod name and shader name to shader ID
		internal Dictionary<int, ModArmorShaderData> modArmorShadersByID = new Dictionary<int, ModArmorShaderData>();
		internal Dictionary<Tuple<string, string>, int> modArmorShadersByMod = new Dictionary<Tuple<string, string>, int>();

		//Dictionary of registered meta shaders by shader ID (it's the only way to uniquely identify them)
		internal Dictionary<int, MetaArmorShaderData> metaShaderRegistry;

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
			ProjectileShader.hooks = new List<Func<int, Projectile, int>>();
			ItemShader.preDrawInv = new List<Func<int, Item, int>>();
			ItemShader.preDrawWorld = new List<Func<int, Item, int>>();
			NPCShader.hooks = new List<Func<int, NPC, int>>();

			instance = this;
		}

		public override void Unload()
		{
			//Reset the shader listings to the vanilla state.
			ShaderReflections.SetShaderList(vanillaShaders);
			ShaderReflections.SetShaderBindings(vanillaBindings);
			ShaderReflections.customShaders = 0;

			instance = null;
		}

		/// <summary>
		/// Gets a ModArmorShaderData object by mod name and shader name.
		/// </summary>
		/// <returns>The ModArmorShaderData object associated with the given mod and shader name, 
		/// or null if one wasn't found.</returns>
		/// <param name="modName">Name of the mod adding this shader.</param>
		/// <param name="shaderName">Name of the shader.</param>
		public ModArmorShaderData GetModShaderByNames(string modName, string shaderName) {
			return GetModShaderByNames(new Tuple<string, string>(modName, shaderName));
		}

		/// <summary>
		/// Gets a ModArmorShaderData object by mod name and shader name.
		/// </summary>
		/// <returns>The ModArmorShaderData object associated with the given mod and shader name, 
		/// or null if one wasn't found.</returns>
		/// <param name="data">A Tuple object in the format modName, shaderName.</param>
		public ModArmorShaderData GetModShaderByNames(Tuple<string, string> data) {
			if(modArmorShadersByMod.ContainsKey(data)){
				return modArmorShadersByID[modArmorShadersByMod[data]];
			}

			return null;
		}

		/// <summary>
		/// Registers the given armor shader with the given mod name and shader name.
		/// </summary>
		/// <returns>The shader ID of the registered shader.</returns>
		/// <param name="modName">Mod name.</param>
		/// <param name="shaderName">Shader name.</param>
		/// <param name="data">ModArmorShaderData object.</param>
		/// <param name="itemID">Item ID to associate with this shader (use for dyes).</param>
		public int RegisterArmorShader(string modName, string shaderName, ModArmorShaderData data, int itemID = -1) {
			int shaderID;
			if(itemID > 0) {
				ShaderReflections.BindArmorShaderWithID(itemID, data);
				shaderID = GameShaders.Armor.GetShaderIdFromItemId(itemID);
			}
			else {
				shaderID = GameShaders.Armor.GetShaderIdFromItemId(ShaderReflections.BindArmorShaderNoID(data));
			}

			modArmorShadersByMod.Add(new Tuple<string, string>(modName, shaderName), shaderID);
			modArmorShadersByID.Add(shaderID, data);

			return shaderID;
		}

		/// <summary>
		/// Registers the meta armor shader.
		/// The shader won't be registered if a duplicate already exists, returning the duplicate's ID.
		/// </summary>
		/// <returns>The meta armor shader's given shader ID, or the ID of the duplicate if one was found.</returns>
		/// <param name="data">MetaArmorShaderData object.</param>
		public int RegisterMetaArmorShader(MetaArmorShaderData data) {
			foreach(var pair in metaShaderRegistry) {
				if(pair.Value == data) return pair.Key;
			}

			int shaderID = GameShaders.Armor.GetShaderIdFromItemId(ShaderReflections.BindArmorShaderNoID(data));
			metaShaderRegistry.Add(shaderID, data);
			return shaderID;
		}
	}
}