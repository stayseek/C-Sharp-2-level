using System;
using System.Drawing;

namespace Asteroids
{
    class Planet : BaseObject
    {
        
        Image planetImage;

        public Planet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            //Путь до файла с изображением пропишем с относительными путями, учитывая что исполняемый файл будет лежать 2-мя "уровнями" ниже
            planetImage = Image.FromFile(@"..\..\images\planet.png");
        }
        //Можно так же создать ещё один конструктор, в который бы передавать или путь до изображения, или Image. 
        //Но так как изображение планеты одно, можно ограничится и простым вариантом.

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(planetImage, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            //Если наследовать класс от Star можно не переопределять этот метод, поведение у них одинаковое.  
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }

    }
}
