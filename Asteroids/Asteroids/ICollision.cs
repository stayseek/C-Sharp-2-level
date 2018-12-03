using System;
using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Реализация проверки столкновения с другим объектом.
    /// </summary>
    interface ICollision
    {
        /// <summary>
        /// Проверка столкновения с другим объектом.
        /// </summary>
        /// <param name="obj">Объект с которым проверяется столкновение.</param>
        /// <returns>Результат проверки.</returns>
        bool Collision(ICollision obj);
        /// <summary>
        /// Прямоугольник, в который вписан объект.
        /// </summary>
        Rectangle Rect { get; }
    }

}
