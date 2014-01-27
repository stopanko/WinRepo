using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;

namespace PhysExp
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        float halfWidth;
        float halfHeight;
        float width;
        float height;
        float rad;
        private World _world;
        
        private Body _circleBody;
        private Body borderBody;
        private Body _circleBody2;
        private Texture2D _circleSprite;
        private Texture2D _1;
        private Vector2 _1Center;
        private Vector2 _circleCenter;


        Vector2 Direction;
        SpriteFont Font;
        string Text = "";
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //graphics.PreferredBackBufferWidth = 800;
            //graphics.PreferredBackBufferHeight = 480;
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            width = ConvertUnits.ToSimUnits(800);
            height = ConvertUnits.ToSimUnits(480);
            //halfWidth = ConvertUnits.ToSimUnits(800);
            //halfHeight = ConvertUnits.ToSimUnits(480);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Font = Content.Load<SpriteFont>("Font1");

            _world = new World(new Vector2(1, 0));
             
            _circleSprite = Content.Load<Texture2D>("CircleSprite");//завантажуємо текстуру
            _1 = Content.Load<Texture2D>("1");
            _circleCenter = ConvertUnits.ToSimUnits(new Vector2(_circleSprite.Width / 2f, _circleSprite.Height / 2f));//знаходимо центр текстури
            _1Center = ConvertUnits.ToSimUnits(new Vector2(_1.Height / 2, _1.Width / 2));
            rad = ConvertUnits.ToSimUnits(_circleSprite.Height / 2f);
            _circleBody = BodyFactory.CreateCircle(_world, ConvertUnits.ToSimUnits(_circleSprite.Height / 2f), 1f, ConvertUnits.ToSimUnits (new Vector2(10, 10))); // (світ, радіус(визнач за текстурою), шось,початкова позиція тіла)
            _circleBody.BodyType = BodyType.Dynamic;
            _circleBody.Restitution = 0.8f;
            _circleBody.Friction = 0.5f;
            
            // TODO: use this.Content to load your game content here



            //_circleBody2 = BodyFactory.CreateCircle(_world, ConvertUnits.ToSimUnits(_circleSprite.Height / 2f), 1f, ConvertUnits.ToSimUnits(new Vector2(150, 120))); // (світ, радіус(визнач за текстурою), шось,початкова позиція тіла)
            //_circleBody2.BodyType = BodyType.Dynamic;
            //_circleBody2.Restitution = 0.8f;
            //_circleBody2.Friction = 0.5f;

            Vertices borders = new Vertices(4);
            borders.Add(new Vector2(0 , 0 ));
            borders.Add(new Vector2(width, 0 ));
            borders.Add(new Vector2(width, height));
            borders.Add(new Vector2(0 , height));

            
            borderBody = BodyFactory.CreateLoopShape(_world, borders);
            borderBody.CollisionCategories = Category.All;
            borderBody.CollidesWith = Category.All;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();



            TouchCollection touches = TouchPanel.GetState();
            foreach (TouchLocation loc in touches)
            {
                if (loc.State == TouchLocationState.Pressed)
                {
                    int speed = 48;
                    Vector2 ClickPos = new Vector2(loc.Position.X, loc.Position.Y);
                    Vector2 DiffDist = ClickPos - ConvertUnits.ToDisplayUnits(_circleBody.Position); // шукає різницю між позицією кліку та позицією шару для того, щоб побудувати вектор в напрямку кліку з початку коордтнат.
                    //double distance = Math.Sqrt(Math.Pow(_circleBody.Position.X - ClickPos.X, 2) + Math.Pow(_circleBody.Position.Y - ClickPos.Y, 2));
                    double Distance = Math.Sqrt(Math.Pow(DiffDist.X, 2) + Math.Pow(DiffDist.Y, 2));//  дистанція нового кліку
                    double match = Distance / speed; // взнаємо в скільки раз відстань між шаром та кліком менша від потрібної швидкості
                    //Direction = ClickPos - _circleBody.Position;
                    
                    //Direction.Normalize();
                    //int Xdirection = 1;
                    //int Ydirection = 1;
                    //_circleBody.ApplyLinearImpulse(ConvertUnits.ToSimUnits(Direction));
                    //_circleBody.ApplyLinearImpulse(new Vector2(20000, 0));
                    //_circleBody.ApplyForce(new Vector2(20000, 0));
                    //_circleBody.Position = ConvertUnits.ToSimUnits(loc.Position);
                    //_circleBody.ApplyTorque(10);
                    //_circleBody.ApplyLinearImpulse(new Vector2(10, -10f)); //прикладає імпульс(сумується з наступним)
                    //_circleBody.ApplyForce(new Vector2(10, -0f)); //прикладає силу(також сумується)
                    //if (ClickPos.X < ConvertUnits.ToDisplayUnits(_circleBody.Position.X))
                    //{
                    //    Xdirection = -1;
                    //}
                    //else
                    //{
                    //    Xdirection = 1;
                    //}
                    //if (ClickPos.Y < ConvertUnits.ToDisplayUnits(_circleBody.Position.Y))
                    //{
                    //    Ydirection = -1;
                    //}
                    //else
                    //{
                    //    Ydirection = 1;
                    //}
                    //_circleBody.LinearVelocity = ConvertUnits.ToSimUnits(new Vector2((float)((ClickPos.X - ConvertUnits.ToDisplayUnits(_circleBody.Position.X)) / match) * Xdirection, (float)((ClickPos.Y - ConvertUnits.ToDisplayUnits(_circleBody.Position.Y)) / match) * Ydirection));
                    // швидкість об'єкта
                    _circleBody.LinearVelocity = ConvertUnits.ToSimUnits(new Vector2((float)(DiffDist.X / match), (float)(DiffDist.Y / match)));
                    //_circleBody.IsBullet = false;//ConvertUnits.ToSimUnits(20);
                    //Text = ConvertUnits.ToDisplayUnits(Direction).ToString();
                    Text = ClickPos.ToString() + "\n" + Distance.ToString() + " " + match.ToString() + "\n" + ((float)(ClickPos.X / match)).ToString() + " " + ((float)(ClickPos.Y / match)).ToString(); //loc.Position.ToString() + "Vids: " + match.ToString() + "V raz: " + (match / 84).ToString() + "\n" + "Nklic:" + new Vector2((float)(loc.Position.X / (match / 84)), (float)(loc.Position.Y / (match / 84))).ToString();
                }
                    
            }
           
            // TODO: Add your update logic here
            //_world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            _world.Step(0.033333f);//оновлюємо світ
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(_circleSprite, ConvertUnits.ToDisplayUnits(_circleBody.Position), null, Color.White, ConvertUnits.ToDisplayUnits(_circleBody.Rotation), ConvertUnits.ToDisplayUnits(_circleCenter), 1f, SpriteEffects.None, 0f);
            //                спрайт            позиція тіла                             обертання тіла     центер текстури(зміщуємо верхній лівий кут в центр)

            //spriteBatch.Draw(_circleSprite, ConvertUnits.ToDisplayUnits(_circleBody2.Position), null, Color.White, ConvertUnits.ToDisplayUnits(_circleBody2.Rotation), ConvertUnits.ToDisplayUnits(_circleCenter), 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Font, Text, new Vector2(100, 0), Color.Black);
            spriteBatch.Draw(_1, ConvertUnits.ToDisplayUnits(_circleBody.Position - _1Center), Color.White);
            spriteBatch.Draw(_1, ConvertUnits.ToDisplayUnits(_circleBody.Position -  new Vector2(_1Center.X - rad, _1Center.Y)), Color.Black);
            spriteBatch.Draw(_1, ConvertUnits.ToDisplayUnits(_circleBody.Position - new Vector2(_1Center.X, _1Center.Y - rad)), Color.Black);
            spriteBatch.Draw(_1, ConvertUnits.ToDisplayUnits(_circleBody.Position - new Vector2(_1Center.X + rad, _1Center.Y)), Color.Black);
            spriteBatch.Draw(_1, ConvertUnits.ToDisplayUnits(_circleBody.Position - new Vector2(_1Center.X, _1Center.Y + rad)), Color.Black);
            //spriteBatch.DrawString(Font, borderBody.Position.ToString(), ConvertUnits.ToDisplayUnits(new Vector2(borderBody.Position.X, borderBody.Position.Y)), Color.Black);
            //spriteBatch.DrawString(Font, _circleBody.Position.ToString(), ConvertUnits.ToDisplayUnits( new Vector2(_circleBody.Position.X, _circleBody.Position.Y)), Color.Red);
            spriteBatch.Draw(_circleSprite, (new Vector2(-(_circleSprite.Width/2), -(_circleSprite.Width/2) )), Color.White);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
