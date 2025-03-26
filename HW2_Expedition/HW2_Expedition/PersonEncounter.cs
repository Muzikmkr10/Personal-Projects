using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2_Expedition
{
    internal class PersonEncounter : Encounter
    {
        int maxPercent;
        int minPercent;
        string name;

        int MaxPercent { get; set; }
        int MinPercent { get; set; }
        string Name { get; set; }

        public PersonEncounter(int maxPercent, int minPercent, string name) : base(maxPercent, minPercent, name)
        {
            this.MaxPercent = maxPercent;
            this.MinPercent = minPercent;
            this.Name = name;
        }

        internal override void Effect(List<PartyMember> members, Inventory inventory)
        {
            foreach (PartyMember member in members)
            {
                AffectHappiness(member);

                TextColors.Role($"{member}'s happiness was affected by the encounter. Their new happiness is {member.Happiness}\n", member);

                if (member.Happiness <= 0)
                {
                    inventory.CurrentPartyMembers.Remove(member);
                    TextColors.Role($"{member} has left your party due to being unhappy with the conditions of the circus.\n", member);
                }

            }
            AffectItems(inventory);
        }

        internal override bool FullEffect(List<PartyMember> members, Inventory inventory)
        {
            switch (Name)
            {
                case "Mugger":
                    TextColors.Encounter("You got approached by a mugger.\n");
                    break;

                case "Voodoo":
                    TextColors.Encounter("You snuck too close to where the black trees grow and met a voodoo lady named Marie Laveau.\n");
                    break;

                case "Give Alms":
                    int money = AffectMoney(inventory);
                    TextColors.Encounter($"You met a homeless man along the way and decided to give them some money, you lost ${money}, but you did a good deed.\n");

                    break;
            }
            Effect(members, inventory);
            int delay = DelayGame();
            if (delay == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Randomly selects whether the party gets delayed
        /// </summary>
        /// <returns></returns>
        internal int DelayGame()
        {
            Random random = new Random();

            switch (Name)
            {
                case "Mugger":
                    return random.Next(7);

                case "Voodoo":
                    return random.Next(7);

                case "Give Alms":
                    return 0;

                default:
                    return 5000;
            }
        }

        /// <summary>
        /// Affects the money the party currently has
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns></returns>
        protected int AffectMoney(Inventory inventory)
        {
            Random rng = new Random();
            int giveMoney = rng.Next(5);
            int money = inventory.currentCash - giveMoney;
            inventory.currentCash = money;
            return giveMoney;
        }

        protected override int AffectHappiness(PartyMember member)
        {
            Random rng = new Random();
            int actualPercent = rng.Next(MinPercent, MaxPercent);
            float affect = ((member.Happiness * actualPercent) / 100);
            if (!(member.Happiness <= 25))
            {
                if (Math.Floor(affect) == affect)
                {
                    switch (Name)
                    {
                        case "Mugger":
                            return member.Happiness -= (int)affect;

                        case "Voodoo":
                            return member.Happiness -= (int)affect;

                        case "Give Alms":
                            int tempHappy = member.Happiness + (int)affect;

                            if (tempHappy >= PartyMember.maxHappiness)
                            {
                                return PartyMember.maxHappiness;
                            }
                            else
                            {
                                return member.Happiness += (int)affect;
                            }

                        default:
                            return 5000;
                    }

                }
                else
                {
                    switch (Name)
                    {
                        case "Mugger":
                            return member.Happiness -= (int)Math.Floor(affect);

                        case "Voodoo":
                            return member.Happiness -= (int)Math.Floor(affect);

                        case "Give Alms":
                            int tempHappy = member.Happiness + (int)affect;

                            if (tempHappy >= PartyMember.maxHappiness)
                            {
                                return PartyMember.maxHappiness;
                            }
                            else
                            {
                                return member.Happiness += (int)Math.Floor(affect);
                            }

                        default:
                            return 5000;
                    }
                }
            }
            else
            {

                int affects = rng.Next(member.Happiness * 2);
                switch (Name)
                {
                    case "Mugger":
                        return member.Happiness -= (affects);

                    case "Voodoo":
                        return member.Happiness -= (affects);

                    case "Give Alms":
                        int tempHappy = member.Happiness + (int)affects;

                        if (tempHappy >= PartyMember.maxHappiness)
                        {
                            return PartyMember.maxHappiness;
                        }
                        else
                        {
                            return member.Happiness += (int)affects;
                        }

                    default:
                        return 5000;
                }
            }
        }

        protected override List<Item> AffectItems(Inventory inventory)
        {
            List<Item> objects = inventory.GetCurrentItems();
            Random random = new Random();

            int numIterations = random.Next((int)Math.Floor((float)((objects.Count / 3) + 1)));

            for (int i = 0; i < numIterations; i++)
            {
                int index = random.Next(objects.Count - 1);
                objects.RemoveAt(index);
            }

            return inventory.ManageWholeInventory(objects);
        }
    }
}
