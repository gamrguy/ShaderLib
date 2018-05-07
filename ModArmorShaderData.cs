using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using ShaderLib.System;

namespace ShaderLib
{
	/// <summary>
	/// A class designed for the creation of custom shaders.
	/// </summary>
	public abstract class ModArmorShaderData : ArmorShaderData
	{
		public Mod Mod { get; internal set; }
		public virtual string Name => GetType().Name;
		public ShaderID ShaderID { get; internal set; }
		public virtual int? BoundItemID => null;

		private Color _primary = new Color(Vector3.One);
		public Color Primary {
			get { return _primary; }
			set {
				_primary = value;
				UseColor(_primary);
			}
		}

		private Color _secondary = new Color(Vector3.One);
		public Color Secondary {
			get { return _secondary; }
			set {
				_secondary = value;
				UseSecondaryColor(_secondary);
			}
		}

		private float _saturation = 1f;
		public float Saturation {
			get { return _saturation; }
			set {
				_saturation = value;
				UseSaturation(_saturation);
			}
		}

		private float _opacity = 1f;
		public float Opacity {
			get { return _opacity; }
			set {
				_opacity = value;
				UseOpacity(_opacity);
			}
		}

		public string PassName {
			get { return _passName; }
			set {
				_passName = value;
				SwapProgram(_passName);
			}
		}

		public new Effect Shader {
			get { return _shader != null ? _shader.Value : null; }
			set { _shader = new Ref<Effect>(value != null ? value : null); }
		}

		public Texture2D Image {
			get { return ShaderReflections.GetImage(this).Value; }
			set { ShaderReflections.SetImage(this, new Ref<Texture2D>(value)); }
		}

		/// <summary>
		/// Override this to change whether this shader autoloads.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public virtual bool Autoload() => true;

		/// <summary>
		/// Called before the shader is applied.
		/// Use this to change the shader's various properties or provide other effects.
		/// </summary>
		public virtual void PreApply(Entity e, DrawData? drawData) { }

		public ModArmorShaderData() : base(ShaderLibMod.shaderRef, "ArmorColored") { }

		public override void Apply(Entity e, DrawData? drawData) {
			PreApply(e, drawData);
			base.Apply(e, drawData);
		}
	}
}