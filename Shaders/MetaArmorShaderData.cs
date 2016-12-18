/*using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using ShaderLib.Dyes;

namespace ShaderLib.Shaders
{
	/// <summary>
	/// A class designed for the creation of custom shaders.
	/// Use of delegate functions allows for maximum customizability.
	/// </summary>
	public class MetaArmorShaderData : ModArmorShaderData
	{
		public Dictionary<Tuple<string, string>, MetaDyeInfo.DyeEffects> components;

		public MetaArmorShaderData(Ref<Effect> shader, string passName) : base(shader, passName){
			components = new Dictionary<Tuple<string, string>, MetaDyeInfo.DyeEffects>();
		}

		public override bool Equals(object obj)
		{
			MetaArmorShaderData data = obj as MetaArmorShaderData;
			if(data == null) return false;
			if(components.Count != data.components.Count) return false;

			foreach(Tuple<string, string> item in components.Keys) {
				if(!data.components.ContainsKey(item))
					return false;
				if(data.components[item] != components[item])
					return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}*/