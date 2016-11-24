using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace ShaderLib.Shaders
{
	/// <summary>
	/// A class designed for the creation of custom shaders.
	/// Use of delegate functions allows for maximum customizability.
	/// </summary>
	public class ModArmorShaderData : ArmorShaderData
	{
		public Color primary = default(Color);    //The primary color.
		public Color secondary = default(Color);  //The secondary color.
		public float saturation = 1f; //The saturation of this shader; how much its color shows through. Probably.
		public Texture2D image;  //Texture to use as a noise image.

		/// <summary>
		/// Called before the shader is applied. Allows customization of the shader's primary color.
		/// </summary>
		public Func<Entity, DrawData?, Color> UpdatePrimary;

		/// <summary>
		/// Called before the shader is applied. Allows customization of the shader's secondary color.
		/// </summary>
		public Func<Entity, DrawData?, Color> UpdateSecondary;

		/// <summary>
		/// Called before the shader is applied.
		/// </summary>
		public Action<Entity, DrawData?> PreApply = new Action<Entity, DrawData?>(delegate(Entity e, DrawData? drawData) {});

		/// <summary>
		/// Called after the shader is applied.
		/// </summary>
		public Action<Entity, DrawData?> PostApply = new Action<Entity, DrawData?>(delegate(Entity e, DrawData? drawData) {});

		/// <summary>
		/// Called when the secondary shader is requested.
		/// </summary>
		public Func<Entity, ArmorShaderData> SecondaryShader;

		public ModArmorShaderData(Effect shader, string passName) : base(shader, passName){
			//Set default delegates if not set by user
			if(UpdatePrimary == null) {
				UpdatePrimary = new Func<Entity, DrawData?, Color>(delegate(Entity e, DrawData? drawData) {
					return primary;
				});
			}
			if(UpdateSecondary == null) {
				UpdateSecondary = new Func<Entity, DrawData?, Color>(delegate(Entity e, DrawData? drawData) {
					return secondary;
				});
			}
			if(SecondaryShader == null) {
				SecondaryShader = new Func<Entity, ArmorShaderData>(delegate(Entity e) {
					return this;
				});
			}
		}

		public override void Apply(Entity e, DrawData? drawData)
		{
			UseColor(UpdatePrimary(e, drawData));
			UseSecondaryColor(UpdateSecondary(e, drawData));
			UseSaturation(saturation);
			if(image != null) ShaderReflections.SetImage(this as ArmorShaderData, new Ref<Texture2D>(image));
			SwapProgram(_passName);

			PreApply(e, drawData);
			base.Apply(e, drawData);
			PostApply(e, drawData);
		}

		public override ArmorShaderData GetSecondaryShader(Entity entity)
		{
			return SecondaryShader(entity);
		}
	}
}