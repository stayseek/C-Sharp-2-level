using System;
using System.Drawing;

namespace Asteroids
{
    class Ship : BaseObject
    {
        /// <summary>
        /// Энергия корабля.
        /// </summary>
        private int _energy = 100;
        /// <summary>
        /// Энергия корабля (Свойство).
        /// </summary>
        public int Energy => _energy;
        /// <summary>
        /// Максимально возможный запас энергии.
        /// </summary>
        const int MAXENERGY = 100;
        /// <summary>
        /// Событие уничтожения корабля.
        /// </summary>
        public static event Message MessageDie;
        /// <summary>
        /// Изменение энергии корабля.
        /// </summary>
        /// <param name="n">Количество вычитаемой энергии.</param>
        public void EnergyLow(int n)
        {
            _energy -= n;
            if (_energy > MAXENERGY) _energy = MAXENERGY;
        }
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="pos">Положение на экране.</param>
        /// <param name="dir">Направление движения.</param>
        /// <param name="size">Размер объекта.</param>
        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        /// <summary>
        /// Отрисовка объекта.
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.Wheat, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        /// <summary>
        /// Обновление позиции объекта. 
        /// </summary>
        public override void Update()
        {
        }
        /// <summary>
        /// Движение корабля вверх.
        /// </summary>
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }
        /// <summary>
        /// Движение корабля вниз.
        /// </summary>
        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }
        /// <summary>
        /// Вызов события уничтожения корабля.
        /// </summary>
        public void Die()
        {
            MessageDie.Invoke();
        }
    }
}
