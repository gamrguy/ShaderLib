using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ShaderLib
{
	/// <summary>
	/// Represents a unique identifier for a shader.
	/// Use these for netcode.
	/// 
	/// A client-side ShaderID will have ID set to the corresponding shader ID.
	/// A server-side ShaderID will have ID set to 0.
	/// Vanilla-style shaders will always have an ID set.
	/// </summary>
	public class ShaderID : IEquatable<ShaderID>, IEquatable<int>
	{
		public string ModName { get; internal set; }
		public string ShaderName { get; internal set; }
		public int ID { get; internal set; }

		public ShaderID(int shaderID) {
			ModName = "Terraria";
			ShaderName = "";
			ID = shaderID;
		}

		public ShaderID(string modName, string shaderName) {
			ModName = modName;
			ShaderName = shaderName;
			ShaderLibMod.instance.Logger.Info(ModName + " | " + ShaderName);
			if(Main.netMode == NetmodeID.Server) ID = 0;
			else ID = ShaderLoader.GetModShader(ModName, ShaderName).ShaderID.ID;
		}

		internal ShaderID(string modName, string shaderName, int id) {
			ModName = modName;
			ShaderName = shaderName;
			ID = id;
		}

		/// <summary>
		/// Saves the given ShaderID on the returned TagCompound.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static TagCompound Save(ShaderID obj) {
			var tag = new TagCompound();

			tag.Set("ModName", obj.ModName);

			if(obj.ModName == "Terraria") tag.Set("ID", obj.ID);
			else tag.Set("ShaderName", obj.ShaderName);

			return tag;
		}

		/// <summary>
		/// Returns a ShaderID extracted from the given TagCompound.
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		public static ShaderID Load(TagCompound tag) {
			if(tag.GetString("ModName") == "Terraria") {
				return new ShaderID(tag.GetInt("ID"));
			} else {
				return new ShaderID(tag.GetString("ModName"), tag.GetString("ShaderName"));
			}
		}

		/// <summary>
		/// Writes the given ShaderID.
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="id"></param>
		public static void Write(BinaryWriter writer, ShaderID id) {
			writer.Write(id.ModName);
			if(id.ModName == "Terraria") writer.Write(id.ID);
			else writer.Write(id.ShaderName);
		}

		/// <summary>
		/// Writes the given numerical ID as a vanilla ShaderID.
		/// DO NOT use to transfer data about modded shaders!
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="id"></param>
		public static void Write(BinaryWriter writer, int id) {
			Write(writer, new ShaderID(id));
		}

		/// <summary>
		/// Writes the given mod name and shader name as a ShaderID.
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="modName"></param>
		/// <param name="shaderName"></param>
		public static void Write(BinaryWriter writer, string modName, string shaderName) {
			Write(writer, new ShaderID(modName, shaderName));
		}

		/// <summary>
		/// Reads in and returns a ShaderID.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		public static ShaderID Read(BinaryReader reader) {
			string modName = reader.ReadString();

			if(modName == "Terraria") return new ShaderID(reader.ReadInt32());
			else return new ShaderID(modName, reader.ReadString());
		}

		/// <summary>
		/// Returns whether this ShaderID is equivalent to the given one.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(ShaderID other) {
			if(ModName == other.ModName && ShaderName == other.ShaderName) {
				if(ModName == "Terraria") return ID == other.ID;
				else return true;
			}
			return false;
		}

		/// <summary>
		/// Returns whether this ShaderID is equivalent to the given numerical ID.
		/// ONLY RETURNS TRUE ON VANILLA-STYLE SHADERS.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Equals(int id) {
			return ModName == "Terraria" && ID == id;
		}
	}
}
