using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HW2_Expedition
{
    /// <summary>
    /// Enum for party roles
    /// </summary>
    public enum PartyRoles
    {
        Trapeze,
        Strongman,
        Clown,
        BeastTamer
    }

    internal class PartyMember
    {
        //member happiness
        private int happiness;

        //max happiness
        internal const int maxHappiness = 100;

        //minimum happiness
        private const int minHappiness = 0;

        //name of member
        private string name;


        public string Name { get; set; }
        public PartyRoles Role {  get; set; }
        public int Happiness { get; set; }

        //constructor
        public PartyMember(string name, PartyRoles role, int happiness)
        {
            this.Name = name;
            this.Role = role;
            this.Happiness = happiness;
        }

        //overloaded constructor for testing
        public PartyMember()
        {
            
        }

        /// <summary>
        /// Again, done before I understood properties, didn't have time to fix
        /// </summary>
        /// <returns></returns>
        internal int GetHappiness()
        {
            if (happiness == 0)
            {
                happiness = maxHappiness;
            }
            return happiness;
        }

        /// <summary>
        /// Allows member to perform an action based on their role
        /// </summary>
        /// <param name="town"></param>
        /// <param name="inventory"></param>
        internal void PerformAction(Town town, Inventory inventory)
        {
            if (Role == PartyRoles.Trapeze)
            {
                Steal(town, inventory);
            }
            if (Role == PartyRoles.Strongman)
            {
                Fight(town, inventory);
            }
            if (Role == PartyRoles.Clown)
            {
                Show(town, inventory);
            }
            if (Role == PartyRoles.BeastTamer)
            {
                PetShow(town, inventory);
            }
        }

        /// <summary>
        /// Trapeze action, has chance to steal an item or pickpocket someone. If fail loses happiness
        /// </summary>
        /// <param name="town"></param>
        /// <param name="inventory"></param>
        internal void Steal(Town town, Inventory inventory)
        {
            Random select = new Random();
            int chance;
            Item item = town.GetShopItems()[select.Next(town.GetShopItems().Count - 1)];
            Console.WriteLine($"What would you like {Name} to steal?");
            Console.WriteLine($"Your options are:\n1: {item.ItemID}\n2: Try to pickpocket somebody");
            string choice = Console.ReadLine().Trim().ToUpper();
            int choiceIndex;
            while (!int.TryParse(choice, out choiceIndex) || (choiceIndex > 2) && (choiceIndex < 0))
            {
                Console.WriteLine("Please enter a number.");
                choice = Console.ReadLine().Trim().ToUpper();
            }

            if (choiceIndex == 1)
            {
                chance = select.Next(9);
                if (chance < 3)
                {
                    TextColors.Role($"{Name} successfully stole {item.ItemID}.\n", Role);
                    inventory.AddToInventory(item);
                }
                else
                {
                    Random rng = new Random();
                    int actualPercent = rng.Next(1, 10);
                    float affect = ((Happiness * actualPercent) / 100);
                    if (!(Happiness <= 25))
                    {
                        if (Math.Floor(affect) == affect)
                        {
                            Happiness -= (int)affect;

                        }
                        else
                        {
                            Happiness -= (int)Math.Floor(affect);
                        }
                    }
                    else
                    {
                        affect = rng.Next(1, Happiness * 2);

                            Happiness -= (int)affect;
                    }

                    TextColors.Role($"{Name} did not manage to steal {item.ItemID}, and lost {affect} happiness.\n", Role);
                    TextColors.Role($"{Name}'s new happiness is {Happiness}.\n", Role);

                }
            }
            if (choiceIndex == 2)
            {

                chance = select.Next(9);
                if (chance < 5)
                {
                    int stolenCash = select.Next(1, 15);
                    TextColors.Role($"{Name} successfully pickpocketed ${stolenCash}.\n", Role);
                    inventory.currentCash += stolenCash;
                }
                else
                {
                    Random rng = new Random();
                    int actualPercent = rng.Next(1, 10);
                    float affect = ((Happiness * actualPercent) / 100);
                    if (!(Happiness <= 25))
                    {
                        if (Math.Floor(affect) == affect)
                        {
                            Happiness -= (int)affect;

                        }
                        else
                        {
                            Happiness -= (int)Math.Floor(affect);
                        }
                    }
                    else
                    {
                        affect = rng.Next(1, Happiness * 2);

                        Happiness -= (int)affect;
                    }

                    TextColors.Role($"{Name} was caught pickpocketing, and lost {affect} happiness.\n", Role);
                    TextColors.Role($"{Name}'s new happiness is {Happiness}.\n", Role);

                }
            }
        }

        /// <summary>
        /// Strongman action, has chance to get varying amounts of money with more unlikely odds, if fail lose happiness
        /// </summary>
        /// <param name="town"></param>
        /// <param name="inventory"></param>
        internal void Fight(Town town, Inventory inventory)
        {
            Random random = new Random();
            Console.WriteLine($"Who would you like {Name} to fight?");
            Console.WriteLine($"Your options are:\n1: Local drunk\n2: Local boxer");
            string choice = Console.ReadLine().Trim().ToUpper();
            int choiceIndex;
            while (!int.TryParse(choice, out choiceIndex) || (choiceIndex > 2) && (choiceIndex < 0))
            {
                Console.WriteLine("Please enter a number.");
                choice = Console.ReadLine().Trim().ToUpper();
            }

            if (choiceIndex == 1)
            {

                int chance = random.Next(9);
                if (chance < 7)
                {
                    int winnings = random.Next(1, 10);
                    TextColors.Role($"{Name} successfully beat the local drunk and won ${winnings}.\n", Role);
                    inventory.currentCash += winnings;
                }
                else
                {
                    Random rng = new Random();
                    int actualPercent = rng.Next(1, 10);
                    float affect = ((Happiness * actualPercent) / 100);
                    if (!(Happiness <= 25))
                    {
                        if (Math.Floor(affect) == affect)
                        {
                            Happiness -= (int)affect;

                        }
                        else
                        {
                            Happiness -= (int)Math.Floor(affect);
                        }
                    }
                    else
                    {
                        affect = rng.Next(1, Happiness * 2);

                        Happiness -= (int)affect;
                    }

                    TextColors.Role($"{Name} lost the fight against the drunk, and lost {affect} happiness.\n", Role);
                    TextColors.Role($"{Name}'s new happiness is {Happiness}.\n", Role);
                }
            }
            if (choiceIndex == 2)
            {

                int chance = random.Next(9);
                if (chance < 3)
                {
                    int winnings = random.Next(10, 25);
                    TextColors.Role($"{Name} successfully beat the local boxer and won ${winnings}\n", Role);
                    inventory.currentCash += winnings;
                }
                else
                {
                    Random rng = new Random();
                    int actualPercent = rng.Next(1, 10);
                    float affect = ((Happiness * actualPercent) / 100);
                    if (!(Happiness <= 25))
                    {
                        if (Math.Floor(affect) == affect)
                        {
                            Happiness -= (int)affect;

                        }
                        else
                        {
                            Happiness -= (int)Math.Floor(affect);
                        }
                    }
                    else
                    {
                        affect = rng.Next(1, Happiness * 2);

                        Happiness -= (int)affect;
                    }

                    TextColors.Role($"{Name} lost the fight against the boxer, and lost {affect} happiness.\n", Role);
                    TextColors.Role($"{Name}'s new happiness is {Happiness}.\n", Role);
                }
            }
        }

        /// <summary>
        /// Clown action, has chance to get varying amounts of money with more unlikely odds, if fail lose happiness
        /// </summary>
        /// <param name="town"></param>
        /// <param name="inventory"></param>
        internal void Show(Town town, Inventory inventory)
        {
            Random random = new Random();
            Console.WriteLine($"What would you like {Name} to perform?");
            Console.WriteLine($"Your options are:\n1: Juggle\n2: Comedy");
            string choice = Console.ReadLine().Trim().ToUpper();
            int choiceIndex;
            while (!int.TryParse(choice, out choiceIndex))
            {
                Console.WriteLine("Please enter a number.");
                choice = Console.ReadLine().Trim().ToUpper();
            }

            if (choiceIndex == 1)
            {

                int chance = random.Next(9);
                if (chance < 6)
                {
                    int winnings = random.Next(1, 15);
                    TextColors.Role($"{Name} put on a great juggling show and won ${winnings}.\n", Role);
                    inventory.currentCash += winnings;
                }
                else
                {
                    Random rng = new Random();
                    int actualPercent = rng.Next(1, 10);
                    float affect = ((Happiness * actualPercent) / 100);
                    if (!(Happiness <= 25))
                    {
                        if (Math.Floor(affect) == affect)
                        {
                            Happiness -= (int)affect;

                        }
                        else
                        {
                            Happiness -= (int)Math.Floor(affect);
                        }
                    }
                    else
                    {
                        affect = rng.Next(1, Happiness * 2);

                        Happiness -= (int)affect;
                    }

                    TextColors.Role($"{Name} was uncoordinated, made a mess of everything, and lost {affect} happiness.\n", Role);
                    TextColors.Role($"{Name}'s new happiness is {Happiness}.\n", Role);
                }
            }
            if (choiceIndex == 2)
            {

                int chance = random.Next(9);
                if (chance < 4)
                {
                    int winnings = random.Next(5, 20);
                    TextColors.Role($"{Name} put on a hilarious show and won ${winnings}.\n", Role);
                    inventory.currentCash += winnings;
                }
                else
                {
                    Random rng = new Random();
                    int actualPercent = rng.Next(1, 10);
                    float affect = ((Happiness * actualPercent) / 100);
                    if (!(Happiness <= 25))
                    {
                        if (Math.Floor(affect) == affect)
                        {
                            Happiness -= (int)affect;

                        }
                        else
                        {
                            Happiness -= (int)Math.Floor(affect);
                        }
                    }
                    else
                    {
                        affect = rng.Next(1, Happiness * 2);

                        Happiness -= (int)affect;
                    }

                    TextColors.Role($"{Name} was a nervous wreck on stage, and lost {affect} happiness.\n", Role);
                    TextColors.Role($"{Name}'s new happiness is {Happiness}.\n", Role);
                }
            }
        }

        /// <summary>
        /// Beast Tamer action, has chance to get varying amounts of money with more unlikely odds, if fail lose happiness
        /// </summary>
        /// <param name="town"></param>
        /// <param name="inventory"></param>
        internal void PetShow(Town town, Inventory inventory)
        {
            Random rand = new Random();
            Console.WriteLine($"What would you like {Name} to let the people of {town} pet?");
            Console.WriteLine($"Your options are:\n1: Elephant\n2: Tigers\n");
            string choice = Console.ReadLine().Trim().ToUpper();
            int choiceIndex;
            while (!int.TryParse(choice, out choiceIndex))
            {
                Console.WriteLine("Please enter a number.");
                choice = Console.ReadLine().Trim().ToUpper();
            }

            if (choiceIndex == 1)
            {

                int chance = rand.Next(9);
                if (chance < 6)
                {
                    int winnings = rand.Next(1, 20);
                    TextColors.Role($"{Name} let many people pet the elephant and raised ${winnings}.\n", Role);
                    inventory.currentCash += winnings;
                }
                else
                {
                    Random rng = new Random();
                    int actualPercent = rng.Next(1, 10);
                    float affect = ((Happiness * actualPercent) / 100);
                    if (!(Happiness <= 25))
                    {
                        if (Math.Floor(affect) == affect)
                        {
                            Happiness -= (int)affect;

                        }
                        else
                        {
                            Happiness -= (int)Math.Floor(affect);
                        }
                    }
                    else
                    {
                        affect = rng.Next(1, Happiness * 2);

                        Happiness -= (int)affect;
                    }

                    TextColors.Role($"{Name} didn't contain the elephant securely enough, and lost {affect} happiness.\n", Role);
                    TextColors.Role($"{Name}'s new happiness is {Happiness}.\n", Role);
                }
            }
            if (choiceIndex == 2)
            {

                int chance = rand.Next(9);
                if (chance < 4)
                {
                    int winnings = rand.Next(5, 25);
                    TextColors.Role($"{Name} let many people pet the tigers and raised ${winnings}.\n", Role);
                    inventory.currentCash += winnings;
                }
                else
                {
                    Random rng = new Random();
                    int actualPercent = rng.Next(1, 10);
                    float affect = ((Happiness * actualPercent) / 100);
                    if (!(Happiness <= 25))
                    {
                        if (Math.Floor(affect) == affect)
                        {
                            Happiness -= (int)affect;

                        }
                        else
                        {
                            Happiness -= (int)Math.Floor(affect);
                        }
                    }
                    else
                    {
                        affect = rng.Next(1, Happiness * 2);

                        Happiness -= (int)affect;
                    }

                    TextColors.Role($"{Name} had the tigers escape, and lost {affect} happiness.\n", Role);
                    TextColors.Role($"{Name}'s new happiness is {Happiness}.\n", Role);
                }
            }
        }

        //Mostly for ease of use in the debugger
        public override string ToString()
        {
            return Name;
        }
    }
}
