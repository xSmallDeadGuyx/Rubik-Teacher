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

		public BasicEffect faceEffect;
		public BasicEffect lineEffect;

		public Stopwatch timer = new Stopwatch();
		private KeyboardState prevKb = new KeyboardState();
		private Point prevMs = new Point();

		public Color bgColor = Color.CornflowerBlue;

		public Color highlightColor = Color.SpringGreen;

		private Cube cube;
		public float angleX = (float) Math.PI / 6.0F;
		public float angleY = MathHelper.PiOver4;

		public Matrix viewMatrix;
		public Matrix projectionMatrix;

		public Texture2D faceTexture;

		public Random rand = new Random();

		public bool showNet = true;

		public FaceID rotatingFace = FaceID.Top;
		public Move lastMove;
		public float faceAngle = 0.0F;
		public float rotatePerStep = MathHelper.PiOver4 / 100.0F;
		public float rotatingTo = MathHelper.PiOver2;
		public bool rotatingBackwards = false;
		public bool paused = false;

		public bool animateFaces = true;
		public int performDelay = 300;
		public int delaySoFar = 0;

		public int zoom = 0;

		public bool[] verticesChanged = new bool[6];
		public List<VertexPositionColorTexture[]> faceVertices = new List<VertexPositionColorTexture[]>();

		public bool[,,] pieceHighlighted = new bool[3, 3, 3];

		public Queue<Move> moveQueue = new Queue<Move>();

		public MainForm form;

		protected override void Initialize() {
			Application.Idle += delegate { Invalidate(); };

			content = new ContentManager(Services, "Content");
			spriteBatch = new SpriteBatch(GraphicsDevice);

			faceTexture = new Texture2D(GraphicsDevice, 64, 64);
			Color[] data = new Color[64 * 64];
			for(int i = 0; i < 64; i++) for(int j = 0; j < 64; j++)
				if(i < 8 || j < 8 || i >= 56 || j >= 56) data[i + j * 64] = Color.Black;
				else data[i + j * 64] = Color.White;
			faceTexture.SetData<Color>(data);


			RasterizerState rs = new RasterizerState();
			rs.CullMode = CullMode.CullCounterClockwiseFace;
			rs.FillMode = FillMode.Solid;
			GraphicsDevice.RasterizerState = rs;
			

			faceEffect = new BasicEffect(GraphicsDevice);
			faceEffect.VertexColorEnabled = true;
			faceEffect.TextureEnabled = true;
			faceEffect.Texture = faceTexture;
			faceEffect.LightingEnabled = false;

			lineEffect = new BasicEffect(GraphicsDevice);
			lineEffect.VertexColorEnabled = true;
			lineEffect.TextureEnabled = false;
			lineEffect.LightingEnabled = false;

			cube = new Cube();

			viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, -7 + zoom), new Vector3(0, 0, 0), new Vector3(0, -1, 0));
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);

			refresh();

			timer.Start();
		}

		public void refresh() {
			faceVertices.Clear();
			for(int i = 0; i < 6; i++) {
				faceVertices.Add(generateFaceVertices((FaceID)i));
				verticesChanged[i] = false;
			}
		}

		protected void update() {
			float correction = 1.0F;
			if(timer.IsRunning) {
				timer.Stop();
				correction = (float) timer.ElapsedTicks / 3000F;
			}

			viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, -7 + zoom), new Vector3(0, 0, 0), new Vector3(0, -1, 0));
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);

			if(!paused) {
				if(moveQueue.Count > 0 && delaySoFar == 0 && faceAngle <= 0.0F) {
					Move move = moveQueue.Dequeue();
					delaySoFar = (int) (performDelay / correction);
					if(animateFaces) {
						verticesChanged[(int) move.face] = true;
						for(int i = 0; i < 6; i++)
							if(areAdjacent(move.face, (FaceID) i))
								verticesChanged[i] = true;

						rotatingBackwards = move.twist == CubeMove.AntiClockwise;
						rotatingFace = move.face;
						faceAngle = rotatePerStep;
						if(move.twist == CubeMove.Double) rotatingTo = MathHelper.Pi;
						else rotatingTo = MathHelper.PiOver2;
						lastMove = move;
					}
					else {
						verticesChanged[(int) move.face] = true;
						for(int i = 0; i < 6; i++)
							if(areAdjacent(move.face, (FaceID) i))
								verticesChanged[i] = true;
						cube.performMove(move);
						updateHighlights(move);
						lastMove = move;

						if(moveQueue.Count == 0)
							onSequenceFinish();
					}
				}
				if(delaySoFar > 0) delaySoFar--;
				if(faceAngle > 0.0F) {
					verticesChanged[(int) lastMove.face] = true;
					for(int i = 0; i < 6; i++)
						if(areAdjacent(lastMove.face, (FaceID) i))
							verticesChanged[i] = true;

					faceAngle += rotatePerStep * correction;
				}
				if(faceAngle >= rotatingTo) {
					verticesChanged[(int) lastMove.face] = true;
					for(int i = 0; i < 6; i++)
						if(areAdjacent(lastMove.face, (FaceID) i))
							verticesChanged[i] = true;

					faceAngle = 0.0F;
					cube.performMove(lastMove);
					updateHighlights(lastMove);

					if(moveQueue.Count == 0)
						onSequenceFinish();
				}
			}

			KeyboardState kb = Keyboard.GetState();
			MouseState ms = Mouse.GetState();

			Point vms = this.PointToClient(new Point(ms.X, ms.Y));

			if(ms.LeftButton == ButtonState.Pressed && inBounds(vms) && inBounds(prevMs)) {
				angleX += (vms.Y - prevMs.Y) * 0.01F / (float) Height * 365F;
				angleY += (prevMs.X - vms.X) * 0.01F / (float) Width * 805F;
			}
			if(angleX > MathHelper.PiOver2) angleX = MathHelper.PiOver2;
			if(angleX < -MathHelper.PiOver2) angleX = -MathHelper.PiOver2;

			prevKb = kb;
			prevMs = vms;

			if(!timer.IsRunning)
				timer.Restart();
		}

		public void onSequenceFinish() {
			form.tutorial.resetSequence();
		}

		public bool inBounds(Point p) {
			return p.X >= 0 && p.Y >= 0 && p.X < this.Width && p.Y < this.Height;
		}

		protected override void Draw() {
			try {
				update();
				GraphicsDevice.Clear(bgColor);

				if(showNet) {
					spriteBatch.Begin();
					for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++) {
							spriteBatch.Draw(faceTexture, new Rectangle(48 + i * 16, j * 16, 16, 16), cube.colourIDs[(int) cube.faceColours[(int) FaceID.Top, i, j]]);
							spriteBatch.Draw(faceTexture, new Rectangle(i * 16, 48 + j * 16, 16, 16), cube.colourIDs[(int) cube.faceColours[(int) FaceID.Left, i, j]]);
							spriteBatch.Draw(faceTexture, new Rectangle(48 + i * 16, 48 + j * 16, 16, 16), cube.colourIDs[(int) cube.faceColours[(int) FaceID.Front, i, j]]);
							spriteBatch.Draw(faceTexture, new Rectangle(96 + i * 16, 48 + j * 16, 16, 16), cube.colourIDs[(int) cube.faceColours[(int) FaceID.Right, i, j]]);
							spriteBatch.Draw(faceTexture, new Rectangle(48 + i * 16, 96 + j * 16, 16, 16), cube.colourIDs[(int) cube.faceColours[(int) FaceID.Bottom, i, j]]);
							spriteBatch.Draw(faceTexture, new Rectangle(48 + i * 16, 144 + j * 16, 16, 16), cube.colourIDs[(int) cube.faceColours[(int) FaceID.Back, 2 - i, 2 - j]]);
						}
					spriteBatch.End();
				}

				Matrix worldMatrix = Matrix.CreateRotationY(angleY) * Matrix.CreateRotationX(angleX);
				lineEffect.World = faceEffect.World = worldMatrix;
				lineEffect.View = faceEffect.View = viewMatrix;
				lineEffect.Projection = faceEffect.Projection = projectionMatrix;

				GraphicsDevice.BlendState = BlendState.Opaque;
				GraphicsDevice.DepthStencilState = DepthStencilState.Default;

				foreach(EffectPass p in faceEffect.CurrentTechnique.Passes)
					p.Apply();

				for(int i = 0; i < 6; i++) {
					if(verticesChanged[i]) {
						verticesChanged[i] = false;
						faceVertices[i] = generateFaceVertices((FaceID) i);
					}
					GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, faceVertices[i], 0, faceVertices[i].Length / 3);
				}

				foreach(EffectPass p in lineEffect.CurrentTechnique.Passes)
					p.Apply();

				for(int x = 0; x < 3; x++)
					for(int y = 0; y < 3; y++)
						for(int z = 0; z < 3; z++)
							if(pieceHighlighted[x, y, z]) {
								VertexPositionColor[] vertices = generateHighlightVertices(x, y, z, highlightColor);
								GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, vertices.Length / 2);
							}
			}
			catch(Exception e) { Console.WriteLine(e.Message + " - " + e.StackTrace); }
		}

		public VertexPositionColorTexture[] generateFaceVertices(FaceID face) {
			VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[108];
			switch(face) {
				case FaceID.Top:
					for(int x = 0; x < 3; x++) for(int z = 0; z < 3; z++) {
						Color c = cube.colourIDs[(int) cube.faceColours[(int) face, x, z]];
						vertices[x * 12 + z * 36 + 0] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, -1.5F, 1.5F - (float) z), c, new Vector2(0, 0));
						vertices[x * 12 + z * 36 + 1] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, -1.5F, 0.5F - (float) z), c, new Vector2(1, 1));
						vertices[x * 12 + z * 36 + 2] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, -1.5F, 1.5F - (float) z), c, new Vector2(1, 0));
						vertices[x * 12 + z * 36 + 3] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, -1.5F, 1.5F - (float) z), c, new Vector2(0, 0));
						vertices[x * 12 + z * 36 + 4] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, -1.5F, 0.5F - (float) z), c, new Vector2(0, 1));
						vertices[x * 12 + z * 36 + 5] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, -1.5F, 0.5F - (float) z), c, new Vector2(1, 1));

						vertices[x * 12 + z * 36 + 6] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, -1.5F, 1.5F - (float) z), Color.Black, Vector2.Zero);
						vertices[x * 12 + z * 36 + 7] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, -1.5F, 1.5F - (float) z), Color.Black, Vector2.Zero);
						vertices[x * 12 + z * 36 + 8] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, -1.5F, 0.5F - (float) z), Color.Black, Vector2.Zero);
						vertices[x * 12 + z * 36 + 9] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, -1.5F, 1.5F - (float) z), Color.Black, Vector2.Zero);
						vertices[x * 12 + z * 36 + 10] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, -1.5F, 0.5F - (float) z), Color.Black, Vector2.Zero);
						vertices[x * 12 + z * 36 + 11] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, -1.5F, 0.5F - (float) z), Color.Black, Vector2.Zero);
					}
					break;
				case FaceID.Bottom:
					for(int x = 0; x < 3; x++) for(int z = 0; z < 3; z++) {
						Color c = cube.colourIDs[(int) cube.faceColours[(int) face, x, z]];
						vertices[x * 12 + z * 36 + 0] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, 1.5F, (float) z - 0.5F), c, new Vector2(0, 0));
						vertices[x * 12 + z * 36 + 1] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, 1.5F, (float) z - 0.5F), c, new Vector2(1, 0));
						vertices[x * 12 + z * 36 + 2] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, 1.5F, (float) z - 1.5F), c, new Vector2(1, 1));
						vertices[x * 12 + z * 36 + 3] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, 1.5F, (float) z - 0.5F), c, new Vector2(0, 0));
						vertices[x * 12 + z * 36 + 4] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, 1.5F, (float) z - 1.5F), c, new Vector2(1, 1));
						vertices[x * 12 + z * 36 + 5] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, 1.5F, (float) z - 1.5F), c, new Vector2(0, 1));

						vertices[x * 12 + z * 36 + 6] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, 1.5F, (float) z - 0.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + z * 36 + 7] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, 1.5F, (float) z - 1.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + z * 36 + 8] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, 1.5F, (float) z - 0.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + z * 36 + 9] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, 1.5F, (float) z - 0.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + z * 36 + 10] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, 1.5F, (float) z - 1.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + z * 36 + 11] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, 1.5F, (float) z - 1.5F), Color.Black, Vector2.Zero);
					}
					break;
				case FaceID.Front:
					for(int x = 0; x < 3; x++) for(int y = 0; y < 3; y++) {
						Color c = cube.colourIDs[(int) cube.faceColours[(int) face, x, y]];
						vertices[x * 12 + y * 36 + 0] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, (float) y - 1.5F, -1.5F), c, new Vector2(0, 0));
						vertices[x * 12 + y * 36 + 1] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, (float) y - 0.5F, -1.5F), c, new Vector2(1, 1));
						vertices[x * 12 + y * 36 + 2] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, (float) y - 1.5F, -1.5F), c, new Vector2(1, 0));
						vertices[x * 12 + y * 36 + 3] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, (float) y - 1.5F, -1.5F), c, new Vector2(0, 0));
						vertices[x * 12 + y * 36 + 4] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, (float) y - 0.5F, -1.5F), c, new Vector2(0, 1));
						vertices[x * 12 + y * 36 + 5] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, (float) y - 0.5F, -1.5F), c, new Vector2(1, 1));

						vertices[x * 12 + y * 36 + 6] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, (float) y - 1.5F, -1.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + y * 36 + 7] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, (float) y - 1.5F, -1.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + y * 36 + 8] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, (float) y - 0.5F, -1.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + y * 36 + 9] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, (float) y - 1.5F, -1.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + y * 36 + 10] = new VertexPositionColorTexture(new Vector3((float) x - 1.5F, (float) y - 0.5F, -1.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + y * 36 + 11] = new VertexPositionColorTexture(new Vector3((float) x - 0.5F, (float) y - 0.5F, -1.5F), Color.Black, Vector2.Zero);
					}
					break;
				case FaceID.Back:
					for(int x = 0; x < 3; x++) for(int y = 0; y < 3; y++) {
						Color c = cube.colourIDs[(int) cube.faceColours[(int) face, x, y]];
						vertices[x * 12 + y * 36 + 0] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, (float) y - 1.5F, 1.5F), c, new Vector2(0, 0));
						vertices[x * 12 + y * 36 + 1] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, (float) y - 1.5F, 1.5F), c, new Vector2(1, 0));
						vertices[x * 12 + y * 36 + 2] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, (float) y - 0.5F, 1.5F), c, new Vector2(1, 1));
						vertices[x * 12 + y * 36 + 3] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, (float) y - 1.5F, 1.5F), c, new Vector2(0, 0));
						vertices[x * 12 + y * 36 + 4] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, (float) y - 0.5F, 1.5F), c, new Vector2(1, 1));
						vertices[x * 12 + y * 36 + 5] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, (float) y - 0.5F, 1.5F), c, new Vector2(0, 1));

						vertices[x * 12 + y * 36 + 6] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, (float) y - 1.5F, 1.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + y * 36 + 7] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, (float) y - 0.5F, 1.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + y * 36 + 8] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, (float) y - 1.5F, 1.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + y * 36 + 9] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, (float) y - 1.5F, 1.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + y * 36 + 10] = new VertexPositionColorTexture(new Vector3(1.5F - (float) x, (float) y - 0.5F, 1.5F), Color.Black, Vector2.Zero);
						vertices[x * 12 + y * 36 + 11] = new VertexPositionColorTexture(new Vector3(0.5F - (float) x, (float) y - 0.5F, 1.5F), Color.Black, Vector2.Zero);
					}
					break;
				case FaceID.Left:
					for(int y = 0; y < 3; y++) for(int z = 0; z < 3; z++) {
						Color c = cube.colourIDs[(int) cube.faceColours[(int) face, z, y]];
						vertices[z * 12 + y * 36 + 0] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 1.5F, 1.5F - (float) z), c, new Vector2(0, 0));
						vertices[z * 12 + y * 36 + 1] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 0.5F, 0.5F - (float) z), c, new Vector2(1, 1));
						vertices[z * 12 + y * 36 + 2] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 0.5F, 1.5F - (float) z), c, new Vector2(1, 0));
						vertices[z * 12 + y * 36 + 3] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 1.5F, 1.5F - (float) z), c, new Vector2(0, 0));
						vertices[z * 12 + y * 36 + 4] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 1.5F, 0.5F - (float) z), c, new Vector2(0, 1));
						vertices[z * 12 + y * 36 + 5] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 0.5F, 0.5F - (float) z), c, new Vector2(1, 1));

						vertices[z * 12 + y * 36 + 6] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 1.5F, 1.5F - (float) z), Color.Black, Vector2.Zero);
						vertices[z * 12 + y * 36 + 7] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 0.5F, 1.5F - (float) z), Color.Black, Vector2.Zero);
						vertices[z * 12 + y * 36 + 8] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 0.5F, 0.5F - (float) z), Color.Black, Vector2.Zero);
						vertices[z * 12 + y * 36 + 9] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 1.5F, 1.5F - (float) z), Color.Black, Vector2.Zero);
						vertices[z * 12 + y * 36 + 10] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 0.5F, 0.5F - (float) z), Color.Black, Vector2.Zero);
						vertices[z * 12 + y * 36 + 11] = new VertexPositionColorTexture(new Vector3(-1.5F, (float) y - 1.5F, 0.5F - (float) z), Color.Black, Vector2.Zero);
					}
					break;
				case FaceID.Right:
					for(int y = 0; y < 3; y++) for(int z = 0; z < 3; z++) {
						Color c = cube.colourIDs[(int) cube.faceColours[(int) face, z, y]];
						vertices[z * 12 + y * 36 + 0] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 1.5F, (float) z - 0.5F), c, new Vector2(0, 0));
						vertices[z * 12 + y * 36 + 1] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 0.5F, (float) z - 0.5F), c, new Vector2(1, 0));
						vertices[z * 12 + y * 36 + 2] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 0.5F, (float) z - 1.5F), c, new Vector2(1, 1));
						vertices[z * 12 + y * 36 + 3] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 1.5F, (float) z - 0.5F), c, new Vector2(0, 0));
						vertices[z * 12 + y * 36 + 4] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 0.5F, (float) z - 1.5F), c, new Vector2(1, 1));
						vertices[z * 12 + y * 36 + 5] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 1.5F, (float) z - 1.5F), c, new Vector2(0, 1));

						vertices[z * 12 + y * 36 + 6] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 1.5F, (float) z - 0.5F), Color.Black, Vector2.Zero);
						vertices[z * 12 + y * 36 + 7] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 0.5F, (float) z - 1.5F), Color.Black, Vector2.Zero);
						vertices[z * 12 + y * 36 + 8] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 0.5F, (float) z - 0.5F), Color.Black, Vector2.Zero);
						vertices[z * 12 + y * 36 + 9] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 1.5F, (float) z - 0.5F), Color.Black, Vector2.Zero);
						vertices[z * 12 + y * 36 + 10] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 1.5F, (float) z - 1.5F), Color.Black, Vector2.Zero);
						vertices[z * 12 + y * 36 + 11] = new VertexPositionColorTexture(new Vector3(1.5F, (float) y - 0.5F, (float) z - 1.5F), Color.Black, Vector2.Zero);
					}
					break;
			}

			if(faceAngle > 0.0F && face == rotatingFace) {
				for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++) for(int n = 0; n < 12; n++)
					vertices[i * 12 + j * 36 + n].Position = rotate(face, vertices[i * 12 + j * 36 + n].Position, faceAngle);
			}
			else if(faceAngle > 0.0F && areAdjacent(face, rotatingFace)) {
				for(int i = 0; i < 3; i++) foreach(FaceCoord coords in new FaceCoord[] { cube.transposeFromFront(rotatingFace, FaceID.Top, i, 2), cube.transposeFromFront(rotatingFace, FaceID.Left, 2, i), cube.transposeFromFront(rotatingFace, FaceID.Right, 0, i), cube.transposeFromFront(rotatingFace, FaceID.Bottom, i, 0) }) {
					if(coords.face == face)
						for(int n = 0; n < 12; n++)
							vertices[coords.i * 12 + coords.j * 36 + n].Position = rotate(rotatingFace, vertices[coords.i * 12 + coords.j * 36 + n].Position, faceAngle);
				}
			}
			return vertices;
		}

		public VertexPositionColor[] generateHighlightVertices(int x, int y, int z, Color c) {
			Vector3 offset = new Vector3(-1.6F + (float) x, -1.6F + (float) y, -1.6F + (float) z);
			VertexPositionColor[] vertices = {
				new VertexPositionColor(offset, c), new VertexPositionColor(offset + new Vector3(1.2F, 0F, 0F), c),
				new VertexPositionColor(offset, c), new VertexPositionColor(offset + new Vector3(0F, 1.2F, 0F), c),
				new VertexPositionColor(offset, c), new VertexPositionColor(offset + new Vector3(0F, 0F, 1.2F), c),
				new VertexPositionColor(offset + new Vector3(0F, 1.2F, 1.2F), c), new VertexPositionColor(offset + new Vector3(0F, 0F, 1.2F), c),
				new VertexPositionColor(offset + new Vector3(0F, 1.2F, 1.2F), c), new VertexPositionColor(offset + new Vector3(0F, 1.2F, 0F), c),
				new VertexPositionColor(offset + new Vector3(0F, 1.2F, 1.2F), c), new VertexPositionColor(offset + new Vector3(1.2F, 1.2F, 1.2F), c),
				new VertexPositionColor(offset + new Vector3(1.2F, 0F, 1.2F), c), new VertexPositionColor(offset + new Vector3(0F, 0F, 1.2F), c),
				new VertexPositionColor(offset + new Vector3(1.2F, 0F, 1.2F), c), new VertexPositionColor(offset + new Vector3(1.2F, 1.2F, 1.2F), c),
				new VertexPositionColor(offset + new Vector3(1.2F, 0F, 1.2F), c), new VertexPositionColor(offset + new Vector3(1.2F, 0F, 0F), c),
				new VertexPositionColor(offset + new Vector3(1.2F, 1.2F, 0F), c), new VertexPositionColor(offset + new Vector3(0F, 1.2F, 0F), c),
				new VertexPositionColor(offset + new Vector3(1.2F, 1.2F, 0F), c), new VertexPositionColor(offset + new Vector3(1.2F, 0F, 0F), c),
				new VertexPositionColor(offset + new Vector3(1.2F, 1.2F, 0F), c), new VertexPositionColor(offset + new Vector3(1.2F, 1.2F, 1.2F), c)
			};

			if(faceAngle > 0.0F)
				for(int i = 0; i < vertices.Length; i++) {
					if(x == 0 && rotatingFace == FaceID.Left)
						vertices[i].Position = rotate(FaceID.Left, vertices[i].Position, faceAngle);
					if(y == 0 && rotatingFace == FaceID.Top)
						vertices[i].Position = rotate(FaceID.Top, vertices[i].Position, faceAngle);
					if(z == 0 && rotatingFace == FaceID.Front)
						vertices[i].Position = rotate(FaceID.Front, vertices[i].Position, faceAngle);
					if(x == 2 && rotatingFace == FaceID.Right)
						vertices[i].Position = rotate(FaceID.Right, vertices[i].Position, faceAngle);
					if(y == 2 && rotatingFace == FaceID.Bottom)
						vertices[i].Position = rotate(FaceID.Bottom, vertices[i].Position, faceAngle);
					if(z == 2 && rotatingFace == FaceID.Back)
						vertices[i].Position = rotate(FaceID.Back, vertices[i].Position, faceAngle);
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

		public Vector3 rotate(FaceID face, Vector3 vector, float angle) {
			if(rotatingBackwards) angle = -angle;
			Vector3 rotated = new Vector3(vector.X, vector.Y, vector.Z);
			switch(face) {
				case FaceID.Top:
					rotated = Vector3.Transform(vector, Matrix.CreateRotationY(angle));
					break;
				case FaceID.Bottom:
					rotated = Vector3.Transform(vector, Matrix.CreateRotationY(-angle));
					break;
				case FaceID.Front:
					rotated = Vector3.Transform(vector, Matrix.CreateRotationZ(angle));
					break;
				case FaceID.Back:
					rotated = Vector3.Transform(vector, Matrix.CreateRotationZ(-angle));
					break;
				case FaceID.Left:
					rotated = Vector3.Transform(vector, Matrix.CreateRotationX(angle));
					break;
				case FaceID.Right:
					rotated = Vector3.Transform(vector, Matrix.CreateRotationX(-angle));
					break;
			}
			return rotated;
		}

		public void performMove(String s) {
			for(int i = 0; i < 6; i++) {
				char l = cube.faceLetters[i];
				if(s == l.ToString()) {
					performMove((FaceID) i, CubeMove.Clockwise);
					return;
				}
				if(s == l.ToString() + "'") {
					performMove((FaceID) i, CubeMove.AntiClockwise);
					return;
				}
				if(s == "2" + l.ToString()) {
					performMove((FaceID) i, CubeMove.Double);
					return;
				}
			}
		}

		public void updateHighlights(Move move) {
			bool[,,] oldHighlights = (bool[,,]) pieceHighlighted.Clone();
			for(int i = 0; i < 3; i++)
				for(int j = 0; j < 3; j++)
					switch(move.twist) {
						case CubeMove.Clockwise:
							switch(move.face) {
								case FaceID.Front:
									pieceHighlighted[2 - j, i, 0] = oldHighlights[i, j, 0];
									break;
								case FaceID.Back:
									pieceHighlighted[j, 2 - i, 2] = oldHighlights[i, j, 2];
									break;
								case FaceID.Left:
									pieceHighlighted[0, 2 - j, i] = oldHighlights[0, i, j];
									break;
								case FaceID.Right:
									pieceHighlighted[2, j, 2 - i] = oldHighlights[2, i, j];
									break;
								case FaceID.Top:
									pieceHighlighted[j, 0, 2 - i] = oldHighlights[i, 0, j];
									break;
								case FaceID.Bottom:
									pieceHighlighted[2 - j, 2, i] = oldHighlights[i, 2, j];
									break;
							}
							break;
						case CubeMove.Double:
							switch(move.face) {
								case FaceID.Front:
									pieceHighlighted[2 - i, 2 - j, 0] = oldHighlights[i, j, 0];
									break;
								case FaceID.Back:
									pieceHighlighted[2 - i, 2 - j, 2] = oldHighlights[i, j, 2];
									break;
								case FaceID.Left:
									pieceHighlighted[0, 2 - i, 2 - j] = oldHighlights[0, i, j];
									break;
								case FaceID.Right:
									pieceHighlighted[2, 2 - i, 2 - j] = oldHighlights[2, i, j];
									break;
								case FaceID.Top:
									pieceHighlighted[2 - i, 0, 2 - j] = oldHighlights[i, 0, j];
									break;
								case FaceID.Bottom:
									pieceHighlighted[2 - i, 2, 2 - j] = oldHighlights[i, 2, j];
									break;
							}
							break;
						case CubeMove.AntiClockwise:
							switch(move.face) {
								case FaceID.Front:
									pieceHighlighted[j, 2 - i, 0] = oldHighlights[i, j, 0];
									break;
								case FaceID.Back:
									pieceHighlighted[2 - j, i, 2] = oldHighlights[i, j, 2];
									break;
								case FaceID.Left:
									pieceHighlighted[0, j, 2 - i] = oldHighlights[0, i, j];
									break;
								case FaceID.Right:
									pieceHighlighted[2, 2 - j, i] = oldHighlights[2, i, j];
									break;
								case FaceID.Top:
									pieceHighlighted[2 - j, 0, i] = oldHighlights[i, 0, j];
									break;
								case FaceID.Bottom:
									pieceHighlighted[j, 2, 2 - i] = oldHighlights[i, 2, j];
									break;
							}
							break;
					}
		}

		public void performMove(FaceID face, CubeMove rot) {
			performMove(new Move(face, rot));
		}

		public void performMove(Move move) {
			moveQueue.Enqueue(move);
		}

		public void fromString(string str) {
			cube.fromString(str);
			refresh();
		}

		public bool isValidMove(string str) {
			return cube.isValidMove(str);
		}

		public string cubeToString() {
			return cube.ToString();
		}

		public bool areMovesHappening() {
			return faceAngle > 0.0F || moveQueue.Count > 0;
		}
	}
}
