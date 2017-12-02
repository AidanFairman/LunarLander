using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary;


namespace LunarLander2
{
    public class Terrain : DrawableGameComponent
    {
        PrimitiveBatch pb;
        private Vector2[] land;
        private Vector2[] landingPads;
        Color landColor;
        Color padColor;
        private string terrainName;
        private string padName;
        float xScale;
        float yScale;
        int horizontalTerrainParts = 25;
        int verticalTerrainParts = 10;


        public Terrain(Game game) : base(game)
        {
        }
        
        public string TerrainName{
            set
            {
                terrainName = value;
                padName = string.Format("{0}Pads", terrainName);
            }

        }

        public override void Initialize()
        {
            //graphics = new GraphicsDeviceManager(this);

            pb = new PrimitiveBatch(Game.GraphicsDevice);
            
            land = LanderTextToVector.GetPoints(String.Format("{0}\\{1}.txt", Game.Content.RootDirectory, terrainName)).ToArray();
            
            landingPads = LanderTextToVector.GetPoints(String.Format("{0}\\{1}.txt", Game.Content.RootDirectory, padName)).ToArray();

            landColor = Color.White;
            padColor = Color.ForestGreen;

            int height = Game.GraphicsDevice.Viewport.Height;
            int width = Game.GraphicsDevice.Viewport.Width;

            xScale = (int)((width / horizontalTerrainParts - 1));
            yScale = 0 - (int)((height * (2.0f / 3.0f)) / verticalTerrainParts);

            for(int i = 0; i < land.Length; ++i)
            {
                land[i].X *= (xScale);
                land[i].Y *= yScale;
                land[i].Y += (height);
            }
            for(int i = 0; i < landingPads.Length; ++i)
            {
                landingPads[i].X *= xScale;
                landingPads[i].Y *= yScale;
                landingPads[i].Y += height;
            }

            base.Initialize();
        }

        public List<Vector2> Land
        {
            get
            {
                return new List<Vector2>(land);
            }
        }

        public List<Vector2> LandingPads
        {
            get
            {
                return new List<Vector2>(landingPads);
            }
        }

        public override void Update(GameTime gameTime)
        {
            //We'll add code in here to "randomly" generate land
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            pb.Begin(PrimitiveType.LineList);
            foreach(Vector2 v2 in land)
            {
                pb.AddVertex(v2, landColor);
            }
            foreach (Vector2 v2 in landingPads)
            {
                pb.AddVertex(v2, padColor);
            }
            pb.End();
            base.Draw(gameTime);
        }
    }
}
