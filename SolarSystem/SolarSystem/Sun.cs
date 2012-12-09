﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarSystem
{
    public class Sun : GameEntity
    {
        public const float Radius = 15f;

        public Sun(float x, float y, float z)
        {
            var pos = new Vector3(x, y, z);

            Scale = Matrix.CreateScale(new Vector3(Radius, Radius, Radius));
            LocalTransform = Scale;

            ModelName = "sphere";

            DiffuseColor = Color.Yellow.ToVector3();
        }

        public override void Draw(float dt)
        {
            if (Model != null)
            {
                foreach (var mesh in Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.DiffuseColor = DiffuseColor;
                        effect.World = LocalTransform * Matrix.CreateTranslation(Position);
                        effect.Projection = Game.Camera.Projection;
                        effect.View = Game.Camera.View;
                    }

                    mesh.Draw();
                }
            }
        }
    }
}
