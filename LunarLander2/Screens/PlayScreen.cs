using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GameLibrary;
using System;


namespace LunarLander2.Screens
{
    class PlayScreen : GameScreen
    {
        private Lander lander;
        private Terrain terrain;
        private static int terrainNumber;
        private string terrainFileName;
        private KeyboardState oldKeyboardState;
        private float fontScale = 6f;
        private SoundEffect win;
        private SoundEffect lose;
        private SoundEffectInstance winLoseInstance = null;
        private Color bgColor;
        private bool sentScore;
        
        public PlayScreen()
        {
            terrainNumber = 1;
            terrainFileName = "Terrain1";
            
        }

        public PlayScreen(int levelNumber)
        {
            terrainNumber = levelNumber;
            terrainFileName = string.Format("Terrain{0}", terrainNumber);
        }

        public override void LoadContent()
        {
            lander = new Lander(StateManager.game);
            lander.Initialize();
            VectorFont.Initialize(StateManager.game);
            terrain = new Terrain(StateManager.game);
            terrain.TerrainName = terrainFileName;
            terrain.Initialize();
            sentScore = false;
            win = StateManager.Content.Load<SoundEffect>("LanderVictorySound");
            lose = StateManager.Content.Load<SoundEffect>("LanderExplosionSound");
            bgColor = Color.Black;
            oldKeyboardState = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime, StateManager screens, GamePadState gamePadState, MouseState mouseState, KeyboardState keyState, InputHandler input)
        {
            if (oldKeyboardState != keyState)
            {
                if (keyState.IsKeyDown(Keys.P))
                {
                    StateManager.Push(new PauseScreen());
                }

                if (keyState.IsKeyDown(Keys.Escape))
                {
                    StateManager.Pop();
                }

                if(keyState.IsKeyDown(Keys.Enter) && lander.State == Lander.landerState.landed)
                {
                    StateManager.Pop();
                    terrainNumber += 1;
                    StateManager.Push(new PlayScreen((terrainNumber % 3)));
                }

                oldKeyboardState = keyState;
                
            }


            lander.Update(gameTime);
            terrain.Update(gameTime);

            if (testCollision(lander.Points.ToArray(), terrain.Land.ToArray()))
            {
                lander.Crash();
                bgColor = Color.DarkRed;
            }
            else if (testCollision(lander.Points.ToArray(), terrain.LandingPads.ToArray()))
            {
                if (lander.XSpeed < 0.3f && lander.YSpeed < 0.6f && lander.Angle == 0)
                {
                    if (!sentScore)
                    {
                        StateManager.Score = StateManager.Score + (int)(lander.Fuel * 100);
                        sentScore = true;
                    }
                    lander.winGame();
                    bgColor = Color.DarkGreen;
                }
                else
                {
                    lander.Crash();
                    bgColor = Color.DarkRed;
                }
            }

            handleSound();
        }

        private void handleSound()
        {
            if (winLoseInstance == null)
            {
                if (lander.State == Lander.landerState.crashed)
                {
                    winLoseInstance = lose.CreateInstance();
                   
                }
                else if (lander.State == Lander.landerState.landed)
                {
                    winLoseInstance = win.CreateInstance();
                }
                else
                {
                    winLoseInstance = null;
                }
                if (winLoseInstance != null)
                {
                    winLoseInstance.IsLooped = false;
                    winLoseInstance.Play();
                }
            }
        }

        private bool testCollision(Vector2[] obj1, Vector2[] obj2)
        {
            bool collided = false;
            Line2D line1;
            Line2D line2;
            for (int i = 0; i < obj1.Length && !collided; i += 2)
            {
                line1 = new Line2D(obj1[i], obj1[i + 1]);
                for (int k = 0; k < obj2.Length && !collided; k += 2)
                {
                    line2 = new Line2D(obj2[k], obj2[k + 1]);

                    if (Line2D.Intersects(line1, line2))
                    {
                        collided = true;
                    }
                }
            }
            return collided;
            
        }

