using System;
using System.Drawing;

namespace Asteroids
{
    class Bullet : BaseObject
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="pos">Положение на экране.</param>
        /// <param name="dir">Направление движения.</param>
        /// <param name="size">Размер объекта.</param>
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
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
            Pos.X = Pos.X + 3;
        }
        /// <summary>
        /// Перенос объекта по координате x.
        /// </summary>
        /// <param name="posX">Новая координата x объекта.</param>
        public override void RegenerateAtX(int posX)
        {
            this.Pos.X = posX;
        }
    }
}
