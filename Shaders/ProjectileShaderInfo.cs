using System;
using Terraria.ModLoader;

namespace ShaderLib.Shaders
{
	public class ProjectileShaderInfo : ProjectileInfo
	{
		public bool parent = true; //Whether this is a parent projectile.
		public int shaderID = -1; //The shader being applied to this projectile
	}
}

