using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using ShaderLib.Shaders;

namespace ShaderLib.Dyes
{
	public class MetaDyeLoader : GlobalItem
	{
		public override bool NeedsCustomSaving(Item item)
		{
			if((mod as ShaderLibMod).unLinkedItems.Contains(item.type)) return true;
			return false;
		}

		public override void SaveCustomData(Item item, BinaryWriter writer)
		{
			MetaDyeInfo info = item.GetModInfo<MetaDyeInfo>(mod);
			writer.Write((byte)info.components.Count);
			foreach(KeyValuePair<Tuple<string, string>, MetaDyeInfo.DyeEffects> pair in info.components){
				writer.Write((byte)pair.Value);
				writer.Write(pair.Key.Item1);
				if(pair.Key.Item1 == "Terraria") {
					writer.Write(int.Parse(pair.Key.Item2));
				} else {
					writer.Write(pair.Key.Item2);
				}
			}
		}

		public override void LoadCustomData(Item item, BinaryReader reader)
		{
			MetaDyeInfo info = item.GetModInfo<MetaDyeInfo>(mod);
			byte num = reader.ReadByte();
			info.components.Clear();

			for(int i = 0; i < num; i++) {
				MetaDyeInfo.DyeEffects effect = (MetaDyeInfo.DyeEffects)reader.ReadByte();
				string modName = reader.ReadString();
				if(modName == "Terraria") {
					info.AddManualComponent(new Tuple<string, string>(modName, reader.ReadInt32().ToString()), effect);
				} else {
					info.AddManualComponent(new Tuple<string, string>(modName, reader.ReadString()), effect);
				}
			}

			int test = FindPreexistingShader(info.components);
			if(test == -1) {
				info.fakeItemID = ShaderReflections.BindArmorShaderNoID<ModArmorShaderData>(MetaDyeInfo.GetDataFromComponents(info.components));
				ErrorLogger.Log("Added meta shader at " + info.fakeItemID);
				return;
			} else {
				info.fakeItemID = test;
				ErrorLogger.Log("Found preexisting shader at " + info.fakeItemID);
				return;
			}

			//ErrorLogger.Log("Error! Shader not set!");
		}

		/// <summary>
		/// Returns the bound fake item ID of a meta shader if it already exists. -1 otherwise.
		/// </summary>
		/// <returns>Bound fake item ID of a preexisting meta shader.</returns>
		/// <param name="components">Components of the meta shader.</param>
		public static int FindPreexistingShader(Dictionary<Tuple<string, string>, MetaDyeInfo.DyeEffects> components){
			System.Predicate<ArmorShaderData> cond = new System.Predicate<ArmorShaderData>(delegate(ArmorShaderData obj) {
				if(obj as MetaArmorShaderData == null) return false;
				return MetaDyeInfo.GetDataFromComponents(components).Equals(obj);
			});

			int test = ShaderReflections.GetShaderList().FindIndex(cond);
			foreach(KeyValuePair<int, int> pair in ShaderReflections.GetShaderBindings()) {
				if(pair.Value == test+1) {
					return pair.Key;
				}
			}

			return -1;
		}
	}
}

