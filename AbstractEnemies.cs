using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace goodbyeworld
{
    public abstract class Enemy : LivingEntity
    {
        protected Dungeon dungeon;
        protected bool damageTileImmunity = false;

        protected bool move;
        protected int dmg;
        protected int minCoin;
        protected int maxCoin;
        protected int keyChance;

        public Enemy() : base() { }

        public Enemy(int xin, int yin, int hpin, int dmgin,Dungeon dungeonIn) : base(xin, yin, hpin)
        {
            dungeon = dungeonIn;
            hp = hpin;
            dmg = dmgin;
            keyChance=1;
            minCoin=1;
            maxCoin=1;
        }
        public abstract void Move();
        public bool HasDamageTileImmunity()
        {
            return damageTileImmunity;
        }




        public virtual bool IsMoveValid(int xpos, int ypos)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            //if the tile theyre moving to is outside the bounds of the array, return false
            if (xpos < 0 || xpos >= width || ypos < 0 || ypos >= height)
            {
                return false;
            }
            //if the tile is null, return false 
            else if (tileLayer[ypos, xpos] == null)
            {
                return false;
            }
            //if the tile isnt null, and the tile has collision, return false
            else if (tileLayer[ypos, xpos].DoesCollide())
            {
                return false;
            }
            //if there is an entity on the tile theyre moving to, return false
            else if (entityLayer[ypos, xpos] != null)
            {
                return false;
            }
            return true;
        }
        public virtual bool IsAttackValid(int x, int y)
        {
            int dunHeight = dungeon.GetDunHeight();
            int dunWidth = dungeon.GetDunWidth();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Player player = dungeon.GetPlayer();
            //if the tile is out of bounds, return false
            if (x >= dunWidth)
            {
                return false;
            }
            else if (y >= dunHeight)
            {
                return false;
            }
            else if (x < 0)
            {
                return false;
            }
            else if (y < 0)
            {
                return false;
            }
            //if the tile is null, return false
            else if (tileLayer[y, x] == null)
            {
                return false;
            }
            //if the tile has collision, return false
            else if (tileLayer[y, x].DoesCollide())
            {
                return false;
            }
            //if there is an entity and its not a player, return false
            else if (entityLayer[y, x] != null && entityLayer[y, x] != player)
            {
                return false;
            }
            return true;
        }
        public virtual void EnemyAttacked(int dmg)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            List<Item>[,] itemLayer = dungeon.GetItemLayer();
            List<Enemy> enemies = dungeon.GetEnemies();
            hp -= dmg;

            if (!IsAlive())
            {
                DropCoins( );
                DropKey();
                //remove the enemy from the enemies list and remove them from the entity layer
                enemies.Remove(this);
                entityLayer[y, x] = null;
            }
        }
        public void DropCoins()
        {
            List<Item>[,] itemLayer = dungeon.GetItemLayer();
            if (itemLayer[y, x] == null)
            {
                itemLayer[y, x] = new List<Item>();

            }
            itemLayer[y, x].Add(new Coin(rand.Next(minCoin, maxCoin + 1)));
        }
        public void DropKey( )
        {
            List<Item>[,] itemLayer = dungeon.GetItemLayer();
            if (rand.Next(0, 100) < keyChance)
            {
                if (itemLayer[y, x] == null)
                {
                    itemLayer[y, x] = new List<Item>();

                }
                itemLayer[y, x].Add(new Key(1));
            }
            
        }
        public int Attack()
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Player player = dungeon.GetPlayer();
            player.ChangeHP(player.GetHP()-dmg,  entityLayer);
            return dmg;
        }

    }
    
    public abstract class StraightRangedEnemy : Enemy
    {
        protected bool rangeAttackChargeUp = false;
        public StraightRangedEnemy(int xin, int yin, int hpin, int dmgin,Dungeon dungeonIn) : base(xin, yin, hpin, dmgin,dungeonIn)
        {
        }
        public bool IsStraightRangedAttackValid()
        {
            Tile[,] tileLayer = dungeon.GetTileLayer();
            Player player = dungeon.GetPlayer();
            if (player.x == x&&player.y!=y-1&&player.y!=y+1)
            {
                if (player.y > y)
                {
                    for (int i = y + 1; i <= player.y; i++)
                    {
                        if (!IsAttackValid(x,i))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    for (int i = y - 1; i >= player.y; i--)
                    {
                        if (!IsAttackValid(x, i))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            else if (player.y == y&&player.x!=x-1&&player.x!=x+1)
            {
                if (player.x > x)
                {
                    for (int i = x + 1; i <= player.x; i++)
                    {
                        if (!IsAttackValid(i, y))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    for (int i = x - 1; i >= player.x; i--)
                    {
                        if (!IsAttackValid(i, y))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        public void StraightRangedAttack()
        {
            Player player = dungeon.GetPlayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
            if (IsStraightRangedAttackValid())
            {
                if (rangeAttackChargeUp)
                {
                    player.ChangeHP(player.GetHP() - dmg, entityLayer);
                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                    rangeAttackChargeUp = false;
                }
                else
                {
                    rangeAttackChargeUp = true;
                    if (player.x == x)
                    {
                        if (player.y > y)
                        {
                            for (int i = y + 1; i <= player.y - 1; i++)
                            {                               
                                effectsLayer[i, x] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                            }
                        }
                        else
                        {
                            for (int i = y - 1; i >= player.y + 1; i--)
                            {
                                effectsLayer[i, x] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                            }
                        }
                    }
                    else if (player.y == y)
                    {
                        if (player.x > x)
                        {
                            for (int i = x + 1; i <= player.x - 1; i++)
                            {
                                effectsLayer[y, i] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                            }
                        }
                        else
                        {
                            for (int i = x - 1; i >= player.x + 1; i--)
                            {
                                effectsLayer[y, i] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                            }
                        }
                    }

                }
            }
            else
            {
                rangeAttackChargeUp = false;
            }
        }

    }
    public abstract class UniformAOEEnemy : Enemy
    {
        protected bool UniformAOEAttackChargeUp = false;
        public UniformAOEEnemy(int xin, int yin, int hpin, int dmgin,Dungeon dungeonIn) : base(xin, yin, hpin, dmgin,dungeonIn)
        {
        }
        public void UniformAOEAttack()
        {
            Player player = dungeon.GetPlayer();
            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            if (UniformAOEAttackChargeUp)
            {
                if (IsUniformAOEAttackValid())
                {
                    player.ChangeHP(player.GetHP() - dmg, entityLayer);
                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                }
                UniformAOEAttackChargeUp = false;
            }
            else
            {
                UniformAOEAttackChargeUp = true;
                if (IsAttackValid(x + 1, y) && (player.x != x + 1 || player.y != y))
                {
                    effectsLayer[y, x+1] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                }
                if (IsAttackValid(x - 1, y) && (player.x != x - 1 || player.y != y))
                {
                    effectsLayer[y, x-1] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                }
                 if (IsAttackValid(x - 1, y + 1) && (player.x != x - 1 || player.y != y + 1))
                {
                    effectsLayer[y+1, x-1] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                }
                 if (IsAttackValid(x + 1, y + 1) && (player.x != x + 1 || player.y != y + 1))
                {
                    effectsLayer[y+1, x+1] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                }
                 if (IsAttackValid(x - 1, y - 1) && (player.x != x - 1 || player.y != y - 1))
                {
                    effectsLayer[y-1, x-1] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                }
                 if (IsAttackValid(x + 1, y - 1) && (player.x != x + 1 || player.y != y - 1))
                {
                    effectsLayer[y-1, x+1] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                }
                 if (IsAttackValid(x, y - 1) && (player.x != x || player.y != y - 1))
                {
                    effectsLayer[y-1, x] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                }
                 if (IsAttackValid(x, y+1) && (player.x != x||player.y!=y+1))
                {
                    effectsLayer[y+1, x] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                }
            }
        }
        public bool IsUniformAOEAttackValid()
        {
            Player player = dungeon.GetPlayer();
            if (x + 1 == player.x && y == player.y)
            {
                return true;
            }
            else if (x - 1 == player.x && y == player.y)
            {
                return true;
            }
            else if (x + 1 == player.x && y + 1 == player.y)
            {
                return true;
            }
            else if (x - 1 == player.x && y + 1 == player.y)
            {
                return true;
            }
            else if (x + 1 == player.x && y - 1 == player.y)
            {
                return true;
            }
            else if (x - 1 == player.x && y - 1 == player.y)
            {
                return true;
            }
            else if (x == player.x && y + 1 == player.y)
            {
                return true;
            }
            else if (x == player.x && y - 1 == player.y)
            {
                return true;
            }
            return false;
        }

    }
}
