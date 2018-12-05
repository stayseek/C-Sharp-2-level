using System;
using System.Drawing;

namespace Asteroids
{
    class Planet : BaseObject
    {
        /// <summary>
        /// Изображение для отображения.
        /// </summary>
        private Image planetImage;
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="pos">Положение на экране.</param>
        /// <param name="dir">Направление движения.</param>
        /// <param name="size">Размер объекта.</param>
        public Planet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            planetImage = Image.FromFile(@"..\..\images\planet.png");
        }
        /// <summary>
        /// Отрисовка объекта.
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(planetImage, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        /// <summary>
        /// Обновление позиции объекта. 
        /// </summary>
        public override void Update()
        { 
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }
    }
}
