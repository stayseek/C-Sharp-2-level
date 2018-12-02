using System;
using System.Windows.Forms;
using System.Drawing;

namespace Asteroids
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        // Свойства
        // Ширина и высота игрового поля
        public static int Width { set; get; }
        public static int Height { set; get; }

        public static BaseObject[] _objs;

        static Game()
        {
        }

        public static void Init(Form form)
        {
            // Графическое устройство для вывода графики
            Graphics g;
            // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            //Загрузка объектов
            Load();
            //Таймер
            Timer timer = new Timer { Interval = 100 };
            timer.Start();
            timer.Tick += Timer_Tick;
        }

        public static void Load()
        {
            _objs = new BaseObject[30];

            // Перепишем кусок, что бы элементы более "равномерно" распределились по экрану. 
            // Старый кусок закомментируем.

            //for (int i = 0; i < _objs.Length / 3; i++)
            //    _objs[i] = new BaseObject(new Point(600, i * 20), new Point(-i, -i), new Size(10, 10));
            //for (int i = _objs.Length / 3; i < 2*(_objs.Length/3); i++)
            //    _objs[i] = new Star(new Point(600, i * 20), new Point(-i, 0), new Size(5, 5));


            //"Волшебные" цифры правильнее будет вынести в константы, но тут ограничимся ими. 
            for (int i = 0; i < _objs.Length / 3; i++)
                _objs[i] = new BaseObject(new Point(600, i * 60), new Point((1 - i) * 3, 2 - i), new Size(10, 10));
            for (int i = _objs.Length / 3; i < 2 * (_objs.Length / 3); i++)
                _objs[i] = new Star(new Point(600, (i - 9) * 50), new Point(-i - 5 * (i % 2), 0), new Size(5, 5));
            for (int i = 2 * (_objs.Length / 3); i < _objs.Length; i++)
                _objs[i] = new Planet(new Point(600, (i - 20) * 60), new Point(-i - 10 - 20 * (i % 2), 0), new Size(16, 16));

        }

        public static void Draw()
        {
            // Как я понял, тут выводится "солнце", но последующим выводом наших элементов мы его "затираем", 
            // так что весь кусок пока можно закомментировать.

            // Проверяем вывод графики
            //Buffer.Graphics.Clear(Color.Black);
            //Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(100, 100, 200, 200));
            //Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(100, 100, 200, 200));
            //Buffer.Render();

            // Отрисовка объектов
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            Buffer.Render();
        }

        public static void Update()
        {
            foreach (BaseObject obj in _objs)
                obj.Update();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

    }
}
