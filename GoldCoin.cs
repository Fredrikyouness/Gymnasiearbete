using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Spaceshooter
{
    // ==========================================================
    // GoldCoin, mynt som ger poäng
    // ==========================================================
    class GoldCoin : PhysicalObject
    {
        // Hur länge guldmyntet lever kvar
        double timeToDie;

        // ==========================================================
        // GoldCoin(), konstruktor för att skapa objektet
        // ==========================================================
        public GoldCoin(Texture2D texture, float X, float Y, GameTime gameTime) :
            base(texture, X, Y, 0, 2f)
        {
            timeToDie = gameTime.TotalGameTime.TotalMilliseconds + 5000;
        }

        // ==========================================================
        // Update(), kontrollerar om guldmyntet ska få leva vidare
        // ==========================================================
        public void Update(GameTime gameTime)
        {
            // Ta bort guldmyntet om det är för gammalt
            if (timeToDie < gameTime.TotalGameTime.TotalMilliseconds)
                isAlive = false;
        }
    }
}
