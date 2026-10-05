using System.Numerics;
using System.Runtime.CompilerServices;

namespace goodbyeworld
{
    public abstract class Weapon:Item{
        protected int dmg;
        public Weapon() { }
        public Weapon(int dmgIn)
        {
            dmg = dmgIn;
            itemName = "Weapon";
        }
        public bool IsValid(int height, int width, int x, int y)
        {
            if (x < 0 || x > width - 1 || y < 0 || y > height - 1)
            {
                return false;
            }
            return true;
        }
        public abstract void AttackInDirection(Dungeon dungeon, Player player, int x, int y, int xInc, int yInc);
        public abstract void UseWeapon(Dungeon dungeon, Player player, ConsoleKey input);
        public int GetDMG()
        {
            return dmg;
        }
    }
    public class Sword : Weapon
    {
        public Sword() { }
        public Sword(int dmgIn)
        {
            dmg = dmgIn;
            itemName = "Sword";
        }
        public override DisplayCharacter GameDisplay()
        {
            return new DisplayCharacter("^",ConsoleColor.Cyan);
        }
        public override void UseWeapon(Dungeon dungeon, Player player, ConsoleKey input)
        {
            if (input == ConsoleKey.W)
            {
                AttackInDirection(dungeon, player, player.x, player.y, 0, -1);
            }
            else if (input == ConsoleKey.A)
            {
                AttackInDirection(dungeon, player, player.x, player.y, -1, 0);
            }
            else if (input == ConsoleKey.S)
            {
                AttackInDirection(dungeon, player, player.x, player.y, 0, 1);
            }
            else if (input == ConsoleKey.D)
            {

                AttackInDirection(dungeon, player, player.x, player.y, 1, 0);
            }
        }
        public override void AttackInDirection(Dungeon dungeon, Player player, int x, int y, int xInc, int yInc)
        {
            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            
            if (IsValid(height, width, x + xInc, y + yInc))

            {
                if (entityLayer[y + yInc, x + xInc] != null && entityLayer[y + yInc, x + xInc].GetType().IsSubclassOf(typeof(Enemy)))
                {

                    Enemy enemy = (Enemy)entityLayer[y + yInc, x + xInc];
                    enemy.EnemyAttacked(player.GetDMG()+dmg);
                    effectsLayer[y+yInc,x+xInc]=new DisplayCharacter("*",ConsoleColor.DarkBlue);
                    

                }

            }

        }
    }
    public class Longsword : Sword
    {
        public Longsword() { }
        public Longsword(int dmgIn):base(dmgIn)
        {
            itemName = "Longsword";
        }
        public override DisplayCharacter GameDisplay()
        {
            return new DisplayCharacter("|",ConsoleColor.Cyan);
        }
        public override void AttackInDirection(Dungeon dungeon, Player player, int x, int y, int xInc, int yInc)
        {
            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            if (IsValid(height, width, x + xInc, y + yInc))

            {
                if (entityLayer[y + yInc, x + xInc] != null && entityLayer[y + yInc, x + xInc].GetType().IsSubclassOf(typeof(Enemy)))
                {

                    Enemy enemy = (Enemy)entityLayer[y + yInc, x + xInc];
                    enemy.EnemyAttacked(player.GetDMG()+dmg);
                    effectsLayer[y+yInc,x+xInc]=new DisplayCharacter("*",ConsoleColor.DarkBlue);

                }
                else if (entityLayer[y + yInc, x + xInc] == null && tileLayer[y + yInc, x + xInc] != null && !tileLayer[y + yInc, x + xInc].DoesCollide())
                {


                    if (IsValid(height, width, x + (xInc * 2), y + (yInc * 2)))
                    {



                        if (entityLayer[y + (yInc * 2), x + (xInc * 2)] != null && entityLayer[y + (yInc * 2), x + (xInc * 2)].GetType().IsSubclassOf(typeof(Enemy)))
                        {

                            Enemy enemy = (Enemy)entityLayer[y + (yInc * 2), x + (xInc * 2)];
                            enemy.EnemyAttacked(player.GetDMG()+dmg);
                            effectsLayer[y + (yInc * 2), x + (xInc * 2)]=new DisplayCharacter("*",ConsoleColor.DarkBlue);
                        }
                    }
                }

            }

        }
    }
    public class Broadsword : Sword
    {
        public Broadsword(int dmgIn) : base(dmgIn)
        {
            itemName = "Broadsword";
        }
        public override DisplayCharacter GameDisplay()
        {
            return new DisplayCharacter("/",ConsoleColor.Cyan);
        }
        public override void AttackInDirection(Dungeon dungeon, Player player, int x, int y, int xInc, int yInc)
        {
            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            if (IsValid(height, width, x + xInc, y + yInc))

            {
                if (entityLayer[y + yInc, x + xInc] != null && entityLayer[y + yInc, x + xInc].GetType().IsSubclassOf(typeof(Enemy)))
                {

                    Enemy enemy = (Enemy)entityLayer[y + yInc, x + xInc];
                    enemy.EnemyAttacked(player.GetDMG()+dmg);
                    effectsLayer[y+yInc,x+xInc]=new DisplayCharacter("*",ConsoleColor.DarkBlue);
                }
                if (xInc == 0)
                {
                    if (IsValid(height, width, x - 1, y + yInc))
                    {
                        if (entityLayer[y + yInc, x - 1] != null && entityLayer[y + yInc, x - 1].GetType().IsSubclassOf(typeof(Enemy)))
                        {

                            Enemy enemy = (Enemy)entityLayer[y + yInc, x - 1];
                            enemy.EnemyAttacked(player.GetDMG()+dmg);
                            effectsLayer[y+yInc,x-1]=new DisplayCharacter("*",ConsoleColor.DarkBlue);
                        }
                    }
                    if (IsValid(height, width, x + 1, y + yInc))
                    {
                        if (entityLayer[y + yInc, x + 1] != null && entityLayer[y + yInc, x + 1].GetType().IsSubclassOf(typeof(Enemy)))
                        {

                            Enemy enemy = (Enemy)entityLayer[y + yInc, x + 1];
                            enemy.EnemyAttacked(player.GetDMG()+dmg);
                            effectsLayer[y+yInc,x+1]=new DisplayCharacter("*",ConsoleColor.DarkBlue);
                        }
                    }
                }
                else if (yInc == 0)
                {
                    if (IsValid(height, width, x + xInc, y - 1))
                    {
                        if (entityLayer[y - 1, x + xInc] != null && entityLayer[y - 1, x + xInc].GetType().IsSubclassOf(typeof(Enemy)))
                        {

                            Enemy enemy = (Enemy)entityLayer[y - 1, x + xInc];
                            enemy.EnemyAttacked(player.GetDMG()+dmg);
                            effectsLayer[y-1,x+xInc]=new DisplayCharacter("*",ConsoleColor.DarkBlue);
                        }
                    }
                    if (IsValid(height, width, x + xInc, y +1))
                    {
                        if (entityLayer[y +1, x + xInc] != null && entityLayer[y +1, x + xInc].GetType().IsSubclassOf(typeof(Enemy)))
                        {

                            Enemy enemy = (Enemy)entityLayer[y +1, x + xInc];
                            enemy.EnemyAttacked(player.GetDMG()+dmg);
                            effectsLayer[y+1,x+xInc]=new DisplayCharacter("*",ConsoleColor.DarkBlue);
                        }
                    }
                }
            }

        }
    }
    public class Mace : Sword
    {
        public Mace(int dmgIn) : base(dmgIn)
        {
            itemName = "Mace";
        }
        public override DisplayCharacter GameDisplay()
        {
            return new DisplayCharacter("9",ConsoleColor.Cyan);
        }
        public override void AttackInDirection(Dungeon dungeon, Player player, int x, int y, int xInc, int yInc)
        {
            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            if (IsValid(height, width, x + 1, y))

            {
                if (entityLayer[y, x + 1] != null && entityLayer[y, x + 1].GetType().IsSubclassOf(typeof(Enemy)))
                {

                    Enemy enemy = (Enemy)entityLayer[y, x + 1];
                    enemy.EnemyAttacked(player.GetDMG()+dmg);
                    effectsLayer[y,x+1]=new DisplayCharacter("*",ConsoleColor.DarkBlue);
                }
            }
            if (IsValid(height, width, x - 1, y))

            {
                if (entityLayer[y, x - 1] != null && entityLayer[y, x - 1].GetType().IsSubclassOf(typeof(Enemy)))
                {

                    Enemy enemy = (Enemy)entityLayer[y, x - 1];
                    enemy.EnemyAttacked(player.GetDMG()+dmg);
                    effectsLayer[y,x-1]=new DisplayCharacter("*",ConsoleColor.DarkBlue);
                }
            }
            if (IsValid(height, width, x, y + 1))

            {
                if (entityLayer[y + 1, x] != null && entityLayer[y + 1, x].GetType().IsSubclassOf(typeof(Enemy)))
                {

                    Enemy enemy = (Enemy)entityLayer[y + 1, x];
                    enemy.EnemyAttacked(player.GetDMG()+dmg);
                    effectsLayer[y+1,x]=new DisplayCharacter("*",ConsoleColor.DarkBlue);
                }
            }
            if (IsValid(height, width, x, y-1))

            {
                if (entityLayer[y-1, x] != null && entityLayer[y-1, x].GetType().IsSubclassOf(typeof(Enemy)))
                {

                    Enemy enemy = (Enemy)entityLayer[y-1, x];
                    enemy.EnemyAttacked(player.GetDMG()+dmg);
                    effectsLayer[y-1,x]=new DisplayCharacter("*",ConsoleColor.DarkBlue);
                }
            }

        }
    }
    
}