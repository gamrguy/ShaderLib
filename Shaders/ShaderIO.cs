using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using ShaderLib;
using ShaderLib.Dyes;


namespace ShaderLib.Shaders
{
	public class ShaderIO
	{
		private enum ShaderTypes
		{
			VANILLA, MODDED, META
		}

		/// <summary>
		/// Writes info on an armor shader to be loaded later.
		/// </summary>
		/// <param name="itemID">Bound item ID of the shader.</param>
		/// <param name="writer">The writer from your SaveCustomData etc. hook.</param>
		public static void WriteArmorShader(int itemID, BinaryWriter writer){
			ArmorShaderData data = GameShaders.Armor.GetShaderFromItemId(itemID);

			//Is meta shader, write components
			/*if(data as MetaArmorShaderData != null) {
				var metaData = data as MetaArmorShaderData;
				writer.Write((byte)ShaderTypes.META);
				writer.Write((byte)metaData.components.Count);
				foreach(var pair in metaData.components) {
					writer.Write(pair.Key.Item1);
					writer.Write(pair.Key.Item2);
					writer.Write((byte)pair.Value);
				}

			//Is modded shader, write mod name and item name
			} else*/ if(data as ModArmorShaderData != null || itemID >= Main.maxItemTypes) {
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
			/*case (byte)ShaderTypes.META:
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
			*/
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
		}
	}
}

