using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheConsoleSlayer
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //Position osztály teszt
            /*Position p = new Position(4, 6);
            Position v = new Position(2, 3);   
            
            Console.WriteLine(Position.Distance(p, v));
            Position e = Position.Add(p, v);
            Console.WriteLine($"{e.X} {e.Y}");

            //ConsoleSprite osztály teszt
            ConsoleSprite c = new ConsoleSprite(ConsoleColor.Green, ConsoleColor.Red, '#');
            Console.BackgroundColor = c.Background;
            Console.ForegroundColor = c.Foreground;
            Console.WriteLine(c.Glyph);
            Console.ResetColor();

            //Player osztály teszt
            Player player = new Player(3, 4);
            Console.BackgroundColor = player.Sprite.Background;
            Console.ForegroundColor = player.Sprite.Foreground;
            Console.WriteLine(player.Sprite.Glyph);
            Console.ResetColor();*/
            
            
            //GameItem és Demon osztály teszt
            /*GameItem a = new GameItem(4, 6,ItemType.Door);
            GameItem b = new GameItem(4, 4, ItemType.Ammo);
            GameItem c = new GameItem(2, 3, ItemType.Medikit);
            GameItem d = new GameItem(7, 1, ItemType.Wall);
            GameItem t = new GameItem(2, 2, ItemType.ToxicWaste);
            GameItem e = new GameItem(2, 1, ItemType.LevelExit);

            Demon z = new Demon(8, 7, DemonType.Zombieman);
            //Demon i = new Demon(5, 2, DemonType.Imp);
            //Demon m = new Demon(6, 8, DemonType.Mancubus);
            List<GameItem> lista = new List<GameItem> { a, b, c, d, t, e};
            List<Demon> lista2 = new List<Demon> {z};*/
            Game g = new Game();
            g.Run();
            
        }
    }
}
