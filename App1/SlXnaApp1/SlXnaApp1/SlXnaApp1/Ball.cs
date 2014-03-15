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

using System.Windows.Ink;
//using System.Windows.Media; // якщо додати то тре буде писати Microsoft.Xna.Framework.Color
using System.Windows.Media.Animation;

using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;


//using Microsoft.Xna.Framework.Graphics.PackedVector;
//AppWarp
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client;

//
//farseer
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
//

namespace SlXnaApp1
{
    public class Ball
    {
        //public World _world = new World(new Vector2(0, 0));
        //ball settings
        private Body _circleBody;
        private Texture2D _circleSprite;
        private Vector2 _circleCenter; // центр текстури для методу Draw
        private float _speed = 48;
        private Vector2 _clickPos = new Vector2(0, 0);
        private Vector2 _direction = new Vector2();
        //
        
        //public Vector2 SpritePos = new Vector2(0, 0);
        private JObject _jObj = new JObject();
        //private float _time;
        //private JObject _jsonPos = new JObject();
        //public JObject _getObj = new JObject();
        
        //float speed = 5f;

        //string[] Mas = new string[2];

        //public static Vector2[] cPos;
        //public static Vector2[] sPos;
        //public static int masItem; 
        
        //Timer senhron
        public float Q = 0;
        public float Delt = 0;
        public float time1, time2; 
        //

        //List<JObject> ballsList = new List<JObject>();

        //List<Vector2> Ballslist = new List<Vector2>();

        public void InitBallContent(int i, World _world)//передаємо і щоб шари не накладалися
        {
            _circleSprite = GamePage.contentManager.Load<Texture2D>("CircleSprite"); //текстура
            _circleCenter = ConvertUnits.ToSimUnits(new Vector2(_circleSprite.Width / 2f, _circleSprite.Height / 2f));//знаходимо центр текстури
            _circleBody = BodyFactory.CreateCircle(_world, ConvertUnits.ToSimUnits(_circleSprite.Height / 2f), 1f, ConvertUnits.ToSimUnits(new Vector2((i + 1) * 10, 10))); // (світ, радіус(визнач за текстурою), шось,початкова позиція тіла)
            _circleBody.BodyType = BodyType.Dynamic;
            _circleBody.Restitution = 0.8f;
            _circleBody.Friction = 0.5f;
            GetMoveDir(_circleBody.Position); //////////!!!!!!!!!!!!!!!!!!!!!!!!
            SendDates(); // initial Jobj
        }

        public void GetMoveDir(Vector2 ClickPos)//передамо вектор кліку
        {
            //Vector2 ClickPos = new Vector2(loc.X, loc.Y);
            _clickPos = ClickPos;
            Vector2 DiffDist = _clickPos - ConvertUnits.ToDisplayUnits(_circleBody.Position); // шукає різницю між позицією кліку та позицією шару для того, щоб побудувати вектор в напрямку кліку з початку коордтнат.
            //double distance = Math.Sqrt(Math.Pow(_circleBody.Position.X - ClickPos.X, 2) + Math.Pow(_circleBody.Position.Y - ClickPos.Y, 2));
            double Distance = Math.Sqrt(Math.Pow(DiffDist.X, 2) + Math.Pow(DiffDist.Y, 2));//  дистанція нового кліку
            double match = Distance / _speed; // взнаємо в скільки раз відстань між шаром та кліком менша від потрібної швидкості
            _direction = ConvertUnits.ToSimUnits(new Vector2((float)(DiffDist.X / match), (float)(DiffDist.Y / match)));
                   
        }


        public void MoveBall()
        {

            //SpritePos += _direction * (float)e.ElapsedTime.Milliseconds / 30 * speed * (float)e.ElapsedTime.Milliseconds / 30;
            _circleBody.LinearVelocity = _direction;// нажаємо прискорення тілу
        }

