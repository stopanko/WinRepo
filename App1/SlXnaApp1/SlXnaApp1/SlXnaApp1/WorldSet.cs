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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//farseer
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
//
namespace SlXnaApp1
{
    public class WorldSet
    {
        
        public World _world; // світ з початковою гравітацією
        float width;
        float height;
        private Body borderBody;
        

        public void InitWorld()
        {
            _world = new World(new Vector2(0, 0));
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            width = ConvertUnits.ToSimUnits(800);
            height = ConvertUnits.ToSimUnits(480);
        }

        public void InitBorders()
        {
            Vertices borders = new Vertices(4);
            borders.Add(new Vector2(0, 0));
            borders.Add(new Vector2(width, 0));
            borders.Add(new Vector2(width, height));
            borders.Add(new Vector2(0, height));

            borderBody = BodyFactory.CreateLoopShape(_world, borders);
            borderBody.CollisionCategories = Category.All;
            borderBody.CollidesWith = Category.All;
        }

    }
}