        public override void Draw(GameTime gameTime)
        {
            
            
            StateManager.graphicsDevice.Clear(bgColor);
            lander.Draw(gameTime);
            terrain.Draw(gameTime);
            if (lander.State == Lander.landerState.playing)
            {
                
                float newFontScale = fontScale * 2.0f / 3.0f;
                Vector2 fontLocation = new Vector2(3, 3);
                VectorFont.DrawString(string.Format("Fuel {0:000.0}", lander.Fuel), fontScale, fontLocation, Color.CornflowerBlue);
                fontLocation.Y += (3 * fontScale);
                VectorFont.DrawString(string.Format("XSpeed {0:00.0}", lander.XSpeed), fontScale, fontLocation, Color.Coral);
                fontLocation.Y += (3 * fontScale);
                VectorFont.DrawString(string.Format("YSpeed {0:00.0}", lander.YSpeed), fontScale, fontLocation, Color.Coral);
                fontLocation.Y += (3 * fontScale);
                VectorFont.DrawString(string.Format("Angle {0:000}", lander.Angle), fontScale, fontLocation, Color.Coral);
                fontLocation.X = (StateManager.graphicsDevice.Viewport.Width / 10) * 7;
                fontLocation.Y = 3;
                VectorFont.DrawString("Esc to quit", newFontScale, fontLocation, Color.Coral);
                fontLocation.Y += 3 * newFontScale;
                VectorFont.DrawString("Space to thrust", newFontScale, fontLocation, Color.Coral);
                fontLocation.Y += 3 * newFontScale;
                VectorFont.DrawString("Right to rotate CW", newFontScale, fontLocation, Color.Coral);
                fontLocation.Y += 3 * newFontScale;
                VectorFont.DrawString("Left to rotate CCW", newFontScale, fontLocation, Color.Coral);

            }
            else if(lander.State == Lander.landerState.crashed)
            {
                Vector2 fontPlace = new Vector2(StateManager.graphicsDevice.Viewport.Width / 5 * 2, 4 * fontScale);
                VectorFont.DrawString("You Crashed", fontScale, fontPlace, Color.White);
                fontPlace.Y += 3 * fontScale;
                fontPlace.X = StateManager.graphicsDevice.Viewport.Width / 5;
                VectorFont.DrawString("You get no more points and are dead", fontScale, fontPlace, Color.White);
                fontPlace.Y += 3 * fontScale;
                VectorFont.DrawString(string.Format("You had {0:000000} points", StateManager.Score), fontScale, fontPlace, Color.White);
                fontPlace.Y += 3 * fontScale;
                VectorFont.DrawString("Press ESC to go back to menu", fontScale, fontPlace, Color.White);
            }
            else
            {
                Vector2 fontPlace = new Vector2(StateManager.graphicsDevice.Viewport.Width / 5 * 2, 4 * fontScale);
                VectorFont.DrawString("You Landed", fontScale, fontPlace, Color.Black);
                fontPlace.Y += 3 * fontScale;
                fontPlace.X = StateManager.graphicsDevice.Viewport.Width / 5;
                VectorFont.DrawString(string.Format("You get {0:0000} points",(lander.Fuel*100)), fontScale, fontPlace, Color.Black);
                fontPlace.Y += 3 * fontScale;
                VectorFont.DrawString(string.Format("You have {0:0000} points", StateManager.Score), fontScale, fontPlace, Color.Black);
                fontPlace.Y += 3 * fontScale;
                fontPlace.X = StateManager.graphicsDevice.Viewport.Width / 10;
                VectorFont.DrawString("Press Enter to go to next landing site", fontScale, fontPlace, Color.Black);
                fontPlace.Y += 3 * fontScale;
                fontPlace.X = StateManager.graphicsDevice.Viewport.Width / 5;
                VectorFont.DrawString("Press ESC to go back to menu", fontScale, fontPlace, Color.Black);
            }
        }
    }
}