        public void SendDates()//відсилає дані від первинної копії іншим(відсилає ввід юзера)
        {
            
            _jObj.RemoveAll();
            _jObj.Add("X", this._clickPos.X);
            _jObj.Add("Y", this._clickPos.Y);
            _jObj.Add("PosX", this._circleBody.Position.X.ToString());
            _jObj.Add("PosY", this._circleBody.Position.Y.ToString());            
            _jObj.Add("Item", GamePage.masItem);//який телефон прислав дані
            _jObj.Add("Sender", GamePage.masItem);//про яку копію дані
            //_jsonClick.Add("Type", 1);//click
            _jObj.Add("Type", "SendDates");

            _jObj.Add("Time", GamePage.time);
            //_jObj.Add("Delt", this.Delt);
            //_jObj.Add("Type", Type);
            //_jObj.Add("Delt", this.Delt);
            //_jObj.Add("Q", this.Q);//відсилаємо різницю часу між копіями на різних телефонах
            //_jDates.Add("St0", this.St0);
            //_jDates.Add("St1", this.St1);
            //_jDates.Add("Ct1", this.Ct1);
            //_jDates.Add("Ct2", this.Ct2);        

            
            WarpClient.GetInstance().SendUpdatePeers(System.Text.Encoding.UTF8.GetBytes(_jObj.ToString()));//sendObj.ToString()));
            //_jsonObj.RemoveAll();
            
        }


        public void SendTime(int sender)// відсилає дані від вторинної копіїі до первинної щоб взнати час затримки для кожної вторинної від первинної
        {
            time1 = GamePage.time;
            _jObj.RemoveAll();
            _jObj.Add("Type", "SendTime");
            _jObj.Add("Time", GamePage.time);
            _jObj.Add("Sender", sender);// вторинна копія яка відсилає час
            _jObj.Add("Item", GamePage.masItem);//який телефон відсилає дані
            WarpClient.GetInstance().SendUpdatePeers(System.Text.Encoding.UTF8.GetBytes(_jObj.ToString()));
        }

        public void SendAnswer(int Recipient)// дані відсилаємо назад тому клієнту від якого отримали
        {
            //_jObj.Add("Time", GamePage.time);
            _jObj.RemoveAll();
            //_jObj.Add("Type", "Ct");
            _jObj.Add("Delt", this.Delt);
            _jObj.Add("Item", GamePage.masItem);
            _jObj.Add("Sender", GamePage.masItem);           
            _jObj.Add("Recipient", Recipient);
            _jObj.Add("Type", "SendDelt");
            
            WarpClient.GetInstance().SendUpdatePeers(System.Text.Encoding.UTF8.GetBytes(_jObj.ToString()));//sendObj.ToString()));

        }
        

        public void SendPos()
        {
            //_jsonPos.RemoveAll();
            ////_jsonPos.Add("X", this._clickPos.X);
            ////_jsonPos.Add("Y", this._clickPos.Y);
            //_jsonPos.Add("Type", 2);//Pos
            //_jsonPos.Add("Item", GamePage.masItem);
            //_jsonPos.Add("PosX", this._circleBody.Position.X.ToString());
            //_jsonPos.Add("PosY", this._circleBody.Position.Y.ToString());
            //WarpClient.GetInstance().SendUpdatePeers(System.Text.Encoding.UTF8.GetBytes(_jsonPos.ToString()));
        }

        public void GetDelt(JObject jsonObj)//отримуємо різницю явсу і обч Q
        {
            time2 = (GamePage.time - time1) / 2;
            Q = ((float)jsonObj["Delt"] + GamePage.time) / 2;

        }

        public void GetTime(JObject jsonObj)//отримуємо час(на первинній копії) і обчислюємо Delt
        {

            Delt = (float)jsonObj["Time"] - 2 * GamePage.time;
        }

