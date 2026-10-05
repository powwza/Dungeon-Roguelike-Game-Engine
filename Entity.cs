using System.Formats.Asn1;
using System.Linq.Expressions;

namespace goodbyeworld
{
    public abstract class Entity
    {
        public static Random rand = new Random();
        public int x;
        public int y;
        public Entity(int xin, int yin)
        {
            x = xin;
            y = yin;
        }
        public Entity(){}
        public abstract DisplayCharacter Display();
        

    }
    public abstract class LivingEntity : Entity
    {
        protected bool damageTileImmunity = false;
        protected int maxHP;
        protected int hp;
        public LivingEntity() : base() { }
        public LivingEntity(int xin, int yin, int hpin) : base(xin, yin)
        {
            maxHP = hpin;
            hp = maxHP;

        }
        public List<Coord> Dijkstras(Dungeon dungeon,Coord destination, Coord source)
        {
            Coord currentNode;
            bool found = false;
            Tile[,] tileLayer = dungeon.GetTileLayer();
            int dunHeight = dungeon.GetDunHeight();
            int dunWidth = dungeon.GetDunWidth();
            Dictionary<Coord, List<Coord>> coordAdjacencyList = dungeon.GetCoordAdjacencyList();
            int[,] weightMap = dungeon.GetWeightMap();
            List<Coord> shortestPath = new List<Coord>();
            PriorityQueue queue = new PriorityQueue(5000);
            bool[,] visited = new bool[dunHeight, dunWidth]; 
            Coord[,] parent = new Coord[dunHeight, dunWidth];
            int[,] distance = new int[dunHeight,dunWidth];
            for (int i = 0; i < dunHeight; i++) 
            {
                for (int j = 0; j < dunWidth; j++)
                {
                    distance[i,j] = 1000000;

                }
            }
            weightMap[source.y,source.x]=0;
            parent[source.y, source.x] = new Coord(-1,-1);
            distance[source.y,source.x]=0;
            visited[source.y, source.x] = true;
            queue.Enqueue(source,distance);
            while (!queue.IsEmpty()) 
            {
                currentNode = queue.Dequeue();
                foreach (Coord neighbour in coordAdjacencyList[currentNode]) //check each neighbour of current node and add them to queue if valid
                {
                    if (!visited[neighbour.y, neighbour.x]) //if it hasnt been visited
                    {
                        if (weightMap[neighbour.y,neighbour.x]+distance[currentNode.y,currentNode.x]<distance[neighbour.y,neighbour.x]) //if the weight at the neighbour+the distance of the current node is smaller than the neighbour's current distance
                        {
                            distance[neighbour.y,neighbour.x]=weightMap[neighbour.y,neighbour.x]+distance[currentNode.y,currentNode.x]; //change the distance
                            parent[neighbour.y,neighbour.x]=currentNode;// change the parent
                        }
                        
                        else //if the weight at the neighbour+the distance of the current node is greater than or equal to the neighbour's current distance
                        {
                            visited[neighbour.y,neighbour.x]=true; //set the neighbour as visited, its distance wont be lower
                        }
                        
                        queue.Enqueue(neighbour,distance);
                    }
                    if (neighbour.y == destination.y && neighbour.x == destination.x)
                    {
                        found = true;
                    }
                }
            
            }
            currentNode = destination;
            bool sourceReached = false;
            if(found)
            {
                while (!sourceReached)
                {
                    shortestPath.Add(currentNode);
                    currentNode = parent[currentNode.y, currentNode.x];
                    if (currentNode.x == source.x && currentNode.y == source.y)
                    {
                        sourceReached = true;
                    }
                }
            }
            shortestPath.Reverse();
            
            return shortestPath;
        }
        public List<Coord> BreadthFirstSearch(Dungeon dungeon,Coord destination, Coord source)
        {
            Coord currentNode;
            Tile[,] tileLayer = dungeon.GetTileLayer();
            int dunHeight = dungeon.GetDunHeight();
            int dunWidth = dungeon.GetDunWidth();
            Dictionary<Coord, List<Coord>> coordAdjacencyList = dungeon.GetCoordAdjacencyList();
            bool found = false;
            List<Coord> shortestPath = new List<Coord>();
            Queue queue = new Queue(1000);
            bool[,] visited = new bool[dunHeight, dunWidth]; //create bool grids to show which nodes of maze have been visited
            Coord[,] parent = new Coord[dunHeight, dunWidth];
            parent[source.y, source.x] = new Coord(-1, -1);
            queue.Enqueue(source);
            visited[source.y, source.x] = true;
            while (!found && !queue.IsEmpty()) //while the destination hasn't been found and the queue still has a node in, repeat
            {
                currentNode = queue.Dequeue(); //look at the next node in the queue
                foreach (Coord neighbour in coordAdjacencyList[currentNode]) //check each neighbour of current node and add them to queue if valid
                {
                    if (!visited[neighbour.y, neighbour.x])
                    {
                        queue.Enqueue(neighbour);
                        visited[neighbour.y, neighbour.x] = true;
                        parent[neighbour.y, neighbour.x] = currentNode;
                        if (neighbour.y == destination.y && neighbour.x == destination.x)
                        {

                            found = true;
                        }
                    }

                }


            }
            currentNode = destination;
            bool sourceReached = false;
            if (found)
            {
                while (!sourceReached)
                {
                    shortestPath.Add(currentNode);
                    currentNode = parent[currentNode.y, currentNode.x];
                    if (currentNode.x == source.x && currentNode.y == source.y)
                    {
                        sourceReached = true;
                    }
                }
                shortestPath.Reverse();
            }
            return shortestPath;

        }
        public bool IsAlive()
        {
            if (hp > 0)
            {
                return true;
            }
            return false;
        }
        public int GetHP()
        {
            return hp;
        }
        public int GetMaxHP()
        {
            return maxHP;
        }
        public void ChangeMaxHP(int amount)
        {
            maxHP = amount;
        }
        public void ChangeHP(int amount,Entity[,] entityLayer)
        {
            if (amount >= maxHP)
            {
                hp = maxHP;
            }
            else if(amount<0)
            {
                hp = 0;
            }
            else
            {
                hp=amount;
            }
            if(!IsAlive()){
                entityLayer[y,x]=null;
            }
        }
        
    }
    public class Player : LivingEntity
    {
        protected int dmg;
        protected List<PassiveStatItem> passiveStatItems= new List<PassiveStatItem>();
        protected ConsumableItem consumableItem;
        protected Weapon weapon = new Sword(1);
        protected PassiveAbility passiveAbility;
        protected ActiveAbility activeAbility;

