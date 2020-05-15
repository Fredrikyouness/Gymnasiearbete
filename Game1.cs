using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Spaceshooter
{
    // ==========================================================
    // Game, hela spelet
    // ==========================================================
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D menuSprite;
        Vector2 menuPos;
        Player player;
        List<Enemy> enemies;
        List<GoldCoin> goldCoins;
        Texture2D goldCoinSprite;
        PrintText printText;

        List<Double> times = new List<Double>();
        double value;

        Stopwatch stopwatch = new Stopwatch();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            goldCoins = new List<GoldCoin>();
            base.Initialize();
        }

        // ==========================================================
        // LoadContent(), laddar in data
        // ==========================================================
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new Player(Content.Load<Texture2D>("images/player/ship"), 380, 400, 2.5f, 4.5f,
                Content.Load<Texture2D>("images/player/bullet"));

            // Skapa fiender
            enemies = new List<Enemy>();
            Random random = new Random();
            Texture2D tmpSprite = Content.Load<Texture2D>("images/enemies/mine");
            for (int i = 0; i < 10; i++)
            {
                int rndX = random.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, Window.ClientBounds.Height / 2);
                Mine temp = new Mine(tmpSprite, rndX, rndY);

                // Lägg till i listan
                enemies.Add(temp);
            }

            tmpSprite = Content.Load<Texture2D>("images/enemies/tripod");
            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, Window.ClientBounds.Height / 2);
                Tripod temp = new Tripod(tmpSprite, rndX, rndY);

                // Lägg till i listan
                enemies.Add(temp);
            }

            printText = new PrintText(Content.Load<SpriteFont>("Fonts/Arial"));
            goldCoinSprite = Content.Load<Texture2D>("images/powerups/coin");
        }

        protected override void UnloadContent()
        {
        }

        // ==========================================================
        // Update(), uppdaterar hela spelet
        // ==========================================================
        protected override void Update(GameTime gameTime)
        {
            //stopwatch.Start();
            stopwatch.Start();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            player.Update(Window, gameTime);

            // Gå igenom alla fiender
            foreach (Enemy e in enemies.ToList())
            {
                if (e.IsAlive)
                {
                    // Gå igenom alla bullets
                    foreach (Bullet b in player.Bullets)
                    {
                        // Kontrollera om fiende kolliderat med bullet
                        if (e.CheckCollision(b))
                        {
                            // Vid kollision tas fiende bort och spelaren får poäng
                            e.IsAlive = false;
                            player.Points++;
                        }
                    }

                    // Kontrollera om fiende kolliderat med spelare
                    if (e.CheckCollision(player))
                        // Vid kollision dör spelaren
                        player.IsAlive = false;

                    // Uppdatera fiende
                    e.Update(Window);
                }

                // Om fiende inte lever tas den bort
                else
                    enemies.Remove(e);
            }

            // Guldmynten ska uppstå slumpmässigt, en chans på 200
            Random random = new Random();
            int newCoin = random.Next(1, 200);
            // Nytt guldmynt uppstår
            if (newCoin == 1)
            {
                // Var guldmyntet ska uppstå
                int rndX = random.Next(0, Window.ClientBounds.Width - goldCoinSprite.Width);
                int rndY = random.Next(0, Window.ClientBounds.Height - goldCoinSprite.Height);

                // Lägg till myntet i listan
                goldCoins.Add(new GoldCoin(goldCoinSprite, rndX, rndY, gameTime));
            }

            // Gå igenom listan med existerande mynt
            foreach (GoldCoin gc in goldCoins.ToList())
            {
                // Kontrollera om guldmyntet lever
                if (gc.IsAlive)
                {
                    // gc.Update() kollar om guldmyntet har blivit för gammalt
                    // för att få leva vidare
                    gc.Update(gameTime);

                    // Kontrollera om det kolliderat med spelaren
                    if (gc.CheckCollision(player))
                    {
                        // Vid kollision tas myntet bort och spelaren får poäng
                        goldCoins.Remove(gc);
                        player.Points++;
                    }
                }

                else
                    goldCoins.Remove(gc);
            }

            if (!player.IsAlive)
            {
                this.Exit();
            }

            base.Update(gameTime);
        }

        // ==========================================================
        // Draw(), ritar ut allt på skärmen
        // ==========================================================
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            player.Draw(spriteBatch);

            foreach (Enemy e in enemies)
                e.Draw(spriteBatch);

            foreach (GoldCoin gc in goldCoins)
                gc.Draw(spriteBatch);

            printText.Print("Points:" + player.Points, spriteBatch, 0, 0);

            spriteBatch.End();
            base.Draw(gameTime);
            stopwatch.Stop();
            time(gameTime);

        }

        // ==========================================================
        // time(), lagrar den mätta tiden
        // ==========================================================
        protected void time(GameTime gameTime)
        {
            if (times.Count < 60)
                times.Add(stopwatch.ElapsedMilliseconds);

            if (times.Count == 60)
            {
                value = 0;

                foreach (Double time in times)
                {
                    value += time;
                }

                value /= times.Count;

                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\Herman\Desktop\Hermans Gymnasiearbete kod\Mätningar.txt", true))
                {
                    file.WriteLine("{0}", value);
                }

                times.Clear();
            }
            if (gameTime.TotalGameTime.TotalMilliseconds == 5000)
                this.Exit();
        }
    }
}
