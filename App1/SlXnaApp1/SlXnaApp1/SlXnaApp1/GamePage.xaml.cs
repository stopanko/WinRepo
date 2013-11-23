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
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;


        Texture2D Circle;
        //Vector2 origin = new Vector2(0, 0);
        //Vector2 indir = new Vector2();
        //float speed1 = 5f;
        //Vector2 clicPos;
        //GameTime time = new GameTime();
        SpriteFont Font;
        ///// <summary>
        ///// 
        ///// </summary>
        //Vector2 sDirection = Vector2.Zero;
        //float mySpeed = 10;
        //Vector2 sPostion = new Vector2(0, 0);
        public string SendTxt = "";

        public Balls Bal = new Balls();


        public void Vect(UpdateEvent eventObj)
        {
            
                GameTimerEventArgs e = new GameTimerEventArgs();
                
                JObject jsonObj = JObject.Parse(System.Text.Encoding.UTF8.GetString(eventObj.getUpdate(), 0, eventObj.getUpdate().Length));
                //clicPos.X = int.Parse(jsonObj["X"].ToString());
                //clicPos.Y = int.Parse(jsonObj["Y"].ToString());
                
                //origin.X = float.Parse(jsonObj["originX"].ToString());
                //origin.Y = float.Parse(jsonObj["originY"].ToString());

              
                //Vector2 _Indir = new Vector2();
                //indir = clicPos - origin;
                //indir.Normalize();

                

                //indir.X = float.Parse(jsonObj["indirX"].ToString());
                //indir.Y = float.Parse(jsonObj["indirY"].ToString());
                
                //SendTxt = jsonObj["X"].ToString() +" | " + jsonObj["Y"].ToString();
                //SendTxt = jsonObj["X"].ToString() +" | " + jsonObj["Y"].ToString() + "// " + (stop-start);
                //SendTxt = "Sended";
 

        }

        //public Vector2 di()
        //{

        //    //indir = sDirection - origin;
        //    //indir.Normalize();
        //    //return (Vector2)indir * speed1;
        //}

        //public Vector2 dir(Vector2 pos, GameTimerEventArgs e)
        //{
            
        //        //return indir * speed1 * (float)e.ElapsedTime.Milliseconds/30;// *(float)e.ElapsedTime.TotalMilliseconds / 1000;
                
        //}



        public GamePage()
        {
            InitializeComponent();

            //new GameNotificationListener(this);
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
                    //clicPos.X = loc.Position.X - 150 / 2;
                    //clicPos.Y = loc.Position.Y - 150 / 2;
                    Bal.GetMoveDir(new Vector2(loc.Position.X - 150 / 2, loc.Position.Y - 150 / 2));
                    Bal.SendDates();
                }                             
                                  

                
            }


            Bal.MoveSprite(e);
            //origin += dir(clicPos, e)* (float)e.ElapsedTime.Milliseconds/30;
            //origin += dir(new Vector2(sDirection.X, sDirection.Y));
            

        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.Draw(Circle, new Microsoft.Xna.Framework.Rectangle((int)(Bal.SpritePos.X), (int)(Bal.SpritePos.Y), 150, 150), Color.White);
            spriteBatch.DrawString(Font, SendTxt, new Vector2(0, 0), Color.White);
            //spriteBatch.Draw(Circle, new Microsoft.Xna.Framework.Rectangle((int)(sPostion.X), (int)(sPostion.Y), 150, 150), Color.White);
            //spriteBatch.Draw(Circle, sPostion,Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here
        }
    }
}