        protected int numCoins = 0;
        protected int numKeys = 1;

        public Player(int xin, int yin, int hpin, int dmgin) : base(xin, yin, hpin)
        {
            dmg = dmgin;
            GivePlayerRandomAbilities();
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("i",ConsoleColor.White);
        }
        public void GivePlayerRandomAbilities()
        {
            List<PassiveAbility> passiveAbilities = new List<PassiveAbility> {new PassiveAOEAttack(15,1,0),new PassiveHeal(30,1,0),new PassiveTemporaryDMGBoost(20,1,5),new PassiveDamageTileTrail(15,1,0) };
            List<ActiveAbility> activeAbilities = new List<ActiveAbility> {new ActiveAOEAttack(10,1,0),new ActiveHeal(30,1,0),new ActiveTemporaryDMGBoost(20,1,5),new ActiveDamageTileTrail(25,1,0) };
            passiveAbility = passiveAbilities[rand.Next(0, passiveAbilities.Count)];
            activeAbility = activeAbilities[rand.Next(0, activeAbilities.Count)];
        }
        public void Input(Dungeon dungeon)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            ConsoleKey input;
            input = Console.ReadKey().Key;
            if (input == ConsoleKey.UpArrow)
            {
                if (IsMoveValid(dungeon, x, y - 1))
                {
                    entityLayer[y, x] = null;
                    y -= 1;
                    entityLayer[y, x] = this;
                }
            }
            else if (input == ConsoleKey.DownArrow)
            {
                if (IsMoveValid(dungeon, x, y + 1))
                {
                    entityLayer[y, x] = null;
                    y += 1;
                    entityLayer[y, x] = this;
                }
            }
            else if (input == ConsoleKey.LeftArrow)
            {
                if (IsMoveValid(dungeon, x - 1, y))
                {
                    entityLayer[y, x] = null;
                    x -= 1;
                    entityLayer[y, x] = this;

                }
            }
            else if (input == ConsoleKey.RightArrow)
            {
                if (IsMoveValid(dungeon, x + 1, y))
                {
                    entityLayer[y, x] = null;
                    x += 1;
                    entityLayer[y, x] = this;
                }
            }
            else if (input == ConsoleKey.W || input == ConsoleKey.A || input == ConsoleKey.S || input == ConsoleKey.D)
            {
                weapon.UseWeapon(dungeon, this, input);
            }
            else if (input == ConsoleKey.E)
            {
                if (consumableItem != null)
                {
                    if (consumableItem.GetType().IsSubclassOf(typeof(DurationConsumableItem)))
                    {
                        DurationConsumableItem item = (DurationConsumableItem)consumableItem;
                        if (item.GetNumUses() > 0)
                        {
                            if (item.GetCurrentDuration() == 0)
                            {
                                item.ChangeCurrentDuration(item.GetDuration());
                                item.DoItemEffect(this, dungeon);
                                item.UseItem();
                                if (item.GetNumUses() <= 0 && item.GetCurrentDuration() <= 0)
                                {
                                    consumableItem = null;
                                }
                            }
                        }
                        else
                        {
                            consumableItem = null;
                        }
                    }
                    else
                    {
                        if (consumableItem.GetNumUses() > 0)
                        {
                            consumableItem.DoItemEffect(this, dungeon);
                            consumableItem.UseItem();
                            if (consumableItem.GetNumUses() <= 0)
                            {
                                consumableItem = null;
                            }
                        }
                        else
                        {
                            consumableItem = null;
                        }
                    }

                }
            }
            else if (input == ConsoleKey.Q)
            {
                if (activeAbility != null && activeAbility.CanUse())
                {
                    if (activeAbility.GetDuration() > 0)
                    {
                        activeAbility.ChangeCurrentDuration(activeAbility.GetDuration());
                    }
                    else
                    {
                        activeAbility.UseAbility(dungeon, this);
                    }
                }
            }
        }
        public void DecreasePlayerCooldownsAndDurations(Dungeon dungeon)
        {
            if (activeAbility!=null)
            {
                activeAbility.DecreaseCooldown(dungeon, this);
            }
            if (passiveAbility!=null)
            {
                passiveAbility.DecreaseCooldown(dungeon, this);
            }
            if (consumableItem!=null && consumableItem.GetType().IsSubclassOf(typeof(DurationConsumableItem)))
            {
                DurationConsumableItem item = (DurationConsumableItem)consumableItem;
                item.DecreaseDuration(this, dungeon);
                if (item.GetCurrentDuration() <= 0 && item.GetNumUses() <= 0)
                {
                    consumableItem = null;
                }
            }
        }
        public bool IsMoveValid(Dungeon dungeon,int x, int y)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            List<Item>[,] itemLayer = dungeon.GetItemLayer();
            List<Enemy> enemies = dungeon.GetEnemies();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            if (x < 0 || x > width - 1 || y < 0 || y > height - 1)
            {
                return false;
            }
            else if (tileLayer[y, x] == null)
            {
                tileLayer[y, x] = new Floor();
                return false;
            }
            else if (tileLayer[y, x].GetType() == new Wall().GetType())
            {
                tileLayer[y, x] = new Floor();
                return false;
            }
            else if (tileLayer[y, x].GetType() == new Door().GetType() && tileLayer[y, x].DoesCollide())
            {
                ((Door)tileLayer[y, x]).Interact();
                return false;

            }
            else if (tileLayer[y, x].GetType() == new Chest().GetType()&& tileLayer[y, x].DoesCollide())
            {
                ((Chest)tileLayer[y, x]).OpenChest(this, itemLayer);
                return false;

            }
            else if (tileLayer[y, x].GetType() == typeof(DungeonBorder))
            {
                return false;
            }
            else if (entityLayer[y, x] != null)
            {
                return false;
            }
            else if (tileLayer[y, x].GetType() == new ExitTile().GetType())
            {
                dungeon.ExitDungeon();
            }
            if (itemLayer[y, x] != null && itemLayer[y, x].Count > 0)
            {
                PickUpItem(dungeon, itemLayer[y, x], x, y);
            }
            return true;
        }
        public void PickUpItem(Dungeon dungeon, List<Item> items, int x, int y)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            List<Item>[,] itemLayer = dungeon.GetItemLayer();
            List<Item> newItems = new List<Item>();
            foreach (Item item in items)
            {

                if (item.GetType() == new Coin().GetType())
                {
                    Coin coin = (Coin)item;
                    numCoins += coin.GetNumCoins();

                }
                else if (item.GetType() == new Key().GetType())
                {
                    Key key = (Key)item;
                    numKeys += key.GetNumKeys();

                }
                else if (item.GetType().IsSubclassOf(typeof(Weapon)))
                {
                    Weapon temp = weapon;
                    weapon = (Weapon)item;
                    newItems.Add(temp);
                }
                else if (item.GetType().IsSubclassOf(typeof(ConsumableItem)))
                {
                    if (consumableItem == null)
                    {
                        consumableItem = (ConsumableItem)item;
                    }
                    else
                    {
                        ConsumableItem temp = consumableItem;
                        consumableItem = (ConsumableItem)item;
                        newItems.Add(temp);
                    }
                }
                else if (item.GetType().IsSubclassOf(typeof(PassiveStatItem)))
                {
                    PassiveStatItem temp = (PassiveStatItem)item;
                    temp.BoostPlayerStat(this, entityLayer);
                    passiveStatItems.Add(temp);
                }

            }
            if (newItems.Count <= 0)
            { itemLayer[y, x] = null; }
            else
            {
                itemLayer[y, x].Clear();
                foreach (Item item in newItems)
                {
                    itemLayer[y, x].Add(item);
                }
            }


        }
        public int EnemyAttack(Enemy enemy, Dungeon dungeon)
        {
            enemy.EnemyAttacked(dmg);
            return dmg;
        }
        public int GetDMG()
        {
            return dmg;
        }
        public void ChangeDMG(int amount)
        {
            dmg = amount;
        }
        public void DisplayInventory()
        {

        }
        public int GetNumCoins()
        {
            return numCoins;
        }
        public int GetNumKeys()
        {
            return numKeys;
        }
        public Weapon GetWeapon()
        {
            return weapon;
        }
        public void ChangeNumCoins(int amount)
        {
            numCoins = amount;
        }
        public void ChangeNumKeys(int amount)
        {
            numKeys = amount;
        }
        public ActiveAbility GetActiveAbility()
        {
            return activeAbility;
        }
        public PassiveAbility GetPassiveAbility()
        {
            return passiveAbility;
        }
        public ConsumableItem GetConsumableItem()
        {
            return consumableItem;
        }
        public bool HasConsumableItem()
        {
            if (consumableItem == null)
            {
                return false;
            }
            return true;
        }
        public bool HasPassiveAbility()
        {
            if (passiveAbility == null)
            {
                return false;
            }
            return true;
        }
        public bool HasActiveAbility()
        {
            if (activeAbility == null)
            {
                return false;
            }
            return true;
        }
    }
}