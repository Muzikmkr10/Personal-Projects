using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2_Expedition
{
    /// <summary>
    /// Base class for each encounter class to polymorphize
    /// </summary>
    internal class Encounter
    {
        //Max percent of happiness encounter can affect
        int maxPercent;

        //minimum percent of happiness encounter can affect
        int minPercent;

        //Name of encounter
        string name;

        int MaxPercent { get; set; }
        int MinPercent { get; set; }
        string Name {  get; set; }

        //Constructor
        public Encounter(int maxPercent, int minPercent, string name)
        {
            this.MaxPercent = maxPercent;
            this.MinPercent = minPercent;
            this.Name = name;
        }

        /// <summary>
        /// Calls the methods that affect happiness and items
        /// </summary>
        /// <param name="members"></param>
        /// <param name="inventory"></param>
        internal virtual void Effect(List<PartyMember> members, Inventory inventory)
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

        /// <summary>
        /// Method that returns if there is a delay
        /// </summary>
        /// <param name="members"></param>
        /// <param name="inventory"></param>
        /// <returns></returns>
        internal virtual bool FullEffect(List<PartyMember> members, Inventory inventory)
        {
            return false;
        }

        /// <summary>
        /// Takes the member happiness and calculates what percentage of happiness is
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        protected virtual int AffectHappiness(PartyMember member)
        {
            Random rng = new Random();
            int actualPercent = rng.Next(MinPercent, MaxPercent) ;
            float affect = ((member.Happiness * actualPercent) / 100);
            if (Math.Floor(affect) == affect)
            {
                int tempHappy = member.Happiness + (int)affect;

                if (tempHappy >= PartyMember.maxHappiness)
                {
                    return PartyMember.maxHappiness;
                }
                else
                {
                    return member.Happiness += (int)affect;
                }

            }
            else
            {
                int tempHappy = member.Happiness + (int)affect;

                if (tempHappy >= PartyMember.maxHappiness)
                {
                    return PartyMember.maxHappiness;
                }
                else
                {
                    return member.Happiness += (int)affect;
                }
            }
        }

        /// <summary>
        /// Affects the items in current inventory
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns></returns>
        protected virtual List<Item> AffectItems(Inventory inventory)
        {
            List<Item> objects = new List<Item>();
            Random random = new Random();

            foreach (Item item in inventory.CurrentItems)
            {
                objects.Add(item);
            }
            
            int numIterations = random.Next((int)Math.Floor((float)((objects.Count / 3) + 1)));

            for (int i = 0; i < numIterations; i++)
            {
                int index = random.Next(objects.Count - 1);
                TextColors.Encounter($"Removed {objects[index]} from your inventory\n");
                objects.RemoveAt(index);
            }

            return inventory.ManageWholeInventory(objects);
        }
    }
}
