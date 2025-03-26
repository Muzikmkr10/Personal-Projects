using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2_Expedition
{
    internal class NaturalDisaster: Encounter
    {
        int maxPercent;
        int minPercent;
        string name;

        int MaxPercent { get; set; }
        int MinPercent { get; set; }
        string Name { get; set; }

        public NaturalDisaster(int maxPercent, int minPercent, string name) : base(maxPercent, minPercent, name)
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
                case "Hurricane":
                    TextColors.Encounter("A hurricane blew in and made a huge storm.\n");
                    break;

                case "Sinkhole":
                    TextColors.Encounter("A massive sinkhole formed in the road.\n");
                    break;

                case "Flood":
                    TextColors.Encounter("A levee broke causing a large flood to occur.\n");
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

        internal int DelayGame()
        {
            Random random = new Random();
            return random.Next(5);
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
                    return member.Happiness -= (int)affect;

                }
                else
                {
                    return member.Happiness -= (int)Math.Floor(affect);
                }
            }
            else
            {
                int affects = rng.Next(member.Happiness * 2);
                int tempHappy = member.Happiness + member.Happiness;

                if (tempHappy >= PartyMember.maxHappiness)
                {
                    return PartyMember.maxHappiness;
                }
                else
                {
                    return member.Happiness -= affects;
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
