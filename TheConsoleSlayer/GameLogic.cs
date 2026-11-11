using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheConsoleSlayer
{
    internal class GameLogic
    {
        Game Game { get; set; }
        public GameLogic(Game g)
        {
            Game = g;
        }
        private void CleanUpGameItems()
        {
            List<GameItem> trash = Game.Items.Where(x => !x.Available).ToList();
            foreach (GameItem item in trash)
            {
                if (Game.Items.Contains(item))
                {
                    Game.Items.Remove(item);
                }
            }
        }
        private void CleanUpDemons()
        {
            List<Demon> dead = Game.Demons.Where(x => !x.Alive).ToList();
            foreach (Demon item in dead)
            {
                if (Game.Demons.Contains(item))
                {
                    Game.Demons.Remove(item);
                }
            }
        }
        private void DemonMoveLogic(Demon dem, long timePassed)
        {
            Random RNG = new Random();
            int x = dem.Position.X + RNG.Next(-1, 2);
            int y = dem.Position.Y + RNG.Next(-1, 2);
            Position targetPosition = new Position(x, y);
            Move(dem, targetPosition, timePassed);
        }
        private void DemonAttackLogic(Demon demon, long timePassed)
        {
            Random rnd = new Random();
            if (demon.Type == DemonType.Zombieman)
            {
                int randomDamage = rnd.Next(3, 15);
                double fullDamage = 2 * randomDamage / (1 + Position.Distance(Game.Player.Position, demon.Position)) * (timePassed / 1000.0);
                Game.PlaySoundEffect(SoundEffectType.Pain);
                Game.Player.TakeDamage((int)fullDamage);
            }
            else if (demon.Type == DemonType.Imp)
            {
                int randomDamage = rnd.Next(3, 24);
                double fullDamage = 2 * randomDamage / (1 + Position.Distance(Game.Player.Position, demon.Position)) * (timePassed / 1000.0);
                Game.PlaySoundEffect(SoundEffectType.Pain);
                Game.Player.TakeDamage((int)fullDamage);
            }
            else
            {
                int randomDamage = rnd.Next(8, 64);
                double fullDamage = 2 * randomDamage / (1 + Position.Distance(Game.Player.Position, demon.Position)) * (timePassed / 1000.0);
                Game.PlaySoundEffect(SoundEffectType.Pain);
                Game.Player.TakeDamage((int)fullDamage);
            }
        }
        private void GetGameItemsWithinDistance(Position pos, double tav)
        {
            Game.activeItems.Clear();
            foreach (GameItem item in  Game.Items)
            {
                if (Position.Distance(pos, item.Position) <= tav)
                {
                    Game.activeItems.Add(item);
                }
            }
        }
        private void GetDemonsWithinDistance(Position pos, double tav)
        {
            Game.activeDemons.Clear();
            foreach (Demon item in Game.Demons)
            {
                if (Position.Distance(pos, item.Position) <= tav)
                {
                    Game.activeDemons.Add(item);
                }
            }
        }
        private double GetTotalFillingRatio(Position pos)
        {
            double ossz = 0;
            GetGameItemsWithinDistance(pos, 0);
            foreach (GameItem item in Game.activeItems)
            {
                ossz += item.FillingRatio;
            }
            GetDemonsWithinDistance(pos, 0);
            foreach (Demon item in Game.activeDemons)
            {
                ossz += item.FillingRatio;
            }
            return ossz;
        }
        public void Move(Player player, Position pos)
        {
            if (GetTotalFillingRatio(pos) + player.FillingRatio <= 1.0)
            {
                player.Position = pos;
            }
        }
        public void Move(Demon dem, Position pos, long timePassed)
        {
            double pMove = (dem.Speed / 100.0) * (timePassed / 1000.0);
            Random rnd = new Random();
            double randomValue = rnd.NextDouble();
            if (randomValue < pMove)
            {
                if (GetTotalFillingRatio(pos) + dem.FillingRatio <= 1.0)
                {
                    dem.Position = pos;
                }
            }
        }
        public void PlayerAttackLogic()
        {
            if (Game.Player.Ammo > 0)
            {
                Game.Player.Shoot();
                Game.PlaySoundEffect(SoundEffectType.Shotgun);
                GetDemonsWithinDistance(Game.Player.Position, Game.Player.SightRange);
                Random rnd = new Random();
                int randomDamage = rnd.Next(35, 105);
                foreach (Demon item in Game.activeDemons)
                {
                    double fullDamage = 2 * randomDamage / (1 + Position.Distance(Game.Player.Position, item.Position));
                    item.TakeDamage((int)fullDamage);
                    if (item.Alive == false)
                    {
                        if (item.Type == DemonType.Zombieman)
                        {
                            Game.Player.AddCombatPoints(1);
                        }
                        else if (item.Type == DemonType.Imp)
                        {
                            Game.Player.AddCombatPoints(3);
                        }
                        else
                        {
                            Game.Player.AddCombatPoints(10);
                        }

                    }
                }

            }
        }
        public void PlayerBFGAttackLogic()
        {
            if (Game.Player.BFGCells > 0)
            {
                Game.Player.ShootBFG();
                Game.PlaySoundEffect(SoundEffectType.BFG);
                GetDemonsWithinDistance(Game.Player.Position, Game.Player.SightRange);
                Random rnd = new Random();
                int randomDamage = rnd.Next(100, 800);
                foreach (Demon item in Game.activeDemons)
                {
                    double fullDamage = 2 * randomDamage / (1 + Position.Distance(Game.Player.Position, item.Position));
                    item.TakeDamage((int)fullDamage);
                    if (item.Alive == false)
                    {
                        if (item.Type == DemonType.Zombieman)
                        {
                            Game.Player.AddCombatPoints(1);
                        }
                        else if (item.Type == DemonType.Imp)
                        {
                            Game.Player.AddCombatPoints(1);
                        }
                        else
                        {
                            Game.Player.AddCombatPoints(1);
                        }

                    }
                }
            }
        }
        private void UpdateDemons(long timePassed)
        {
            foreach (Demon item in Game.Demons)
            {
                item.UpdateState(Game.Player);
                if (item.State == DemonStateType.Move)
                {
                    DemonMoveLogic(item, timePassed);
                }
                else if (item.State == DemonStateType.Attack)
                {
                    DemonAttackLogic(item, timePassed);
                }
                DemonIndirectInteractionLogic(item);
            }
        }
        public void PlayerDirectInteractionLogic()
        {
            GetGameItemsWithinDistance(Game.Player.Position, 1);
            foreach (GameItem item in Game.activeItems)
            {
                if (item.Type == ItemType.Door)
                {
                    item.Interact();
                    Game.PlaySoundEffect(SoundEffectType.Door);
                }
                else if (item.Type == ItemType.LevelExit)
                {
                    item.Interact();
                    Game.Exited = true;
                }
            }
        }
        public void PlayerIndirectInteractionLogic()
        {
            GetGameItemsWithinDistance(Game.Player.Position, 0);
            foreach (GameItem item in Game.activeItems)
            {
                if (item.Type == ItemType.Ammo)
                {
                    item.Interact();
                    Game.PlaySoundEffect(SoundEffectType.ItemPickup);
                    Game.Player.PickUpAmmo(5);
                }
                if (item.Type == ItemType.BFGCell)
                {
                    item.Interact();
                    Game.PlaySoundEffect(SoundEffectType.ItemPickup);
                    Game.Player.PickUpBFGCell(1);
                }
                else if (item.Type == ItemType.Medikit)
                {
                    item.Interact();
                    Game.PlaySoundEffect(SoundEffectType.ItemPickup);
                    Game.Player.PickUpHealth(25);
                }
                else if (item.Type == ItemType.ToxicWaste)
                {
                    Game.PlaySoundEffect(SoundEffectType.Pain);
                    Game.Player.TakeDamage(5);
                }
            }
        }
        private void DemonIndirectInteractionLogic(Demon dem)
        {
            GetGameItemsWithinDistance(dem.Position, 0);
            foreach (GameItem item in Game.activeItems)
            {
                if (item.Type == ItemType.ToxicWaste)
                {
                    dem.TakeDamage(5);
                }
            }
        }
        public void UpdateGameState(long timePassed)
        {
            UpdateDemons(timePassed);
            CleanUpGameItems();
            CleanUpDemons();
            PlayerIndirectInteractionLogic();
        }
    }
}
