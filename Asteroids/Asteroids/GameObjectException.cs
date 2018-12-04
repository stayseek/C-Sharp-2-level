using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class GameObjectException : Exception
    {
        /// <summary>
        /// Исключение при создании игрового объекта с ошибочными характеристиками.
        /// </summary>
        public GameObjectException()
        {
            Console.WriteLine(base.Message);
        }
    }
}
