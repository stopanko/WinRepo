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
        //ball settings
        private Body _circleBody;
        private Texture2D _circleSprite;
        private Vector2 _circleCenter; // центр текстури для методу Draw
        private float _speed = 48;
        private Vector2 _clickPos = new Vector2();
        private Vector2 _direction = new Vector2();
        //

        public Vector2 SpritePos = new Vector2(0, 0);
        
        
        //float speed = 5f;

        string[] Mas = new string[2];

        //public static Vector2[] cPos;
        //public static Vector2[] sPos;
        //public static int masItem; 


        //List<JObject> ballsList = new List<JObject>();

        //List<Vector2> Ballslist = new List<Vector2>();

        public void InitBallContent()
        {
            _circleSprite = GamePage.contentManager.Load<Texture2D>("CircleSprite"); //текстура
            _circleCenter = ConvertUnits.ToSimUnits(new Vector2(_circleSprite.Width / 2f, _circleSprite.Height / 2f));//знаходимо центр текстури
            _circleBody = BodyFactory.CreateCircle(GamePage._world, ConvertUnits.ToSimUnits(_circleSprite.Height / 2f), 1f, ConvertUnits.ToSimUnits(new Vector2(50, 50))); // (світ, радіус(визнач за текстурою), шось,початкова позиція тіла)
            _circleBody.BodyType = BodyType.Dynamic;
            _circleBody.Restitution = 0.8f;
            _circleBody.Friction = 0.5f;
        }

        public void GetMoveDir(Vector2 ClickPos)//передамо вектор кліку
        {
            //Vector2 ClickPos = new Vector2(loc.X, loc.Y);
            Vector2 DiffDist = ClickPos - ConvertUnits.ToDisplayUnits(_circleBody.Position); // шукає різницю між позицією кліку та позицією шару для того, щоб побудувати вектор в напрямку кліку з початку коордтнат.
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

        public void SendDates()
        {
            
            JObject sendObj = new JObject();
            sendObj.Add("X", this._clickPos.X);
            sendObj.Add("Y", this._clickPos.Y);
            sendObj.Add("Item", GamePage.masItem);
            
            WarpClient.GetInstance().SendUpdatePeers(System.Text.Encoding.UTF8.GetBytes(sendObj.ToString()));//sendObj.ToString()));
        }


        public void GetDates(UpdateEvent eventObj)
        {

            JObject jsonObj = JObject.Parse(System.Text.Encoding.UTF8.GetString(eventObj.getUpdate(), 0, eventObj.getUpdate().Length));
            _clickPos.X = int.Parse(jsonObj["X"].ToString());
            _clickPos.Y = int.Parse(jsonObj["Y"].ToString());
            this.GetMoveDir(_clickPos);       
 
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
