namespace goodbyeworld
{

    public class Room
    {
        public static Random rand = new Random();
        public int x;
        public int y;
        public int height;
        public int width;
        protected bool enemies;
        protected int minEnemies;
        protected int maxEnemies;
        public Room(int xin, int yin, int heightin, int widthin)
        {
            x = xin;
            y = yin;
            height = heightin;
            width = widthin;
            enemies = false;
            minEnemies = 1;
            if ((height - 2) * (width - 2) < 16)
            {
                maxEnemies = 1;
            }
            else if ((height - 2) * (width - 2) < 25)
            {
                maxEnemies = 2;
            }
            else if ((height - 2) * (width - 2) < 36)
            {
                maxEnemies = 3;
            }
            else if ((height - 2) * (width - 2) < 49)
            {
                minEnemies = 2;
                maxEnemies = 4;
            }
            else if ((height - 2) * (width - 2) < 64)
            {
                minEnemies = 3;
                maxEnemies = 5;
            }
            else if ((height - 2) * (width - 2) < 81)
            {
                minEnemies = 4;
                maxEnemies = 6;
            }
            else
            {
                minEnemies = 4;
                maxEnemies = 6;
            }
        }
        public Room() { }
        
        public virtual void AddRoom(Tile[,] dungeon)
        {
            for (int i = y; i < y + height; i++)
            {
                for (int j = x; j < x + width; j++)
                {
                    if ((i == y || i == y + height - 1) && (j == x || j == x + width - 1))
                    {
                        continue;
                    }
                    else if (i == y || i == y + height - 1)
                    {
                        dungeon[i, j] = new Wall();
                    }
                    else if (j == x || j == x + width - 1)
                    {
                        dungeon[i, j] = new Wall();
                    }
                    else
                    {
                        dungeon[i, j] = new Floor();
                    }

                }
            }
        }
        public virtual bool IsRoomValid(Tile[,] dungeon)
        {
            for (int i = y - 1; i < y + height + 1; i++)
            {
                for (int j = x - 1; j < x + width + 1; j++)
                {
                    if (dungeon[i, j] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public int GetNumEnemies()
        {
            return rand.Next(minEnemies, maxEnemies + 1);
        }
        public bool HasEnemies()
        {
            return enemies;
        }
    }
    public class ChestRoom : Room
    {
        public ChestRoom() { }
        public ChestRoom(int xin, int yin):base(xin,yin,7,7)
        {
            enemies = false;
        }
        public override void AddRoom(Tile[,] dungeon)
        {
            for (int i = y; i < y + height; i++)
            {
                for (int j = x; j < x + width; j++)
                {
                    if ((i == y || i == y + height - 1) && (j == x || j == x + width - 1))
                    {
                        continue;
                    }
                    else if (i == y || i == y + height - 1)
                    {
                        dungeon[i, j] = new Wall();
                    }
                    else if (j == x || j == x + width - 1)
                    {
                        dungeon[i, j] = new Wall();
                    }
                    else if (i == y + (height-1)/2 && j == x + (width-1) / 2)
                    {
                        dungeon[i, j] = new Chest(j, i);
                    }
                    else
                    {
                        dungeon[i, j] = new Floor();
                    }

                }
            }
        }
    }
    public class RandomRoom : Room
    {
        protected bool exitRoom;
        protected bool hasRandomWalls = false;
        public RandomRoom() { }
        public RandomRoom(int maxx, int maxy, int minHeight, int maxHeight, int minWidth, int maxWidth, bool isExitRoom) : base()
        {
            height = rand.Next(minHeight, maxHeight + 1);
            width = rand.Next(minWidth, maxWidth + 1);
            x = rand.Next(1, maxx - width - 1);
            y = rand.Next(1, maxy - height - 1);
            enemies = true;
            exitRoom = isExitRoom;
            minEnemies = 1;
            if (rand.Next(0, 2) == 0)
            {
                hasRandomWalls = true;
            }
            if ((height - 1) * (width - 1) < 16)
            {
                maxEnemies = 1;
            }
            else if ((height - 1) * (width - 1) < 25)
            {
                maxEnemies = 2;
            }
            else if ((height - 1) * (width - 1) < 36)
            {
                maxEnemies = 3;
            }
            else if ((height - 1) * (width - 1) < 49)
            {
                minEnemies = 2;
                maxEnemies = 4;
            }
            else if ((height - 1) * (width - 1) < 64)
            {
                minEnemies = 3;
                maxEnemies = 5;
            }
            else if ((height - 1) * (width - 1) < 81)
            {
                minEnemies = 4;
                maxEnemies = 6;
            }
            else
            {
                minEnemies = 4;
                maxEnemies = 6;
            }
            
        }
        public override void AddRoom(Tile[,] dungeon)
        {
            for (int i = y; i < y + height; i++)
            {
                for (int j = x; j < x + width; j++)
                {
                    if ((i == y || i == y + height - 1) && (j == x || j == x + width - 1))
                    {
                        continue;
                    }
                    else if (i == y || i == y + height - 1)
                    {
                        dungeon[i, j] = new Wall();
                    }
                    else if (j == x || j == x + width - 1)
                    {
                        dungeon[i, j] = new Wall();
                    }
                    else
                    {
                        if (rand.Next(0, (height - 2) * (width - 2)) < (height - 2) * (width - 2) / 7)//1/7 chance of a floor being a wall instead
                        {
                            dungeon[i, j] = new Wall();
                        }
                        else
                        {
                            dungeon[i, j] = new Floor();
                        }
                    }

                }
            }
            if(exitRoom)
            {dungeon[rand.Next(y+1,y+height-1),rand.Next(x+1,x+width-1)]=new ExitTile();}
        }
    }
    public class BossRoom : Room
    {
        protected double difficulty;
        public BossRoom(int xin, int yin,int heightIn, int widthIn,double difficultyIn):base(xin,yin,heightIn,widthIn)
        {
            enemies = false;
            difficulty=difficultyIn;
        }
        public override void AddRoom(Tile[,] dungeon)
        {
            for (int i = y; i < y + height; i++)
            {
                for (int j = x; j < x + width; j++)
                {
                    if ((i == y || i == y + height - 1) && (j == x || j == x + width - 1))
                    {
                        continue;
                    }
                    else if (i == y || i == y + height - 1)
                    {
                        dungeon[i, j] = new Wall();
                    }
                    else if (j == x || j == x + width - 1)
                    {
                        dungeon[i, j] = new Wall();
                    }
                    else
                    {
                        int chance = rand.Next(0,50);
                        if (difficulty > 4)
                        {
                            if (chance < 44)
                            { 
                                dungeon[i, j] = new Floor(); 
                            }
                            else if(chance<48)
                            {
                                dungeon[i, j] = new Wall();
                            }
                            else
                            {
                                dungeon[i,j] = new DamageTile(1,true);
                            }
                        }
                        else
                        {
                            if (chance < 48)
                            { 
                                dungeon[i, j] = new Floor(); 
                            }
                            else
                            {
                                dungeon[i, j] = new Wall();
                            }
                        }
                    }

                }
            }
        }
    }
}