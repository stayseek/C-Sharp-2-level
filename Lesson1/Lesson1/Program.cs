//Продвинутый курс C#
//Урок 1. Объектно-ориентированное программирование. Часть 1
//Домашнее задание.
//Автор: Станислав Митрофанов

using System;
using System.Windows.Forms;

namespace MyGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form();
            form.Width = 800;
            form.Height = 600;
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);
        }
    }
}
