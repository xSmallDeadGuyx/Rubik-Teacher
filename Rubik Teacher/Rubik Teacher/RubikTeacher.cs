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

		public BasicEffect faceEffect; // 3d effects
		public BasicEffect lineEffect;

		public Stopwatch timer = new Stopwatch(); // stopwatch for timing steps and compensating
		private KeyboardState prevKb = new KeyboardState(); // previous states of input to determine changes
		private Point prevMs = new Point();

		public Color bgColor = Color.CornflowerBlue; // background colour to draw behind cube

		public Color highlightColor = Color.SpringGreen; // colours for cube highlights
		public Color targetHighlightColor = Color.BlueViolet;

		private Cube cube; // cube object
		public float angleX = (float) Math.PI / 6.0F; // angle of rotation of the cube
		public float angleY = MathHelper.PiOver4;

		public Matrix viewMatrix; // matrices representing the camera and viewport of the 3d space
		public Matrix projectionMatrix;

		public Texture2D faceTexture; // texture generated for the faces

		public Random rand = new Random(); // random number generator for shuffling and things

		public bool showNet = true; // setting for whether to draw a 2d net of the cube

		public FaceID rotatingFace = FaceID.Top; // face currently rotating
		public Move lastMove; // last move performed/currently performing move
		public float faceAngle = 0.0F; // current angle of face being turned
		public float rotatePerStep = MathHelper.PiOver4 / 100.0F; // rotation to be made each update tick
		public float rotatingTo = MathHelper.PiOver2; // maximum value the total rotation must reach (can change to Pi for double moves)
		public bool rotatingBackwards = false; // whether the piece is rotating anti-clockwise
		public bool paused = false; // whether the rotation is paused

		public bool animateFaces = true; // setting for whether to animate the face rotations
		public int performDelay = 300; // delay between rotations if performed too fast (namely non-animated)
		public int delaySoFar = 0; // delay so far between rotations

		public int zoom = 0; // zoom level

		public bool[] verticesChanged = new bool[6]; // identifier of vertices changed (used to optimize 3d drawing when stationary and on faces not being updated)
		public List<VertexPositionColorTexture[]> faceVertices = new List<VertexPositionColorTexture[]>(); // cache of vertices generated for cube faces

		public bool[,,] pieceHighlighted = new bool[3, 3, 3]; // identifiers of which pieces and positions are highlighted
		public bool[,,] targetHighlighted = new bool[3, 3, 3];

		public Queue<Move> moveQueue = new Queue<Move>(); // queue of moves to perform

		public MainForm form; // form object for communicating with other controls

		protected override void Initialize() {
			Application.Idle += delegate { Invalidate(); }; // make the application invalidate and therefore redraw/update the control in idle time

			content = new ContentManager(Services, "Content"); // set up a content manager and sprite batch
			spriteBatch = new SpriteBatch(GraphicsDevice);

			faceTexture = new Texture2D(GraphicsDevice, 64, 64); // generate a black texture with white inside square for the faces, where white is blended to the colour for that face
			Color[] data = new Color[64 * 64];
			for(int i = 0; i < 64; i++) for(int j = 0; j < 64; j++)
				if(i < 8 || j < 8 || i >= 56 || j >= 56) data[i + j * 64] = Color.Black;
				else data[i + j * 64] = Color.White;
			faceTexture.SetData<Color>(data);


			RasterizerState rs = new RasterizerState(); // create a new rasterizer
			rs.CullMode = CullMode.CullCounterClockwiseFace; // make sure it culls counter-clockwise (won't draw vertices set up clockwise with respect to the camera)
			rs.FillMode = FillMode.Solid; // fill the vertices draw
			GraphicsDevice.RasterizerState = rs; // update the used rasterizer to new one
			

			faceEffect = new BasicEffect(GraphicsDevice); // create a textured effect for drawing faces
			faceEffect.VertexColorEnabled = true;
			faceEffect.TextureEnabled = true;
			faceEffect.Texture = faceTexture;
			faceEffect.LightingEnabled = false;

			lineEffect = new BasicEffect(GraphicsDevice); // create a simple colour effect for drawing lines (highlights)
			lineEffect.VertexColorEnabled = true;
			lineEffect.TextureEnabled = false;
			lineEffect.LightingEnabled = false;

			cube = new Cube(); // create the cube object

			viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, -7 + zoom), new Vector3(0, 0, 0), new Vector3(0, -1, 0)); // set up the initial view matrices
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);

			refresh(); // update all the vertices initially

			timer.Start(); // start the step timer
		}

		public void refresh() { // clear the vertices cache and regenerate (useful for when entire cube changes at once)
			faceVertices.Clear();
			for(int i = 0; i < 6; i++) {
				faceVertices.Add(generateFaceVertices((FaceID)i));
				verticesChanged[i] = false;
			}
		}

		protected void update() { // called on every step in idle application time
			float correction = 1.0F; // initial time correction value
			if(timer.IsRunning) {
				timer.Stop();
				correction = (float) timer.ElapsedTicks / 3000F; // get the difference in time between this step and the last and create compensation for it
			}

			viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, -7 + zoom), new Vector3(0, 0, 0), new Vector3(0, -1, 0)); // update matrices for new zoom and new viewport if changed
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);

			if(!paused) { // if not paused update any rotations on the cube
				if(moveQueue.Count > 0 && delaySoFar == 0 && faceAngle <= 0.0F) { // no move currently being performed so start a new one
					Move move = moveQueue.Dequeue(); // get the next move
					delaySoFar = (int) (performDelay / correction); // set up the delay for this move
					if(animateFaces) { // if animate the faces then set up a slow face rotation
						verticesChanged[(int) move.face] = true; // invalidate the relevant faces for refreshing
						for(int i = 0; i < 6; i++)
							if(areAdjacent(move.face, (FaceID) i))
								verticesChanged[i] = true;

						rotatingBackwards = move.twist == CubeMove.AntiClockwise; // update the current rotation
						rotatingFace = move.face;
						faceAngle = rotatePerStep;
						if(move.twist == CubeMove.Double) rotatingTo = MathHelper.Pi;
						else rotatingTo = MathHelper.PiOver2;
						lastMove = move;
					}
					else { // faces not animated so perform rotation instantly
						verticesChanged[(int) move.face] = true;
						for(int i = 0; i < 6; i++)
							if(areAdjacent(move.face, (FaceID) i))
								verticesChanged[i] = true;
						cube.performMove(move);
						updateHighlights(move);
						lastMove = move;

						if(moveQueue.Count == 0) // update when sequence has finished
							onSequenceFinish();
					}
				}
				if(delaySoFar > 0) delaySoFar--; // update delay between rotations
				if(faceAngle > 0.0F) { // face currently being rotated so continue it
					verticesChanged[(int) lastMove.face] = true; // invalidate faces
					for(int i = 0; i < 6; i++)
						if(areAdjacent(lastMove.face, (FaceID) i))
							verticesChanged[i] = true;

					faceAngle += rotatePerStep * correction; // update rotation with correction for time differences
				}
				if(faceAngle >= rotatingTo) { // if rotation is finished, end it
					verticesChanged[(int) lastMove.face] = true; // invalidate faces
					for(int i = 0; i < 6; i++)
						if(areAdjacent(lastMove.face, (FaceID) i))
							verticesChanged[i] = true;

					faceAngle = 0.0F; // reset face angle, perform latest move on actual cube stored, and update changes in pieces highlighted
					cube.performMove(lastMove);
					updateHighlights(lastMove);

					if(moveQueue.Count == 0) // update when sequence has finished
						onSequenceFinish();
				}
			}

			KeyboardState kb = Keyboard.GetState(); // get current user input data
			MouseState ms = Mouse.GetState();

			Point vms = this.PointToClient(new Point(ms.X, ms.Y));

			if(ms.LeftButton == ButtonState.Pressed && inBounds(vms) && inBounds(prevMs)) { // if mouse is within control bounds and was in control bounds last step with the mouse button held
				angleX += (vms.Y - prevMs.Y) * 0.01F / (float) Height * 365F; // update rotations of entire cube
				angleY += (prevMs.X - vms.X) * 0.01F / (float) Width * 805F * (angleX > MathHelper.PiOver2 || angleX < -MathHelper.PiOver2 ? -1 : 1);
			}
			angleX = angleX % (float) (2 * Math.PI); // ensure angleX doesn't exceed -2*pi or 2*pi when not necessary (for above rotation logic)

			prevKb = kb; // input finished with so update input from last step
			prevMs = vms;

			if(!timer.IsRunning) // restart step timer
				timer.Restart();
		}

		public void onSequenceFinish() { // when sequence of moves is finished update the buttons in the tutorial
			form.tutorial.updateButtons();
			form.tutorial.nextSequence();
		}

		public bool inBounds(Point p) { // check if a point is within the bounds of the form control
			return p.X >= 0 && p.Y >= 0 && p.X < this.Width && p.Y < this.Height;
		}

		protected override void Draw() { // draw the form control
			try {
				update();
				GraphicsDevice.Clear(bgColor); // clear the background with the current colour

				if(showNet) { // if set to draw the net then do it
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

				Matrix worldMatrix = Matrix.CreateRotationY(angleY) * Matrix.CreateRotationX(angleX); // update matrices in effects with new angles
				lineEffect.World = faceEffect.World = worldMatrix;
				lineEffect.View = faceEffect.View = viewMatrix;
				lineEffect.Projection = faceEffect.Projection = projectionMatrix;

				GraphicsDevice.BlendState = BlendState.Opaque; // set blend states correctly
				GraphicsDevice.DepthStencilState = DepthStencilState.Default;

				foreach(EffectPass p in faceEffect.CurrentTechnique.Passes) // apply passes in effect for drawing faces
					p.Apply();

				for(int i = 0; i < 6; i++) { // for each face
					if(verticesChanged[i]) { // if vertices need refreshing then generate new ones
						verticesChanged[i] = false;
						faceVertices[i] = generateFaceVertices((FaceID) i);
					}
					GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, faceVertices[i], 0, faceVertices[i].Length / 3); // draw the vertices for that face
				}

				foreach(EffectPass p in lineEffect.CurrentTechnique.Passes) // apply passes in effect for drawing lines
					p.Apply();

				for(int x = 0; x < 3; x++)
					for(int y = 0; y < 3; y++)
						for(int z = 0; z < 3; z++) { // for each piece draw relevant highlights if applicable
							if(pieceHighlighted[x, y, z]) {
								VertexPositionColor[] vertices = generateHighlightVertices(x, y, z, highlightColor);
								GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, vertices.Length / 2);
							}
							if(targetHighlighted[x, y, z]) {
								VertexPositionColor[] vertices = generateTargetHighlightVertices(x, y, z, targetHighlightColor);
								GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, vertices.Length / 2);
							}
						}
			}
			catch(Exception e) { Console.WriteLine(e.Message + " - " + e.StackTrace); } // catch any errors and write to console (otherwise no information and just a red cross is drawn)
		}

		public VertexPositionColorTexture[] generateFaceVertices(FaceID face) { // generate the 3d vertices for face
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

			if(faceAngle > 0.0F && face == rotatingFace) { // if a face is being rotated and it is this face then rotate all the vertices by the rotation amount
				for(int i = 0; i < 3; i++) for(int j = 0; j < 3; j++) for(int n = 0; n < 12; n++)
					vertices[i * 12 + j * 36 + n].Position = rotate(face, vertices[i * 12 + j * 36 + n].Position, faceAngle);
			}
			else if(faceAngle > 0.0F && areAdjacent(face, rotatingFace)) { // otherwise if a face is being rotated and it's adjacent to this face then rotate the relevant vertices
				for(int i = 0; i < 3; i++) foreach(FaceCoord coords in new FaceCoord[] { cube.transposeFromFront(rotatingFace, FaceID.Top, i, 2), cube.transposeFromFront(rotatingFace, FaceID.Left, 2, i), cube.transposeFromFront(rotatingFace, FaceID.Right, 0, i), cube.transposeFromFront(rotatingFace, FaceID.Bottom, i, 0) }) {
					if(coords.face == face)
						for(int n = 0; n < 12; n++)
							vertices[coords.i * 12 + coords.j * 36 + n].Position = rotate(rotatingFace, vertices[coords.i * 12 + coords.j * 36 + n].Position, faceAngle);
				}
			}
			return vertices;
		}

		public VertexPositionColor[] generateHighlightVertices(int x, int y, int z, Color c) { // generate vertices for piece being highlighted
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

			if(faceAngle > 0.0F) // if a face is being rotated
				for(int i = 0; i < vertices.Length; i++) { // for each vertex
					if(x == 0 && rotatingFace == FaceID.Left) // for each face check if affected by a face rotation and rotate it if so
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

		public VertexPositionColor[] generateTargetHighlightVertices(int x, int y, int z, Color c) { // generate vertices for current highlighted target position
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

			return vertices;
		}

		public bool areAdjacent(FaceID face1, FaceID face2) { // return whether 2 faces are adjacent
			return !((face1 == FaceID.Top && face2 == FaceID.Bottom) ||
				(face1 == FaceID.Front && face2 == FaceID.Back) ||
				(face1 == FaceID.Right && face2 == FaceID.Left) ||
				(face1 == FaceID.Bottom && face2 == FaceID.Top) ||
				(face1 == FaceID.Back && face2 == FaceID.Front) ||
				(face1 == FaceID.Left && face2 == FaceID.Right));
		}

		public Vector3 rotate(FaceID face, Vector3 vector, float angle) { // perform rotation on a vector based on angle and the face
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

		public void performMove(String s) { // perform move stored in string
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

		public void updateHighlights(Move move) { // update any changes in rotated piece highlights
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

		public void performMove(FaceID face, CubeMove rot) { // performing move type methods
			performMove(new Move(face, rot));
		}

		public void performMove(Move move) { // queue up move
			moveQueue.Enqueue(move);
		}

		public void fromString(string str) { // update cube from string and refreshing
			cube.fromString(str);
			refresh();
		}

		public bool isValidMove(string str) { // check whether a move is valid
			return cube.isValidMove(str);
		}

		public string cubeToString() { // get the cube as a string
			return cube.ToString();
		}

		public void clearHighlights() { // reset any cube highlights
			for(int i = 0; i < 3; i++)
				for(int j = 0; j < 3; j++)
					for(int k = 0; k < 3; k++)
						targetHighlighted[i, j, k] = pieceHighlighted[i, j, k] = false;
		}
	}
}
