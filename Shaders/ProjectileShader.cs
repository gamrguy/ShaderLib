using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace ShaderLib.Shaders
{
	public class ProjectileShader : GlobalProjectile
	{
		public override bool PreDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileShaderInfo projInfo = projectile.GetModInfo<ProjectileShaderInfo>(mod);

			//Only affect projectiles with shaders
			if(projInfo.shaderID > 0) {
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationZ(0f) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0f)));

				DrawData data = new DrawData();
				data.origin = projectile.Center;
				data.position = projectile.position - Main.screenPosition;
				data.scale = new Vector2(projectile.scale, projectile.scale);
				data.texture = Main.projectileTexture[projectile.type];
				data.sourceRect = data.texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
				GameShaders.Armor.ApplySecondary(projInfo.shaderID, Main.player[projectile.owner], data);

				//only apply custom drawing to projectiles without it
				if(projectile.modProjectile == null || projectile.modProjectile.PreDraw(spriteBatch, lightColor)) {
					DrawProj(projectile.whoAmI/*, projectile*/);
					//Main.instance.DrawProj(projectile.whoAmI);
				}
				return false;
			}

			return true;
		}

		//Resets the SpriteBatch after drawing projectile, to prepare for next projectile
		public override void PostDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationZ(0f) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0f)));
		}

		//Vanilla code. Ignore. Please.
		private static void DrawProj(int i){
			float num = 0f;
			float num2 = 0f;
			Projectile projectile = Main.projectile[i];
			//this.LoadProjectile(projectile.type);
			Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
			if (projectile.aiStyle == 99)
			{
				Vector2 vector = mountedCenter;
				vector.Y += Main.player[projectile.owner].gfxOffY;
				float num3 = projectile.Center.X - vector.X;
				float num4 = projectile.Center.Y - vector.Y;
				Math.Sqrt((double)(num3 * num3 + num4 * num4));
				float rotation = (float)Math.Atan2((double)num4, (double)num3) - 1.57f;
				if (!projectile.counterweight)
				{
					int num5 = -1;
					if (projectile.position.X + (float)(projectile.width / 2) < Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2))
					{
						num5 = 1;
					}
					num5 *= -1;
					Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num4 * (float)num5), (double)(num3 * (float)num5));
				}
				bool flag = true;
				if (num3 == 0f && num4 == 0f)
				{
					flag = false;
				}
				else
				{
					float num6 = (float)Math.Sqrt((double)(num3 * num3 + num4 * num4));
					num6 = 12f / num6;
					num3 *= num6;
					num4 *= num6;
					vector.X -= num3 * 0.1f;
					vector.Y -= num4 * 0.1f;
					num3 = projectile.position.X + (float)projectile.width * 0.5f - vector.X;
					num4 = projectile.position.Y + (float)projectile.height * 0.5f - vector.Y;
				}
				while (flag)
				{
					float num7 = 12f;
					float num8 = (float)Math.Sqrt((double)(num3 * num3 + num4 * num4));
					float num9 = num8;
					if (float.IsNaN(num8) || float.IsNaN(num9))
					{
						flag = false;
					}
					else
					{
						if (num8 < 20f)
						{
							num7 = num8 - 8f;
							flag = false;
						}
						num8 = 12f / num8;
						num3 *= num8;
						num4 *= num8;
						vector.X += num3;
						vector.Y += num4;
						num3 = projectile.position.X + (float)projectile.width * 0.5f - vector.X;
						num4 = projectile.position.Y + (float)projectile.height * 0.1f - vector.Y;
						if (num9 > 12f)
						{
							float num10 = 0.3f;
							float num11 = Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y);
							if (num11 > 16f)
							{
								num11 = 16f;
							}
							num11 = 1f - num11 / 16f;
							num10 *= num11;
							num11 = num9 / 80f;
							if (num11 > 1f)
							{
								num11 = 1f;
							}
							num10 *= num11;
							if (num10 < 0f)
							{
								num10 = 0f;
							}
							num10 *= num11;
							num10 *= 0.5f;
							if (num4 > 0f)
							{
								num4 *= 1f + num10;
								num3 *= 1f - num10;
							}
							else
							{
								num11 = Math.Abs(projectile.velocity.X) / 3f;
								if (num11 > 1f)
								{
									num11 = 1f;
								}
								num11 -= 0.5f;
								num10 *= num11;
								if (num10 > 0f)
								{
									num10 *= 2f;
								}
								num4 *= 1f + num10;
								num3 *= 1f - num10;
							}
						}
						rotation = (float)Math.Atan2((double)num4, (double)num3) - 1.57f;
						int stringColor = Main.player[projectile.owner].stringColor;
						Microsoft.Xna.Framework.Color color = WorldGen.paintColor(stringColor);
						if (color.R < 75)
						{
							color.R = 75;
						}
						if (color.G < 75)
						{
							color.G = 75;
						}
						if (color.B < 75)
						{
							color.B = 75;
						}
						if (stringColor == 13)
						{
							color = new Microsoft.Xna.Framework.Color(20, 20, 20);
						}
						else if (stringColor == 14 || stringColor == 0)
						{
							color = new Microsoft.Xna.Framework.Color(200, 200, 200);
						}
						else if (stringColor == 28)
						{
							color = new Microsoft.Xna.Framework.Color(163, 116, 91);
						}
						else if (stringColor == 27)
						{
							color = new Microsoft.Xna.Framework.Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
						}
						color.A = (byte)((float)color.A * 0.4f);
						float num12 = 0.5f;
						color = Lighting.GetColor((int)vector.X / 16, (int)(vector.Y / 16f), color);
						color = new Microsoft.Xna.Framework.Color((int)((byte)((float)color.R * num12)), (int)((byte)((float)color.G * num12)), (int)((byte)((float)color.B * num12)), (int)((byte)((float)color.A * num12)));
						Main.spriteBatch.Draw(Main.fishingLineTexture, new Vector2(vector.X - Main.screenPosition.X + (float)Main.fishingLineTexture.Width * 0.5f, vector.Y - Main.screenPosition.Y + (float)Main.fishingLineTexture.Height * 0.5f) - new Vector2(6f, 0f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.fishingLineTexture.Width, (int)num7)), color, rotation, new Vector2((float)Main.fishingLineTexture.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			if (projectile.bobber && Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].holdStyle > 0)
			{
				num = mountedCenter.X;
				num2 = mountedCenter.Y;
				num2 += Main.player[projectile.owner].gfxOffY;
				int type = Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].type;
				float gravDir = Main.player[projectile.owner].gravDir;
				if (type == 2289)
				{
					num += (float)(43 * Main.player[projectile.owner].direction);
					if (Main.player[projectile.owner].direction < 0)
					{
						num -= 13f;
					}
					num2 -= 36f * gravDir;
				}
				else if (type == 2291)
				{
					num += (float)(43 * Main.player[projectile.owner].direction);
					if (Main.player[projectile.owner].direction < 0)
					{
						num -= 13f;
					}
					num2 -= 34f * gravDir;
				}
				else if (type == 2292)
				{
					num += (float)(46 * Main.player[projectile.owner].direction);
					if (Main.player[projectile.owner].direction < 0)
					{
						num -= 13f;
					}
					num2 -= 34f * gravDir;
				}
				else if (type == 2293)
				{
					num += (float)(43 * Main.player[projectile.owner].direction);
					if (Main.player[projectile.owner].direction < 0)
					{
						num -= 13f;
					}
					num2 -= 34f * gravDir;
				}
				else if (type == 2294)
				{
					num += (float)(43 * Main.player[projectile.owner].direction);
					if (Main.player[projectile.owner].direction < 0)
					{
						num -= 13f;
					}
					num2 -= 30f * gravDir;
				}
				else if (type == 2295)
				{
					num += (float)(43 * Main.player[projectile.owner].direction);
					if (Main.player[projectile.owner].direction < 0)
					{
						num -= 13f;
					}
					num2 -= 30f * gravDir;
				}
				else if (type == 2296)
				{
					num += (float)(43 * Main.player[projectile.owner].direction);
					if (Main.player[projectile.owner].direction < 0)
					{
						num -= 13f;
					}
					num2 -= 30f * gravDir;
				}
				else if (type == 2421)
				{
					num += (float)(47 * Main.player[projectile.owner].direction);
					if (Main.player[projectile.owner].direction < 0)
					{
						num -= 13f;
					}
					num2 -= 36f * gravDir;
				}
				else if (type == 2422)
				{
					num += (float)(47 * Main.player[projectile.owner].direction);
					if (Main.player[projectile.owner].direction < 0)
					{
						num -= 13f;
					}
					num2 -= 32f * gravDir;
				}
				if (gravDir == -1f)
				{
					num2 -= 12f;
				}
				Vector2 value = new Vector2(num, num2);
				value = Main.player[projectile.owner].RotatedRelativePoint(value + new Vector2(8f), true) - new Vector2(8f);
				float num13 = projectile.position.X + (float)projectile.width * 0.5f - value.X;
				float num14 = projectile.position.Y + (float)projectile.height * 0.5f - value.Y;
				Math.Sqrt((double)(num13 * num13 + num14 * num14));
				float rotation2 = (float)Math.Atan2((double)num14, (double)num13) - 1.57f;
				bool flag2 = true;
				if (num13 == 0f && num14 == 0f)
				{
					flag2 = false;
				}
				else
				{
					float num15 = (float)Math.Sqrt((double)(num13 * num13 + num14 * num14));
					num15 = 12f / num15;
					num13 *= num15;
					num14 *= num15;
					value.X -= num13;
					value.Y -= num14;
					num13 = projectile.position.X + (float)projectile.width * 0.5f - value.X;
					num14 = projectile.position.Y + (float)projectile.height * 0.5f - value.Y;
				}
				while (flag2)
				{
					float num16 = 12f;
					float num17 = (float)Math.Sqrt((double)(num13 * num13 + num14 * num14));
					float num18 = num17;
					if (float.IsNaN(num17) || float.IsNaN(num18))
					{
						flag2 = false;
					}
					else
					{
						if (num17 < 20f)
						{
							num16 = num17 - 8f;
							flag2 = false;
						}
						num17 = 12f / num17;
						num13 *= num17;
						num14 *= num17;
						value.X += num13;
						value.Y += num14;
						num13 = projectile.position.X + (float)projectile.width * 0.5f - value.X;
						num14 = projectile.position.Y + (float)projectile.height * 0.1f - value.Y;
						if (num18 > 12f)
						{
							float num19 = 0.3f;
							float num20 = Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y);
							if (num20 > 16f)
							{
								num20 = 16f;
							}
							num20 = 1f - num20 / 16f;
							num19 *= num20;
							num20 = num18 / 80f;
							if (num20 > 1f)
							{
								num20 = 1f;
							}
							num19 *= num20;
							if (num19 < 0f)
							{
								num19 = 0f;
							}
							num20 = 1f - projectile.localAI[0] / 100f;
							num19 *= num20;
							if (num14 > 0f)
							{
								num14 *= 1f + num19;
								num13 *= 1f - num19;
							}
							else
							{
								num20 = Math.Abs(projectile.velocity.X) / 3f;
								if (num20 > 1f)
								{
									num20 = 1f;
								}
								num20 -= 0.5f;
								num19 *= num20;
								if (num19 > 0f)
								{
									num19 *= 2f;
								}
								num14 *= 1f + num19;
								num13 *= 1f - num19;
							}
						}
						rotation2 = (float)Math.Atan2((double)num14, (double)num13) - 1.57f;
						Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), new Microsoft.Xna.Framework.Color(200, 200, 200, 100));
						if (type == 2294)
						{
							color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), new Microsoft.Xna.Framework.Color(100, 180, 230, 100));
						}
						if (type == 2295)
						{
							color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), new Microsoft.Xna.Framework.Color(250, 90, 70, 100));
						}
						if (type == 2293)
						{
							color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), new Microsoft.Xna.Framework.Color(203, 190, 210, 100));
						}
						if (type == 2421)
						{
							color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), new Microsoft.Xna.Framework.Color(183, 77, 112, 100));
						}
						if (type == 2422)
						{
							color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), new Microsoft.Xna.Framework.Color(255, 226, 116, 100));
						}
						Main.spriteBatch.Draw(Main.fishingLineTexture, new Vector2(value.X - Main.screenPosition.X + (float)Main.fishingLineTexture.Width * 0.5f, value.Y - Main.screenPosition.Y + (float)Main.fishingLineTexture.Height * 0.5f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.fishingLineTexture.Width, (int)num16)), color2, rotation2, new Vector2((float)Main.fishingLineTexture.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 32)
			{
				Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num21 = mountedCenter.X - vector2.X;
				float num22 = mountedCenter.Y - vector2.Y;
				float rotation3 = (float)Math.Atan2((double)num22, (double)num21) - 1.57f;
				bool flag3 = true;
				if (num21 == 0f && num22 == 0f)
				{
					flag3 = false;
				}
				else
				{
					float num23 = (float)Math.Sqrt((double)(num21 * num21 + num22 * num22));
					num23 = 8f / num23;
					num21 *= num23;
					num22 *= num23;
					vector2.X -= num21;
					vector2.Y -= num22;
					num21 = mountedCenter.X - vector2.X;
					num22 = mountedCenter.Y - vector2.Y;
				}
				while (flag3)
				{
					float num24 = (float)Math.Sqrt((double)(num21 * num21 + num22 * num22));
					if (num24 < 28f)
					{
						flag3 = false;
					}
					else if (float.IsNaN(num24))
					{
						flag3 = false;
					}
					else
					{
						num24 = 28f / num24;
						num21 *= num24;
						num22 *= num24;
						vector2.X += num21;
						vector2.Y += num22;
						num21 = mountedCenter.X - vector2.X;
						num22 = mountedCenter.Y - vector2.Y;
						Microsoft.Xna.Framework.Color color3 = Lighting.GetColor((int)vector2.X / 16, (int)(vector2.Y / 16f));
						Main.spriteBatch.Draw(Main.chain5Texture, new Vector2(vector2.X - Main.screenPosition.X, vector2.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain5Texture.Width, Main.chain5Texture.Height)), color3, rotation3, new Vector2((float)Main.chain5Texture.Width * 0.5f, (float)Main.chain5Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 73)
			{
				Vector2 vector3 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num25 = mountedCenter.X - vector3.X;
				float num26 = mountedCenter.Y - vector3.Y;
				float rotation4 = (float)Math.Atan2((double)num26, (double)num25) - 1.57f;
				bool flag4 = true;
				while (flag4)
				{
					float num27 = (float)Math.Sqrt((double)(num25 * num25 + num26 * num26));
					if (num27 < 25f)
					{
						flag4 = false;
					}
					else if (float.IsNaN(num27))
					{
						flag4 = false;
					}
					else
					{
						num27 = 12f / num27;
						num25 *= num27;
						num26 *= num27;
						vector3.X += num25;
						vector3.Y += num26;
						num25 = mountedCenter.X - vector3.X;
						num26 = mountedCenter.Y - vector3.Y;
						Microsoft.Xna.Framework.Color color4 = Lighting.GetColor((int)vector3.X / 16, (int)(vector3.Y / 16f));
						Main.spriteBatch.Draw(Main.chain8Texture, new Vector2(vector3.X - Main.screenPosition.X, vector3.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain8Texture.Width, Main.chain8Texture.Height)), color4, rotation4, new Vector2((float)Main.chain8Texture.Width * 0.5f, (float)Main.chain8Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 186)
			{
				Vector2 vector4 = new Vector2(projectile.localAI[0], projectile.localAI[1]);
				float num28 = Vector2.Distance(projectile.Center, vector4) - projectile.velocity.Length();
				float num29 = (float)Main.chain17Texture.Height - num28;
				if (num28 > 0f && projectile.ai[1] > 0f)
				{
					Microsoft.Xna.Framework.Color color5 = Lighting.GetColor((int)projectile.position.X / 16, (int)projectile.position.Y / 16);
					Main.spriteBatch.Draw(Main.chain17Texture, vector4 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, (int)num29, Main.chain17Texture.Width, (int)num28)), color5, projectile.rotation, new Vector2((float)(Main.chain17Texture.Width / 2), 0f), 1f, SpriteEffects.None, 0f);
				}
			}
			else if (projectile.type == 74)
			{
				Vector2 vector5 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num30 = mountedCenter.X - vector5.X;
				float num31 = mountedCenter.Y - vector5.Y;
				float rotation5 = (float)Math.Atan2((double)num31, (double)num30) - 1.57f;
				bool flag5 = true;
				while (flag5)
				{
					float num32 = (float)Math.Sqrt((double)(num30 * num30 + num31 * num31));
					if (num32 < 25f)
					{
						flag5 = false;
					}
					else if (float.IsNaN(num32))
					{
						flag5 = false;
					}
					else
					{
						num32 = 12f / num32;
						num30 *= num32;
						num31 *= num32;
						vector5.X += num30;
						vector5.Y += num31;
						num30 = mountedCenter.X - vector5.X;
						num31 = mountedCenter.Y - vector5.Y;
						Microsoft.Xna.Framework.Color color6 = Lighting.GetColor((int)vector5.X / 16, (int)(vector5.Y / 16f));
						Main.spriteBatch.Draw(Main.chain9Texture, new Vector2(vector5.X - Main.screenPosition.X, vector5.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain8Texture.Width, Main.chain8Texture.Height)), color6, rotation5, new Vector2((float)Main.chain8Texture.Width * 0.5f, (float)Main.chain8Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 171)
			{
				Vector2 vector6 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num33 = -projectile.velocity.X;
				float num34 = -projectile.velocity.Y;
				float num35 = 1f;
				if (projectile.ai[0] <= 17f)
				{
					num35 = projectile.ai[0] / 17f;
				}
				int num36 = (int)(30f * num35);
				float num37 = 1f;
				if (projectile.ai[0] <= 30f)
				{
					num37 = projectile.ai[0] / 30f;
				}
				float num38 = 0.4f * num37;
				float num39 = num38;
				num34 += num39;
				Vector2[] array = new Vector2[num36];
				float[] array2 = new float[num36];
				for (int j = 0; j < num36; j++)
				{
					float num40 = (float)Math.Sqrt((double)(num33 * num33 + num34 * num34));
					float num41 = 5.6f;
					if (Math.Abs(num33) + Math.Abs(num34) < 1f)
					{
						num41 *= Math.Abs(num33) + Math.Abs(num34) / 1f;
					}
					num40 = num41 / num40;
					num33 *= num40;
					num34 *= num40;
					float num42 = (float)Math.Atan2((double)num34, (double)num33) - 1.57f;
					array[j].X = vector6.X;
					array[j].Y = vector6.Y;
					array2[j] = num42;
					vector6.X += num33;
					vector6.Y += num34;
					num33 = -projectile.velocity.X;
					num34 = -projectile.velocity.Y;
					num39 += num38;
					num34 += num39;
				}
				for (int k = num36 - 1; k >= 0; k--)
				{
					vector6.X = array[k].X;
					vector6.Y = array[k].Y;
					float rotation6 = array2[k];
					Microsoft.Xna.Framework.Color color7 = Lighting.GetColor((int)vector6.X / 16, (int)(vector6.Y / 16f));
					Main.spriteBatch.Draw(Main.chain16Texture, new Vector2(vector6.X - Main.screenPosition.X, vector6.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain16Texture.Width, Main.chain16Texture.Height)), color7, rotation6, new Vector2((float)Main.chain16Texture.Width * 0.5f, (float)Main.chain16Texture.Height * 0.5f), 0.8f, SpriteEffects.None, 0f);
				}
			}
			else if (projectile.type == 475)
			{
				Vector2 vector7 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num43 = -projectile.velocity.X;
				float num44 = -projectile.velocity.Y;
				float num45 = 1f;
				if (projectile.ai[0] <= 17f)
				{
					num45 = projectile.ai[0] / 17f;
				}
				int num46 = (int)(30f * num45);
				float num47 = 1f;
				if (projectile.ai[0] <= 30f)
				{
					num47 = projectile.ai[0] / 30f;
				}
				float num48 = 0.4f * num47;
				float num49 = num48;
				num44 += num49;
				Vector2[] array3 = new Vector2[num46];
				float[] array4 = new float[num46];
				for (int l = 0; l < num46; l++)
				{
					float num50 = (float)Math.Sqrt((double)(num43 * num43 + num44 * num44));
					float num51 = 5.6f;
					if (Math.Abs(num43) + Math.Abs(num44) < 1f)
					{
						num51 *= Math.Abs(num43) + Math.Abs(num44) / 1f;
					}
					num50 = num51 / num50;
					num43 *= num50;
					num44 *= num50;
					float num52 = (float)Math.Atan2((double)num44, (double)num43) - 1.57f;
					array3[l].X = vector7.X;
					array3[l].Y = vector7.Y;
					array4[l] = num52;
					vector7.X += num43;
					vector7.Y += num44;
					num43 = -projectile.velocity.X;
					num44 = -projectile.velocity.Y;
					num49 += num48;
					num44 += num49;
				}
				int num53 = 0;
				for (int m = num46 - 1; m >= 0; m--)
				{
					vector7.X = array3[m].X;
					vector7.Y = array3[m].Y;
					float rotation7 = array4[m];
					Microsoft.Xna.Framework.Color color8 = Lighting.GetColor((int)vector7.X / 16, (int)(vector7.Y / 16f));
					if (num53 % 2 == 0)
					{
						Main.spriteBatch.Draw(Main.chain38Texture, new Vector2(vector7.X - Main.screenPosition.X, vector7.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain38Texture.Width, Main.chain38Texture.Height)), color8, rotation7, new Vector2((float)Main.chain38Texture.Width * 0.5f, (float)Main.chain38Texture.Height * 0.5f), 0.8f, SpriteEffects.None, 0f);
					}
					else
					{
						Main.spriteBatch.Draw(Main.chain39Texture, new Vector2(vector7.X - Main.screenPosition.X, vector7.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain39Texture.Width, Main.chain39Texture.Height)), color8, rotation7, new Vector2((float)Main.chain39Texture.Width * 0.5f, (float)Main.chain39Texture.Height * 0.5f), 0.8f, SpriteEffects.None, 0f);
					}
					num53++;
				}
			}
			else if (projectile.type == 505 || projectile.type == 506)
			{
				Vector2 vector8 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num54 = -projectile.velocity.X;
				float num55 = -projectile.velocity.Y;
				float num56 = 1f;
				if (projectile.ai[0] <= 17f)
				{
					num56 = projectile.ai[0] / 17f;
				}
				int num57 = (int)(30f * num56);
				float num58 = 1f;
				if (projectile.ai[0] <= 30f)
				{
					num58 = projectile.ai[0] / 30f;
				}
				float num59 = 0.4f * num58;
				float num60 = num59;
				num55 += num60;
				Vector2[] array5 = new Vector2[num57];
				float[] array6 = new float[num57];
				for (int n = 0; n < num57; n++)
				{
					float num61 = (float)Math.Sqrt((double)(num54 * num54 + num55 * num55));
					float num62 = 5.6f;
					if (Math.Abs(num54) + Math.Abs(num55) < 1f)
					{
						num62 *= Math.Abs(num54) + Math.Abs(num55) / 1f;
					}
					num61 = num62 / num61;
					num54 *= num61;
					num55 *= num61;
					float num63 = (float)Math.Atan2((double)num55, (double)num54) - 1.57f;
					array5[n].X = vector8.X;
					array5[n].Y = vector8.Y;
					array6[n] = num63;
					vector8.X += num54;
					vector8.Y += num55;
					num54 = -projectile.velocity.X;
					num55 = -projectile.velocity.Y;
					num60 += num59;
					num55 += num60;
				}
				int num64 = 0;
				for (int num65 = num57 - 1; num65 >= 0; num65--)
				{
					vector8.X = array5[num65].X;
					vector8.Y = array5[num65].Y;
					float rotation8 = array6[num65];
					Microsoft.Xna.Framework.Color color9 = Lighting.GetColor((int)vector8.X / 16, (int)(vector8.Y / 16f));
					int num66 = 4;
					if (projectile.type == 506)
					{
						num66 = 6;
					}
					num66 += num64 % 2;
					Main.spriteBatch.Draw(Main.chainsTexture[num66], new Vector2(vector8.X - Main.screenPosition.X, vector8.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chainsTexture[num66].Width, Main.chainsTexture[num66].Height)), color9, rotation8, new Vector2((float)Main.chainsTexture[num66].Width * 0.5f, (float)Main.chainsTexture[num66].Height * 0.5f), 0.8f, SpriteEffects.None, 0f);
					num64++;
				}
			}
			else if (projectile.type == 165)
			{
				Vector2 vector9 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num67 = mountedCenter.X - vector9.X;
				float num68 = mountedCenter.Y - vector9.Y;
				float rotation9 = (float)Math.Atan2((double)num68, (double)num67) - 1.57f;
				bool flag6 = true;
				while (flag6)
				{
					float num69 = (float)Math.Sqrt((double)(num67 * num67 + num68 * num68));
					if (num69 < 25f)
					{
						flag6 = false;
					}
					else if (float.IsNaN(num69))
					{
						flag6 = false;
					}
					else
					{
						num69 = 24f / num69;
						num67 *= num69;
						num68 *= num69;
						vector9.X += num67;
						vector9.Y += num68;
						num67 = mountedCenter.X - vector9.X;
						num68 = mountedCenter.Y - vector9.Y;
						Microsoft.Xna.Framework.Color color10 = Lighting.GetColor((int)vector9.X / 16, (int)(vector9.Y / 16f));
						Main.spriteBatch.Draw(Main.chain15Texture, new Vector2(vector9.X - Main.screenPosition.X, vector9.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain15Texture.Width, Main.chain15Texture.Height)), color10, rotation9, new Vector2((float)Main.chain15Texture.Width * 0.5f, (float)Main.chain15Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type >= 230 && projectile.type <= 235)
			{
				int num70 = projectile.type - 229;
				Vector2 vector10 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num71 = mountedCenter.X - vector10.X;
				float num72 = mountedCenter.Y - vector10.Y;
				float rotation10 = (float)Math.Atan2((double)num72, (double)num71) - 1.57f;
				bool flag7 = true;
				while (flag7)
				{
					float num73 = (float)Math.Sqrt((double)(num71 * num71 + num72 * num72));
					if (num73 < 25f)
					{
						flag7 = false;
					}
					else if (float.IsNaN(num73))
					{
						flag7 = false;
					}
					else
					{
						num73 = (float)Main.gemChainTexture[num70].Height / num73;
						num71 *= num73;
						num72 *= num73;
						vector10.X += num71;
						vector10.Y += num72;
						num71 = mountedCenter.X - vector10.X;
						num72 = mountedCenter.Y - vector10.Y;
						Microsoft.Xna.Framework.Color color11 = Lighting.GetColor((int)vector10.X / 16, (int)(vector10.Y / 16f));
						Main.spriteBatch.Draw(Main.gemChainTexture[num70], new Vector2(vector10.X - Main.screenPosition.X, vector10.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.gemChainTexture[num70].Width, Main.gemChainTexture[num70].Height)), color11, rotation10, new Vector2((float)Main.gemChainTexture[num70].Width * 0.5f, (float)Main.gemChainTexture[num70].Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 256)
			{
				Vector2 vector11 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num74 = mountedCenter.X - vector11.X;
				float num75 = mountedCenter.Y - vector11.Y;
				float num76 = (float)Math.Atan2((double)num75, (double)num74) - 1.57f;
				bool flag8 = true;
				while (flag8)
				{
					float num77 = (float)Math.Sqrt((double)(num74 * num74 + num75 * num75));
					if (num77 < 26f)
					{
						flag8 = false;
					}
					else if (float.IsNaN(num77))
					{
						flag8 = false;
					}
					else
					{
						num77 = 26f / num77;
						num74 *= num77;
						num75 *= num77;
						vector11.X += num74;
						vector11.Y += num75;
						num74 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector11.X;
						num75 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector11.Y;
						Microsoft.Xna.Framework.Color color12 = Lighting.GetColor((int)vector11.X / 16, (int)(vector11.Y / 16f));
						Main.spriteBatch.Draw(Main.chain20Texture, new Vector2(vector11.X - Main.screenPosition.X, vector11.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain20Texture.Width, Main.chain20Texture.Height)), color12, num76 - 0.785f, new Vector2((float)Main.chain20Texture.Width * 0.5f, (float)Main.chain20Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 322)
			{
				Vector2 vector12 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num78 = mountedCenter.X - vector12.X;
				float num79 = mountedCenter.Y - vector12.Y;
				float rotation11 = (float)Math.Atan2((double)num79, (double)num78) - 1.57f;
				bool flag9 = true;
				while (flag9)
				{
					float num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
					if (num80 < 22f)
					{
						flag9 = false;
					}
					else if (float.IsNaN(num80))
					{
						flag9 = false;
					}
					else
					{
						num80 = 22f / num80;
						num78 *= num80;
						num79 *= num80;
						vector12.X += num78;
						vector12.Y += num79;
						num78 = mountedCenter.X - vector12.X;
						num79 = mountedCenter.Y - vector12.Y;
						Microsoft.Xna.Framework.Color color13 = Lighting.GetColor((int)vector12.X / 16, (int)(vector12.Y / 16f));
						Main.spriteBatch.Draw(Main.chain29Texture, new Vector2(vector12.X - Main.screenPosition.X, vector12.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain29Texture.Width, Main.chain29Texture.Height)), color13, rotation11, new Vector2((float)Main.chain29Texture.Width * 0.5f, (float)Main.chain29Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 315)
			{
				Vector2 vector13 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num81 = mountedCenter.X - vector13.X;
				float num82 = mountedCenter.Y - vector13.Y;
				float rotation12 = (float)Math.Atan2((double)num82, (double)num81) - 1.57f;
				bool flag10 = true;
				while (flag10)
				{
					float num83 = (float)Math.Sqrt((double)(num81 * num81 + num82 * num82));
					if (num83 < 50f)
					{
						flag10 = false;
					}
					else if (float.IsNaN(num83))
					{
						flag10 = false;
					}
					else
					{
						num83 = 40f / num83;
						num81 *= num83;
						num82 *= num83;
						vector13.X += num81;
						vector13.Y += num82;
						num81 = mountedCenter.X - vector13.X;
						num82 = mountedCenter.Y - vector13.Y;
						Microsoft.Xna.Framework.Color color14 = Lighting.GetColor((int)vector13.X / 16, (int)(vector13.Y / 16f));
						Main.spriteBatch.Draw(Main.chain28Texture, new Vector2(vector13.X - Main.screenPosition.X, vector13.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain28Texture.Width, Main.chain28Texture.Height)), color14, rotation12, new Vector2((float)Main.chain28Texture.Width * 0.5f, (float)Main.chain28Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 331)
			{
				Vector2 vector14 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num84 = mountedCenter.X - vector14.X;
				float num85 = mountedCenter.Y - vector14.Y;
				float rotation13 = (float)Math.Atan2((double)num85, (double)num84) - 1.57f;
				bool flag11 = true;
				while (flag11)
				{
					float num86 = (float)Math.Sqrt((double)(num84 * num84 + num85 * num85));
					if (num86 < 30f)
					{
						flag11 = false;
					}
					else if (float.IsNaN(num86))
					{
						flag11 = false;
					}
					else
					{
						num86 = 24f / num86;
						num84 *= num86;
						num85 *= num86;
						vector14.X += num84;
						vector14.Y += num85;
						num84 = mountedCenter.X - vector14.X;
						num85 = mountedCenter.Y - vector14.Y;
						Microsoft.Xna.Framework.Color color15 = Lighting.GetColor((int)vector14.X / 16, (int)(vector14.Y / 16f));
						Main.spriteBatch.Draw(Main.chain30Texture, new Vector2(vector14.X - Main.screenPosition.X, vector14.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain30Texture.Width, Main.chain30Texture.Height)), color15, rotation13, new Vector2((float)Main.chain30Texture.Width * 0.5f, (float)Main.chain30Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 332)
			{
				int num87 = 0;
				Vector2 vector15 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num88 = mountedCenter.X - vector15.X;
				float num89 = mountedCenter.Y - vector15.Y;
				float rotation14 = (float)Math.Atan2((double)num89, (double)num88) - 1.57f;
				bool flag12 = true;
				while (flag12)
				{
					float num90 = (float)Math.Sqrt((double)(num88 * num88 + num89 * num89));
					if (num90 < 30f)
					{
						flag12 = false;
					}
					else if (float.IsNaN(num90))
					{
						flag12 = false;
					}
					else
					{
						int i2 = (int)vector15.X / 16;
						int j2 = (int)vector15.Y / 16;
						if (num87 == 0)
						{
							Lighting.AddLight(i2, j2, 0f, 0.2f, 0.2f);
						}
						if (num87 == 1)
						{
							Lighting.AddLight(i2, j2, 0.1f, 0.2f, 0f);
						}
						if (num87 == 2)
						{
							Lighting.AddLight(i2, j2, 0.2f, 0.1f, 0f);
						}
						if (num87 == 3)
						{
							Lighting.AddLight(i2, j2, 0.2f, 0f, 0.2f);
						}
						num90 = 16f / num90;
						num88 *= num90;
						num89 *= num90;
						vector15.X += num88;
						vector15.Y += num89;
						num88 = mountedCenter.X - vector15.X;
						num89 = mountedCenter.Y - vector15.Y;
						Microsoft.Xna.Framework.Color color16 = Lighting.GetColor((int)vector15.X / 16, (int)(vector15.Y / 16f));
						Main.spriteBatch.Draw(Main.chain31Texture, new Vector2(vector15.X - Main.screenPosition.X, vector15.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, Main.chain31Texture.Height / 4 * num87, Main.chain31Texture.Width, Main.chain31Texture.Height / 4)), color16, rotation14, new Vector2((float)Main.chain30Texture.Width * 0.5f, (float)(Main.chain30Texture.Height / 8)), 1f, SpriteEffects.None, 0f);
						Main.spriteBatch.Draw(Main.chain32Texture, new Vector2(vector15.X - Main.screenPosition.X, vector15.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, Main.chain31Texture.Height / 4 * num87, Main.chain31Texture.Width, Main.chain31Texture.Height / 4)), new Microsoft.Xna.Framework.Color(200, 200, 200, 0), rotation14, new Vector2((float)Main.chain30Texture.Width * 0.5f, (float)(Main.chain30Texture.Height / 8)), 1f, SpriteEffects.None, 0f);
						num87++;
						if (num87 > 3)
						{
							num87 = 0;
						}
					}
				}
			}
			else if (projectile.type == 372 || projectile.type == 383 || projectile.type == 396 || projectile.type == 403 || projectile.type == 404 || projectile.type == 446 || (projectile.type >= 486 && projectile.type <= 489) || (projectile.type >= 646 && projectile.type <= 649) || projectile.type == 652)
			{
				Texture2D texture2D = null;
				Microsoft.Xna.Framework.Color transparent = Microsoft.Xna.Framework.Color.Transparent;
				Texture2D texture2D2 = Main.chain33Texture;
				if (projectile.type == 383)
				{
					texture2D2 = Main.chain34Texture;
				}
				if (projectile.type == 396)
				{
					texture2D2 = Main.chain35Texture;
				}
				if (projectile.type == 403)
				{
					texture2D2 = Main.chain36Texture;
				}
				if (projectile.type == 404)
				{
					texture2D2 = Main.chain37Texture;
				}
				if (projectile.type == 446)
				{
					texture2D2 = Main.extraTexture[3];
				}
				if (projectile.type >= 486 && projectile.type <= 489)
				{
					texture2D2 = Main.chainsTexture[projectile.type - 486];
				}
				if (projectile.type >= 646 && projectile.type <= 649)
				{
					texture2D2 = Main.chainsTexture[projectile.type - 646 + 8];
					texture2D = Main.chainsTexture[projectile.type - 646 + 12];
					transparent = new Microsoft.Xna.Framework.Color(255, 255, 255, 127);
				}
				if (projectile.type == 652)
				{
					texture2D2 = Main.chainsTexture[16];
				}
				Vector2 vector16 = projectile.Center;
				Microsoft.Xna.Framework.Rectangle? sourceRectangle = null;
				Vector2 origin = new Vector2((float)texture2D2.Width * 0.5f, (float)texture2D2.Height * 0.5f);
				float num91 = (float)texture2D2.Height;
				float num92 = 0f;
				if (projectile.type == 446)
				{
					int num93 = 7;
					int num94 = (int)projectile.localAI[0] / num93;
					sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, texture2D2.Height / 4 * num94, texture2D2.Width, texture2D2.Height / 4));
					origin.Y /= 4f;
					num91 /= 4f;
				}
				int type2 = projectile.type;
				if (type2 != 383)
				{
					if (type2 != 446)
					{
						switch (type2)
						{
						case 487:
							num92 = 8f;
							break;
						case 489:
							num92 = 10f;
							break;
						}
					}
					else
					{
						num92 = 20f;
					}
				}
				else
				{
					num92 = 14f;
				}
				if (num92 != 0f)
				{
					float num95 = -1.57f;
					Vector2 value2 = new Vector2((float)Math.Cos((double)(projectile.rotation + num95)), (float)Math.Sin((double)(projectile.rotation + num95)));
					vector16 -= value2 * num92;
					value2 = mountedCenter - vector16;
					value2.Normalize();
					vector16 -= value2 * num91 / 2f;
				}
				Vector2 vector17 = mountedCenter - vector16;
				float rotation15 = (float)Math.Atan2((double)vector17.Y, (double)vector17.X) - 1.57f;
				bool flag13 = true;
				if (float.IsNaN(vector16.X) && float.IsNaN(vector16.Y))
				{
					flag13 = false;
				}
				if (float.IsNaN(vector17.X) && float.IsNaN(vector17.Y))
				{
					flag13 = false;
				}
				while (flag13)
				{
					float num96 = vector17.Length();
					if (num96 < num91 + 1f)
					{
						flag13 = false;
					}
					else
					{
						Vector2 value3 = vector17;
						value3.Normalize();
						vector16 += value3 * num91;
						vector17 = mountedCenter - vector16;
						Microsoft.Xna.Framework.Color color17 = Lighting.GetColor((int)vector16.X / 16, (int)(vector16.Y / 16f));
						if (projectile.type == 396)
						{
							color17 *= (float)(255 - projectile.alpha) / 255f;
						}
						if (projectile.type == 446)
						{
							color17 = projectile.GetAlpha(color17);
						}
						if (projectile.type == 488)
						{
							Lighting.AddLight(vector16, 0.2f, 0f, 0.175f);
							color17 = new Microsoft.Xna.Framework.Color(255, 255, 255, 255);
						}
						if (projectile.type >= 646 && projectile.type <= 649)
						{
							color17 = projectile.GetAlpha(color17);
						}
						Main.spriteBatch.Draw(texture2D2, vector16 - Main.screenPosition, sourceRectangle, color17, rotation15, origin, 1f, SpriteEffects.None, 0f);
						if (texture2D != null)
						{
							Main.spriteBatch.Draw(texture2D, vector16 - Main.screenPosition, sourceRectangle, transparent, rotation15, origin, 1f, SpriteEffects.None, 0f);
						}
					}
				}
			}
			else if (projectile.aiStyle == 7)
			{
				Vector2 vector18 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num97 = mountedCenter.X - vector18.X;
				float num98 = mountedCenter.Y - vector18.Y;
				float rotation16 = (float)Math.Atan2((double)num98, (double)num97) - 1.57f;
				bool flag14 = true;
				while (flag14)
				{
					float num99 = (float)Math.Sqrt((double)(num97 * num97 + num98 * num98));
					if (num99 < 25f)
					{
						flag14 = false;
					}
					else if (float.IsNaN(num99))
					{
						flag14 = false;
					}
					else
					{
						num99 = 12f / num99;
						num97 *= num99;
						num98 *= num99;
						vector18.X += num97;
						vector18.Y += num98;
						num97 = mountedCenter.X - vector18.X;
						num98 = mountedCenter.Y - vector18.Y;
						Microsoft.Xna.Framework.Color color18 = Lighting.GetColor((int)vector18.X / 16, (int)(vector18.Y / 16f));
						Main.spriteBatch.Draw(Main.chainTexture, new Vector2(vector18.X - Main.screenPosition.X, vector18.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chainTexture.Width, Main.chainTexture.Height)), color18, rotation16, new Vector2((float)Main.chainTexture.Width * 0.5f, (float)Main.chainTexture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 262)
			{
				float num100 = projectile.Center.X;
				float num101 = projectile.Center.Y;
				float num102 = projectile.velocity.X;
				float num103 = projectile.velocity.Y;
				float num104 = (float)Math.Sqrt((double)(num102 * num102 + num103 * num103));
				num104 = 4f / num104;
				if (projectile.ai[0] == 0f)
				{
					num100 -= projectile.velocity.X * num104;
					num101 -= projectile.velocity.Y * num104;
				}
				else
				{
					num100 += projectile.velocity.X * num104;
					num101 += projectile.velocity.Y * num104;
				}
				Vector2 vector19 = new Vector2(num100, num101);
				num102 = mountedCenter.X - vector19.X;
				num103 = mountedCenter.Y - vector19.Y;
				float rotation17 = (float)Math.Atan2((double)num103, (double)num102) - 1.57f;
				if (projectile.alpha == 0)
				{
					int num105 = -1;
					if (projectile.position.X + (float)(projectile.width / 2) < mountedCenter.X)
					{
						num105 = 1;
					}
					if (Main.player[projectile.owner].direction == 1)
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num103 * (float)num105), (double)(num102 * (float)num105));
					}
					else
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num103 * (float)num105), (double)(num102 * (float)num105));
					}
				}
				bool flag15 = true;
				while (flag15)
				{
					float num106 = (float)Math.Sqrt((double)(num102 * num102 + num103 * num103));
					if (num106 < 25f)
					{
						flag15 = false;
					}
					else if (float.IsNaN(num106))
					{
						flag15 = false;
					}
					else
					{
						num106 = 12f / num106;
						num102 *= num106;
						num103 *= num106;
						vector19.X += num102;
						vector19.Y += num103;
						num102 = mountedCenter.X - vector19.X;
						num103 = mountedCenter.Y - vector19.Y;
						Microsoft.Xna.Framework.Color color19 = Lighting.GetColor((int)vector19.X / 16, (int)(vector19.Y / 16f));
						Main.spriteBatch.Draw(Main.chain22Texture, new Vector2(vector19.X - Main.screenPosition.X, vector19.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain22Texture.Width, Main.chain22Texture.Height)), color19, rotation17, new Vector2((float)Main.chain22Texture.Width * 0.5f, (float)Main.chain22Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 273)
			{
				float num107 = projectile.Center.X;
				float num108 = projectile.Center.Y;
				float num109 = projectile.velocity.X;
				float num110 = projectile.velocity.Y;
				float num111 = (float)Math.Sqrt((double)(num109 * num109 + num110 * num110));
				num111 = 4f / num111;
				if (projectile.ai[0] == 0f)
				{
					num107 -= projectile.velocity.X * num111;
					num108 -= projectile.velocity.Y * num111;
				}
				else
				{
					num107 += projectile.velocity.X * num111;
					num108 += projectile.velocity.Y * num111;
				}
				Vector2 vector20 = new Vector2(num107, num108);
				num109 = mountedCenter.X - vector20.X;
				num110 = mountedCenter.Y - vector20.Y;
				float rotation18 = (float)Math.Atan2((double)num110, (double)num109) - 1.57f;
				if (projectile.alpha == 0)
				{
					int num112 = -1;
					if (projectile.position.X + (float)(projectile.width / 2) < mountedCenter.X)
					{
						num112 = 1;
					}
					if (Main.player[projectile.owner].direction == 1)
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num110 * (float)num112), (double)(num109 * (float)num112));
					}
					else
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num110 * (float)num112), (double)(num109 * (float)num112));
					}
				}
				bool flag16 = true;
				while (flag16)
				{
					float num113 = (float)Math.Sqrt((double)(num109 * num109 + num110 * num110));
					if (num113 < 25f)
					{
						flag16 = false;
					}
					else if (float.IsNaN(num113))
					{
						flag16 = false;
					}
					else
					{
						num113 = 12f / num113;
						num109 *= num113;
						num110 *= num113;
						vector20.X += num109;
						vector20.Y += num110;
						num109 = mountedCenter.X - vector20.X;
						num110 = mountedCenter.Y - vector20.Y;
						Microsoft.Xna.Framework.Color color20 = Lighting.GetColor((int)vector20.X / 16, (int)(vector20.Y / 16f));
						Main.spriteBatch.Draw(Main.chain23Texture, new Vector2(vector20.X - Main.screenPosition.X, vector20.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain23Texture.Width, Main.chain23Texture.Height)), color20, rotation18, new Vector2((float)Main.chain23Texture.Width * 0.5f, (float)Main.chain23Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 481)
			{
				float num114 = projectile.Center.X;
				float num115 = projectile.Center.Y;
				float num116 = projectile.velocity.X;
				float num117 = projectile.velocity.Y;
				float num118 = (float)Math.Sqrt((double)(num116 * num116 + num117 * num117));
				num118 = 4f / num118;
				if (projectile.ai[0] == 0f)
				{
					num114 -= projectile.velocity.X * num118;
					num115 -= projectile.velocity.Y * num118;
				}
				else
				{
					num114 += projectile.velocity.X * num118;
					num115 += projectile.velocity.Y * num118;
				}
				Vector2 vector21 = new Vector2(num114, num115);
				num116 = mountedCenter.X - vector21.X;
				num117 = mountedCenter.Y - vector21.Y;
				float rotation19 = (float)Math.Atan2((double)num117, (double)num116) - 1.57f;
				if (projectile.alpha == 0)
				{
					int num119 = -1;
					if (projectile.position.X + (float)(projectile.width / 2) < mountedCenter.X)
					{
						num119 = 1;
					}
					if (Main.player[projectile.owner].direction == 1)
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num117 * (float)num119), (double)(num116 * (float)num119));
					}
					else
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num117 * (float)num119), (double)(num116 * (float)num119));
					}
				}
				bool flag17 = true;
				while (flag17)
				{
					float num120 = 0.85f;
					float num121 = (float)Math.Sqrt((double)(num116 * num116 + num117 * num117));
					float num122 = num121;
					if ((double)num121 < (double)Main.chain40Texture.Height * 1.5)
					{
						flag17 = false;
					}
					else if (float.IsNaN(num121))
					{
						flag17 = false;
					}
					else
					{
						num121 = (float)Main.chain40Texture.Height * num120 / num121;
						num116 *= num121;
						num117 *= num121;
						vector21.X += num116;
						vector21.Y += num117;
						num116 = mountedCenter.X - vector21.X;
						num117 = mountedCenter.Y - vector21.Y;
						if (num122 > (float)(Main.chain40Texture.Height * 2))
						{
							for (int num123 = 0; num123 < 2; num123++)
							{
								float num124 = 0.75f;
								float num125;
								if (num123 == 0)
								{
									num125 = Math.Abs(Main.player[projectile.owner].velocity.X);
								}
								else
								{
									num125 = Math.Abs(Main.player[projectile.owner].velocity.Y);
								}
								if (num125 > 10f)
								{
									num125 = 10f;
								}
								num125 /= 10f;
								num124 *= num125;
								num125 = num122 / 80f;
								if (num125 > 1f)
								{
									num125 = 1f;
								}
								num124 *= num125;
								if (num124 < 0f)
								{
									num124 = 0f;
								}
								if (!float.IsNaN(num124))
								{
									if (num123 == 0)
									{
										if (Main.player[projectile.owner].velocity.X < 0f && projectile.Center.X < mountedCenter.X)
										{
											num117 *= 1f - num124;
										}
										if (Main.player[projectile.owner].velocity.X > 0f && projectile.Center.X > mountedCenter.X)
										{
											num117 *= 1f - num124;
										}
									}
									else
									{
										if (Main.player[projectile.owner].velocity.Y < 0f && projectile.Center.Y < mountedCenter.Y)
										{
											num116 *= 1f - num124;
										}
										if (Main.player[projectile.owner].velocity.Y > 0f && projectile.Center.Y > mountedCenter.Y)
										{
											num116 *= 1f - num124;
										}
									}
								}
							}
						}
						Microsoft.Xna.Framework.Color color21 = Lighting.GetColor((int)vector21.X / 16, (int)(vector21.Y / 16f));
						Main.spriteBatch.Draw(Main.chain40Texture, new Vector2(vector21.X - Main.screenPosition.X, vector21.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain40Texture.Width, Main.chain40Texture.Height)), color21, rotation19, new Vector2((float)Main.chain40Texture.Width * 0.5f, (float)Main.chain40Texture.Height * 0.5f), num120, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 271)
			{
				float num126 = projectile.Center.X;
				float num127 = projectile.Center.Y;
				float num128 = projectile.velocity.X;
				float num129 = projectile.velocity.Y;
				float num130 = (float)Math.Sqrt((double)(num128 * num128 + num129 * num129));
				num130 = 4f / num130;
				if (projectile.ai[0] == 0f)
				{
					num126 -= projectile.velocity.X * num130;
					num127 -= projectile.velocity.Y * num130;
				}
				else
				{
					num126 += projectile.velocity.X * num130;
					num127 += projectile.velocity.Y * num130;
				}
				Vector2 vector22 = new Vector2(num126, num127);
				num128 = mountedCenter.X - vector22.X;
				num129 = mountedCenter.Y - vector22.Y;
				float rotation20 = (float)Math.Atan2((double)num129, (double)num128) - 1.57f;
				if (projectile.alpha == 0)
				{
					int num131 = -1;
					if (projectile.position.X + (float)(projectile.width / 2) < mountedCenter.X)
					{
						num131 = 1;
					}
					if (Main.player[projectile.owner].direction == 1)
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num129 * (float)num131), (double)(num128 * (float)num131));
					}
					else
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num129 * (float)num131), (double)(num128 * (float)num131));
					}
				}
				bool flag18 = true;
				while (flag18)
				{
					float num132 = (float)Math.Sqrt((double)(num128 * num128 + num129 * num129));
					if (num132 < 25f)
					{
						flag18 = false;
					}
					else if (float.IsNaN(num132))
					{
						flag18 = false;
					}
					else
					{
						num132 = 12f / num132;
						num128 *= num132;
						num129 *= num132;
						vector22.X += num128;
						vector22.Y += num129;
						num128 = mountedCenter.X - vector22.X;
						num129 = mountedCenter.Y - vector22.Y;
						Microsoft.Xna.Framework.Color color22 = Lighting.GetColor((int)vector22.X / 16, (int)(vector22.Y / 16f));
						Main.spriteBatch.Draw(Main.chain18Texture, new Vector2(vector22.X - Main.screenPosition.X, vector22.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain18Texture.Width, Main.chain18Texture.Height)), color22, rotation20, new Vector2((float)Main.chain18Texture.Width * 0.5f, (float)Main.chain18Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.aiStyle == 13)
			{
				float num133 = projectile.position.X + 8f;
				float num134 = projectile.position.Y + 2f;
				float num135 = projectile.velocity.X;
				float num136 = projectile.velocity.Y;
				if (num135 == 0f && num136 == 0f)
				{
					num136 = 0.0001f;
				}
				float num137 = (float)Math.Sqrt((double)(num135 * num135 + num136 * num136));
				num137 = 20f / num137;
				if (projectile.ai[0] == 0f)
				{
					num133 -= projectile.velocity.X * num137;
					num134 -= projectile.velocity.Y * num137;
				}
				else
				{
					num133 += projectile.velocity.X * num137;
					num134 += projectile.velocity.Y * num137;
				}
				Vector2 vector23 = new Vector2(num133, num134);
				num135 = mountedCenter.X - vector23.X;
				num136 = mountedCenter.Y - vector23.Y;
				float rotation21 = (float)Math.Atan2((double)num136, (double)num135) - 1.57f;
				if (projectile.alpha == 0)
				{
					int num138 = -1;
					if (projectile.position.X + (float)(projectile.width / 2) < mountedCenter.X)
					{
						num138 = 1;
					}
					if (Main.player[projectile.owner].direction == 1)
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num136 * (float)num138), (double)(num135 * (float)num138));
					}
					else
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num136 * (float)num138), (double)(num135 * (float)num138));
					}
				}
				bool flag19 = true;
				while (flag19)
				{
					float num139 = (float)Math.Sqrt((double)(num135 * num135 + num136 * num136));
					if (num139 < 25f)
					{
						flag19 = false;
					}
					else if (float.IsNaN(num139))
					{
						flag19 = false;
					}
					else
					{
						num139 = 12f / num139;
						num135 *= num139;
						num136 *= num139;
						vector23.X += num135;
						vector23.Y += num136;
						num135 = mountedCenter.X - vector23.X;
						num136 = mountedCenter.Y - vector23.Y;
						Microsoft.Xna.Framework.Color color23 = Lighting.GetColor((int)vector23.X / 16, (int)(vector23.Y / 16f));
						Main.spriteBatch.Draw(Main.chainTexture, new Vector2(vector23.X - Main.screenPosition.X, vector23.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chainTexture.Width, Main.chainTexture.Height)), color23, rotation21, new Vector2((float)Main.chainTexture.Width * 0.5f, (float)Main.chainTexture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			else if (projectile.type == 190)
			{
				float x = projectile.position.X + (float)(projectile.width / 2);
				float y = projectile.position.Y + (float)(projectile.height / 2);
				float num140 = projectile.velocity.X;
				float num141 = projectile.velocity.Y;
				Math.Sqrt((double)(num140 * num140 + num141 * num141));
				Vector2 vector24 = new Vector2(x, y);
				num140 = mountedCenter.X - vector24.X;
				num141 = mountedCenter.Y + Main.player[projectile.owner].gfxOffY - vector24.Y;
				Math.Atan2((double)num141, (double)num140);
				if (projectile.alpha == 0)
				{
					int num142 = -1;
					if (projectile.position.X + (float)(projectile.width / 2) < mountedCenter.X)
					{
						num142 = 1;
					}
					if (Main.player[projectile.owner].direction == 1)
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num141 * (float)num142), (double)(num140 * (float)num142));
					}
					else
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num141 * (float)num142), (double)(num140 * (float)num142));
					}
				}
			}
			else if (projectile.aiStyle == 15)
			{
				Vector2 vector25 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num143 = mountedCenter.X - vector25.X;
				float num144 = mountedCenter.Y - vector25.Y;
				float rotation22 = (float)Math.Atan2((double)num144, (double)num143) - 1.57f;
				if (projectile.alpha == 0)
				{
					int num145 = -1;
					if (projectile.position.X + (float)(projectile.width / 2) < mountedCenter.X)
					{
						num145 = 1;
					}
					if (Main.player[projectile.owner].direction == 1)
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num144 * (float)num145), (double)(num143 * (float)num145));
					}
					else
					{
						Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num144 * (float)num145), (double)(num143 * (float)num145));
					}
				}
				bool flag20 = true;
				while (flag20)
				{
					float num146 = (float)Math.Sqrt((double)(num143 * num143 + num144 * num144));
					if (num146 < 25f)
					{
						flag20 = false;
					}
					else if (float.IsNaN(num146))
					{
						flag20 = false;
					}
					else
					{
						if (projectile.type == 154 || projectile.type == 247)
						{
							num146 = 18f / num146;
						}
						else
						{
							num146 = 12f / num146;
						}
						num143 *= num146;
						num144 *= num146;
						vector25.X += num143;
						vector25.Y += num144;
						num143 = mountedCenter.X - vector25.X;
						num144 = mountedCenter.Y - vector25.Y;
						Microsoft.Xna.Framework.Color color24 = Lighting.GetColor((int)vector25.X / 16, (int)(vector25.Y / 16f));
						if (projectile.type == 25)
						{
							Main.spriteBatch.Draw(Main.chain2Texture, new Vector2(vector25.X - Main.screenPosition.X, vector25.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain2Texture.Width, Main.chain2Texture.Height)), color24, rotation22, new Vector2((float)Main.chain2Texture.Width * 0.5f, (float)Main.chain2Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
						}
						else if (projectile.type == 35)
						{
							Main.spriteBatch.Draw(Main.chain6Texture, new Vector2(vector25.X - Main.screenPosition.X, vector25.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain6Texture.Width, Main.chain6Texture.Height)), color24, rotation22, new Vector2((float)Main.chain6Texture.Width * 0.5f, (float)Main.chain6Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
						}
						else if (projectile.type == 247)
						{
							Main.spriteBatch.Draw(Main.chain19Texture, new Vector2(vector25.X - Main.screenPosition.X, vector25.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain19Texture.Width, Main.chain19Texture.Height)), color24, rotation22, new Vector2((float)Main.chain19Texture.Width * 0.5f, (float)Main.chain19Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
						}
						else if (projectile.type == 63)
						{
							Main.spriteBatch.Draw(Main.chain7Texture, new Vector2(vector25.X - Main.screenPosition.X, vector25.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain7Texture.Width, Main.chain7Texture.Height)), color24, rotation22, new Vector2((float)Main.chain7Texture.Width * 0.5f, (float)Main.chain7Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
						}
						else if (projectile.type == 154)
						{
							Main.spriteBatch.Draw(Main.chain13Texture, new Vector2(vector25.X - Main.screenPosition.X, vector25.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain13Texture.Width, Main.chain13Texture.Height)), color24, rotation22, new Vector2((float)Main.chain13Texture.Width * 0.5f, (float)Main.chain13Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
						}
						else
						{
							Main.spriteBatch.Draw(Main.chain3Texture, new Vector2(vector25.X - Main.screenPosition.X, vector25.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain3Texture.Width, Main.chain3Texture.Height)), color24, rotation22, new Vector2((float)Main.chain3Texture.Width * 0.5f, (float)Main.chain3Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
						}
					}
				}
			}
			Microsoft.Xna.Framework.Color color25 = Lighting.GetColor((int)((double)projectile.position.X + (double)projectile.width * 0.5) / 16, (int)(((double)projectile.position.Y + (double)projectile.height * 0.5) / 16.0));
			if (projectile.hide && !ProjectileID.Sets.DontAttachHideToAlpha[projectile.type])
			{
				color25 = Lighting.GetColor((int)mountedCenter.X / 16, (int)(mountedCenter.Y / 16f));
			}
			if (projectile.type == 14)
			{
				color25 = Microsoft.Xna.Framework.Color.White;
			}
			int num147 = 0;
			int num148 = 0;
			if (projectile.type == 175)
			{
				num147 = 10;
			}
			if (projectile.type == 392)
			{
				num147 = -2;
			}
			if (projectile.type == 499)
			{
				num147 = 12;
			}
			if (projectile.bobber)
			{
				num147 = 8;
			}
			if (projectile.type == 519)
			{
				num147 = 6;
				num148 -= 6;
			}
			if (projectile.type == 520)
			{
				num147 = 12;
			}
			if (projectile.type == 492)
			{
				num148 -= 4;
				num147 += 5;
			}
			if (projectile.type == 498)
			{
				num147 = 6;
			}
			if (projectile.type == 489)
			{
				num147 = -2;
			}
			if (projectile.type == 486)
			{
				num147 = -6;
			}
			if (projectile.type == 525)
			{
				num147 = 5;
			}
			if (projectile.type == 488)
			{
				num148 -= 8;
			}
			if (projectile.type == 373)
			{
				num148 = -10;
				num147 = 6;
			}
			if (projectile.type == 375)
			{
				num148 = -11;
				num147 = 12;
			}
			if (projectile.type == 423)
			{
				num148 = -5;
			}
			if (projectile.type == 346)
			{
				num147 = 4;
			}
			if (projectile.type == 331)
			{
				num148 = -4;
			}
			if (projectile.type == 254)
			{
				num147 = 3;
			}
			if (projectile.type == 273)
			{
				num148 = 2;
			}
			if (projectile.type == 335)
			{
				num147 = 6;
			}
			if (projectile.type == 162)
			{
				num147 = 1;
				num148 = 1;
			}
			if (projectile.type == 377)
			{
				num147 = -6;
			}
			if (projectile.type == 353)
			{
				num147 = 36;
				num148 = -12;
			}
			if (projectile.type == 324)
			{
				num147 = 22;
				num148 = -6;
			}
			if (projectile.type == 266)
			{
				num147 = 10;
				num148 = -10;
			}
			if (projectile.type == 319)
			{
				num147 = 10;
				num148 = -12;
			}
			if (projectile.type == 315)
			{
				num147 = -13;
				num148 = -6;
			}
			if (projectile.type == 313 && projectile.height != 54)
			{
				num148 = -12;
				num147 = 20;
			}
			if (projectile.type == 314)
			{
				num148 = -8;
				num147 = 0;
			}
			if (projectile.type == 269)
			{
				num147 = 18;
				num148 = -14;
			}
			if (projectile.type == 268)
			{
				num147 = 22;
				num148 = -2;
			}
			if (projectile.type == 18)
			{
				num147 = 3;
				num148 = 3;
			}
			if (projectile.type == 16)
			{
				num147 = 6;
			}
			if (projectile.type == 17 || projectile.type == 31)
			{
				num147 = 2;
			}
			if (projectile.type == 25 || projectile.type == 26 || projectile.type == 35 || projectile.type == 63 || projectile.type == 154)
			{
				num147 = 6;
				num148 -= 6;
			}
			if (projectile.type == 28 || projectile.type == 37 || projectile.type == 75)
			{
				num147 = 8;
			}
			if (projectile.type == 29 || projectile.type == 470 || projectile.type == 637)
			{
				num147 = 11;
			}
			if (projectile.type == 43)
			{
				num147 = 4;
			}
			if (projectile.type == 208)
			{
				num147 = 2;
				num148 -= 12;
			}
			if (projectile.type == 209)
			{
				num147 = 4;
				num148 -= 8;
			}
			if (projectile.type == 210)
			{
				num147 = 2;
				num148 -= 22;
			}
			if (projectile.type == 251)
			{
				num147 = 18;
				num148 -= 10;
			}
			if (projectile.type == 163 || projectile.type == 310)
			{
				num147 = 10;
			}
			if (projectile.type == 69 || projectile.type == 70)
			{
				num147 = 4;
				num148 = 4;
			}
			float num149 = (float)(Main.projectileTexture[projectile.type].Width - projectile.width) * 0.5f + (float)projectile.width * 0.5f;
			if (projectile.type == 50 || projectile.type == 53 || projectile.type == 515)
			{
				num148 = -8;
			}
			if (projectile.type == 473)
			{
				num148 = -6;
				num147 = 2;
			}
			if (projectile.type == 72 || projectile.type == 86 || projectile.type == 87)
			{
				num148 = -16;
				num147 = 8;
			}
			if (projectile.type == 74)
			{
				num148 = -6;
			}
			if (projectile.type == 99)
			{
				num147 = 1;
			}
			if (projectile.type == 655)
			{
				num147 = 1;
			}
			if (projectile.type == 111)
			{
				num147 = 18;
				num148 = -16;
			}
			if (projectile.type == 334)
			{
				num148 = -18;
				num147 = 8;
			}
			if (projectile.type == 200)
			{
				num147 = 12;
				num148 = -12;
			}
			if (projectile.type == 211)
			{
				num147 = 14;
				num148 = 0;
			}
			if (projectile.type == 236)
			{
				num147 = 30;
				num148 = -14;
			}
			if (projectile.type >= 191 && projectile.type <= 194)
			{
				num147 = 26;
				if (projectile.direction == 1)
				{
					num148 = -10;
				}
				else
				{
					num148 = -22;
				}
			}
			if (projectile.type >= 390 && projectile.type <= 392)
			{
				num148 = 4 * projectile.direction;
			}
			if (projectile.type == 112)
			{
				num147 = 12;
			}
			//int arg_536F_0 = projectile.type;
			if (projectile.type == 517)
			{
				num147 = 6;
			}
			if (projectile.type == 516)
			{
				num147 = 6;
			}
			if (projectile.type == 127)
			{
				num147 = 8;
			}
			if (projectile.type == 155)
			{
				num147 = 3;
				num148 = 3;
			}
			if (projectile.type == 397)
			{
				num149 -= 1f;
				num147 = -2;
				num148 = -2;
			}
			if (projectile.type == 398)
			{
				num147 = 8;
			}
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			if (projectile.type == 221)
			{
				for (int num150 = 1; num150 < 10; num150++)
				{
					float num151 = projectile.velocity.X * (float)num150 * 0.5f;
					float num152 = projectile.velocity.Y * (float)num150 * 0.5f;
					Microsoft.Xna.Framework.Color alpha = projectile.GetAlpha(color25);
					float num153 = 0f;
					if (num150 == 1)
					{
						num153 = 0.9f;
					}
					if (num150 == 2)
					{
						num153 = 0.8f;
					}
					if (num150 == 3)
					{
						num153 = 0.7f;
					}
					if (num150 == 4)
					{
						num153 = 0.6f;
					}
					if (num150 == 5)
					{
						num153 = 0.5f;
					}
					if (num150 == 6)
					{
						num153 = 0.4f;
					}
					if (num150 == 7)
					{
						num153 = 0.3f;
					}
					if (num150 == 8)
					{
						num153 = 0.2f;
					}
					if (num150 == 9)
					{
						num153 = 0.1f;
					}
					alpha.R = (byte)((float)alpha.R * num153);
					alpha.G = (byte)((float)alpha.G * num153);
					alpha.B = (byte)((float)alpha.B * num153);
					alpha.A = (byte)((float)alpha.A * num153);
					int num154 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
					int y2 = num154 * projectile.frame;
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148 - num151, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY - num152), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, Main.projectileTexture[projectile.type].Width, num154)), alpha, projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
				}
			}
			if (projectile.type == 408 || projectile.type == 435 || projectile.type == 436 || projectile.type == 438 || projectile.type == 452 || projectile.type == 454 || projectile.type == 459 || projectile.type == 462 || projectile.type == 503 || projectile.type == 532 || projectile.type == 533 || projectile.type == 573 || projectile.type == 582 || projectile.type == 585 || projectile.type == 592 || projectile.type == 601 || projectile.type == 636 || projectile.type == 638 || projectile.type == 640 || projectile.type == 639 || projectile.type == 424 || projectile.type == 425 || projectile.type == 426 || projectile.type == 660 || projectile.type == 661)
			{
				Texture2D texture2D3 = Main.projectileTexture[projectile.type];
				int num155 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				int y3 = num155 * projectile.frame;
				Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(0, y3, texture2D3.Width, num155);
				Vector2 origin2 = rectangle.Size() / 2f;
				if (projectile.type == 503)
				{
					origin2.Y = 70f;
				}
				if (projectile.type == 438)
				{
				}
				if (projectile.type == 452)
				{
				}
				if (projectile.type == 408)
				{
				}
				if (projectile.type == 636)
				{
					origin2.Y = 10f;
				}
				if (projectile.type == 638)
				{
					origin2.Y = 2f;
				}
				if (projectile.type == 640 || projectile.type == 639)
				{
					origin2.Y = 5f;
				}
				int num156 = 8;
				int num157 = 2;
				float value4 = 1f;
				float num158 = 0f;
				if (projectile.type == 503)
				{
					num156 = 9;
					num157 = 3;
					value4 = 0.5f;
				}
				else if (projectile.type == 582)
				{
					num156 = 10;
					num157 = 2;
					value4 = 0.7f;
					num158 = 0.2f;
				}
				else if (projectile.type == 638)
				{
					num156 = 5;
					num157 = 1;
					value4 = 1f;
				}
				else if (projectile.type == 660)
				{
					num156 = 3;
					num157 = 1;
					value4 = 8f;
					rectangle = new Microsoft.Xna.Framework.Rectangle(38 * projectile.frame, 0, 38, 38);
					origin2 = rectangle.Size() / 2f;
				}
				else if (projectile.type == 639)
				{
					num156 = 10;
					num157 = 1;
					value4 = 1f;
				}
				else if (projectile.type == 640)
				{
					num156 = 20;
					num157 = 1;
					value4 = 1f;
				}
				else if (projectile.type == 436)
				{
					num157 = 2;
					value4 = 0.5f;
				}
				else if (projectile.type == 424 || projectile.type == 425 || projectile.type == 426)
				{
					num156 = 10;
					num157 = 2;
					value4 = 0.6f;
				}
				else if (projectile.type == 438)
				{
					num156 = 10;
					num157 = 2;
					value4 = 1f;
				}
				else if (projectile.type == 452)
				{
					num156 = 10;
					num157 = 3;
					value4 = 0.5f;
				}
				else if (projectile.type == 454)
				{
					num156 = 5;
					num157 = 1;
					value4 = 0.2f;
				}
				else if (projectile.type == 462)
				{
					num156 = 7;
					num157 = 1;
					value4 = 0.2f;
				}
				else if (projectile.type == 661)
				{
					num156 = 0;
					num157 = 1;
					value4 = 0.5f;
				}
				else if (projectile.type == 585)
				{
					num156 = 7;
					num157 = 1;
					value4 = 0.2f;
				}
				else if (projectile.type == 459)
				{
					num156 = (int)(projectile.scale * 8f);
					num157 = num156 / 4;
					if (num157 < 1)
					{
						num157 = 1;
					}
					value4 = 0.3f;
				}
				else if (projectile.type == 532)
				{
					num156 = 10;
					num157 = 1;
					value4 = 0.7f;
					num158 = 0.2f;
				}
				else if (projectile.type == 592)
				{
					num156 = 10;
					num157 = 2;
					value4 = 1f;
				}
				else if (projectile.type == 601)
				{
					num156 = 8;
					num157 = 1;
					value4 = 0.3f;
				}
				else if (projectile.type == 636)
				{
					num156 = 20;
					num157 = 3;
					value4 = 0.5f;
				}
				else if (projectile.type == 533)
				{
					if (projectile.ai[0] >= 6f && projectile.ai[0] <= 8f)
					{
						num156 = ((projectile.ai[0] == 6f) ? 8 : 4);
						num157 = 1;
						if (projectile.ai[0] != 7f)
						{
							num158 = 0.2f;
						}
					}
					else
					{
						num157 = (num156 = 0);
					}
				}
				for (int num159 = 1; num159 < num156; num159 += num157)
				{
					Microsoft.Xna.Framework.Color color26 = color25;
					if (projectile.type == 408 || projectile.type == 435)
					{
						color26 = Microsoft.Xna.Framework.Color.Lerp(color26, Microsoft.Xna.Framework.Color.Blue, 0.5f);
					}
					else if (projectile.type == 436)
					{
						color26 = Microsoft.Xna.Framework.Color.Lerp(color26, Microsoft.Xna.Framework.Color.LimeGreen, 0.5f);
					}
					else if (projectile.type >= 424 && projectile.type <= 426)
					{
						color26 = Microsoft.Xna.Framework.Color.Lerp(color26, Microsoft.Xna.Framework.Color.Red, 0.5f);
					}
					else if (projectile.type == 640 || projectile.type == 639)
					{
						color26.A = 127;
					}
					color26 = projectile.GetAlpha(color26);
					if (projectile.type == 438)
					{
						color26.G /= (byte)num159;
						color26.B /= (byte)num159;
					}
					else if (projectile.type == 592)
					{
						color26.R /= (byte)num159;
						color26.G /= (byte)num159;
					}
					else if (projectile.type == 640)
					{
						color26.R /= (byte)num159;
						color26.A /= (byte)num159;
					}
					else if (projectile.type >= 424 && projectile.type <= 426)
					{
						color26.B /= (byte)num159;
						color26.G /= (byte)num159;
						color26.A /= (byte)num159;
					}
					color26 *= (float)(num156 - num159) / ((float)ProjectileID.Sets.TrailCacheLength[projectile.type] * 1.5f);
					Vector2 value5 = projectile.oldPos[num159];
					float num160 = projectile.rotation;
					SpriteEffects effects = spriteEffects;
					if (ProjectileID.Sets.TrailingMode[projectile.type] == 2)
					{
						num160 = projectile.oldRot[num159];
						effects = ((projectile.oldSpriteDirection[num159] == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
					}
					Main.spriteBatch.Draw(texture2D3, value5 + projectile.Size / 2f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, num160 + projectile.rotation * num158 * (float)(num159 - 1) * (float)(-(float)spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt()), origin2, MathHelper.Lerp(projectile.scale, value4, (float)num159 / 15f), effects, 0f);
				}
				if (projectile.type == 661)
				{
					Microsoft.Xna.Framework.Color color27 = new Microsoft.Xna.Framework.Color(120, 40, 222, 120);
					for (int num161 = 0; num161 < 4; num161++)
					{
						Main.spriteBatch.Draw(Main.extraTexture[75], projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY) + projectile.rotation.ToRotationVector2().RotatedBy((double)(1.57079637f * (float)num161), default(Vector2)) * 4f, new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, projectile.rotation, origin2, projectile.scale, spriteEffects, 0f);
					}
				}
				Microsoft.Xna.Framework.Color color28 = projectile.GetAlpha(color25);
				if (projectile.type == 640)
				{
					color28 = Microsoft.Xna.Framework.Color.Transparent;
				}
				Main.spriteBatch.Draw(texture2D3, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color28, projectile.rotation, origin2, projectile.scale, spriteEffects, 0f);
				if (projectile.type == 503)
				{
					Main.spriteBatch.Draw(Main.extraTexture[36], projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Microsoft.Xna.Framework.Color.White, projectile.localAI[0], origin2, projectile.scale, spriteEffects, 0f);
				}
				else if (projectile.type == 533)
				{
					Main.spriteBatch.Draw(Main.glowMaskTexture[128], projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Microsoft.Xna.Framework.Color.White * 0.3f, projectile.rotation, origin2, projectile.scale, spriteEffects, 0f);
				}
				else if (projectile.type == 601)
				{
					Microsoft.Xna.Framework.Color white = Microsoft.Xna.Framework.Color.White;
					white.A = 0;
					Main.spriteBatch.Draw(texture2D3, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), white, projectile.rotation, origin2, projectile.scale * 0.7f, spriteEffects, 0f);
				}
			}
			else if (projectile.type == 440 || projectile.type == 449 || projectile.type == 606)
			{
				Microsoft.Xna.Framework.Rectangle value6 = new Microsoft.Xna.Framework.Rectangle((int)Main.screenPosition.X - 500, (int)Main.screenPosition.Y - 500, Main.screenWidth + 1000, Main.screenHeight + 1000);
				if (projectile.getRect().Intersects(value6))
				{
					Vector2 value7 = new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY);
					float num162 = 100f;
					float scaleFactor = 3f;
					if (projectile.type == 606)
					{
						num162 = 150f;
						scaleFactor = 3f;
					}
					if (projectile.ai[1] == 1f)
					{
						num162 = (float)((int)projectile.localAI[0]);
					}
					for (int num163 = 1; num163 <= (int)projectile.localAI[0]; num163++)
					{
						Vector2 value8 = Vector2.Normalize(projectile.velocity) * (float)num163 * scaleFactor;
						Microsoft.Xna.Framework.Color color29 = projectile.GetAlpha(color25);
						color29 *= (num162 - (float)num163) / num162;
						color29.A = 0;
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], value7 - value8, null, color29, projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
					}
				}
			}
			else if (projectile.type == 651)
			{
				Player player = Main.player[projectile.owner];
				Microsoft.Xna.Framework.Point point = new Vector2(projectile.ai[0], projectile.ai[1]).ToPoint();
				Microsoft.Xna.Framework.Point point2 = projectile.Center.ToTileCoordinates();
				Microsoft.Xna.Framework.Color color30 = new Microsoft.Xna.Framework.Color(255, 255, 255, 0);
				Microsoft.Xna.Framework.Color color31 = new Microsoft.Xna.Framework.Color(127, 127, 127, 0);
				int num164 = 1;
				float num165 = 0f;
				WiresUI.Settings.MultiToolMode toolMode = WiresUI.Settings.ToolMode;
				bool flag21 = toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Actuator);
				if (toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Red))
				{
					num165 += 1f;
					color31 = Microsoft.Xna.Framework.Color.Lerp(color31, Microsoft.Xna.Framework.Color.Red, 1f / num165);
				}
				if (toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Blue))
				{
					num165 += 1f;
					color31 = Microsoft.Xna.Framework.Color.Lerp(color31, Microsoft.Xna.Framework.Color.Blue, 1f / num165);
				}
				if (toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Green))
				{
					num165 += 1f;
					color31 = Microsoft.Xna.Framework.Color.Lerp(color31, new Microsoft.Xna.Framework.Color(0, 255, 0), 1f / num165);
				}
				if (toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Yellow))
				{
					num165 += 1f;
					color31 = Microsoft.Xna.Framework.Color.Lerp(color31, new Microsoft.Xna.Framework.Color(255, 255, 0), 1f / num165);
				}
				if (toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Cutter))
				{
					color30 = new Microsoft.Xna.Framework.Color(50, 50, 50, 255);
				}
				color31.A = 0;
				if (point == point2)
				{
					Vector2 position = point2.ToVector2() * 16f - Main.screenPosition;
					Microsoft.Xna.Framework.Rectangle value9 = new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16);
					if (flag21)
					{
						Main.spriteBatch.Draw(Main.wireUITexture[11], position, null, color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position, new Microsoft.Xna.Framework.Rectangle?(value9), color31, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					value9.Y = 18;
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position, new Microsoft.Xna.Framework.Rectangle?(value9), color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				}
				else if (point.X == point2.X)
				{
					int num166 = point2.Y - point.Y;
					int num167 = Math.Sign(num166);
					Vector2 position2 = point.ToVector2() * 16f - Main.screenPosition;
					Microsoft.Xna.Framework.Rectangle value10 = new Microsoft.Xna.Framework.Rectangle((num166 * num164 > 0) ? 72 : 18, 0, 16, 16);
					if (flag21)
					{
						Main.spriteBatch.Draw(Main.wireUITexture[11], position2, null, color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position2, new Microsoft.Xna.Framework.Rectangle?(value10), color31, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					value10.Y = 18;
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position2, new Microsoft.Xna.Framework.Rectangle?(value10), color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					for (int num168 = point.Y + num167; num168 != point2.Y; num168 += num167)
					{
						position2 = new Vector2((float)(point.X * 16), (float)(num168 * 16)) - Main.screenPosition;
						value10.Y = 0;
						value10.X = 90;
						if (flag21)
						{
							Main.spriteBatch.Draw(Main.wireUITexture[11], position2, null, color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
						}
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position2, new Microsoft.Xna.Framework.Rectangle?(value10), color31, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
						value10.Y = 18;
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position2, new Microsoft.Xna.Framework.Rectangle?(value10), color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					position2 = point2.ToVector2() * 16f - Main.screenPosition;
					value10 = new Microsoft.Xna.Framework.Rectangle((num166 * num164 > 0) ? 18 : 72, 0, 16, 16);
					if (flag21)
					{
						Main.spriteBatch.Draw(Main.wireUITexture[11], position2, null, color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position2, new Microsoft.Xna.Framework.Rectangle?(value10), color31, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					value10.Y = 18;
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position2, new Microsoft.Xna.Framework.Rectangle?(value10), color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				}
				else if (point.Y == point2.Y)
				{
					int num169 = point2.X - point.X;
					int num170 = Math.Sign(num169);
					Vector2 position3 = point.ToVector2() * 16f - Main.screenPosition;
					Microsoft.Xna.Framework.Rectangle value11 = new Microsoft.Xna.Framework.Rectangle((num169 > 0) ? 36 : 144, 0, 16, 16);
					if (flag21)
					{
						Main.spriteBatch.Draw(Main.wireUITexture[11], position3, null, color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position3, new Microsoft.Xna.Framework.Rectangle?(value11), color31, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					value11.Y = 18;
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position3, new Microsoft.Xna.Framework.Rectangle?(value11), color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					for (int num171 = point.X + num170; num171 != point2.X; num171 += num170)
					{
						position3 = new Vector2((float)(num171 * 16), (float)(point.Y * 16)) - Main.screenPosition;
						value11.Y = 0;
						value11.X = 180;
						if (flag21)
						{
							Main.spriteBatch.Draw(Main.wireUITexture[11], position3, null, color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
						}
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position3, new Microsoft.Xna.Framework.Rectangle?(value11), color31, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
						value11.Y = 18;
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position3, new Microsoft.Xna.Framework.Rectangle?(value11), color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					position3 = point2.ToVector2() * 16f - Main.screenPosition;
					value11 = new Microsoft.Xna.Framework.Rectangle((num169 > 0) ? 144 : 36, 0, 16, 16);
					if (flag21)
					{
						Main.spriteBatch.Draw(Main.wireUITexture[11], position3, null, color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position3, new Microsoft.Xna.Framework.Rectangle?(value11), color31, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					value11.Y = 18;
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position3, new Microsoft.Xna.Framework.Rectangle?(value11), color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				}
				else
				{
					Math.Abs(point.X - point2.X);
					Math.Abs(point.Y - point2.Y);
					int num172 = Math.Sign(point2.X - point.X);
					int num173 = Math.Sign(point2.Y - point.Y);
					Microsoft.Xna.Framework.Point p = default(Microsoft.Xna.Framework.Point);
					bool flag22 = false;
					bool flag23 = player.direction == 1;
					int num174;
					int num175;
					int num176;
					if (flag23)
					{
						p.X = point.X;
						num174 = point.Y;
						num175 = point2.Y;
						num176 = num173;
					}
					else
					{
						p.Y = point.Y;
						num174 = point.X;
						num175 = point2.X;
						num176 = num172;
					}
					Vector2 position4 = point.ToVector2() * 16f - Main.screenPosition;
					Microsoft.Xna.Framework.Rectangle value12 = new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16);
					if (!flag23)
					{
						value12.X = ((num176 > 0) ? 36 : 144);
					}
					else
					{
						value12.X = ((num176 > 0) ? 72 : 18);
					}
					if (flag21)
					{
						Main.spriteBatch.Draw(Main.wireUITexture[11], position4, null, color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position4, new Microsoft.Xna.Framework.Rectangle?(value12), color31, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					value12.Y = 18;
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position4, new Microsoft.Xna.Framework.Rectangle?(value12), color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					int num177 = num174 + num176;
					while (num177 != num175 && !flag22)
					{
						if (flag23)
						{
							p.Y = num177;
						}
						else
						{
							p.X = num177;
						}
						if (WorldGen.InWorld(p.X, p.Y, 1))
						{
							Tile tile = Main.tile[p.X, p.Y];
							if (tile != null)
							{
								position4 = p.ToVector2() * 16f - Main.screenPosition;
								value12.Y = 0;
								if (!flag23)
								{
									value12.X = 180;
								}
								else
								{
									value12.X = 90;
								}
								if (flag21)
								{
									Main.spriteBatch.Draw(Main.wireUITexture[11], position4, null, color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
								}
								Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position4, new Microsoft.Xna.Framework.Rectangle?(value12), color31, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
								value12.Y = 18;
								Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position4, new Microsoft.Xna.Framework.Rectangle?(value12), color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
							}
						}
						num177 += num176;
					}
					if (flag23)
					{
						p.Y = point2.Y;
						num174 = point.X;
						num175 = point2.X;
						num176 = num172;
					}
					else
					{
						p.X = point2.X;
						num174 = point.Y;
						num175 = point2.Y;
						num176 = num173;
					}
					position4 = p.ToVector2() * 16f - Main.screenPosition;
					value12 = new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16);
					if (!flag23)
					{
						value12.X += ((num172 > 0) ? 144 : 36);
						value12.X += ((num173 * num164 > 0) ? 72 : 18);
					}
					else
					{
						value12.X += ((num172 > 0) ? 36 : 144);
						value12.X += ((num173 * num164 > 0) ? 18 : 72);
					}
					if (flag21)
					{
						Main.spriteBatch.Draw(Main.wireUITexture[11], position4, null, color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position4, new Microsoft.Xna.Framework.Rectangle?(value12), color31, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					value12.Y = 18;
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position4, new Microsoft.Xna.Framework.Rectangle?(value12), color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					int num178 = num174 + num176;
					while (num178 != num175 && !flag22)
					{
						if (!flag23)
						{
							p.Y = num178;
						}
						else
						{
							p.X = num178;
						}
						if (WorldGen.InWorld(p.X, p.Y, 1))
						{
							Tile tile = Main.tile[p.X, p.Y];
							if (tile != null)
							{
								position4 = p.ToVector2() * 16f - Main.screenPosition;
								value12.Y = 0;
								if (!flag23)
								{
									value12.X = 90;
								}
								else
								{
									value12.X = 180;
								}
								if (flag21)
								{
									Main.spriteBatch.Draw(Main.wireUITexture[11], position4, null, color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
								}
								Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position4, new Microsoft.Xna.Framework.Rectangle?(value12), color31, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
								value12.Y = 18;
								Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position4, new Microsoft.Xna.Framework.Rectangle?(value12), color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
							}
						}
						num178 += num176;
					}
					position4 = point2.ToVector2() * 16f - Main.screenPosition;
					value12 = new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16);
					if (!flag23)
					{
						value12.X += ((num173 * num164 > 0) ? 18 : 72);
					}
					else
					{
						value12.X += ((num172 > 0) ? 144 : 36);
					}
					if (flag21)
					{
						Main.spriteBatch.Draw(Main.wireUITexture[11], position4, null, color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position4, new Microsoft.Xna.Framework.Rectangle?(value12), color31, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					value12.Y = 18;
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], position4, new Microsoft.Xna.Framework.Rectangle?(value12), color30, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				}
			}
			else if (projectile.type == 586)
			{
				float num179 = 300f;
				if (projectile.ai[0] >= 100f)
				{
					num179 = MathHelper.Lerp(300f, 600f, (projectile.ai[0] - 100f) / 200f);
				}
				if (num179 > 600f)
				{
					num179 = 600f;
				}
				if (projectile.ai[0] >= 500f)
				{
					num179 = MathHelper.Lerp(600f, 1200f, (projectile.ai[0] - 500f) / 100f);
				}
				float rotation23 = projectile.rotation;
				Texture2D texture2D4 = Main.projectileTexture[projectile.type];
				Microsoft.Xna.Framework.Color alpha2 = projectile.GetAlpha(color25);
				alpha2.A /= 2;
				int num180 = (int)(projectile.ai[0] / 6f);
				Vector2 spinningpoint = new Vector2(0f, -num179);
				int num181 = 0;
				while ((float)num181 < 10f)
				{
					Microsoft.Xna.Framework.Rectangle rectangle2 = texture2D4.Frame(1, 5, 0, (num180 + num181) % 5);
					float num182 = rotation23 + 0.628318548f * (float)num181;
					Vector2 position5 = spinningpoint.RotatedBy((double)num182, default(Vector2)) / 3f + projectile.Center - Main.screenPosition;
					Main.spriteBatch.Draw(texture2D4, position5, new Microsoft.Xna.Framework.Rectangle?(rectangle2), alpha2, num182, rectangle2.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
					num181++;
				}
				int num183 = 0;
				while ((float)num183 < 20f)
				{
					Microsoft.Xna.Framework.Rectangle rectangle3 = texture2D4.Frame(1, 5, 0, (num180 + num183) % 5);
					float num184 = -rotation23 + 0.314159274f * (float)num183;
					num184 *= 2f;
					Vector2 position6 = spinningpoint.RotatedBy((double)num184, default(Vector2)) + projectile.Center - Main.screenPosition;
					Main.spriteBatch.Draw(texture2D4, position6, new Microsoft.Xna.Framework.Rectangle?(rectangle3), alpha2, num184, rectangle3.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
					num183++;
				}
			}
			else if (projectile.type == 536 || projectile.type == 591 || projectile.type == 607)
			{
				Texture2D texture2D5 = Main.projectileTexture[projectile.type];
				Vector2 position7 = projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
				Vector2 scale = new Vector2(1f, projectile.velocity.Length() / (float)texture2D5.Height);
				Main.spriteBatch.Draw(texture2D5, position7, null, projectile.GetAlpha(color25), projectile.rotation, texture2D5.Frame(1, 1, 0, 0).Bottom(), scale, spriteEffects, 0f);
			}
			else if (projectile.type == 409)
			{
				Texture2D texture2D6 = Main.projectileTexture[projectile.type];
				int num185 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				int y4 = num185 * projectile.frame;
				int num186 = 10;
				int num187 = 2;
				float value13 = 0.5f;
				for (int num188 = 1; num188 < num186; num188 += num187)
				{
					//Vector2 arg_7BB5_0 = Main.npc[i].oldPos[num188];
					Microsoft.Xna.Framework.Color color32 = color25;
					color32 = projectile.GetAlpha(color32);
					color32 *= (float)(num186 - num188) / 15f;
					//projectile.oldPos[num188] - Main.screenPosition + new Vector2(num149 + (float)num148, (float)(projectile.height / 2) + projectile.gfxOffY);
					Main.spriteBatch.Draw(texture2D6, projectile.oldPos[num188] + new Vector2((float)projectile.width, (float)projectile.height) / 2f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y4, texture2D6.Width, num185)), color32, projectile.rotation, new Vector2((float)texture2D6.Width / 2f, (float)num185 / 2f), MathHelper.Lerp(projectile.scale, value13, (float)num188 / 15f), spriteEffects, 0f);
				}
				Main.spriteBatch.Draw(texture2D6, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y4, texture2D6.Width, num185)), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)texture2D6.Width / 2f, (float)num185 / 2f), projectile.scale, spriteEffects, 0f);
			}
			else if (projectile.type == 437)
			{
				Texture2D texture2D7 = Main.projectileTexture[projectile.type];
				int num189 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				int y5 = num189 * projectile.frame;
				int num190 = 10;
				int num191 = 2;
				float value14 = 0.2f;
				for (int num192 = 1; num192 < num190; num192 += num191)
				{
					//Vector2 arg_7E2E_0 = Main.npc[i].oldPos[num192];
					Microsoft.Xna.Framework.Color color33 = color25;
					color33 = projectile.GetAlpha(color33);
					color33 *= (float)(num190 - num192) / 15f;
					//projectile.oldPos[num192] - Main.screenPosition + new Vector2(num149 + (float)num148, (float)(projectile.height / 2) + projectile.gfxOffY);
					Main.spriteBatch.Draw(texture2D7, projectile.oldPos[num192] + new Vector2((float)projectile.width, (float)projectile.height) / 2f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y5, texture2D7.Width, num189)), color33, projectile.rotation, new Vector2((float)texture2D7.Width / 2f, (float)num189 / 2f), MathHelper.Lerp(projectile.scale, value14, (float)num192 / 15f), spriteEffects, 0f);
				}
				Main.spriteBatch.Draw(texture2D7, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y5, texture2D7.Width, num189)), Microsoft.Xna.Framework.Color.White, projectile.rotation, new Vector2((float)texture2D7.Width / 2f, (float)num189 / 2f), projectile.scale + 0.2f, spriteEffects, 0f);
				Main.spriteBatch.Draw(texture2D7, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y5, texture2D7.Width, num189)), projectile.GetAlpha(Microsoft.Xna.Framework.Color.White), projectile.rotation, new Vector2((float)texture2D7.Width / 2f, (float)num189 / 2f), projectile.scale + 0.2f, spriteEffects, 0f);
			}
			else if (projectile.type == 384 || projectile.type == 386)
			{
				Texture2D texture2D8 = Main.projectileTexture[projectile.type];
				int num193 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				int y6 = num193 * projectile.frame;
				Main.spriteBatch.Draw(texture2D8, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y6, texture2D8.Width, num193)), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)texture2D8.Width / 2f, (float)num193 / 2f), projectile.scale, spriteEffects, 0f);
			}
			else if (projectile.type == 439 || projectile.type == 460 || projectile.type == 600 || projectile.type == 615 || projectile.type == 630 || projectile.type == 633)
			{
				Texture2D texture2D9 = Main.projectileTexture[projectile.type];
				int num194 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				int y7 = num194 * projectile.frame;
				Vector2 vector26 = (projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition).Floor();
				float scale2 = 1f;
				if (Main.player[projectile.owner].shroomiteStealth && Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].ranged)
				{
					float num195 = Main.player[projectile.owner].stealth;
					if ((double)num195 < 0.03)
					{
						num195 = 0.03f;
					}
					//float arg_8314_0 = (1f + num195 * 10f) / 11f;
					color25 *= num195;
					scale2 = num195;
				}
				if (Main.player[projectile.owner].setVortex && Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].ranged)
				{
					float num196 = Main.player[projectile.owner].stealth;
					if ((double)num196 < 0.03)
					{
						num196 = 0.03f;
					}
					//float arg_83B5_0 = (1f + num196 * 10f) / 11f;
					color25 = color25.MultiplyRGBA(new Microsoft.Xna.Framework.Color(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num196)));
					scale2 = num196;
				}
				Main.spriteBatch.Draw(texture2D9, vector26, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y7, texture2D9.Width, num194)), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)texture2D9.Width / 2f, (float)num194 / 2f), projectile.scale, spriteEffects, 0f);
				if (projectile.type == 439)
				{
					Main.spriteBatch.Draw(Main.glowMaskTexture[35], vector26, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y7, texture2D9.Width, num194)), new Microsoft.Xna.Framework.Color(255, 255, 255, 0) * scale2, projectile.rotation, new Vector2((float)texture2D9.Width / 2f, (float)num194 / 2f), projectile.scale, spriteEffects, 0f);
				}
				else if (projectile.type == 615)
				{
					Main.spriteBatch.Draw(Main.glowMaskTexture[192], vector26, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y7, texture2D9.Width, num194)), new Microsoft.Xna.Framework.Color(255, 255, 255, 127) * scale2, projectile.rotation, new Vector2((float)texture2D9.Width / 2f, (float)num194 / 2f), projectile.scale, spriteEffects, 0f);
				}
				else if (projectile.type == 630)
				{
					Main.spriteBatch.Draw(Main.glowMaskTexture[200], vector26, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y7, texture2D9.Width, num194)), new Microsoft.Xna.Framework.Color(255, 255, 255, 127) * scale2, projectile.rotation, new Vector2((float)texture2D9.Width / 2f, (float)num194 / 2f), projectile.scale, spriteEffects, 0f);
					if (projectile.localAI[0] > 0f)
					{
						int frameY = 6 - (int)(projectile.localAI[0] / 1f);
						texture2D9 = Main.extraTexture[65];
						Main.spriteBatch.Draw(texture2D9, vector26 + Vector2.Normalize(projectile.velocity) * 2f, new Microsoft.Xna.Framework.Rectangle?(texture2D9.Frame(1, 6, 0, frameY)), new Microsoft.Xna.Framework.Color(255, 255, 255, 127) * scale2, projectile.rotation, new Vector2((float)(spriteEffects.HasFlag(SpriteEffects.FlipHorizontally) ? texture2D9.Width : 0), (float)num194 / 2f - 2f), projectile.scale, spriteEffects, 0f);
					}
				}
				else if (projectile.type == 600)
				{
					Microsoft.Xna.Framework.Color portalColor = PortalHelper.GetPortalColor(projectile.owner, (int)projectile.ai[1]);
					portalColor.A = 70;
					Main.spriteBatch.Draw(Main.glowMaskTexture[173], vector26, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y7, texture2D9.Width, num194)), portalColor, projectile.rotation, new Vector2((float)texture2D9.Width / 2f, (float)num194 / 2f), projectile.scale, spriteEffects, 0f);
				}
				else if (projectile.type == 460)
				{
					if (Math.Abs(projectile.rotation - 1.57079637f) > 1.57079637f)
					{
						spriteEffects |= SpriteEffects.FlipVertically;
					}
					Main.spriteBatch.Draw(Main.glowMaskTexture[102], vector26, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y7, texture2D9.Width, num194)), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), projectile.rotation - 1.57079637f, new Vector2((float)texture2D9.Width / 2f, (float)num194 / 2f), projectile.scale, spriteEffects, 0f);
					if (projectile.ai[0] > 180f && Main.projectile[(int)projectile.ai[1]].type == 461)
					{
						DrawProj((int)projectile.ai[1]);
					}
				}
				else if (projectile.type == 633)
				{
					float scaleFactor2 = (float)Math.Cos((double)(6.28318548f * (projectile.ai[0] / 30f))) * 2f + 2f;
					if (projectile.ai[0] > 120f)
					{
						scaleFactor2 = 4f;
					}
					for (float num197 = 0f; num197 < 4f; num197 += 1f)
					{
						Main.spriteBatch.Draw(texture2D9, vector26 + Vector2.UnitY.RotatedBy((double)(num197 * 6.28318548f / 4f), default(Vector2)) * scaleFactor2, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y7, texture2D9.Width, num194)), projectile.GetAlpha(color25).MultiplyRGBA(new Microsoft.Xna.Framework.Color(255, 255, 255, 0)) * 0.03f, projectile.rotation, new Vector2((float)texture2D9.Width / 2f, (float)num194 / 2f), projectile.scale, spriteEffects, 0f);
					}
				}
			}
			else if (projectile.type == 442)
			{
				Texture2D texture2D10 = Main.projectileTexture[projectile.type];
				int num198 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				int y8 = num198 * projectile.frame;
				Vector2 position8 = projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
				Main.spriteBatch.Draw(texture2D10, position8, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y8, texture2D10.Width, num198)), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)texture2D10.Width / 2f, (float)num198 / 2f), projectile.scale, spriteEffects, 0f);
				Main.spriteBatch.Draw(Main.glowMaskTexture[37], position8, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y8, texture2D10.Width, num198)), new Microsoft.Xna.Framework.Color(255, 255, 255, 0) * (1f - (float)projectile.alpha / 255f), projectile.rotation, new Vector2((float)texture2D10.Width / 2f, (float)num198 / 2f), projectile.scale, spriteEffects, 0f);
			}
			else if (projectile.type == 447)
			{
				Texture2D texture2D11 = Main.projectileTexture[projectile.type];
				Texture2D texture2D12 = Main.extraTexture[4];
				int num199 = texture2D11.Height / Main.projFrames[projectile.type];
				int y9 = num199 * projectile.frame;
				int num200 = texture2D12.Height / Main.projFrames[projectile.type];
				int num201 = num200 * projectile.frame;
				Microsoft.Xna.Framework.Rectangle value15 = new Microsoft.Xna.Framework.Rectangle(0, num201, texture2D12.Width, num200);
				Vector2 vector27 = projectile.position + new Vector2((float)projectile.width, 0f) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
				Main.spriteBatch.Draw(Main.extraTexture[4], vector27, new Microsoft.Xna.Framework.Rectangle?(value15), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)(texture2D12.Width / 2), 0f), projectile.scale, spriteEffects, 0f);
				int num202 = projectile.height - num199 - 14;
				if (num202 < 0)
				{
					num202 = 0;
				}
				if (num202 > 0)
				{
					if (num201 == num200 * 3)
					{
						num201 = num200 * 2;
					}
					Main.spriteBatch.Draw(Main.extraTexture[4], vector27 + Vector2.UnitY * (float)(num200 - 1), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, num201 + num200 - 1, texture2D12.Width, 1)), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)(texture2D12.Width / 2), 0f), new Vector2(1f, (float)num202), spriteEffects, 0f);
				}
				value15.Width = texture2D11.Width;
				value15.Y = y9;
				Main.spriteBatch.Draw(texture2D11, vector27 + Vector2.UnitY * (float)(num200 - 1 + num202), new Microsoft.Xna.Framework.Rectangle?(value15), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)texture2D11.Width / 2f, 0f), projectile.scale, spriteEffects, 0f);
			}
			else if (projectile.type == 455)
			{
				if (projectile.velocity == Vector2.Zero)
				{
					return;
				}
				Texture2D texture2D13 = Main.projectileTexture[projectile.type];
				Texture2D texture2D14 = Main.extraTexture[21];
				Texture2D texture2D15 = Main.extraTexture[22];
				float num203 = projectile.localAI[1];
				Microsoft.Xna.Framework.Color color34 = new Microsoft.Xna.Framework.Color(255, 255, 255, 0) * 0.9f;
				Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition, null, color34, projectile.rotation, texture2D13.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
				num203 -= (float)(texture2D13.Height / 2 + texture2D15.Height) * projectile.scale;
				Vector2 value16 = projectile.Center;
				value16 += projectile.velocity * projectile.scale * (float)texture2D13.Height / 2f;
				if (num203 > 0f)
				{
					float num204 = 0f;
					Microsoft.Xna.Framework.Rectangle value17 = new Microsoft.Xna.Framework.Rectangle(0, 16 * (projectile.timeLeft / 3 % 5), texture2D14.Width, 16);
					while (num204 + 1f < num203)
					{
						if (num203 - num204 < (float)value17.Height)
						{
							value17.Height = (int)(num203 - num204);
						}
						Main.spriteBatch.Draw(texture2D14, value16 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(value17), color34, projectile.rotation, new Vector2((float)(value17.Width / 2), 0f), projectile.scale, SpriteEffects.None, 0f);
						num204 += (float)value17.Height * projectile.scale;
						value16 += projectile.velocity * (float)value17.Height * projectile.scale;
						value17.Y += 16;
						if (value17.Y + value17.Height > texture2D14.Height)
						{
							value17.Y = 0;
						}
					}
				}
				Main.spriteBatch.Draw(texture2D15, value16 - Main.screenPosition, null, color34, projectile.rotation, texture2D15.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
			}
			else if (projectile.type == 461)
			{
				if (projectile.velocity == Vector2.Zero)
				{
					return;
				}
				Texture2D texture2D16 = Main.projectileTexture[projectile.type];
				float num205 = projectile.localAI[1];
				Microsoft.Xna.Framework.Color color35 = new Microsoft.Xna.Framework.Color(255, 255, 255, 0) * 0.9f;
				Microsoft.Xna.Framework.Rectangle rectangle4 = new Microsoft.Xna.Framework.Rectangle(0, 0, texture2D16.Width, 22);
				Vector2 value18 = new Vector2(0f, Main.player[projectile.owner].gfxOffY);
				Main.spriteBatch.Draw(texture2D16, projectile.Center.Floor() - Main.screenPosition + value18, new Microsoft.Xna.Framework.Rectangle?(rectangle4), color35, projectile.rotation, rectangle4.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
				num205 -= 33f * projectile.scale;
				Vector2 value19 = projectile.Center.Floor();
				value19 += projectile.velocity * projectile.scale * 10.5f;
				rectangle4 = new Microsoft.Xna.Framework.Rectangle(0, 25, texture2D16.Width, 28);
				if (num205 > 0f)
				{
					float num206 = 0f;
					while (num206 + 1f < num205)
					{
						if (num205 - num206 < (float)rectangle4.Height)
						{
							rectangle4.Height = (int)(num205 - num206);
						}
						Main.spriteBatch.Draw(texture2D16, value19 - Main.screenPosition + value18, new Microsoft.Xna.Framework.Rectangle?(rectangle4), color35, projectile.rotation, new Vector2((float)(rectangle4.Width / 2), 0f), projectile.scale, SpriteEffects.None, 0f);
						num206 += (float)rectangle4.Height * projectile.scale;
						value19 += projectile.velocity * (float)rectangle4.Height * projectile.scale;
					}
				}
				rectangle4 = new Microsoft.Xna.Framework.Rectangle(0, 56, texture2D16.Width, 22);
				Main.spriteBatch.Draw(texture2D16, value19 - Main.screenPosition + value18, new Microsoft.Xna.Framework.Rectangle?(rectangle4), color35, projectile.rotation, texture2D16.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
			}
			else if (projectile.type == 632)
			{
				if (projectile.velocity == Vector2.Zero)
				{
					return;
				}
				Texture2D tex = Main.projectileTexture[projectile.type];
				float num207 = projectile.localAI[1];
				float prismHue = projectile.GetPrismHue(projectile.ai[0]);
				Microsoft.Xna.Framework.Color value20 = Main.hslToRgb(prismHue, 1f, 0.5f);
				value20.A = 0;
				Vector2 value21 = projectile.Center.Floor();
				value21 += projectile.velocity * projectile.scale * 10.5f;
				num207 -= projectile.scale * 14.5f * projectile.scale;
				Vector2 vector28 = new Vector2(projectile.scale);
				DelegateMethods.f_1 = 1f;
				DelegateMethods.c_1 = value20 * 0.75f * projectile.Opacity;
				//projectile.oldPos[0] + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
				Utils.DrawLaser(Main.spriteBatch, tex, value21 - Main.screenPosition, value21 + projectile.velocity * num207 - Main.screenPosition, vector28, new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw));
				DelegateMethods.c_1 = new Microsoft.Xna.Framework.Color(255, 255, 255, 127) * 0.75f * projectile.Opacity;
				Utils.DrawLaser(Main.spriteBatch, tex, value21 - Main.screenPosition, value21 + projectile.velocity * num207 - Main.screenPosition, vector28 / 2f, new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw));
			}
			else if (projectile.type == 642)
			{
				if (projectile.velocity == Vector2.Zero)
				{
					return;
				}
				Texture2D tex2 = Main.projectileTexture[projectile.type];
				float num208 = projectile.localAI[1];
				Microsoft.Xna.Framework.Color c_ = new Microsoft.Xna.Framework.Color(255, 255, 255, 127);
				Vector2 value22 = projectile.Center.Floor();
				num208 -= projectile.scale * 10.5f;
				Vector2 vector29 = new Vector2(projectile.scale);
				DelegateMethods.f_1 = 1f;
				DelegateMethods.c_1 = c_;
				DelegateMethods.i_1 = 54000 - (int)Main.time / 2;
				//projectile.oldPos[0] + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
				Utils.DrawLaser(Main.spriteBatch, tex2, value22 - Main.screenPosition, value22 + projectile.velocity * num208 - Main.screenPosition, vector29, new Utils.LaserLineFraming(DelegateMethods.TurretLaserDraw));
				DelegateMethods.c_1 = new Microsoft.Xna.Framework.Color(255, 255, 255, 127) * 0.75f * projectile.Opacity;
				Utils.DrawLaser(Main.spriteBatch, tex2, value22 - Main.screenPosition, value22 + projectile.velocity * num208 - Main.screenPosition, vector29 / 2f, new Utils.LaserLineFraming(DelegateMethods.TurretLaserDraw));
			}
			else if (projectile.type == 611)
			{
				//projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
				Texture2D texture2D17 = Main.projectileTexture[projectile.type];
				Microsoft.Xna.Framework.Color alpha3 = projectile.GetAlpha(color25);
				if (projectile.velocity == Vector2.Zero)
				{
					return;
				}
				float num209 = projectile.velocity.Length() + 16f;
				bool flag24 = num209 < 100f;
				Vector2 value23 = Vector2.Normalize(projectile.velocity);
				Microsoft.Xna.Framework.Rectangle rectangle5 = new Microsoft.Xna.Framework.Rectangle(0, 2, texture2D17.Width, 40);
				Vector2 value24 = new Vector2(0f, Main.player[projectile.owner].gfxOffY);
				float rotation24 = projectile.rotation + 3.14159274f;
				Main.spriteBatch.Draw(texture2D17, projectile.Center.Floor() - Main.screenPosition + value24, new Microsoft.Xna.Framework.Rectangle?(rectangle5), alpha3, rotation24, rectangle5.Size() / 2f - Vector2.UnitY * 4f, projectile.scale, SpriteEffects.None, 0f);
				num209 -= 40f * projectile.scale;
				Vector2 vector30 = projectile.Center.Floor();
				vector30 += value23 * projectile.scale * 24f;
				rectangle5 = new Microsoft.Xna.Framework.Rectangle(0, 68, texture2D17.Width, 18);
				if (num209 > 0f)
				{
					float num210 = 0f;
					while (num210 + 1f < num209)
					{
						if (num209 - num210 < (float)rectangle5.Height)
						{
							rectangle5.Height = (int)(num209 - num210);
						}
						Main.spriteBatch.Draw(texture2D17, vector30 - Main.screenPosition + value24, new Microsoft.Xna.Framework.Rectangle?(rectangle5), alpha3, rotation24, new Vector2((float)(rectangle5.Width / 2), 0f), projectile.scale, SpriteEffects.None, 0f);
						num210 += (float)rectangle5.Height * projectile.scale;
						vector30 += value23 * (float)rectangle5.Height * projectile.scale;
					}
				}
				Vector2 value25 = vector30;
				vector30 = projectile.Center.Floor();
				vector30 += value23 * projectile.scale * 24f;
				rectangle5 = new Microsoft.Xna.Framework.Rectangle(0, 46, texture2D17.Width, 18);
				int num211 = 18;
				if (flag24)
				{
					num211 = 9;
				}
				float num212 = num209;
				if (num209 > 0f)
				{
					float num213 = 0f;
					float num214 = num212 / (float)num211;
					num213 += num214 * 0.25f;
					vector30 += value23 * num214 * 0.25f;
					for (int num215 = 0; num215 < num211; num215++)
					{
						float num216 = num214;
						if (num215 == 0)
						{
							num216 *= 0.75f;
						}
						Main.spriteBatch.Draw(texture2D17, vector30 - Main.screenPosition + value24, new Microsoft.Xna.Framework.Rectangle?(rectangle5), alpha3, rotation24, new Vector2((float)(rectangle5.Width / 2), 0f), projectile.scale, SpriteEffects.None, 0f);
						num213 += num216;
						vector30 += value23 * num216;
					}
				}
				rectangle5 = new Microsoft.Xna.Framework.Rectangle(0, 90, texture2D17.Width, 48);
				Main.spriteBatch.Draw(texture2D17, value25 - Main.screenPosition + value24, new Microsoft.Xna.Framework.Rectangle?(rectangle5), alpha3, rotation24, texture2D17.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
			}
			else if (projectile.type == 537)
			{
				if (projectile.velocity == Vector2.Zero)
				{
					return;
				}
				Texture2D texture2D18 = Main.projectileTexture[projectile.type];
				float num217 = projectile.localAI[1];
				Microsoft.Xna.Framework.Color color36 = new Microsoft.Xna.Framework.Color(255, 255, 255, 0) * 0.9f;
				Microsoft.Xna.Framework.Rectangle rectangle6 = new Microsoft.Xna.Framework.Rectangle(0, 0, texture2D18.Width, 22);
				Vector2 value26 = new Vector2(0f, Main.npc[(int)projectile.ai[1]].gfxOffY);
				Main.spriteBatch.Draw(texture2D18, projectile.Center.Floor() - Main.screenPosition + value26, new Microsoft.Xna.Framework.Rectangle?(rectangle6), color36, projectile.rotation, rectangle6.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
				num217 -= 33f * projectile.scale;
				Vector2 value27 = projectile.Center.Floor();
				value27 += projectile.velocity * projectile.scale * 10.5f;
				rectangle6 = new Microsoft.Xna.Framework.Rectangle(0, 25, texture2D18.Width, 28);
				if (num217 > 0f)
				{
					float num218 = 0f;
					while (num218 + 1f < num217)
					{
						if (num217 - num218 < (float)rectangle6.Height)
						{
							rectangle6.Height = (int)(num217 - num218);
						}
						Main.spriteBatch.Draw(texture2D18, value27 - Main.screenPosition + value26, new Microsoft.Xna.Framework.Rectangle?(rectangle6), color36, projectile.rotation, new Vector2((float)(rectangle6.Width / 2), 0f), projectile.scale, SpriteEffects.None, 0f);
						num218 += (float)rectangle6.Height * projectile.scale;
						value27 += projectile.velocity * (float)rectangle6.Height * projectile.scale;
					}
				}
				rectangle6 = new Microsoft.Xna.Framework.Rectangle(0, 56, texture2D18.Width, 22);
				Main.spriteBatch.Draw(texture2D18, value27 - Main.screenPosition + value26, new Microsoft.Xna.Framework.Rectangle?(rectangle6), color36, projectile.rotation, texture2D18.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
			}
			else if (projectile.type == 456)
			{
				Texture2D texture2D19 = Main.projectileTexture[projectile.type];
				Texture2D texture2D20 = Main.extraTexture[23];
				Texture2D texture2D21 = Main.extraTexture[24];
				Vector2 value28 = new Vector2(0f, 216f);
				Vector2 value29 = Main.npc[(int)Math.Abs(projectile.ai[0]) - 1].Center - projectile.Center + value28;
				float num219 = value29.Length();
				Vector2 value30 = Vector2.Normalize(value29);
				Microsoft.Xna.Framework.Rectangle rectangle7 = texture2D19.Frame(1, 1, 0, 0);
				rectangle7.Height /= 4;
				rectangle7.Y += projectile.frame * rectangle7.Height;
				color25 = Microsoft.Xna.Framework.Color.Lerp(color25, Microsoft.Xna.Framework.Color.White, 0.3f);
				Main.spriteBatch.Draw(texture2D19, projectile.Center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(rectangle7), projectile.GetAlpha(color25), projectile.rotation, rectangle7.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
				num219 -= (float)(rectangle7.Height / 2 + texture2D21.Height) * projectile.scale;
				Vector2 vector31 = projectile.Center;
				vector31 += value30 * projectile.scale * (float)rectangle7.Height / 2f;
				if (num219 > 0f)
				{
					float num220 = 0f;
					Microsoft.Xna.Framework.Rectangle rectangle8 = new Microsoft.Xna.Framework.Rectangle(0, 0, texture2D20.Width, texture2D20.Height);
					while (num220 + 1f < num219)
					{
						if (num219 - num220 < (float)rectangle8.Height)
						{
							rectangle8.Height = (int)(num219 - num220);
						}
						Microsoft.Xna.Framework.Point point3 = vector31.ToTileCoordinates();
						Microsoft.Xna.Framework.Color color37 = Lighting.GetColor(point3.X, point3.Y);
						color37 = Microsoft.Xna.Framework.Color.Lerp(color37, Microsoft.Xna.Framework.Color.White, 0.3f);
						Main.spriteBatch.Draw(texture2D20, vector31 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(rectangle8), projectile.GetAlpha(color37), projectile.rotation, rectangle8.Bottom(), projectile.scale, SpriteEffects.None, 0f);
						num220 += (float)rectangle8.Height * projectile.scale;
						vector31 += value30 * (float)rectangle8.Height * projectile.scale;
					}
				}
				Microsoft.Xna.Framework.Point point4 = vector31.ToTileCoordinates();
				Microsoft.Xna.Framework.Color color38 = Lighting.GetColor(point4.X, point4.Y);
				color38 = Microsoft.Xna.Framework.Color.Lerp(color38, Microsoft.Xna.Framework.Color.White, 0.3f);
				Microsoft.Xna.Framework.Rectangle value31 = texture2D21.Frame(1, 1, 0, 0);
				if (num219 < 0f)
				{
					value31.Height += (int)num219;
				}
				Main.spriteBatch.Draw(texture2D21, vector31 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(value31), color38, projectile.rotation, new Vector2((float)value31.Width / 2f, (float)value31.Height), projectile.scale, SpriteEffects.None, 0f);
			}
			else if (projectile.type == 443)
			{
				Texture2D texture2D22 = Main.projectileTexture[projectile.type];
				float num221 = 30f;
				float num222 = num221 * 4f;
				float num223 = 6.28318548f * projectile.ai[0] / num221;
				float num224 = 6.28318548f * projectile.ai[0] / num222;
				Vector2 vector32 = -Vector2.UnitY.RotatedBy((double)num223, default(Vector2));
				float scale3 = 0.75f + vector32.Y * 0.25f;
				float scale4 = 0.8f - vector32.Y * 0.2f;
				int num225 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				int y10 = num225 * projectile.frame;
				Vector2 position9 = projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
				Main.spriteBatch.Draw(texture2D22, position9, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y10, texture2D22.Width, num225)), projectile.GetAlpha(color25), projectile.rotation + num224, new Vector2((float)texture2D22.Width / 2f, (float)num225 / 2f), scale3, spriteEffects, 0f);
				Main.spriteBatch.Draw(texture2D22, position9, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y10, texture2D22.Width, num225)), projectile.GetAlpha(color25), projectile.rotation + (6.28318548f - num224), new Vector2((float)texture2D22.Width / 2f, (float)num225 / 2f), scale4, spriteEffects, 0f);
			}
			else if (projectile.type == 656 || projectile.type == 657)
			{
				float num226 = 900f;
				if (projectile.type == 657)
				{
					num226 = 300f;
				}
				float num227 = 15f;
				float num228 = 15f;
				float num229 = projectile.ai[0];
				float scale5 = MathHelper.Clamp(num229 / 30f, 0f, 1f);
				if (num229 > num226 - 60f)
				{
					scale5 = MathHelper.Lerp(1f, 0f, (num229 - (num226 - 60f)) / 60f);
				}
				Microsoft.Xna.Framework.Point point5 = projectile.Center.ToTileCoordinates();
				int num230;
				int num231;
				Collision.ExpandVertically(point5.X, point5.Y, out num230, out num231, (int)num227, (int)num228);
				num230++;
				num231--;
				float num232 = 0.2f;
				Vector2 value32 = new Vector2((float)point5.X, (float)num230) * 16f + new Vector2(8f);
				Vector2 value33 = new Vector2((float)point5.X, (float)num231) * 16f + new Vector2(8f);
				Vector2.Lerp(value32, value33, 0.5f);
				Vector2 vector33 = new Vector2(0f, value33.Y - value32.Y);
				vector33.X = vector33.Y * num232;
				new Vector2(value32.X - vector33.X / 2f, value32.Y);
				Texture2D texture2D23 = Main.projectileTexture[projectile.type];
				Microsoft.Xna.Framework.Rectangle rectangle9 = texture2D23.Frame(1, 1, 0, 0);
				Vector2 origin3 = rectangle9.Size() / 2f;
				float num233 = -0.06283186f * num229;
				Vector2 spinningpoint2 = Vector2.UnitY.RotatedBy((double)(num229 * 0.1f), default(Vector2));
				float num234 = 0f;
				float num235 = 5.1f;
				Microsoft.Xna.Framework.Color value34 = new Microsoft.Xna.Framework.Color(212, 192, 100);
				for (float num236 = (float)((int)value33.Y); num236 > (float)((int)value32.Y); num236 -= num235)
				{
					num234 += num235;
					float num237 = num234 / vector33.Y;
					float num238 = num234 * 6.28318548f / -20f;
					float num239 = num237 - 0.15f;
					Vector2 vector34 = spinningpoint2.RotatedBy((double)num238, default(Vector2));
					Vector2 value35 = new Vector2(0f, num237 + 1f);
					value35.X = value35.Y * num232;
					Microsoft.Xna.Framework.Color color39 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.Transparent, value34, num237 * 2f);
					if (num237 > 0.5f)
					{
						color39 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.Transparent, value34, 2f - num237 * 2f);
					}
					color39.A = (byte)((float)color39.A * 0.5f);
					color39 *= scale5;
					vector34 *= value35 * 100f;
					vector34.Y = 0f;
					vector34.X = 0f;
					vector34 += new Vector2(value33.X, num236) - Main.screenPosition;
					Main.spriteBatch.Draw(texture2D23, vector34, new Microsoft.Xna.Framework.Rectangle?(rectangle9), color39, num233 + num238, origin3, 1f + num239, SpriteEffects.None, 0f);
				}
			}
			else if (projectile.type == 444 || projectile.type == 446 || projectile.type == 490 || projectile.type == 464 || projectile.type == 502 || projectile.type == 538 || projectile.type == 540 || projectile.type == 579 || projectile.type == 578 || projectile.type == 583 || projectile.type == 584 || projectile.type == 616 || projectile.type == 617 || projectile.type == 618 || projectile.type == 641 || (projectile.type >= 646 && projectile.type <= 649) || projectile.type == 653 || projectile.type == 186)
			{
				Vector2 position10 = projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
				Texture2D texture2D24 = Main.projectileTexture[projectile.type];
				Microsoft.Xna.Framework.Color alpha4 = projectile.GetAlpha(color25);
				Vector2 origin4 = new Vector2((float)texture2D24.Width, (float)texture2D24.Height) / 2f;
				if (projectile.type == 446)
				{
					origin4.Y = 4f;
				}
				if (projectile.type == 502)
				{
					//this.LoadProjectile(250);
					Texture2D texture2D25 = Main.projectileTexture[250];
					Vector2 origin5 = new Vector2((float)(texture2D25.Width / 2), 0f);
					Vector2 value36 = new Vector2((float)projectile.width, (float)projectile.height) / 2f;
					Microsoft.Xna.Framework.Color white2 = Microsoft.Xna.Framework.Color.White;
					white2.A = 127;
					for (int num240 = projectile.oldPos.Length - 1; num240 > 0; num240--)
					{
						Vector2 vector35 = projectile.oldPos[num240] + value36;
						if (!(vector35 == value36))
						{
							Vector2 vector36 = projectile.oldPos[num240 - 1] + value36;
							float rotation25 = (vector36 - vector35).ToRotation() - 1.57079637f;
							Vector2 scale6 = new Vector2(1f, Vector2.Distance(vector35, vector36) / (float)texture2D25.Height);
							Microsoft.Xna.Framework.Color color40 = white2 * (1f - (float)num240 / (float)projectile.oldPos.Length);
							Main.spriteBatch.Draw(texture2D25, vector35 - Main.screenPosition, null, color40, rotation25, origin5, scale6, spriteEffects, 0f);
						}
					}
				}
				else if (projectile.type == 540 && projectile.velocity != Vector2.Zero)
				{
					float num241 = 0f;
					if (projectile.ai[0] >= 10f)
					{
						num241 = (projectile.ai[0] - 10f) / 10f;
					}
					if (projectile.ai[0] >= 20f)
					{
						num241 = (20f - projectile.ai[0]) / 10f;
					}
					if (num241 > 1f)
					{
						num241 = 1f;
					}
					if (num241 < 0f)
					{
						num241 = 0f;
					}
					if (num241 != 0f)
					{
						Texture2D texture2D26 = Main.extraTexture[47];
						Vector2 origin6 = new Vector2((float)(texture2D26.Width / 2), 0f);
						Microsoft.Xna.Framework.Color color41 = alpha4 * num241 * 0.7f;
						Vector2 vector37 = projectile.Center - Main.screenPosition;
						Vector2 value37 = projectile.velocity.ToRotation().ToRotationVector2() * (float)texture2D24.Width / 3f;
						value37 = Vector2.Zero;
						vector37 += value37;
						float rotation26 = projectile.velocity.ToRotation() - 1.57079637f;
						Vector2 scale7 = new Vector2(1f, (projectile.velocity.Length() - value37.Length() * 2f) / (float)texture2D26.Height);
						Main.spriteBatch.Draw(texture2D26, vector37, null, color41, rotation26, origin6, scale7, SpriteEffects.None, 0f);
					}
				}
				if (projectile.type == 578 || projectile.type == 579 || projectile.type == 641)
				{
					Microsoft.Xna.Framework.Color color42 = alpha4 * 0.8f;
					color42.A /= 2;
					Microsoft.Xna.Framework.Color color43 = Microsoft.Xna.Framework.Color.Lerp(alpha4, Microsoft.Xna.Framework.Color.Black, 0.5f);
					color43.A = alpha4.A;
					float num242 = 0.95f + (projectile.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
					color43 *= num242;
					float scale8 = 0.6f + projectile.scale * 0.6f * num242;
					Main.spriteBatch.Draw(Main.extraTexture[50], position10, null, color43, -projectile.rotation + 0.35f, origin4, scale8, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
					Main.spriteBatch.Draw(Main.extraTexture[50], position10, null, alpha4, -projectile.rotation, origin4, projectile.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
					Main.spriteBatch.Draw(texture2D24, position10, null, color42, -projectile.rotation * 0.7f, origin4, projectile.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
					Main.spriteBatch.Draw(Main.extraTexture[50], position10, null, alpha4 * 0.8f, projectile.rotation * 0.5f, origin4, projectile.scale * 0.9f, spriteEffects, 0f);
					alpha4.A = 0;
				}
				if (projectile.type == 617)
				{
					Microsoft.Xna.Framework.Color color44 = alpha4 * 0.8f;
					color44.A /= 2;
					Microsoft.Xna.Framework.Color color45 = Microsoft.Xna.Framework.Color.Lerp(alpha4, Microsoft.Xna.Framework.Color.Black, 0.5f);
					color45.A = alpha4.A;
					float num243 = 0.95f + (projectile.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
					color45 *= num243;
					float scale9 = 0.6f + projectile.scale * 0.6f * num243;
					Main.spriteBatch.Draw(Main.extraTexture[50], position10, null, color45, -projectile.rotation + 0.35f, origin4, scale9, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
					Main.spriteBatch.Draw(Main.extraTexture[50], position10, null, alpha4, -projectile.rotation, origin4, projectile.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
					Main.spriteBatch.Draw(texture2D24, position10, null, color44, -projectile.rotation * 0.7f, origin4, projectile.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
					Main.spriteBatch.Draw(Main.extraTexture[50], position10, null, alpha4 * 0.8f, projectile.rotation * 0.5f, origin4, projectile.scale * 0.9f, spriteEffects, 0f);
					alpha4.A = 0;
				}
				bool flag25 = false;
				if (!(flag25 | (projectile.type == 464 && projectile.ai[1] != 1f)))
				{
					Main.spriteBatch.Draw(texture2D24, position10, null, alpha4, projectile.rotation, origin4, projectile.scale, spriteEffects, 0f);
				}
				if (projectile.type == 464 && projectile.ai[1] != 1f)
				{
					texture2D24 = Main.extraTexture[35];
					Microsoft.Xna.Framework.Rectangle rectangle10 = texture2D24.Frame(1, 3, 0, 0);
					origin4 = rectangle10.Size() / 2f;
					Vector2 value38 = new Vector2(0f, -720f).RotatedBy((double)projectile.velocity.ToRotation(), default(Vector2));
					float scaleFactor3 = projectile.ai[0] % 45f / 45f;
					Vector2 spinningpoint3 = value38 * scaleFactor3;
					for (int num244 = 0; num244 < 6; num244++)
					{
						float num245 = (float)num244 * 6.28318548f / 6f;
						Vector2 value39 = projectile.Center + spinningpoint3.RotatedBy((double)num245, default(Vector2));
						Main.spriteBatch.Draw(texture2D24, value39 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(rectangle10), alpha4, num245 + projectile.velocity.ToRotation() + 3.14159274f, origin4, projectile.scale, spriteEffects, 0f);
						rectangle10.Y += rectangle10.Height;
						if (rectangle10.Y >= texture2D24.Height)
						{
							rectangle10.Y = 0;
						}
					}
				}
				else if (projectile.type == 490)
				{
					Main.spriteBatch.Draw(Main.extraTexture[34], position10, null, alpha4, -projectile.rotation, Main.extraTexture[34].Size() / 2f, projectile.scale, spriteEffects, 0f);
					Main.spriteBatch.Draw(texture2D24, position10, null, alpha4, projectile.rotation, origin4, projectile.scale * 0.42f, spriteEffects, 0f);
					Main.spriteBatch.Draw(Main.extraTexture[34], position10, null, alpha4, -projectile.rotation, Main.extraTexture[34].Size() / 2f, projectile.scale * 0.42f, spriteEffects, 0f);
				}
				else if (projectile.type == 616)
				{
					texture2D24 = Main.glowMaskTexture[193];
					Main.spriteBatch.Draw(texture2D24, position10, null, new Microsoft.Xna.Framework.Color(127, 127, 127, 0), projectile.rotation, origin4, projectile.scale, spriteEffects, 0f);
				}
				else if (projectile.type >= 646 && projectile.type <= 649)
				{
					texture2D24 = Main.glowMaskTexture[203 + projectile.type - 646];
					Main.spriteBatch.Draw(texture2D24, position10, null, new Microsoft.Xna.Framework.Color(255, 255, 255, 127), projectile.rotation, origin4, projectile.scale, spriteEffects, 0f);
				}
			}
			else if (projectile.type == 465 || projectile.type == 467 || projectile.type == 468 || projectile.type == 500 || projectile.type == 518 || projectile.type == 535 || projectile.type == 539 || projectile.type == 575 || projectile.type == 574 || projectile.type == 589 || projectile.type == 590 || projectile.type == 593 || projectile.type == 602 || projectile.type == 596 || projectile.type == 612 || projectile.type == 613 || projectile.type == 614 || projectile.type == 623 || projectile.type == 625 || projectile.type == 626 || projectile.type == 627 || projectile.type == 628 || projectile.type == 634 || projectile.type == 635 || projectile.type == 643 || projectile.type == 644 || projectile.type == 645 || projectile.type == 650 || projectile.type == 652 || projectile.type == 658 || projectile.type == 659)
			{
				Vector2 vector38 = projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
				Texture2D texture2D27 = Main.projectileTexture[projectile.type];
				Microsoft.Xna.Framework.Rectangle rectangle11 = texture2D27.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
				Microsoft.Xna.Framework.Color alpha5 = projectile.GetAlpha(color25);
				Vector2 origin7 = rectangle11.Size() / 2f;
				if (projectile.type == 539)
				{
					if (projectile.ai[0] >= 210f)
					{
						float num246 = projectile.ai[0] - 210f;
						num246 /= 20f;
						if (num246 > 1f)
						{
							num246 = 1f;
						}
						Main.spriteBatch.Draw(Main.extraTexture[46], vector38, null, new Microsoft.Xna.Framework.Color(255, 255, 255, 128) * num246, projectile.rotation, new Vector2(17f, 22f), projectile.scale, spriteEffects, 0f);
					}
				}
				else if (projectile.type == 602)
				{
					origin7.X = (float)(rectangle11.Width - 6);
					origin7.Y -= 1f;
					rectangle11.Height -= 2;
				}
				else if (projectile.type == 589)
				{
					rectangle11 = texture2D27.Frame(5, 1, (int)projectile.ai[1], 0);
					origin7 = rectangle11.Size() / 2f;
				}
				else if (projectile.type == 590)
				{
					rectangle11 = texture2D27.Frame(3, 1, projectile.frame, 0);
					origin7 = rectangle11.Size() / 2f;
				}
				else if (projectile.type == 650)
				{
					origin7.Y -= 4f;
				}
				else if (projectile.type == 623)
				{
					alpha5.A /= 2;
				}
				else if (projectile.type >= 625 && projectile.type <= 628)
				{
					alpha5.A /= 2;
				}
				else if (projectile.type == 644)
				{
					Microsoft.Xna.Framework.Color color46 = Main.hslToRgb(projectile.ai[0], 1f, 0.5f).MultiplyRGBA(new Microsoft.Xna.Framework.Color(255, 255, 255, 0));
					Main.spriteBatch.Draw(texture2D27, vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), color46, projectile.rotation, origin7, projectile.scale * 2f, spriteEffects, 0f);
					Main.spriteBatch.Draw(texture2D27, vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), color46, 0f, origin7, projectile.scale * 2f, spriteEffects, 0f);
					if (projectile.ai[1] != -1f && projectile.Opacity > 0.3f)
					{
						Vector2 vector39 = Main.projectile[(int)projectile.ai[1]].Center - projectile.Center;
						Vector2 vector40 = new Vector2(1f, vector39.Length() / (float)texture2D27.Height);
						float rotation27 = vector39.ToRotation() + 1.57079637f;
						float num247 = MathHelper.Distance(30f, projectile.localAI[1]) / 20f;
						num247 = MathHelper.Clamp(num247, 0f, 1f);
						if (num247 > 0f)
						{
							Main.spriteBatch.Draw(texture2D27, vector38 + vector39 / 2f, new Microsoft.Xna.Framework.Rectangle?(rectangle11), color46 * num247, rotation27, origin7, vector40, spriteEffects, 0f);
							Main.spriteBatch.Draw(texture2D27, vector38 + vector39 / 2f, new Microsoft.Xna.Framework.Rectangle?(rectangle11), alpha5 * num247, rotation27, origin7, vector40 / 2f, spriteEffects, 0f);
						}
					}
				}
				else if (projectile.type == 658)
				{
					Microsoft.Xna.Framework.Color color47 = Main.hslToRgb(0.136f, 1f, 0.5f).MultiplyRGBA(new Microsoft.Xna.Framework.Color(255, 255, 255, 0));
					Main.spriteBatch.Draw(texture2D27, vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), color47, 0f, origin7, new Vector2(1f, 5f) * projectile.scale * 2f, spriteEffects, 0f);
				}
				Main.spriteBatch.Draw(texture2D27, vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), alpha5, projectile.rotation, origin7, projectile.scale, spriteEffects, 0f);
				if (projectile.type == 535)
				{
					for (int num248 = 0; num248 < 1000; num248++)
					{
						if (Main.projectile[num248].active && Main.projectile[num248].owner == projectile.owner && Main.projectile[num248].type == 536)
						{
							DrawProj(num248);
						}
					}
				}
				else if (projectile.type == 644)
				{
					Main.spriteBatch.Draw(texture2D27, vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), alpha5, 0f, origin7, projectile.scale, spriteEffects, 0f);
				}
				else if (projectile.type == 658)
				{
					Main.spriteBatch.Draw(texture2D27, vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), alpha5, 0f, origin7, new Vector2(1f, 8f) * projectile.scale, spriteEffects, 0f);
				}
				else if (projectile.type == 602)
				{
					texture2D27 = Main.extraTexture[60];
					Microsoft.Xna.Framework.Color color48 = alpha5;
					color48.A = 0;
					color48 *= 0.3f;
					origin7 = texture2D27.Size() / 2f;
					Main.spriteBatch.Draw(texture2D27, vector38, null, color48, projectile.rotation - 1.57079637f, origin7, projectile.scale, spriteEffects, 0f);
					texture2D27 = Main.extraTexture[59];
					color48 = alpha5;
					color48.A = 0;
					color48 *= 0.13f;
					origin7 = texture2D27.Size() / 2f;
					Main.spriteBatch.Draw(texture2D27, vector38, null, color48, projectile.rotation - 1.57079637f, origin7, projectile.scale * 0.9f, spriteEffects, 0f);
				}
				else if (projectile.type == 539)
				{
					Main.spriteBatch.Draw(Main.glowMaskTexture[140], vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), projectile.rotation, origin7, projectile.scale, spriteEffects, 0f);
				}
				else if (projectile.type == 613)
				{
					Main.spriteBatch.Draw(Main.glowMaskTexture[189], vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), new Microsoft.Xna.Framework.Color(128 - projectile.alpha / 2, 128 - projectile.alpha / 2, 128 - projectile.alpha / 2, 0), projectile.rotation, origin7, projectile.scale, spriteEffects, 0f);
				}
				else if (projectile.type == 614)
				{
					Main.spriteBatch.Draw(Main.glowMaskTexture[190], vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), new Microsoft.Xna.Framework.Color(128 - projectile.alpha / 2, 128 - projectile.alpha / 2, 128 - projectile.alpha / 2, 0), projectile.rotation, origin7, projectile.scale, spriteEffects, 0f);
				}
				else if (projectile.type == 574)
				{
					Main.spriteBatch.Draw(Main.glowMaskTexture[148], vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), projectile.rotation, origin7, projectile.scale, spriteEffects, 0f);
				}
				else if (projectile.type == 590)
				{
					Main.spriteBatch.Draw(Main.glowMaskTexture[168], vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), new Microsoft.Xna.Framework.Color(127 - projectile.alpha / 2, 127 - projectile.alpha / 2, 127 - projectile.alpha / 2, 0), projectile.rotation, origin7, projectile.scale, spriteEffects, 0f);
				}
				else if (projectile.type == 623 || (projectile.type >= 625 && projectile.type <= 628))
				{
					if (Main.player[projectile.owner].ghostFade != 0f)
					{
						float scaleFactor4 = Main.player[projectile.owner].ghostFade * 5f;
						for (float num249 = 0f; num249 < 4f; num249 += 1f)
						{
							Main.spriteBatch.Draw(texture2D27, vector38 + Vector2.UnitY.RotatedBy((double)(num249 * 6.28318548f / 4f), default(Vector2)) * scaleFactor4, new Microsoft.Xna.Framework.Rectangle?(rectangle11), alpha5 * 0.1f, projectile.rotation, origin7, projectile.scale, spriteEffects, 0f);
						}
					}
				}
				else if (projectile.type == 643)
				{
					float scaleFactor5 = (float)Math.Cos((double)(6.28318548f * (projectile.localAI[0] / 60f))) + 3f + 3f;
					for (float num250 = 0f; num250 < 4f; num250 += 1f)
					{
						Main.spriteBatch.Draw(texture2D27, vector38 + Vector2.UnitY.RotatedBy((double)(num250 * 1.57079637f), default(Vector2)) * scaleFactor5, new Microsoft.Xna.Framework.Rectangle?(rectangle11), alpha5 * 0.2f, projectile.rotation, origin7, projectile.scale, spriteEffects, 0f);
					}
				}
				else if (projectile.type == 650)
				{
					int num251 = (int)(projectile.localAI[0] / 6.28318548f);
					float f = projectile.localAI[0] % 6.28318548f - 3.14159274f;
					float num252 = (float)Math.IEEERemainder((double)projectile.localAI[1], 1.0);
					if (num252 < 0f)
					{
						num252 += 1f;
					}
					int num253 = (int)Math.Floor((double)projectile.localAI[1]);
					float scaleFactor6 = 5f;
					float scale10 = 1f + (float)num253 * 0.02f;
					if ((float)num251 == 1f)
					{
						scaleFactor6 = 7f;
					}
					Vector2 value40 = f.ToRotationVector2() * num252 * scaleFactor6 * projectile.scale;
					texture2D27 = Main.extraTexture[66];
					Main.spriteBatch.Draw(texture2D27, vector38 + value40, null, alpha5, projectile.rotation, texture2D27.Size() / 2f, scale10, SpriteEffects.None, 0f);
				}
			}
			else if (projectile.type == 466)
			{
				Vector2 end = projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
				Texture2D tex3 = Main.extraTexture[33];
				projectile.GetAlpha(color25);
				Vector2 scale11 = new Vector2(projectile.scale) / 2f;
				for (int num254 = 0; num254 < 3; num254++)
				{
					if (num254 == 0)
					{
						scale11 = new Vector2(projectile.scale) * 0.6f;
						DelegateMethods.c_1 = new Microsoft.Xna.Framework.Color(115, 204, 219, 0) * 0.5f;
					}
					else if (num254 == 1)
					{
						scale11 = new Vector2(projectile.scale) * 0.4f;
						DelegateMethods.c_1 = new Microsoft.Xna.Framework.Color(113, 251, 255, 0) * 0.5f;
					}
					else
					{
						scale11 = new Vector2(projectile.scale) * 0.2f;
						DelegateMethods.c_1 = new Microsoft.Xna.Framework.Color(255, 255, 255, 0) * 0.5f;
					}
					DelegateMethods.f_1 = 1f;
					for (int num255 = projectile.oldPos.Length - 1; num255 > 0; num255--)
					{
						if (!(projectile.oldPos[num255] == Vector2.Zero))
						{
							Vector2 start = projectile.oldPos[num255] + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
							Vector2 end2 = projectile.oldPos[num255 - 1] + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
							Utils.DrawLaser(Main.spriteBatch, tex3, start, end2, scale11, new Utils.LaserLineFraming(DelegateMethods.LightningLaserDraw));
						}
					}
					if (projectile.oldPos[0] != Vector2.Zero)
					{
						DelegateMethods.f_1 = 1f;
						Vector2 start2 = projectile.oldPos[0] + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
						Utils.DrawLaser(Main.spriteBatch, tex3, start2, end, scale11, new Utils.LaserLineFraming(DelegateMethods.LightningLaserDraw));
					}
				}
			}
			else if (projectile.type == 580)
			{
				Vector2 end3 = projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
				Texture2D tex4 = Main.extraTexture[33];
				projectile.GetAlpha(color25);
				Vector2 scale12 = new Vector2(projectile.scale) / 2f;
				for (int num256 = 0; num256 < 2; num256++)
				{
					float num257 = (projectile.localAI[1] == -1f || projectile.localAI[1] == 1f) ? -0.2f : 0f;
					if (num256 == 0)
					{
						scale12 = new Vector2(projectile.scale) * (0.5f + num257);
						DelegateMethods.c_1 = new Microsoft.Xna.Framework.Color(115, 244, 219, 0) * 0.5f;
					}
					else
					{
						scale12 = new Vector2(projectile.scale) * (0.3f + num257);
						DelegateMethods.c_1 = new Microsoft.Xna.Framework.Color(255, 255, 255, 0) * 0.5f;
					}
					DelegateMethods.f_1 = 1f;
					for (int num258 = projectile.oldPos.Length - 1; num258 > 0; num258--)
					{
						if (!(projectile.oldPos[num258] == Vector2.Zero))
						{
							Vector2 start3 = projectile.oldPos[num258] + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
							Vector2 end4 = projectile.oldPos[num258 - 1] + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
							Utils.DrawLaser(Main.spriteBatch, tex4, start3, end4, scale12, new Utils.LaserLineFraming(DelegateMethods.LightningLaserDraw));
						}
					}
					if (projectile.oldPos[0] != Vector2.Zero)
					{
						DelegateMethods.f_1 = 1f;
						Vector2 start4 = projectile.oldPos[0] + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
						Utils.DrawLaser(Main.spriteBatch, tex4, start4, end3, scale12, new Utils.LaserLineFraming(DelegateMethods.LightningLaserDraw));
					}
				}
			}
			else if (projectile.type == 445)
			{
				Vector2 vector41 = projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
				Texture2D texture2D28 = Main.projectileTexture[projectile.type];
				Microsoft.Xna.Framework.Color alpha6 = projectile.GetAlpha(color25);
				Vector2 vector42 = Main.player[projectile.owner].RotatedRelativePoint(mountedCenter, true) + Vector2.UnitY * Main.player[projectile.owner].gfxOffY;
				Vector2 vector43 = vector41 + Main.screenPosition - vector42;
				Vector2 value41 = Vector2.Normalize(vector43);
				float num259 = vector43.Length();
				float num260 = vector43.ToRotation() + 1.57079637f;
				float num261 = -5f;
				float num262 = num261 + 30f;
				new Vector2(2f, num259 - num262);
				Vector2 value42 = Vector2.Lerp(vector41 + Main.screenPosition, vector42 + value41 * num262, 0.5f);
				Vector2 vector44 = -Vector2.UnitY.RotatedBy((double)(projectile.localAI[0] / 60f * 3.14159274f), default(Vector2));
				Vector2[] array7 = new Vector2[]
				{
					vector44,
					vector44.RotatedBy(1.5707963705062866, default(Vector2)),
					vector44.RotatedBy(3.1415927410125732, default(Vector2)),
					vector44.RotatedBy(4.71238911151886, default(Vector2))
				};
				if (num259 > num262)
				{
					for (int num263 = 0; num263 < 2; num263++)
					{
						Microsoft.Xna.Framework.Color color49 = Microsoft.Xna.Framework.Color.White;
						if (num263 % 2 == 0)
						{
							color49 = Microsoft.Xna.Framework.Color.LimeGreen;
							color49.A = 128;
							color49 *= 0.5f;
						}
						else
						{
							color49 = Microsoft.Xna.Framework.Color.CornflowerBlue;
							color49.A = 128;
							color49 *= 0.5f;
						}
						Vector2 value43 = new Vector2(array7[num263].X, 0f).RotatedBy((double)num260, default(Vector2)) * 4f;
						Main.spriteBatch.Draw(Main.magicPixel, value42 - Main.screenPosition + value43, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 1, 1)), color49, num260, Vector2.One / 2f, new Vector2(2f, num259 - num262), spriteEffects, 0f);
					}
				}
				Texture2D texture2D29 = Main.itemTexture[Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].type];
				Microsoft.Xna.Framework.Color color50 = Lighting.GetColor((int)vector42.X / 16, (int)vector42.Y / 16);
				Main.spriteBatch.Draw(texture2D29, vector42 - Main.screenPosition + value41 * num261, null, color50, projectile.rotation + 1.57079637f + ((spriteEffects == SpriteEffects.None) ? 3.14159274f : 0f), new Vector2((float)((spriteEffects == SpriteEffects.None) ? 0 : texture2D29.Width), (float)texture2D29.Height / 2f) + Vector2.UnitY * 1f, Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].scale, spriteEffects, 0f);
				Main.spriteBatch.Draw(Main.glowMaskTexture[39], vector42 - Main.screenPosition + value41 * num261, null, new Microsoft.Xna.Framework.Color(255, 255, 255, 0), projectile.rotation + 1.57079637f + ((spriteEffects == SpriteEffects.None) ? 3.14159274f : 0f), new Vector2((float)((spriteEffects == SpriteEffects.None) ? 0 : texture2D29.Width), (float)texture2D29.Height / 2f) + Vector2.UnitY * 1f, Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].scale, spriteEffects, 0f);
				if (num259 > num262)
				{
					for (int num264 = 2; num264 < 4; num264++)
					{
						Microsoft.Xna.Framework.Color color51 = Microsoft.Xna.Framework.Color.White;
						if (num264 % 2 == 0)
						{
							color51 = Microsoft.Xna.Framework.Color.LimeGreen;
							color51.A = 128;
							color51 *= 0.5f;
						}
						else
						{
							color51 = Microsoft.Xna.Framework.Color.CornflowerBlue;
							color51.A = 128;
							color51 *= 0.5f;
						}
						Vector2 value44 = new Vector2(array7[num264].X, 0f).RotatedBy((double)num260, default(Vector2)) * 4f;
						Main.spriteBatch.Draw(Main.magicPixel, value42 - Main.screenPosition + value44, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 1, 1)), color51, num260, Vector2.One / 2f, new Vector2(2f, num259 - num262), spriteEffects, 0f);
					}
				}
				float num265 = projectile.localAI[0] / 60f;
				if (num265 > 0.5f)
				{
					num265 = 1f - num265;
				}
				Main.spriteBatch.Draw(texture2D28, vector41, null, alpha6 * num265 * 2f, projectile.rotation, new Vector2((float)texture2D28.Width, (float)texture2D28.Height) / 2f, projectile.scale, spriteEffects, 0f);
				Main.spriteBatch.Draw(Main.glowMaskTexture[40], vector41, null, alpha6 * (0.5f - num265) * 2f, projectile.rotation, new Vector2((float)texture2D28.Width, (float)texture2D28.Height) / 2f, projectile.scale, spriteEffects, 0f);
			}
			else if ((projectile.type >= 393 && projectile.type <= 395) || projectile.type == 398 || projectile.type == 423 || projectile.type == 450)
			{
				Texture2D texture2D30 = Main.projectileTexture[projectile.type];
				int num266 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				int y11 = num266 * projectile.frame;
				Main.spriteBatch.Draw(texture2D30, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY - 2f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y11, texture2D30.Width, num266)), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)texture2D30.Width / 2f, (float)num266 / 2f), projectile.scale, spriteEffects, 0f);
				if (projectile.type == 398)
				{
					texture2D30 = Main.miniMinotaurTexture;
					Main.spriteBatch.Draw(texture2D30, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY - 2f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y11, texture2D30.Width, num266)), new Microsoft.Xna.Framework.Color(250, 250, 250, projectile.alpha), projectile.rotation, new Vector2((float)texture2D30.Width / 2f, (float)num266 / 2f), projectile.scale, spriteEffects, 0f);
				}
				if (projectile.type == 423)
				{
					texture2D30 = Main.glowMaskTexture[0];
					Main.spriteBatch.Draw(texture2D30, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY - 2f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y11, texture2D30.Width, num266)), new Microsoft.Xna.Framework.Color(250, 250, 250, projectile.alpha), projectile.rotation, new Vector2((float)texture2D30.Width / 2f, (float)num266 / 2f), projectile.scale, spriteEffects, 0f);
				}
			}
			else if (projectile.type == 385)
			{
				Texture2D texture2D31 = Main.projectileTexture[projectile.type];
				int num267 = texture2D31.Height / Main.projFrames[projectile.type];
				int y12 = num267 * projectile.frame;
				int num268 = 8;
				int num269 = 2;
				float value45 = 0.4f;
				for (int num270 = 1; num270 < num268; num270 += num269)
				{
					//Vector2 arg_D72B_0 = projectile.oldPos[num270];
					Microsoft.Xna.Framework.Color color52 = color25;
					color52 = projectile.GetAlpha(color52);
					color52 *= (float)(num268 - num270) / 15f;
					Microsoft.Xna.Framework.Color alpha7 = projectile.GetAlpha(color25);
					//projectile.oldPos[num270] - Main.screenPosition + new Vector2(num149 + (float)num148, (float)(projectile.height / 2) + projectile.gfxOffY);
					Main.spriteBatch.Draw(texture2D31, projectile.oldPos[num270] + new Vector2((float)projectile.width, (float)projectile.height) / 2f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y12, texture2D31.Width, num267)), Microsoft.Xna.Framework.Color.Lerp(alpha7, color52, 0.3f), projectile.rotation, new Vector2((float)texture2D31.Width / 2f, (float)num267 / 2f), MathHelper.Lerp(projectile.scale, value45, (float)num270 / 15f), spriteEffects, 0f);
				}
				Main.spriteBatch.Draw(texture2D31, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y12, texture2D31.Width, num267)), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)texture2D31.Width / 2f, (float)num267 / 2f), projectile.scale, spriteEffects, 0f);
			}
			else if (projectile.type == 388)
			{
				Texture2D texture2D32 = Main.projectileTexture[projectile.type];
				int num271 = texture2D32.Height / Main.projFrames[projectile.type];
				int y13 = num271 * projectile.frame;
				int num272;
				int num273;
				if (projectile.ai[0] == 2f)
				{
					num272 = 10;
					num273 = 1;
				}
				else
				{
					num273 = 2;
					num272 = 5;
				}
				for (int num274 = 1; num274 < num272; num274 += num273)
				{
					//Vector2 arg_D9D4_0 = Main.npc[i].oldPos[num274];
					Microsoft.Xna.Framework.Color color53 = color25;
					color53 = projectile.GetAlpha(color53);
					color53 *= (float)(num272 - num274) / 15f;
					Vector2 position11 = projectile.oldPos[num274] - Main.screenPosition + new Vector2(num149 + (float)num148, (float)(projectile.height / 2) + projectile.gfxOffY);
					Main.spriteBatch.Draw(texture2D32, position11, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y13, texture2D32.Width, num271)), color53, projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
				}
				Main.spriteBatch.Draw(texture2D32, projectile.position - Main.screenPosition + new Vector2(num149 + (float)num148, (float)(projectile.height / 2) + projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y13, texture2D32.Width, num271)), projectile.GetAlpha(color25), projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
			}
			else if (Main.projFrames[projectile.type] > 1)
			{
				int num275 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				int y14 = num275 * projectile.frame;
				if (projectile.type == 111)
				{
					int r = (int)Main.player[projectile.owner].shirtColor.R;
					int g = (int)Main.player[projectile.owner].shirtColor.G;
					int b = (int)Main.player[projectile.owner].shirtColor.B;
					Microsoft.Xna.Framework.Color oldColor = new Microsoft.Xna.Framework.Color((int)((byte)r), (int)((byte)g), (int)((byte)b));
					color25 = Lighting.GetColor((int)((double)projectile.position.X + (double)projectile.width * 0.5) / 16, (int)(((double)projectile.position.Y + (double)projectile.height * 0.5) / 16.0), oldColor);
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y14, Main.projectileTexture[projectile.type].Width, num275)), projectile.GetAlpha(color25), projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
				}
				else
				{
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y14, Main.projectileTexture[projectile.type].Width, num275 - 1)), projectile.GetAlpha(color25), projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
					if (projectile.type == 387)
					{
						Main.spriteBatch.Draw(Main.eyeLaserSmallTexture, new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y14, Main.projectileTexture[projectile.type].Width, num275)), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
					}
				}
			}
			else if (projectile.type == 383 || projectile.type == 399)
			{
				Texture2D texture2D33 = Main.projectileTexture[projectile.type];
				Main.spriteBatch.Draw(texture2D33, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)texture2D33.Width, (float)texture2D33.Height) / 2f, projectile.scale, spriteEffects, 0f);
			}
			else if (projectile.type == 157 || projectile.type == 378)
			{
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + (float)(projectile.width / 2), projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)(Main.projectileTexture[projectile.type].Width / 2), (float)(Main.projectileTexture[projectile.type].Height / 2)), projectile.scale, spriteEffects, 0f);
			}
			else if (projectile.type == 306)
			{
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + (float)(projectile.width / 2), projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)(Main.projectileTexture[projectile.type].Width / 2), (float)(Main.projectileTexture[projectile.type].Height / 2)), projectile.scale, spriteEffects, 0f);
			}
			else if (projectile.type == 256)
			{
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + (float)(projectile.width / 2), projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)(Main.projectileTexture[projectile.type].Width / 2), (float)(Main.projectileTexture[projectile.type].Height / 2)), projectile.scale, spriteEffects, 0f);
			}
			else if (projectile.aiStyle == 27)
			{
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + (float)(projectile.width / 2), projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)Main.projectileTexture[projectile.type].Width, 0f), projectile.scale, spriteEffects, 0f);
			}
			else if (projectile.aiStyle == 19)
			{
				Vector2 zero = Vector2.Zero;
				if (projectile.spriteDirection == -1)
				{
					zero.X = (float)Main.projectileTexture[projectile.type].Width;
				}
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + (float)(projectile.width / 2), projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), projectile.GetAlpha(color25), projectile.rotation, zero, projectile.scale, spriteEffects, 0f);
			}
			else if (projectile.type == 451)
			{
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, null, projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)Main.projectileTexture[projectile.type].Width, 0f), projectile.scale, spriteEffects, 0f);
			}
			else if (projectile.type == 434)
			{
				Vector2 value46 = new Vector2(projectile.ai[0], projectile.ai[1]);
				Vector2 v = projectile.position - value46;
				float num276 = (float)Math.Sqrt((double)(v.X * v.X + v.Y * v.Y));
				new Vector2(4f, num276);
				float rotation28 = v.ToRotation() + 1.57079637f;
				Vector2 value47 = Vector2.Lerp(projectile.position, value46, 0.5f);
				Microsoft.Xna.Framework.Color color54 = Microsoft.Xna.Framework.Color.Red;
				color54.A = 0;
				Microsoft.Xna.Framework.Color color55 = Microsoft.Xna.Framework.Color.White;
				color54 *= projectile.localAI[0];
				color55 *= projectile.localAI[0];
				float num277 = (float)Math.Sqrt((double)(projectile.damage / 50));
				Main.spriteBatch.Draw(Main.magicPixel, value47 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 1, 1)), color54, rotation28, Vector2.One / 2f, new Vector2(2f * num277, num276 + 8f), spriteEffects, 0f);
				Main.spriteBatch.Draw(Main.magicPixel, value47 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 1, 1)), color54, rotation28, Vector2.One / 2f, new Vector2(4f * num277, num276), spriteEffects, 0f);
				Main.spriteBatch.Draw(Main.magicPixel, value47 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 1, 1)), color55, rotation28, Vector2.One / 2f, new Vector2(2f * num277, num276), spriteEffects, 0f);
			}
			else
			{
				if (projectile.type == 94 && projectile.ai[1] > 6f)
				{
					for (int num278 = 0; num278 < 10; num278++)
					{
						Microsoft.Xna.Framework.Color alpha8 = projectile.GetAlpha(color25);
						float num279 = (float)(9 - num278) / 9f;
						alpha8.R = (byte)((float)alpha8.R * num279);
						alpha8.G = (byte)((float)alpha8.G * num279);
						alpha8.B = (byte)((float)alpha8.B * num279);
						alpha8.A = (byte)((float)alpha8.A * num279);
						float num280 = (float)(9 - num278) / 9f;
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.oldPos[num278].X - Main.screenPosition.X + num149 + (float)num148, projectile.oldPos[num278].Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), alpha8, projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), num280 * projectile.scale, spriteEffects, 0f);
					}
				}
				if (projectile.type == 301)
				{
					for (int num281 = 0; num281 < 10; num281++)
					{
						Microsoft.Xna.Framework.Color alpha9 = projectile.GetAlpha(color25);
						float num282 = (float)(9 - num281) / 9f;
						alpha9.R = (byte)((float)alpha9.R * num282);
						alpha9.G = (byte)((float)alpha9.G * num282);
						alpha9.B = (byte)((float)alpha9.B * num282);
						alpha9.A = (byte)((float)alpha9.A * num282);
						float num283 = (float)(9 - num281) / 9f;
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.oldPos[num281].X - Main.screenPosition.X + num149 + (float)num148, projectile.oldPos[num281].Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), alpha9, projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), num283 * projectile.scale, spriteEffects, 0f);
					}
				}
				if (projectile.type == 323 && projectile.alpha == 0)
				{
					for (int num284 = 1; num284 < 8; num284++)
					{
						float num285 = projectile.velocity.X * (float)num284;
						float num286 = projectile.velocity.Y * (float)num284;
						Microsoft.Xna.Framework.Color alpha10 = projectile.GetAlpha(color25);
						float num287 = 0f;
						if (num284 == 1)
						{
							num287 = 0.7f;
						}
						if (num284 == 2)
						{
							num287 = 0.6f;
						}
						if (num284 == 3)
						{
							num287 = 0.5f;
						}
						if (num284 == 4)
						{
							num287 = 0.4f;
						}
						if (num284 == 5)
						{
							num287 = 0.3f;
						}
						if (num284 == 6)
						{
							num287 = 0.2f;
						}
						if (num284 == 7)
						{
							num287 = 0.1f;
						}
						alpha10.R = (byte)((float)alpha10.R * num287);
						alpha10.G = (byte)((float)alpha10.G * num287);
						alpha10.B = (byte)((float)alpha10.B * num287);
						alpha10.A = (byte)((float)alpha10.A * num287);
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148 - num285, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY - num286), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), alpha10, projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), num287 + 0.2f, spriteEffects, 0f);
					}
				}
				if (projectile.type == 117 && projectile.ai[0] > 3f)
				{
					for (int num288 = 1; num288 < 5; num288++)
					{
						float num289 = projectile.velocity.X * (float)num288;
						float num290 = projectile.velocity.Y * (float)num288;
						Microsoft.Xna.Framework.Color alpha11 = projectile.GetAlpha(color25);
						float num291 = 0f;
						if (num288 == 1)
						{
							num291 = 0.4f;
						}
						if (num288 == 2)
						{
							num291 = 0.3f;
						}
						if (num288 == 3)
						{
							num291 = 0.2f;
						}
						if (num288 == 4)
						{
							num291 = 0.1f;
						}
						alpha11.R = (byte)((float)alpha11.R * num291);
						alpha11.G = (byte)((float)alpha11.G * num291);
						alpha11.B = (byte)((float)alpha11.B * num291);
						alpha11.A = (byte)((float)alpha11.A * num291);
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148 - num289, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY - num290), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), alpha11, projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
					}
				}
				if (projectile.bobber)
				{
					if (projectile.ai[1] > 0f && projectile.ai[1] < 3797f && projectile.ai[0] == 1f)
					{
						int num292 = (int)projectile.ai[1];
						Vector2 center = projectile.Center;
						float num293 = projectile.rotation;
						Vector2 vector45 = center;
						float num294 = num - vector45.X;
						float num295 = num2 - vector45.Y;
						num293 = (float)Math.Atan2((double)num295, (double)num294);
						if (projectile.velocity.X > 0f)
						{
							spriteEffects = SpriteEffects.None;
							num293 = (float)Math.Atan2((double)num295, (double)num294);
							num293 += 0.785f;
							if (projectile.ai[1] == 2342f)
							{
								num293 -= 0.785f;
							}
						}
						else
						{
							spriteEffects = SpriteEffects.FlipHorizontally;
							num293 = (float)Math.Atan2((double)(-(double)num295), (double)(-(double)num294));
							num293 -= 0.785f;
							if (projectile.ai[1] == 2342f)
							{
								num293 += 0.785f;
							}
						}
						Main.spriteBatch.Draw(Main.itemTexture[num292], new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.itemTexture[num292].Width, Main.itemTexture[num292].Height)), color25, num293, new Vector2((float)(Main.itemTexture[num292].Width / 2), (float)(Main.itemTexture[num292].Height / 2)), projectile.scale, spriteEffects, 0f);
					}
					else if (projectile.ai[0] <= 1f)
					{
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), projectile.GetAlpha(color25), projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
					}
				}
				else
				{
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), projectile.GetAlpha(color25), projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
					if (projectile.glowMask != -1)
					{
						Main.spriteBatch.Draw(Main.glowMaskTexture[(int)projectile.glowMask], new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), new Microsoft.Xna.Framework.Color(250, 250, 250, projectile.alpha), projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
					}
					if (projectile.type == 473)
					{
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), new Microsoft.Xna.Framework.Color(255, 255, 0, 0), projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
					}
				}
				if (projectile.type == 106)
				{
					Main.spriteBatch.Draw(Main.lightDiscTexture, new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), new Microsoft.Xna.Framework.Color(200, 200, 200, 0), projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
				}
				if (projectile.type == 554 || projectile.type == 603)
				{
					for (int num296 = 1; num296 < 5; num296++)
					{
						float num297 = projectile.velocity.X * (float)num296 * 0.5f;
						float num298 = projectile.velocity.Y * (float)num296 * 0.5f;
						Microsoft.Xna.Framework.Color alpha12 = projectile.GetAlpha(color25);
						float num299 = 0f;
						if (num296 == 1)
						{
							num299 = 0.4f;
						}
						if (num296 == 2)
						{
							num299 = 0.3f;
						}
						if (num296 == 3)
						{
							num299 = 0.2f;
						}
						if (num296 == 4)
						{
							num299 = 0.1f;
						}
						alpha12.R = (byte)((float)alpha12.R * num299);
						alpha12.G = (byte)((float)alpha12.G * num299);
						alpha12.B = (byte)((float)alpha12.B * num299);
						alpha12.A = (byte)((float)alpha12.A * num299);
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148 - num297, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY - num298), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), alpha12, projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
					}
				}
				else if (projectile.type == 604)
				{
					int num300 = (int)projectile.ai[1] + 1;
					if (num300 > 7)
					{
						num300 = 7;
					}
					for (int num301 = 1; num301 < num300; num301++)
					{
						float num302 = projectile.velocity.X * (float)num301 * 1.5f;
						float num303 = projectile.velocity.Y * (float)num301 * 1.5f;
						Microsoft.Xna.Framework.Color alpha13 = projectile.GetAlpha(color25);
						if (num301 == 1)
						{
						}
						if (num301 == 2)
						{
						}
						if (num301 == 3)
						{
						}
						if (num301 == 4)
						{
						}
						float num304 = 0.4f - (float)num301 * 0.06f;
						num304 *= 1f - (float)projectile.alpha / 255f;
						alpha13.R = (byte)((float)alpha13.R * num304);
						alpha13.G = (byte)((float)alpha13.G * num304);
						alpha13.B = (byte)((float)alpha13.B * num304);
						alpha13.A = (byte)((float)alpha13.A * num304 / 2f);
						float num305 = projectile.scale;
						num305 -= (float)num301 * 0.1f;
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148 - num302, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY - num303), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), alpha13, projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), num305, spriteEffects, 0f);
					}
				}
				else if (projectile.type == 553)
				{
					for (int num306 = 1; num306 < 5; num306++)
					{
						float num307 = projectile.velocity.X * (float)num306 * 0.4f;
						float num308 = projectile.velocity.Y * (float)num306 * 0.4f;
						Microsoft.Xna.Framework.Color alpha14 = projectile.GetAlpha(color25);
						float num309 = 0f;
						if (num306 == 1)
						{
							num309 = 0.4f;
						}
						if (num306 == 2)
						{
							num309 = 0.3f;
						}
						if (num306 == 3)
						{
							num309 = 0.2f;
						}
						if (num306 == 4)
						{
							num309 = 0.1f;
						}
						alpha14.R = (byte)((float)alpha14.R * num309);
						alpha14.G = (byte)((float)alpha14.G * num309);
						alpha14.B = (byte)((float)alpha14.B * num309);
						alpha14.A = (byte)((float)alpha14.A * num309);
						Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.position.X - Main.screenPosition.X + num149 + (float)num148 - num307, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY - num308), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height)), alpha14, projectile.rotation, new Vector2(num149, (float)(projectile.height / 2 + num147)), projectile.scale, spriteEffects, 0f);
					}
				}
			}
			if (projectile.type == 525 && (!Main.gamePaused || Main.gameMenu))
			{
				Vector2 vector46 = projectile.position - Main.screenPosition;
				if ((float)Main.mouseX > vector46.X && (float)Main.mouseX < vector46.X + (float)projectile.width && (float)Main.mouseY > vector46.Y && (float)Main.mouseY < vector46.Y + (float)projectile.height)
				{
					int num310 = (int)(Main.player[Main.myPlayer].Center.X / 16f);
					int num311 = (int)(Main.player[Main.myPlayer].Center.Y / 16f);
					int num312 = (int)projectile.Center.X / 16;
					int num313 = (int)projectile.Center.Y / 16;
					int lastTileRangeX = Main.player[Main.myPlayer].lastTileRangeX;
					int lastTileRangeY = Main.player[Main.myPlayer].lastTileRangeY;
					if (num310 >= num312 - lastTileRangeX && num310 <= num312 + lastTileRangeX + 1 && num311 >= num313 - lastTileRangeY && num311 <= num313 + lastTileRangeY + 1)
					{
						Main.player[Main.myPlayer].noThrow = 2;
						Main.player[Main.myPlayer].showItemIcon = true;
						Main.player[Main.myPlayer].showItemIcon2 = 3213;
						if (PlayerInput.UsingGamepad)
						{
							Main.player[Main.myPlayer].GamepadEnableGrappleCooldown();
						}
						if (Main.mouseRight && Main.mouseRightRelease && Player.StopMoneyTroughFromWorking == 0)
						{
							Main.mouseRightRelease = false;
							if (Main.player[Main.myPlayer].chest == -2)
							{
								Main.PlaySound(2, -1, -1, 59);
								Main.player[Main.myPlayer].chest = -1;
								Recipe.FindRecipes();
								return;
							}
							Main.player[Main.myPlayer].flyingPigChest = i;
							Main.player[Main.myPlayer].chest = -2;
							Main.player[Main.myPlayer].chestX = (int)(projectile.Center.X / 16f);
							Main.player[Main.myPlayer].chestY = (int)(projectile.Center.Y / 16f);
							Main.player[Main.myPlayer].talkNPC = -1;
							Main.npcShop = 0;
							Main.playerInventory = true;
							Main.PlaySound(2, -1, -1, 59);
							Recipe.FindRecipes();
						}
					}
				}
			}
		}
	}
}

