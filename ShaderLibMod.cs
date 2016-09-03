using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ShaderLib
{
	public class ShaderLibMod : Mod
	{
		//public static UI.Windows.CustomizerUI gui;
		public static bool guiOn;

		public ShaderLibMod()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public override void Load()
		{
			
		}
	}
}

