using System;
using System.Windows.Forms;

namespace Asteroids
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form
            {
                Width = Screen.PrimaryScreen.WorkingArea.Width,
                Height = Screen.PrimaryScreen.WorkingArea.Height
            };
            Game.Init(form);
            form.SetDesktopLocation(0, 0);
            form.Show();
            Application.Run(form);
        }

    }
}
