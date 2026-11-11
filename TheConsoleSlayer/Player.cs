using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheConsoleSlayer
{
    internal class Player
    {
        public Position Position { get; set; }
        public ConsoleSprite Sprite { get; set; }
        public double FillingRatio { get; set; }   
        public bool Alive { get; set; }
        public int CombatPoints { get; set; }
        public int SightRange { get; set; }
        private int _bFGCells;
        public int BFGCells
        {
            get { return _bFGCells; }
            set
            {
                _bFGCells = Math.Max(0, Math.Min(value, (int)MaxBFGCells));
            }
        }
        public int MaxBFGCells { get { return 3; } }
        private int _health;
        public int Health
        {
            get {  return _health; }
            set
            {              
                _health = Math.Max(0, Math.Min(value, (int)MaxHealth));        
            }
        }
        private int _ammo;
        public int Ammo {
            get { return _ammo; }
            set
            {              
                _ammo = Math.Max(0, Math.Min(value, (int)MaxAmmo));            
            }
        }
        public double MaxHealth
        {
            get { return 100 + (CombatPoints / 10); }
            //get { return 100 + (CombatPoints); } //V2
        }
        public double MaxAmmo
        {
            get { return 10 + (CombatPoints / 50); }
            //get { return 10 + (CombatPoints); } //V2
        }
        public Player(int x, int y)
        {
            Position = new Position(x, y);
            Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.Green, 'O');
            FillingRatio = 0.4;
            Health = 100;
            SightRange = 8;
            Alive = true;
            Ammo = 10;
        }
        public void Shoot()
        {
            this.Ammo--;
        }
        public void AddCombatPoints(int ertek)
        {
            this.CombatPoints += ertek;
        }
        public void TakeDamage(int ertek)
        {
            this.Health -= ertek;
            if (this.Health <= 0)
            {
                this.Alive = false;
            }
        }
        public void PickUpAmmo(int ertek)
        {
            this.Ammo += ertek;
        }
        public void PickUpHealth(int ertek)
        {
            this.Health += ertek;
        }
        public void PickUpBFGCell(int ertek)
        {
            if (this.BFGCells + ertek <= this.MaxBFGCells)
            {
                this.BFGCells += ertek;
            }
        }
        public void ShootBFG()
        {
            this.BFGCells--;
        }
    }
}
