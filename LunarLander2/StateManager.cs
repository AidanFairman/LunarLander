using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LunarLander2
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class StateManager : DrawableGameComponent
    {
        // The stack which contains the screens
        public static int Score
        {
            get;
            set;
        }
        private static Stack<Screens.GameScreen> screens;
        
        // Hold commonly used objects in a central location that is available to all screens
        // in the State Manager
        private static ContentManager m_Content;
        private static GraphicsDevice m_graphicsDevice;
        private static InputHandler m_input;
        private static Game m_game;

        public static Game game
        {
            get { return m_game; }
            set { m_game = value; }
        }

        // All gamestates can access the Game Content Manager from the static context
        public static ContentManager Content
        {
            get { return m_Content; }
        }

        // All gamestates can access the Graphics Device from the static context
        public static GraphicsDevice graphicsDevice
        {
            get { return m_graphicsDevice; }
        }

        public StateManager(Game game)
            : base(game)
        {
            screens = new Stack<Screens.GameScreen>();
            m_Content = game.Content;
            m_graphicsDevice = game.GraphicsDevice;
            m_input = new InputHandler(game);
            m_game = game;
        }

        // insert a screen into the stack so that it becomes 
        // the screen that will be displayed
        public static void Push(Screens.GameScreen screen)
        {
            screens.Push(screen);
            Top().LoadContent();
        }

        // Remove the currently displayed screen
        //
        public static Screens.GameScreen Pop()
        {
            return screens.Pop();
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;
        }

        // Return the top screen in the stack
        public static Screens.GameScreen Top()
        {
            return screens.Peek();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();
            m_input.Update(gameTime);

            // Update the current gamestate
            Top().Update(gameTime, this, gamePadState, mouseState, keyState, m_input);
        }

        public override void Draw(GameTime gameTime)
        {
            //Draw the current gamestate
            Top().Draw(gameTime);
        }
    }
}