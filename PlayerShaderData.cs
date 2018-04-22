namespace ShaderLib
{
	public struct PlayerShaderData
	{
		public int bodySkinShader;
		public int legsSkinShader;
		public int shirtShader;
		public int underShirtShader;
		public int pantsShader;
		public int shoesShader;
		public int faceShader;
		public int hairShader;

		public PlayerShaderData(int body = 0, int legs = 0, int shirt = 0, int underShirt = 0, int pants = 0, int shoes = 0, int face = 0, int hair = 0) {
			bodySkinShader = body;
			legsSkinShader = legs;
			shirtShader = shirt;
			underShirtShader = underShirt;
			pantsShader = pants;
			shoesShader = shoes;
			faceShader = face;
			hairShader = hair;
		}
	}
}
