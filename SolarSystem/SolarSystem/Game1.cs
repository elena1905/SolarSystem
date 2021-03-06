using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolarSystem
{
    public class Game1 : Game
    {
        public Setting Setting { get; set; }
        public Camera Camera { get; set; }
        public Sun Sun { get; set; }
        public Earth Earth { get; set; }
        public Moon Moon { get; set; }
        public List<GameEntity> Children { get; private set; }

        public GraphicsDeviceManager Graphics { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        public Model model;
        public Texture2D texture;

        public static Game1 Instance { get; private set; }

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 1366;
            Graphics.PreferredBackBufferHeight = 768;
            Graphics.PreferMultiSampling = true;
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            Instance = this;

            Setting = new Setting();
            Camera = new Camera();
            Children = new List<GameEntity>();
        }

        protected override void Initialize()
        {
            Sun = new Sun(0, 0, 0);
            Earth = new Earth();
            Moon = new Moon();

            Children.Add(Sun);
            Children.Add(Earth);
            Children.Add(Moon);
            Children.Add(new Monitor());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            model = Content.Load<Model>("Skybox");
            texture = Content.Load<Texture2D>("space");

            foreach (var child in Children)
            {
                child.LoadContent();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var dt = (float)gameTime.ElapsedGameTime.TotalDays*Setting.Speed;

            Setting.Update(dt);

            foreach (var child in Children)
            {
                child.Update(dt);
            }

            Camera.Update(dt);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();

            //=================== Using Skybox =====================//
            GraphicsDevice.DepthStencilState = DepthStencilState.None;

            Matrix[] skytransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(skytransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                // Set mesh orientation, and camera projection
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.TextureEnabled = true;
                    effect.Texture = texture;
                    effect.AmbientLightColor = new Vector3(1, 1, 1);
                    effect.World = skytransforms[mesh.ParentBone.Index] * Matrix.CreateScale(2000.0f) * Matrix.CreateTranslation(Camera.Position);
                    //effect.World = skytransforms[mesh.ParentBone.Index] * Matrix.CreateScale(2000.0f) * RotationMatrix * Matrix.CreateTranslation(Camera.Position);
                    effect.View = Camera.View;
                    effect.Projection = Camera.Projection;
                }
                
                mesh.Draw();
            }
            //===================== End of Using Skybox ==============//

            var state = new DepthStencilState {DepthBufferEnable = true};
            GraphicsDevice.DepthStencilState = state;

            var dt = (float)gameTime.ElapsedGameTime.TotalDays * Setting.Speed;

            foreach (var child in Children)
            {
                child.Draw(dt);
            }

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
