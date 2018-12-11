using System;
using System.Drawing;

namespace Asteroids
{
    class HealthPack: Asteroid, IDisposable
    {
        /// <summary>
        /// Изображение аптечки.
        /// </summary>
        private Image healthpackImage;
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="pos">Положение на экране.</param>
        /// <param name="dir">Направление движения.</param>
        /// <param name="size">Размер объекта.</param>
        public HealthPack(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            healthpackImage = Image.FromFile(@"..\..\images\healthpack.png");
        }

        public void Dispose()
        {
            healthpackImage.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Отрисовка объекта
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(healthpackImage, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
    }
}
