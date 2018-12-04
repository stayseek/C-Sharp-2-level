using System;
using System.Drawing;

namespace Asteroids
{
    abstract class BaseObject : ICollision
    {
        /// <summary>
        /// Позиция объекта на экране.
        /// </summary>
        protected Point Pos;
        /// <summary>
        /// Направление движения объекта.
        /// </summary>
        protected Point Dir;
        /// <summary>
        /// Размер объекта.
        /// </summary>
        protected Size Size;
        /// <summary>
        /// Максимальная скорость по одной из координат
        /// </summary>
        const int MAXSPEED = 50;
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="pos">Положение на экране.</param>
        /// <param name="dir">Направление движения.</param>
        /// <param name="size">Размер объекта.</param>
        protected BaseObject(Point pos, Point dir, Size size)
        {
            if (pos.X < 0 || pos.Y < 0 || size.Width < 0 || size.Height < 0 || Math.Abs(dir.X) > MAXSPEED || Math.Abs(dir.Y) > MAXSPEED)
            {
                throw new GameObjectException();
            }
            Pos = pos;
            Dir = dir;
            Size = size;
        }

        public abstract void Draw();

        public abstract void Update();

        public abstract void RegenerateAtX(int posX);

        /// <summary>
        /// Проверка столкновения с другим объектом.
        /// </summary>
        /// <param name="o">Объект с которым проверяется столкновение.</param>
        /// <returns>Результат проверки.</returns>
        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);
        /// <summary>
        /// Прямоугольник, в который вписан объект.
        /// </summary>
        public Rectangle Rect => new Rectangle(Pos, Size);
    }
}
