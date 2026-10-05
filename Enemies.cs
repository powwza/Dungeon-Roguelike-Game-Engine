namespace goodbyeworld
{
    public class DummyEnemy : Enemy
    {
        public DummyEnemy(int xin, int yin,int hpin, int dmgin,Dungeon dungeonIn) : base(xin, yin, hpin, dmgin, dungeonIn)
        {
            
        }
        public override void Move()
        {
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("x",ConsoleColor.Red);
        }
    }
    public class HorizontalEnemy : Enemy
    {
        protected bool right;
        public HorizontalEnemy(int xin, int yin, int hpin, int dmgin, Dungeon dungeonIn) : base(xin, yin, hpin, dmgin, dungeonIn)
        {
            if (rand.Next(0, 2) == 0)
            {
                right = true;
                move = false;
            }
            else
            {
                right = false;
                move = true;

            }
            double difficulty = dungeon.GetDifficulty();
            minCoin = Convert.ToInt32(3 * difficulty);
            maxCoin = Convert.ToInt32(6 * difficulty);
            keyChance = 3;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("a",ConsoleColor.Red);
        }
        public override void Move()
        {
            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
            Player player = dungeon.GetPlayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            if (move)
            {
                if (right)
                {
                    if (IsMoveValid(x + 1, y))
                    {
                        entityLayer[y, x] = null;
                        x++;
                        entityLayer[y, x] = this;
                    }
                    else if (entityLayer[y, x + 1] == player)
                    {
                        Attack();
                        effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                    }
                    else
                    {
                        right = false;
                    }
                }
                else
                {
                    if (IsMoveValid(x - 1, y))
                    {
                        entityLayer[y, x] = null;
                        x--;
                        entityLayer[y, x] = this;
                    }
                    else if (entityLayer[y, x - 1] == player)
                    {
                        Attack();
                        effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                    }
                    else
                    {
                        right = true;
                    }
                }
                move = false;
            }
            else
            {
                move = true;
            }
        }

    }
    public class VerticalEnemy : Enemy
    {
        protected bool down;
        public VerticalEnemy(int xin, int yin, int hpin, int dmgin, Dungeon dungeonIn) : base(xin, yin, hpin, dmgin, dungeonIn)
        {
            if (rand.Next(0, 2) == 0)
            {
                down = true;
                move = false;
            }
            else
            {
                down = false;
                move = true;
            }
            double difficulty = dungeon.GetDifficulty();
            minCoin = Convert.ToInt32(3 * difficulty);
            maxCoin = Convert.ToInt32(6 * difficulty);
            keyChance = 3;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("b",ConsoleColor.Red);
        }
        public override void Move()
        {
            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
            Player player = dungeon.GetPlayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            if (move)
            {
                if (down)
                {
                    if (IsMoveValid(x, y + 1))
                    {
                        entityLayer[y, x] = null;
                        y++;
                        entityLayer[y, x] = this;
                    }
                    else if (entityLayer[y + 1, x] == player)
                    {
                        Attack();
                        effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                    }
                    else
                    {
                        down = false;
                    }
                }
                else
                {
                    if (IsMoveValid(x, y - 1))
                    {
                        entityLayer[y, x] = null;
                        y--;
                        entityLayer[y, x] = this;
                    }
                    else if (entityLayer[y - 1, x] == player)
                    {
                        Attack();
                        effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                    }
                    else
                    {
                        down = true;
                    }
                }
                move = false;
            }
            else
            {
                move = true;
            }
        }
    }
    public class RandomEnemy : Enemy
    {
        public RandomEnemy(int xin, int yin, int hpin, int dmgin, Dungeon dungeonIn) : base(xin, yin, hpin, dmgin, dungeonIn)
        {
            if (rand.Next(0, 2) == 0)
            {
                move = false;
            }
            else
            {
                move = true;
            }
            double difficulty = dungeon.GetDifficulty();
            minCoin = Convert.ToInt32(3 * difficulty);
            maxCoin = Convert.ToInt32(6 * difficulty);
            keyChance = 5;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("c",ConsoleColor.Red);
        }
        public override void Move()
        {
            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
            Player player = dungeon.GetPlayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            int[] moves = new int[] { -1, 1 };
            int xpos = 0;
            int ypos = 0;
            if (move)
            {
                if (rand.Next(0, 2) == 0)
                {
                    xpos = x + moves[rand.Next(0, 2)];
                    ypos = y;
                }
                else
                {
                    ypos = y + moves[rand.Next(0, 2)];
                    xpos = x;
                }
                if (IsMoveValid(xpos, ypos))
                {
                    entityLayer[y, x] = null;

                    entityLayer[ypos, xpos] = this;
                    y = ypos;
                    x = xpos;
                    
                }
                else if (entityLayer[ypos, xpos] == player)
                {
                    Attack();
                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                }

                move = false;
            }
            else
            {
                move = true;
            }
        }

    }
    public class TrackingEnemy : Enemy
    {
        public TrackingEnemy(int xin, int yin, int hpin, int dmgin, Dungeon dungeonIn) : base(xin, yin, hpin, dmgin, dungeonIn)
        {
            double difficulty = dungeon.GetDifficulty();
            if (rand.Next(0, 2) == 0)
            {
                move = false;
            }
            else
            {
                move = true;
            }
            minCoin = Convert.ToInt32(8 * difficulty);
            maxCoin = Convert.ToInt32(12 * difficulty);
            keyChance = 7;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("d", ConsoleColor.Red);
        }
        public override void Move()
        {
            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
            Player player = dungeon.GetPlayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Dictionary<Coord, List<Coord>> coordAdjacencyList = dungeon.GetCoordAdjacencyList();
            if (move)
            {
                List<Coord> nextCoords = BreadthFirstSearch(dungeon, new Coord(player.x, player.y), new Coord(x, y));
                if (nextCoords.Count > 0)
                {
                    Coord nextCoord = nextCoords[0];
                    if (IsMoveValid(nextCoord.x, nextCoord.y))
                    {
                        entityLayer[y, x] = null;
                        x = nextCoord.x;
                        y = nextCoord.y;
                        entityLayer[y, x] = this;
                    }
                    else if (entityLayer[nextCoord.y, nextCoord.x] == player)
                    {
                        Attack();
                        effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                    }
                }
                move = false;
            }
            else
            {
                move = true;
            }

        }
    }
    public class StaticStraightRangedEnemy : StraightRangedEnemy
    {
        public StaticStraightRangedEnemy(int xin, int yin, int hpin, int dmgin, Dungeon dungeonIn) : base(xin, yin, hpin, dmgin, dungeonIn)
        {
            double difficulty = dungeon.GetDifficulty();
            if (rand.Next(0, 2) == 0)
            {
                move = false;
            }
            else
            {
                move = true;
            }
            minCoin = Convert.ToInt32(8 * difficulty);
            maxCoin = Convert.ToInt32(12 * difficulty);
            keyChance = 15;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("e",ConsoleColor.Red);
        }
        public override void Move()
        {
            if (rangeAttackChargeUp)
            {
                StraightRangedAttack();
            }
            else
            {
                if (move)
                {
                    if(IsStraightRangedAttackValid())
                    {
                        StraightRangedAttack();
                    }
                    move = false;
                }
                else
                {
                    move = true;
                }
            }
        }
    }
    public class StaticUniformAOEEnemy : UniformAOEEnemy
    {
        public StaticUniformAOEEnemy(int xin, int yin, int hpin, int dmgin, Dungeon dungeonIn) : base(xin, yin, hpin, dmgin, dungeonIn)
        {
            double difficulty = dungeon.GetDifficulty();
            if (rand.Next(0, 2) == 0)
            {
                move = false;
            }
            else
            {
                move = true;
            }
            minCoin = Convert.ToInt32(8 * difficulty);
            maxCoin = Convert.ToInt32(12 * difficulty);
            keyChance = 15;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("f", ConsoleColor.Red);
        }
        public override void Move()
        {
            if (UniformAOEAttackChargeUp)
            {
                UniformAOEAttack();
            }
            else
            {
                if (move)
                {
                    UniformAOEAttack();
                    move = false;
                }
                else
                {
                    move = true;
                }
            }
        }
    }
    public class TrailDamageTileTrackingEnemy : Enemy
    {
        protected int moveCounter = 1;
        protected int moveDelay;
        public TrailDamageTileTrackingEnemy(int xin, int yin, int hpin, int dmgin, Dungeon dungeonIn,int moveDelayIn) : base(xin, yin, hpin, dmgin, dungeonIn)
        {
            double difficulty = dungeon.GetDifficulty();
            if (rand.Next(0, 2) == 0)
            {
                move = false;
            }
            else
            {
                move = true;
            }
            minCoin = Convert.ToInt32(8 * difficulty);
            maxCoin = Convert.ToInt32(12 * difficulty);
            keyChance = 7;
            moveDelay = moveDelayIn;
            damageTileImmunity = true;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("g", ConsoleColor.Red);
        }
        public override void Move()
        {
            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
            moveCounter++;
            Player player = dungeon.GetPlayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            Dictionary<Coord, List<Coord>> coordAdjacencyList = dungeon.GetCoordAdjacencyList();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            if (move&&moveCounter%moveDelay==0)
            {
                moveCounter = 0;
                List<Coord> nextCoords = BreadthFirstSearch(dungeon, new Coord(player.x, player.y), new Coord(x, y));
                if (nextCoords.Count > 0)
                {
                    Coord nextCoord = nextCoords[0];
                    if (IsMoveValid(nextCoord.x, nextCoord.y))
                    {
                        entityLayer[y, x] = null;
                        x = nextCoord.x;
                        y = nextCoord.y;
                        entityLayer[y, x] = this;
                        if(rand.Next(0,2)==0)
                        {
                            tileLayer[y, x] = new DamageTile(1,true);
                        }
                    }
                    else if (entityLayer[nextCoord.y, nextCoord.x] == player)
                    {
                        player.ChangeHP(player.GetHP() - dmg, entityLayer);
                        effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                    }
                }
                move = false;
            }
            else
            {
                move = true;
            }

        }
    }
    public class SmartTrackingEnemy : Enemy
    {
        public SmartTrackingEnemy(int xin, int yin, int hpin, int dmgin, Dungeon dungeonIn) : base(xin, yin, hpin, dmgin, dungeonIn)
        {
            double difficulty = dungeon.GetDifficulty();
            if (rand.Next(0, 2) == 0)
            {
                move = false;
            }
            else
            {
                move = true;
            }
            minCoin = Convert.ToInt32(8 * difficulty);
            maxCoin = Convert.ToInt32(12 * difficulty);
            keyChance = 7;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("h", ConsoleColor.Red);
        }
        public override void Move()
        {
            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
            Player player = dungeon.GetPlayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            Dictionary<Coord, List<Coord>> coordAdjacencyList = dungeon.GetCoordAdjacencyList();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            if (move)
            {
                List<Coord> nextCoords = Dijkstras(dungeon, new Coord(player.x, player.y), new Coord(x, y));
                if (nextCoords.Count > 0)
                {
                    Coord nextCoord = nextCoords[0];
                    if (IsMoveValid(nextCoord.x, nextCoord.y))
                    {
                        entityLayer[y, x] = null;
                        x = nextCoord.x;
                        y = nextCoord.y;
                        entityLayer[y, x] = this;
                    }
                    else if (entityLayer[nextCoord.y, nextCoord.x] == player)
                    {
                        player.ChangeHP(player.GetHP() - dmg, entityLayer);
                        effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                        
                    }
                }
                move = false;
            }
            else
            {
                move = true;
            }

        }
    }
    public class TrackingUniformAOEEnemy: UniformAOEEnemy
    {
        public TrackingUniformAOEEnemy(int xin, int yin, int hpin, int dmgin, Dungeon dungeonIn) : base(xin, yin, hpin, dmgin, dungeonIn)
        {
            double difficulty = dungeon.GetDifficulty();
            if (rand.Next(0, 2) == 0)
            {
                move = false;
            }
            else
            {
                move = true;
            }
            minCoin = Convert.ToInt32(10 * difficulty);
            maxCoin = Convert.ToInt32(14 * difficulty);
            keyChance = 20;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("j", ConsoleColor.Red);
        }
        public override void Move()
        {

            if (UniformAOEAttackChargeUp)
            {
                UniformAOEAttack();
            }
            else
            {
                if (move)
                {
                    Player player = dungeon.GetPlayer();
                    Entity[,] entityLayer = dungeon.GetEntityLayer();
                    List<Coord> nextCoords = BreadthFirstSearch(dungeon, new Coord(player.x, player.y), new Coord(x, y));
                    if (nextCoords.Count > 0)
                    {
                        Coord nextCoord = nextCoords[0];
                        if (IsMoveValid(nextCoord.x, nextCoord.y))
                        {
                            entityLayer[y, x] = null;
                            x = nextCoord.x;
                            y = nextCoord.y;
                            entityLayer[y, x] = this;
                        }
                        else if (entityLayer[nextCoord.y, nextCoord.x] == player)
                        {
                            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
                            Attack();
                            effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                        }
                    }
                    UniformAOEAttack();
                    move = false;
                }
                else
                {
                    move = true;
                }
            }
        }
    }
    public class TrackingStraightRangedEnemy : StraightRangedEnemy
    {
        public TrackingStraightRangedEnemy(int xin, int yin, int hpin, int dmgin, Dungeon dungeonIn) : base(xin, yin, hpin, dmgin, dungeonIn)
        {
            double difficulty = dungeon.GetDifficulty();
            if (rand.Next(0, 2) == 0)
            {
                move = false;
            }
            else
            {
                move = true;
            }
            minCoin = Convert.ToInt32(10 * difficulty);
            maxCoin = Convert.ToInt32(14 * difficulty);
            keyChance = 20;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("k",ConsoleColor.Red);
        }
        public override void Move()
        {
            if (rangeAttackChargeUp)
            {
                StraightRangedAttack();
            }
            else
            {
                if (move)
                {
                    if(IsStraightRangedAttackValid())
                    {
                        StraightRangedAttack();
                    }
                    else
                    {
                        Player player = dungeon.GetPlayer();
                        Entity[,] entityLayer = dungeon.GetEntityLayer();
                        List<Coord> nextCoords = BreadthFirstSearch(dungeon, new Coord(player.x, player.y), new Coord(x, y));
                        if (nextCoords.Count > 0)
                        {
                            Coord nextCoord = nextCoords[0];
                            if (IsMoveValid(nextCoord.x, nextCoord.y))
                            {
                                entityLayer[y, x] = null;
                                x = nextCoord.x;
                                y = nextCoord.y;
                                entityLayer[y, x] = this;
                            }
                            else if (entityLayer[nextCoord.y, nextCoord.x] == player)
                            {
                                DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
                                Attack();
                                effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                            }
                        }
                    }
                    move = false;
                }
                else
                {
                    move = true;
                }
            }
        }
    }
    public class Boss:Enemy{
        
        protected bool rangeAttackChargeUp = false;
        protected bool UniformAOEAttackChargeUp = false;
        protected double difficulty;
        public Boss(int xin, int yin, int hpin, int dmgin, Dungeon dungeonIn) : base(xin, yin, hpin, dmgin, dungeonIn)
        {
            damageTileImmunity=true;
            difficulty = dungeon.GetDifficulty();
            if (rand.Next(0, 2) == 0)
            {
                move = false;
            }
            else
            {
                move = true;
            }
            minCoin = Convert.ToInt32(20 * difficulty);
            maxCoin = Convert.ToInt32(25 * difficulty);
            keyChance = 100;
            hp = Convert.ToInt32(hpin * difficulty);
            dmg = Convert.ToInt32(dmgin * ((1 + difficulty) / 2));
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("z",ConsoleColor.Red);
        }
        public override void Move()
        {
            
            Player player = dungeon.GetPlayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            Dictionary<Coord, List<Coord>> coordAdjacencyList = dungeon.GetCoordAdjacencyList();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            if (rangeAttackChargeUp)
            {
                StraightRangedAttack();
            }
            if (UniformAOEAttackChargeUp)
            {
                UniformAOEAttack();
            }
            if (move)
            {
                if (IsStraightRangedAttackValid() && rand.Next(0, 2) == 0&&difficulty>=2.5)
                {
                    StraightRangedAttack();
                }
                else if (IsUniformAOEAttackValid() && rand.Next(0, 2) == 0&&difficulty>=2)
                {
                    UniformAOEAttack();
                }
                else
                {
                    List<Coord> nextCoords = Dijkstras(dungeon, new Coord(player.x, player.y), new Coord(x, y));
                    if (nextCoords.Count > 0)
                    {
                        Coord nextCoord = nextCoords[0];
                        if (IsMoveValid(nextCoord.x, nextCoord.y))
                        {
                            entityLayer[y, x] = null;
                            x = nextCoord.x;
                            y = nextCoord.y;
                            entityLayer[y, x] = this;
                        }
                        else if (entityLayer[nextCoord.y, nextCoord.x] == player)
                        {
                            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
                            Attack();
                            effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                        }
                        
                    }
                    move = false;
                }

            }
            else
            {
                move = true;
                
            }
            

        }
        public void UniformAOEAttack()
        {
            Player player = dungeon.GetPlayer();
            DisplayCharacter[,] effectsLayer = dungeon.GetEffectsLayer();
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            if (UniformAOEAttackChargeUp)
            {
                if (x + 1 == player.x && y == player.y)
                {
                    player.ChangeHP(player.GetHP() - dmg, entityLayer);
                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                }
                else if (x - 1 == player.x && y == player.y)
                {
                    player.ChangeHP(player.GetHP() - dmg, entityLayer);
                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                }
                else if (x + 1 == player.x && y + 1 == player.y)
                {
                    player.ChangeHP(player.GetHP() - dmg, entityLayer);
                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                }
                else if (x - 1 == player.x && y + 1 == player.y)
                {
                    player.ChangeHP(player.GetHP() - dmg, entityLayer);
                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                }
                else if (x + 1 == player.x && y - 1 == player.y)
                {
                    player.ChangeHP(player.GetHP() - dmg, entityLayer);
                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                }
                else if (x - 1 == player.x && y - 1 == player.y)
                {
                    player.ChangeHP(player.GetHP() - dmg, entityLayer);
                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                }
                else if (x == player.x && y + 1 == player.y)
                {
                    player.ChangeHP(player.GetHP() - dmg, entityLayer);
                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                }
                else if (x == player.x && y - 1 == player.y)
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
            if (player.x == x || player.y == y)
            {
                if (rangeAttackChargeUp)
                {
                    if (player.x == x)
                    {
                        if (player.y > y)
                        {
                            for (int i = y+1; i <= player.y; i++)
                            {
                                if (!IsAttackValid(x,i))
                                {
                                    break;
                                }
                                else if (i == player.y)
                                {
                                    player.ChangeHP(player.GetHP() - dmg, entityLayer);
                                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                                }
                            }
                        }
                        else
                        {
                            for (int i = y-1; i >= player.y; i--)
                            {
                                if (!IsAttackValid(x,i))
                                {
                                    break;
                                }
                                else if (i == player.y)
                                {
                                    player.ChangeHP(player.GetHP() - dmg, entityLayer);
                                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                                }
                            }
                        }
                    }
                    else if (player.y == y)
                    {
                        if (player.x > x)
                        {
                            for (int i = x+1; i <= player.x; i++)
                            {
                                if (!IsAttackValid(i,y))
                                {
                                    break;
                                }
                                else if (i == player.x)
                                {
                                    player.ChangeHP(player.GetHP() - dmg, entityLayer);
                                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                                }
                            }
                        }
                        else
                        {
                            for (int i = x-1; i >= player.x; i--)
                            {
                                if (!IsAttackValid(i,y))
                                {
                                    break;
                                }
                                else if (i == player.x)
                                {
                                    player.ChangeHP(player.GetHP() - dmg, entityLayer);
                                    effectsLayer[player.y, player.x] = new DisplayCharacter("*",ConsoleColor.DarkRed);
                                }
                            }
                        }
                    }
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
                                if (!IsAttackValid(x,i))
                                {
                                    break;
                                }
                                effectsLayer[i, x] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                            }
                        }
                        else
                        {
                            for (int i = y - 1; i >= player.y + 1; i--)
                            {
                                if (!IsAttackValid(x,i))
                                {
                                    break;
                                }
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
                                if (!IsAttackValid(i,y))
                                {
                                    break;
                                }
                                effectsLayer[y, i] = new DisplayCharacter("~",ConsoleColor.DarkRed);
                            }
                        }
                        else
                        {
                            for (int i = x - 1; i >= player.x + 1; i--)
                            {
                                if (!IsAttackValid(i,y))
                                {
                                    break;
                                }
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
        public override void EnemyAttacked(int dmg)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            List<Item>[,] itemLayer = dungeon.GetItemLayer();
            List<Enemy> enemies = dungeon.GetEnemies();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            Player player = dungeon.GetPlayer();
            hp -= dmg;
            bool exitTilePlaced = false;
            int tileX;
            int tileY;
            int numAttempts = 0;
            int maxAttempts = 100;
            if (!IsAlive())
            {
                DropCoins( );
                DropKey();
                enemies.Remove(this);
                entityLayer[y, x] = null;
                while (!exitTilePlaced && numAttempts <= maxAttempts)
                {
                    tileY = rand.Next(y - 5, y + 5);
                    tileX = rand.Next(x - 5, x + 5);
                    if (!tileLayer[tileY, tileX].DoesCollide() && entityLayer[tileY, tileX] != player && entityLayer[tileY, tileX] != this)
                    {
                        tileLayer[tileY, tileX] = new ExitTile();
                        exitTilePlaced = true;
                    }
                    numAttempts++;
                    
                }
                if(numAttempts==maxAttempts&&!exitTilePlaced)
                {tileLayer[y, x] = new ExitTile();}
            }
        }
    }
}
