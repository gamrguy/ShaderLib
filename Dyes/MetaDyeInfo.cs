/*using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ShaderLib.Shaders;

namespace ShaderLib.Dyes
{
	/// <summary>
	/// Holds info for "meta dyes", or dyes which are unique to the item.
	/// These are loaded into the shader list when the item is loaded, and ideally when the item is created.
	/// Such shaders should only be created when the same item may contain different unique dyes.
	/// Additionally, a "meta dye" can only be formed from parts of other dyes.
	/// </summary>
	public class MetaDyeInfo : ItemInfo
	{
		[Flags]
		public enum DyeEffects
		{
			//Normal flags, determines which component provides which effect on the result dye.
			PRIMARY = 1, SECONDARY = 2, SATURATION = 4, TYPE = 8, SPECIAL = 16, 

			//Special flags, these modify how the component applies its effects
			PRIMARY_AS_SECONDARY = 32, SECONDARY_AS_PRIMARY = 64
		}

		public int fakeItemID = 0;
		public Dictionary<Tuple<string, string>, DyeEffects> components = new Dictionary<Tuple<string, string>, DyeEffects>();

		public override ItemInfo Clone()
		{
			MetaDyeInfo clone = new MetaDyeInfo();

			//creates a deep copy, just to be safe
			foreach(KeyValuePair<Tuple<string, string>, DyeEffects> pair in components) {
				clone.AddManualComponent(new Tuple<string, string>(pair.Key.Item1, pair.Key.Item2), pair.Value);
			}
			clone.fakeItemID = fakeItemID;
			return clone;
		}

		/// <summary>
		/// Sets the given item as a component of this Meta Dye.
		/// The given DyeEffects may contain one or more flags,
		/// and the set flags determine which parts this Meta Dye
		/// inherits from the given item.
		/// 
		/// The given item MUST be a vanilla or modded dye.
		/// </summary>
		/// <param name="item">Item to inherit effects from.</param>
		/// <param name="effect">Effect flag(s), to determine which effect(s) to inherit.</param>
		public void SetEffect(Item item, DyeEffects effect){
			//If the given item isn't bound to a shader, NOPE.
			if(GameShaders.Armor.GetShaderFromItemId(item.type) == null){ 
				return;
			}

			//Remove any entries that share effects, prevents duplicate entries
			RemoveOffendingComponents(effect);

			Tuple<string, string> entry;
			if(item.modItem == null) {
				entry = new Tuple<string, string>("Terraria", item.type.ToString());
			} else {
				entry = new Tuple<string, string>(item.modItem.mod.Name, Main.itemName[item.type]);
			}

			//if item is already a component, add the given effects to it
			if(components.ContainsKey(entry)) components[entry] |= effect;
			else components.Add(entry, effect);
		}

		/// <summary>
		/// A method for manual component additions.
		/// Good for when an Item isn't available.
		/// </summary>
		/// <param name="entry">Tuple(mod name, item name)</param>
		/// <param name="effect">DyeEffects flags</param>
		public void AddManualComponent(Tuple<string, string> entry, DyeEffects effect){
			int type;
			if(entry.Item1 == "Terraria") {
				type = int.Parse(entry.Item2);
			} else {
				type = ModLoader.GetMod(entry.Item1)?.ItemType(entry.Item2) ?? 0;
				if(type == 0) return; //This is a currently unloaded item. Dye is borked.
			}
			if(GameShaders.Armor.GetShaderFromItemId(type) == null) return;
			RemoveOffendingComponents(effect);

			if(components.ContainsKey(entry)) components[entry] |= effect;
			else components.Add(entry, effect);
		}

		private void RemoveOffendingComponents(DyeEffects effect){
			foreach(DyeEffects e in Enum.GetValues(typeof(DyeEffects))) {
				int x = components.Count-1;
				for(int i = x; i >= 0; i--) {
					if(effect.HasFlag(e) && components.ElementAt(i).Value.HasFlag(e)){
						components.Remove(components.ElementAt(i).Key);
					}
				}
			}
		}

		/// <summary>
		/// Returns a composite ModArmorShaderData based
		/// on the given components.
		/// </summary>
		/// <returns>The data from components.</returns>
		public static ModArmorShaderData GetDataFromComponents(Dictionary<Tuple<string, string>, DyeEffects> components){
			MetaArmorShaderData result = new MetaArmorShaderData(ShaderLibMod.shaderRef, "ArmorColored");
			foreach(KeyValuePair<Tuple<string, string>, DyeEffects> pair in components) {
				int type;
				if(pair.Key.Item1 == "Terraria") {
					type = int.Parse(pair.Key.Item2);
				} else {
					type = ModLoader.GetMod(pair.Key.Item1)?.ItemType(pair.Key.Item2) ?? 0;
					if(type == 0) return null; //This is a currently unloaded item. Dye is borked.
				}

				ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(type);
				if(shader == null) return null; //This shader isn't loaded. Dye is borked.

				ModArmorShaderData modShader = shader as ModArmorShaderData;
				if(pair.Value.HasFlag(DyeEffects.PRIMARY)) {
					if(modShader == null) {
						if(pair.Value.HasFlag(DyeEffects.SECONDARY_AS_PRIMARY)) {
							result.primary = ShaderReflections.GetSecondaryColor(shader);
						} else {
							result.primary = ShaderReflections.GetPrimaryColor(shader);
						}
					} else {
						if(pair.Value.HasFlag(DyeEffects.SECONDARY_AS_PRIMARY)) {
							result.primary = modShader.secondary;
							result.UpdatePrimary = modShader.UpdateSecondary;
						} else {
							result.primary = modShader.primary;
							result.UpdatePrimary = modShader.UpdatePrimary;
						}
					}
				}
				if(pair.Value.HasFlag(DyeEffects.SECONDARY)) {
					if(modShader == null) {
						if(pair.Value.HasFlag(DyeEffects.PRIMARY_AS_SECONDARY)){
							result.secondary = ShaderReflections.GetPrimaryColor(shader);
						} else {
							result.secondary = ShaderReflections.GetSecondaryColor(shader);
						}
					} else {
						if(pair.Value.HasFlag(DyeEffects.PRIMARY_AS_SECONDARY)) {
							result.secondary = modShader.primary;
							result.UpdateSecondary = modShader.UpdatePrimary;
						} else {
							result.secondary = modShader.secondary;
							result.UpdateSecondary = modShader.UpdateSecondary;
						}
					}
				}
				if(pair.Value.HasFlag(DyeEffects.SATURATION)) {
					if(modShader == null) {
						result.saturation = ShaderReflections.GetSaturation(shader);
					} else {
						result.saturation = modShader.saturation;
					}
				}
				if(pair.Value.HasFlag(DyeEffects.TYPE)) {
					result.SwapProgram(ShaderReflections.GetPassName(shader));
					if(modShader == null) {
						Ref<Texture2D> image = ShaderReflections.GetImage(shader);
						if(image != null) {
							result.image = image.Value;
						}
					} else {
						result.image = modShader.image;
					}
				}
				if(pair.Value.HasFlag(DyeEffects.SPECIAL)){
					if(modShader != null){
						result.PreApply = modShader.PreApply;
						result.PostApply = modShader.PostApply;
					}
				}
				
				result.components.Add(new Tuple<string, string>(pair.Key.Item1, pair.Key.Item2), pair.Value);
			}
			return result;
		}
	}
}
*/
