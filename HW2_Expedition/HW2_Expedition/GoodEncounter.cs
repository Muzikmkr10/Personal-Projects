using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2_Expedition
{
    internal class GoodEncounter: Encounter
    {
        int maxPercent;
        int minPercent;
        string name;

        int MaxPercent { get; set; }
        int MinPercent { get; set; }
        string Name { get; set; }


        public GoodEncounter(int maxPercent, int minPercent, string name): base(maxPercent, minPercent, name)
        {
            this.MaxPercent = maxPercent;
            this.MinPercent = minPercent;
            this.Name = name;
        }

        /// <summary>
        /// Checks if member has enough happiness to stay in party and handles them accordingly
        /// </summary>
        /// <param name="members"></param>
        /// <param name="inventory"></param>
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

        /// <summary>
        /// Calls effect method and returns a bool of whether there is a delay
        /// </summary>
        /// <param name="members"></param>
        /// <param name="inventory"></param>
        /// <returns></returns>
        internal override bool FullEffect(List<PartyMember> members, Inventory inventory)
        {
            switch (Name)
            {
                case "Mardi Gras":
                    TextColors.Encounter($"A {Name} parade happened right in front of you. You and your party members had a great time.\n");
                    break;

                case "Contest Win":
                    TextColors.Encounter($"One of your party members entered and won a contest. What a great surprise.\n");
                    break;

                case "Tea Time":
                    TextColors.Encounter($"{Name} is a great time to hang out with friends.\n");
                    break;
            }

            Effect(members, inventory);
            return false;
        }

        /// <summary>
        /// Affects member happiness positively
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        protected override int AffectHappiness(PartyMember member)
        {
            Random rng = new Random();
            int actualPercent = rng.Next(MinPercent, MaxPercent);
            float affect = ((member.Happiness * actualPercent) / 100);
            if (!(member.Happiness <= 10))
            {
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
            else
            {

                int affects = rng.Next(member.Happiness);
                int tempHappy = member.Happiness + member.Happiness;

                if (tempHappy >= PartyMember.maxHappiness)
                {
                    return PartyMember.maxHappiness;
                }
                else
                {
                    return member.Happiness += affects;
                }

            }
        }

        /// <summary>
        /// Determines whether items are added to inventory and adds them if so
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns></returns>
        protected override List<Item> AffectItems(Inventory inventory)
        {
            List<Item> objects = new List<Item>();
            List<Item> items = inventory.CurrentItems;
            Random random = new Random();

            objects = LoadPossibleItems("AllPossibleItems.txt");

            int numIterations = random.Next((int)Math.Floor((float)(inventory.CurrentItems.Count / 2)));

            for (int i = 0; i < numIterations; i++)
            {
                int j = random.Next((objects.Count - 1));
                if (!(items.Count > inventory.MaxItems))
                {
                    items.Add(objects[j]);
                    TextColors.Encounter($"Added {items.Last().ItemID} to inventory.\n");
                }
                else
                {
                    break;
                }
            }

            return inventory.ManageWholeInventory(items);
        }

        /// <summary>
        /// Loads every possible item from a file in order to potentially add any item at random for the encounter
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private List<Item> LoadPossibleItems(string fileName)
        {
            List<Item> items = new List<Item>();
            StreamReader reader = null;

            try
            {
                reader = new StreamReader("../../../" + fileName);

                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] strings = line.Split(',');

                    if (strings[2] == "true")
                    {
                        items.Add(new Item(strings[0], int.Parse(strings[1]), true));
                    }
                    else
                    {
                        items.Add(new Item(strings[0], int.Parse(strings[1]), false));
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error with loading file: " + e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return items;
        }
    }
}

