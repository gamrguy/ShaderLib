using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ShaderLib;
using ShaderLib.Dyes;


namespace ShaderLib.Shaders
{
	/// <summary>
	/// A class used for easy and standardized saving and loading of shaders.
	/// It's easy to use: just give it a shader ID and it'll do the rest.
	/// When loading, the resultant shader ID should refer to the same shader you saved.
	/// </summary>
	public static class ShaderIO
	{
		//This is just old legacy code. It's totally worthless and overcomplicated.
		//In fact, I think I'll just ditch this entirely. With the new NBT system, there's no data limits.
		//If I need to store shader data from a dye, I can just save the whole item.
		/*private enum ShaderTypes
		{
			VANILLA, MODDED, META
		}*/

		/// <summary>
		/// Writes info on an armor shader to be loaded later.
		/// </summary>
		/// <param name="itemID">Bound item ID of the shader.</param>
		/// <param name="writer">The writer from your SaveCustomData etc. hook.</param>
		/*public static void WriteArmorShader(int itemID, BinaryWriter writer){
			ArmorShaderData data = GameShaders.Armor.GetShaderFromItemId(itemID);

			//Is meta shader, write components
			if(data as MetaArmorShaderData != null) {
				var metaData = data as MetaArmorShaderData;
				writer.Write((byte)ShaderTypes.META);
				writer.Write((byte)metaData.components.Count);
				foreach(var pair in metaData.components) {
					writer.Write(pair.Key.Item1);
					writer.Write(pair.Key.Item2);
					writer.Write((byte)pair.Value);
				}

			//Is modded shader, write mod name and item name
			} else if(data as ModArmorShaderData != null || itemID >= Main.maxItemTypes) {
				writer.Write((byte)ShaderTypes.MODDED);
				Item dummy = new Item();
				dummy.SetDefaults(itemID);
				writer.Write(dummy.modItem.mod.Name);
				writer.Write(Main.itemName[itemID]);

			//Is vanilla shader, write item ID
			} else {
				writer.Write((byte)ShaderTypes.VANILLA);
				writer.Write(itemID);
			}
		}

		/// <summary>
		/// Reads written info about an armor shader.
		/// </summary>
		/// <returns>The bound item ID for the written shader.</returns>
		/// <param name="reader">The reader from your LoadCustomData etc. hook.</param>
		public static int ReadArmorShader(BinaryReader reader){
			byte x = reader.ReadByte();

			switch(x) {
			//Is meta shader, read components and create new meta shader if not already existing
			case (byte)ShaderTypes.META:
				byte c = reader.ReadByte();
				var components = new Dictionary<Tuple<string, string>, MetaDyeInfo.DyeEffects>();
				for(int i = 0; i < c; i++) {
					components.Add(new Tuple<string, string>(reader.ReadString(), reader.ReadString()), (MetaDyeInfo.DyeEffects)reader.ReadByte());
				}
				var result = MetaDyeInfo.GetDataFromComponents(components);
				int preExist = MetaDyeLoader.FindPreexistingShader(components);
				if(preExist != -1) {
					return preExist;
				} else {
					return ShaderReflections.BindArmorShaderNoID<ModArmorShaderData>(result);
				}

			//Is modded shader, read mod name and item name, grab type
			case (byte)ShaderTypes.MODDED:
				string modName = reader.ReadString();
				string itemName = reader.ReadString();
				return ModLoader.GetMod(modName)?.ItemType(itemName) ?? 0;
			
			//Is vanilla shader, read and return item type
			case (byte)ShaderTypes.VANILLA:
				return reader.ReadInt32();

			//In case something "WHOOPS" happens
			default:
				ErrorLogger.Log("Error! Corrupted shader data!");
				return -1;
			}
		}*/

