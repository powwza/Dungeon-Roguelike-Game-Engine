namespace goodbyeworld
{
    public abstract class Item
    {
        protected string itemName;
        protected string itemDesc;
        public Item()
        {
            itemName = "ITEM";
            itemDesc = "Nothing";
        }
        public void InventoryDisplay()
        {
            System.Console.WriteLine($"ITEM: {itemName}\nDESCRIPTION: {itemDesc}\n");
        }
        public string GetName()
        {
            return itemName;
        }
        public abstract DisplayCharacter GameDisplay();


    }
    public class Coin : Item
    {
        private int numCoins;
        public Coin()
        {
            itemName = "Coin";
            itemDesc = "Money";
            numCoins = 0;
        }
        public Coin(int numCoinsIn)
        {
            itemName = "Coin";
            itemDesc = "Money";
            numCoins = numCoinsIn;
        }
        public override DisplayCharacter GameDisplay()
        {
            return new DisplayCharacter("$",ConsoleColor.Yellow);
        }
        public int GetNumCoins()
        {
            return numCoins;
        }
        public void ChangeNumCoins(int amount)
        {
            numCoins = amount;
        }
    }
    public class Key : Item
    {
        private int numKeys;
        public Key()
        {
            itemName = "Key";
            itemDesc = "Opens chests";
            numKeys = 0;
        }
        public Key(int numKeysIn)
        {
            itemName = "Key";
            itemDesc = "Opens chests";
            numKeys = numKeysIn;
        }
        public int GetNumKeys()
        {
            return numKeys;
        }
        public void ChangeNumKeys(int amount)
        {
            numKeys = amount;
        }
        public override DisplayCharacter GameDisplay()
        {
            return new DisplayCharacter("€",ConsoleColor.Yellow);
        }
    }
    public abstract class ConsumableItem : Item
    {
        protected int numUses;
        public ConsumableItem(int numUsesIn) : base()
        {
            numUses = numUsesIn;
        }
        public abstract void DoItemEffect(Player player, Dungeon dungeon);
        public void UseItem()
        {
            if(numUses>0)
            {numUses--;}
        }
        public int GetNumUses()
        {
            return numUses;
        }
        public override DisplayCharacter GameDisplay()
        {
            return new DisplayCharacter("$",ConsoleColor.Green);
        }

    }
    public abstract class DurationConsumableItem:ConsumableItem
    {
        protected int duration;
        protected int currentDuration;
        public DurationConsumableItem(int numUsesIn, int durationIn) : base(numUsesIn)
        {
            numUses = numUsesIn;
            duration = durationIn+1;
            currentDuration = 0;
        }
        public  void DecreaseDuration(Player player, Dungeon dungeon)
        {
            if (currentDuration > 0)
            {
                currentDuration--;
                DoItemEffect(player, dungeon);
            }
        }
        
        public int GetDuration()
        {
            return duration;
        }
        public void ChangeCurrentDuration(int amount)
        {
            currentDuration = amount;
        }
        public int GetCurrentDuration()
        {
            return currentDuration;
        }
    }
    public abstract class PassiveStatItem : Item
    {
        public PassiveStatItem() : base()
        {

        }
        public abstract void BoostPlayerStat(Player player, Entity[,] entityLayer);
        public override DisplayCharacter GameDisplay()
        {
            return new DisplayCharacter("€",ConsoleColor.DarkGreen);
        }
    }
}