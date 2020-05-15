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
    // Player, klass för att skapa ett spelarobjekt. Klassen ska
    // hantera spelarens rymdskepp (sprite) och ta emot 
    // tangenttryckningar för att ändra rymdskeppets position
    // ==========================================================
    class Player : PhysicalObject
    {
        int points = 0;
        List<Bullet> bullets; // Alla skott
        Texture2D bulletTexture; // skottets bild
        double timeSinceLastBullet = 0; // I millisekunder

        // ==========================================================
        // Player(), konstruktor för att skapa ett spelar-objekt
        // ==========================================================
        public Player(Texture2D texture, float X, float Y, float speedX,
            float speedY, Texture2D bulletTexture) 
            : base(texture, X, Y, speedX, speedY)
        {
            bullets = new List<Bullet>();
            this.bulletTexture = bulletTexture;
        }

        // ==========================================================
        // Update(), flyttar på spelaren
        // ==========================================================
        public void Update(GameWindow window, GameTime gameTime)
        {
            // Läs in tangenttryckningar
            KeyboardState keyboardState = Keyboard.GetState();

            // Flytta på rymdskeppet efter tangenttryckningar (om det inte är
            // på väg ut ur skärmen)
            if (vector.X <= window.ClientBounds.Width - texture.Width && vector.X >= 0)
            {
                if (keyboardState.IsKeyDown(Keys.Right))
                    vector.X += speed.X;
                if (keyboardState.IsKeyDown(Keys.Left))
                    vector.X -= speed.X;
            }

            if (vector.Y <= window.ClientBounds.Height - texture.Height && vector.Y >= 0)
            {
                if (keyboardState.IsKeyDown(Keys.Down))
                    vector.Y += speed.Y;
                if (keyboardState.IsKeyDown(Keys.Up))
                    vector.Y -= speed.Y;
            }

            // Kontrollera ifall rymdskeppet har åkt ut från kanten, 
            // om det har det, så återställ dess position
            // Vänster kant
            if (vector.X < 0)
                vector.X = 0;

            // Höger kant
            if (vector.X > window.ClientBounds.Width - texture.Width)
                vector.X = window.ClientBounds.Width - texture.Width;

            // Upptill
            if (vector.Y < 0)
                vector.Y = 0;

            // Nedtill
            if (vector.Y > window.ClientBounds.Height - texture.Height)
                vector.Y = window.ClientBounds.Height - texture.Height;

            if (keyboardState.IsKeyDown(Keys.Escape))
                isAlive = false;

            // Spelaren skjuter
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                // Kontrollera om spelaren får skjuta
                if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200)
                {
                    // Skapa skott
                    Bullet temp = new Bullet(bulletTexture, vector.X + texture.Width / 2, vector.Y);

                    // Lägg till skottet i listan
                    bullets.Add(temp);

                    // Sätt timeSinceLast<bullet till detta ögonblick
                    timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }

            // Flytta på alla skott
            foreach(Bullet b in bullets.ToList())
            {
                // Flytta på skottet
                b.Update();

                // Kontrollera så att skottet inte är "dött"
                if (!b.IsAlive)
                    // Om dött tas skottet bort
                    bullets.Remove(b);
            }
        }

        // ==========================================================
        // Draw(), ritar ut bilden på skärmen
        // ==========================================================
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, Color.White);
            foreach (Bullet b in bullets)
                b.Draw(spriteBatch); 
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        public List<Bullet> Bullets { get { return bullets; } }
    }

    // ==========================================================
    // Bullet, en klass för att skapa skott
    // ==========================================================
    class Bullet : PhysicalObject
    {
        // ==========================================================
        // Bullet(), konstruktor för att skapa ett skott-objekt
        // ==========================================================
        public Bullet(Texture2D texture, float X, float Y) : 
            base(texture, X, Y, 0f, 3f)
        {
        }

        // ==========================================================
        // Update(), uppdaterar skottets position och tar bort det
        // om det åker utanför fönstret
        // ==========================================================
        public void Update()
        {
            vector.Y -= speed.Y;
            if (vector.Y < 0)
                isAlive = false;
        }
    }
}