        public void GetDates(JObject jsonObj)
        {

            //_jsonObj = jsonObj;
            if ((int)jsonObj["Item"] != GamePage.masItem )//провіряємо щоб не обробляти своїж дані
            {
                //провірили що отримуємо дані від первинної копії(St, Click) на іншому клієнті

                //обробка вхід даних
                _clickPos.X = float.Parse(jsonObj["X"].ToString());
                _clickPos.Y = float.Parse(jsonObj["Y"].ToString());
                this.GetMoveDir(_clickPos);
                this.MoveBall();


                // спочатку обробляємо таймер
                //if ((int)jsonObj["Item"] == 0) // ми отримали дані від першого(головного) клієнта
                //{

                //    //St0 = (float)jsonObj["Time"];
                //    //Ct2 = GamePage.time;
                //    //SendDates();
                //}

                //if (GamePage.masItem == 0)
                //{
                //    St1 = GamePage.time;
                //    Ct1 = (float)jsonObj["Time"];

                //    Q = ((St1 - Ct1) - (Ct2 - St0)) / 2;
                //}

                
                    
                
                //else if ((string)jsonObj["Type"] == "St")//  ми отримали дані з сервера(з первинної копії)
                //{
                //    //Q = (St1 + St0 - 2Ct0) / 2; 
                //    //Q разница во времени между сервером и клиентом сетевой игры
                //    //St0 момент передачи сервером сетевой игры первого пакета данных клиенту сетевой игры
                //    //St1 момент получения сервером сетевой игры второго пакета данных от клиента сетевой игры
                //    //Ct0 момент передачи клиентом сетевой игры второго пакета данных серверу сетевой игры
                //    Delt = (float)jsonObj["Time"] - 2 * GamePage.time; //St0 - 2Ct0 (Delt)
                //    this.SendAnswer((int)jsonObj["Item"]);// дані відсилає назад клієнт(вторинна копія)(вдісилає тільки Delt)
                    
                //}
                


            }                      

            //_circleBody.Position = new Vector2((float)((_circleBody.Position.X + float.Parse(jsonObj["PosX"].ToString())) * 0.5), (float)((_circleBody.Position.Y + float.Parse(jsonObj["PosY"].ToString())) * 0.5));//(_circleBody.Position + new Vector2(, float.Parse(jsonObj["PosY"].ToString()))); 
            ////////////////////////////////////////////////
            //double targetX = (_circleBody.Position.X + float.Parse(jsonObj["PosX"].ToString())) * 0.5;
            //double targetY = (_circleBody.Position.Y + float.Parse(jsonObj["PosY"].ToString())) * 0.5;
            //double errorX = targetX - _circleBody.Position.X;
            //double errorY = targetY - _circleBody.Position.Y;
            //_circleBody.ApplyForce(new Vector2((float)(1 * errorX), (float)(1 * errorY)));
            //_circleBody.Position = new Vector2((float)((float.Parse(_jsonObj["PosX"].ToString()))), (float)((float.Parse(_jsonObj["PosY"].ToString()))));//(_circleBody.Position + new Vector2(, float.Parse(jsonObj["PosY"].ToString()))); 
            
        }


        public void GetPos()//JObject jsonObj)
        {
            //_getObj = jsonObj;
            //double targetX = (_circleBody.Position.X + float.Parse(jsonObj["PosX"].ToString())) * 0.5;
            //double targetY = (_circleBody.Position.Y + float.Parse(jsonObj["PosY"].ToString())) * 0.5;
            //double errorX = targetX - _circleBody.Position.X;
            //double errorY = targetY - _circleBody.Position.Y;
            //_circleBody.ApplyForce(new Vector2((float)(1 * errorX), (float)(1 * errorY)));
            //_circleBody.Position = new Vector2(float.Parse(_getObj["PosX"].ToString()), float.Parse(_getObj["PosY"].ToString()));

        }

        public void DrawBall(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            spriteBatch.Draw(_circleSprite, ConvertUnits.ToDisplayUnits(_circleBody.Position), null, Color.White, ConvertUnits.ToDisplayUnits(_circleBody.Rotation), ConvertUnits.ToDisplayUnits(_circleCenter), 1f, SpriteEffects.None, 0f);
            
            //                спрайт            позиція тіла                             обертання тіла     центер текстури(зміщуємо верхній лівий кут в центр)
            
            //spriteBatch.End();
        }
    }
}
