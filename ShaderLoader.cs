using System;
using ShaderLib.System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System.Reflection;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace ShaderLib
{
	public static class ShaderLoader
	{
		internal static IDictionary<string, Assembly> Mods;
		internal static IDictionary<string, IDictionary<string, ModArmorShaderData>> ModShaders;
		internal static IDictionary<int, ModArmorShaderData> ModShadersByID;
		internal static IDictionary<string, IDictionary<string, GlobalShader>> GlobalShaders;
		private static int itemID;

		internal static void Initialize() {
			itemID = int.MinValue;
			Mods = new ConcurrentDictionary<string, Assembly>();
			ModShaders = new Dictionary<string, IDictionary<string, ModArmorShaderData>>();
			ModShadersByID = new Dictionary<int, ModArmorShaderData>();
			GlobalShaders = new Dictionary<string, IDictionary<string, GlobalShader>>();
		}

		internal static void Unload() {
			itemID = int.MinValue;
			Mods = null;
			ModShaders = null;
			ModShadersByID = null;
			GlobalShaders = null;
		}

		internal static void SetupContent() {
			if(Main.netMode == Terraria.ID.NetmodeID.Server) return;

			foreach(var kvp in Mods) {
				var ordered = kvp.Value
					.GetTypes()
					.OrderBy(x => x.FullName, StringComparer.InvariantCulture)
					.Where(t => t.IsClass && !t.IsAbstract); /* || type.GetConstructor(new Type[0]) == null*/

				var globalShaders = ordered.Where(x => x.IsSubclassOf(typeof(GlobalShader)));
				var modShaderData = ordered.Where(x => x.IsSubclassOf(typeof(ModArmorShaderData)));

				foreach(Type type in globalShaders) {
					AutoloadGlobalShader(type, ModLoader.GetMod(kvp.Key));
				}

				foreach(Type type in modShaderData) {
					AutoloadModArmorShaderData(type, ModLoader.GetMod(kvp.Key));
				}
			}

			//ErrorLogger.ClearLog();
			//ErrorLogger.Log(string.Join("\n", Rarities.Select(r => r.Value.Name)));
			//ErrorLogger.Log(string.Join("\n", Effects.Select(e => e.Value.Description)));
		}

		/// <summary>
		/// Registers the given mod as a mod to support loading of this library's classes.
		/// </summary>
		/// <param name="mod"></param>
		public static void RegisterMod(Mod mod) {
			if(Main.netMode == Terraria.ID.NetmodeID.Server) return;

			if(Mods == null) Initialize();

			bool? b = mod.GetType().GetField("loading", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(mod) as bool?;
			if(b != null && !b.Value) {
				throw new Exception("RegisterMod can only be called from Mod.Load or Mod.Autoload");
			}

			if(Mods.ContainsKey(mod.Name)) {
				throw new Exception($"Mod {mod.Name} is already registered");
			}

			Mods.Add(mod.Name, mod.Code);
			ModShaders.Add(mod.Name, new Dictionary<string, ModArmorShaderData>());
			GlobalShaders.Add(mod.Name, new Dictionary<string, GlobalShader>());
		}

		private static void AutoloadGlobalShader(Type type, Mod mod) {
			var shader = (GlobalShader)Activator.CreateInstance(type);
			if(shader.Autoload()) AddGlobalShader(shader, mod);
		}

		/// <summary>
		/// Adds the given GlobalShader.
		/// </summary>
		/// <param name="shader"></param>
		/// <param name="mod"></param>
		public static void AddGlobalShader(GlobalShader shader, Mod mod) {
			if(Main.netMode == Terraria.ID.NetmodeID.Server) return;

			//bool? b = mod.GetType().GetField("loading", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(mod) as bool?;
			//if (b != null && !b.Value)
			//	throw new Exception("AddGlobalShader can only be called from Mod.Load or Mod.Autoload");
			if(!Mods.ContainsKey(mod.Name))
				throw new Exception($"Mod {mod.Name} is not registered, please register before adding");
			if(GlobalShaders[mod.Name].ContainsKey(shader.Name))
				throw new Exception($"You have already added a GlobalShader with the name {shader.Name}");

			shader.Mod = mod;
			GlobalShaders[mod.Name].Add(shader.Name, shader);
		}

		private static void AutoloadModArmorShaderData(Type type, Mod mod) {
			var shader = (ModArmorShaderData)Activator.CreateInstance(type);
			shader.Mod = mod;
			if(shader.Autoload()) AddModArmorShaderData(shader, mod, shader.BoundItemID);
		}

		/// <summary>
		/// Adds and binds the given ModArmorShaderData.
		/// Optional item ID may be given to bind the shader to (default starts at minimum value).
		/// Returns the shader's given shader ID.
		/// </summary>
		/// <param name="shader"></param>
		/// <param name="mod"></param>
		/// <param name="item"></param>
		public static ShaderID AddModArmorShaderData(ModArmorShaderData shader, Mod mod, int? item = null) {
			if(Main.netMode == Terraria.ID.NetmodeID.Server)
				throw new Exception("Cannot load shaders on server");

			if(!Mods.ContainsKey(mod.Name))
				throw new Exception($"Mod {mod.Name} is not registered, please register before adding");
			if(ModShaders[mod.Name].ContainsKey(shader.Name))
				throw new Exception($"You have already added a ModArmorShaderData with the name {shader.Name}");

			if(item == null) item = itemID++;

			shader.Mod = mod;
			ModShaders[mod.Name].Add(shader.Name, shader);
			GameShaders.Armor.BindShader(item.Value, shader);
			shader.ShaderID = new ShaderID(mod.Name, shader.Name, GameShaders.Armor.GetShaderIdFromItemId(item.Value));
			ModShadersByID[shader.ShaderID.ID] = shader;
			return shader.ShaderID;
		}

		public static T GetModShader<T>(string modName, string name) where T : ModArmorShaderData {
			return (T)GetModShader(modName, name);
		}

		public static T GetModShader<T>(Mod mod, string name) where T : ModArmorShaderData {
			return GetModShader<T>(mod.Name, name);
		}

		public static T GetModShader<T>(int shaderID) where T : ModArmorShaderData {
			return (T)ModShadersByID[shaderID];
		}

		public static T GetModShader<T>(Mod mod) where T : ModArmorShaderData {
			return GetModShader<T>(mod.Name, typeof(T).Name);
		}

		public static ModArmorShaderData GetModShader(Mod mod, string name) {
			return GetModShader<ModArmorShaderData>(mod.Name, name);
		}

		public static ModArmorShaderData GetModShader(string modName, string name) {
			return ModShaders[modName][name];
		}

		public static ModArmorShaderData GetModShader(ShaderID id) {
			return GetModShader(id.ModName, id.ShaderName);
		}

		public static T GetGlobalShader<T>(string modName, string name) where T : GlobalShader {
			return (T)GlobalShaders[modName][name];
		}

		public static T GetGlobalShader<T>(Mod mod, string name) where T : GlobalShader {
			return GetGlobalShader<T>(mod.Name, name);
		}

		public static T GetGlobalShader<T>(Mod mod) where T : GlobalShader {
			return GetGlobalShader<T>(mod.Name, typeof(T).Name);
		}

		public static ShaderID GetShaderID(Item item) {
			if(item.modItem != null && item.modItem as IDye != null) return (item.modItem as IDye).DyeID;
			return new ShaderID(GameShaders.Armor.GetShaderIdFromItemId(item.type));
		}

		public static int GetShaderIDNum(Item item) {
			return GetShaderID(item).ID;
		}

		public static void ItemInventoryShader(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			foreach(var mod in GlobalShaders.Values) {
				foreach(var shader in mod.Values) {
					int shaderID = shader.ItemInventoryShader(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);

					if(shaderID > 0) {
						spriteBatch.Restart(Main.UIScaleMatrix, worldDraw: false);

						DrawData data = new DrawData
						{
							position = position - Main.screenPosition,
							scale = new Vector2(scale, scale),
							sourceRect = frame,
							texture = Main.itemTexture[item.type]
						};
						GameShaders.Armor.ApplySecondary(shaderID, Main.player[item.owner], data);
						break;
					}
				}
			}
		}

		public static void ItemWorldShader(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
			foreach(var mod in GlobalShaders.Values) {
				foreach(var shader in mod.Values) {
					int shaderID = shader.ItemWorldShader(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);

					if(shaderID > 0) {
						spriteBatch.Restart(Main.LocalPlayer.gravDir == 1f? Main.GameViewMatrix.ZoomMatrix : Main.GameViewMatrix.TransformationMatrix);

						DrawData data = new DrawData
						{
							position = item.position - Main.screenPosition,
							scale = new Vector2(scale, scale),
							texture = Main.itemTexture[item.type],
							rotation = rotation
						};
						GameShaders.Armor.ApplySecondary(shaderID, Main.player[item.owner], data);
					}
				}
			}
		}

		public static void ProjectileShader(Projectile projectile, SpriteBatch spriteBatch, Color lightColor) {
			foreach(var mod in GlobalShaders.Values) {
				foreach(var shader in mod.Values) {
					int shaderID = shader.ProjectileShader(projectile, spriteBatch, lightColor);

					if(shaderID > 0) {
						spriteBatch.Restart(Main.LocalPlayer.gravDir == 1f ? Main.GameViewMatrix.ZoomMatrix : Main.GameViewMatrix.TransformationMatrix);

						DrawData data = new DrawData
						{
							position = projectile.position - Main.screenPosition,
							scale = new Vector2(projectile.scale, projectile.scale),
							texture = Main.projectileTexture[projectile.type],
							rotation = projectile.rotation
						};
						data.sourceRect = data.texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
						GameShaders.Armor.ApplySecondary(shaderID, Main.player[projectile.owner], data);
					}
				}
			}
		}

		public static void NPCShader(NPC npc, SpriteBatch spriteBatch, Color drawColor) {
			foreach(var mod in GlobalShaders.Values) {
				foreach(var shader in mod.Values) {
					int shaderID = shader.NPCShader(npc, spriteBatch, drawColor);

					if(shaderID > 0) {
						spriteBatch.Restart(Main.LocalPlayer.gravDir == 1f ? Main.GameViewMatrix.ZoomMatrix : Main.GameViewMatrix.TransformationMatrix);

						DrawData data = new DrawData
						{
							position = npc.position - Main.screenPosition,
							scale = new Vector2(npc.scale, npc.scale),
							texture = Main.npcTexture[npc.type],
							sourceRect = npc.frame,//data.texture.Frame(1, Main.npcFrameCount[npc.type], 0, npc.frame);
							rotation = npc.rotation
						};
						GameShaders.Armor.ApplySecondary(shaderID, npc, data);
					}
				}
			}
		}

		public static PlayerShaderData PlayerShader(PlayerDrawInfo drawInfo) {
			var data = new PlayerShaderData(0);
			foreach(var mod in GlobalShaders.Values)
				foreach(var shader in mod.Values)
					shader.PlayerShader(ref data, drawInfo);
			return data;
		}

		public static void HeldItemShader(out int shaderID, Item item, PlayerDrawInfo drawInfo) {
			shaderID = 0;
			foreach(var mod in GlobalShaders.Values)
				foreach(var shader in mod.Values)
					shader.HeldItemShader(ref shaderID, item, drawInfo);
		}
	}
}
