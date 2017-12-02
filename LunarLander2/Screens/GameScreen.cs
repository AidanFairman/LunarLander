using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LunarLander2.Screens
{
    public abstract class GameScreen
    {
        // Force all GameScreen derrived components to call LoadContent() automatically 
        // Why not make all screens drawable game components?  This is a good question. 
        // Component managers are typicall game components and the elements managed by a 
        // component should only be managed by the manager (ie. not updated() or drawn()
        // automatically
        public GameScreen()
        {
        }

        // Force all derived classes to implemnet these methods

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime, StateManager screens, GamePadState gamePadState, MouseState mouseState, KeyboardState keyState, InputHandler input);

        public abstract void Draw(GameTime gameTime);
    }
}