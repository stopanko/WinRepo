using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client;
using System.Collections.Generic;
using System.Linq;
namespace SlXnaApp1
{
    public class Balls
    {

        public Vector2 SpritePos = new Vector2(0, 0);
        public Vector2 Direction = new Vector2();
        private Vector2 ClickPos = new Vector2();
        float speed = 5f;

        string[] Mas = new string[2];

        //public static Vector2[] cPos;
        //public static Vector2[] sPos;
        //public static int masItem; 




        List<JObject> ballsList = new List<JObject>();

        List<Vector2> Ballslist = new List<Vector2>();
        public void GetMoveDir(Vector2 clickpos)
        {
            
            ClickPos.X = clickpos.X;
            ClickPos.Y = clickpos.Y;
            
            Direction = ClickPos - SpritePos;
            Direction.Normalize();
            
            
           

        }


        public void MoveSprite(GameTimerEventArgs e)
        {

            SpritePos += Direction * (float)e.ElapsedTime.Milliseconds / 30 * speed * (float)e.ElapsedTime.Milliseconds / 30;
 
        }

        public void SendDates()
        {
            
            JObject sendObj = new JObject();
            sendObj.Add("X", this.ClickPos.X);
            sendObj.Add("Y", this.ClickPos.Y);
            sendObj.Add("Item", GamePage.masItem);
            
            WarpClient.GetInstance().SendUpdatePeers(System.Text.Encoding.UTF8.GetBytes(sendObj.ToString()));//sendObj.ToString()));
        }


        public void GetDates(UpdateEvent eventObj)
        {

            JObject jsonObj = JObject.Parse(System.Text.Encoding.UTF8.GetString(eventObj.getUpdate(), 0, eventObj.getUpdate().Length));
            ClickPos.X = int.Parse(jsonObj["X"].ToString());
            ClickPos.Y = int.Parse(jsonObj["Y"].ToString());
            this.GetMoveDir(ClickPos);
            
           
 
        }

    }
}
