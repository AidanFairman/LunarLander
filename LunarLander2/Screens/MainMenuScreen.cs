using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary;

namespace LunarLander2.Screens
{
    class MainMenuScreen : GameScreen
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        KeyboardState oldKeyState;
        

        public override void LoadContent()
        {
            VectorFont.Initialize(StateManager.game);
            spriteBatch = new SpriteBatch(StateManager.graphicsDevice);
            spriteFont = StateManager.Content.Load<SpriteFont>("Font");
            oldKeyState = Keyboard.GetState();
            
        }

        public override void Update(GameTime gameTime, StateManager screens, GamePadState gamePadState, MouseState mouseState, KeyboardState keyState, InputHandler input)
        {
            StateManager.Score = 0;
            if(keyState != oldKeyState)
            {
                if (keyState.IsKeyDown(Keys.C))
                {
                    StateManager.Push(new CreditsScreen());
                }

                if (keyState.IsKeyDown(Keys.Enter))
                {
                    StateManager.Push(new PlayScreen());
                }

                if (keyState.IsKeyDown(Keys.Q))
                {
                    StateManager.game.Exit();
                }
                oldKeyState = keyState;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            StateManager.graphicsDevice.Clear(Color.Black);
            float Scale = 7;
            Vector2 location = new Vector2(3, 3);
            VectorFont.DrawString("Press Enter to Play", Scale, location, Color.CornflowerBlue);
            location.Y += (3 * Scale);
            VectorFont.DrawString("Press C to see credits", Scale, location, Color.CornflowerBlue);
            location.Y += (3 * Scale);
            VectorFont.DrawString("Press Q to quit", Scale, location, Color.CornflowerBlue);
        }
    }
}
