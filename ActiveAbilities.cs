using System.Security.Authentication;

namespace goodbyeworld
{
    public class ActiveTemporaryDMGBoost:ActiveAbility
    {
        protected int dmgBoost;
        public ActiveTemporaryDMGBoost(int cooldownIn, int dmgBoostIn, int durationIn):base(cooldownIn,durationIn)
        {
            dmgBoost = dmgBoostIn;
            abilityName = "Active Temporary Damage Boost";
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
    public class ActiveHeal : ActiveAbility
    {
        protected int healAmount;
        public ActiveHeal(int cooldownIn, int healAmountIn, int durationIn):base(cooldownIn,durationIn)
        {
            healAmount = healAmountIn;
            abilityName = "Active Heal";
            abilityDesc = $"Heals {healAmount} HP";
        }
        public override void UseAbility(Dungeon dungeon,Player player)
        {

            Entity[,] entityLayer = dungeon.GetEntityLayer();
            if(currentDuration>0)
            {player.ChangeHP(player.GetHP() + healAmount, entityLayer);
                currentCooldown = cooldown;
                currentDuration--;
            }
        }
    }
    public class ActiveAOEAttack : ActiveAbility
    {
        protected int dmg;
        public ActiveAOEAttack(int cooldownIn, int dmgIn, int durationIn) : base(cooldownIn,durationIn)
        {
            dmg = dmgIn;
            abilityName = "Active AOE Attack";
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
                if (IsValid(dungeon,player.x+1, player.y + 1))
                {
                    Enemy enemy = (Enemy)entityLayer[player.y + 1, player.x+1];
                    enemy.EnemyAttacked(dmg);
                }
                Environment.Exit(0);
                currentCooldown = cooldown;
                currentDuration--;
            
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
    }
    public class ActiveDamageTileTrail : ActiveAbility
    {
        protected int dmg;
        public ActiveDamageTileTrail(int cooldownIn, int dmgIn,int durationIn) : base(cooldownIn,durationIn)
        {
            dmg=dmgIn;
            abilityName = "Active Damage Tile Trail";
            abilityDesc = "Place damage tiles, and have immunity to your own damage tiles";
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