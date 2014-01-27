using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Linq;
using System.Collections.Generic;

using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
//AppWarp
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//
//farseer
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
//

namespace SlXnaApp1
{
    public partial class GamePage : PhoneApplicationPage
    {
        //settings винести в окремий клас
        public static int masItem;
        public static int maxUsers = 2;// = RoomReqListener.Maxusers;
        public static World _world; // світ з початковою гравітацією
        float width;
        float height;
        private Body borderBody;
        //

        //standart
        public static ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        //

        //bool first = true;
        //bool sec = false;
        public string NameF;

        public static Ball[] Balls_mas = new Ball[maxUsers];
        
        Texture2D Circle;
        
        SpriteFont Font;
        
        public static string SendTxt = " ";

        


        



        public GamePage()
        {
            InitializeComponent();
            // initial word dates винести в клас
            _world = new World(new Vector2(10, 0));
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            width = ConvertUnits.ToSimUnits(800);
            height = ConvertUnits.ToSimUnits(480);
            //
            
            WarpClient game = WarpClient.GetInstance();
            game.AddNotificationListener(new GameNotificationListener(this));

            

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);
            

            //винести в настройки світу
            Vertices borders = new Vertices(4);
            borders.Add(new Vector2(0, 0));
            borders.Add(new Vector2(width, 0));
            borders.Add(new Vector2(width, height));
            borders.Add(new Vector2(0, height));

            borderBody = BodyFactory.CreateLoopShape(_world, borders);
            borderBody.CollisionCategories = Category.All;
            borderBody.CollidesWith = Category.All;
            //

            for (int i = 0; i < maxUsers; i++)
            {
                Ball b = new Ball();
                b.InitBallContent();
                Balls_mas[i] = b;// new Balls() ;
            }
            // TODO: use this.content to load your game content here
            //Circle = contentManager.Load<Texture2D>("ball");
            //Font = contentManager.Load<SpriteFont>("Font1");
            // Start the timer
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {



            TouchCollection touches = TouchPanel.GetState();
            foreach (TouchLocation loc in touches)
            {
                if (loc.State == TouchLocationState.Pressed)
                {
                    
                    Balls_mas[masItem].GetMoveDir(new Vector2(loc.Position.X, loc.Position.Y));
                    Balls_mas[masItem].SendDates();
                    Balls_mas[masItem].MoveBall();
                    
                }



            }


            //Physics.Colision(Balls_mas);

            foreach (Ball b in Balls_mas)
            {
                //b.MoveBall(e);
                //Physics.FringePhysics(b);
            }
            


        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (Ball b in Balls_mas)
            {
                //spriteBatch.Draw(Circle, new Microsoft.Xna.Framework.Rectangle((int)(b.SpritePos.X), (int)(b.SpritePos.Y), 150, 150), Color.White);
                b.DrawBall(spriteBatch);
            }
           
            //spriteBatch.DrawString(Font, SendTxt, new Vector2(10, 10), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here
        }
    }
}