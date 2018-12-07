using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

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
        /// Массив аптечек.
        /// </summary>
        private static HealthPack[] _healthpacks;
        /// <summary>
        /// Корабль игрока.
        /// </summary>
        private static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(10, 10));
        /// <summary>
        /// Счёт игры.
        /// </summary>
        private static int _score;
        /// <summary>
        /// Таймер для событий.
        /// </summary>
        private static Timer _timer = new Timer();
        /// <summary>
        /// Рандомизатор.
        /// </summary>
        public static Random rnd = new Random();
        /// <summary>
        /// Максимальная размерность экрана для выброса исключения.
        /// </summary>
        const int MAXSCREENSIZE = 1920;
        /// <summary>
        /// Количество астероидов на экране.
        /// </summary>
        const int ASTEROIDSCOUNT = 30;
        /// <summary>
        /// Начальное количество аптечек.
        /// </summary>
        const int HEALTHPACKSCOUNT = 5;
        /// <summary>
        /// Скорость пули.
        /// </summary>
        const int BULLETSPEED = 20;
        /// <summary>
        /// Стоимость уничтожения одного астероида.
        /// </summary>
        const int ASTEROIDSCORE = 10;
        /// <summary>
        /// Имя файла лога.
        /// </summary>
        const string LOGFILENAME = "game.log";
        /// <summary>
        /// Делегат для функции логирования.
        /// </summary>
        private static Action<string> WriteToLog;

        static Game()
        {
        }
        /// <summary>
        /// Инициализация графической системы, вызов загрузки объектов, запуск таймера, регистрация событий таймера, управления кораблёмб окончания игры.
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
            WriteToLog = LogToConsole;
            WriteToLog += LogToFile;
            WriteToLog($"Игра началась.");
            Load(); //Загрузка объектов
            _score = 0;
            _timer.Start();
            _timer.Tick += Timer_Tick;
            form.KeyDown += Form_KeyDown;
            Ship.MessageDie += Finish;
        }
        /// <summary>
        /// Создание массивов объектов, астероидов и пули, задание их начальных положений, скорости и размеров.
        /// </summary>
        private static void Load()
        {
            _objs = new BaseObject[30];
            _asteroids = new Asteroid[ASTEROIDSCOUNT];
            _healthpacks = new HealthPack[HEALTHPACKSCOUNT];

            for (var i = 0; i < _objs.Length/2; i++)
            {
                int r = rnd.Next(5, 50);
                _objs[i] = new Star(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
                WriteToLog($"Звезда {i} создана.");
            }
            for (var i = _objs.Length / 2; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _objs[i] = new Planet(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(16, 16));
                WriteToLog($"Планета {i} создана.");
            }
            for (var i = 0; i < _asteroids.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _asteroids[i] = new Asteroid(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r));
                WriteToLog($"Астероид {i} создан.");
            }
            for (var i = 0; i < _healthpacks.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _healthpacks[i] = new HealthPack(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(16, 16));
                WriteToLog($"Аптечка {i} создана.");
            }
        }
        /// <summary>
        /// Очистка экрана и вывод на экран игровых объектов. 
        /// </summary>
        private static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (Asteroid a in _asteroids)
            {
                if (a == null) continue;
                a.Draw();
            }
            foreach (HealthPack h in _healthpacks)
            {
                if (h == null) continue;
                h.Draw();
            }
            if (_bullet != null)
            {
                _bullet.Draw();
            }
            if (_ship != null)
            {
                _ship.Draw();
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy + "\nScore:" + _score, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            }    
            Buffer.Render();
        }
        /// <summary>
        /// Обновление положения игровых объектов, отслеживание столкновений.
        /// </summary>
        public static void Update()
        {
            foreach (BaseObject obj in _objs) obj.Update();
            if (_bullet != null)
            {
                _bullet.Update();
                if (_bullet.OffScreen) _bullet = null;
            }
            for (var i = 0; i < _asteroids.Length; i++)
            {
                if (_asteroids[i] == null) continue;
                _asteroids[i].Update();
                if (_bullet != null && _bullet.Collision(_asteroids[i]))
                {
                    System.Media.SystemSounds.Hand.Play();
                    _asteroids[i] = null;
                    _bullet = null;
                    _score += ASTEROIDSCORE;
                    WriteToLog($"Астероид {i} уничтожен.");
                    continue;
                }
                if (!_ship.Collision(_asteroids[i])) continue;
                _ship.EnergyLow(rnd.Next(1, 10));
                System.Media.SystemSounds.Asterisk.Play();
                WriteToLog($"Корабль столкнулся с астероидом {i}.");
                if (_ship.Energy <= 0) _ship.Die();
            }
            for (var i = 0; i < _healthpacks.Length; i++)
            {
                if (_healthpacks[i] == null) continue;
                _healthpacks[i].Update();
                if (!_ship.Collision(_healthpacks[i])) continue;
                _ship.EnergyLow(-rnd.Next(1, 10));
                _healthpacks[i].Dispose();
                _healthpacks[i] = null;
                WriteToLog($"Аптечка {i} собрана.");
                System.Media.SystemSounds.Asterisk.Play();
            }
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
        /// <summary>
        /// События управления кораблём.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Данные события нажатия клавиши.</param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                if (_bullet == null)
                {
                    _bullet = new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4), new Point(BULLETSPEED, 0), new Size(4, 1));
                    WriteToLog($"Произошел выстрел.");
                }
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }
        /// <summary>
        /// Событие окончания игры.
        /// </summary>
        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
            WriteToLog($"Игра закончена.");
        }
        /// <summary>
        /// Запись лога в консоль.
        /// </summary>
        /// <param name="message"></param>
        private static void LogToConsole (string message)
        {
            Console.WriteLine(message);
        }
        /// <summary>
        /// Запись лога в файл.
        /// </summary>
        /// <param name="message"></param>
        private static void LogToFile(string message)
        {
            using (var r = new StreamWriter(LOGFILENAME,true))
            {
                r.WriteLine(message);
            }
        }
    }
}
