using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheConsoleSlayer
{
    enum DemonType { Zombieman, Imp, Mancubus }
    enum DemonStateType { Idle, Move, Attack }
    internal class Demon
    {
        public Position Position { get; set; }
        public ConsoleSprite Sprite { get; set; }
        public DemonType Type { get; set; }
        public DemonStateType State { get; set; }
        public double FillingRatio { get; set; }
        public bool Alive { get; set; }
        public int SightRange { get; set; }
        public int AttackRange { get; set; }
        public int Speed {  get; set; }
        private int _health;
        public int Health
        {
            get { return _health; }
            set
            {
                _health = Math.Max(0, value);
            }
        }
        public Demon(int x, int y, DemonType type)
        {
            Position = new Position(x, y);
            Type = type;
            Alive = true;
            State = DemonStateType.Idle;
            SetInitialProperties();
        }
        private void SetInitialProperties()
        {
            switch (Type)
            {
                case DemonType.Zombieman:
                    Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.White, 'o');
                    FillingRatio = 0.4;
                    Health = 20;
                    Speed = 70;
                    SightRange = 3;
                    AttackRange = 1;
                    break;
                case DemonType.Imp:
                    Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.Red, 'o');
                    FillingRatio = 0.4;
                    Health = 60;
                    Speed = 93;
                    SightRange = 6;
                    AttackRange = 3;
                    break;
                case DemonType.Mancubus:
                    Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.DarkMagenta, 'O');
                    FillingRatio = 0.9;
                    Health = 600;
                    Speed = 70;
                    SightRange = 9;
                    AttackRange = 6;
                    break;
                default:
                    break;
            }
        }
        public void UpdateState(Player player)
        {
            if (Position.Distance(this.Position, player.Position) < this.AttackRange)
            {
                this.State = DemonStateType.Attack;
            }
            else if (Position.Distance(this.Position, player.Position) < this.SightRange)
            {
                this.State = DemonStateType.Move;
            }
            else
            {
                this.State = DemonStateType.Idle;
            }
        }
        public void TakeDamage(int ertek)
        {
            this.Health -= ertek;
            if (this.Health <= 0)
            {
                this.Alive = false;
            }
        }
    }
}
