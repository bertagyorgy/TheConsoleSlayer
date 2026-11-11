using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WMPLib;

namespace TheConsoleSlayer
{
    enum SoundEffectType { BFG, Door, ItemPickup, Pain, PlayerDeath, Shotgun }
    
    internal class Game
    {
        public WindowsMediaPlayer MusicPlayer {  get; set; }
        public WindowsMediaPlayer SfxPlayer { get; set; }

        public static List<GameItem> activeItems = new List<GameItem>();
        public static List<Demon> activeDemons = new List<Demon>();
        public Player Player { get; set; }
        public List<GameItem> Items { get; set; }
        public List<Demon> Demons { get; set; }
        ConsoleRenderer ConsoleRenderer { get; set; }
        GameLogic GameLogic { get; set; }
        public bool Exited;
        private bool _isRunning;
        public bool IsRunning 
        { 
            get { return _isRunning; }
            set
            {
                if((bool)(_isRunning && Exited) == value) return;
            }
        }
        public Game()
        {
            MusicPlayer = new WindowsMediaPlayer();
            SfxPlayer = new WindowsMediaPlayer();
            //Player = new Player(0, 0);
            Items = new List<GameItem>();
            Demons = new List<Demon>();
            ConsoleRenderer = new ConsoleRenderer(this);
            GameLogic = new GameLogic(this);
            LoadMapFromPlainText("level.txt");
        }
        Stopwatch swRender = new Stopwatch();
        Stopwatch swLogic = new Stopwatch();

        public void PlaySoundEffect(SoundEffectType type)
        {
            switch (type)
            {
                case SoundEffectType.BFG:
                    SfxPlayer.URL = "sounds/bfg.mp3";
                    break;
                case SoundEffectType.Door:
                    SfxPlayer.URL = "sounds/door.mp3";
                    break;
                case SoundEffectType.ItemPickup:
                    SfxPlayer.URL = "sounds/item_pickup.mp3";
                    break;
                case SoundEffectType.Pain:
                    SfxPlayer.URL = "sounds/pain.mp3";
                    break;
                case SoundEffectType.PlayerDeath:
                    SfxPlayer.URL = "sounds/player_death.mp3";
                    break;
                case SoundEffectType.Shotgun:
                    SfxPlayer.URL = "sounds/shotgun.mp3";
                    break;
            }
            SfxPlayer.controls.play();
        }
        public void PlayMusic(string filePath)
        {
            MusicPlayer.URL = filePath;
            MusicPlayer.controls.play();
        }
        private void UserAction()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressed = Console.ReadKey(true);
                Position newPosition = Player.Position;
                switch (pressed.Key)
                {              
                    case ConsoleKey.Escape:
                        Exited = true;
                        Console.ResetColor();
                        break;
                    case ConsoleKey.LeftArrow:
                        newPosition = Position.Add(Player.Position, new Position(0, -1));
                        GameLogic.Move(Player, newPosition);
                        break;
                    case ConsoleKey.RightArrow:
                        newPosition = Position.Add(Player.Position, new Position(0, 1));
                        GameLogic.Move(Player, newPosition);
                        break;
                    case ConsoleKey.UpArrow:
                        newPosition = Position.Add(Player.Position, new Position(-1, 0));
                        GameLogic.Move(Player, newPosition);
                        break;
                    case ConsoleKey.DownArrow:
                        newPosition = Position.Add(Player.Position, new Position(1, 0));
                        GameLogic.Move(Player, newPosition);
                        break;
                    case ConsoleKey.D:
                        GameLogic.PlayerDirectInteractionLogic();
                        break;
                    case ConsoleKey.A:
                        GameLogic.PlayerAttackLogic();
                        break;
                    case ConsoleKey.S:
                        GameLogic.PlayerBFGAttackLogic();
                        break;

                }         
            }
        }
        public void LoadMapFromPlainText(string plainText)
        {
            using (StreamReader sr = new StreamReader(plainText))
            {
                string[] dimenziok = sr.ReadLine().Split(',');
                int sorokSzama = int.Parse(dimenziok[0]);
                int oszlopokSzama = int.Parse(dimenziok[1]);
                int rowIndex = 0;

                while (!sr.EndOfStream && rowIndex < sorokSzama)
                {
                    string line = sr.ReadLine();
                    for (int i = 0; i < line.Length; i++)
                    {
                        char symbol = line[i];
                        switch (symbol)
                        {
                            case 'A':
                                Items.Add(new GameItem(rowIndex, i, ItemType.Ammo));
                                break;
                            case 'B':
                                Items.Add(new GameItem(rowIndex, i, ItemType.BFGCell));
                                break;
                            case 'D':
                                Items.Add(new GameItem(rowIndex, i, ItemType.Door));
                                break;
                            case 'E':
                                Items.Add(new GameItem(rowIndex, i, ItemType.LevelExit));
                                break;
                            case 'M':
                                Items.Add(new GameItem(rowIndex, i, ItemType.Medikit));
                                break;
                            case 'T':
                                Items.Add(new GameItem(rowIndex, i, ItemType.ToxicWaste));
                                break;
                            case 'W':
                                Items.Add(new GameItem(rowIndex, i, ItemType.Wall));
                                break;
                            case 'z':
                                Demons.Add(new Demon(rowIndex, i, DemonType.Zombieman));
                                break;
                            case 'i':
                                Demons.Add(new Demon(rowIndex, i, DemonType.Imp));
                                break;
                            case 'm':
                                Demons.Add(new Demon(rowIndex, i, DemonType.Mancubus));
                                break;
                            case 'p':
                                Player = new Player(rowIndex, i);
                                break;
                            case '_':
                                break;
                            default:
                                break;
                        }
                    }
                    rowIndex++;
                }
            }
        }


        public void Run()
        {
            PlayMusic("sounds/doom_music.mp3");
            while (!Exited && Player.Alive)
            {
                swRender.Start();
                swLogic.Start();
                if (swLogic.ElapsedMilliseconds > 500)
                {
                    GameLogic.UpdateGameState(swLogic.ElapsedMilliseconds);
                    swLogic.Restart();
                }
                if (swRender.ElapsedMilliseconds > 25)
                {
                    ConsoleRenderer.RenderGame();
                    swRender.Restart();
                }
                UserAction();
                Thread.Sleep(25);
            }
            if (Exited)
            {
                PlayMusic("sounds/level_complete.mp3");
                Console.SetCursorPosition(0, 17);
                Console.WriteLine("YOU WIN!");
            }
            else if (!Player.Alive)
            {
                PlaySoundEffect(SoundEffectType.PlayerDeath);
                Console.SetCursorPosition(0, 17);
                Console.WriteLine("YOU LOSE!");
            }
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
