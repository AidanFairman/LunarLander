using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameLibrary;

namespace LunarLander2.Screens
{
    class CreditsScreen : GameScreen
    {
        KeyboardState oldKeyboardState;
        public override void LoadContent()
        {
            VectorFont.Initialize(StateManager.game);
            oldKeyboardState = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime, StateManager screens, GamePadState gamePadState, MouseState mouseState, KeyboardState keyState, InputHandler input)
        {
            if (oldKeyboardState != keyState)
            {
                if (keyState.IsKeyDown(Keys.Escape))
                {
                    StateManager.Pop();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            StateManager.graphicsDevice.Clear(Color.Black);
            float scale = 7f;
            Vector2 location = new Vector2(3, 3);
            VectorFont.DrawString("Programmed by", scale, location, Color.CornflowerBlue);
            location.Y += 3 * scale;
            VectorFont.DrawString("Aidan Fairman", scale, location, Color.Coral);
        }
    }
}
