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
        public bool isTarget;
        public Body _circleBody;
        private Texture2D _circleSprite;
        private Vector2 _circleCenter; // центр текстури для методу Draw
        private float _speed = 68;
        private Vector2 _clickPos = new Vector2(0, 0);
        private Vector2 _direction = new Vector2();
        private double _deltaDir;//містить різницю позицій між копіями.
        private double _DistDelta = 0;//якщо різниця координат більша за це то жорстко синхронізуємо;
        //
        private Vector2 colisVector;
        float dist;//диса=танція яку пройшов шар за час затримки даних (пінг * на шв шара).
        //public Vector2 SpritePos = new Vector2(0, 0);
        private JObject _jObj = new JObject();
        //private float _time;
        //private JObject _jsonPos = new JObject();
        //public JObject _getObj = new JObject();
        
        //float speed = 5f;
        Vector2[] PosList1 = new Vector2[2]; // ліст з останніми двома позиціями первинної копіє які прийшли з даними
        Vector2[] PosList2 = new Vector2[2]; // ліст з останніми двома позиціями вториннох(локальної) копії
        int masI = 0;
        //string[] Mas = new string[2];

        //public static Vector2[] cPos;
        //public static Vector2[] sPos;
        //public static int masItem; 
        Vector2 OldForse = new Vector2(0, 0);
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
            _circleBody = BodyFactory.CreateCircle(_world, ConvertUnits.ToSimUnits(_circleSprite.Height / 2f), 1f, ConvertUnits.ToSimUnits(new Vector2((i + 1) * ((i + 1) * (_circleSprite.Height / 2 + 5)), _circleSprite.Height / 2 + 5))); // (світ, радіус(визнач за текстурою), шось,початкова позиція тіла)
            _circleBody.BodyType = BodyType.Static;
            _circleBody.Restitution = 0.8f;
            _circleBody.Friction = 0.5f;
            _circleBody.OnCollision += MyOnCollision;
            //GetMoveDir(_circleBody.Position); //////////!!!!!!!!!!!!!!!!!!!!!!!!
            //SendDates(); // initial Jobj
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
            //_circleBody.Position += new Vector2((((time2 / 1000) * ConvertUnits.ToSimUnits(_speed)) / 1),(((time2 / 1000) * ConvertUnits.ToSimUnits(_speed)) / 1));
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
            _jObj.Add("PosX", this._circleBody.Position.X.ToString());
            _jObj.Add("PosY", this._circleBody.Position.Y.ToString()); 
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
            _jObj.Add("PosX", this._circleBody.Position.X.ToString());
            _jObj.Add("PosY", this._circleBody.Position.Y.ToString());             
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

        public void GetDelt(JObject jsonObj)//отримуємо різницю часу і обч Q
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
                //this.getSynchronization(jsonObj);
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

        public void getPosSynchronization(JObject jsonObj)
        {
            if ((int)jsonObj["Item"] != GamePage.masItem)
            {
                dist = (((time2 / 1000) * ConvertUnits.ToSimUnits(_speed)) / 1);

                _circleBody.Position = new Vector2(float.Parse(jsonObj["PosX"].ToString()), float.Parse(jsonObj["PosY"].ToString()));

                
                //_circleBody.LinearVelocity += new Vector2((float.Parse(jsonObj["PosX"].ToString()) - _circleBody.Position.X), (float.Parse(jsonObj["PosY"].ToString()) - _circleBody.Position.Y));
                //_circleBody.ApplyForce(new Vector2((float.Parse(jsonObj["PosX"].ToString()) - _circleBody.Position.X), (float.Parse(jsonObj["PosY"].ToString()) - _circleBody.Position.Y)));
                //for (int i = 0; i < 30; i++) //розбиваємо dist на 30 частин і почучуть додаємо компенсуючи затримку.
                //{
                    //_circleBody.Position += new Vector2(dist, dist);

                //}
                //_circleBody.ApplyForce(new Vector2(dist, dist));
            }
 
        }

        public void getSynchronization(JObject jsonObj)
        {
            if ((int)jsonObj["Item"] != GamePage.masItem)
            {
                colisVector = new Vector2((float.Parse(jsonObj["PosX"].ToString())) - _circleBody.Position.X, (float.Parse(jsonObj["PosY"].ToString())) - _circleBody.Position.Y);
                //dist = 0;
                //dist = (((time2 / 1000) * ConvertUnits.ToSimUnits(_speed)) / 1);
                //_DistDelta = Math.Sqrt(Math.Pow((_circleBody.Position.X - double.Parse(jsonObj["PosX"].ToString())), 2) + Math.Pow((_circleBody.Position.Y - double.Parse(jsonObj["PosY"].ToString())), 2));

                //_circleBody.ApplyForce(-OldForse);
                //OldForse = colisVector;
                //_circleBody.ApplyForce(colisVector);

                
                _circleBody.Position += new Vector2((float.Parse(jsonObj["PosX"].ToString()) - _circleBody.Position.X) / 2, (float.Parse(jsonObj["PosY"].ToString()) - _circleBody.Position.Y) / 2);

                //_circleBody.LinearVelocity += ( new Vector2((float.Parse(jsonObj["PosX"].ToString()) - _circleBody.Position.X), (float.Parse(jsonObj["PosY"].ToString()) - _circleBody.Position.Y)));
                //if (_DistDelta > dist)
                //{
                //_circleBody.Position += new Vector2((float.Parse(jsonObj["PosX"].ToString()) - _circleBody.Position.X) / 2, (float.Parse(jsonObj["PosY"].ToString()) - _circleBody.Position.Y) / 2);
                //_circleBody.ApplyForce(ConvertUnits.ToDisplayUnits(new Vector2((float.Parse(jsonObj["PosX"].ToString()) - _circleBody.Position.X), (float.Parse(jsonObj["PosY"].ToString()) - _circleBody.Position.Y))));
                //}

                // //пінг(час в секундах) множимо на відстань яку пройде шар за секунту і знаємо відстань яку мала пройти вторинна копія за час який йшли дані.
                //_deltaDir = Math.Abs(Math.Sqrt(Math.Pow((_circleBody.Position.X - double.Parse(jsonObj["PosX"].ToString())), 2) + Math.Pow((_circleBody.Position.Y - double.Parse(jsonObj["PosY"].ToString())), 2)));
                ////_DistDelta = Math.Sqrt(Math.Pow((_circleBody.Position.X - double.Parse(jsonObj["PosX"].ToString())), 2) + Math.Pow((_circleBody.Position.Y - double.Parse(jsonObj["PosY"].ToString())), 2));
                //if (masI == 2)
                //{
                //    masI = 0;
                //    //_DistDelta = (Math.Sqrt(Math.Pow((PosList[0].X - PosList[1].X), 2) + Math.Pow((PosList[0].Y - PosList[1].Y), 2)));//відстань між сусідніми позиціями шара
                //    //double match = (Math.Sqrt(Math.Pow(_direction.X, 2) + Math.Pow(_direction.Y, 2))) / _DistDelta;//в сікільки раз поз між сусідніми поз менша за direction
                //    //_DistDelta = (Math.Sqrt(Math.Pow(_direction.X, 2) + Math.Pow(_direction.Y, 2))) / match;
                //    //_circleBody.Position += new Vector2(PosList[0].X - PosList[1].X, PosList[0].Y - PosList[1].Y); 
                //    //_circleBody.Position += ( new Vector2(PosList1[1].X - PosList2[1].X, PosList1[1].Y - PosList2[1].Y));
                //}
                //PosList1[masI] = new Vector2(float.Parse(jsonObj["PosX"].ToString()), float.Parse(jsonObj["PosY"].ToString()));
                //PosList1[masI] = _circleBody.Position;
                //masI++;
                //////

                

                /////
                ////for (int i = 0; i < 30; i++)
                ////{
                ////    _circleBody.Position += new Vector2((float)(_DistDelta / 30), (float)(_DistDelta / 30));
                ////}

                //    //if (_deltaDir > dist) //якщо різниця в відстанні більша за відстань, що мала пройти вторинна копія за час який йшли дані то жорстко прирівнюємо позиції
                //    //{
                //    //_circleBody.Position = new Vector2(float.Parse(jsonObj["PosX"].ToString()), float.Parse(jsonObj["PosY"].ToString()) );
                //    //}
                //    //for (int i = 0; i < 30; i++) //розбиваємо dist на 30 частин і почучуть додаємо компенсуючи затримку.
                //    //{
                //    //    _circleBody.Position += new Vector2(dist / 30, dist / 30);

                //    //}
                //if (_deltaDir > dist) //якщо різниця в відстанні більша за відстань, що мала пройти вторинна копія за час який йшли дані то жорстко прирівнюємо позиції
                //{
                //    //_circleBody.ApplyLinearImpulse(new Vector2(dist,dist), _circleBody.Position);
                //    //float match = _direction.X / dist;
                //    //_circleBody.Position += new Vector2(_circleBody.LinearVelocity.X / match, _circleBody.LinearVelocity.Y / match);
                //}

                //if (_deltaDir > dist) //якщо різниця в відстанні більша за відстань, що мала пройти вторинна копія за час який йшли дані то жорстко прирівнюємо позиції
                //{
                //    //тут ЗРОБИТИ ЗБІЛЬШЕННЯ КООРДИНАТ ВЕКТОРА НА ВІДСТАНЬ
                //    //_circleBody.Position = new Vector2(float.Parse(jsonObj["PosX"].ToString()) + (float.Parse(jsonObj["PosX"].ToString()) * (time2 / 1000)), float.Parse(jsonObj["PosY"].ToString()) + (float.Parse(jsonObj["PosY"].ToString()) * (time2 / 1000)));
                //    //dist = (((time2 / 1000) * ConvertUnits.ToSimUnits(_speed)) / 1);
                //    //double dist2 = Math.Sqrt(Math.Pow(_circleBody.Position.X, 2) + Math.Pow(_circleBody.Position.Y, 2));
                //    //float match = (float)(dist2 / dist);

                //    _circleBody.Position = new Vector2(float.Parse(jsonObj["PosX"].ToString()), float.Parse(jsonObj["PosY"].ToString()));

                //}
                //else
                //{


                //    //_circleBody.Position += new Vector2(dist, dist);



                //}
            }

        }

        public void DrawBall(SpriteBatch spriteBatch)
        {
            Color color;
            if (isTarget == true)
            {
                color = Color.Red;
            }
            else
            {
                color = Color.White;
            }
            
            //spriteBatch.Begin();
            spriteBatch.Draw(_circleSprite, ConvertUnits.ToDisplayUnits(_circleBody.Position), null, color, ConvertUnits.ToDisplayUnits(_circleBody.Rotation), ConvertUnits.ToDisplayUnits(_circleCenter), 1f, SpriteEffects.None, 0f);
            
            //                спрайт            позиція тіла                             обертання тіла     центер текстури(зміщуємо верхній лівий кут в центр)
            
            //spriteBatch.End();
        }

        public bool MyOnCollision(Fixture f1, Fixture f2, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (f1.Body.BodyId == _circleBody.BodyId && f2.Body.BodyId != WorldSet.borderBody.BodyId)
            {
                //_circleBody.Position = colisVector;
                _circleBody.LinearVelocity = _circleBody.LinearVelocity * -1;//при зіткненні 2 шарів міняємо напрамок на протилежний
                if (isTarget == true)
                {
                    isTarget = false;
                }
                else
                {
                    isTarget = true;
                }

            }

            if (f1.Body.BodyId == _circleBody.BodyId && f2.Body.BodyId == WorldSet.borderBody.BodyId)
            {
                //якщо cтик з правим та нижнім бордером
                if ((_circleBody.Position.X + ConvertUnits.ToSimUnits(_circleSprite.Width / 2f)) > ConvertUnits.ToSimUnits(800 - 20))
                {
                    _circleBody.LinearVelocity = new Vector2(_circleBody.LinearVelocity.X * -1, _circleBody.LinearVelocity.Y);
                    //isTarget = false;
                }

                if ((_circleBody.Position.Y + ConvertUnits.ToSimUnits(_circleSprite.Width / 2f)) > ConvertUnits.ToSimUnits(480 - 20))
                {
                    _circleBody.LinearVelocity = new Vector2(_circleBody.LinearVelocity.X, _circleBody.LinearVelocity.Y * -1);
                    //isTarget = false;
                }

                /////якщо cтик з лівим та верхнім бордером
                if ((_circleBody.Position.X - ConvertUnits.ToSimUnits(_circleSprite.Width / 2f + 20)) < 0)
                {
                    _circleBody.LinearVelocity = new Vector2(_circleBody.LinearVelocity.X * -1, _circleBody.LinearVelocity.Y);
                    //isTarget = false;
                }

                if ((_circleBody.Position.Y - ConvertUnits.ToSimUnits(_circleSprite.Width / 2f + 20)) < 0)
                {
                    _circleBody.LinearVelocity = new Vector2(_circleBody.LinearVelocity.X, _circleBody.LinearVelocity.Y * -1);
                    //isTarget = false;
                }
                
            }
       
         return true;

        }


       
    }
}
