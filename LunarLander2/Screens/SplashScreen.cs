using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace LunarLander2.Screens
{
    class SplashScreen : GameScreen
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        string studioName;
        Texture2D splashImage;

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(StateManager.graphicsDevice);
            spriteFont = StateManager.Content.Load<SpriteFont>("Font");
            splashImage = StateManager.Content.Load<Texture2D>("darkPlanet");
            studioName = "Dark Planet Games";
            StateManager.game.Window.Title = "Lunar Lander";
        }

        public override void Draw(GameTime gameTime)
        {
            int screenHeightDifference = (splashImage.Height - StateManager.graphicsDevice.Viewport.Height) / 2;
            int screenWidthDifference = (splashImage.Width - StateManager.graphicsDevice.Viewport.Height) / 2;
            Rectangle picture = new Rectangle(0 - screenWidthDifference, 0 - screenHeightDifference, splashImage.Width, splashImage.Height);
            spriteBatch.Begin();
            spriteBatch.Draw(splashImage, picture, Color.White);
            /*Vector2 center = new Vector2(StateManager.graphicsDevice.Viewport.Width / 2, StateManager.graphicsDevice.Viewport.Height / 2);
            Vector2 v = spriteFont.MeasureString(studioName) / new Vector2(2, 2);*/
            spriteBatch.DrawString(spriteFont, studioName,new Vector2(StateManager.graphicsDevice.Viewport.Width / 4, StateManager.graphicsDevice.Viewport.Height / 2), Color.White);
            
            spriteBatch.End();

        }

        

        public override void Update(GameTime gameTime, StateManager screens, GamePadState gamePadState, MouseState mouseState, KeyboardState keyState, InputHandler input)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Enter) || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                StateManager.Pop();
                StateManager.Push(new MainMenuScreen());
            }
        }
    }
}
