using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game {
    class SandboxAndersState : GameState {
        private Player minTank;
        
        public SandboxAndersState()
        {
            minTank = new Player("Player 1", ResourceManager.GetUnitType("basic_tank.xml"));
            minTank.SetController (Player.Controllers.Forward, 	Keys.W);
            minTank.SetController (Player.Controllers.Back, 	Keys.S);
            minTank.SetController (Player.Controllers.Left, 	Keys.A);
            minTank.SetController (Player.Controllers.Right, 	Keys.D);
        }
                
        protected override void Draw(RenderEngine renderEngine)
        {
            renderEngine.Render(minTank);
            ParticleEngine.Render(renderEngine);
            renderEngine.DrawString("Position: " + minTank.GetX () + ", " + minTank.GetY (), 200, 200, Color.White);
        }

        protected override void Update(TimeSpan delta, KeyboardState keyboard, MouseState mouse, RenderEngine renderEngine)
        {
            if (keyboard.IsKeyDown(Keys.Escape)) {
                NextGameState = null; //Go back to main menu
            }
            minTank.Update(delta, keyboard, mouse);
        }
        
    }
}