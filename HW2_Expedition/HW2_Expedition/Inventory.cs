using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HW2_Expedition
{
    internal class Inventory
    {
        //Gets number of members... implemented before understanding properties... didn't have time to fix
        private int numMembers;

        //List of the items that are currently in your inventory
        private List<Item> currentItems;

        //Makes sure that the player cannot have more than 15 items
        private const int maxItems = 15;

        //Makes sure the player can only have 4 party members, implemented when I was going to have a feature to add members, but didn't have time for it
        private int maxPartyMembers = 4;

        //List of members currently in the party
        private List<PartyMember> currentPartyMembers;

        //A list that is used to get the current members of the party
        private List<PartyMember> tempMembers;

        //Shows how much cash player currently has
        internal int currentCash;

        //How much cash the player starts with
        internal const int startCash = 100;

        /// <summary>
        /// Party role indexer
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public PartyMember this[PartyRoles role]
        {
            get 
            { 
                switch (role)
                {
                    case PartyRoles.Trapeze:
                        int i = 0;
                        foreach (PartyMember member in currentPartyMembers)
                        {
                            if (member.Role == PartyRoles.Trapeze)
                            {
                                return member;
                            }
                            i++;
                        }
                        return null;
                    case PartyRoles.Strongman:
                        i = 0;
                        foreach (PartyMember member in currentPartyMembers)
                        {
                            if (member.Role == PartyRoles.Strongman)
                            {
                                return member;
                            }
                            i++;
                        }
                        return null;
                    case PartyRoles.Clown:
                        i = 0;
                        foreach (PartyMember member in currentPartyMembers)
                        {
                            if (member.Role == PartyRoles.Clown)
                            {
                                return member;
                            }
                            i++;
                        }
                        return null;
                    case PartyRoles.BeastTamer:
                        i = 0;
                        foreach (PartyMember member in currentPartyMembers)
                        {
                            if (member.Role == PartyRoles.BeastTamer)
                            {
                                return member;
                            }
                            i++;
                        }
                        return null;
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// returns the current members of your party
        /// </summary>
        public List<PartyMember> CurrentPartyMembers 
        { 
            get 
            { 
                tempMembers = new List<PartyMember>();
                foreach (PartyMember member in currentPartyMembers)
                {
                    tempMembers.Add(member);
                }
                return tempMembers;
            }
        }

        //gets the items in inventory
        public List<Item> CurrentItems
        {
            get
            {
                return currentItems;
            }
        }

        //Gets the max number of items for calculations
        public int MaxItems
        {
            get
            {
                return maxItems;
            }
        }

        //Constructor
        public Inventory()
        {
            currentItems = new List<Item>();
            currentPartyMembers = new List<PartyMember>();
        }

        //Added before properties were understood, and didn't have time to fix
        internal int GetStartCash()
        {
            return startCash;
        }

        //Same as GetStartCash()
        internal List<Item> GetCurrentItems()
        {
            return currentItems;
        }

        //Same as GetStartCash()
        internal int GetNumMembers()
        {
            numMembers = currentPartyMembers.Count;
            return numMembers;
        }

        //Same as GetStartCash()
        internal List<PartyMember> GetPartyMembers()
        {
            return currentPartyMembers;
        }

        /// <summary>
        /// Method that allows for items to be used and affects the paramaterized member, calls appropriate methods
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        internal int UseItem(PartyMember member)
        {
            
            List<Item> items = DisplayUseableInventory();
            if (items.Count > 0)
            {
                Console.WriteLine("What item would you like to use? Type the corresponding number.\nOr you can type (D)escription to see what the item is.");
                string choice = Console.ReadLine().Trim().ToUpper();
                //string descChoice = choice;
                int descChoose;
                int choose;
                if (choice.StartsWith('D'))
                {
                    do
                    {
                        Console.WriteLine("What item would you like to see the description of?\nType the corresponding number.");
                        choice = Console.ReadLine().Trim().ToUpper();
                        while (!int.TryParse(choice, out descChoose))
                        {
                            TextColors.Error("Please enter a number.");
                            choice = Console.ReadLine().Trim().ToUpper();
                        }
                        Console.WriteLine(CurrentItems[descChoose - 1].GetItemDesc());
                        Console.WriteLine("Would you like to see the description of any more items?\nType (D)escription if yes.\nType the number of the item you would like to use if no");
                        choice = Console.ReadLine().Trim().ToUpper();
                    } while (choice.StartsWith('D'));
                }
                while (!int.TryParse(choice, out choose))
                {
                    TextColors.Error("Please enter a number.");
                    choice = Console.ReadLine().Trim().ToUpper();
                }
                int affect = items[choose - 1].AffectHappiness(member);
                currentItems.Remove(items[choose - 1]);
                if ((member.Happiness += affect) > PartyMember.maxHappiness)
                {
                    return PartyMember.maxHappiness;
                }
                return member.Happiness += affect;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Displays the inventory when needed
        /// </summary>
        internal void DisplayInventory()
        {
            Console.WriteLine("Current Items in your inventory:");
            int i = 1;
            foreach (Item item in currentItems)
            { 
                if (i < 10)
                {
                    Console.Write(i + ":  \t"+ item.ItemID + "\n");
                    i++;
                }
                if (i >= 10)
                {
                    Console.Write(i + ": \t" + item.ItemID + "\n");
                    i++;
                }
            }
        }

        /// <summary>
        /// Displays the inventory of useable items... which are currently all of them because I didn't get to unuseable ones
        /// </summary>
        /// <returns></returns>
        internal List<Item> DisplayUseableInventory()
        {
            List<Item> useableInventory = new List<Item>();
            Console.WriteLine("Current useable items in your inventory:");
            int i = 1;
            foreach (Item item in CurrentItems)
            {
                if (item.IsConsumable)
                {
                    if (i < 10)
                    {
                        Console.Write(i + ":  \t" + item.ItemID + "\n");
                        i++;
                    }
                    else if (i >= 10)
                    {
                        Console.Write(i + ": \t" + item.ItemID + "\n");
                        i++;
                    }
                    useableInventory.Add(item);
                }
            }
            return useableInventory;
        }

        /// <summary>
        /// Adds items to inventory after buying or stealing them
        /// </summary>
        /// <param name="item"></param>
        internal void AddToInventory(Item item)
        {
            if (currentItems.Count < maxItems)
            {
                currentItems.Add(item);
            }
            else
            {
                Console.WriteLine("Cannot add " + item.ItemID + " because you have the maximum number of " + maxItems + " items in your inventory.");
                DisplayInventory();
                Console.WriteLine("What would you like to do?\n(1) Replace an item \n(2) Discard the current item");
                ManageInventory(item);
            }
        }

        /// <summary>
        /// Allows for items to be discarded, or replaced if inventory is full, or shows the item description
        /// </summary>
        /// <param name="item"></param>
        internal void ManageInventory(Item item)
        {
            string strChoice = Console.ReadLine().Trim();
            int choice;
            while (!int.TryParse(strChoice, out choice))
            {
                Console.WriteLine("Please type a number.");
                strChoice = Console.ReadLine().Trim();
            }

            switch(choice)
            {
                //Replace item
                case (1):
                    Console.WriteLine("You can choose from: ");
                    DisplayInventory();
                    Console.WriteLine("Type the number of the item you would like to replace.");
                    string strReplace = Console.ReadLine();
                    int replace;
                    int.TryParse(strReplace, out replace);
                    replace = replace - 1;
                    Console.WriteLine("Are you sure you would like to get rid of " + currentItems[replace].ItemID +"? \nType (Y)es/(N)o");
                    string confirm = Console.ReadLine().Trim().ToUpper();
                    if (confirm.StartsWith('Y'))
                    {
                        currentItems.RemoveAt(replace);
                        currentItems.Insert(replace, item);
                    }
                    else if (confirm.StartsWith('N'))
                    {
                        Console.WriteLine("Do you want to:\n(1) Replace a different item\n(2) Discard current item?");
                        ManageInventory(item);
                    }
                    DisplayInventory();
                    break;

                //Discards Item
                case (2):
                    Console.WriteLine("Item discarded");
                    break;
                    
                //Shows Item description
                case (3):
                    Console.WriteLine(item.GetItemDesc());
                    break;
            }
        }

        /// <summary>
        /// Allows for the whole inventory to be managed in backend, from encounters because they have the chance to remove or add multiple items
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        internal List<Item> ManageWholeInventory(List<Item> items)
        {
            return currentItems = items;
        }

        /// <summary>
        /// Allows for player to select members that they want in their party at start
        /// </summary>
        /// <param name="members"></param>
        /// <param name="iteration"></param>
        internal void SelectMembers(List<PartyMember> members, int iteration)
        {
            int i = 1;
            foreach (PartyMember member in members)
            {
                if (i < 10)
                {
                    Console.Write($"{i}:  "); TextColors.Role($"{member.Role}", member); Console.WriteLine($" - {member.Name}");
                }
                else
                {
                    Console.Write($"{i}: "); TextColors.Role($"{member.Role}", member); Console.WriteLine($" - {member.Name}");
                }
                i++;
            }
            Console.WriteLine();
            while (currentPartyMembers.Count < maxPartyMembers)
            {
                string strSelect = Console.ReadLine();
                int select;
                while (!int.TryParse(strSelect, out select))
                {
                    Console.WriteLine("Please enter a number corresponding with the performer you want in your party.");
                    strSelect = Console.ReadLine().Trim().ToUpper();
                }
                select -= 1;
                currentPartyMembers.Add(members[select]);
            }

                for (int x = 0; x < currentPartyMembers.Count; x++)
                {
                    for (int y = 0; y < currentPartyMembers.Count; y++)
                    {
                        if (currentPartyMembers[x].Role == currentPartyMembers[y].Role && currentPartyMembers[x] != currentPartyMembers[y])
                        {
                            currentPartyMembers.Clear();
                            Console.WriteLine("Please select new members making sure that you have one of each role.");
                            SelectMembers(members, 1);
                            
                        }
                    }

                }

            if (iteration == 0)
            {
                Console.WriteLine("\nThe party members selected are: ");
                int j = 0;
                foreach (PartyMember member in currentPartyMembers)
                {
                    Console.WriteLine(currentPartyMembers[j] + ": " + currentPartyMembers[j].Role);
                    j++;
                }
            }

        }

        /// <summary>
        /// Loads performers from a file at start in order to be selected
        /// </summary>
        internal void LoadPerformers()
        {
            List<PartyMember> allPartyMembers = new List<PartyMember>();
            StreamReader reader = null;
            PartyMember tempMember = new PartyMember();

            try
            {
                reader = new StreamReader("../../../PartyMembers.txt");

                //Ask about File.ReadAllText and File.ReadLines
                //string[] lines = null;
                //lines = File.ReadAllLines("../../../TownList.txt");

                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("Trapeze:"))
                    {
                        allPartyMembers.Add(new PartyMember(line.Substring(9), PartyRoles.Trapeze, tempMember.GetHappiness()));
                    }
                    if (line.Contains("Strongman:"))
                    {
                        allPartyMembers.Add(new PartyMember(line.Substring(11), PartyRoles.Strongman, tempMember.GetHappiness()));
                    }
                    if (line.Contains("Clown:"))
                    {
                        allPartyMembers.Add(new PartyMember(line.Substring(7), PartyRoles.Clown, tempMember.GetHappiness()));
                    }
                    if (line.Contains("BeastTamer:"))
                    {
                        allPartyMembers.Add(new PartyMember(line.Substring(12), PartyRoles.BeastTamer, tempMember.GetHappiness()));
                    }
                    if (line == null)
                    {
                        Console.WriteLine("The dev is an idiot.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There's been an error with the file:" + e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            SelectMembers(allPartyMembers, 0);
        }

        /// <summary>
        /// Overloaded method that does the same thing except it does not call Select members method after loading from a save file
        /// </summary>
        /// <param name="fileName"></param>
        internal void ExternalPerformerLoad(string fileName)
        {
            StreamReader reader = null;
            PartyMember tempMember = new PartyMember();

            try
            {
                reader = new StreamReader("../../../" + fileName);

                //Ask about File.ReadAllText and File.ReadLines
                //string[] lines = null;
                //lines = File.ReadAllLines("../../../TownList.txt");

                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    int startIndex = line.IndexOf(':') + 2;
                    int endIndex = line.IndexOf(",");
                    if (line.Contains("Trapeze:"))
                    {

                        currentPartyMembers.Add(new PartyMember(line.Substring(startIndex, endIndex - startIndex), PartyRoles.Trapeze, int.Parse(line.Substring(endIndex + 1))));
                    }
                    if (line.Contains("Strongman:"))
                    {
                        currentPartyMembers.Add(new PartyMember(line.Substring(startIndex, endIndex - startIndex), PartyRoles.Strongman, int.Parse(line.Substring(endIndex + 1))));
                    }
                    if (line.Contains("Clown:"))
                    {
                        currentPartyMembers.Add(new PartyMember(line.Substring(startIndex, endIndex-startIndex), PartyRoles.Clown, int.Parse(line.Substring(endIndex + 1))));
                    }
                    if (line.Contains("BeastTamer:"))
                    {
                        currentPartyMembers.Add(new PartyMember(line.Substring(startIndex, endIndex - startIndex), PartyRoles.BeastTamer, int.Parse(line.Substring(endIndex + 1))));
                    }
                    if (line == null)
                    {
                        Console.WriteLine("The dev is an idiot.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There's been an error with the file:" + e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Loads items from a save file 
        /// </summary>
        /// <param name="fileName"></param>
        internal void ItemLoad(string fileName)
        {
            StreamReader reader = null;

            try
            {
                reader = new StreamReader("../../../" + fileName);

                string line = null;

                while ((line = reader.ReadLine()) != null)
                { 
                    string newLine = line.Substring(line.IndexOf(':') + 2);

                    string[] strings = newLine.Split(',');

                    if (line.Contains("Item:"))
                    {
                        if (strings[2] == "True")
                        {
                            currentItems.Add(new Item(strings[0], int.Parse(strings[1]), true));
                        }
                        else
                        {
                            currentItems.Add(new Item(strings[0], int.Parse(strings[1]), false));
                        }

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error with loading file: " + e.Message);
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }
            }
        }

    }
}
