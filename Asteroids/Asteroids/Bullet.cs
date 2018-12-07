using System;
using System.Drawing;

namespace Asteroids
{
    class Bullet : BaseObject
    {
        /// <summary>
        /// Флаг вылета пули за кран отображаемой области.
        /// </summary>
        public bool OffScreen { private set; get; }
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="pos">Положение на экране.</param>
        /// <param name="dir">Направление движения.</param>
        /// <param name="size">Размер объекта.</param>
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            OffScreen = false;
        }
        /// <summary>
        /// Отрисовка объекта.
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawRectangle(Pens.OrangeRed, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        /// <summary>
        /// Обновление позиции объекта. 
        /// </summary>
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X > Game.Width)
            {
                OffScreen = true;
            }
        }
    }
}
