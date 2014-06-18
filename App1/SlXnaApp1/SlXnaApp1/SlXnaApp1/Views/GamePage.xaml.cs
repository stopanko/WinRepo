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
        public static int maxUsers;// = RoomReqListener.Maxusers;
        //public static World _world; // світ з початковою гравітацією
        //float width;
        //float height;
        //private Body borderBody;
        int timeCount = 1;
        bool Draw = false;
        //
        SpriteFont font;
        GameTime t = new GameTime();
        public static float time = 0;
        public static List<JObject> DatesList = new List<JObject>();
        //настройки світу
        public static WorldSet Ws = new WorldSet();
        //
        Texture2D Loos;
        Texture2D Win;
        //standart
        public static ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        //
        float rountTime = 35;

        bool clickToExit = false;

        public static Ball[] Balls_mas;// = new Ball[maxUsers];

        string Lefttime = "";
        string Roundtime = "";
        public static string Txt = " ";

        


        



        public GamePage()
        {
            InitializeComponent();
            this.SupportedOrientations = SupportedPageOrientation.Landscape;

            Balls_mas = new Ball[maxUsers];// ініціал масиву шарів
            Ws.InitWorld();
            // initial word dates винести в клас
            //_world = new World(new Vector2(0, 0));
            //ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            //width = ConvertUnits.ToSimUnits(800);
            //height = ConvertUnits.ToSimUnits(480);
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
            Loos = contentManager.Load<Texture2D>("YouLose");
            Win = contentManager.Load<Texture2D>("passed");
            Ws.InitBorders();
            //винести в настройки світу
            //Vertices borders = new Vertices(4);
            //borders.Add(new Vector2(0, 0));
            //borders.Add(new Vector2(width, 0));
            //borders.Add(new Vector2(width, height));
            //borders.Add(new Vector2(0, height));
            font = contentManager.Load<SpriteFont>("Font1");
            //borderBody = BodyFactory.CreateLoopShape(_world, borders);
            //borderBody.CollisionCategories = Category.All;
            //borderBody.CollidesWith = Category.All;
            //

            for (int i = 0; i < maxUsers; i++)
            {
                
                Balls_mas[i] = new Ball();
                Balls_mas[i].InitBallContent(i, Ws._world);
                //Balls_mas[i]._circleBody.BodyType = BodyType.Dynamic;
            }
            Balls_mas[0].isTarget = true;
            // TODO: use this.content to load your game content here
            
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
                    if (clickToExit == true)
                    {
                        NavigationService.Navigate(new Uri("/Views/RoomPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        Balls_mas[masItem].GetMoveDir(new Vector2(loc.Position.X, loc.Position.Y));
                        Balls_mas[masItem].MoveBall(); ////////перевірити в якому порядку методи краще викликати
                        Balls_mas[masItem].SendDates();//Only click send 
                    }
                }
            }


            //Os.ForEach(
            //    delegate(Type o)
            //    {
            //        if (!o.cond) Os.Remove(o);
            //    }
            //    );


            DatesList.ForEach(
                delegate(JObject Jo)
                {
                    if ((string)Jo["Type"] == "SendDates")
                    {

                        GamePage.Balls_mas[(int)Jo["Item"]].getPosSynchronization(Jo);// жорстко прирівнюємо позиції втор копії при кліку
                        
                        GamePage.Balls_mas[(int)Jo["Item"]].GetDates(Jo);// приймаємо дані на вторинних копіях
                        
                    }

                    else if ((string)Jo["Type"] == "SendTime" && (int)Jo["Sender"] == GamePage.masItem)
                    {
                        GamePage.Balls_mas[(int)Jo["Sender"]].GetTime(Jo);// отримуємо час від вторичної копії і обч Delt і зразу відсил в той самий телефон відповідь
                        GamePage.Balls_mas[(int)Jo["Sender"]].SendAnswer((int)Jo["Item"]);
                    }

                    if ((string)Jo["Type"] == "SendDelt" && (int)Jo["Recipient"] == GamePage.masItem)//попали на телефон з якого відсилали дані
                    {
                        GamePage.Balls_mas[(int)Jo["Sender"]].GetDelt(Jo);//обч пінг
                        GamePage.Balls_mas[(int)Jo["Sender"]].getSynchronization(Jo);//синхронізує вторинні шари з первинними шарами 
                    }
                    //}
                    GamePage.DatesList.Remove(Jo);
                    
                }
                );

            //foreach (JObject Jo in GamePage.DatesList)
            //{
            //    GamePage.Balls_mas[(int)Jo["Item"]].GetDates(Jo);
            //    GamePage.DatesList.Remove(Jo);
            //}
            
               
            time += float.Parse(e.ElapsedTime.TotalMilliseconds.ToString());

            if (time / (1000 * timeCount) >= 0.2)
            {
                //Draw = true;
                //Balls_mas[masItem].SendDates();//Send dates до вторинної копії з сервера на другому тедефоні   
                //time = 0;
                for (int i = 0; i < maxUsers; i++)
                {
                    if (i != masItem)
                    {
                        Balls_mas[i].SendTime(i);
                    }
                }
                timeCount++;               
 
            }

            //відсилаємо час для коректування (від вторинних копій до первинних)


            if ((time / 1000) > 5)
            {                
                Lefttime = "";
                Roundtime = Math.Round((rountTime - (time / 1000)), 0).ToString();
                for (int i = 0; i < maxUsers; i++)
                {
                    Balls_mas[i]._circleBody.BodyType = BodyType.Dynamic;
                }

            }
            else
            {
                Lefttime = Math.Round((time / 1000), 0).ToString();
                Roundtime = "";
            }

            if ((time / 1000) > rountTime)
            {
                for (int i = 0; i < maxUsers; i++)
                {
                    Balls_mas[i]._circleBody.BodyType = BodyType.Static;
                }
               
                //NavigationService.Navigate(new Uri("/Views/RoomPage.xaml", UriKind.Relative));
            }
                
            

                Ws._world.Step(0.033333f);
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
                
                b.DrawBall(spriteBatch);
                
            }
            //spriteBatch.DrawString(font, "Item " + masItem.ToString() + ":  " + time.ToString() + "  " + timeCount.ToString(), new Vector2(0,0), Color.Red);
            spriteBatch.DrawString(font, Lefttime, new Vector2(400, 240), Color.White);
            spriteBatch.DrawString(font, Roundtime, new Vector2(400, 240), Color.White);
            if ((time / 1000) > rountTime)
            {
                clickToExit = true;
                if (Balls_mas[masItem].isTarget == true)
                {
                    spriteBatch.Draw(Loos, new Vector2(400 - Loos.Width / 2, 240 - Loos.Height / 2), Color.White);
                }
                else
                {
                    spriteBatch.Draw(Win, new Vector2(400 - Win.Width / 2, 240 - Win.Height / 2), Color.White);
                }
            }

            //if (Draw == true)
            //{
            for (int i = 0; i < maxUsers; i++)
            {
                if (i != masItem)
                {
                    //spriteBatch.DrawString(font, ("ForItem " + i.ToString() + "Q=  " + Balls_mas[i].Q / 1000 + "   ping= " + Balls_mas[i].time2.ToString()).ToString(), new Vector2(0, (i+1) * 10), Color.Black);
                }
 
            }
                //spriteBatch.DrawString(font, (Balls_mas[masItem].Q / 1000).ToString(), new Vector2(100, 100), Color.Black);
            //    Draw = false;
 
            //}
            spriteBatch.End();
            // TODO: Add your drawing code here
        }
    }
}