using System.Formats.Asn1;
using System.Runtime.CompilerServices;

namespace goodbyeworld
{


    public abstract class Ability
    {
        protected int cooldown;
        protected int currentCooldown;
        protected string abilityName;
        protected string abilityDesc;
        protected int duration;
        protected int currentDuration;
        protected int cooldownCap = 0;
        protected int durationCap = 100;

        public Ability()
        {
            abilityName = "NONE";
            abilityDesc = "NONE";
        }
        public Ability(int cooldownIn, int durationIn)
        {
            abilityName = "NONE";
            abilityDesc = "NONE";
            cooldown = cooldownIn;
            currentCooldown = cooldownIn;
            duration = durationIn+1;
            currentDuration = 0;

        }
        public void Display()
        {
            System.Console.WriteLine($"{abilityName} ABILITY COOLDOWN: {currentCooldown}/{cooldown}, DURATION: {currentDuration}");
        }

        public bool CanUse()
        {

            if (currentCooldown <= 0)
            {
                return true;
            }
            return false;
        }
        public abstract void UseAbility(Dungeon dungeon, Player player);
        public abstract void DecreaseCooldown(Dungeon dungeon, Player player);
        public void ChangeCooldown(int amount)
        {
            if (amount <= cooldownCap)
            {
                cooldown=cooldownCap;
            }
            else
            {
                cooldown=amount;
            }
        }
        public void ChangeDuration(int amount)
        {
            if (amount <= 0)
            {
                duration=0;
            }
            else if(amount>=durationCap)
            {
                duration=durationCap;
            }
            else
            {
                duration=amount;
            }
        }
        public void ChangeCurrentDuration(int currentDurationIn)
        {
            currentDuration = currentDurationIn;
        }
        public int GetDuration()
        {
            return duration;
        }
        public int GetCurrentDuration()
        {
            return currentDuration;
        }
        public int GetCooldown()
        {
            return cooldown;
        }

    }
    public abstract class ActiveAbility : Ability
    {

        public ActiveAbility() : base()
        {
        }
        public ActiveAbility(int cooldownIn, int durationIn) : base(cooldownIn,durationIn)
        {
        }
        public override void DecreaseCooldown(Dungeon dungeon,Player player)
        {
            if (currentCooldown > 0)
            {
                if (currentDuration > 0)
                {
                    UseAbility(dungeon, player);
                }
                else
                {
                    currentCooldown -= 1;
                }
            }
            else
            {
                if (currentDuration > 0)
                {
                    UseAbility(dungeon, player);
                }
            }
            
        }



    }
    public abstract class PassiveAbility : Ability
    {
        public PassiveAbility():base()
        {
        }
        public PassiveAbility(int cooldownIn,int durationIn):base(cooldownIn,durationIn)
        {
        }
        public override void DecreaseCooldown(Dungeon dungeon,Player player)
        {
            if (currentCooldown > 0)
            {
                if (currentDuration > 0)
                {
                    UseAbility(dungeon, player);
                }
                else
                {
                    currentCooldown--;
                }
            }
            else
            {
                currentDuration = duration;
                UseAbility(dungeon,player);
            }
        }

    }

}

