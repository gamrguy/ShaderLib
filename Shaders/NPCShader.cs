using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace ShaderLib.Shaders
{
	public class NPCShader : GlobalNPC
	{
		public List<Func<int, int>> hooks;
		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			int shaderID = 0;

			foreach(var hook in hooks) {
				shaderID = hook(shaderID);
			}

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationZ(0f) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0f)));

			DrawData data = new DrawData();
			data.origin = npc.Center;
			data.position = npc.position - Main.screenPosition;
			data.scale = new Vector2(npc.scale, npc.scale);
			data.texture = Main.npcTexture[npc.type];
			data.sourceRect = npc.frame;//data.texture.Frame(1, Main.npcFrameCount[npc.type], 0, npc.frame);
			GameShaders.Armor.ApplySecondary(116/*shaderID*/, npc, data);

			return true;
		}

		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationZ(0f) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0f)));
		}

		/*private void DrawNPC(int i, bool behindTiles)
		{
			int type = Main.npc[i].type;
			//this.LoadNPC(type);
			if (Main.npc[i].setFrameSize)
			{
				Main.npc[i].frame = new Microsoft.Xna.Framework.Rectangle(0, 0, Main.npcTexture[type].Width, Main.npcTexture[type].Height / Main.npcFrameCount[type]);
				Main.npc[i].setFrameSize = false;
			}
			if (Main.npc[i].realLife == -1 && Main.npc[i].life >= Main.npc[i].lifeMax && !Main.npc[i].boss)
			{
				bool flag = Lighting.GetColor((int)((double)Main.npc[i].position.X + (double)Main.npc[i].width * 0.5) / 16, (int)(((double)Main.npc[i].position.Y + (double)Main.npc[i].height * 0.5) / 16.0)).ToVector3().Length() > 0.4325f;
				bool flag2 = false;
				if (LockOnHelper.AimedTarget == Main.npc[i])
				{
					flag2 = true;
				}
				else
				{
					float num = Main.npc[i].Distance(Main.player[Main.myPlayer].Center);
					if (num < 400f && flag)
					{
						flag2 = true;
					}
				}
				if (flag2 && Main.npc[i].lifeMax < 5)
				{
					flag2 = false;
				}
				if (flag2 && Main.npc[i].aiStyle == 25 && Main.npc[i].ai[0] == 0f)
				{
					flag2 = false;
				}
				if (flag2)
				{
					Main.npc[i].nameOver = MathHelper.Clamp(Main.npc[i].nameOver + 0.025f, 0f, 1f);
				}
				else
				{
					Main.npc[i].nameOver = MathHelper.Clamp(Main.npc[i].nameOver - 0.025f, 0f, 1f);
				}
			}
			else
			{
				Main.npc[i].nameOver = MathHelper.Clamp(Main.npc[i].nameOver - 0.025f, 0f, 1f);
			}
			if (type == 101)
			{
				bool flag3 = true;
				Vector2 vector = new Vector2(Main.npc[i].position.X + (float)(Main.npc[i].width / 2), Main.npc[i].position.Y + (float)(Main.npc[i].height / 2));
				float num2 = Main.npc[i].ai[0] * 16f + 8f - vector.X;
				float num3 = Main.npc[i].ai[1] * 16f + 8f - vector.Y;
				float rotation = (float)Math.Atan2((double)num3, (double)num2) - 1.57f;
				bool flag4 = true;
				while (flag4)
				{
					float num4 = 0.75f;
					int height = 28;
					float num5 = (float)Math.Sqrt((double)(num2 * num2 + num3 * num3));
					if (num5 < 28f * num4)
					{
						height = (int)num5 - 40 + 28;
						flag4 = false;
					}
					num5 = 20f * num4 / num5;
					num2 *= num5;
					num3 *= num5;
					vector.X += num2;
					vector.Y += num3;
					num2 = Main.npc[i].ai[0] * 16f + 8f - vector.X;
					num3 = Main.npc[i].ai[1] * 16f + 8f - vector.Y;
					Microsoft.Xna.Framework.Color color = Lighting.GetColor((int)vector.X / 16, (int)(vector.Y / 16f));
					if (!flag3)
					{
						flag3 = true;
						Main.spriteBatch.Draw(Main.chain10Texture, new Vector2(vector.X - Main.screenPosition.X, vector.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain10Texture.Width, height)), color, rotation, new Vector2((float)Main.chain10Texture.Width * 0.5f, (float)Main.chain10Texture.Height * 0.5f), num4, SpriteEffects.None, 0f);
					}
					else
					{
						flag3 = false;
						Main.spriteBatch.Draw(Main.chain11Texture, new Vector2(vector.X - Main.screenPosition.X, vector.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain10Texture.Width, height)), color, rotation, new Vector2((float)Main.chain10Texture.Width * 0.5f, (float)Main.chain10Texture.Height * 0.5f), num4, SpriteEffects.None, 0f);
					}
				}
			}
			else if (Main.npc[i].aiStyle == 13)
			{
				Vector2 vector2 = new Vector2(Main.npc[i].position.X + (float)(Main.npc[i].width / 2), Main.npc[i].position.Y + (float)(Main.npc[i].height / 2));
				float num6 = Main.npc[i].ai[0] * 16f + 8f - vector2.X;
				float num7 = Main.npc[i].ai[1] * 16f + 8f - vector2.Y;
				float rotation2 = (float)Math.Atan2((double)num7, (double)num6) - 1.57f;
				bool flag5 = true;
				while (flag5)
				{
					int num8 = 28;
					int num9 = 40;
					if (type == 259 || type == 260)
					{
						num9 = 20;
						num8 = 12;
					}
					float num10 = (float)Math.Sqrt((double)(num6 * num6 + num7 * num7));
					if (num10 < (float)num9)
					{
						num8 = (int)num10 - num9 + num8;
						flag5 = false;
					}
					num10 = (float)num8 / num10;
					num6 *= num10;
					num7 *= num10;
					vector2.X += num6;
					vector2.Y += num7;
					num6 = Main.npc[i].ai[0] * 16f + 8f - vector2.X;
					num7 = Main.npc[i].ai[1] * 16f + 8f - vector2.Y;
					Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)vector2.X / 16, (int)(vector2.Y / 16f));
					if (type == 259 || type == 260)
					{
						color2.B = 255;
						if (color2.R < 100)
						{
							color2.R = 100;
						}
						if (color2.G < 150)
						{
							color2.G = 150;
						}
					}
					if (type == 56)
					{
						Main.spriteBatch.Draw(Main.chain5Texture, new Vector2(vector2.X - Main.screenPosition.X, vector2.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain4Texture.Width, num8)), color2, rotation2, new Vector2((float)Main.chain4Texture.Width * 0.5f, (float)Main.chain4Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
					else if (type == 175)
					{
						Main.spriteBatch.Draw(Main.chain14Texture, new Vector2(vector2.X - Main.screenPosition.X, vector2.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain14Texture.Width, num8)), color2, rotation2, new Vector2((float)Main.chain14Texture.Width * 0.5f, (float)Main.chain14Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
					else if (type == 259)
					{
						Main.spriteBatch.Draw(Main.chain24Texture, new Vector2(vector2.X - Main.screenPosition.X, vector2.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain24Texture.Width, num8)), color2, rotation2, new Vector2((float)Main.chain24Texture.Width * 0.5f, (float)Main.chain24Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
					else if (type == 260)
					{
						Main.spriteBatch.Draw(Main.chain25Texture, new Vector2(vector2.X - Main.screenPosition.X, vector2.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain25Texture.Width, num8)), color2, rotation2, new Vector2((float)Main.chain25Texture.Width * 0.5f, (float)Main.chain25Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
					else
					{
						Main.spriteBatch.Draw(Main.chain4Texture, new Vector2(vector2.X - Main.screenPosition.X, vector2.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain4Texture.Width, num8)), color2, rotation2, new Vector2((float)Main.chain4Texture.Width * 0.5f, (float)Main.chain4Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			if (type == 327)
			{
				float rotation3 = 0f;
				Vector2 vector3 = new Vector2(Main.npc[i].Center.X, Main.npc[i].Center.Y + 80f);
				int num11 = (int)Main.npc[i].localAI[1];
				Microsoft.Xna.Framework.Color color3 = Lighting.GetColor((int)vector3.X / 16, (int)(vector3.Y / 16f));
				Main.spriteBatch.Draw(Main.pumpkingCloakTexture, new Vector2(vector3.X - Main.screenPosition.X, vector3.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, Main.pumpkingCloakTexture.Height / 5 * num11, Main.pumpkingCloakTexture.Width, Main.pumpkingCloakTexture.Height / 5)), color3, rotation3, new Vector2((float)Main.pumpkingCloakTexture.Width * 0.5f, (float)Main.pumpkingCloakTexture.Height * 0.5f / 5f), 1f, SpriteEffects.None, 0f);
			}
			if (type == 328)
			{
				Vector2 vector4 = new Vector2(Main.npc[i].position.X + (float)Main.npc[i].width * 0.5f - 5f * Main.npc[i].ai[0], Main.npc[i].position.Y + 20f);
				for (int j = 0; j < 2; j++)
				{
					float num12 = Main.npc[(int)Main.npc[i].ai[1]].position.X + (float)(Main.npc[(int)Main.npc[i].ai[1]].width / 2) - vector4.X;
					float num13 = Main.npc[(int)Main.npc[i].ai[1]].position.Y + (float)(Main.npc[(int)Main.npc[i].ai[1]].height / 2) - 30f - vector4.Y;
					float num14;
					if (j == 0)
					{
						num12 -= 200f * Main.npc[i].ai[0];
						num13 += 130f;
						num14 = (float)Math.Sqrt((double)(num12 * num12 + num13 * num13));
						num14 = 92f / num14;
						vector4.X += num12 * num14;
						vector4.Y += num13 * num14;
					}
					else
					{
						num12 -= 50f * Main.npc[i].ai[0];
						num13 += 80f;
						num14 = (float)Math.Sqrt((double)(num12 * num12 + num13 * num13));
						num14 = 60f / num14;
						vector4.X += num12 * num14;
						vector4.Y += num13 * num14;
					}
					float rotation4 = (float)Math.Atan2((double)num13, (double)num12) - 1.57f;
					Microsoft.Xna.Framework.Color color4 = Lighting.GetColor((int)vector4.X / 16, (int)(vector4.Y / 16f));
					Main.spriteBatch.Draw(Main.pumpkingArmTexture, new Vector2(vector4.X - Main.screenPosition.X, vector4.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.pumpkingArmTexture.Width, Main.pumpkingArmTexture.Height)), color4, rotation4, new Vector2((float)Main.pumpkingArmTexture.Width * 0.5f, (float)Main.pumpkingArmTexture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					if (j == 0)
					{
						vector4.X += num12 * num14 / 2f;
						vector4.Y += num13 * num14 / 2f;
					}
				}
			}
			if (type == 36)
			{
				Vector2 vector5 = new Vector2(Main.npc[i].position.X + (float)Main.npc[i].width * 0.5f - 5f * Main.npc[i].ai[0], Main.npc[i].position.Y + 20f);
				for (int k = 0; k < 2; k++)
				{
					float num15 = Main.npc[(int)Main.npc[i].ai[1]].position.X + (float)(Main.npc[(int)Main.npc[i].ai[1]].width / 2) - vector5.X;
					float num16 = Main.npc[(int)Main.npc[i].ai[1]].position.Y + (float)(Main.npc[(int)Main.npc[i].ai[1]].height / 2) - vector5.Y;
					float num17;
					if (k == 0)
					{
						num15 -= 200f * Main.npc[i].ai[0];
						num16 += 130f;
						num17 = (float)Math.Sqrt((double)(num15 * num15 + num16 * num16));
						num17 = 92f / num17;
						vector5.X += num15 * num17;
						vector5.Y += num16 * num17;
					}
					else
					{
						num15 -= 50f * Main.npc[i].ai[0];
						num16 += 80f;
						num17 = (float)Math.Sqrt((double)(num15 * num15 + num16 * num16));
						num17 = 60f / num17;
						vector5.X += num15 * num17;
						vector5.Y += num16 * num17;
					}
					float rotation5 = (float)Math.Atan2((double)num16, (double)num15) - 1.57f;
					Microsoft.Xna.Framework.Color color5 = Lighting.GetColor((int)vector5.X / 16, (int)(vector5.Y / 16f));
					Main.spriteBatch.Draw(Main.boneArmTexture, new Vector2(vector5.X - Main.screenPosition.X, vector5.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.boneArmTexture.Width, Main.boneArmTexture.Height)), color5, rotation5, new Vector2((float)Main.boneArmTexture.Width * 0.5f, (float)Main.boneArmTexture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					if (k == 0)
					{
						vector5.X += num15 * num17 / 2f;
						vector5.Y += num16 * num17 / 2f;
					}
					else if (base.IsActive)
					{
						vector5.X += num15 * num17 - 16f;
						vector5.Y += num16 * num17 - 6f;
						int num18 = Dust.NewDust(new Vector2(vector5.X, vector5.Y), 30, 10, 5, num15 * 0.02f, num16 * 0.02f, 0, default(Microsoft.Xna.Framework.Color), 2f);
						Main.dust[num18].noGravity = true;
					}
				}
			}
			if (Main.npc[i].aiStyle == 47)
			{
				Vector2 vector6 = new Vector2(Main.npc[i].Center.X, Main.npc[i].Center.Y);
				float num19 = Main.npc[NPC.golemBoss].Center.X - vector6.X;
				float num20 = Main.npc[NPC.golemBoss].Center.Y - vector6.Y;
				num20 -= 7f;
				if (type == 247)
				{
					num19 -= 70f;
				}
				else
				{
					num19 += 66f;
				}
				float rotation6 = (float)Math.Atan2((double)num20, (double)num19) - 1.57f;
				bool flag6 = true;
				while (flag6)
				{
					float num21 = (float)Math.Sqrt((double)(num19 * num19 + num20 * num20));
					if (num21 < 16f)
					{
						flag6 = false;
					}
					else
					{
						num21 = 16f / num21;
						num19 *= num21;
						num20 *= num21;
						vector6.X += num19;
						vector6.Y += num20;
						num19 = Main.npc[NPC.golemBoss].Center.X - vector6.X;
						num20 = Main.npc[NPC.golemBoss].Center.Y - vector6.Y;
						num20 -= 7f;
						if (type == 247)
						{
							num19 -= 70f;
						}
						else
						{
							num19 += 66f;
						}
						Microsoft.Xna.Framework.Color color6 = Lighting.GetColor((int)vector6.X / 16, (int)(vector6.Y / 16f));
						Main.spriteBatch.Draw(Main.chain21Texture, new Vector2(vector6.X - Main.screenPosition.X, vector6.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain21Texture.Width, Main.chain21Texture.Height)), color6, rotation6, new Vector2((float)Main.chain21Texture.Width * 0.5f, (float)Main.chain21Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			if (Main.npc[i].aiStyle >= 33 && Main.npc[i].aiStyle <= 36)
			{
				Vector2 vector7 = new Vector2(Main.npc[i].position.X + (float)Main.npc[i].width * 0.5f - 5f * Main.npc[i].ai[0], Main.npc[i].position.Y + 20f);
				for (int l = 0; l < 2; l++)
				{
					float num22 = Main.npc[(int)Main.npc[i].ai[1]].position.X + (float)(Main.npc[(int)Main.npc[i].ai[1]].width / 2) - vector7.X;
					float num23 = Main.npc[(int)Main.npc[i].ai[1]].position.Y + (float)(Main.npc[(int)Main.npc[i].ai[1]].height / 2) - vector7.Y;
					float num24;
					if (l == 0)
					{
						num22 -= 200f * Main.npc[i].ai[0];
						num23 += 130f;
						num24 = (float)Math.Sqrt((double)(num22 * num22 + num23 * num23));
						num24 = 92f / num24;
						vector7.X += num22 * num24;
						vector7.Y += num23 * num24;
					}
					else
					{
						num22 -= 50f * Main.npc[i].ai[0];
						num23 += 80f;
						num24 = (float)Math.Sqrt((double)(num22 * num22 + num23 * num23));
						num24 = 60f / num24;
						vector7.X += num22 * num24;
						vector7.Y += num23 * num24;
					}
					float rotation7 = (float)Math.Atan2((double)num23, (double)num22) - 1.57f;
					Microsoft.Xna.Framework.Color color7 = Lighting.GetColor((int)vector7.X / 16, (int)(vector7.Y / 16f));
					Main.spriteBatch.Draw(Main.boneArm2Texture, new Vector2(vector7.X - Main.screenPosition.X, vector7.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.boneArmTexture.Width, Main.boneArmTexture.Height)), color7, rotation7, new Vector2((float)Main.boneArmTexture.Width * 0.5f, (float)Main.boneArmTexture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
					if (l == 0)
					{
						vector7.X += num22 * num24 / 2f;
						vector7.Y += num23 * num24 / 2f;
					}
					else if (base.IsActive)
					{
						vector7.X += num22 * num24 - 16f;
						vector7.Y += num23 * num24 - 6f;
						int num25 = Dust.NewDust(new Vector2(vector7.X, vector7.Y), 30, 10, 6, num22 * 0.02f, num23 * 0.02f, 0, default(Microsoft.Xna.Framework.Color), 2.5f);
						Main.dust[num25].noGravity = true;
					}
				}
			}
			if (Main.npc[i].aiStyle == 20)
			{
				Vector2 vector8 = new Vector2(Main.npc[i].position.X + (float)(Main.npc[i].width / 2), Main.npc[i].position.Y + (float)(Main.npc[i].height / 2));
				float num26 = Main.npc[i].ai[1] - vector8.X;
				float num27 = Main.npc[i].ai[2] - vector8.Y;
				float num28 = (float)Math.Atan2((double)num27, (double)num26) - 1.57f;
				Main.npc[i].rotation = num28;
				bool flag7 = true;
				while (flag7)
				{
					int height2 = 12;
					float num29 = (float)Math.Sqrt((double)(num26 * num26 + num27 * num27));
					if (num29 < 20f)
					{
						height2 = (int)num29 - 20 + 12;
						flag7 = false;
					}
					num29 = 12f / num29;
					num26 *= num29;
					num27 *= num29;
					vector8.X += num26;
					vector8.Y += num27;
					num26 = Main.npc[i].ai[1] - vector8.X;
					num27 = Main.npc[i].ai[2] - vector8.Y;
					Microsoft.Xna.Framework.Color color8 = Lighting.GetColor((int)vector8.X / 16, (int)(vector8.Y / 16f));
					Main.spriteBatch.Draw(Main.chainTexture, new Vector2(vector8.X - Main.screenPosition.X, vector8.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chainTexture.Width, height2)), color8, num28, new Vector2((float)Main.chainTexture.Width * 0.5f, (float)Main.chainTexture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
				}
				Main.spriteBatch.Draw(Main.spikeBaseTexture, new Vector2(Main.npc[i].ai[1] - Main.screenPosition.X, Main.npc[i].ai[2] - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.spikeBaseTexture.Width, Main.spikeBaseTexture.Height)), Lighting.GetColor((int)Main.npc[i].ai[1] / 16, (int)(Main.npc[i].ai[2] / 16f)), num28 - 0.75f, new Vector2((float)Main.spikeBaseTexture.Width * 0.5f, (float)Main.spikeBaseTexture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
			}
			Microsoft.Xna.Framework.Color color9 = Lighting.GetColor((int)((double)Main.npc[i].position.X + (double)Main.npc[i].width * 0.5) / 16, (int)(((double)Main.npc[i].position.Y + (double)Main.npc[i].height * 0.5) / 16.0));
			if (type >= 277 && type <= 280)
			{
				if (color9.R < 255)
				{
					color9.R = 255;
				}
				if (color9.G < 175)
				{
					color9.G = 175;
				}
			}
			if (type == -4)
			{
				int num30 = (int)color9.R;
				int num31 = (int)color9.G;
				int num32 = (int)color9.B;
				num30 *= 2;
				if (num30 > 255)
				{
					num30 = 255;
				}
				num31 *= 2;
				if (num31 > 255)
				{
					num31 = 255;
				}
				num32 *= 2;
				if (num32 > 255)
				{
					num32 = 255;
				}
				color9 = new Microsoft.Xna.Framework.Color(num30, num31, num32);
			}
			if (behindTiles && type != 113 && type != 114)
			{
				int num33 = (int)((Main.npc[i].position.X - 8f) / 16f);
				int num34 = (int)((Main.npc[i].position.X + (float)Main.npc[i].width + 8f) / 16f);
				int num35 = (int)((Main.npc[i].position.Y - 8f) / 16f);
				int num36 = (int)((Main.npc[i].position.Y + (float)Main.npc[i].height + 8f) / 16f);
				for (int m = num33; m <= num34; m++)
				{
					for (int n = num35; n <= num36; n++)
					{
						if (Lighting.Brightness(m, n) == 0f)
						{
							color9 = Microsoft.Xna.Framework.Color.Black;
						}
					}
				}
			}
			float num37 = 1f;
			float num38 = 1f;
			float num39 = 1f;
			float a = 1f;
			if (Main.npc[i].poisoned)
			{
				if (Main.rand.Next(30) == 0)
				{
					int num40 = Dust.NewDust(Main.npc[i].position, Main.npc[i].width, Main.npc[i].height, 46, 0f, 0f, 120, default(Microsoft.Xna.Framework.Color), 0.2f);
					Main.dust[num40].noGravity = true;
					Main.dust[num40].fadeIn = 1.9f;
				}
				num37 *= 0.65f;
				num39 *= 0.75f;
				color9 = Main.buffColor(color9, num37, num38, num39, a);
			}
			if (Main.npc[i].venom)
			{
				if (Main.rand.Next(10) == 0)
				{
					int num41 = Dust.NewDust(Main.npc[i].position, Main.npc[i].width, Main.npc[i].height, 171, 0f, 0f, 100, default(Microsoft.Xna.Framework.Color), 0.5f);
					Main.dust[num41].noGravity = true;
					Main.dust[num41].fadeIn = 1.5f;
				}
				num38 *= 0.45f;
				num37 *= 0.75f;
				color9 = Main.buffColor(color9, num37, num38, num39, a);
			}
			if (Main.npc[i].midas)
			{
				num39 *= 0.3f;
				num37 *= 0.85f;
				color9 = Main.buffColor(color9, num37, num38, num39, a);
			}
			if (Main.npc[i].shadowFlame && Main.rand.Next(5) < 4)
			{
				int num42 = Dust.NewDust(new Vector2(Main.npc[i].position.X - 2f, Main.npc[i].position.Y - 2f), Main.npc[i].width + 4, Main.npc[i].height + 4, 27, Main.npc[i].velocity.X * 0.4f, Main.npc[i].velocity.Y * 0.4f, 180, default(Microsoft.Xna.Framework.Color), 1.95f);
				Main.dust[num42].noGravity = true;
				Main.dust[num42].velocity *= 0.75f;
				Dust expr_1E56_cp_0 = Main.dust[num42];
				expr_1E56_cp_0.velocity.X = expr_1E56_cp_0.velocity.X * 0.75f;
				Dust expr_1E74_cp_0 = Main.dust[num42];
				expr_1E74_cp_0.velocity.Y = expr_1E74_cp_0.velocity.Y - 1f;
				if (Main.rand.Next(4) == 0)
				{
					Main.dust[num42].noGravity = false;
					Main.dust[num42].scale *= 0.5f;
				}
			}
			if (Main.npc[i].onFire)
			{
				if (Main.rand.Next(4) < 3)
				{
					int num43 = Dust.NewDust(new Vector2(Main.npc[i].position.X - 2f, Main.npc[i].position.Y - 2f), Main.npc[i].width + 4, Main.npc[i].height + 4, 6, Main.npc[i].velocity.X * 0.4f, Main.npc[i].velocity.Y * 0.4f, 100, default(Microsoft.Xna.Framework.Color), 3.5f);
					Main.dust[num43].noGravity = true;
					Main.dust[num43].velocity *= 1.8f;
					Dust expr_1FAD_cp_0 = Main.dust[num43];
					expr_1FAD_cp_0.velocity.Y = expr_1FAD_cp_0.velocity.Y - 0.5f;
					if (Main.rand.Next(4) == 0)
					{
						Main.dust[num43].noGravity = false;
						Main.dust[num43].scale *= 0.5f;
					}
				}
				Lighting.AddLight((int)(Main.npc[i].position.X / 16f), (int)(Main.npc[i].position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
			}
			if (Main.npc[i].daybreak)
			{
				if (Main.rand.Next(4) < 3)
				{
					int num44 = Dust.NewDust(new Vector2(Main.npc[i].position.X - 2f, Main.npc[i].position.Y - 2f), Main.npc[i].width + 4, Main.npc[i].height + 4, 158, Main.npc[i].velocity.X * 0.4f, Main.npc[i].velocity.Y * 0.4f, 100, default(Microsoft.Xna.Framework.Color), 3.5f);
					Main.dust[num44].noGravity = true;
					Main.dust[num44].velocity *= 2.8f;
					Dust expr_2134_cp_0 = Main.dust[num44];
					expr_2134_cp_0.velocity.Y = expr_2134_cp_0.velocity.Y - 0.5f;
					if (Main.rand.Next(4) == 0)
					{
						Main.dust[num44].noGravity = false;
						Main.dust[num44].scale *= 0.5f;
					}
				}
				Lighting.AddLight((int)(Main.npc[i].position.X / 16f), (int)(Main.npc[i].position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
			}
			if (Main.npc[i].dryadWard && Main.npc[i].velocity.X != 0f && Main.rand.Next(4) == 0)
			{
				int num45 = Dust.NewDust(new Vector2(Main.npc[i].position.X - 2f, Main.npc[i].position.Y + (float)Main.npc[i].height - 2f), Main.npc[i].width + 4, 4, 163, Main.npc[i].velocity.X * 0.4f, Main.npc[i].velocity.Y * 0.4f, 100, default(Microsoft.Xna.Framework.Color), 1.5f);
				Main.dust[num45].noGravity = true;
				Main.dust[num45].noLight = true;
				Main.dust[num45].velocity *= 0f;
			}
			if (Main.npc[i].dryadBane && Main.rand.Next(4) == 0)
			{
				int num46 = Dust.NewDust(new Vector2(Main.npc[i].position.X - 2f, Main.npc[i].position.Y), Main.npc[i].width + 4, Main.npc[i].height, 163, Main.npc[i].velocity.X * 0.4f, Main.npc[i].velocity.Y * 0.4f, 100, default(Microsoft.Xna.Framework.Color), 1.5f);
				Main.dust[num46].noGravity = true;
				Main.dust[num46].velocity *= new Vector2(Main.rand.NextFloat() * 4f - 2f, 0f);
				Main.dust[num46].noLight = true;
			}
			if (Main.npc[i].loveStruck && Main.rand.Next(5) == 0)
			{
				Vector2 value = new Vector2((float)Main.rand.Next(-10, 11), (float)Main.rand.Next(-10, 11));
				value.Normalize();
				value.X *= 0.66f;
				int num47 = Gore.NewGore(Main.npc[i].position + new Vector2((float)Main.rand.Next(Main.npc[i].width + 1), (float)Main.rand.Next(Main.npc[i].height + 1)), value * (float)Main.rand.Next(3, 6) * 0.33f, 331, (float)Main.rand.Next(40, 121) * 0.01f);
				Main.gore[num47].sticky = false;
				Main.gore[num47].velocity *= 0.4f;
				Gore expr_2501_cp_0 = Main.gore[num47];
				expr_2501_cp_0.velocity.Y = expr_2501_cp_0.velocity.Y - 0.6f;
			}
			if (Main.npc[i].stinky)
			{
				num37 *= 0.7f;
				num39 *= 0.55f;
				color9 = Main.buffColor(color9, num37, num38, num39, a);
				if (Main.rand.Next(5) == 0)
				{
					Vector2 value2 = new Vector2((float)Main.rand.Next(-10, 11), (float)Main.rand.Next(-10, 11));
					value2.Normalize();
					value2.X *= 0.66f;
					value2.Y = Math.Abs(value2.Y);
					Vector2 vector9 = value2 * (float)Main.rand.Next(3, 5) * 0.25f;
					int num48 = Dust.NewDust(Main.npc[i].position, Main.npc[i].width, Main.npc[i].height, 188, vector9.X, vector9.Y * 0.5f, 100, default(Microsoft.Xna.Framework.Color), 1.5f);
					Main.dust[num48].velocity *= 0.1f;
					Dust expr_264D_cp_0 = Main.dust[num48];
					expr_264D_cp_0.velocity.Y = expr_264D_cp_0.velocity.Y - 0.5f;
				}
			}
			if (Main.npc[i].dripping && Main.rand.Next(4) != 0)
			{
				Vector2 position = Main.npc[i].position;
				position.X -= 2f;
				position.Y -= 2f;
				if (Main.rand.Next(2) == 0)
				{
					int num49 = Dust.NewDust(position, Main.npc[i].width + 4, Main.npc[i].height + 2, 211, 0f, 0f, 50, default(Microsoft.Xna.Framework.Color), 0.8f);
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num49].alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num49].alpha += 25;
					}
					Main.dust[num49].noLight = true;
					Main.dust[num49].velocity *= 0.2f;
					Dust expr_278A_cp_0 = Main.dust[num49];
					expr_278A_cp_0.velocity.Y = expr_278A_cp_0.velocity.Y + 0.2f;
					Main.dust[num49].velocity += Main.npc[i].velocity;
				}
				else
				{
					int num50 = Dust.NewDust(position, Main.npc[i].width + 8, Main.npc[i].height + 8, 211, 0f, 0f, 50, default(Microsoft.Xna.Framework.Color), 1.1f);
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num50].alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num50].alpha += 25;
					}
					Main.dust[num50].noLight = true;
					Main.dust[num50].noGravity = true;
					Main.dust[num50].velocity *= 0.2f;
					Dust expr_2899_cp_0 = Main.dust[num50];
					expr_2899_cp_0.velocity.Y = expr_2899_cp_0.velocity.Y + 1f;
					Main.dust[num50].velocity += Main.npc[i].velocity;
				}
			}
			if (Main.npc[i].drippingSlime)
			{
				if (Main.rand.Next(4) != 0)
				{
					int alpha = 175;
					Microsoft.Xna.Framework.Color newColor = new Microsoft.Xna.Framework.Color(0, 80, 255, 100);
					Vector2 position2 = Main.npc[i].position;
					position2.X -= 2f;
					position2.Y -= 2f;
					if (Main.rand.Next(2) == 0)
					{
						int num51 = Dust.NewDust(position2, Main.npc[i].width + 4, Main.npc[i].height + 2, 4, 0f, 0f, alpha, newColor, 1.4f);
						if (Main.rand.Next(2) == 0)
						{
							Main.dust[num51].alpha += 25;
						}
						if (Main.rand.Next(2) == 0)
						{
							Main.dust[num51].alpha += 25;
						}
						Main.dust[num51].noLight = true;
						Main.dust[num51].velocity *= 0.2f;
						Dust expr_2A02_cp_0 = Main.dust[num51];
						expr_2A02_cp_0.velocity.Y = expr_2A02_cp_0.velocity.Y + 0.2f;
						Main.dust[num51].velocity += Main.npc[i].velocity;
					}
				}
				num37 *= 0.8f;
				num38 *= 0.8f;
				color9 = Main.buffColor(color9, num37, num38, num39, a);
			}
			if (Main.npc[i].ichor)
			{
				color9 = new Microsoft.Xna.Framework.Color(255, 255, 0, 255);
			}
			if (Main.npc[i].onFrostBurn)
			{
				if (Main.rand.Next(4) < 3)
				{
					int num52 = Dust.NewDust(new Vector2(Main.npc[i].position.X - 2f, Main.npc[i].position.Y - 2f), Main.npc[i].width + 4, Main.npc[i].height + 4, 135, Main.npc[i].velocity.X * 0.4f, Main.npc[i].velocity.Y * 0.4f, 100, default(Microsoft.Xna.Framework.Color), 3.5f);
					Main.dust[num52].noGravity = true;
					Main.dust[num52].velocity *= 1.8f;
					Dust expr_2B79_cp_0 = Main.dust[num52];
					expr_2B79_cp_0.velocity.Y = expr_2B79_cp_0.velocity.Y - 0.5f;
					if (Main.rand.Next(4) == 0)
					{
						Main.dust[num52].noGravity = false;
						Main.dust[num52].scale *= 0.5f;
					}
				}
				Lighting.AddLight((int)(Main.npc[i].position.X / 16f), (int)(Main.npc[i].position.Y / 16f + 1f), 0.1f, 0.6f, 1f);
			}
			if (Main.npc[i].onFire2)
			{
				if (Main.rand.Next(4) < 3)
				{
					int num53 = Dust.NewDust(new Vector2(Main.npc[i].position.X - 2f, Main.npc[i].position.Y - 2f), Main.npc[i].width + 4, Main.npc[i].height + 4, 75, Main.npc[i].velocity.X * 0.4f, Main.npc[i].velocity.Y * 0.4f, 100, default(Microsoft.Xna.Framework.Color), 3.5f);
					Main.dust[num53].noGravity = true;
					Main.dust[num53].velocity *= 1.8f;
					Dust expr_2CFD_cp_0 = Main.dust[num53];
					expr_2CFD_cp_0.velocity.Y = expr_2CFD_cp_0.velocity.Y - 0.5f;
					if (Main.rand.Next(4) == 0)
					{
						Main.dust[num53].noGravity = false;
						Main.dust[num53].scale *= 0.5f;
					}
				}
				Lighting.AddLight((int)(Main.npc[i].position.X / 16f), (int)(Main.npc[i].position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
			}
			if (Main.player[Main.myPlayer].detectCreature && Main.npc[i].lifeMax > 1)
			{
				byte b;
				byte b2;
				byte b3;
				if (Main.npc[i].friendly || Main.npc[i].catchItem > 0 || (Main.npc[i].damage == 0 && Main.npc[i].lifeMax == 5))
				{
					b = 50;
					b2 = 255;
					b3 = 50;
				}
				else
				{
					b = 255;
					b2 = 50;
					b3 = 50;
				}
				if (color9.R < b)
				{
					color9.R = b;
				}
				if (color9.G < b2)
				{
					color9.G = b2;
				}
				if (color9.B < b3)
				{
					color9.B = b3;
				}
			}
			if (type == 50)
			{
				Vector2 zero = Vector2.Zero;
				float num54 = 0f;
				zero.Y -= Main.npc[i].velocity.Y;
				zero.X -= Main.npc[i].velocity.X * 2f;
				num54 += Main.npc[i].velocity.X * 0.05f;
				if (Main.npc[i].frame.Y == 120)
				{
					zero.Y += 2f;
				}
				if (Main.npc[i].frame.Y == 360)
				{
					zero.Y -= 2f;
				}
				if (Main.npc[i].frame.Y == 480)
				{
					zero.Y -= 6f;
				}
				Main.spriteBatch.Draw(Main.ninjaTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) + zero.X, Main.npc[i].position.Y - Main.screenPosition.Y + (float)(Main.npc[i].height / 2) + zero.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.ninjaTexture.Width, Main.ninjaTexture.Height)), color9, num54, new Vector2((float)(Main.ninjaTexture.Width / 2), (float)(Main.ninjaTexture.Height / 2)), 1f, SpriteEffects.None, 0f);
			}
			if (type == 71)
			{
				Vector2 zero2 = Vector2.Zero;
				float num55 = 0f;
				zero2.Y -= Main.npc[i].velocity.Y * 0.3f;
				zero2.X -= Main.npc[i].velocity.X * 0.6f;
				num55 += Main.npc[i].velocity.X * 0.09f;
				if (Main.npc[i].frame.Y == 120)
				{
					zero2.Y += 2f;
				}
				if (Main.npc[i].frame.Y == 360)
				{
					zero2.Y -= 2f;
				}
				if (Main.npc[i].frame.Y == 480)
				{
					zero2.Y -= 6f;
				}
				Main.spriteBatch.Draw(Main.itemTexture[327], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) + zero2.X, Main.npc[i].position.Y - Main.screenPosition.Y + (float)(Main.npc[i].height / 2) + zero2.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.itemTexture[327].Width, Main.itemTexture[327].Height)), color9, num55, new Vector2((float)(Main.itemTexture[327].Width / 2), (float)(Main.itemTexture[327].Height / 2)), 1f, SpriteEffects.None, 0f);
			}
			if (type == 69)
			{
				Main.spriteBatch.Draw(Main.antLionTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2), Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height + 14f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.antLionTexture.Width, Main.antLionTexture.Height)), color9, -Main.npc[i].rotation * 0.3f, new Vector2((float)(Main.antLionTexture.Width / 2), (float)(Main.antLionTexture.Height / 2)), 1f, SpriteEffects.None, 0f);
			}
			if (type == 1 && Main.npc[i].ai[1] > 0f)
			{
				int num56 = (int)Main.npc[i].ai[1];
				float num57 = 1f;
				float num58 = 22f * Main.npc[i].scale;
				float num59 = 18f * Main.npc[i].scale;
				float num60 = (float)Main.itemTexture[num56].Width;
				float num61 = (float)Main.itemTexture[num56].Height;
				if (num60 > num58)
				{
					num57 *= num58 / num60;
					num60 *= num57;
					num61 *= num57;
				}
				if (num61 > num59)
				{
					num57 *= num59 / num61;
					num60 *= num57;
					num61 *= num57;
				}
				float num62 = -1f;
				float num63 = 1f;
				int num64 = Main.npc[i].frame.Y / (Main.npcTexture[type].Height / Main.npcFrameCount[type]);
				num63 -= (float)num64;
				num62 += (float)(num64 * 2);
				float num65 = 0.2f;
				num65 -= 0.3f * (float)num64;
				Main.spriteBatch.Draw(Main.itemTexture[num56], new Vector2(Main.npc[i].Center.X - Main.screenPosition.X + num62, Main.npc[i].Center.Y - Main.screenPosition.Y + Main.npc[i].gfxOffY + num63), null, color9, num65, new Vector2((float)(Main.itemTexture[num56].Width / 2), (float)(Main.itemTexture[num56].Height / 2)), num57, SpriteEffects.None, 0f);
			}
			float num66 = 0f;
			float num67 = Main.NPCAddHeight(i);
			Vector2 vector10 = new Vector2((float)(Main.npcTexture[type].Width / 2), (float)(Main.npcTexture[type].Height / Main.npcFrameCount[type] / 2));
			if (type == 108 || type == 124)
			{
				num66 = 2f;
			}
			else if (type == 357)
			{
				num66 = Main.npc[i].localAI[0];
			}
			else if (type == 467)
			{
				num66 = 7f;
			}
			else if (type == 537)
			{
				num66 = 2f;
			}
			else if (type == 509)
			{
				num66 = -6f;
			}
			else if (type == 490)
			{
				num66 = 4f;
			}
			else if (type == 484)
			{
				num66 = 2f;
			}
			else if (type == 483)
			{
				num66 = 14f;
			}
			else if (type == 477)
			{
				num67 = 22f;
			}
			else if (type == 478)
			{
				num66 -= 2f;
			}
			else if (type == 469 && Main.npc[i].ai[2] == 1f)
			{
				num66 = 14f;
			}
			else if (type == 4)
			{
				vector10 = new Vector2(55f, 107f);
			}
			else if (type == 125)
			{
				vector10 = new Vector2(55f, 107f);
			}
			else if (type == 126)
			{
				vector10 = new Vector2(55f, 107f);
			}
			else if (type == 63 || type == 64 || type == 103)
			{
				vector10.Y += 4f;
			}
			else if (type == 69)
			{
				vector10.Y += 8f;
			}
			else if (type == 262)
			{
				vector10.Y = 77f;
				num67 += 26f;
			}
			else if (type == 264)
			{
				vector10.Y = 21f;
				num67 += 2f;
			}
			else if (type == 266)
			{
				num67 += 50f;
			}
			else if (type == 268)
			{
				num67 += 16f;
			}
			else if (type == 288)
			{
				num67 += 6f;
			}
			if (Main.npc[i].aiStyle == 10 || type == 72)
			{
				color9 = Microsoft.Xna.Framework.Color.White;
			}
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Main.npc[i].spriteDirection == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			if (type == 124 && Main.npc[i].localAI[0] == 0f)
			{
				int num68 = 0;
				if (Main.npc[i].frame.Y > 56)
				{
					num68 += 4;
				}
				num68 += Main.npc[i].frame.Y / 56;
				if (num68 >= Main.OffsetsPlayerHeadgear.Length)
				{
					num68 = 0;
				}
				float y = Main.OffsetsPlayerHeadgear[num68].Y;
				this.LoadProjectile(582);
				Texture2D texture2D = Main.projectileTexture[582];
				Vector2 vector11 = Main.npc[i].Center - Main.screenPosition;
				vector11 -= new Vector2((float)texture2D.Width, (float)(texture2D.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
				vector11 += new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY + y);
				vector11 += new Vector2((float)(-(float)Main.npc[i].spriteDirection * 2), -2f);
				Main.spriteBatch.Draw(texture2D, vector11, null, Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, texture2D.Size() * new Vector2(0f, 0.5f), Main.npc[i].scale, spriteEffects, 0f);
			}
			if (type == 427 || type == 426 || type == 428 || type == 509 || type == 521 || type == 523 || type == 541 || (type >= 542 && type <= 545) || type == 546)
			{
				Texture2D texture2D2 = Main.npcTexture[type];
				Microsoft.Xna.Framework.Color value3 = Microsoft.Xna.Framework.Color.White;
				float amount = 0f;
				float amount2 = 0f;
				int num69 = 0;
				int num70 = 0;
				int num71 = 1;
				int num72 = 15;
				int num73 = 0;
				float scale = Main.npc[i].scale;
				float value4 = Main.npc[i].scale;
				int num74 = 0;
				float num75 = 0f;
				float scaleFactor = 0f;
				float num76 = 0f;
				Microsoft.Xna.Framework.Color color10 = color9;
				int num77 = type;
				if (num77 <= 509)
				{
					switch (num77)
					{
					case 426:
						num74 = 4;
						scaleFactor = 4f;
						num75 = (float)Math.Cos((double)(Main.GlobalTime % 1.2f / 1.2f * 6.28318548f)) / 2f + 0.5f;
						value3 = Microsoft.Xna.Framework.Color.Turquoise;
						amount = 0.5f;
						num69 = 6;
						num70 = 2;
						num72 = num69;
						break;
					case 427:
						num69 = 8;
						num70 = 2;
						num72 = num69 * 3;
						break;
					default:
						if (num77 == 509)
						{
							num69 = 6;
							num70 = 2;
							num72 = num69 * 3;
						}
						break;
					}
				}
				else
				{
					switch (num77)
					{
					case 521:
						num69 = 10;
						num70 = 2;
						num72 = num69;
						num73 = 1;
						value4 = 0.3f;
						break;
					case 522:
						break;
					case 523:
						num74 = 3;
						scaleFactor = 10f * Main.npc[i].scale;
						amount = 0.5f;
						amount2 = 0.8f;
						value3 = Microsoft.Xna.Framework.Color.HotPink;
						value3.A = 128;
						num76 = Main.npc[i].localAI[0];
						num75 = Main.npc[i].localAI[1];
						break;
					default:
						switch (num77)
						{
						case 541:
							num74 = 4;
							scaleFactor = 6f;
							num75 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
							value3 = Microsoft.Xna.Framework.Color.Gold;
							amount = 0.5f;
							break;
						case 542:
						case 543:
						case 544:
						case 545:
							num69 = 6;
							num70 = 3;
							num72 = num69 * 2;
							break;
						case 546:
							num69 = 8;
							num70 = 2;
							num72 = num69 * 3;
							break;
						}
						break;
					}
				}
				for (int num78 = num71; num78 < num69; num78 += num70)
				{
					Vector2 arg_3B2C_0 = Main.npc[i].oldPos[num78];
					Microsoft.Xna.Framework.Color color11 = color10;
					color11 = Microsoft.Xna.Framework.Color.Lerp(color11, value3, amount);
					color11 = Main.npc[i].GetAlpha(color11);
					color11 *= (float)(num69 - num78) / (float)num72;
					float arg_3B6D_0 = Main.npc[i].rotation;
					if (num73 == 1)
					{
						float arg_3B82_0 = Main.npc[i].oldRot[num78];
					}
					float scale2 = MathHelper.Lerp(scale, value4, 1f - (float)(num69 - num78) / (float)num72);
					Vector2 vector12 = Main.npc[i].oldPos[num78] + new Vector2((float)Main.npc[i].width, (float)Main.npc[i].height) / 2f - Main.screenPosition;
					vector12 -= new Vector2((float)texture2D2.Width, (float)(texture2D2.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
					vector12 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
					Main.spriteBatch.Draw(texture2D2, vector12, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color11, Main.npc[i].rotation, vector10, scale2, spriteEffects, 0f);
				}
				for (int num79 = 0; num79 < num74; num79++)
				{
					Microsoft.Xna.Framework.Color color12 = color9;
					color12 = Microsoft.Xna.Framework.Color.Lerp(color12, value3, amount);
					color12 = Main.npc[i].GetAlpha(color12);
					color12 = Microsoft.Xna.Framework.Color.Lerp(color12, value3, amount2);
					color12 *= 1f - num75;
					Vector2 vector13 = Main.npc[i].Center + ((float)num79 / (float)num74 * 6.28318548f + Main.npc[i].rotation + num76).ToRotationVector2() * scaleFactor * num75 - Main.screenPosition;
					vector13 -= new Vector2((float)texture2D2.Width, (float)(texture2D2.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
					vector13 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
					Main.spriteBatch.Draw(texture2D2, vector13, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color12, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
				}
				Vector2 vector14 = Main.npc[i].Center - Main.screenPosition;
				vector14 -= new Vector2((float)texture2D2.Width, (float)(texture2D2.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
				vector14 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
				Main.spriteBatch.Draw(texture2D2, vector14, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
				if (type == 427)
				{
					Main.spriteBatch.Draw(Main.glowMaskTexture[152], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
				}
				else if (type == 426)
				{
					Main.spriteBatch.Draw(Main.glowMaskTexture[153], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
				}
				if (type == 541)
				{
					Microsoft.Xna.Framework.Color color13 = new Microsoft.Xna.Framework.Color(127 - Main.npc[i].alpha, 127 - Main.npc[i].alpha, 127 - Main.npc[i].alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.Gold);
					for (int num80 = 0; num80 < num74; num80++)
					{
						Microsoft.Xna.Framework.Color color14 = color13;
						color14 = Main.npc[i].GetAlpha(color14);
						color14 *= 1f - num75;
						Vector2 vector15 = Main.npc[i].Center + ((float)num80 / (float)num74 * 6.28318548f + Main.npc[i].rotation + num76).ToRotationVector2() * (4f * num75 + 2f) - Main.screenPosition;
						vector15 -= new Vector2((float)texture2D2.Width, (float)(texture2D2.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
						vector15 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
						Main.spriteBatch.Draw(Main.glowMaskTexture[216], vector15, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color14, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
					}
					Main.spriteBatch.Draw(Main.glowMaskTexture[216], vector14, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color13, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
				}
				if (type == 546)
				{
					Main.spriteBatch.Draw(Main.extraTexture[76], vector14, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255, 255, 255, 200), MathHelper.Clamp(Main.npc[i].velocity.X * 0.1f, -0.3926991f, 0.3926991f), vector10, Main.npc[i].scale, spriteEffects, 0f);
					return;
				}
			}
			else
			{
				if (type == 371 || (type >= 454 && type <= 459))
				{
					Texture2D texture2D3 = Main.npcTexture[type];
					Vector2 vector16 = Main.npc[i].Center - Main.screenPosition;
					vector16 -= new Vector2((float)texture2D3.Width, (float)(texture2D3.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
					vector16 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
					Main.spriteBatch.Draw(texture2D3, vector16, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
					return;
				}
				if (type == 493 || type == 507 || type == 422 || type == 517)
				{
					Texture2D texture2D4 = Main.npcTexture[type];
					Vector2 vector17 = Main.npc[i].Center - Main.screenPosition;
					Vector2 value5 = vector17 - new Vector2(300f, 310f);
					vector17 -= new Vector2((float)texture2D4.Width, (float)(texture2D4.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
					vector17 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
					Main.spriteBatch.Draw(texture2D4, vector17, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
					if (type == 493)
					{
						texture2D4 = Main.glowMaskTexture[132];
						float scaleFactor2 = 4f + (Main.npc[i].GetAlpha(color9).ToVector3() - new Vector3(0.5f)).Length() * 4f;
						for (int num81 = 0; num81 < 4; num81++)
						{
							Main.spriteBatch.Draw(texture2D4, vector17 + Main.npc[i].velocity.RotatedBy((double)((float)num81 * 1.57079637f), default(Vector2)) * scaleFactor2, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(64, 64, 64, 0) * Main.npc[i].Opacity, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
						}
					}
					else if (type == 507)
					{
						texture2D4 = Main.glowMaskTexture[143];
						float scaleFactor3 = 4f + (Main.npc[i].GetAlpha(color9).ToVector3() - new Vector3(0.5f)).Length() * 4f;
						for (int num82 = 0; num82 < 4; num82++)
						{
							Main.spriteBatch.Draw(texture2D4, vector17 + Main.npc[i].velocity.RotatedBy((double)((float)num82 * 1.57079637f), default(Vector2)) * scaleFactor3, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(64, 64, 64, 0) * Main.npc[i].Opacity, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
						}
					}
					else if (type == 422)
					{
						texture2D4 = Main.glowMaskTexture[149];
						float scaleFactor4 = 4f + (Main.npc[i].GetAlpha(color9).ToVector3() - new Vector3(0.5f)).Length() * 4f;
						for (int num83 = 0; num83 < 4; num83++)
						{
							Main.spriteBatch.Draw(texture2D4, vector17 + Main.npc[i].velocity.RotatedBy((double)((float)num83 * 1.57079637f), default(Vector2)) * scaleFactor4, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(64, 64, 64, 0) * Main.npc[i].Opacity, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
						}
					}
					else if (type == 517)
					{
						texture2D4 = Main.glowMaskTexture[162];
						float scaleFactor5 = 2f + (Main.npc[i].GetAlpha(color9).ToVector3() - new Vector3(0.5f)).Length() * 9f;
						for (int num84 = 0; num84 < 4; num84++)
						{
							Main.spriteBatch.Draw(texture2D4, vector17 + Main.npc[i].velocity.RotatedBy((double)((float)num84 * 1.57079637f), default(Vector2)) * scaleFactor5 + Vector2.UnitX * 2f, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(64, 64, 64, 0) * Main.npc[i].Opacity, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
						}
					}
					int num85 = 0;
					string key = "";
					int num77 = type;
					if (num77 <= 493)
					{
						if (num77 != 422)
						{
							if (num77 == 493)
							{
								num85 = NPC.ShieldStrengthTowerStardust;
								key = "Stardust";
							}
						}
						else
						{
							num85 = NPC.ShieldStrengthTowerVortex;
							key = "Vortex";
						}
					}
					else if (num77 != 507)
					{
						if (num77 == 517)
						{
							num85 = NPC.ShieldStrengthTowerSolar;
							key = "Solar";
						}
					}
					else
					{
						num85 = NPC.ShieldStrengthTowerNebula;
						key = "Nebula";
					}
					float num86 = (float)num85 / (float)NPC.ShieldStrengthTowerMax;
					if (num85 > 0)
					{
						Main.spriteBatch.End();
						Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);
						float num87 = 0f;
						if (Main.npc[i].ai[3] > 0f && Main.npc[i].ai[3] <= 30f)
						{
							num87 = 1f - Main.npc[i].ai[3] / 30f;
						}
						Filters.Scene[key].GetShader().UseIntensity(1f + num87).UseProgress(0f);
						DrawData value6 = new DrawData(TextureManager.Load("Images/Misc/Perlin"), value5 + new Vector2(300f, 300f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 600, 600)), Microsoft.Xna.Framework.Color.White * (num86 * 0.8f + 0.2f), Main.npc[i].rotation, new Vector2(300f, 300f), Main.npc[i].scale * (1f + num87 * 0.05f), spriteEffects, 0);
						GameShaders.Misc["ForceField"].UseColor(new Vector3(1f + num87 * 0.5f));
						GameShaders.Misc["ForceField"].Apply(new DrawData?(value6));
						value6.Draw(Main.spriteBatch);
						Main.spriteBatch.End();
						Main.spriteBatch.Begin();
						return;
					}
					if (Main.npc[i].ai[3] > 0f)
					{
						Main.spriteBatch.End();
						Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
						float num88 = Main.npc[i].ai[3] / 120f;
						float num89 = Math.Min(Main.npc[i].ai[3] / 30f, 1f);
						Filters.Scene[key].GetShader().UseIntensity(Math.Min(5f, 15f * num88) + 1f).UseProgress(num88);
						DrawData value7 = new DrawData(TextureManager.Load("Images/Misc/Perlin"), value5 + new Vector2(300f, 300f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 600, 600)), new Microsoft.Xna.Framework.Color(new Vector4(1f - (float)Math.Sqrt((double)num89))), Main.npc[i].rotation, new Vector2(300f, 300f), Main.npc[i].scale * (1f + num89), spriteEffects, 0);
						GameShaders.Misc["ForceField"].UseColor(new Vector3(2f));
						GameShaders.Misc["ForceField"].Apply(new DrawData?(value7));
						value7.Draw(Main.spriteBatch);
						Main.spriteBatch.End();
						Main.spriteBatch.Begin();
						return;
					}
					Filters.Scene[key].GetShader().UseIntensity(0f).UseProgress(0f);
					return;
				}
				else
				{
					if (type == 402)
					{
						this.LoadNPC(403);
						this.LoadNPC(404);
						NPC nPC = Main.npc[i];
						Texture2D texture2D5 = Main.npcTexture[nPC.type];
						Vector2 vector18 = nPC.Center - Main.screenPosition;
						vector18 -= new Vector2((float)texture2D5.Width, (float)(texture2D5.Height / Main.npcFrameCount[nPC.type])) * nPC.scale / 2f;
						vector18 += vector10 * nPC.scale + new Vector2(0f, num66 + num67 + nPC.gfxOffY);
						int num90 = 0;
						float num91 = 2f / (float)nPC.oldPos.Length * 0.7f;
						int num92 = nPC.oldPos.Length - 1;
						while ((float)num92 >= 1f)
						{
							if (num90 == 0)
							{
								texture2D5 = Main.npcTexture[404];
							}
							else
							{
								texture2D5 = Main.npcTexture[403];
							}
							Main.spriteBatch.Draw(texture2D5, vector18 + nPC.oldPos[num92] - nPC.position, null, nPC.GetAlpha(color9) * (0.8f - num91 * (float)num92 / 2f), nPC.oldRot[num92], vector10, nPC.scale, spriteEffects, 0f);
							if (num90 == 0)
							{
								texture2D5 = Main.glowMaskTexture[134];
							}
							else
							{
								texture2D5 = Main.glowMaskTexture[133];
							}
							Main.spriteBatch.Draw(texture2D5, vector18 + nPC.oldPos[num92] - nPC.position, null, new Microsoft.Xna.Framework.Color(255, 255, 255, 0) * (1f - num91 * (float)num92 / 2f), nPC.oldRot[num92], vector10, nPC.scale, spriteEffects, 0f);
							num90++;
							num92 -= 2;
						}
						texture2D5 = Main.npcTexture[nPC.type];
						Main.spriteBatch.Draw(texture2D5, vector18, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
						texture2D5 = Main.glowMaskTexture[135];
						Main.spriteBatch.Draw(texture2D5, vector18, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
						return;
					}
					if (type == 519)
					{
						NPC nPC2 = Main.npc[i];
						Texture2D texture2D6 = Main.npcTexture[nPC2.type];
						Vector2 vector19 = nPC2.Center - Main.screenPosition;
						vector19 -= new Vector2((float)texture2D6.Width, (float)(texture2D6.Height / Main.npcFrameCount[nPC2.type])) * nPC2.scale / 2f;
						vector19 += vector10 * nPC2.scale + new Vector2(0f, num66 + num67 + nPC2.gfxOffY);
						texture2D6 = Main.npcTexture[nPC2.type];
						Main.spriteBatch.Draw(texture2D6, vector19, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
						int num93 = 0;
						float num94 = 1f / (float)nPC2.oldPos.Length * 0.7f;
						int num95 = nPC2.oldPos.Length - 1;
						while ((float)num95 >= 0f)
						{
							float num96 = (float)(nPC2.oldPos.Length - num95) / (float)nPC2.oldPos.Length;
							Microsoft.Xna.Framework.Color color15 = Microsoft.Xna.Framework.Color.Pink;
							color15 *= 1f - num94 * (float)num95 / 1f;
							color15.A = (byte)((float)color15.A * (1f - num96));
							Main.spriteBatch.Draw(texture2D6, vector19 + nPC2.oldPos[num95] - nPC2.position, null, color15, nPC2.oldRot[num95], vector10, nPC2.scale * MathHelper.Lerp(0.3f, 1.1f, num96), spriteEffects, 0f);
							num93++;
							num95--;
						}
						return;
					}
					if (type == 522)
					{
						NPC nPC3 = Main.npc[i];
						Texture2D texture2D7 = Main.npcTexture[nPC3.type];
						Vector2 vector20 = nPC3.Center - Main.screenPosition;
						vector20 -= new Vector2((float)texture2D7.Width, (float)(texture2D7.Height / Main.npcFrameCount[nPC3.type])) * nPC3.scale / 2f;
						vector20 += vector10 * nPC3.scale + new Vector2(0f, num66 + num67 + nPC3.gfxOffY);
						int num97 = 0;
						float num98 = 1f / (float)nPC3.oldPos.Length * 1.1f;
						int num99 = nPC3.oldPos.Length - 1;
						while ((float)num99 >= 0f)
						{
							float num100 = (float)(nPC3.oldPos.Length - num99) / (float)nPC3.oldPos.Length;
							Microsoft.Xna.Framework.Color color16 = Microsoft.Xna.Framework.Color.White;
							color16 *= 1f - num98 * (float)num99 / 1f;
							color16.A = (byte)((float)color16.A * (1f - num100));
							Main.spriteBatch.Draw(texture2D7, vector20 + nPC3.oldPos[num99] - nPC3.position, null, color16, nPC3.oldRot[num99], vector10, nPC3.scale * MathHelper.Lerp(0.8f, 0.3f, num100), spriteEffects, 0f);
							num97++;
							num99--;
						}
						texture2D7 = Main.extraTexture[57];
						Main.spriteBatch.Draw(texture2D7, vector20, null, new Microsoft.Xna.Framework.Color(255, 255, 255, 0), 0f, texture2D7.Size() / 2f, Main.npc[i].scale, spriteEffects, 0f);
						return;
					}
					if (type == 488)
					{
						return;
					}
					if (type == 370 || type == 372 || type == 373)
					{
						Texture2D texture2D8 = Main.npcTexture[type];
						Microsoft.Xna.Framework.Color color17 = Microsoft.Xna.Framework.Color.White;
						float amount3 = 0f;
						bool flag8 = type == 370 && Main.npc[i].ai[0] > 4f;
						bool flag9 = type == 370 && Main.npc[i].ai[0] > 9f;
						int num101 = 120;
						int num102 = 60;
						Microsoft.Xna.Framework.Color color18 = color9;
						if (flag9)
						{
							color9 = Main.buffColor(color9, 0.4f, 0.8f, 0.4f, 1f);
						}
						else if (flag8)
						{
							color9 = Main.buffColor(color9, 0.5f, 0.7f, 0.5f, 1f);
						}
						else if (type == 370 && Main.npc[i].ai[0] == 4f && Main.npc[i].ai[2] > (float)num101)
						{
							float num103 = Main.npc[i].ai[2] - (float)num101;
							num103 /= (float)num102;
							color9 = Main.buffColor(color9, 1f - 0.5f * num103, 1f - 0.3f * num103, 1f - 0.5f * num103, 1f);
						}
						int num104 = 10;
						int num105 = 2;
						if (type == 370)
						{
							if (Main.npc[i].ai[0] == -1f)
							{
								num104 = 0;
							}
							if (Main.npc[i].ai[0] == 0f || Main.npc[i].ai[0] == 5f || Main.npc[i].ai[0] == 10f)
							{
								num104 = 7;
							}
							if (Main.npc[i].ai[0] == 1f)
							{
								color17 = Microsoft.Xna.Framework.Color.Blue;
								amount3 = 0.5f;
							}
							else
							{
								color18 = color9;
							}
						}
						else if ((type == 372 || type == 373) && Main.npc[i].ai[0] == 1f)
						{
							color17 = Microsoft.Xna.Framework.Color.Blue;
							amount3 = 0.5f;
						}
						for (int num106 = 1; num106 < num104; num106 += num105)
						{
							Vector2 arg_57AC_0 = Main.npc[i].oldPos[num106];
							Microsoft.Xna.Framework.Color color19 = color18;
							color19 = Microsoft.Xna.Framework.Color.Lerp(color19, color17, amount3);
							color19 = Main.npc[i].GetAlpha(color19);
							color19 *= (float)(num104 - num106) / 15f;
							Vector2 vector21 = Main.npc[i].oldPos[num106] + new Vector2((float)Main.npc[i].width, (float)Main.npc[i].height) / 2f - Main.screenPosition;
							vector21 -= new Vector2((float)texture2D8.Width, (float)(texture2D8.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
							vector21 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
							Main.spriteBatch.Draw(texture2D8, vector21, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color19, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
						}
						int num107 = 0;
						float num108 = 0f;
						float scaleFactor6 = 0f;
						if (type == 370)
						{
							if (Main.npc[i].ai[0] == -1f)
							{
								num107 = 0;
							}
							if (Main.npc[i].ai[0] == 3f || Main.npc[i].ai[0] == 8f)
							{
								int num109 = 60;
								int num110 = 30;
								if (Main.npc[i].ai[2] > (float)num109)
								{
									num107 = 6;
									num108 = 1f - (float)Math.Cos((double)((Main.npc[i].ai[2] - (float)num109) / (float)num110 * 6.28318548f));
									num108 /= 3f;
									scaleFactor6 = 40f;
								}
							}
							if (Main.npc[i].ai[0] == 4f && Main.npc[i].ai[2] > (float)num101)
							{
								num107 = 6;
								num108 = 1f - (float)Math.Cos((double)((Main.npc[i].ai[2] - (float)num101) / (float)num102 * 6.28318548f));
								num108 /= 3f;
								scaleFactor6 = 60f;
							}
							if (Main.npc[i].ai[0] == 9f && Main.npc[i].ai[2] > (float)num101)
							{
								num107 = 6;
								num108 = 1f - (float)Math.Cos((double)((Main.npc[i].ai[2] - (float)num101) / (float)num102 * 6.28318548f));
								num108 /= 3f;
								scaleFactor6 = 60f;
							}
							if (Main.npc[i].ai[0] == 12f)
							{
								num107 = 6;
								num108 = 1f - (float)Math.Cos((double)(Main.npc[i].ai[2] / 30f * 6.28318548f));
								num108 /= 3f;
								scaleFactor6 = 20f;
							}
						}
						for (int num111 = 0; num111 < num107; num111++)
						{
							Microsoft.Xna.Framework.Color color20 = color9;
							color20 = Microsoft.Xna.Framework.Color.Lerp(color20, color17, amount3);
							color20 = Main.npc[i].GetAlpha(color20);
							color20 *= 1f - num108;
							Vector2 vector22 = Main.npc[i].Center + ((float)num111 / (float)num107 * 6.28318548f + Main.npc[i].rotation).ToRotationVector2() * scaleFactor6 * num108 - Main.screenPosition;
							vector22 -= new Vector2((float)texture2D8.Width, (float)(texture2D8.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
							vector22 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
							Main.spriteBatch.Draw(texture2D8, vector22, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color20, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
						}
						Vector2 vector23 = Main.npc[i].Center - Main.screenPosition;
						vector23 -= new Vector2((float)texture2D8.Width, (float)(texture2D8.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
						vector23 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
						Main.spriteBatch.Draw(texture2D8, vector23, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
						if (type == 370 && Main.npc[i].ai[0] >= 4f)
						{
							texture2D8 = Main.dukeFishronTexture;
							Microsoft.Xna.Framework.Color color21 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.White, Microsoft.Xna.Framework.Color.Yellow, 0.5f);
							color17 = Microsoft.Xna.Framework.Color.Yellow;
							amount3 = 1f;
							num108 = 0.5f;
							scaleFactor6 = 10f;
							num105 = 1;
							if (Main.npc[i].ai[0] == 4f)
							{
								float num112 = Main.npc[i].ai[2] - (float)num101;
								num112 /= (float)num102;
								color17 *= num112;
								color21 *= num112;
							}
							if (Main.npc[i].ai[0] == 12f)
							{
								float num113 = Main.npc[i].ai[2];
								num113 /= 30f;
								if (num113 > 0.5f)
								{
									num113 = 1f - num113;
								}
								num113 *= 2f;
								num113 = 1f - num113;
								color17 *= num113;
								color21 *= num113;
							}
							for (int num114 = 1; num114 < num104; num114 += num105)
							{
								Vector2 arg_5E46_0 = Main.npc[i].oldPos[num114];
								Microsoft.Xna.Framework.Color color22 = color21;
								color22 = Microsoft.Xna.Framework.Color.Lerp(color22, color17, amount3);
								color22 *= (float)(num104 - num114) / 15f;
								Vector2 vector24 = Main.npc[i].oldPos[num114] + new Vector2((float)Main.npc[i].width, (float)Main.npc[i].height) / 2f - Main.screenPosition;
								vector24 -= new Vector2((float)texture2D8.Width, (float)(texture2D8.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
								vector24 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
								Main.spriteBatch.Draw(texture2D8, vector24, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color22, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
							}
							for (int num115 = 1; num115 < num107; num115++)
							{
								Microsoft.Xna.Framework.Color color23 = color21;
								color23 = Microsoft.Xna.Framework.Color.Lerp(color23, color17, amount3);
								color23 = Main.npc[i].GetAlpha(color23);
								color23 *= 1f - num108;
								Vector2 vector25 = Main.npc[i].Center + ((float)num115 / (float)num107 * 6.28318548f + Main.npc[i].rotation).ToRotationVector2() * scaleFactor6 * num108 - Main.screenPosition;
								vector25 -= new Vector2((float)texture2D8.Width, (float)(texture2D8.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
								vector25 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
								Main.spriteBatch.Draw(texture2D8, vector25, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color23, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
							}
							Main.spriteBatch.Draw(texture2D8, vector23, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color21, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
							return;
						}
					}
					else
					{
						if (type == 439 || type == 440)
						{
							int num116 = Main.npc[i].frame.Y / (Main.npcTexture[type].Height / Main.npcFrameCount[type]);
							Texture2D texture2D9 = Main.npcTexture[type];
							Texture2D texture2D10 = Main.extraTexture[30];
							Microsoft.Xna.Framework.Rectangle rectangle = texture2D10.Frame(1, 1, 0, 0);
							rectangle.Height /= 2;
							if (num116 >= 4)
							{
								rectangle.Y += rectangle.Height;
							}
							Microsoft.Xna.Framework.Color white = Microsoft.Xna.Framework.Color.White;
							float amount4 = 0f;
							Microsoft.Xna.Framework.Color color24 = color9;
							int num117 = 0;
							int num118 = 0;
							int num119 = 0;
							if (Main.npc[i].ai[0] == -1f)
							{
								if (Main.npc[i].ai[1] >= 320f && Main.npc[i].ai[1] < 960f)
								{
									white = Microsoft.Xna.Framework.Color.White;
									amount4 = 0.5f;
									num117 = 6;
									num118 = 2;
									num119 = 1;
								}
							}
							else if (Main.npc[i].ai[0] == 1f)
							{
								white = Microsoft.Xna.Framework.Color.White;
								amount4 = 0.5f;
								num117 = 4;
								num118 = 2;
								num119 = 1;
							}
							else
							{
								color24 = color9;
							}
							for (int num120 = num119; num120 < num117; num120 += num118)
							{
								Vector2 arg_626F_0 = Main.npc[i].oldPos[num120];
								Microsoft.Xna.Framework.Color color25 = color24;
								color25 = Microsoft.Xna.Framework.Color.Lerp(color25, white, amount4);
								color25 = Main.npc[i].GetAlpha(color25);
								color25 *= (float)(num117 - num120) / (float)num117;
								color25.A = 100;
								Vector2 vector26 = Main.npc[i].oldPos[num120] + new Vector2((float)Main.npc[i].width, (float)Main.npc[i].height) / 2f - Main.screenPosition;
								vector26 -= rectangle.Size() * Main.npc[i].scale / 2f;
								vector26 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
								Main.spriteBatch.Draw(texture2D10, vector26, new Microsoft.Xna.Framework.Rectangle?(rectangle), color25, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
							}
							int num121 = 0;
							float num122 = 0f;
							float scaleFactor7 = 0f;
							if (Main.npc[i].ai[0] == 5f && Main.npc[i].ai[1] >= 0f && Main.npc[i].ai[1] < 30f)
							{
								num121 = 4;
								num122 = 1f - (float)Math.Cos((double)((Main.npc[i].ai[1] - 0f) / 30f * 3.14159274f));
								num122 /= 2f;
								scaleFactor7 = 70f;
							}
							for (int num123 = 0; num123 < num121; num123++)
							{
								Microsoft.Xna.Framework.Color color26 = color9;
								color26 = Microsoft.Xna.Framework.Color.Lerp(color26, white, amount4);
								color26 = Main.npc[i].GetAlpha(color26);
								color26 *= 1f - num122;
								Vector2 vector27 = Main.npc[i].Center + ((float)num123 / (float)num121 * 6.28318548f + Main.npc[i].rotation).ToRotationVector2() * scaleFactor7 * num122 - Main.screenPosition;
								vector27 -= new Vector2((float)texture2D9.Width, (float)(texture2D9.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
								vector27 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
								Main.spriteBatch.Draw(texture2D10, vector27, new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
							}
							Vector2 vector28 = Main.npc[i].Center - Main.screenPosition;
							vector28 -= new Vector2((float)texture2D9.Width, (float)(texture2D9.Height / Main.npcFrameCount[type])) * Main.npc[i].scale / 2f;
							vector28 += vector10 * Main.npc[i].scale + new Vector2(0f, num66 + num67 + Main.npc[i].gfxOffY);
							Main.spriteBatch.Draw(texture2D9, vector28, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
							return;
						}
						if (type == 392 || type == 393 || type == 394 || type == 395)
						{
							Texture2D texture = Main.npcTexture[type];
							Vector2 vector29 = Main.npc[i].Center - Main.screenPosition + Vector2.UnitY * Main.npc[i].gfxOffY;
							vector29 = vector29.Floor();
							float scaleFactor8 = 0f;
							if (type == 393)
							{
								scaleFactor8 = -8f;
							}
							Main.spriteBatch.Draw(texture, vector29, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10 + Vector2.UnitY * scaleFactor8, Main.npc[i].scale, spriteEffects, 0f);
							if (type == 392)
							{
								Main.spriteBatch.Draw(Main.glowMaskTexture[48], vector29, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(200, 200, 200, 0), Main.npc[i].rotation, vector10 + Vector2.UnitY * scaleFactor8, Main.npc[i].scale, spriteEffects, 0f);
							}
							if (type == 395)
							{
								Main.spriteBatch.Draw(Main.glowMaskTexture[49], vector29, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(200, 200, 200, 0), Main.npc[i].rotation, vector10 + Vector2.UnitY * scaleFactor8, Main.npc[i].scale, spriteEffects, 0f);
							}
							if (type == 394)
							{
								Main.spriteBatch.Draw(Main.glowMaskTexture[50], vector29, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(200, 200, 200, 0), Main.npc[i].rotation, vector10 + Vector2.UnitY * scaleFactor8, Main.npc[i].scale, spriteEffects, 0f);
								return;
							}
						}
						else
						{
							if (type == 83 || type == 84 || type == 179)
							{
								Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Microsoft.Xna.Framework.Color.White, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								return;
							}
							if (type >= 87 && type <= 92)
							{
								Microsoft.Xna.Framework.Color alpha2 = Main.npc[i].GetAlpha(color9);
								byte b4 = (Main.tileColor.R + Main.tileColor.G + Main.tileColor.B) / 3;
								if (alpha2.R < b4)
								{
									alpha2.R = b4;
								}
								if (alpha2.G < b4)
								{
									alpha2.G = b4;
								}
								if (alpha2.B < b4)
								{
									alpha2.B = b4;
								}
								Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), alpha2, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								return;
							}
							if (type == 491)
							{
								NPC nPC4 = Main.npc[i];
								Texture2D texture2D11 = Main.npcTexture[nPC4.type];
								Microsoft.Xna.Framework.Rectangle rectangle2 = nPC4.frame;
								Vector2 origin = rectangle2.OriginFlip(new Vector2(208f, 460f), spriteEffects);
								Vector2 vector30 = nPC4.Center - Main.screenPosition;
								Vector2 value8 = new Vector2((float)(spriteEffects.HasFlag(SpriteEffects.FlipHorizontally) ? -1 : 1), 1f);
								Microsoft.Xna.Framework.Color alpha3 = nPC4.GetAlpha(color9);
								Main.spriteBatch.Draw(texture2D11, vector30, new Microsoft.Xna.Framework.Rectangle?(rectangle2), alpha3, nPC4.rotation, origin, nPC4.scale, spriteEffects, 0f);
								int num124 = (int)nPC4.localAI[3] / 8;
								texture2D11 = Main.extraTexture[40];
								rectangle2 = texture2D11.Frame(1, 4, 0, num124 % 4);
								origin = rectangle2.Size() * new Vector2(0.5f, 1f);
								Main.spriteBatch.Draw(texture2D11, vector30 + (new Vector2(102f, -384f) * value8).RotatedBy((double)nPC4.rotation, default(Vector2)), new Microsoft.Xna.Framework.Rectangle?(rectangle2), alpha3, nPC4.rotation, origin, nPC4.scale, spriteEffects, 0f);
								texture2D11 = Main.extraTexture[41];
								rectangle2 = texture2D11.Frame(1, 8, 0, num124 % 8);
								origin = rectangle2.Size() * new Vector2(0.5f, 0f) + new Vector2(0f, 10f);
								for (int num125 = 0; num125 < 5; num125++)
								{
									Main.spriteBatch.Draw(texture2D11, vector30 + (new Vector2((float)(-96 + 34 * num125), 40f) * value8).RotatedBy((double)nPC4.rotation, default(Vector2)), new Microsoft.Xna.Framework.Rectangle?(rectangle2), alpha3, nPC4.rotation, origin, nPC4.scale, spriteEffects, 0f);
								}
								texture2D11 = Main.extraTexture[42];
								rectangle2 = texture2D11.Frame(1, 4, 0, num124 % 4);
								origin = rectangle2.Size() * new Vector2(0.5f, 0f);
								for (int num126 = 0; num126 < 2; num126++)
								{
									Main.spriteBatch.Draw(texture2D11, vector30 + (new Vector2((float)(158 - 106 * num126), -302f) * value8).RotatedBy((double)nPC4.rotation, default(Vector2)), new Microsoft.Xna.Framework.Rectangle?(rectangle2), alpha3, nPC4.rotation, origin, nPC4.scale, spriteEffects, 0f);
								}
								texture2D11 = Main.extraTexture[43];
								rectangle2 = texture2D11.Frame(1, 4, 0, num124 % 4);
								origin = rectangle2.Size() * new Vector2(0.5f, 0f);
								for (int num127 = 0; num127 < 2; num127++)
								{
									Main.spriteBatch.Draw(texture2D11, vector30 + (new Vector2((float)(42 - 178 * num127), -444f) * value8).RotatedBy((double)nPC4.rotation, default(Vector2)), new Microsoft.Xna.Framework.Rectangle?(rectangle2), alpha3, nPC4.rotation, origin, nPC4.scale, spriteEffects, 0f);
								}
								texture2D11 = Main.extraTexture[44];
								rectangle2 = texture2D11.Frame(1, 4, 0, num124 % 4);
								origin = rectangle2.Size() * new Vector2(0.5f, 0f);
								Main.spriteBatch.Draw(texture2D11, vector30 + (new Vector2(-134f, -302f) * value8).RotatedBy((double)nPC4.rotation, default(Vector2)), new Microsoft.Xna.Framework.Rectangle?(rectangle2), alpha3, nPC4.rotation, origin, nPC4.scale, spriteEffects, 0f);
								texture2D11 = Main.extraTexture[45];
								rectangle2 = texture2D11.Frame(1, 4, 0, (2 + num124) % 4);
								origin = rectangle2.Size() * new Vector2(0.5f, 0f);
								Main.spriteBatch.Draw(texture2D11, vector30 + (new Vector2(-60f, -330f) * value8).RotatedBy((double)nPC4.rotation, default(Vector2)), new Microsoft.Xna.Framework.Rectangle?(rectangle2), alpha3, nPC4.rotation, origin, nPC4.scale, spriteEffects, 0f);
								this.LoadNPC(492);
								if (Main.NPCLoaded[492])
								{
									texture2D11 = Main.npcTexture[492];
									rectangle2 = texture2D11.Frame(1, 9, 0, 0);
									origin = rectangle2.Size() * new Vector2(0.5f, 0f) + new Vector2(0f, 10f);
									for (int num128 = 0; num128 < 4; num128++)
									{
										int num129 = (int)nPC4.ai[num128];
										if (num129 >= 0)
										{
											rectangle2.Y = Main.npc[num129].frame.Y;
											Main.spriteBatch.Draw(texture2D11, vector30 + (new Vector2((float)(-122 + 68 * num128), -20f) * value8).RotatedBy((double)nPC4.rotation, default(Vector2)), new Microsoft.Xna.Framework.Rectangle?(rectangle2), alpha3, nPC4.rotation, origin, nPC4.scale, spriteEffects, 0f);
										}
									}
									return;
								}
							}
							else
							{
								if (type == 398)
								{
									bool flag10 = false;
									Texture2D texture2 = Main.npcTexture[type];
									Texture2D texture3 = Main.extraTexture[16];
									Texture2D texture2D12 = Main.extraTexture[14];
									float num130 = 340f;
									float scaleFactor9 = 0.5f;
									Vector2 value9 = new Vector2(220f, -60f);
									Vector2 vector31 = new Vector2(76f, 66f);
									Texture2D texture2D13 = Main.extraTexture[13];
									Vector2 origin2 = new Vector2((float)texture2D13.Width, 278f);
									Vector2 origin3 = new Vector2(0f, 278f);
									Vector2 value10 = new Vector2(0f, 76f);
									Vector2 center = Main.npc[i].Center;
									Microsoft.Xna.Framework.Point point = (Main.npc[i].Center + new Vector2(0f, -150f)).ToTileCoordinates();
									Microsoft.Xna.Framework.Color alpha4 = Main.npc[i].GetAlpha(Microsoft.Xna.Framework.Color.Lerp(Lighting.GetColor(point.X, point.Y), Microsoft.Xna.Framework.Color.White, 0.3f));
									for (int num131 = 0; num131 < 2; num131++)
									{
										bool flag11 = num131 == 0;
										Vector2 value11 = new Vector2((float)(flag11 ? -1 : 1), 1f);
										int num132 = -1;
										for (int num133 = 0; num133 < 200; num133++)
										{
											if (Main.npc[num133].active && Main.npc[num133].type == 397 && Main.npc[num133].ai[2] == (float)num131 && Main.npc[num133].ai[3] == (float)i)
											{
												num132 = num133;
												break;
											}
										}
										if (num132 != -1)
										{
											Vector2 vector32 = center + value9 * value11;
											Vector2 value12 = Main.npc[num132].Center + value10;
											Vector2 vector33 = (value12 - vector32) * scaleFactor9;
											if (flag10)
											{
												Main.dust[Dust.NewDust(vector32 + vector33, 0, 0, 6, 0f, 0f, 0, default(Microsoft.Xna.Framework.Color), 1f)].noGravity = true;
											}
											float num134 = (float)Math.Acos((double)(vector33.Length() / num130)) * -value11.X;
											SpriteEffects effects = flag11 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
											Vector2 origin4 = vector31;
											if (!flag11)
											{
												origin4.X = (float)texture2D12.Width - origin4.X;
											}
											Main.spriteBatch.Draw(texture2D12, vector32 - Main.screenPosition, null, alpha4, vector33.ToRotation() - num134 - 1.57079637f, origin4, 1f, effects, 0f);
											if (flag10)
											{
												Main.dust[Dust.NewDust(vector32, 0, 0, 6, 0f, 0f, 0, default(Microsoft.Xna.Framework.Color), 1f)].noGravity = true;
											}
											if (flag10)
											{
												Main.dust[Dust.NewDust(center, 0, 0, 6, 0f, 0f, 0, default(Microsoft.Xna.Framework.Color), 1f)].noGravity = true;
											}
											if (flag10)
											{
												Main.dust[Dust.NewDust(vector32 + new Vector2(0f, num130).RotatedBy((double)(vector33.ToRotation() - num134 - 1.57079637f), default(Vector2)), 0, 0, 6, 0f, 0f, 0, default(Microsoft.Xna.Framework.Color), 1f)].noGravity = true;
											}
										}
									}
									Main.spriteBatch.Draw(texture2D13, center - Main.screenPosition, null, alpha4, 0f, origin2, 1f, SpriteEffects.None, 0f);
									Main.spriteBatch.Draw(texture2D13, center - Main.screenPosition, null, alpha4, 0f, origin3, 1f, SpriteEffects.FlipHorizontally, 0f);
									Main.spriteBatch.Draw(texture3, center - Main.screenPosition, null, alpha4, 0f, new Vector2(112f, 101f), 1f, SpriteEffects.None, 0f);
									Main.spriteBatch.Draw(texture2, center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), alpha4, 0f, Main.npc[i].frame.Size() / 2f, 1f, SpriteEffects.None, 0f);
									return;
								}
								if (type == 397)
								{
									Texture2D texture2D14 = Main.npcTexture[type];
									float num135 = 0.5f;
									Vector2 value13 = new Vector2(220f, -60f);
									Vector2 value14 = new Vector2(0f, 76f);
									Texture2D texture2D15 = Main.extraTexture[15];
									Vector2 vector34 = new Vector2(60f, 30f);
									float num136 = 340f;
									Vector2 center2 = Main.npc[(int)Main.npc[i].ai[3]].Center;
									Microsoft.Xna.Framework.Point point2 = Main.npc[i].Center.ToTileCoordinates();
									Microsoft.Xna.Framework.Color alpha5 = Main.npc[i].GetAlpha(Microsoft.Xna.Framework.Color.Lerp(Lighting.GetColor(point2.X, point2.Y), Microsoft.Xna.Framework.Color.White, 0.3f));
									bool flag12 = Main.npc[i].ai[2] == 0f;
									Vector2 value15 = new Vector2((float)(flag12 ? -1 : 1), 1f);
									Vector2 origin5 = new Vector2(120f, 180f);
									if (!flag12)
									{
										origin5.X = (float)texture2D14.Width - origin5.X;
									}
									Texture2D texture2D16 = Main.extraTexture[17];
									Texture2D texture2D17 = Main.extraTexture[19];
									Vector2 vector35 = new Vector2(26f, 42f);
									if (!flag12)
									{
										vector35.X = (float)texture2D16.Width - vector35.X;
									}
									Vector2 value16 = new Vector2(30f, 66f);
									Vector2 value17 = new Vector2(1f * -value15.X, 3f);
									Texture2D texture2D18 = Main.extraTexture[26];
									Microsoft.Xna.Framework.Rectangle value18 = texture2D18.Frame(1, 1, 0, 0);
									value18.Height /= 4;
									Vector2 value19 = center2 + value13 * value15;
									Vector2 vector36 = Main.npc[i].Center + value14;
									Vector2 vector37 = value19 - vector36;
									vector37 *= 1f - num135;
									Vector2 origin6 = vector34;
									if (!flag12)
									{
										origin6.X = (float)texture2D15.Width - origin6.X;
									}
									float num137 = (float)Math.Acos((double)(vector37.Length() / num136)) * -value15.X;
									Main.spriteBatch.Draw(texture2D15, vector36 - Main.screenPosition, null, alpha5, vector37.ToRotation() + num137 - 1.57079637f, origin6, 1f, spriteEffects, 0f);
									if (Main.npc[i].ai[0] == -2f)
									{
										int num138 = (int)Main.npc[i].ai[1];
										num138 /= 8;
										value18.Y += value18.Height * num138;
										Main.spriteBatch.Draw(texture2D18, Main.npc[i].Center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(value18), alpha5, 0f, vector35 - new Vector2(4f, 4f), 1f, spriteEffects, 0f);
									}
									else
									{
										Main.spriteBatch.Draw(texture2D16, Main.npc[i].Center - Main.screenPosition, null, alpha5, 0f, vector35, 1f, spriteEffects, 0f);
										Vector2 value20 = Utils.Vector2FromElipse(Main.npc[i].localAI[0].ToRotationVector2(), value16 * Main.npc[i].localAI[1]);
										Main.spriteBatch.Draw(texture2D17, Main.npc[i].Center - Main.screenPosition + value20 + value17, null, alpha5, 0f, new Vector2((float)texture2D17.Width, (float)texture2D17.Height) / 2f, 1f, SpriteEffects.None, 0f);
									}
									Main.spriteBatch.Draw(texture2D14, Main.npc[i].Center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), alpha5, 0f, origin5, 1f, spriteEffects, 0f);
									return;
								}
								if (type == 396)
								{
									Texture2D texture4 = Main.npcTexture[type];
									Vector2 origin7 = new Vector2(191f, 130f);
									Texture2D texture5 = Main.extraTexture[18];
									Texture2D texture2D19 = Main.extraTexture[19];
									Vector2 vector38 = new Vector2(19f, 34f);
									Vector2 value21 = new Vector2(27f, 59f);
									Vector2 value22 = new Vector2(0f, 0f);
									Texture2D texture2D20 = Main.extraTexture[25];
									Vector2 value23 = new Vector2(0f, 214f).RotatedBy((double)Main.npc[i].rotation, default(Vector2));
									Microsoft.Xna.Framework.Rectangle rectangle3 = texture2D20.Frame(1, 1, 0, 0);
									rectangle3.Height /= 3;
									rectangle3.Y += rectangle3.Height * (int)(Main.npc[i].localAI[2] / 7f);
									Texture2D texture2D21 = Main.extraTexture[29];
									Vector2 value24 = new Vector2(0f, 4f).RotatedBy((double)Main.npc[i].rotation, default(Vector2));
									Microsoft.Xna.Framework.Rectangle rectangle4 = texture2D21.Frame(1, 1, 0, 0);
									rectangle4.Height /= 4;
									rectangle4.Y += rectangle4.Height * (int)(Main.npc[i].localAI[3] / 5f);
									Texture2D texture2D22 = Main.extraTexture[26];
									Microsoft.Xna.Framework.Rectangle value25 = texture2D22.Frame(1, 1, 0, 0);
									value25.Height /= 4;
									Vector2 arg_7E39_0 = Main.npc[(int)Main.npc[i].ai[3]].Center;
									Microsoft.Xna.Framework.Point point3 = Main.npc[i].Center.ToTileCoordinates();
									Microsoft.Xna.Framework.Color alpha6 = Main.npc[i].GetAlpha(Microsoft.Xna.Framework.Color.Lerp(Lighting.GetColor(point3.X, point3.Y), Microsoft.Xna.Framework.Color.White, 0.3f));
									if (Main.npc[i].ai[0] < 0f)
									{
										int num139 = (int)Main.npc[i].ai[1];
										num139 /= 8;
										value25.Y += value25.Height * num139;
										Main.spriteBatch.Draw(texture2D22, Main.npc[i].Center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(value25), alpha6, Main.npc[i].rotation, vector38 + new Vector2(4f, 4f), 1f, spriteEffects, 0f);
									}
									else
									{
										Main.spriteBatch.Draw(texture5, Main.npc[i].Center - Main.screenPosition, null, alpha6, Main.npc[i].rotation, vector38, 1f, spriteEffects, 0f);
										Vector2 value26 = Utils.Vector2FromElipse(Main.npc[i].localAI[0].ToRotationVector2(), value21 * Main.npc[i].localAI[1]);
										Main.spriteBatch.Draw(texture2D19, Main.npc[i].Center - Main.screenPosition + value26 + value22, null, alpha6, Main.npc[i].rotation, new Vector2((float)texture2D19.Width, (float)texture2D19.Height) / 2f, 1f, SpriteEffects.None, 0f);
									}
									Main.spriteBatch.Draw(texture4, Main.npc[i].Center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), alpha6, Main.npc[i].rotation, origin7, 1f, spriteEffects, 0f);
									Main.spriteBatch.Draw(texture2D21, (Main.npc[i].Center - Main.screenPosition + value24).Floor(), new Microsoft.Xna.Framework.Rectangle?(rectangle4), alpha6, Main.npc[i].rotation, rectangle4.Size() / 2f, 1f, spriteEffects, 0f);
									Main.spriteBatch.Draw(texture2D20, (Main.npc[i].Center - Main.screenPosition + value23).Floor(), new Microsoft.Xna.Framework.Rectangle?(rectangle3), alpha6, Main.npc[i].rotation, rectangle3.Size() / 2f, 1f, spriteEffects, 0f);
									return;
								}
								if (type == 400)
								{
									Texture2D texture6 = Main.npcTexture[type];
									Texture2D texture2D23 = Main.extraTexture[19];
									Vector2 origin8 = new Vector2(40f, 40f);
									Vector2 value27 = new Vector2(30f, 30f);
									Vector2 arg_81C3_0 = Main.npc[i].Center;
									Microsoft.Xna.Framework.Point point4 = Main.npc[i].Center.ToTileCoordinates();
									Microsoft.Xna.Framework.Color alpha7 = Main.npc[i].GetAlpha(Microsoft.Xna.Framework.Color.Lerp(Lighting.GetColor(point4.X, point4.Y), Microsoft.Xna.Framework.Color.White, 0.3f));
									Main.spriteBatch.Draw(texture6, Main.npc[i].Center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), alpha7, Main.npc[i].rotation, origin8, 1f, spriteEffects, 0f);
									Vector2 value28 = Utils.Vector2FromElipse(Main.npc[i].localAI[0].ToRotationVector2(), value27 * Main.npc[i].localAI[1]);
									Main.spriteBatch.Draw(texture2D23, Main.npc[i].Center - Main.screenPosition + value28, null, alpha7, Main.npc[i].rotation, texture2D23.Size() / 2f, Main.npc[i].localAI[2], SpriteEffects.None, 0f);
									return;
								}
								if (type == 384)
								{
									return;
								}
								if (type == 416)
								{
									int num140 = -1;
									int num141 = (int)Main.npc[i].ai[0];
									Vector2 position3 = Main.npc[i].position;
									Vector2 spinningpoint = Vector2.Zero;
									if (Main.npc[num141].active && Main.npc[num141].type == 415)
									{
										num140 = num141;
									}
									if (num140 != -1)
									{
										Vector2 position4 = Main.npc[i].position;
										Main.npc[i].Bottom = Main.npc[num140].Bottom;
										position3 = Main.npc[i].position;
										Main.npc[i].position = position4;
										Main.npc[i].gfxOffY = Main.npc[num140].gfxOffY;
										spinningpoint = Main.npc[num140].velocity;
									}
									Microsoft.Xna.Framework.Rectangle frame = Main.npc[i].frame;
									Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(position3.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, position3.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									if (Main.npc[i].color != default(Microsoft.Xna.Framework.Color))
									{
										Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(position3.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, position3.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame), Main.npc[i].GetColor(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
									Main.spriteBatch.Draw(Main.glowMaskTexture[156], position3 + Main.npc[i].Size * new Vector2(0.5f, 1f) - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									float scaleFactor10 = 0.5f + (Main.npc[i].GetAlpha(color9).ToVector3() - new Vector3(0.5f)).Length() * 0.5f;
									for (int num142 = 0; num142 < 4; num142++)
									{
										Main.spriteBatch.Draw(Main.glowMaskTexture[156], position3 + Main.npc[i].Size * new Vector2(0.5f, 1f) - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY) + spinningpoint.RotatedBy((double)((float)num142 * 1.57079637f), default(Vector2)) * scaleFactor10, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(64, 64, 64, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
									return;
								}
								if (type == 399)
								{
									Texture2D texture2D24 = Main.npcTexture[type];
									Vector2 vec = Main.npc[i].position - Main.screenPosition + Vector2.UnitY * Main.npc[i].gfxOffY;
									vec = vec.Floor();
									float num143 = 5f;
									int num144 = 0;
									while ((float)num144 < num143)
									{
										float num145 = 1f - (Main.GlobalTime + (float)num144) % num143 / num143;
										Microsoft.Xna.Framework.Color color27 = Microsoft.Xna.Framework.Color.LimeGreen;
										if (Main.npc[i].ai[0] == 1f)
										{
											color27 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.LimeGreen, Microsoft.Xna.Framework.Color.Red, MathHelper.Clamp(Main.npc[i].ai[1] / 20f, 0f, 1f));
										}
										if (Main.npc[i].ai[0] == 2f)
										{
											color27 = Microsoft.Xna.Framework.Color.Red;
										}
										color27 *= 1f - num145;
										color27.A = 0;
										for (int num146 = 0; num146 < 2; num146++)
										{
											Main.spriteBatch.Draw(Main.extraTexture[27], Main.npc[i].Center - Main.screenPosition + Vector2.UnitY * (Main.npc[i].gfxOffY - 4f + 6f), null, color27, 1.57079637f, new Vector2(10f, 48f), num145 * 4f, SpriteEffects.None, 0f);
										}
										num144++;
									}
									Main.spriteBatch.Draw(texture2D24, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									texture2D24 = Main.glowMaskTexture[100];
									Main.spriteBatch.Draw(texture2D24, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(127 - Main.npc[i].alpha / 2, 127 - Main.npc[i].alpha / 2, 127 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									texture2D24 = Main.extraTexture[20];
									Microsoft.Xna.Framework.Rectangle value29 = texture2D24.Frame(1, 4, 0, (int)Main.npc[i].ai[0] + 1);
									Vector2 position5 = new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)texture2D24.Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY + 18f + 6f);
									Main.spriteBatch.Draw(texture2D24, position5, new Microsoft.Xna.Framework.Rectangle?(value29), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									texture2D24 = Main.glowMaskTexture[101];
									Main.spriteBatch.Draw(texture2D24, position5, new Microsoft.Xna.Framework.Rectangle?(value29), new Microsoft.Xna.Framework.Color(127 - Main.npc[i].alpha / 2, 127 - Main.npc[i].alpha / 2, 127 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									return;
								}
								if (type == 94)
								{
									for (int num147 = 1; num147 < 6; num147 += 2)
									{
										Vector2 arg_8FF2_0 = Main.npc[i].oldPos[num147];
										Microsoft.Xna.Framework.Color alpha8 = Main.npc[i].GetAlpha(color9);
										alpha8.R = (byte)((int)alpha8.R * (10 - num147) / 15);
										alpha8.G = (byte)((int)alpha8.G * (10 - num147) / 15);
										alpha8.B = (byte)((int)alpha8.B * (10 - num147) / 15);
										alpha8.A = (byte)((int)alpha8.A * (10 - num147) / 15);
										Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].oldPos[num147].X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].oldPos[num147].Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), alpha8, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								if (type == 125 || type == 126 || type == 127 || type == 128 || type == 129 || type == 130 || type == 131 || type == 139 || type == 140)
								{
									for (int num148 = 9; num148 >= 0; num148 -= 2)
									{
										Vector2 arg_9224_0 = Main.npc[i].oldPos[num148];
										Microsoft.Xna.Framework.Color alpha9 = Main.npc[i].GetAlpha(color9);
										alpha9.R = (byte)((int)alpha9.R * (10 - num148) / 20);
										alpha9.G = (byte)((int)alpha9.G * (10 - num148) / 20);
										alpha9.B = (byte)((int)alpha9.B * (10 - num148) / 20);
										alpha9.A = (byte)((int)alpha9.A * (10 - num148) / 20);
										Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].oldPos[num148].X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].oldPos[num148].Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), alpha9, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								if (type == 417 && Main.npc[i].ai[0] >= 6f && Main.npc[i].ai[0] <= 6f)
								{
									for (int num149 = 5; num149 >= 0; num149--)
									{
										Vector2 arg_944E_0 = Main.npc[i].oldPos[num149];
										Microsoft.Xna.Framework.Color alpha10 = Main.npc[i].GetAlpha(color9);
										alpha10.R = (byte)((int)alpha10.R * (10 - num149) / 20);
										alpha10.G = (byte)((int)alpha10.G * (10 - num149) / 20);
										alpha10.B = (byte)((int)alpha10.B * (10 - num149) / 20);
										alpha10.A = (byte)((int)alpha10.A * (10 - num149) / 20);
										Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].oldPos[num149].X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].oldPos[num149].Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), alpha10, Main.npc[i].oldRot[num149], vector10, MathHelper.Lerp(0.5f, 1f, (5f - (float)num149) / 6f), spriteEffects, 0f);
									}
								}
								if (type == 419 && Main.npc[i].ai[2] <= -9f)
								{
									int num150 = Main.glowMaskTexture[154].Height / Main.npcFrameCount[type];
									int num151 = Main.npc[i].frame.Y / num150;
									for (int num152 = 6; num152 >= 0; num152--)
									{
										Vector2 arg_96AF_0 = Main.npc[i].oldPos[num152];
										Microsoft.Xna.Framework.Color white2 = Microsoft.Xna.Framework.Color.White;
										white2.R = (byte)(255 * (10 - num152) / 20);
										white2.G = (byte)(255 * (10 - num152) / 20);
										white2.B = (byte)(255 * (10 - num152) / 20);
										white2.A = 0;
										Microsoft.Xna.Framework.Rectangle frame2 = Main.npc[i].frame;
										int num153 = (num151 - 3 - num152) % 3;
										if (num153 < 0)
										{
											num153 += 3;
										}
										num153 += 5;
										frame2.Y = num150 * num153;
										Main.spriteBatch.Draw(Main.glowMaskTexture[154], new Vector2(Main.npc[i].oldPos[num152].X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].oldPos[num152].Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(frame2), white2, Main.npc[i].oldRot[num152], vector10, MathHelper.Lerp(0.75f, 1.2f, (10f - (float)num152) / 10f), spriteEffects, 0f);
									}
								}
								if (type == 418 && (Main.npc[i].ai[0] == 2f || Main.npc[i].ai[0] == 4f))
								{
									Texture2D texture2D25 = Main.extraTexture[55];
									Vector2 origin9 = new Vector2((float)(texture2D25.Width / 2), (float)(texture2D25.Height / 8 + 14));
									int num154 = (int)Main.npc[i].ai[1] / 2;
									float num155 = -1.57079637f * (float)Main.npc[i].spriteDirection;
									float num156 = Main.npc[i].ai[1] / 45f;
									if (num156 > 1f)
									{
										num156 = 1f;
									}
									num154 %= 4;
									for (int num157 = 6; num157 >= 0; num157--)
									{
										Vector2 arg_99A2_0 = Main.npc[i].oldPos[num157];
										Microsoft.Xna.Framework.Color color28 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.Gold, Microsoft.Xna.Framework.Color.OrangeRed, num156);
										color28 = Microsoft.Xna.Framework.Color.Lerp(color28, Microsoft.Xna.Framework.Color.Blue, (float)num157 / 12f);
										color28.A = (byte)(64f * num156);
										color28.R = (byte)((int)color28.R * (10 - num157) / 20);
										color28.G = (byte)((int)color28.G * (10 - num157) / 20);
										color28.B = (byte)((int)color28.B * (10 - num157) / 20);
										color28.A = (byte)((int)color28.A * (10 - num157) / 20);
										color28 *= num156;
										int num158 = (num154 - num157) % 4;
										if (num158 < 0)
										{
											num158 += 4;
										}
										Microsoft.Xna.Framework.Rectangle value30 = texture2D25.Frame(1, 4, 0, num158);
										Main.spriteBatch.Draw(texture2D25, new Vector2(Main.npc[i].oldPos[num157].X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].oldPos[num157].Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(value30), color28, Main.npc[i].oldRot[num157] + num155, origin9, MathHelper.Lerp(0.1f, 1.2f, (10f - (float)num157) / 10f), spriteEffects, 0f);
									}
								}
								if (type == 516)
								{
									int num159 = Main.npcTexture[type].Height / Main.npcFrameCount[type];
									int num160 = Main.npc[i].frame.Y / num159;
									for (int num161 = 6; num161 >= 0; num161--)
									{
										Vector2 arg_9C5E_0 = Main.npc[i].oldPos[num161];
										Microsoft.Xna.Framework.Color color29 = Microsoft.Xna.Framework.Color.White;
										color29.R = (byte)(255 * (10 - num161) / 20);
										color29.G = (byte)(255 * (10 - num161) / 20);
										color29.B = (byte)(255 * (10 - num161) / 20);
										color29.A = (byte)(255 * (10 - num161) / 20);
										color29 = Microsoft.Xna.Framework.Color.Lerp(color29, Microsoft.Xna.Framework.Color.Transparent, (float)num161 / 6f);
										Microsoft.Xna.Framework.Rectangle frame3 = Main.npc[i].frame;
										int num162 = (num160 - 4 - num161) % 4;
										if (num162 < 0)
										{
											num162 += 4;
										}
										frame3.Y = num159 * num162;
										Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].oldPos[num161].X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].oldPos[num161].Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(frame3), color29, Main.npc[i].rotation, vector10, MathHelper.Lerp(0.35f, 1.2f, (10f - (float)num161) / 10f), spriteEffects, 0f);
									}
								}
								Microsoft.Xna.Framework.Rectangle frame4 = Main.npc[i].frame;
								if (type == 182 || type == 289)
								{
									frame4.Height -= 2;
								}
								if (Main.npc[i].aiStyle == 7)
								{
									NPC n2 = Main.npc[i];
									this.DrawNPCExtras(n2, true, num67, num66, color9, vector10, spriteEffects);
								}
								if (type == 346 && (double)Main.npc[i].life < (double)Main.npc[i].lifeMax * 0.5)
								{
									Main.spriteBatch.Draw(Main.santaTankTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 356)
								{
									frame4.Height--;
									Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 360)
								{
									float num163 = 0f;
									if (Main.npc[i].ai[2] == 0f)
									{
										if (Main.npc[i].rotation == 3.14f || Main.npc[i].rotation == -3.14f)
										{
											num67 = 2f;
										}
										if (Main.npc[i].direction < 0 && (Main.npc[i].rotation == 1.57f || Main.npc[i].rotation == 4.71f))
										{
											num163 = 1f;
										}
										if (Main.npc[i].direction > 0 && (Main.npc[i].rotation == 1.57f || Main.npc[i].rotation == 4.71f))
										{
											num163 = -1f;
										}
									}
									Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale + num163, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 266 && Main.npc[i].life < Main.npc[i].lifeMax && Main.expertMode)
								{
									Microsoft.Xna.Framework.Color alpha11 = Main.npc[i].GetAlpha(color9);
									float num164 = 1f - (float)Main.npc[i].life / (float)Main.npc[i].lifeMax;
									num164 *= num164;
									alpha11.R = (byte)((float)alpha11.R * num164);
									alpha11.G = (byte)((float)alpha11.G * num164);
									alpha11.B = (byte)((float)alpha11.B * num164);
									alpha11.A = (byte)((float)alpha11.A * num164);
									for (int num165 = 0; num165 < 4; num165++)
									{
										Vector2 position6 = Main.npc[i].position;
										float num166 = Math.Abs(Main.npc[i].Center.X - Main.player[Main.myPlayer].Center.X);
										float num167 = Math.Abs(Main.npc[i].Center.Y - Main.player[Main.myPlayer].Center.Y);
										if (num165 == 0 || num165 == 2)
										{
											position6.X = Main.player[Main.myPlayer].Center.X + num166;
										}
										else
										{
											position6.X = Main.player[Main.myPlayer].Center.X - num166;
										}
										position6.X -= (float)(Main.npc[i].width / 2);
										if (num165 == 0 || num165 == 1)
										{
											position6.Y = Main.player[Main.myPlayer].Center.Y + num167;
										}
										else
										{
											position6.Y = Main.player[Main.myPlayer].Center.Y - num167;
										}
										position6.Y -= (float)(Main.npc[i].height / 2);
										Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(position6.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, position6.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), alpha11, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
									Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 421 && Main.npc[i].ai[0] == 5f)
								{
									Player player = Main.player[Main.npc[i].target];
									if (player.gravDir == -1f)
									{
										spriteEffects |= SpriteEffects.FlipVertically;
									}
									Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2((float)(player.direction * 4), player.gfxOffY) + ((player.gravDir == 1f) ? player.Top : player.Bottom) - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(frame4), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, frame4.Size() / 2f, Main.npc[i].scale, spriteEffects, 0f);
									Main.spriteBatch.Draw(Main.glowMaskTexture[146], new Vector2((float)(player.direction * 4), player.gfxOffY) + ((player.gravDir == 1f) ? player.Top : player.Bottom) - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(frame4), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, frame4.Size() / 2f, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 518)
								{
									Vector2 value31 = new Vector2(-10f, 0f);
									Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10 + value31, Main.npc[i].scale, spriteEffects, 0f);
									if (Main.npc[i].color != default(Microsoft.Xna.Framework.Color))
									{
										Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), Main.npc[i].GetColor(color9), Main.npc[i].rotation, vector10 + value31, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else
								{
									Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), Main.npc[i].GetAlpha(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									if (Main.npc[i].color != default(Microsoft.Xna.Framework.Color))
									{
										Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), Main.npc[i].GetColor(color9), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								if (Main.npc[i].confused)
								{
									Main.spriteBatch.Draw(Main.confuseTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 - (float)Main.confuseTexture.Height - 20f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.confuseTexture.Width, Main.confuseTexture.Height)), new Microsoft.Xna.Framework.Color(250, 250, 250, 70), Main.npc[i].velocity.X * -0.05f, new Vector2((float)(Main.confuseTexture.Width / 2), (float)(Main.confuseTexture.Height / 2)), Main.essScale + 0.2f, SpriteEffects.None, 0f);
								}
								if (type >= 134 && type <= 136 && color9 != Microsoft.Xna.Framework.Color.Black)
								{
									Main.spriteBatch.Draw(Main.destTexture[type - 134], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255, 255, 255, 0) * (1f - (float)Main.npc[i].alpha / 255f), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 125)
								{
									Main.spriteBatch.Draw(Main.EyeLaserTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 139)
								{
									Main.spriteBatch.Draw(Main.probeTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 127)
								{
									Main.spriteBatch.Draw(Main.BoneEyesTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(200, 200, 200, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 131)
								{
									Main.spriteBatch.Draw(Main.BoneLaserTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(200, 200, 200, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 120)
								{
									for (int num168 = 1; num168 < Main.npc[i].oldPos.Length; num168++)
									{
										Vector2 arg_B717_0 = Main.npc[i].oldPos[num168];
										Microsoft.Xna.Framework.Color color30 = default(Microsoft.Xna.Framework.Color);
										color30.R = (byte)(150 * (10 - num168) / 15);
										color30.G = (byte)(100 * (10 - num168) / 15);
										color30.B = (byte)(150 * (10 - num168) / 15);
										color30.A = (byte)(50 * (10 - num168) / 15);
										Main.spriteBatch.Draw(Main.chaosTexture, new Vector2(Main.npc[i].oldPos[num168].X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].oldPos[num168].Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color30, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 137 || type == 138)
								{
									for (int num169 = 1; num169 < Main.npc[i].oldPos.Length; num169++)
									{
										Vector2 arg_B90B_0 = Main.npc[i].oldPos[num169];
										Microsoft.Xna.Framework.Color color31 = default(Microsoft.Xna.Framework.Color);
										color31.R = (byte)(150 * (10 - num169) / 15);
										color31.G = (byte)(100 * (10 - num169) / 15);
										color31.B = (byte)(150 * (10 - num169) / 15);
										color31.A = (byte)(50 * (10 - num169) / 15);
										Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].oldPos[num169].X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].oldPos[num169].Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color31, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 327)
								{
									Main.spriteBatch.Draw(Main.pumpkingFaceTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Microsoft.Xna.Framework.Color.White, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									for (int num170 = 1; num170 < 10; num170++)
									{
										Microsoft.Xna.Framework.Color color32 = new Microsoft.Xna.Framework.Color(110 - num170 * 10, 110 - num170 * 10, 110 - num170 * 10, 110 - num170 * 10);
										Vector2 value32 = new Vector2((float)Main.rand.Next(-10, 11) * 0.2f, (float)Main.rand.Next(-10, 11) * 0.2f);
										Main.spriteBatch.Draw(Main.pumpkingFaceTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67) + value32, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color32, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 325)
								{
									Main.spriteBatch.Draw(Main.treeFaceTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Microsoft.Xna.Framework.Color.White, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									for (int num171 = 1; num171 < 10; num171++)
									{
										Microsoft.Xna.Framework.Color color33 = new Microsoft.Xna.Framework.Color(110 - num171 * 10, 110 - num171 * 10, 110 - num171 * 10, 110 - num171 * 10);
										Vector2 value33 = new Vector2((float)Main.rand.Next(-10, 11) * 0.2f, (float)Main.rand.Next(-10, 11) * 0.2f);
										Main.spriteBatch.Draw(Main.treeFaceTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67) + value33, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color33, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 345)
								{
									Main.spriteBatch.Draw(Main.iceQueenTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Microsoft.Xna.Framework.Color.White, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									for (int num172 = 1; num172 < 5; num172++)
									{
										Microsoft.Xna.Framework.Color color34 = new Microsoft.Xna.Framework.Color(100 - num172 * 10, 100 - num172 * 10, 100 - num172 * 10, 100 - num172 * 10);
										Main.spriteBatch.Draw(Main.iceQueenTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67) - Main.npc[i].velocity * (float)num172 * 0.2f, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color34, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 355)
								{
									Main.spriteBatch.Draw(Main.fireflyTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 358)
								{
									Main.spriteBatch.Draw(Main.lightningbugTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 82)
								{
									Main.spriteBatch.Draw(Main.wraithEyeTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Microsoft.Xna.Framework.Color.White, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									for (int num173 = 1; num173 < 10; num173++)
									{
										Microsoft.Xna.Framework.Color color35 = new Microsoft.Xna.Framework.Color(110 - num173 * 10, 110 - num173 * 10, 110 - num173 * 10, 110 - num173 * 10);
										Main.spriteBatch.Draw(Main.wraithEyeTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67) - Main.npc[i].velocity * (float)num173 * 0.5f, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color35, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 253)
								{
									Main.spriteBatch.Draw(Main.reaperEyeTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 3f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), Microsoft.Xna.Framework.Color.White, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									for (int num174 = 1; num174 < 20; num174++)
									{
										Microsoft.Xna.Framework.Color color36 = new Microsoft.Xna.Framework.Color(210 - num174 * 20, 210 - num174 * 20, 210 - num174 * 20, 210 - num174 * 20);
										Main.spriteBatch.Draw(Main.reaperEyeTexture, new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 3f + vector10.Y * Main.npc[i].scale + num67) - Main.npc[i].velocity * (float)num174 * 0.5f, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color36, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 245 && Main.npc[i].alpha == 0)
								{
									Microsoft.Xna.Framework.Color color37 = new Microsoft.Xna.Framework.Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 0);
									Main.spriteBatch.Draw(Main.golemTexture[3], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), color37, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 246)
								{
									Microsoft.Xna.Framework.Color color38 = new Microsoft.Xna.Framework.Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 0);
									if (Main.npc[i].frame.Y < 222)
									{
										Main.spriteBatch.Draw(Main.golemTexture[1], new Vector2(Main.npc[i].Center.X - Main.screenPosition.X - 20f, Main.npc[i].Center.Y - Main.screenPosition.Y - 27f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.golemTexture[1].Width, Main.golemTexture[1].Height / 2)), color38, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
									}
									else if (Main.npc[i].frame.Y < 444)
									{
										Main.spriteBatch.Draw(Main.golemTexture[2], new Vector2(Main.npc[i].Center.X - Main.screenPosition.X + 26f, Main.npc[i].Center.Y - Main.screenPosition.Y - 28f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.golemTexture[2].Width, Main.golemTexture[2].Height / 4)), color38, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
									}
									else
									{
										Main.spriteBatch.Draw(Main.golemTexture[2], new Vector2(Main.npc[i].Center.X - Main.screenPosition.X - 38f, Main.npc[i].Center.Y - Main.screenPosition.Y - 28f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, Main.golemTexture[2].Height / 2, Main.golemTexture[2].Width, Main.golemTexture[2].Height / 4)), color38, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
									}
								}
								else if (type == 249)
								{
									Microsoft.Xna.Framework.Color color39 = new Microsoft.Xna.Framework.Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 0);
									Main.spriteBatch.Draw(Main.golemTexture[1], new Vector2(Main.npc[i].Center.X - Main.screenPosition.X - 20f, Main.npc[i].Center.Y - Main.screenPosition.Y - 47f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.golemTexture[1].Width, Main.golemTexture[1].Height / 2)), color39, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
								}
								else if (type == 383)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[11], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									if (Main.npc[i].ai[2] != 0f && Main.npc[(int)Main.npc[i].ai[2] - 1].active && Main.npc[(int)Main.npc[i].ai[2] - 1].type == 384)
									{
										float arg_D12F_0 = Main.npc[i].ai[2];
										Main.spriteBatch.Draw(Main.npcTexture[384], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), null, new Microsoft.Xna.Framework.Color(100, 100, 100, 0), Main.npc[i].rotation, new Vector2((float)Main.npcTexture[384].Width, (float)Main.npcTexture[384].Height) / 2f, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 381)
								{
									Vector2 vector39 = Vector2.Zero;
									Vector2 zero3 = Vector2.Zero;
									int num175 = Main.npcTexture[type].Height / Main.npcFrameCount[type];
									int num176 = Main.npc[i].frame.Y / num175;
									Microsoft.Xna.Framework.Rectangle value34 = new Microsoft.Xna.Framework.Rectangle(0, 0, 32, 42);
									switch (num176)
									{
									case 0:
										vector39 += new Vector2(8f, 32f);
										break;
									case 1:
										vector39 += new Vector2(6f, 72f);
										break;
									case 2:
										vector39 += new Vector2(8f, 126f);
										break;
									case 3:
										vector39 += new Vector2(6f, 174f);
										break;
									case 4:
										vector39 += new Vector2(6f, 224f);
										break;
									case 5:
										vector39 += new Vector2(8f, 272f);
										break;
									case 6:
										vector39 += new Vector2(10f, 318f);
										break;
									case 7:
										vector39 += new Vector2(14f, 366f);
										break;
									case 8:
										vector39 += new Vector2(10f, 414f);
										break;
									}
									vector39.Y -= (float)(num175 * num176);
									vector39 -= vector10;
									int num177 = 2;
									if (Main.npc[i].ai[2] > 0f)
									{
										num177 = (int)Main.npc[i].ai[2] - 1;
									}
									if (Main.npc[i].velocity.Y != 0f)
									{
										num177 = 3;
									}
									value34.Y += 44 * num177;
									switch (num177)
									{
									case 0:
										zero3 = new Vector2(10f, 18f);
										break;
									case 1:
										zero3 = new Vector2(8f, 20f);
										break;
									case 2:
										zero3 = new Vector2(8f, 20f);
										break;
									case 3:
										zero3 = new Vector2(8f, 20f);
										break;
									case 4:
										zero3 = new Vector2(6f, 18f);
										break;
									}
									if (spriteEffects.HasFlag(SpriteEffects.FlipHorizontally))
									{
										vector39.X *= -1f;
										zero3.X = (float)value34.Width - zero3.X;
									}
									vector39 += Main.npc[i].Center;
									vector39 -= Main.screenPosition;
									vector39.Y += Main.npc[i].gfxOffY;
									Main.spriteBatch.Draw(Main.extraTexture[0], vector39, new Microsoft.Xna.Framework.Rectangle?(value34), color9, Main.npc[i].rotation, zero3, Main.npc[i].scale, spriteEffects, 0f);
									Main.spriteBatch.Draw(Main.glowMaskTexture[24], vector39, new Microsoft.Xna.Framework.Rectangle?(value34), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), Main.npc[i].rotation, zero3, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 382)
								{
									Vector2 vector40 = Vector2.Zero;
									Vector2 zero4 = Vector2.Zero;
									int num178 = Main.npcTexture[type].Height / Main.npcFrameCount[type];
									int num179 = Main.npc[i].frame.Y / num178;
									Microsoft.Xna.Framework.Rectangle value35 = new Microsoft.Xna.Framework.Rectangle(0, 0, 30, 42);
									switch (num179)
									{
									case 0:
										vector40 += new Vector2(8f, 30f);
										break;
									case 1:
										vector40 += new Vector2(6f, 68f);
										break;
									case 2:
										vector40 += new Vector2(8f, 120f);
										break;
									case 3:
										vector40 += new Vector2(6f, 166f);
										break;
									case 4:
										vector40 += new Vector2(6f, 214f);
										break;
									case 5:
										vector40 += new Vector2(8f, 260f);
										break;
									case 6:
										vector40 += new Vector2(14f, 304f);
										break;
									case 7:
										vector40 += new Vector2(14f, 350f);
										break;
									case 8:
										vector40 += new Vector2(10f, 396f);
										break;
									}
									vector40.Y -= (float)(num178 * num179);
									vector40 -= vector10;
									int num180 = 2;
									if (Main.npc[i].ai[2] > 0f)
									{
										num180 = (int)Main.npc[i].ai[2] - 1;
									}
									if (Main.npc[i].velocity.Y != 0f)
									{
										num180 = 3;
									}
									value35.Y += 44 * num180;
									switch (num180)
									{
									case 0:
										zero4 = new Vector2(10f, 18f);
										break;
									case 1:
										zero4 = new Vector2(8f, 20f);
										break;
									case 2:
										zero4 = new Vector2(8f, 20f);
										break;
									case 3:
										zero4 = new Vector2(8f, 20f);
										break;
									case 4:
										zero4 = new Vector2(6f, 18f);
										break;
									}
									if (spriteEffects.HasFlag(SpriteEffects.FlipHorizontally))
									{
										vector40.X *= -1f;
										zero4.X = (float)value35.Width - zero4.X;
									}
									vector40 += Main.npc[i].Center;
									vector40 -= Main.screenPosition;
									vector40.Y += Main.npc[i].gfxOffY;
									Main.spriteBatch.Draw(Main.extraTexture[1], vector40, new Microsoft.Xna.Framework.Rectangle?(value35), color9, Main.npc[i].rotation, zero4, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 520)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[164], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									Vector2 vector41 = Vector2.Zero;
									Vector2 origin10 = new Vector2(4f, 4f);
									int num181 = Main.npcTexture[type].Height / Main.npcFrameCount[type];
									int arg_DBB8_0 = Main.npc[i].frame.Y / num181;
									if (spriteEffects.HasFlag(SpriteEffects.FlipHorizontally))
									{
										vector41.X *= -1f;
										origin10.X = (float)Main.extraTexture[56].Width - origin10.X;
									}
									vector41 += Main.npc[i].Top + new Vector2(0f, 20f);
									vector41 -= Main.screenPosition;
									vector41.Y += Main.npc[i].gfxOffY;
									float num182 = Main.npc[i].localAI[3];
									if (spriteEffects.HasFlag(SpriteEffects.FlipHorizontally))
									{
										num182 += 3.14159274f;
									}
									Main.spriteBatch.Draw(Main.extraTexture[56], vector41, null, color9, num182, origin10, Main.npc[i].scale, spriteEffects, 0f);
									Main.spriteBatch.Draw(Main.glowMaskTexture[165], vector41, null, new Microsoft.Xna.Framework.Color(255, 255, 255, 0), num182, origin10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 386)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[31], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 387)
								{
									Microsoft.Xna.Framework.Color color40 = new Microsoft.Xna.Framework.Color(1f, 1f, 1f, 1f) * 0.75f;
									if (Main.npc[i].ai[0] > 0f)
									{
										float amount5 = (Main.npc[i].ai[0] + 1f) / 60f;
										color40 = Microsoft.Xna.Framework.Color.Lerp(color40, Microsoft.Xna.Framework.Color.White, amount5);
										color40.A = (byte)MathHelper.Lerp((float)color40.A, 0f, amount5);
									}
									color40 *= (255f - (float)Main.npc[i].alpha) / 255f;
									Main.spriteBatch.Draw(Main.glowMaskTexture[32], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), color40, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 388)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[33], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 389)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[34], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), new Microsoft.Xna.Framework.Color(255, 255, 255, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 4 && Main.npc[i].ai[1] >= 4f && Main.npc[i].ai[0] == 3f)
								{
									for (int num183 = 1; num183 < Main.npc[i].oldPos.Length; num183++)
									{
										Vector2 arg_E341_0 = Main.npc[i].oldPos[num183];
										Microsoft.Xna.Framework.Color color41 = color9;
										color41.R = (byte)(0.5 * (double)color41.R * (double)(10 - num183) / 20.0);
										color41.G = (byte)(0.5 * (double)color41.G * (double)(10 - num183) / 20.0);
										color41.B = (byte)(0.5 * (double)color41.B * (double)(10 - num183) / 20.0);
										color41.A = (byte)(0.5 * (double)color41.A * (double)(10 - num183) / 20.0);
										Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].oldPos[num183].X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].oldPos[num183].Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color41, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 437)
								{
									Microsoft.Xna.Framework.Color white3 = Microsoft.Xna.Framework.Color.White;
									white3.A = 200;
									Main.spriteBatch.Draw(Main.glowMaskTexture[109], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame4), white3, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									Main.spriteBatch.Draw(Main.glowMaskTexture[108], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + num66 + Main.npc[i].gfxOffY), null, white3, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 471 && Main.npc[i].ai[3] < 0f)
								{
									for (int num184 = 1; num184 < Main.npc[i].oldPos.Length; num184++)
									{
										Vector2 arg_E817_0 = Main.npc[i].oldPos[num184];
										Microsoft.Xna.Framework.Color color42 = color9;
										color42.R = (byte)(0.5 * (double)color42.R * (double)(10 - num184) / 20.0);
										color42.G = (byte)(0.5 * (double)color42.G * (double)(10 - num184) / 20.0);
										color42.B = (byte)(0.5 * (double)color42.B * (double)(10 - num184) / 20.0);
										color42.A = (byte)(0.5 * (double)color42.A * (double)(10 - num184) / 20.0);
										Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].oldPos[num184].X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].oldPos[num184].Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color42, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 477 && Main.npc[i].velocity.Length() > 9f)
								{
									for (int num185 = 1; num185 < Main.npc[i].oldPos.Length; num185++)
									{
										Vector2 arg_EA7E_0 = Main.npc[i].oldPos[num185];
										Microsoft.Xna.Framework.Color color43 = color9;
										color43.R = (byte)(0.5 * (double)color43.R * (double)(10 - num185) / 20.0);
										color43.G = (byte)(0.5 * (double)color43.G * (double)(10 - num185) / 20.0);
										color43.B = (byte)(0.5 * (double)color43.B * (double)(10 - num185) / 20.0);
										color43.A = (byte)(0.5 * (double)color43.A * (double)(10 - num185) / 20.0);
										Microsoft.Xna.Framework.Rectangle frame5 = Main.npc[i].frame;
										int num186 = Main.npcTexture[type].Height / Main.npcFrameCount[type];
										frame5.Y -= num186 * num185;
										while (frame5.Y < 0)
										{
											frame5.Y += num186 * Main.npcFrameCount[type];
										}
										Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].oldPos[num185].X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].oldPos[num185].Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(frame5), color43, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								if (type == 479 && (double)Main.npc[i].velocity.Length() > 6.5)
								{
									for (int num187 = 1; num187 < Main.npc[i].oldPos.Length; num187++)
									{
										Vector2 arg_ED48_0 = Main.npc[i].oldPos[num187];
										Microsoft.Xna.Framework.Color color44 = color9;
										color44.R = (byte)(0.5 * (double)color44.R * (double)(10 - num187) / 20.0);
										color44.G = (byte)(0.5 * (double)color44.G * (double)(10 - num187) / 20.0);
										color44.B = (byte)(0.5 * (double)color44.B * (double)(10 - num187) / 20.0);
										color44.A = (byte)(0.5 * (double)color44.A * (double)(10 - num187) / 20.0);
										Microsoft.Xna.Framework.Rectangle frame6 = Main.npc[i].frame;
										int num188 = Main.npcTexture[type].Height / Main.npcFrameCount[type];
										frame6.Y -= num188 * num187;
										while (frame6.Y < 0)
										{
											frame6.Y += num188 * Main.npcFrameCount[type];
										}
										Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].oldPos[num187].X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].oldPos[num187].Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(frame6), color44, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 472)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[110], new Vector2(Main.npc[i].position.X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].position.Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (Main.npc[i].aiStyle == 87)
								{
									if ((int)Main.npc[i].ai[0] == 4 || Main.npc[i].ai[0] == 5f || Main.npc[i].ai[0] == 6f)
									{
										for (int num189 = 1; num189 < Main.npc[i].oldPos.Length; num189++)
										{
											Vector2 arg_F1A1_0 = Main.npc[i].oldPos[num189];
											Microsoft.Xna.Framework.Color color45 = color9;
											color45.R = (byte)(0.5 * (double)color45.R * (double)(10 - num189) / 20.0);
											color45.G = (byte)(0.5 * (double)color45.G * (double)(10 - num189) / 20.0);
											color45.B = (byte)(0.5 * (double)color45.B * (double)(10 - num189) / 20.0);
											color45.A = (byte)(0.5 * (double)color45.A * (double)(10 - num189) / 20.0);
											Main.spriteBatch.Draw(Main.npcTexture[type], new Vector2(Main.npc[i].oldPos[num189].X - Main.screenPosition.X + (float)(Main.npc[i].width / 2) - (float)Main.npcTexture[type].Width * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, Main.npc[i].oldPos[num189].Y - Main.screenPosition.Y + (float)Main.npc[i].height - (float)Main.npcTexture[type].Height * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color45, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
										}
									}
								}
								else if (type == 50)
								{
									Texture2D texture2D26 = Main.extraTexture[39];
									Vector2 center3 = Main.npc[i].Center;
									float num190 = 0f;
									switch (Main.npc[i].frame.Y / (Main.npcTexture[type].Height / Main.npcFrameCount[type]))
									{
									case 0:
										num190 = 2f;
										break;
									case 1:
										num190 = -6f;
										break;
									case 2:
										num190 = 2f;
										break;
									case 3:
										num190 = 10f;
										break;
									case 4:
										num190 = 2f;
										break;
									case 5:
										num190 = 0f;
										break;
									}
									center3.Y += Main.npc[i].gfxOffY - (70f - num190) * Main.npc[i].scale;
									Main.spriteBatch.Draw(texture2D26, center3 - Main.screenPosition, null, color9, 0f, texture2D26.Size() / 2f, 1f, spriteEffects, 0f);
								}
								else if (type == 411)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[136], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 409)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[138], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 410)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[137], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 407)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[139], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 405)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[141], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 406)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[142], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 424)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[144], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 423)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[145], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 421)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[146], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 420)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[147], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 425)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[150], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 429)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[151], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 418)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[161], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									float scaleFactor11 = 0.25f + (Main.npc[i].GetAlpha(color9).ToVector3() - new Vector3(0.5f)).Length() * 0.25f;
									for (int num191 = 0; num191 < 4; num191++)
									{
										Main.spriteBatch.Draw(Main.glowMaskTexture[161], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY) + Main.npc[i].velocity.RotatedBy((double)((float)num191 * 1.57079637f), default(Vector2)) * scaleFactor11, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(64, 64, 64, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type >= 412 && type <= 414)
								{
									Microsoft.Xna.Framework.Color color46 = new Microsoft.Xna.Framework.Color(255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 0);
									int num192 = 157 + type - 412;
									if (type == 414 && Main.npc[i].localAI[2] != 0f)
									{
										int num193 = (int)Main.npc[i].localAI[2];
										if (Main.npc[i].localAI[2] < 0f)
										{
											num193 = 128 + (int)Main.npc[i].localAI[2];
										}
										int num194 = 255 - num193;
										color46 = new Microsoft.Xna.Framework.Color(num194, num193, num193, num194);
									}
									Main.spriteBatch.Draw(Main.glowMaskTexture[num192], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), color46, Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 415)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[155], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									float scaleFactor12 = 0.5f + (Main.npc[i].GetAlpha(color9).ToVector3() - new Vector3(0.5f)).Length() * 0.5f;
									for (int num195 = 0; num195 < 4; num195++)
									{
										Main.spriteBatch.Draw(Main.glowMaskTexture[155], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY) + Main.npc[i].velocity.RotatedBy((double)((float)num195 * 1.57079637f), default(Vector2)) * scaleFactor12, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(64, 64, 64, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 419)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[154], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									if (Main.npc[i].ai[2] >= -6f)
									{
										float scaleFactor13 = 0.5f + (Main.npc[i].GetAlpha(color9).ToVector3() - new Vector3(0.5f)).Length() * 0.5f;
										for (int num196 = 0; num196 < 4; num196++)
										{
											Main.spriteBatch.Draw(Main.glowMaskTexture[154], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY) + Main.npc[i].velocity.RotatedBy((double)((float)num196 * 1.57079637f), default(Vector2)) * scaleFactor13, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(64, 64, 64, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
										}
									}
									else
									{
										float scaleFactor14 = 4f;
										for (int num197 = 0; num197 < 4; num197++)
										{
											Main.spriteBatch.Draw(Main.glowMaskTexture[154], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY) + Vector2.UnitX.RotatedBy((double)((float)num197 * 1.57079637f), default(Vector2)) * scaleFactor14, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(64, 64, 64, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
										}
									}
								}
								else if (type == 417)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[160], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									float scaleFactor15 = 0.25f + (Main.npc[i].GetAlpha(color9).ToVector3() - new Vector3(0.5f)).Length() * 0.25f;
									for (int num198 = 0; num198 < 4; num198++)
									{
										Main.spriteBatch.Draw(Main.glowMaskTexture[160], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY) + Main.npc[i].velocity.RotatedBy((double)((float)num198 * 1.57079637f), default(Vector2)) * scaleFactor15, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(64, 64, 64, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 516)
								{
									Main.spriteBatch.Draw(Main.npcTexture[type], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									float scaleFactor16 = 0.5f + (Main.npc[i].GetAlpha(color9).ToVector3() - new Vector3(0.5f)).Length() * 0.5f;
									for (int num199 = 0; num199 < 4; num199++)
									{
										Main.spriteBatch.Draw(Main.npcTexture[type], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY) + Main.npc[i].velocity.RotatedBy((double)((float)num199 * 1.57079637f), default(Vector2)) * scaleFactor16, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(64, 64, 64, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 518)
								{
									Vector2 value36 = new Vector2(-10f, 0f);
									Main.spriteBatch.Draw(Main.glowMaskTexture[163], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha, 255 - Main.npc[i].alpha), Main.npc[i].rotation, vector10 + value36, Main.npc[i].scale, spriteEffects, 0f);
									float scaleFactor17 = 0.5f + (Main.npc[i].GetAlpha(color9).ToVector3() - new Vector3(0.5f)).Length() * 0.5f;
									for (int num200 = 0; num200 < 4; num200++)
									{
										Main.spriteBatch.Draw(Main.glowMaskTexture[163], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY) + Main.npc[i].velocity.RotatedBy((double)((float)num200 * 1.57079637f), default(Vector2)) * scaleFactor17, new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(64, 64, 64, 0), Main.npc[i].rotation, vector10 + value36, Main.npc[i].scale, spriteEffects, 0f);
									}
								}
								else if (type == 525)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[169], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(200, 200, 200, 100), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 526)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[170], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(200, 200, 200, 100), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 527)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[171], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(200, 200, 200, 100), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 533)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[172], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(255, 255, 255, 100), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 160)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[166], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								else if (type == 209)
								{
									Main.spriteBatch.Draw(Main.glowMaskTexture[167], Main.npc[i].Bottom - Main.screenPosition + new Vector2((float)(-(float)Main.npcTexture[type].Width) * Main.npc[i].scale / 2f + vector10.X * Main.npc[i].scale, (float)(-(float)Main.npcTexture[type].Height) * Main.npc[i].scale / (float)Main.npcFrameCount[type] + 4f + vector10.Y * Main.npc[i].scale + num67 + Main.npc[i].gfxOffY), new Microsoft.Xna.Framework.Rectangle?(Main.npc[i].frame), new Microsoft.Xna.Framework.Color(128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 128 - Main.npc[i].alpha / 2, 0), Main.npc[i].rotation, vector10, Main.npc[i].scale, spriteEffects, 0f);
								}
								if (Main.npc[i].aiStyle == 7)
								{
									NPC n3 = Main.npc[i];
									this.DrawNPCExtras(n3, false, num67, num66, color9, vector10, spriteEffects);
								}
							}
						}
					}
				}
			}
		}*/
	}
}

