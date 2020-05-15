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
    // Enemy, (bas)klass för fiender
    // ==========================================================
    abstract class Enemy : PhysicalObject
    {
        // ==========================================================
        // Enemy(), konstruktor för att skapa objektet
        // ==========================================================
        public Enemy(Texture2D texture, float X, float Y, float speedX, float speedY) :
            base(texture, X, Y, speedX, speedY)
        {
        }

        // ==========================================================
        // Update(), abstrakt metod som måste implementeras i alla
        // härledda fiender. Används för att uppdatera fiendernas 
        // position.
        // ==========================================================
        public abstract void Update(GameWindow window);
    }

    // ==========================================================
    // Mine, en elak mina som rör sig fram och tillbaka över skärmen
    // ==========================================================
    class Mine : Enemy
    {
        // ==========================================================
        // Mine(), konstruktor för att skapa objektet
        // ==========================================================
        public Mine(Texture2D texture, float X, float Y) : 
            base(texture, X, Y, 6f, 0.3f)
        {
        }

        // ==========================================================
        // Update(), uppdaterar fiendens position
        // ==========================================================
        public override void Update(GameWindow window)
        {
            // Flytta på fienden
            vector.X += speed.X;
            vector.Y += speed.Y;

            // Kontrollera så att den inte åker utanför fönstret på sidorna
            if (vector.X > window.ClientBounds.Width - texture.Width || vector.X < 0)
                speed.X *= -1;

            // Ta bort fienden om den åker ut där nere
            if (vector.Y > window.ClientBounds.Height)
                isAlive = false;
        }
    }

    // ==========================================================
    // Tripod, en elak fiende som åker i full fart neråt
    // ==========================================================
    class Tripod : Enemy
    {
        // ==========================================================
        // Tripod(), konstruktor för attskapa objektet
        // ==========================================================
        public Tripod(Texture2D texture, float X, float Y) : 
            base(texture, X, Y, 0f, 3f)
        {
        }

        // ==========================================================
        // Update(), uppdatera fiendens position
        // ==========================================================
        public override void Update(GameWindow window)
        {
            // Flytta på fienden
            vector.Y += speed.Y;

            // Gör fienden inaktiv om den åker ut där nere
            if (vector.Y > window.ClientBounds.Height)
                isAlive = false;
        }
    }
}
