using System;
using Terraria.ModLoader.IO;

namespace ShaderLib.Shaders
{
	/// <summary>
	/// A class used for easy and standardized saving and loading of shaders.
	/// It's easy to use: just give it a shader ID and it'll do the rest.
	/// When loading, the resultant shader ID should refer to the same shader you saved.
	/// </summary>
	public static class ShaderIO
	{
		private enum ShaderTypes
		{
			VANILLA, MODDED
		}

		/// <summary>
		/// Extracts a saved shader from a given TagCompound, returning its shader ID.
		/// </summary>
		/// <returns>The extracted shader ID.</returns>
		/// <param name="compound">TagCompound to extract the shader from.</param>
		public static int LoadShader(TagCompound savedShader) {
			try
			{
				byte shaderType = savedShader.GetByte("Type");

				//Load vanilla shader; easily identified by unchanging shader ID
				if (shaderType == (byte)ShaderTypes.VANILLA)
				{
					return savedShader.GetInt("ShaderID");
				}

				//Load modded shader, uniquely identified by mod name and shader name
				else if (shaderType == (byte)ShaderTypes.MODDED)
				{
					string modName = savedShader.GetString("ModName");
					string shaderName = savedShader.GetString("ShaderName");

					var shader = ShaderLoader.ModShaders[modName][shaderName];
					if (savedShader.ContainsKey("CustomData"))
						shader.Load(savedShader.GetCompound("CustomData"));

					return shader.ID;
				}
			}
			catch (Exception)
			{
				return 0;
			}

			return 0;
		}

		/// <summary>
		/// Saves the given shader to a returned TagCompound.
		/// </summary>
		/// <returns>A TagCompound containing information on the shader to be loaded later.</returns>
		/// <param name="shaderID">Shader ID of the shader to be saved.</param>
		public static TagCompound SaveShader(int shaderID) {
			TagCompound compound = new TagCompound();

            //Save vanilla ID if in vanilla shaders
            if (shaderID <= ShaderLibMod.maxVanillaID)
            {
                compound.Set("Type", (byte)ShaderTypes.VANILLA);
                compound.Set("ShaderID", shaderID);
            }

            //Save mod name and shader name if modded shader
            else
            {
                compound.Set("Type", (byte)ShaderTypes.MODDED);

				var shader = ShaderLoader.ModShadersByID[shaderID];
                compound.Set("ModName", shader.Mod.Name);
                compound.Set("ShaderName", shader.Name);

				var custom = shader.Save();
				if (custom != null) compound.Set("CustomData", custom);
            }

			//If nothing was saved (perhaps an invalid shader ID) then nothing is saved and nothing is loaded (you get 0)
			return compound;
		}
	}
}