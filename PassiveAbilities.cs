namespace goodbyeworld
{
    public class PassiveHeal : PassiveAbility
    {
        protected int healAmount;
        public PassiveHeal(int cooldownIn, int healAmountIn, int durationIn) : base(cooldownIn, durationIn)
        {
            healAmount = healAmountIn;
            abilityName = "Passive Heal";
            abilityDesc = $"Heals {healAmount} HP every {cooldown} moves";
        }
        public override void UseAbility(Dungeon dungeon, Player player)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            player.ChangeHP(player.GetHP() + healAmount, entityLayer);
            currentCooldown = cooldown;
            currentDuration--;
        }
        public void ChangeHeal(int healAmountIn)
        {
            healAmount = healAmountIn;
        }
        public int GetHealAmount()
        {
            return healAmount;
        }
    }
    public class PassiveTemporaryDMGBoost:PassiveAbility
    {
        protected int dmgBoost;
        public PassiveTemporaryDMGBoost(int cooldownIn, int dmgBoostIn, int durationIn):base(cooldownIn,durationIn)
        {
            dmgBoost = dmgBoostIn;
            abilityName = "Passive Temporary Damage Boost";
            abilityDesc = $"Boosts damage by {dmgBoost} for {cooldownIn} turns";
        }
        public override void UseAbility(Dungeon dungeon,Player player)
        {

            if (currentDuration == duration)
            {
                player.ChangeDMG(player.GetDMG() + dmgBoost);
                currentDuration--;
                if (currentDuration == 0)
                {
                    player.ChangeDMG(player.GetDMG() - dmgBoost);
                }
            }
            else if(currentDuration>0)
            {
                currentDuration--;
                if (currentDuration == 0)
                {
                    player.ChangeDMG(player.GetDMG() - dmgBoost);
                }
            }
            currentCooldown = cooldown;
        }
    }
    public class PassiveAOEAttack : PassiveAbility
    {
        protected int dmg;
        public PassiveAOEAttack(int cooldownIn, int dmgIn,int durationIn) : base(cooldownIn,durationIn)
        {
            dmg = dmgIn;
            abilityName = "Passive AOE Attack";
            abilityDesc = "Attacks all of the spaces surrounding the player";
        }
        public override void UseAbility(Dungeon dungeon,Player player)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            if (IsValid(dungeon,player.x, player.y - 1))
            {
                Enemy enemy = (Enemy)entityLayer[player.y - 1, player.x];
                enemy.EnemyAttacked(dmg);
            }
            if (IsValid(dungeon,player.x, player.y + 1))
            {
                Enemy enemy = (Enemy)entityLayer[player.y + 1, player.x];
                enemy.EnemyAttacked(dmg);
            }
            if (IsValid(dungeon,player.x-1, player.y))
            {
                Enemy enemy = (Enemy)entityLayer[player.y, player.x-1];
                enemy.EnemyAttacked(dmg);
            }
            if (IsValid(dungeon,player.x+1, player.y))
            {
                Enemy enemy = (Enemy)entityLayer[player.y, player.x+1];
                enemy.EnemyAttacked(dmg);
            }
            if (IsValid(dungeon,player.x-1, player.y - 1))
            {
                Enemy enemy = (Enemy)entityLayer[player.y - 1, player.x-1];
                enemy.EnemyAttacked(dmg);
            }
            if (IsValid(dungeon,player.x+1, player.y - 1))
            {
                Enemy enemy = (Enemy)entityLayer[player.y - 1, player.x+1];
                enemy.EnemyAttacked(dmg);
            }
            if (IsValid(dungeon,player.x-1, player.y + 1))
            {
                Enemy enemy = (Enemy)entityLayer[player.y + 1, player.x-1];
                enemy.EnemyAttacked(dmg);
            }
            if (IsValid(dungeon, player.x + 1, player.y + 1))
            {
                Enemy enemy = (Enemy)entityLayer[player.y + 1, player.x + 1];
                enemy.EnemyAttacked(dmg);
            }
            currentDuration--;
            currentCooldown = cooldown;
        }
                public bool IsValid(Dungeon dungeon,int x, int y)
        {
            Entity[,] entityLayer = dungeon.GetEntityLayer();
            Tile[,] tileLayer = dungeon.GetTileLayer();
            int height = dungeon.GetDunHeight();
            int width = dungeon.GetDunWidth();
            if (x < 0 || x > width - 1 || y < 0 || y > height - 1)
            {
                return false;
            }
            else if (tileLayer[y, x] == null)
            {
                return false;
            }
            else if (tileLayer[y, x].DoesCollide())
            {
                return false;
            }
            else if (entityLayer[y, x] == null)
            {
                return false;
            }
            if (entityLayer[y, x].GetType().IsSubclassOf(typeof(Enemy)))
            {
                return true;
            }
            return false;
        }
        public void ChangeDMG(int dmgIn)
        {
            dmg = dmgIn;
        }
        public int GetDMG()
        {
            return dmg;
        }
    }
    public class PassiveDamageTileTrail : PassiveAbility
    {
        protected int dmg;
        public PassiveDamageTileTrail(int cooldownIn, int dmgIn,int durationIn) : base(cooldownIn,durationIn)
        {
            dmg=dmgIn;
            abilityName = "Passive Damage Tile Trail";
            abilityDesc = "Place damage tiles passively, and have immunity to your own damage tiles";
        }
        public override void UseAbility(Dungeon dungeon, Player player)
        {
            Tile[,] tileLayer = dungeon.GetTileLayer();
            tileLayer[player.y,player.x]=new DamageTile(dmg,false);
            currentDuration--;
            currentCooldown=cooldown;
        }
    }
}