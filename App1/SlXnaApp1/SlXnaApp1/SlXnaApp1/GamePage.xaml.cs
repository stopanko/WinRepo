using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SlXnaApp1
{
    public partial class GamePage : PhoneApplicationPage
    {
        public static int masItem;
        public static int maxUsers;// = RoomReqListener.Maxusers;
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        //bool first = true;
        //bool sec = false;
        public string NameF;

        public static Balls[] Balls_mas = new Balls[2];
        
        Texture2D Circle;
        
        SpriteFont Font;
        
        public static string SendTxt = "";

        


        



        public GamePage()
        {
            InitializeComponent();

            
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
            
            for (int i = 0; i < maxUsers; i++)
            {
                Balls b = new Balls();
                b.SpritePos = new Vector2(i * 150, i * 150);
                Balls_mas[i] = b;// new Balls() ;
            }
            // TODO: use this.content to load your game content here
            Circle = contentManager.Load<Texture2D>("ball");
            Font = contentManager.Load<SpriteFont>("Font1");
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
                    
                    Balls_mas[masItem].GetMoveDir(new Vector2(loc.Position.X - 150 / 2, loc.Position.Y - 150 / 2));
                    Balls_mas[masItem].SendDates();
                    
                }



            }


            Physics.Colision(Balls_mas);

            foreach (Balls b in Balls_mas)
            {
                b.MoveSprite(e);
                Physics.FringePhysics(b);
            }
            


        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (Balls b in Balls_mas)
            {
                spriteBatch.Draw(Circle, new Microsoft.Xna.Framework.Rectangle((int)(b.SpritePos.X), (int)(b.SpritePos.Y), 150, 150), Color.White);
            }
           
            spriteBatch.DrawString(Font, SendTxt, new Vector2(10, 10), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here
        }
    }
}