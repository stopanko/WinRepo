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

namespace SlXnaApp1
{
    public class Physics
    {


        public static void FringePhysics(Balls B)
        {

            if (B.SpritePos.X < 0)
            {
                B.Direction.X *= -1; 
            }
            else if (B.SpritePos.X + 150 > 480)
            {
                B.Direction.X *= -1;
            }
            else if (B.SpritePos.Y < 0)
            {
                B.Direction.Y *= -1;
            }
            else if (B.SpritePos.Y + 150 > 800)
            {
                B.Direction.Y *= -1; 
            }
        
        }

    }
}
