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
using Microsoft.Xna.Framework;

namespace SlXnaApp1
{
    public class Physics
    {


        //public static void FringePhysics(Ball B)
        //{

        //    if (B.SpritePos.X < 0)
        //    {
        //        B._direction.X *= -1; 
        //    }
        //    else if (B.SpritePos.X + 150 > 480)
        //    {
        //        B._direction.X *= -1;
        //    }
        //    else if (B.SpritePos.Y < 0)
        //    {
        //        B._direction.Y *= -1;
        //    }
        //    else if (B.SpritePos.Y + 150 > 800)
        //    {
        //        B._direction.Y *= -1; 
        //    }
        
        //}

        //public static void Colision(Ball[] masB)
        //{
        //    foreach (Ball b in masB)
        //    {
        //        for (int i = 0; i < masB.Length; i++)
        //        {
        //            //if (Math.Sqrt(Math.Pow(b.SpritePos.X - masB[i].SpritePos.X, 2) + (Math.Pow(b.SpritePos.Y - masB[i].SpritePos.Y, 2))) == 150 / 2)
        //            //{
        //            //GamePage.SendTxt = b.SpritePos.ToString() + masB[i].SpritePos.ToString() + "|" + (Math.Sqrt(Math.Pow(masB[i].SpritePos.X - masB[i+1].SpritePos.X, 2) + Math.Pow(masB[i].SpritePos.Y - masB[i+1].SpritePos.Y, 2))).ToString();
        //            Vector2 v = (b.SpritePos + new Vector2(150 / 2, 150 / 2)) - (masB[i].SpritePos + new Vector2(150 / 2, 150 / 2));
        //            double distance = Math.Sqrt((Math.Pow(v.X, 2) + Math.Pow(v.Y, 2)));
        //            if(distance !=0)
        //            {

        //                //GamePage.SendTxt = Math.Sqrt((Math.Pow(v.X, 2) + Math.Pow(v.Y, 2))).ToString();
        //                //GamePage.SendTxt = Math.Round(distance).ToString();
        //                if (Math.Round(distance) < 150 && Math.Round(distance) > 147)///!!!!!!!!!
        //                {
        //                    GamePage.SendTxt = "yes";
        //                    b._direction.X = -b._direction.X; masB[i]._direction.X *= -1;
        //                    //masB[i].SpritePos = new Vector2(0, 0);
        //                    //b.SpritePos += new Vector2(50, 50);
                            
        //                }
        //                else
        //                {
        //                    GamePage.SendTxt = "no";
        //                }
        //            }
        //            //}
        //            //else
        //            //{
        //            //    GamePage.SendTxt = "no";
        //            //}
        //        }
        //    }
 
        //}

    }
}
