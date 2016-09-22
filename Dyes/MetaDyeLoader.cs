using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

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
				//ErrorLogger.Log("Wrote item: " + pair.Key.Item1 + "/" + pair.Key.Item2); 
			}
			//ErrorLogger.Log("Finished saving");
		}

		public override void LoadCustomData(Item item, BinaryReader reader)
		{
			MetaDyeInfo info = item.GetModInfo<MetaDyeInfo>(mod);
			byte num = reader.ReadByte();
			//ErrorLogger.Log("Beginning load...");
			info.components.Clear();

			for(int i = 0; i < num; i++) {
				MetaDyeInfo.DyeEffects effect = (MetaDyeInfo.DyeEffects)reader.ReadByte();
				string modName = reader.ReadString();
				if(modName == "Terraria") {
					info.AddManualComponent(new Tuple<string, string>(modName, reader.ReadInt32().ToString()), effect);
				} else {
					info.AddManualComponent(new Tuple<string, string>(modName, reader.ReadString()), effect);
				}
				//ErrorLogger.Log("Loaded item from mod: " + modName);
			}

			int test = FindPreexistingShader(info);
			if(test == -1) {
				info.fakeItemID = ShaderReflections.BindArmorShaderNoID<ModArmorShaderData>(info.GetDataFromComponents());
				//ErrorLogger.Log("No preexisting shader found, creating: " + info.fakeItemID);
				return;
			} else {
				info.fakeItemID = test;
			}

			ErrorLogger.Log("Error! Shader not set!");
		}

		public static int FindPreexistingShader(MetaDyeInfo info){
			System.Predicate<ArmorShaderData> cond = new System.Predicate<ArmorShaderData>(delegate(ArmorShaderData obj) {
				if(obj as MetaArmorShaderData == null) return false;
				return info.GetDataFromComponents().Equals(obj);
			});

			int test = ShaderReflections.GetShaderList().FindIndex(cond);
			foreach(KeyValuePair<int, int> pair in ShaderReflections.GetShaderBindings()) {
				if(pair.Value == test+1) {
					//ErrorLogger.Log("Found preexisting shader: " + info.fakeItemID);
					return pair.Key;
				}
			}

			return -1;
		}
	}
}

