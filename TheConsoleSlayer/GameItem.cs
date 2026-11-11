using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace TheConsoleSlayer
{
    enum ItemType { Ammo, BFGCell, Door, LevelExit, Medikit, ToxicWaste, Wall }
    internal class GameItem
    {
        public Position Position { get; set; }
        public ConsoleSprite Sprite { get; set; }
        public ItemType Type { get; set; }
        public double FillingRatio { get; set; }
        public bool Available { get; set; }
        public GameItem(int x, int y, ItemType type)
        {
            Position = new Position(x, y);
            Type = type;
            Available = true;
            SetInitialProperties();
        }
        private void SetInitialProperties()
        {
            switch (Type)
            {
                case ItemType.Ammo:
                    Sprite = new ConsoleSprite(ConsoleColor.Red, ConsoleColor.Yellow, 'A'); break;               
                case ItemType.BFGCell:
                    Sprite = new ConsoleSprite(ConsoleColor.Green, ConsoleColor.White, 'B'); break;
                case ItemType.Door:
                    Sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Yellow, '/'); FillingRatio = 1.0; break;
                case ItemType.LevelExit:
                    Sprite = new ConsoleSprite(ConsoleColor.Blue, ConsoleColor.Black, 'E'); FillingRatio = 1.0; break;
                case ItemType.Medikit:
                    Sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Red, '+'); break;
                case ItemType.ToxicWaste:
                    Sprite = new ConsoleSprite(ConsoleColor.DarkGreen, ConsoleColor.Green, ':'); break;
                case ItemType.Wall:
                    Sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Gray, ' '); FillingRatio = 1.0; break;
                default:
                    break;
            }
        }
        public void Interact()
        {
            if (Type == ItemType.Ammo || Type == ItemType.BFGCell || Type == ItemType.Medikit)
            {
                Available = false;
            }
            if (Type == ItemType.Door)
            {
                if (FillingRatio == 1.0)
                {
                    FillingRatio = 0.0;
                    Sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.DarkYellow, '/');
                }
                else
                {
                    FillingRatio = 1.0;
                    Sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Yellow, '/');
                }
                
                
            }
        }
    }
}
