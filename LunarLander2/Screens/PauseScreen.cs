using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameLibrary;

namespace LunarLander2.Screens
{
    class PauseScreen : GameScreen
    {

        KeyboardState oldKeyState;
       

        public override void LoadContent()
        {
            VectorFont.Initialize(StateManager.game);
            oldKeyState = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime, StateManager screens, GamePadState gamePadState, MouseState mouseState, KeyboardState keyState, InputHandler input)
        {
            if (keyState != oldKeyState)
            {
                if (keyState.IsKeyDown(Keys.P))
                {
                    StateManager.Pop();
                }
                oldKeyState = keyState;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            StateManager.graphicsDevice.Clear(Color.Black);
            Vector2 center = new Vector2(StateManager.graphicsDevice.Viewport.Width / 4, StateManager.graphicsDevice.Viewport.Height / 2);
            VectorFont.DrawString("Paused. Press P to resume", 7, center, Color.CornflowerBlue);
        }
    }
}
