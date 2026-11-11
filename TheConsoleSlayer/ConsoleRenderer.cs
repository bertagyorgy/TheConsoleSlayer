using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheConsoleSlayer
{
    internal class ConsoleRenderer
    {
        public Game Game { get; set; }
        public ConsoleRenderer(Game g)
        {
            Game = g;
        }
        private void RenderSingleSprite(Position ps, ConsoleSprite cs)
        {
            if (ps.X >= 0 && ps.Y >= 0 && ps.X < Console.WindowWidth && ps.Y < Console.WindowHeight)
            {
                Console.SetCursorPosition(ps.Y, ps.X);
                Console.BackgroundColor = cs.Background;
                Console.ForegroundColor = cs.Foreground;
                Console.WriteLine(cs.Glyph);

            }
        }
        private void RenderHUD()
        {
            Console.SetCursorPosition(0, 15);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"|HP: {Game.Player.Health} | AMMO: {Game.Player.Ammo}" +
                $" | BFG: {Game.Player.BFGCells} | XP: {Game.Player.CombatPoints} |");
            Console.ResetColor();
        }
        public void RenderGame()
        {
            Console.CursorVisible = false;
            Console.ResetColor();
            Console.Clear();
            for (int i = 0; i < Game.Items.Count; i++)
            {
                if (Position.Distance(Game.Player.Position, Game.Items[i].Position) <= Game.Player.SightRange)
                {
                    RenderSingleSprite(Game.Items[i].Position, Game.Items[i].Sprite);
                }               
            }
            for (int i = 0; i < Game.Demons.Count; i++)
            {
                if (Position.Distance(Game.Player.Position, Game.Demons[i].Position) <= Game.Player.SightRange)
                {
                    RenderSingleSprite(Game.Demons[i].Position, Game.Demons[i].Sprite);
                }
            }
            RenderSingleSprite(Game.Player.Position, Game.Player.Sprite);
            RenderHUD();
        }
    }
}
