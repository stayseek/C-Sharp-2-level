using System;
using System.Drawing;

namespace Asteroids
{
    class Asteroid : BaseObject, ICloneable, IComparable<Asteroid>
    {
        /// <summary>
        /// Наносимый урон.
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
           Power = 3;
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
        /// Создание копии существующего астероида. (создано по методичке, пока не используется)
        /// </summary>
        /// <returns>Копия астероида.</returns>
        public object Clone()
        {
            Asteroid asteroid = new Asteroid(new Point(Pos.X, Pos.Y), new Point(Dir.X, Dir.Y), new Size(Size.Width, Size.Height));
            asteroid.Power = Power;
            return asteroid;
        }
        /// <summary>
        /// Сравнение 2 астероидов по энергии. (создано по методичке, пока не используется)
        /// </summary>
        /// <param name="other">Астероид для сравнения.</param>
        /// <returns></returns>
        int IComparable<Asteroid>.CompareTo(Asteroid other)
        {
            if (Power > other.Power)
                return 1;
            if (Power < other.Power)
                return -1;
            return 0;
        }
    }
}
