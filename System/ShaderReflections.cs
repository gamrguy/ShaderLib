using System.Collections.Generic;
using System.Reflection;
using log4net.Repository.Hierarchy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace ShaderLib.System
{
	/// <summary>
	/// A helper class with many functions allowing access to normally private
	/// variables of the ArmorShaderData and ArmorShaderDataSet classes.
	/// If you are a modder looking to implement this library, this class
	/// is most likely not for you. You could break something.
	/// 
	/// Well, except for the BindShader functions. Those are quite handy.
	/// 
	/// Also, it's internal now, so if you want it you have to use reflection. Have fun with that.
	/// </summary>
	internal static class ShaderReflections
	{
		public static int customShaders = 0;

		public static FieldInfo GetFieldInfo(string fieldName){
			var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			return typeof(ArmorShaderData).GetField(fieldName, bindFlags);
		}

		public static FieldInfo GetSetFieldInfo(string fieldName){
			var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			return typeof(ArmorShaderDataSet).GetField(fieldName, bindFlags);
		}

		public static Color GetPrimaryColor(ArmorShaderData data){
			var field = GetFieldInfo("_uColor");
			return new Color((Vector3)field.GetValue(data));
		}

		public static Color GetSecondaryColor(ArmorShaderData data){
			var field = GetFieldInfo("_uSecondaryColor");
			return new Color((Vector3)field.GetValue(data));
		}

		public static string GetPassName(ArmorShaderData data){
			var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			var field = typeof(ShaderData).GetField("_passName", bindFlags);
			return (string)field.GetValue((ShaderData)data);
		}

		public static float GetSaturation(ArmorShaderData data){
			var field = GetFieldInfo("_uSaturation");
			return (float)field.GetValue(data);
		}

		public static float GetOpacity(ArmorShaderData data){
			var field = GetFieldInfo("_uOpacity");
			return (float)field.GetValue(data);
		}

		public static Ref<Texture2D> GetImage(ArmorShaderData data){
			var field = GetFieldInfo("_uImage");
			return (Ref<Texture2D>)field.GetValue(data);
		}

		public static void SetImage(ArmorShaderData data, Ref<Texture2D> image){
			var field = GetFieldInfo("_uImage");
			field.SetValue(data, image);
		}

		public static List<ArmorShaderData> GetShaderList(){
			var field = GetSetFieldInfo("_shaderData");
			return (List<ArmorShaderData>)field.GetValue(GameShaders.Armor);
		}

		public static void SetShaderList(List<ArmorShaderData> list){
			var field = GetSetFieldInfo("_shaderData");
			field.SetValue(GameShaders.Armor, list);
		}

		public static Dictionary<int, int> GetShaderBindings(){
			var field = GetSetFieldInfo("_shaderLookupDictionary");
			return (Dictionary<int, int>)field.GetValue(GameShaders.Armor);
		}

		public static void SetShaderBindings(Dictionary<int, int> dict){
			var field = GetSetFieldInfo("_shaderLookupDictionary");
			field.SetValue(GameShaders.Armor, dict);
		}

		public static void SetShaderCount(int count) {
			var field = GetSetFieldInfo("_shaderDataCount");
			field.SetValue(GameShaders.Armor, count);
		}

		public static void IncrementShaderCount(){
			var field = GetSetFieldInfo("_shaderDataCount");
			field.SetValue(GameShaders.Armor, (int)(field.GetValue(GameShaders.Armor))+1);
		}

		/// <summary>
		/// Uses an item ID to bind a given shader to that item.
		/// This is similar to the vanilla implementation, but
		/// it doesn't care whether the amount of shaders exceeds 255.
		/// </summary>
		/// <param name="itemID">Item ID of the item to bind to</param>
		/// <param name="data">ArmorShaderData of the modded shader</param>
		public static void BindArmorShaderWithID<T>(int itemID, T data) where T : ArmorShaderData{
			var list = GetShaderList();
			var lookup = GetShaderBindings();
			list.Add(data);
			lookup.Add(itemID, list.Count);
			IncrementShaderCount();
			ShaderLibMod.instance.Logger.Info("Bound shader: " + list.Count);
		}

		/// <summary>
		/// Adds a shader without an item ID. Useful for special reasons.
		/// Technically speaking, this still uses item IDs, but since
		/// it starts at the lowest possible int value, there should be no issues.
		/// </summary>
		/// <returns>The item ID of the newly bound shader, which can be used to find shader ID</returns>
		/// <param name="data">ArmorShaderData of the modded shader</param>
		public static int BindArmorShaderNoID<T>(T data) where T : ArmorShaderData{
			var list = GetShaderList();
			var lookup = GetShaderBindings();
			list.Add(data);
			lookup.Add(customShaders + int.MinValue, list.Count);
			IncrementShaderCount();
			ShaderLibMod.instance.Logger.Info("Bound unlinked shader with ID: " + (customShaders + int.MinValue));
			return customShaders++ + int.MinValue;
		}
	}
}