		//New, non-legacy code for easy shader saving and loading
		/// <summary>
		/// Extracts a saved shader from a given TagCompound, returning its shader ID.
		/// </summary>
		/// <returns>The extracted shader ID.</returns>
		/// <param name="compound">TagCompound to extract the shader from.</param>
		public static int LoadShader(TagCompound compound) {
			var savedShader = compound.GetCompound("SavedShader");
			string shaderType = savedShader.GetString("Type");

			//Load vanilla shader; easily identified by unchanging shader ID
			if(shaderType == "Vanilla") {
				return savedShader.GetInt("ShaderID");
			}

			//Load modded shader, uniquely identified by mod name and shader name
			else if(shaderType == "Modded") {
				string modName = savedShader.GetString("ModName");
				string shaderName = savedShader.GetString("ShaderName");

				return ShaderLibMod.instance.modArmorShadersByMod[new Tuple<string, string>(modName, shaderName)];
			}

			//Load meta shader, a complicated mashup of its own values and other shader's functions
			else if(shaderType == "Meta") {
				
				//Get the mod name and shader name of all the possible functions/objects (unused functions are "")
				var updatePrimary   = new Tuple<string, string>(savedShader.GetString("UpdatePrimaryModName"),   savedShader.GetString("UpdatePrimaryShaderName"));
				var updateSecondary = new Tuple<string, string>(savedShader.GetString("UpdateSecondaryModName"), savedShader.GetString("UpdateSecondaryShaderName"));
				var preApply        = new Tuple<string, string>(savedShader.GetString("PreApplyModName"),        savedShader.GetString("PreApplyShaderName"));
				var postApply       = new Tuple<string, string>(savedShader.GetString("PostApplyModName"),       savedShader.GetString("PostApplyShaderName"));
				var secondaryShader = new Tuple<string, string>(savedShader.GetString("SecondaryShaderModName"), savedShader.GetString("SecondaryShaderShaderName"));
				var image           = new Tuple<string, string>(savedShader.GetString("ImageModName"),           savedShader.GetString("ImageShaderName"));

				//Grab the primary and secondary colors of the shader
				Color primaryColor = intArrToColor(savedShader.GetIntArray("PrimaryColor"));
				Color secondaryColor = intArrToColor(savedShader.GetIntArray("SecondaryColor"));
				float saturation = savedShader.GetFloat("Saturation");
				string passName = savedShader.GetString("PassName");

				var meta = new MetaArmorShaderData(ShaderLibMod.shaderRef, passName);
				meta.primary             = primaryColor;
				meta.secondary           = secondaryColor;
				meta.saturation          = saturation;
				meta.MetaUpdatePrimary   = updatePrimary;
				meta.MetaUpdateSecondary = updateSecondary;
				meta.MetaPreApply        = preApply;
				meta.MetaPostApply       = postApply;
				meta.MetaUpdateSecondary = secondaryShader;
				meta.NoiseImage          = image;
				meta.SwapProgram(passName);

				foreach(var pair in ShaderLibMod.instance.metaShaderRegistry) {
					if(meta == pair.Value) return pair.Key;
				}

				return GameShaders.Armor.GetShaderIdFromItemId(ShaderReflections.BindArmorShaderNoID<MetaArmorShaderData>(meta));
			}

			//If no valid shader found (corrupted data or just missing)
			return 0;
		}

		/// <summary>
		/// Saves the given shader to a returned TagCompound.
		/// </summary>
		/// <returns>A TagCompound containing information on the shader to be loaded later.</returns>
		/// <param name="shaderID">Shader ID of the shader to be saved.</param>
		public static TagCompound SaveShader(int shaderID) {
			TagCompound compound = new TagCompound();

			//Save vanilla ID if in vanilla shaders
			if(shaderID <= ShaderLibMod.maxVanillaID) {
				compound.Set("Type", "Vanilla");
				compound.Set("ShaderID", shaderID);
			}

			//Save mod name and shader name if modded shader
			else if(ShaderLibMod.instance.modArmorShadersByID.ContainsKey(shaderID)) {
				compound.Set("Type", "Modded");

				var tuple = new Tuple<string, string>("", "");
				foreach(var pair in ShaderLibMod.instance.modArmorShadersByMod) if(pair.Value == shaderID) tuple = pair.Key;
				compound.Set("ModName", tuple.Item1);
				compound.Set("ShaderName", tuple.Item2);
			}

			//Save FRIGGING EVERYTHING if it's a meta shader
			else if(ShaderLibMod.instance.metaShaderRegistry.ContainsKey(shaderID)) {
				compound.Set("Type", "Meta");

				var meta = ShaderLibMod.instance.metaShaderRegistry[shaderID];
				compound.Set("UpdatePrimaryModName",      meta.MetaUpdatePrimary.Item1);
				compound.Set("UpdatePrimaryShaderName",   meta.MetaUpdatePrimary.Item2);
				compound.Set("UpdateSecondaryModName",    meta.MetaUpdateSecondary.Item1);
				compound.Set("UpdateSecondaryShaderName", meta.MetaUpdatePrimary.Item2);
				compound.Set("PreApplyModName",           meta.MetaPreApply.Item1);
				compound.Set("PreApplyShaderName",        meta.MetaPreApply.Item2);
				compound.Set("PostApplyModName",          meta.MetaPostApply.Item1);
				compound.Set("PostApplyShaderName",       meta.MetaPostApply.Item2);
				compound.Set("SecondaryShaderModName",    meta.MetaSecondaryShader.Item1);
				compound.Set("SecondaryShaderShaderName", meta.MetaUpdatePrimary.Item2);
				compound.Set("ImageModName",              meta.NoiseImage.Item1);
				compound.Set("ImageShaderName",           meta.NoiseImage.Item2);

				compound.Set("PrimaryColor",   colorToIntArr(meta.primary));
				compound.Set("SecondaryColor", colorToIntArr(meta.secondary));
				compound.Set("Saturation",     meta.saturation);
				compound.Set("PassName",       meta.GetPassName());
			}

			//If nothing was saved (perhaps an invalid shader ID) then nothing is saved and nothing is loaded (you get 0)
			return compound;
		}


		//Little helper method
		private static Color intArrToColor(int[] nums) {
			return new Color(nums[0], nums[1], nums[2]);
		}

		//Another little helper method
		private static int[] colorToIntArr(Color color) {
			return new int[] { color.R, color.G, color.B };
		}
	}
}