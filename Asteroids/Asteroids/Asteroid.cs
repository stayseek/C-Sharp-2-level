using System;
using System.Drawing;

namespace Asteroids
{
    class Asteroid : BaseObject
    {
        /// <summary>
        /// Энергия или сила удара, будет известно в дальнейшем.
        /// </summary>
        public int Power { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="pos">Положение на экране.</param>
        /// <param name="dir">Направление движения.</param>
        /// <param name="size">Размер объекта.</param>
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }
        /// <summary>
        /// Отрисовка объекта
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.White, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        /// <summary>
        /// Обновление позиции объекта. 
        /// </summary>
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
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
