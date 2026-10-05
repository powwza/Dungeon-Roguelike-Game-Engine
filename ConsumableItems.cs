namespace goodbyeworld
{
    public class SmallHealPotion : ConsumableItem
    {
        public SmallHealPotion(int numUsesIn) : base(numUsesIn)
        {
            itemName = "Small Healing Potion";
            itemDesc = "Heals a heart";
        }
        public override void DoItemEffect(Player player, Dungeon dungeon)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            if (numUses > 0)
            {
                player.ChangeHP(player.GetHP() + 1, entityLayer);
            }

        }
    }
    public class MediumHealPotion : ConsumableItem
    {
        public MediumHealPotion(int numUsesIn) : base(numUsesIn)
        {
            itemName = "Medium Healing Potion";
            itemDesc = "Heals two hearts";
        }
        public override void DoItemEffect(Player player, Dungeon dungeon)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            if (numUses > 0)
            {
                player.ChangeHP(player.GetHP() + 2, entityLayer);
            }

        }
    }
    public class LargeHealPotion : ConsumableItem
    {
        public LargeHealPotion(int numUsesIn) : base(numUsesIn)
        {
            itemName = "Large Healing Potion";
            itemDesc = "Heals three hearts";
        }
        public override void DoItemEffect(Player player, Dungeon dungeon)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            if (numUses > 0)
            {
                player.ChangeHP(player.GetHP() + 3, entityLayer);
            }
        }
    }
    public class StrengthPotion : DurationConsumableItem
    {
        public StrengthPotion(int numUsesIn, int durationIn) : base(numUsesIn, durationIn)
        {
            itemName = "Strength Potion";
            itemDesc = "Increases damage by 1";
        }
        public override void DoItemEffect(Player player, Dungeon dungeon)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            if (currentDuration == duration)
            {
                player.ChangeDMG(player.GetDMG() + 1);
            }
            else if (currentDuration == 0)
            {
                player.ChangeDMG(player.GetDMG() - 1);
            }
            
        }
    }
    public class RegenerationPotion : DurationConsumableItem
    {
        protected int healDelay;
        public RegenerationPotion(int numUsesIn, int durationIn,int healDelayIn) : base(numUsesIn, durationIn)
        {
            itemName = "Regeneration Potion";
            itemDesc = $"Heals player by 1 every {healDelayIn} moves";
            healDelay=healDelayIn;
        }
        public override void DoItemEffect(Player player, Dungeon dungeon)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            if (currentDuration % healDelay==0)
            {
                player.ChangeHP(player.GetHP() + 1,entityLayer);
            }
        }
    }
    public class ActiveAbilityDurationItem : ConsumableItem
    {
        public ActiveAbilityDurationItem(int numUsesIn) : base(numUsesIn)
        {
            itemName = "Active ability duration item";
            itemDesc = "Increases duration of your active ability by 1";
        }
        public override void DoItemEffect(Player player, Dungeon dungeon)
        {
            player.GetActiveAbility().ChangeDuration(player.GetActiveAbility().GetDuration()+1);
        }
    }
    public class PassiveAbilityDurationItem : ConsumableItem
    {
        public PassiveAbilityDurationItem(int numUsesIn) : base(numUsesIn)
        {
            itemName = "Passive ability duration item";
            itemDesc = "Increases duration of your passive ability by 1";
        }
        public override void DoItemEffect(Player player, Dungeon dungeon)
        {
            player.GetPassiveAbility().ChangeDuration(player.GetPassiveAbility().GetDuration()+1);
        }
    }
    public class ActiveAbilityCooldownItem : ConsumableItem
    {
        public ActiveAbilityCooldownItem(int numUsesIn) : base(numUsesIn)
        {
            itemName = "Active ability cooldown item";
            itemDesc = "Decreases coolown of your active ability by 1";
        }
        public override void DoItemEffect(Player player, Dungeon dungeon)
        {
            player.GetActiveAbility().ChangeCooldown(player.GetActiveAbility().GetCooldown()-1);
            if (player.GetActiveAbility().GetCurrentDuration() > player.GetActiveAbility().GetDuration())
            {
                player.GetActiveAbility().ChangeCurrentDuration(player.GetActiveAbility().GetDuration());
            }
        }
    }
    public class PassiveAbilityCooldownItem : ConsumableItem
    {
        public PassiveAbilityCooldownItem(int numUsesIn) : base(numUsesIn)
        {
            itemName = "Passive ability cooldown item";
            itemDesc = "Decreases coolown of your passive ability by 1";
        }
        public override void DoItemEffect(Player player, Dungeon dungeon)
        {
            player.GetPassiveAbility().ChangeCooldown(player.GetPassiveAbility().GetCooldown()-1);
            if (player.GetPassiveAbility().GetCurrentDuration() > player.GetPassiveAbility().GetDuration())
            {
                player.GetPassiveAbility().ChangeCurrentDuration(player.GetPassiveAbility().GetDuration());
            }
        }
    }
}



