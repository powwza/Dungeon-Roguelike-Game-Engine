using System.ComponentModel.DataAnnotations;

namespace goodbyeworld
{
    public abstract class Tile
    {
        protected bool collision;
        public Tile()
        {

        }
        public virtual bool DoesCollide()
        {
            if (collision)
            {
                return true;
            }
            return false;
        }
        public virtual DisplayCharacter Display()
        {
            
            return new DisplayCharacter(" ",ConsoleColor.Black);
        }
    }
    public class DungeonBorder : Tile
    {
        public DungeonBorder()
        {
            collision = true;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("@",ConsoleColor.Black);
        }
    }
    public class Floor : Tile
    {
        public Floor() : base()
        {
            collision = false;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter(" ",ConsoleColor.Black);
        }
    }
    public class CorridorFloor : Tile
    {
        public CorridorFloor()
        {
            collision = false;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter(" ",ConsoleColor.Black);
        }
    }
    public class Wall : Tile
    {
        public Wall()
        {
            collision = true;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter("@",ConsoleColor.Gray);
        }
    }
    public class Door : Tile
    {
        int x;
        int y;
        public Door() : base()
        {
            collision = true;
        }
        public Door(int xin, int yin) : base()
        {
            x = xin;
            y = yin;
            collision = true;
        }
        public void Interact()
        {
            collision = false;
        }
        public override DisplayCharacter Display()
        {
            if (collision)
            { return new DisplayCharacter("X",ConsoleColor.Gray); }
            return new DisplayCharacter("X",ConsoleColor.DarkGray);
        }
    }
    public class Chest : Tile
    {
        Random rand = new Random();
        private int x;
        private int y;
        public Chest() : base()
        {
            collision = true;
        }
        public Chest(int xin, int yin) : base()
        {
            x = xin;
            y = yin;
            collision = true;
        }
        public void OpenChest(Player player, List<Item>[,] itemLayer)
        {
            if (player.GetNumKeys() > 0)
            {
                player.ChangeNumKeys(player.GetNumKeys() - 1);
                //itemLayer[y, x].Add(DropItem());
                if (itemLayer[y, x] == null)
                {
                    itemLayer[y, x] = new List<Item>();
                }
                itemLayer[y, x].Add(DropItem());
                collision = false;
            }


        }
        public Item DropItem()
        {
            int chance = rand.Next(0, 100);
            if (chance < 50)
            {
                List<Item> commonItems = new List<Item> { new SmallHealPotion(1), new StrengthPotion(1, 10),new PassiveAbilityCooldownItem(1),new ActiveAbilityCooldownItem(1) };
                return commonItems[rand.Next(0, commonItems.Count)];
            }
            else if (chance < 80)
            {
                List<Item> uncommonItems = new List<Item> {new Longsword(1), new Broadsword(1),new MediumHealPotion(1),new SmallHealPotion(2), new StrengthPotion(2, 15), new DMGBoostItem(1),new FullHeartItem(1),new PassiveAbilityCooldownItem(2),new ActiveAbilityCooldownItem(2) };
                return uncommonItems[rand.Next(0, uncommonItems.Count)];
            }
            else if (chance < 95)
            {
                List<Item> rareItems = new List<Item> {new SmallHealPotion(3), new MediumHealPotion(2),new LargeHealPotion(1), new StrengthPotion(3, 20), new Broadsword(2),new Longsword(2),new DMGBoostItem(2),new FullHeartItem(2),new PassiveAbilityDurationItem(1),new ActiveAbilityDurationItem(1),new PassiveAbilityCooldownItem(4),new ActiveAbilityCooldownItem(4),new Mace(1) };
                return rareItems[rand.Next(0, rareItems.Count)];
            }
            else if(chance<100)
            {
                List<Item> epicItems = new List<Item> { new Mace(3), new Longsword(3), new FullHeartItem(3),new LargeHealPotion(3),new DMGBoostItem(3),new FullHeartItem(4),new PassiveAbilityDurationItem(3),new ActiveAbilityDurationItem(3) };
                return epicItems[rand.Next(0, epicItems.Count)];
            }
            return new Coin(1);
        }
        public override DisplayCharacter Display()
        {
            if(collision)
            {return new DisplayCharacter("+",ConsoleColor.Yellow);}
            return new DisplayCharacter("+",ConsoleColor.DarkGray);

        }

    }
    public class ExitTile : Tile
    {
        public ExitTile()
        {
            collision = false;
        }
        public void Enter(ref bool isFightingBoss)
        {
            isFightingBoss = true;
        }
        public override DisplayCharacter Display()
        {
            return new DisplayCharacter(":", ConsoleColor.Red);
        }
    }
    public class DamageTile : Tile
    {
        protected bool isEnemyDamageTile;
        protected int dmg;
        public DamageTile(int dmgIn,bool isEnemyDamageTileIn)
        {
            dmg = dmgIn;
            collision = false;
            isEnemyDamageTile=isEnemyDamageTileIn;
        }
        public void DoDamage(LivingEntity entity, Entity[,] entityLayer)
        {
            entity.ChangeHP(entity.GetHP() - dmg, entityLayer);
        }
        public override DisplayCharacter Display()
        {
            if(isEnemyDamageTile)
            {
                return new DisplayCharacter("#", ConsoleColor.DarkRed);
            }
            else
            {
                return new DisplayCharacter("#", ConsoleColor.DarkBlue);
            }
        }
        public bool IsEnemyDamageTile()
        {
            return isEnemyDamageTile;
        }
    }
}