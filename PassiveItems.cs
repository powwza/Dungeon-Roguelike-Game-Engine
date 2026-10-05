namespace goodbyeworld
{
    public class FullHeartItem : PassiveStatItem
    {
        protected int extraHearts;
        public FullHeartItem(int extraHeartsIn) : base()
        {
            extraHearts = extraHeartsIn;
            itemName = $"{extraHearts} Full Heart(s)";
            itemDesc = $"Gives you {extraHearts} extra full heart(s)";
        }
        public override void BoostPlayerStat(Player player, Entity[,] entityLayer)
        {
            player.ChangeMaxHP(player.GetMaxHP() + extraHearts);
            player.ChangeHP(player.GetHP() + extraHearts, entityLayer);
        }
        

    }
    public class DMGBoostItem : PassiveStatItem
    {
        protected int dmgBoost;
        public DMGBoostItem(int dmgIn) : base()
        {
            dmgBoost = dmgIn;
            itemName = "Damage boost";
            itemDesc = $"Gives you {dmgBoost} extra damage";
        }
        public override void BoostPlayerStat(Player player, Entity[,] entityLayer)
        {
            player.ChangeDMG(player.GetDMG() + dmgBoost);
        }
    }
}