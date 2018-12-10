using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

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
        /// Список пуль.
        /// </summary>
        private static List<Bullet> _bullets = new List<Bullet>();
        /// <summary>
        /// Список астероидов.
        /// </summary>
        private static List<Asteroid> _asteroids = new List<Asteroid>();
        /// <summary>
        /// Список аптечек.
        /// </summary>
        private static List<HealthPack> _healthpacks = new List<HealthPack>();
        /// <summary>
        /// Корабль игрока.
        /// </summary>
        private static Ship _ship; 
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
        /// Логгер.
        /// </summary>
        private static Logger Log;
        /// <summary>
        /// Количество астероидов на экране.
        /// </summary>
        private static int AsteroidsCount;
        /// <summary>
        /// Максимальная размерность экрана для выброса исключения.
        /// </summary>
        const int MAXSCREENSIZE = 1920;
        /// <summary>
        /// Начальное количество астероидов на экране.
        /// </summary>
        const int STARTASTEROIDSCOUNT = 3;
        /// <summary>
        /// Начальное количество аптечек.
        /// </summary>
        const int HEALTHPACKSCOUNT = 5;
        /// <summary>
        /// Максимальное количество пуль на экране.
        /// </summary>
        const int MAXBULLETSCOUNT = 5;
        /// <summary>
        /// Скорость пули.
        /// </summary>
        const int BULLETSPEED = 10;
        /// <summary>
        /// Стоимость уничтожения одного астероида.
        /// </summary>
        const int ASTEROIDSCORE = 10;
        /// <summary>
        /// Имя файла лога.
        /// </summary>
        const string LOGFILENAME = "game.log";

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

            Log = new Logger(LOGFILENAME);
            Ship.MessageDie += Finish;
            Ship.CreateShip += Log.WriteToLog;
            _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(10, 10));
            Load(); //Загрузка объектов
            _score = 0;
            _timer.Start();
            _timer.Tick += Timer_Tick;
            form.KeyDown += Form_KeyDown;
            Log.WriteToLog($"Игра началась.");
        }
        /// <summary>
        /// Создание массивов объектов, астероидов и пули, задание их начальных положений, скорости и размеров.
        /// </summary>
        private static void Load()
        {
            _objs = new BaseObject[30];
            for (var i = 0; i < _objs.Length/2; i++)
            {
                _objs[i] = new Star(new Point(1000, rnd.Next(0, Game.Height)), new Point(-rnd.Next(5, 50), rnd.Next(5, 50)), new Size(3, 3));
                Log.WriteToLog($"Звезда {i} создана.");
            }
            for (var i = _objs.Length / 2; i < _objs.Length; i++)
            {
                _objs[i] = new Planet(new Point(1000, rnd.Next(0, Game.Height)), new Point(-rnd.Next(5, 15), rnd.Next(5, 50)), new Size(16, 16));
                Log.WriteToLog($"Планета {i} создана.");
            }
            AsteroidsCount = STARTASTEROIDSCOUNT;
            LoadAsteroids(AsteroidsCount);
            LoadHealthPacks();
        }
        /// <summary>
        /// Загрузка астероидов.
        /// </summary>
        /// <param name="Count">Количество астероидов.</param>
        private static void LoadAsteroids(int Count)
        {
            for (int i = 0; i < Count; i++)
            {
                int r = rnd.Next(10, 50);
                _asteroids.Add(new Asteroid(new Point(1000, rnd.Next(0, Game.Height-r)), new Point(-r / 5, r), new Size(r, r)));
                Log.WriteToLog($"Астероид {i} создан.");
            }
        }
        /// <summary>
        /// Загрузка аптечек до заданного предела.
        /// </summary>
        private static void LoadHealthPacks()
        {
            for (int i = _healthpacks.Count; i < HEALTHPACKSCOUNT; i++)
            {
                int r = rnd.Next(5, 50);
               _healthpacks.Add(new HealthPack(new Point(1000, rnd.Next(0, Game.Height-16)), new Point(-r / 5, r), new Size(16, 16)));
                Log.WriteToLog($"Аптечка {i} создана.");
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
                a.Draw();
            foreach (HealthPack h in _healthpacks)
                h.Draw();
            foreach (Bullet bullet in _bullets)
                bullet.Draw();
            _ship?.Draw();
            Buffer.Graphics.DrawString("Energy:" + _ship.Energy + "\nScore:" + _score, SystemFonts.DefaultFont, Brushes.White, 0, 0);  
            Buffer.Render();
        }
        /// <summary>
        /// Обновление положения игровых объектов, отслеживание столкновений.
        /// </summary>
        public static void Update()
        {
            foreach (BaseObject obj in _objs) obj.Update();
            if (_bullets.Count > 0)
            {
                for (int i = 0; (i < _bullets.Count) && (i >= 0); i++)
                {
                    _bullets[i].Update();
                    if (_bullets[i].OffScreen)
                    {
                        _bullets.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (_asteroids.Count > 0)
            {
                for (var i = 0; (i < _asteroids.Count) && (i>=0); i++)
                {
                    _asteroids[i].Update();

                    for (int j = 0; (j < _bullets.Count) && (j >= 0) && (i >= 0); j++)
                        if (_bullets[j].Collision(_asteroids[i]))
                        {
                            System.Media.SystemSounds.Hand.Play();
                            _asteroids.RemoveAt(i);
                            _bullets.RemoveAt(j);
                            _score += ASTEROIDSCORE;
                            Log.WriteToLog($"Астероид {i} уничтожен.");
                            j--;
                            i--;
                        }
                    if ( (i < 0) || !_ship.Collision(_asteroids[i])) continue;
                    _ship?.EnergyLow(rnd.Next(1, 10));
                    System.Media.SystemSounds.Asterisk.Play();
                    Log.WriteToLog($"Корабль столкнулся с астероидом {i}.");
                    if (_ship?.Energy <= 0) _ship?.Die();
                }
            }
            else
            {
                LoadAsteroids(++AsteroidsCount);
                LoadHealthPacks();
            }

            if (_healthpacks.Count > 0)
            {
                for (var i = 0; (i < _healthpacks.Count) && (i >= 0); i++)
                {
                    _healthpacks[i].Update();
                    if (!_ship.Collision(_healthpacks[i])) continue;
                    _ship.EnergyLow(-rnd.Next(1, 10));
                    _healthpacks[i].Dispose();
                    _healthpacks.RemoveAt(i);
                    System.Media.SystemSounds.Asterisk.Play();
                    Log.WriteToLog($"Аптечка {i} собрана.");
                    i--;
                }
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
                if (_bullets.Count < MAXBULLETSCOUNT)
                {
                    _bullets.Add(new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4), new Point(4, 0), new Size(4, 1)));
                    Log.WriteToLog($"Произошел выстрел.");
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
            Log.WriteToLog($"Игра закончена.");
        }
    }
}
