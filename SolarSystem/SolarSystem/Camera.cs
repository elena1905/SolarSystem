﻿using Microsoft.Xna.Framework;

namespace SolarSystem
{
    public class Camera : GameEntity
    {
        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }

        public Camera()
        {
        }

        public override void Update(GameTime gameTime)
        {
            var pos = new Vector3(0, 200, 200);
            var tar = new Vector3(0, 0, 0);
            var up = new Vector3(0, 1, 0);

            View = Matrix.CreateLookAt(pos, tar, up);
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, Game1.Instance.Graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f, 10000.0f);
        }

        public override void Draw(GameTime gameTime)
        {
        }
    }
}