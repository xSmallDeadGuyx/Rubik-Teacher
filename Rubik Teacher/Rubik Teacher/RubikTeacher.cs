using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RubikTeacher;
using System.Windows.Forms;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using System.Diagnostics;
using Point = System.Drawing.Point;

namespace Rubik_Teacher {
	public class RubikTeacher : GraphicsDeviceControl {
		public ContentManager content;
		public SpriteBatch spriteBatch;
		public BasicEffect effect;

		public Stopwatch timer;
		private KeyboardState prevKb = new KeyboardState();
		private Point prevMs = new Point();

		public Cube cube;
		public float angleX = 0.0F;
		public float angleY = 0.0F;

		public Matrix viewMatrix;
		public Matrix projectionMatrix;

		public Texture2D texture;

		public Random rand = new Random();

		public FaceID rotatingFace = FaceID.Top;
		public float faceAngle = 0.0F;

		public bool[] verticesChanged = new bool[6];
		public List<VertexPositionColorTexture[]> faceVertices = new List<VertexPositionColorTexture[]>();

		protected override void Initialize() {
			timer = Stopwatch.StartNew();
			Application.Idle += delegate { Invalidate(); };

			content = new ContentManager(Services, "Content");
			spriteBatch = new SpriteBatch(GraphicsDevice);

			texture = new Texture2D(GraphicsDevice, 64, 64);
			Color[] data = new Color[64 * 64];
			for(int i = 0; i < 64; i++) for(int j = 0; j < 64; j++)
				if(i < 8 || j < 8 || i >= 56 || j >= 56) data[i + j * 64] = Color.Black;
				else data[i + j * 64] = Color.White;
			texture.SetData<Color>(data);

			RasterizerState rs = new RasterizerState();
			rs.CullMode = CullMode.CullCounterClockwiseFace;
			rs.FillMode = FillMode.Solid;
			GraphicsDevice.RasterizerState = rs;

			effect = new BasicEffect(GraphicsDevice);
			effect.VertexColorEnabled = true;
			effect.TextureEnabled = true;
			effect.Texture = texture;
			effect.LightingEnabled = false;

			cube = new Cube();

			viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, -7), new Vector3(0, 0, 0), new Vector3(0, -1, 0));
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);

			for(int i = 0; i < 6; i++) {
				faceVertices.Add(generateFaceVertices((FaceID) i));
				verticesChanged[i] = false;
			}
		}

		protected void update() {
			KeyboardState kb = Keyboard.GetState();
			MouseState ms = Mouse.GetState();

			Point vms = this.PointToClient(new Point(ms.X, ms.Y));

			if(ms.LeftButton == ButtonState.Pressed) {
				angleX += (vms.Y - prevMs.Y) * 0.01F;
				angleY += (prevMs.X - vms.X) * 0.01F;
			}
			if(angleX > MathHelper.PiOver2) angleX = MathHelper.PiOver2;
			if(angleX < -MathHelper.PiOver2) angleX = -MathHelper.PiOver2;

			prevKb = kb;
			prevMs = vms;
		}

		public bool inBounds(Point p) {
			return p.X >= 0 && p.Y >= 0 && p.X < this.Width && p.Y < this.Height;
		}

		protected override void Draw() {
			try {
				update();
				GraphicsDevice.Clear(Color.CornflowerBlue);

				if(Program.form.showNetButton.Checked) {
					spriteBatch.Begin();
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++) {
							spriteBatch.Draw(texture, new Rectangle(48 + i * 16, j * 16, 16, 16), cube.colourIDs[(int) cube.faceColours[(int) FaceID.Top, i, j]]);
							spriteBatch.Draw(texture, new Rectangle(i * 16, 48 + j * 16, 16, 16), cube.colourIDs[(int) cube.faceColours[(int) FaceID.Left, i, j]]);
							spriteBatch.Draw(texture, new Rectangle(48 + i * 16, 48 + j * 16, 16, 16), cube.colourIDs[(int) cube.faceColours[(int) FaceID.Front, i, j]]);
							spriteBatch.Draw(texture, new Rectangle(96 + i * 16, 48 + j * 16, 16, 16), cube.colourIDs[(int) cube.faceColours[(int) FaceID.Right, i, j]]);
							spriteBatch.Draw(texture, new Rectangle(48 + i * 16, 96 + j * 16, 16, 16), cube.colourIDs[(int) cube.faceColours[(int) FaceID.Bottom, i, j]]);
							spriteBatch.Draw(texture, new Rectangle(48 + i * 16, 144 + j * 16, 16, 16), cube.colourIDs[(int) cube.faceColours[(int) FaceID.Back, 2 - i, 2 - j]]);
						}
					spriteBatch.End();
				}

				Matrix worldMatrix = Matrix.CreateRotationY(angleY) * Matrix.CreateRotationX(angleX);
				effect.World = worldMatrix;
				effect.View = viewMatrix;
				effect.Projection = projectionMatrix;

				foreach(EffectPass p in effect.CurrentTechnique.Passes)
					p.Apply();

				for(int i = 0; i < 6; i++) {
					if(verticesChanged[i]) {
						verticesChanged[i] = false;
						faceVertices[i] = generateFaceVertices((FaceID) i);
					}
					GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, faceVertices[i], 0, faceVertices[i].Length / 3, VertexPositionColorTexture.VertexDeclaration);
				}
			}
			catch(Exception e) { Console.WriteLine(e.Message + " - " + e.StackTrace); }
		}

		public VertexPositionColorTexture[] generateFaceVertices(FaceID face) {
			VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[54];
			switch(face) {
				case FaceID.Top:
					for(int x = 0; x < 3; x++) for(int z = 0; z < 3; z++) {
							Color c = cube.colourIDs[(int) cube.faceColours[(int) face, x, z]];
							vertices[x * 6 + z * 18 + 0] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, -1.5F, (float) z - 1.5F), c, new Vector2(0, 0));
							vertices[x * 6 + z * 18 + 1] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, -1.5F, (float) z - 0.5F), c, new Vector2(1, 1));
							vertices[x * 6 + z * 18 + 2] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, -1.5F, (float) z - 1.5F), c, new Vector2(1, 0));
							vertices[x * 6 + z * 18 + 3] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, -1.5F, (float) z - 1.5F), c, new Vector2(0, 0));
							vertices[x * 6 + z * 18 + 4] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, -1.5F, (float) z - 0.5F), c, new Vector2(0, 1));
							vertices[x * 6 + z * 18 + 5] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, -1.5F, (float) z - 0.5F), c, new Vector2(1, 1));
						}
					break;
				case FaceID.Bottom:
					for(int x = 0; x < 3; x++) for(int z = 0; z < 3; z++) {
							Color c = cube.colourIDs[(int) cube.faceColours[(int) face, x, z]];
							vertices[x * 6 + z * 18 + 0] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, 1.5F, 0.5F - (float) z), c, new Vector2(0, 0));
							vertices[x * 6 + z * 18 + 1] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, 1.5F, 0.5F - (float) z), c, new Vector2(1, 0));
							vertices[x * 6 + z * 18 + 2] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, 1.5F, 1.5F - (float) z), c, new Vector2(1, 1));
							vertices[x * 6 + z * 18 + 3] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, 1.5F, 0.5F - (float) z), c, new Vector2(0, 0));
							vertices[x * 6 + z * 18 + 4] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, 1.5F, 1.5F - (float) z), c, new Vector2(1, 1));
							vertices[x * 6 + z * 18 + 5] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, 1.5F, 1.5F - (float) z), c, new Vector2(0, 1));
						}
					break;
				case FaceID.Front:
					for(int x = 0; x < 3; x++) for(int y = 0; y < 3; y++) {
							Color c = cube.colourIDs[(int) cube.faceColours[(int) face, x, y]];
							vertices[x * 6 + y * 18 + 0] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, (float) y - 1.5F, 1.5F), c, new Vector2(0, 0));
							vertices[x * 6 + y * 18 + 1] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, (float) y - 0.5F, 1.5F), c, new Vector2(1, 1));
							vertices[x * 6 + y * 18 + 2] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, (float) y - 1.5F, 1.5F), c, new Vector2(1, 0));
							vertices[x * 6 + y * 18 + 3] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, (float) y - 1.5F, 1.5F), c, new Vector2(0, 0));
							vertices[x * 6 + y * 18 + 4] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, (float) y - 0.5F, 1.5F), c, new Vector2(0, 1));
							vertices[x * 6 + y * 18 + 5] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, (float) y - 0.5F, 1.5F), c, new Vector2(1, 1));
						}
					break;
				case FaceID.Back:
					for(int x = 0; x < 3; x++) for(int y = 0; y < 3; y++) {
							Color c = cube.colourIDs[(int) cube.faceColours[(int) face, x, y]];
							vertices[x * 6 + y * 18 + 0] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, (float) y - 1.5F, -1.5F), c, new Vector2(0, 0));
							vertices[x * 6 + y * 18 + 1] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, (float) y - 1.5F, -1.5F), c, new Vector2(1, 0));
							vertices[x * 6 + y * 18 + 2] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, (float) y - 0.5F, -1.5F), c, new Vector2(1, 1));
							vertices[x * 6 + y * 18 + 3] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, (float) y - 1.5F, -1.5F), c, new Vector2(0, 0));
							vertices[x * 6 + y * 18 + 4] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, (float) y - 0.5F, -1.5F), c, new Vector2(1, 1));
							vertices[x * 6 + y * 18 + 5] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, (float) y - 0.5F, -1.5F), c, new Vector2(0, 1));
						}
					break;
				case FaceID.Left:
					for(int y = 0; y < 3; y++) for(int z = 0; z < 3; z++) {
							Color c = cube.colourIDs[(int) cube.faceColours[(int) face, z, y]];
							vertices[y * 6 + z * 18 + 0] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 1.5F, (float) z - 1.5F), c, new Vector2(0, 0));
							vertices[y * 6 + z * 18 + 1] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 0.5F, (float) z - 0.5F), c, new Vector2(1, 1));
							vertices[y * 6 + z * 18 + 2] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 0.5F, (float) z - 1.5F), c, new Vector2(1, 0));
							vertices[y * 6 + z * 18 + 3] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 1.5F, (float) z - 1.5F), c, new Vector2(0, 0));
							vertices[y * 6 + z * 18 + 4] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 1.5F, (float) z - 0.5F), c, new Vector2(0, 1));
							vertices[y * 6 + z * 18 + 5] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 0.5F, (float) z - 0.5F), c, new Vector2(1, 1));
						}
					break;
				case FaceID.Right:
					for(int y = 0; y < 3; y++) for(int z = 0; z < 3; z++) {
							Color c = cube.colourIDs[(int) cube.faceColours[(int) face, z, y]];
							vertices[y * 6 + z * 18 + 0] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 1.5F, 0.5F - (float) z), c, new Vector2(0, 0));
							vertices[y * 6 + z * 18 + 1] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 0.5F, 0.5F - (float) z), c, new Vector2(1, 0));
							vertices[y * 6 + z * 18 + 2] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 0.5F, 1.5F - (float) z), c, new Vector2(1, 1));
							vertices[y * 6 + z * 18 + 3] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 1.5F, 0.5F - (float) z), c, new Vector2(0, 0));
							vertices[y * 6 + z * 18 + 4] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 0.5F, 1.5F - (float) z), c, new Vector2(1, 1));
							vertices[y * 6 + z * 18 + 5] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 1.5F, 1.5F - (float) z), c, new Vector2(0, 1));
						}
					break;
			}
			return vertices;
		}

		public bool areAdjacent(FaceID face1, FaceID face2) {
			return !((face1 == FaceID.Top && face2 == FaceID.Bottom) ||
				(face1 == FaceID.Front && face2 == FaceID.Back) ||
				(face1 == FaceID.Right && face2 == FaceID.Left) ||
				(face1 == FaceID.Bottom && face2 == FaceID.Top) ||
				(face1 == FaceID.Back && face2 == FaceID.Front) ||
				(face1 == FaceID.Left && face2 == FaceID.Right));
		}
	}
}
