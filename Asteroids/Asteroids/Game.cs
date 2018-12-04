using System;
using System.Windows.Forms;
using System.Drawing;

namespace Asteroids
{
    static class Game
    {
        /// <summary>
        /// Буфер графического контекста для текущего приложения.
        /// </summary>
        private static BufferedGraphicsContext _context;
        /// <summary>
        /// Буфер в памяти.
        /// </summary>
        public static BufferedGraphics Buffer;
        /// <summary>
        /// Ширина экрана.
        /// </summary>
        public static int Width { set; get; }
        /// <summary>
        /// Высота экрана.
        /// </summary>
        public static int Height { set; get; }

        /// <summary>
        /// Массив объектов фона.
        /// </summary>
        private static BaseObject[] _objs;
        /// <summary>
        /// Пуля.
        /// </summary>
        private static Bullet _bullet;
        /// <summary>
        /// Массив астероидов.
        /// </summary>
        private static Asteroid[] _asteroids;
        /// <summary>
        /// Максимальная размерность экрана для выброса исключения.
        /// </summary>
        const int MAXSCREENSIZE = 1600;
        /// <summary>
        /// Количество астероидов на экране.
        /// </summary>
        const int ASTEROIDSCOUNT = 30;
        
        static Game()
        {
        }
        /// <summary>
        /// Инициализация графической системы, вызов загрузки объектов и запуск таймера.
        /// </summary>
        /// <param name="form">Форма для вывода графики.</param>
        public static void Init(Form form)
        {
            Graphics g; // Графическое устройство для вывода графики
            _context = BufferedGraphicsManager.Current; // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            g = form.CreateGraphics();
            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            if (Width > MAXSCREENSIZE || Height > MAXSCREENSIZE || Width < 0 || Height < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height)); // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Load(); //Загрузка объектов
            Timer timer = new Timer { Interval = 100 }; //Таймер
            timer.Start();
            timer.Tick += Timer_Tick;
        }
        /// <summary>
        /// Создание массивов объектов, астероидов и пули, задание их начальных положений, скорости и размеров.
        /// </summary>
        public static void Load()
        {
            _objs = new BaseObject[30];
            _asteroids = new Asteroid[ASTEROIDSCOUNT];
            //try
            //{
                _bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(4, 1));
                var rnd = new Random();
                for (var i = 0; i < _objs.Length; i++)
                {
                    int r = rnd.Next(5, 50);
                    _objs[i] = new Star(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
                }
                for (var i = 0; i < _asteroids.Length; i++)
                {
                    int r = rnd.Next(5, 50);
                    _asteroids[i] = new Asteroid(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r));
                }
            //}
            //catch (GameObjectException)
            //{
            //}
            
        }
        /// <summary>
        /// Очистка экрана и вывод на экран игровых объектов. 
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (Asteroid obj in _asteroids)
                obj.Draw();
            _bullet.Draw();
            Buffer.Render();
        }
        /// <summary>
        /// Обновление положения игровых объектов, отслеживание столкновения пули с астероидом и осуществление их регенерации в разных концах экрана.
        /// </summary>
        public static void Update()
        {
            foreach (BaseObject obj in _objs)
                obj.Update();
            foreach (Asteroid a in _asteroids)
            {
                a.Update();
                if (a.Collision(_bullet))
                {
                    System.Media.SystemSounds.Hand.Play();
                    a.RegenerateAtX(Width);
                    _bullet.RegenerateAtX(0);
                }
            }
            _bullet.Update();
        }
        /// <summary>
        /// Событие срабатывания таймера.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

    }
}
