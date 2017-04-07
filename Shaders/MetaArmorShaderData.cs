using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace ShaderLib.Shaders
{
	/// <summary>
	/// A class designed for the creation of custom shaders by mixing other ones.
	/// Use of delegate functions allows for maximum customizability.
	///
	/// It's basically a niche feature I built for my own use.
	/// If you think you need it for whatever reason, go ahead, it's right here.
	/// </summary>
	public class MetaArmorShaderData : ModArmorShaderData
	{
		//Private values
		private Tuple<string, string> updatePrimary   = new Tuple<string, string>("", "");
		private Tuple<string, string> updateSecondary = new Tuple<string, string>("", "");
		private Tuple<string, string> preApply        = new Tuple<string, string>("", "");
		private Tuple<string, string> postApply       = new Tuple<string, string>("", "");
		private Tuple<string, string> secondaryShader = new Tuple<string, string>("", "");
		private Tuple<string, string> noiseImage      = new Tuple<string, string>("", "");

		//Properties for ease of use :D. Each one automatically sets the meta shader's method based on the information given.
		//If the information isn't correct (no shader found), the method and value are set to the default values.
		#region properties
		public Tuple<string, string> MetaUpdatePrimary {
			get {
				return updatePrimary;
			}
			set {
				var shader = mod.GetModShaderByNames(value);
				UpdatePrimary = shader != null ? shader.UpdatePrimary : new Func<Entity, DrawData?, Color>(delegate (Entity e, DrawData? drawData) { return primary; });
				updatePrimary = shader != null ? value : new Tuple<string, string>("", "");
			}
		}
		public Tuple<string, string> MetaUpdateSecondary {
			get {
				return updateSecondary;
			}
			set {
				var shader = mod.GetModShaderByNames(value);
				UpdateSecondary = shader != null ? shader.UpdateSecondary : new Func<Entity, DrawData?, Color>(delegate (Entity e, DrawData? drawData) { return secondary; });
				updateSecondary = shader != null ? value : new Tuple<string, string>("", "");
			}
		}
		public Tuple<string, string> MetaPreApply {
			get {
				return preApply;
			}
			set {
				var shader = mod.GetModShaderByNames(value);
				PreApply = shader != null ? shader.PreApply : new Action<Entity, DrawData?>(delegate (Entity e, DrawData? drawData) { });
				preApply = shader != null ? value : new Tuple<string, string>("", "");
			}
		}
		public Tuple<string, string> MetaPostApply {
			get {
				return postApply;
			}
			set {
				var shader = mod.GetModShaderByNames(value);
				PostApply = shader != null ? shader.PostApply : new Action<Entity, DrawData?>(delegate (Entity e, DrawData? drawData) { });
				postApply = shader != null ? value : new Tuple<string, string>("", "");
			}
		}
		public Tuple<string, string> MetaSecondaryShader {
			get {
				return secondaryShader;
			}
			set {
				var shader = mod.GetModShaderByNames(value);
				SecondaryShader = shader != null ? shader.SecondaryShader : SecondaryShader = new Func<Entity, ArmorShaderData>(delegate (Entity e) { return this; });
				secondaryShader = shader != null ? value : new Tuple<string, string>("", "");
			}
		}
		public Tuple<string, string> NoiseImage {
			get {
				return noiseImage;
			}
			set {
				var shader = value.Item1 != "Terraria" && int.Parse(value.Item2) <= ShaderLibMod.maxVanillaID ? (ArmorShaderData)mod.GetModShaderByNames(value) : ShaderReflections.GetShaderList()[int.Parse(value.Item2)];
				noiseImage = shader != null ? value : new Tuple<string, string>("", "");
				image = shader != null ? ShaderReflections.GetImage(shader).Value : null;
			}
		}
		#endregion

		public MetaArmorShaderData(Ref<Effect> shader, string passName) : base(shader, passName){ }

		public override bool Equals(object obj)
		{
			MetaArmorShaderData other = obj as MetaArmorShaderData;
			if(other == null) return false;
			if(other.updatePrimary   == this.updatePrimary   && other.updateSecondary == this.updateSecondary &&
			   other.preApply        == this.preApply        && other.postApply       == this.postApply       &&
			   other.secondaryShader == this.secondaryShader && other.noiseImage      == this.noiseImage      &&
			   other._passName       == this._passName       && other.primary         == this.primary         &&
			   other.secondary       == this.secondary       && other.saturation      == this.saturation) {
				return true;
			}

			return false;
		}

		//Stops a compiler warning :P
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
