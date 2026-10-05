namespace goodbyeworld
{
    public struct DisplayCharacter(string characterIn,ConsoleColor colourIn)
    {
        public string character = characterIn;
        public ConsoleColor colour = colourIn;
    }
    public class Dungeon
    {
        private static Random rand = new Random();
        private Dictionary<Coord, List<Coord>> coordAdjacencyList;
        private List<Room> rooms;
        private List<Enemy> enemies;
        private int dunHeight;
        private int dunWidth;
        private int numChestRooms;

        private int maxNumRandomRooms;
        private int minRandomRoomHeight;
        private int maxRandomRoomHeight;
        private int minRandomRoomWidth;
        private int maxRandomRoomWidth;
        private Tile[,] tileLayer;
        private Entity[,] entityLayer;
        private List<Item>[,] itemLayer;
        private DisplayCharacter[,] effectsLayer;
        private int[,] weightMap;
        private Player player;
        private double difficulty;
        private const int cameraHeight =17;
        private const int cameraWidth = 23;
        private const int startingRoomHeight = 5;
        private const int startingRoomWidth = 5;
        private bool exitedDungeon = false;
        private Boss boss;

        public Dungeon(ref Player playerIn, double difficultyIn)
        {
            player = playerIn;
            difficulty = difficultyIn;
            rooms = new List<Room>();
            enemies = new List<Enemy>();
            coordAdjacencyList = new Dictionary<Coord, List<Coord>>();

            //calculate the width and height of the dungeon
            dunWidth = (int)(45 * difficulty);
            if (dunWidth > 150)
            {
                dunWidth = 150;
            }
            dunHeight = (int)(45 * difficulty);
            if (dunHeight > 150)
            {
                dunHeight = 150;
            }


            tileLayer = new Tile[dunHeight, dunWidth];
            entityLayer = new Entity[dunHeight, dunWidth];
            effectsLayer = new DisplayCharacter[dunHeight, dunWidth];
            itemLayer = new List<Item>[dunHeight, dunWidth];
            GenerateTileLayer(); //add rooms and corridors to dungeon
            AddEnemies(); //add enemies to dungeon
            
            //add player to dungeon
            player.x = rooms[0].x + rooms[0].width / 2;
            player.y = rooms[0].y + rooms[0].height / 2;
            entityLayer[player.y, player.x] = player;
        }
        public void GenerateTileLayer()
        {
            int maxAttempts = 100;
            int numAttempts = 0;
            bool roomsValid = false;
            bool corridorsValid = false;
            numChestRooms = 2;
            //calculate the number of random rooms
            maxNumRandomRooms = Convert.ToInt32(3 * difficulty);
            if (maxNumRandomRooms > 8)
            {
                maxNumRandomRooms = 8;
            }
            //calculate the minimum and maximum height and width of random rooms
            minRandomRoomHeight = Convert.ToInt32(4 * (difficulty + 1) / 2);
            if (minRandomRoomHeight > 7)
            {
                minRandomRoomHeight = 7;
            }
            maxRandomRoomHeight = Convert.ToInt32(7 * (difficulty + 1) / 2);
            if (maxRandomRoomHeight > 15)
            {
                maxRandomRoomHeight = 15;
            }
            minRandomRoomWidth = Convert.ToInt32(4 * (difficulty + 1) / 2);
            if (minRandomRoomWidth > 7)
            {
                minRandomRoomWidth = 7;
            }
            maxRandomRoomWidth = Convert.ToInt32(7 * (difficulty + 1) / 2);
            if (maxRandomRoomWidth > 15)
            {
                maxRandomRoomWidth = 15;
            }
            //Attempt to add rooms and corridors to dungeon
            while ((!roomsValid || !corridorsValid) && numAttempts < maxAttempts) //repeat until dungeon is generated or max attempts is reached
            {
                rooms.Clear();
                tileLayer = new Tile[dunHeight, dunWidth];
                AddDungeonBorder();
                roomsValid = AddRooms();
                corridorsValid = AddCorridors();
                numAttempts++;
            }
            if (numAttempts == maxAttempts) //if max attempts is reached, dungeon hasn't generated so throw an exception
            {
                throw new Exception("Dungeon failed to generate");
            }
        }
        public void AddDungeonBorder() //adds a border to the dungeon which the player cannot break
        {
            for(int i = 0; i < dunHeight; i++)
            {
                for(int j = 0; j < dunWidth; j++)
                {
                    if (i == 0 || i == dunHeight - 1 || j == 0 || j == dunWidth - 1)
                    {
                        tileLayer[i, j] = new DungeonBorder();
                    }
                }
            }
        }
        public void ExitDungeon(){
            exitedDungeon=true;
        }
        public bool Play()
        {

            while (player.IsAlive() && !exitedDungeon) //while the player is alive and the player hasnt exited the dungeon, repeat
            {
                Console.Clear();

                TopDisplay();
                DisplayDungeon();
                BottomDisplay();

                effectsLayer = new DisplayCharacter[dunHeight, dunWidth];

                PlayerMove();
                
                EnemiesMove();

                coordAdjacencyList.Clear();
            }
            if (exitedDungeon)
            {
                return true;
            }
            else
            {
                Console.Clear();
                TopDisplay();
                Console.BackgroundColor=ConsoleColor.DarkRed;
                DisplayDungeon();
                Console.ResetColor();
                BottomDisplay();
                Thread.Sleep(3000);
            }
            return false;
        }
        public void PlayerMove()
        {
            player.Input(this); //allow player to make a move
            player.DecreasePlayerCooldownsAndDurations(this);
            if (tileLayer[player.y, player.x].GetType() == typeof(DamageTile)&&((DamageTile)tileLayer[player.y, player.x]).IsEnemyDamageTile())
            {
                ((DamageTile)tileLayer[player.y, player.x]).DoDamage(player, entityLayer);
                effectsLayer[player.y,player.x]=new DisplayCharacter("*",ConsoleColor.DarkRed);
            }
        }
        public void EnemiesMove()
        {
            List<Enemy> temp = new List<Enemy>();
            MazeToAdjacencyList();
            GenerateWeightMap();
            foreach (Enemy enemy in enemies) //make all enemies in dungeon move
            {
                enemy.Move(); //enemy moves
                //if the enemy moves onto a damage tile
                if (tileLayer[enemy.y, enemy.x].GetType() == typeof(DamageTile)) 
                {
                    //if the enemy has damage tile immunity and the damage tile is from the player or if the enemy
                    //doesnt have immunity, then do damage to the enemy
                    if((enemy.HasDamageTileImmunity()&&!((DamageTile)tileLayer[enemy.y, enemy.x]).IsEnemyDamageTile())||(!enemy.HasDamageTileImmunity()))
                    {
                        ((DamageTile)tileLayer[enemy.y, enemy.x]).DoDamage(enemy, entityLayer);
                        if(((DamageTile)tileLayer[enemy.y, enemy.x]).IsEnemyDamageTile())
                        {
                            effectsLayer[enemy.y,enemy.x]=new DisplayCharacter("*",ConsoleColor.DarkRed);
                        }
                        else
                        {
                            effectsLayer[enemy.y,enemy.x]=new DisplayCharacter("*",ConsoleColor.DarkBlue);
                        }
                    }
                }
                
                if (enemy.GetHP() > 0)
                {
                    temp.Add(enemy);
                }
            }
            //clear enemies
            enemies.Clear();
            //add all enemies from temp list(enemies that are alive) to enemies list
            foreach(Enemy enemy in temp)
            {
                enemies.Add(enemy);
            }


        }
        public void TopDisplay()
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            //display red hearts for hp the player currently has
            Console.Write("HP: ");
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < player.GetHP(); i++)
            {
                Console.Write("<3 ");
            }
            //display dark grey hearts for hp the player has lost
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < player.GetMaxHP() - player.GetHP(); i++)
            {
                Console.Write("<3 ");
            }
            System.Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("COINS: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(player.GetNumCoins());
            System.Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Gray;

        }
        public void BottomDisplay()
        {
            System.Console.WriteLine($"DMG: {player.GetDMG()+player.GetWeapon().GetDMG()}");
            System.Console.WriteLine($"KEYS: {player.GetNumKeys()}");
            System.Console.WriteLine($"WEAPON: {player.GetWeapon().GetName()}");
            if (player.HasConsumableItem())
            { 
                System.Console.Write($"ITEM: {player.GetConsumableItem().GetName()}, {player.GetConsumableItem().GetNumUses()} USES");
                if (player.GetConsumableItem().GetType().IsSubclassOf(typeof(DurationConsumableItem)))
                {
                    DurationConsumableItem item = (DurationConsumableItem)player.GetConsumableItem();
                    System.Console.Write($", DURATION: {item.GetCurrentDuration()}\n");
                }
                else
                {
                    System.Console.WriteLine();
                }
            }
            else
            {
                System.Console.WriteLine("ITEM: NONE");
            }
            if (player.HasActiveAbility())
            { 
                player.GetActiveAbility().Display(); 
            }
            if (player.HasPassiveAbility())
            { 
                player.GetPassiveAbility().Display();
            }
        }
        public void GenerateBossRoom()
        {
            dunHeight = 99;
            dunWidth = 99;
            tileLayer = new Tile[dunHeight, dunWidth];
            entityLayer = new Entity[dunHeight, dunWidth];
            itemLayer = new List<Item>[dunHeight, dunWidth];
            effectsLayer = new DisplayCharacter[dunHeight, dunWidth];
            enemies = new List<Enemy>();
            rooms.Clear();

            int bossRoomWidth = 29;
            int bossRoomHeight = 29;
            int bossRoomX = ((dunWidth-1)/2)-((bossRoomWidth-1)/2);
            int bossRoomY = ((dunHeight - 1) / 2) - ((bossRoomHeight - 1) / 2);

            AddDungeonBorder();

            Room startingRoom = new Room(dunWidth / 2 - 5 / 2, dunHeight - 5 - 10, startingRoomHeight, startingRoomWidth);
            BossRoom bossRoom = new BossRoom(bossRoomX,bossRoomY,bossRoomHeight,bossRoomWidth,difficulty);
            bossRoom.AddRoom(tileLayer);
            startingRoom.AddRoom(tileLayer);
            rooms.Add(startingRoom);
            rooms.Add(bossRoom);

            AddVerticalCorridor(startingRoom.y, bossRoomY + bossRoomHeight - 1, bossRoomX + ((bossRoomWidth - 1) / 2));

            player.x = rooms[0].x+rooms[0].width/2;
            player.y = rooms[0].y+rooms[0].height/2;
            entityLayer[player.y,player.x] = player;


            boss = new Boss(-1, -1, 4, 1, this);
            AddEnemyToEntityLayer(rooms[1], boss);

            
        }
        public void DisplayDungeon()
        {
            List<DisplayCharacter> displayOutput = new List<DisplayCharacter>();
            int cameraTop;
            int cameraBottom;
            int cameraLeft;
            int cameraRight;
            //if the top of the camera is within the dungeon
            if (player.y - (cameraHeight - 1) / 2 >= 0)
            {
                //if the bottom of the camera is within the dungeon
                if (player.y + (cameraHeight - 1) / 2 <= dunHeight - 1)
                {
                    cameraTop = player.y - (cameraHeight - 1) / 2;
                    cameraBottom = player.y + (cameraHeight - 1) / 2;
                }
                //if the bottom of the camera isnt within the dungeon
                else
                {
                    cameraTop = dunHeight - cameraHeight;
                    cameraBottom = dunHeight - 1;
                }
            }
            //if the top of the camera isnt within the dungeon
            else
            {
                cameraTop = 0;
                cameraBottom = cameraHeight - 1;
            }
            //if the left of the camera is within the dungeon
            if (player.x - (cameraWidth - 1) / 2 >= 0)
            {
                //if the right of the camera is within the dungeon
                if (player.x + (cameraWidth - 1) / 2 <= dunWidth - 1)
                {
                    cameraLeft = player.x - (cameraWidth - 1) / 2;
                    cameraRight = player.x + (cameraWidth - 1) / 2;
                }
                //if the right of the camera isnt within the dungeon
                else
                {
                    cameraLeft = dunWidth - cameraWidth;
                    cameraRight = dunWidth - 1;
                }
            }
            //if the top of the camera isnt within the dungeon
            else
            {
                cameraLeft = 0;
                cameraRight = cameraWidth - 1;
            }
            for (int i = 0; i < cameraWidth+3; i++)
            {
                displayOutput.Add(new DisplayCharacter("==", ConsoleColor.White));
            }
            displayOutput.Add(new DisplayCharacter("\n", ConsoleColor.Black));
            for (int i = cameraTop; i <= cameraBottom; i++)
            {
                for (int j = cameraLeft; j <= cameraRight; j++)
                {
                    if (j == cameraLeft)
                    {
                        displayOutput.Add(new DisplayCharacter("[] ", ConsoleColor.White));
                    }
                    if(tileLayer[i,j]!=null&&tileLayer[i,j].GetType()==new ExitTile().GetType())
                    {
                        DisplayCharacter displayCharacter = new DisplayCharacter();
                        displayCharacter = tileLayer[i, j].Display();
                        displayCharacter.character=displayCharacter.character+" ";
                        displayOutput.Add(displayCharacter);
                    }
                    else if (effectsLayer[i, j].character!=null)
                    {
                        DisplayCharacter displayCharacter = effectsLayer[i,j];
                        displayCharacter.character=displayCharacter.character+" ";
                        displayOutput.Add(displayCharacter);
                    }
                    else if (entityLayer[i, j] != null)
                    {
                        DisplayCharacter displayCharacter = entityLayer[i, j].Display();
                        displayCharacter.character=displayCharacter.character+" ";
                        displayOutput.Add(displayCharacter);
                    }
                    else if (itemLayer[i, j] != null)
                    {
                        DisplayCharacter displayCharacter = itemLayer[i, j][0].GameDisplay();
                        displayCharacter.character=displayCharacter.character+" ";
                        displayOutput.Add(displayCharacter);
                    }
                    else
                    {
                        DisplayCharacter displayCharacter = new DisplayCharacter();
                        if (tileLayer[i, j] == null)
                        {
                            displayCharacter = new DisplayCharacter("O",ConsoleColor.Black);
                        }
                        else
                        {
                            displayCharacter = tileLayer[i, j].Display();
                        }
                        displayCharacter.character=displayCharacter.character+" ";
                        displayOutput.Add(displayCharacter);
                    }
                    if (j == cameraRight)
                    {
                        displayOutput.Add(new DisplayCharacter("[] ", ConsoleColor.White));
                    }
                }
                displayOutput.Add(new DisplayCharacter("\n", ConsoleColor.Black));
            }
            for (int i = 0; i < cameraWidth+3; i++)
            {
                displayOutput.Add(new DisplayCharacter("==", ConsoleColor.White));
            }
            displayOutput.Add(new DisplayCharacter("\n", ConsoleColor.Black));
            foreach (DisplayCharacter character in displayOutput)
            {
                Console.ForegroundColor = character.colour;
                Console.Write(character.character);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public bool AddRooms()
        {
            int currentNumChestRooms = 0;
            int currentNumRandomRooms = 0;
            int maxAttempts = (numChestRooms + maxNumRandomRooms) * 10;
            int numAttempts = 0;
            int exitRoom=rand.Next(0,maxNumRandomRooms);
            Room room;

            Room startingRoom = new Room(dunWidth / 2 - 5 / 2, dunHeight / 2 - 5 / 2, 5, 5);
            rooms.Add(startingRoom);
            startingRoom.AddRoom(tileLayer);

            while (currentNumChestRooms < numChestRooms&& numAttempts < maxAttempts)
            {
                room = new ChestRoom(rand.Next(1, dunWidth - 7 - 1), rand.Next(1, dunWidth - 7 - 1));
                numAttempts++;
                if (room.IsRoomValid(tileLayer))
                {
                    currentNumChestRooms++;
                    rooms.Add(room);
                    room.AddRoom(tileLayer);
                }
            }
            if (currentNumChestRooms != numChestRooms)
            {
                return false;
            }
            while (currentNumRandomRooms < maxNumRandomRooms && numAttempts < maxAttempts)
            {
                if(currentNumRandomRooms==exitRoom)
                {
                    room = new RandomRoom(dunWidth, dunHeight, minRandomRoomHeight, maxRandomRoomHeight, minRandomRoomWidth, maxRandomRoomWidth,true);
                }
                else
                {
                    room = new RandomRoom(dunWidth, dunHeight, minRandomRoomHeight, maxRandomRoomHeight, minRandomRoomWidth, maxRandomRoomWidth,false);
                }
                numAttempts++;
                if (room.IsRoomValid(tileLayer))
                {
                    rooms.Add(room);
                    currentNumRandomRooms++;
                    room.AddRoom(tileLayer);
                }

            }
            if (currentNumRandomRooms==maxNumRandomRooms)
            {
                return true;
            }
            return false;
        }
        public bool AddCorridors()
        {
            Room room1 = new Room();
            Room room2 = new Room();
            int maxAttempts = (rooms.Count - 1) * 10;
            int numAttempts = 0;
            int numCorridors = 0;
            int currentRoom1 = numChestRooms;
            int currentRoom2;
            bool corridorValid = false;
            while (numCorridors < rooms.Count - 1 && numAttempts < maxAttempts)
            {
                corridorValid = false;
                room1 = rooms[currentRoom1];
                
                currentRoom2 = (currentRoom1 + 1) % rooms.Count;
                while (!corridorValid && numAttempts < maxAttempts)
                {

                    if (currentRoom2 == currentRoom1)
                    {
                        return false;
                    }
                    room2 = rooms[currentRoom2];
                    if (IsHorizontalCorridorValid( room1.x + (room1.width - 1) / 2, room2.x + (room2.width - 1) / 2, room1.y + (room1.height - 1) / 2) && IsVerticalCorridorValid(room2.y + (room2.height - 1) / 2, room1.y + (room1.height - 1) / 2, room2.x + (room2.width - 1) / 2))
                    {
                        corridorValid = true;
                        AddHorizontalCorridor(room1.x + (room1.width - 1) / 2, room2.x + (room2.width - 1) / 2, room1.y + (room1.height - 1) / 2);
                        AddVerticalCorridor(room2.y + (room2.height - 1) / 2, room1.y + (room1.height - 1) / 2, room2.x + (room2.width - 1) / 2);
                    }
                    else if (IsHorizontalCorridorValid( room2.x + (room2.width - 1) / 2, room1.x + (room1.width - 1) / 2, room2.y + (room2.height - 1) / 2) && IsVerticalCorridorValid(room1.y + (room1.height - 1) / 2, room2.y + (room2.height - 1) / 2, room1.x + (room1.width - 1) / 2))
                    {
                        corridorValid = true;
                        AddHorizontalCorridor(room2.x + (room2.width - 1) / 2, room1.x + (room1.width - 1) / 2, room2.y + (room2.height - 1) / 2);
                        AddVerticalCorridor(room1.y + (room1.height - 1) / 2, room2.y + (room2.height - 1) / 2, room1.x + (room1.width - 1) / 2);
                    }
                    else
                    {
                        currentRoom2 = (currentRoom2 + 1) % rooms.Count;
                    }
                    numAttempts++;
                }
                currentRoom1 = (currentRoom1 + 1) % rooms.Count; ;

                numCorridors++;
            }
            if (numCorridors == rooms.Count - 1)
            {
                return true;
            }
            return false;
        }
        public bool IsHorizontalCorridorValid(int x1, int x2, int y)
        {
            int consecutiveWalls = 0;
            if (x1 < x2)
            {
                for (int i = x1; i <= x2; i++)
                {
                    if (tileLayer[y, i] == null)
                    {
                        consecutiveWalls = 0;
                    }

                    else if (tileLayer[y, i].GetType() == (new Wall()).GetType())
                    {
                        consecutiveWalls++;
                    }

                    if (consecutiveWalls > 1)
                    {
                        return false;
                    }
                }
            }
            else if (x2 < x1)
            {
                for (int i = x1; i >= x2; i--)
                {
                    if (tileLayer[y, i] == null)
                    {
                        consecutiveWalls = 0;
                    }

                    else if (tileLayer[y, i].GetType() == (new Wall()).GetType())
                    {
                        consecutiveWalls++;
                    }

                    if (consecutiveWalls > 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool IsVerticalCorridorValid( int y1, int y2, int x)
        {
            int consecutiveWalls = 0;
            if (y1 < y2)
            {
                for (int i = y1; i <= y2; i++)
                {
                    if (tileLayer[i, x] == null)
                    {
                        consecutiveWalls = 0;
                    }

                    else if (tileLayer[i, x].GetType() == (new Wall()).GetType())
                    {
                        consecutiveWalls++;
                    }

                    if (consecutiveWalls > 1)
                    {
                        return false;
                    }
                }
            }
            else if (y2 < y1)
            {
                for (int i = y1; i >= y2; i--)
                {
                    if (tileLayer[i, x] == null)
                    {
                        consecutiveWalls = 0;
                    }

                    else if (tileLayer[i, x].GetType() == (new Wall()).GetType())
                    {
                        consecutiveWalls++;
                    }

                    if (consecutiveWalls > 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public void AddHorizontalCorridor(int x1, int x2, int y)
        {
            if (x1 < x2)
            {
                for (int i = x1; i <= x2; i++)
                {
                    if (tileLayer[y, i] == null)
                    {
                        tileLayer[y, i] = new CorridorFloor();
                    }
                    else if (tileLayer[y, i].GetType() == (new Wall()).GetType())
                    {
                        tileLayer[y, i] = new Door(i, y);
                    }
                }
            }
            else if (x2 < x1)
            {
                for (int i = x1; i >= x2; i--)
                {
                    if (tileLayer[y, i] == null)
                    {
                        tileLayer[y, i] = new CorridorFloor();
                    }
                    else if (tileLayer[y, i].GetType() == (new Wall()).GetType())
                    {
                        tileLayer[y, i] = new Door(i, y);
                    }
                }
            }

        }
        public void AddVerticalCorridor(int y1, int y2, int x)
        {
            if (y1 < y2)
            {
                for (int i = y1; i <= y2; i++)
                {
                    if (tileLayer[i, x] == null)
                    {
                        tileLayer[i, x] = new CorridorFloor();
                    }
                    else if (tileLayer[i, x].GetType() == (new Wall()).GetType())
                    {
                        tileLayer[i, x] = new Door(i, x);
                    }
                }
            }
            else if (y2 < y1)
            {
                for (int i = y1; i >= y2; i--)
                {
                    if (tileLayer[i, x] == null)
                    {
                        tileLayer[i, x] = new CorridorFloor();
                    }
                    else if (tileLayer[i, x].GetType() == (new Wall()).GetType())
                    {
                        tileLayer[i, x] = new Door(i, x);
                    }
                }
            }

        }
        public void AddEnemies()
        {
            int numEnemiesInRoom;
            enemies = new List<Enemy>();
            foreach(Room room in rooms)
            {
                if(room.HasEnemies())
                {numEnemiesInRoom = room.GetNumEnemies();
                for(int j = 0;j<numEnemiesInRoom;j++)
                {
                    AddEnemy(room);
                }}
            }
        }
        public void AddEnemy(Room room)
        {
            Enemy enemy = new RandomEnemy(-1,-1,-1,-1,this);
            int random;
            if (difficulty <= 1.5)
            {

                random = rand.Next(0, 3);
                switch (random)
                {
                    case 0:
                        enemy = new VerticalEnemy(-1, -1, 1, 1, this);
                        break;
                    case 1:
                        enemy = new HorizontalEnemy(-1, -1, 1, 1, this);
                        break;
                    case 2:
                        enemy = new RandomEnemy(-1, -1, 1, 1, this);
                        break;
                }
            }
            else if (difficulty <= 2)
            {
                random = rand.Next(0, 4);
                switch (random)
                {
                    case 0:
                        enemy = new VerticalEnemy(-1, -1, 2, 1, this);
                        break;
                    case 1:
                        enemy = new HorizontalEnemy(-1, -1, 2, 1, this);
                        break;
                    case 2:
                        enemy = new RandomEnemy(-1, -1, 1, 1, this);
                        break;
                    case 3:
                        enemy = new TrackingEnemy(-1, -1, 1, 1, this);
                        break;
                }
            }
            else if (difficulty <= 3)
            {
                random = rand.Next(0, 6);
                switch (random)
                {
                    case 0:
                        enemy = new VerticalEnemy(-1, -1, 2, 1, this);
                        break;

                    case 1:
                        enemy = new HorizontalEnemy(-1, -1, 2, 1, this);
                        break;

                    case 2:
                        enemy = new RandomEnemy(-1, -1, 2, 1, this);
                        break;

                    case 3:
                        enemy = new TrackingEnemy(-1, -1, 2, 1, this);
                        break;

                    case 4:
                        enemy = new StaticStraightRangedEnemy(-1, -1, 1, 1, this);
                        break;
                    case 5:
                        enemy = new StaticUniformAOEEnemy(-1, -1, 1, 1, this);
                        break;

                }
            }
            else if (difficulty <= 4)
            {
                random = rand.Next(0, 7);
                switch (random)
                {
                    case 0:
                        enemy = new VerticalEnemy(-1, -1, Convert.ToInt32(1 * difficulty), Convert.ToInt32(difficulty / 2), this);
                        break;

                    case 1:
                        enemy = new HorizontalEnemy(-1, -1, Convert.ToInt32(1 * difficulty), Convert.ToInt32(difficulty / 2), this);
                        break;

                    case 2:
                        enemy = new RandomEnemy(-1, -1, Convert.ToInt32(1 * difficulty), Convert.ToInt32(difficulty / 2), this);
                        break;

                    case 3:
                        enemy = new TrackingEnemy(-1, -1, Convert.ToInt32(1 * difficulty) - 1, Convert.ToInt32(difficulty / 2), this);
                        break;

                    case 4:
                        enemy = new StaticStraightRangedEnemy(-1, -1, Convert.ToInt32(1 * difficulty) - 1, Convert.ToInt32((difficulty / 2) - 0.5), this);
                        break;
                    case 5:
                        enemy = new StaticUniformAOEEnemy(-1, -1, Convert.ToInt32(1 * difficulty) - 1, Convert.ToInt32((difficulty / 2) - 0.5), this);
                        break;
                    case 6:
                        enemy = new TrailDamageTileTrackingEnemy(-1, -1, 1, 1, this, 5);
                        break;

                }
            }
            else if (difficulty <= 5)
            {
                random = rand.Next(0, 7);
                switch (random)
                {
                    case 0:
                        enemy = new VerticalEnemy(-1, -1, Convert.ToInt32(1 * difficulty), 1 + Convert.ToInt32(difficulty / 2), this);
                        break;

                    case 1:
                        enemy = new HorizontalEnemy(-1, -1, Convert.ToInt32(1 * difficulty), 1 + Convert.ToInt32(difficulty / 2), this);
                        break;

                    case 2:
                        enemy = new RandomEnemy(-1, -1, Convert.ToInt32(1 * difficulty), 1 + Convert.ToInt32(difficulty / 2), this);
                        break;
                    case 3:
                        enemy = new SmartTrackingEnemy(-1, -1, Convert.ToInt32(1 * difficulty) - 1, Convert.ToInt32(difficulty / 2), this);
                        break;
                    case 4:
                        enemy = new StaticStraightRangedEnemy(-1, -1, Convert.ToInt32(1 * difficulty) - 1, Convert.ToInt32((difficulty / 2) - 0.5), this);
                        break;
                    case 5:
                        enemy = new StaticUniformAOEEnemy(-1, -1, Convert.ToInt32(1 * difficulty) - 1, Convert.ToInt32((difficulty / 2) - 0.5), this);
                        break;
                    case 6:
                        enemy = new TrailDamageTileTrackingEnemy(-1, -1, 1, 1, this, 5);
                        break;

                }
            }
            else
            {
                random = rand.Next(0, 9);
                switch (random)
                {
                    case 0:
                        enemy = new VerticalEnemy(-1, -1, Convert.ToInt32(1 * difficulty), 1 + Convert.ToInt32(difficulty / 2), this);
                        break;

                    case 1:
                        enemy = new HorizontalEnemy(-1, -1, Convert.ToInt32(1 * difficulty), 1 + Convert.ToInt32(difficulty / 2), this);
                        break;

                    case 2:
                        enemy = new RandomEnemy(-1, -1, Convert.ToInt32(1 * difficulty), 1 + Convert.ToInt32(difficulty / 2), this);
                        break;

                    case 3:
                        enemy = new SmartTrackingEnemy(-1, -1, Convert.ToInt32(1 * difficulty) - 1, Convert.ToInt32(difficulty / 2), this);
                        break;

                    case 4:
                        enemy = new StaticStraightRangedEnemy(-1, -1, Convert.ToInt32(1 * difficulty) - 1, Convert.ToInt32((difficulty / 2) - 0.5), this);
                        break;
                    case 5:
                        enemy = new StaticUniformAOEEnemy(-1, -1, Convert.ToInt32(1 * difficulty) - 1, Convert.ToInt32((difficulty / 2) - 0.5), this);
                        break;
                    case 6:
                        enemy = new TrailDamageTileTrackingEnemy(-1, -1, Convert.ToInt32(1 * difficulty) - 3, Convert.ToInt32((difficulty / 2) - 0.5), this, 3);
                        break;
                    case 7:
                        enemy=new TrackingUniformAOEEnemy(-1,-1,Convert.ToInt32(1 * difficulty) - 4,Convert.ToInt32((difficulty / 2) - 1.5),this);
                        break;
                    case 8:
                        enemy=new TrackingStraightRangedEnemy(-1,-1,Convert.ToInt32(1 * difficulty) - 4,Convert.ToInt32((difficulty / 2) - 1.5),this);
                        break;

                }
            }
            AddEnemyToEntityLayer(room, enemy);

        }
        public bool AddEnemyToEntityLayer(Room room, Enemy enemy)
        {
            int enemyx;
            int enemyy;
            int maxAttempts = 1000;
            int numAttempts = 0;
            while (numAttempts < maxAttempts)
            {
                enemyx = rand.Next(room.x + 1, room.x + room.width - 1);
                enemyy = rand.Next(room.y + 1, room.y + room.height - 1);
                if (!tileLayer[enemyy, enemyx].DoesCollide() && entityLayer[enemyy, enemyx] == null)
                {
                    enemy.x = enemyx;
                    enemy.y = enemyy;
                    entityLayer[enemyy, enemyx] = enemy;
                    enemies.Add(enemy);
                    return true;
                }
            }
            return false;

        }
        public void MazeToAdjacencyList()
        {

            Coord currentCoord;
            coordAdjacencyList = new Dictionary<Coord, List<Coord>>();
            for (int i = 0; i < dunHeight; i++)
            {
                for (int j = 0; j < dunWidth; j++)
                {

                    currentCoord = new Coord(j, i);

                    if (tileLayer[i, j] != null && !tileLayer[i, j].DoesCollide())
                    {
                        coordAdjacencyList.Add(currentCoord, new List<Coord>());

                        if (tileLayer[i + 1, j] != null && !tileLayer[i + 1, j].DoesCollide())
                        {
                            coordAdjacencyList[currentCoord].Add(new Coord(j, i + 1));

                        }
                        if (tileLayer[i - 1, j] != null && !tileLayer[i - 1, j].DoesCollide())
                        {
                            coordAdjacencyList[currentCoord].Add(new Coord(j, i - 1));

                        }
                        if (tileLayer[i, j + 1] != null && !tileLayer[i, j + 1].DoesCollide())
                        {
                            coordAdjacencyList[currentCoord].Add(new Coord(j + 1, i));

                        }
                        if (tileLayer[i, j - 1] != null && !tileLayer[i, j - 1].DoesCollide())
                        {
                            coordAdjacencyList[currentCoord].Add(new Coord(j - 1, i));

                        }
                    }


                }
            }

        }
        public void GenerateWeightMap()
        {
            weightMap = new int[dunHeight,dunWidth];
            for(int i = 0; i < dunHeight; i++)
            {
                for(int j = 0; j < dunWidth; j++)
                {
                    if (tileLayer[i, j] == null)
                    {
                        weightMap[i,j]=1000000;
                    }
                    else if (tileLayer[i, j].DoesCollide())
                    {
                        weightMap[i,j]=1000000;
                    }
                    else if (tileLayer[i, j].GetType() == typeof(Floor))
                    {
                        weightMap[i,j]=1;
                    }
                    else if (tileLayer[i, j].GetType() == typeof(DamageTile))
                    {
                        weightMap[i,j]=100;
                    }
                    else
                    {
                        weightMap[i,j]=1;
                    }
                }
            }
        }
        public Player GetPlayer()
        {
            return player;
        }
        public Dictionary<Coord,List<Coord>> GetCoordAdjacencyList()
        {
            return coordAdjacencyList;
        }
        public List<Item>[,] GetItemLayer()
        {
            return itemLayer;
        }
        public Tile[,] GetTileLayer()
        {
            return tileLayer;
        }
        public Entity[,] GetEntityLayer()
        {
            return entityLayer;
        }
        public DisplayCharacter[,] GetEffectsLayer()
        {
            return effectsLayer;
        }
        public int[,] GetWeightMap()
        {
            return weightMap;
        }
        public List<Enemy> GetEnemies(){
            return enemies;
        }
        public int GetDunWidth()
        {
            return dunWidth;
        }
        public int GetDunHeight()
        {
            return dunHeight;
        }
        public double GetDifficulty()
        {
            return difficulty;
        }
    }
}