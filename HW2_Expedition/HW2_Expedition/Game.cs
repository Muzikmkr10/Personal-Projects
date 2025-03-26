using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;

namespace HW2_Expedition
{
    internal class Game
    {
        //Queue to hold the order of towns to travel to
        internal Queue<Town> townOrder = new Queue<Town>();
        //List to load the towns into, in order to select them at random
        private List<string> towns = new List<string>();
        //A list of the visited towns, allows for saving progress and tracking what "day" the player is on
        internal List<Town> visitedTowns = new List<Town>();
        //Declare a player inventory
        Inventory inventory = new Inventory();
        //List of encounters to be selected at random, employs polymorphism
        private List<Encounter> encounters = new List<Encounter>()
        {
            new GoodEncounter(15, 10, "Mardi Gras"),
            new GoodEncounter(10, 6, "Contest Win"),
            new GoodEncounter(5, 2, "Tea Time"),

            new PersonEncounter(45, 30, "Mugger"),
            new PersonEncounter(40, 25, "Voodoo"),
            new PersonEncounter(5, 2, "Give Alms"),

            new NaturalDisaster(45, 30, "Hurricane"),
            new NaturalDisaster(40, 25, "Sinkhole"),
            new NaturalDisaster(35, 20, "Flood")
        };

        /// <summary>
        /// Game loop method that calls the required methods for the game to run
        /// </summary>
        internal void GameLoop()
        {

            inventory.currentCash = inventory.GetStartCash();
            int numTowns = townOrder.Count;
            if (numTowns > visitedTowns.Count || visitedTowns == null)
            {
                TextColors.Title("\tWelcome to The Travelling Circus\n------------------------------------------------\n\n");
                Console.WriteLine("You have to get through ten different towns in order to get to your final performance location.\nYou need to get there with at least one remaining party member, otherwise there will be no show to put on.");
                Console.WriteLine("Do you have a file you want to load? (Y)es/(N)o");
                string choice = Console.ReadLine().Trim().ToUpper();
                while (choice == null || !choice.StartsWith('Y') && !choice.StartsWith('N'))
                {
                    Console.WriteLine("Please enter either (Y)es or (N)o");
                    choice = Console.ReadLine().Trim().ToUpper();
                }
                if (choice.StartsWith('Y'))
                {
                    LoadGame();
                }

                //Cycle one, selecting members and getting started in the first town
                while (visitedTowns.Count == 0)
                {
                    Console.Write($"You currently have "); TextColors.Inventory($"{inventory.CurrentItems.Count}"); Console.Write(" items and "); TextColors.Inventory($"{inventory.CurrentPartyMembers.Count}"); Console.Write(" party members.\n");
                    Console.WriteLine("Please select the four party members you would like to have.\nYou need to have at least one of each type.\nTo select, type the number next to the character you want. Then hit enter, and repeat.\n");
                    inventory.LoadPerformers();
                    Console.WriteLine("\nYou now have "); TextColors.Inventory($"{inventory.GetNumMembers()}"); Console.Write(" members\n");
                    Console.WriteLine("Would you like to: \n(C)ontinue and start the game?\n(S)ave your progress and quit?\n(Q)uit without saving?");
                    string cont = Console.ReadLine().Trim().ToUpper();
                    while (cont == null || !cont.StartsWith('C') && !cont.StartsWith('S') && !cont.StartsWith('Q'))
                    {
                        Console.WriteLine("Please choose an option between:\n(C)ontinue\n(S)ave\n(Q)uit");
                        cont = Console.ReadLine().Trim().ToUpper();
                    }
                    if (cont.StartsWith('C'))
                    {
                        visitedTowns.Add(townOrder.Dequeue());
                        break;
                    }
                    else if (cont.StartsWith('S'))
                    {
                        LoadGame(SaveGame());
                        System.Environment.Exit(0);
                    }
                    else if (cont.StartsWith('Q'))
                    {
                        System.Environment.Exit(0);
                    }
                }

                //A variable that prevents loading store inventory more than once
                //Gets incremented if the party gets delayed
                int iteration = 0;
                //Game loops, each cycle has three parts
                while (visitedTowns.Count == 1)
                {
                    iteration = DayCycle(iteration);
                    NightCycle(Between());
                }

                iteration = 0;
                while (visitedTowns.Count == 2)
                {
                    iteration = DayCycle(iteration);                    
                    NightCycle(Between());
                }

                iteration = 0;
                while (visitedTowns.Count == 3)
                {
                    iteration = DayCycle(iteration);                    
                    NightCycle(Between());
                }

                iteration = 0;
                while (visitedTowns.Count == 4)
                {
                    iteration = DayCycle(iteration);                    
                    NightCycle(Between());
                }

                iteration = 0;
                while (visitedTowns.Count == 5)
                {
                    iteration = DayCycle(iteration);                    
                    NightCycle(Between());
                }

                iteration = 0;
                while (visitedTowns.Count == 6)
                {
                    iteration = DayCycle(iteration);
                    NightCycle(Between());
                }

                iteration = 0;
                while (visitedTowns.Count == 7)
                {
                    iteration = DayCycle(iteration);
                    NightCycle(Between());
                }

                iteration = 0;
                while (visitedTowns.Count == 8)
                {
                    iteration = DayCycle(iteration);
                    NightCycle(Between());
                }

                iteration = 0;
                while (visitedTowns.Count == 9)
                {
                    iteration = DayCycle(iteration);
                    NightCycle(Between());
                }

                iteration = 0;
                while (visitedTowns.Count == 10)
                {
                    iteration = DayCycle(iteration);
                    NightCycle(Between());
                }

                //Win state and final words
                while (visitedTowns.Count == 11)
                {
                    if (inventory.CurrentPartyMembers.Count >= 1)
                    {
                        Console.Write("Congratulations! You made it to your destination with "); TextColors.Inventory($"{inventory.CurrentPartyMembers.Count}"); Console.WriteLine(" party members left.");
                        Console.WriteLine("You put on a great show and you win the game!");
                        Console.WriteLine("Thanks for playing!");
                        System.Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("You did not make it with any party members, that means you can't perform a show. You can try again though.");
                        Console.WriteLine("Thanks for playing!");
                        System.Environment.Exit(0);
                    }
                }
            }
        }

        /// <summary>
        /// Switch statement that changes up the text depending on how many towns have been passed. Calls the shop interaction methods
        /// </summary>
        /// <param name="iteration"></param>
        /// <returns></returns>
        internal int DayCycle(int iteration)
        {
            if (inventory.CurrentPartyMembers.Count == 0)
            {
                Console.WriteLine("You have no remaining party members, that means you can't perform a show. You can try again though.");
                Console.WriteLine("Thanks for playing!");
                System.Environment.Exit(0);
            }
            int day = visitedTowns.Count;
            switch (day)
            {
                case 1:
                    TextColors.Normal("The first stop on our trip is ");
                    break;

                case 2:
                    TextColors.Normal("The second stop on our trip is ");
                    break;

                case 3:
                    TextColors.Normal("The next stop on our trip is ");
                    break;

                case 4:
                    TextColors.Normal("The fourth stop on our trip is ");
                    break;

                case 5:
                    TextColors.Normal("The fifth stop on our trip is ");
                    break;

                case 6:
                    TextColors.Normal("The next stop on our trip is ");
                    break;

                case 7:
                    TextColors.Normal("Our next stop is ");
                    break;

                case 8:
                    TextColors.Normal("The eighth stop on our trip is ");
                    break;

                case 9:
                    TextColors.Normal("The next stop on our trip is ");
                    break;

                case 10:
                    TextColors.Normal("Our final stop and destination is ");
                    break;
            }
            TextColors.Towns(visitedTowns[visitedTowns.Count - 1].ToString() + "\n");
            TextColors.Normal(visitedTowns[visitedTowns.Count - 1].TownDescription() + "\n");
            StoreInteraction(iteration);
            return iteration+1;
        }

        /// <summary>
        /// Between method that allows player to use items and calls methods accordingly. Then selects the encounter from the list of possible encounters. Determines if there is a delay caused by the encounter. Allows for player to save and/or exit game
        /// </summary>
        /// <returns></returns>
        internal bool Between()
        {
            List<PartyMember> members = new List<PartyMember>();
            if (visitedTowns.Count > 1)
            {
                Console.Write("Would you like to use an item? You currently have "); TextColors.Inventory($"{inventory.CurrentItems.Count}"); Console.WriteLine(" items. (Y)es/(N)o");
                string choice = Console.ReadLine().Trim().ToUpper();
                while (string.IsNullOrEmpty(choice))
                {
                    Console.WriteLine("Please enter (Y) or (N)");
                    choice = Console.ReadLine().Trim().ToUpper();
                }
                if (choice.StartsWith('Y'))
                {
                    if (inventory.CurrentItems.Count > 0)
                    {
                        Console.Write("How many items would you like to use? You can use one per party member.\nYou currently have "); TextColors.Inventory($"{inventory.CurrentPartyMembers.Count}"); Console.WriteLine(" party members.");
                        int i = 1;
                        foreach (PartyMember member in inventory.CurrentPartyMembers)
                        {
                            Console.Write($"{i}: "); TextColors.Role($"{member.Role}", member); Console.WriteLine($" - {member.Name}: Happiness - {member.Happiness}");
                            i++;
                        }
                        string strchoose = Console.ReadLine().Trim().ToUpper();
                        int choose;
                        while (!int.TryParse(strchoose, out choose) || inventory.CurrentItems.Count < choose)
                        {
                            TextColors.Error("Please enter a number that is less than or equal to the amount of items you have.\n");
                            strchoose = Console.ReadLine().Trim().ToUpper();
                        }
                        Console.WriteLine("Which party members would you like to use an item?");
                        i = 1;
                        foreach (PartyMember member in inventory.CurrentPartyMembers)
                        {
                            Console.Write($"{i}: "); TextColors.Role($"{member.Role}", member); Console.WriteLine($" - {member.Name}: Happiness - {member.Happiness}");
                            i++;
                        }
                        while (members.Count < choose)
                        {
                            string strSelect = Console.ReadLine();
                            int select;
                            while (!int.TryParse(strSelect, out select) || (select - 1) >= inventory.CurrentPartyMembers.Count)
                            {
                                TextColors.Error($"Please enter a number 1 - {inventory.CurrentPartyMembers.Count}\n");
                                strSelect = Console.ReadLine().Trim().ToUpper();
                            }
                            select -= 1;

                            members.Add(inventory.CurrentPartyMembers[select]);
                        }
                        foreach (PartyMember member in members)
                        {
                            inventory.UseItem(member);
                            TextColors.Role($"{member.Name}'s new happiness is {member.Happiness}\n", member);
                        }
                    }
                }
            }

            bool delay = GetEncounter();
            if (!delay)
            {
                TextColors.Normal("Would you like to:\n(C)ontinue?\n(S)ave your progress and continue?\nSave your (P)rogress and quit?\n(Q)uit without saving?\n");
                string cont = Console.ReadLine().Trim().ToUpper();
                while (cont == null || !cont.StartsWith('C') && !cont.StartsWith('S') && !cont.StartsWith('Q') && !cont.StartsWith('P'))
                {
                    TextColors.Normal("Please choose an option between:\n(C)ontinue\n(S)ave\n(Q)uit\n");
                    cont = Console.ReadLine().Trim().ToUpper();
                }
                if (cont.StartsWith('C'))
                {
                    visitedTowns.Add(townOrder.Dequeue());
                    return delay;
                }
                else if (cont.StartsWith('S'))
                {
                    visitedTowns.Add(townOrder.Dequeue());
                    LoadGame(SaveGame());
                    return delay;
                }
                else if (cont.StartsWith('P'))
                {
                    visitedTowns.Add(townOrder.Dequeue());
                    SaveGame();
                    System.Environment.Exit(0);
                    return delay;
                }
                else
                {
                    System.Environment.Exit(0);
                    return delay;
                }
            }

            else
            {
                TextColors.Normal("Would you like to:\n(C)ontinue?\n(S)ave your progress and continue?\nSave your (P)rogress and quit?\n(Q)uit without saving?\n");
                string cont = Console.ReadLine().Trim().ToUpper();
                while (cont == null || !cont.StartsWith('C') && !cont.StartsWith('S') && !cont.StartsWith('Q') && !cont.StartsWith('P'))
                {
                    TextColors.Normal("Please choose an option between:\n(C)ontinue\n(S)ave\n(Q)uit\n");
                    cont = Console.ReadLine().Trim().ToUpper();
                }
                if (cont.StartsWith('C'))
                {
                        return delay;
                }
                else if (cont.StartsWith('S'))
                {
                    LoadGame(SaveGame());
                    return delay;
                }
                else if (cont.StartsWith('P'))
                {
                    SaveGame();
                    System.Environment.Exit(0);
                    return delay;
                }
                else
                {
                    System.Environment.Exit(0);
                    return delay;
                }
            }

        }

        /// <summary>
        /// Allows player to use performer actions. 
        /// </summary>
        /// <param name="delay"></param>
        internal void NightCycle(bool delay)
        {
            int i = 1;
            Console.WriteLine("Who would you like to perform an action this day? Type the corresponding number.");
            foreach (PartyMember member in inventory.GetPartyMembers())
            {
                Console.WriteLine($"{i} - {member.Name} - {member.Role}");
                i++;
            }
            string choice = Console.ReadLine().Trim().ToUpper();
            int choose;
            while (!int.TryParse(choice, out choose))
            {
                Console.WriteLine("Please enter a number");
                choice = Console.ReadLine().Trim().ToUpper();
            }

            if (delay || visitedTowns.Count == 1)
            {
                inventory.GetPartyMembers()[choose - 1].PerformAction(visitedTowns[visitedTowns.Count-1], inventory);
            }
            else
            {
                inventory.GetPartyMembers()[choose - 1].PerformAction(visitedTowns[visitedTowns.Count - 2], inventory);
            }

        }

        /// <summary>
        /// Calls the store interact methods and calls the method to load the shop items from a file. Checks if player has enough money and inventory spaces and handles each accordingly
        /// </summary>
        /// <param name="iteration"></param>
        internal void StoreInteraction(int iteration)
        {
            if (visitedTowns[visitedTowns.Count - 1].StoreInteraction(iteration, inventory) == true)
            {
                int i = 0;
                TextColors.Shop("You currently have ");
                TextColors.Inventory($"${inventory.currentCash}\n");
                TextColors.Shop("Would you like to buy anything?\n"); TextColors.Emphasis("(Y)es/(N)o\n");
                string choice = Console.ReadLine().Trim().ToUpper();
                while (choice == null)
                {
                    Console.WriteLine("Please enter (Y)es or (N)o");
                    choice = Console.ReadLine().Trim().ToUpper();
                }
                
                while (choice.StartsWith('Y'))
                {
                    if (i > 0)
                    {
                        int j = 1;
                        {
                            TextColors.Shop("Our available wares are:\n");
                            foreach (Item item in visitedTowns[visitedTowns.Count() - 1].GetShopItems())
                            {
                                if (inventory.currentCash >= item.ItemPrice)
                                {
                                    TextColors.Normal($"{j}: {item.ItemID} - ${item.ItemPrice}\n");
                                }
                                else
                                {
                                    TextColors.Expensive($"{j}: {item.ItemID} - ${item.ItemPrice}\n");
                                }

                                j++;
                            }
                        }
                    }

                    i++;

                    TextColors.Normal("Type the number of the item you would like to buy.\n");
                    string chosenWare = Console.ReadLine().Trim().ToUpper();
                    int ware;
                    while (chosenWare == null)
                    {
                        Console.WriteLine("Please enter the number of the item that you would like to buy");
                        chosenWare = Console.ReadLine().Trim().ToUpper();
                    }
                    while (!int.TryParse(chosenWare, out ware))
                    {
                        Console.WriteLine("Please type a number");
                        chosenWare = Console.ReadLine().Trim().ToUpper();
                    }

                    Console.WriteLine("How many would you like?");
                    int num;
                    string wareNum = Console.ReadLine().Trim().ToUpper();
                    while (!int.TryParse(wareNum, out num))
                    {
                        Console.WriteLine("Please type a number");
                        wareNum = Console.ReadLine().Trim().ToUpper();
                    }
                    string itemName = visitedTowns[visitedTowns.Count - 1].GetShopItems()[int.Parse(chosenWare) - 1].ItemID;
                    int itemCost = visitedTowns[visitedTowns.Count - 1].GetShopItems()[int.Parse(chosenWare) - 1].ItemPrice;
                    int totalCost = num * itemCost;
                    Console.Write($"Are you sure you would like to buy {num} {itemName}?\nYou have "); TextColors.Inventory($"{inventory.MaxItems - inventory.CurrentItems.Count}"); Console.Write(" inventory spots left.\nThis will cost $"); TextColors.Inventory($"{totalCost}"); Console.WriteLine("(Y)es/(N)o");
                    choice = Console.ReadLine().Trim().ToUpper();
                    if (choice.StartsWith('Y'))
                    {
                        inventory.currentCash -= totalCost;
                        TextColors.Inventory($"You have ${inventory.currentCash} left.\n");
                        
                        if (inventory.currentCash < 0)
                        {
                            inventory.currentCash += totalCost;
                            TextColors.Inventory($"{inventory.currentCash}\n");
                            TextColors.Shop("I'm sorry, but you don't have enough to buy this, would you like to buy something else?\n");
                            choice = Console.ReadLine().Trim().ToUpper();
                        }
                        else
                        {
                            for (int x = 0; x < num; x++)
                            {
                                inventory.AddToInventory(visitedTowns[visitedTowns.Count - 1].GetShopItems()[int.Parse(chosenWare) - 1]);
                                if (inventory.CurrentItems.Count > inventory.MaxItems)
                                {
                                    inventory.ManageInventory(visitedTowns[visitedTowns.Count - 1].GetShopItems()[int.Parse(chosenWare) - 1]);
                                }
                            }
                            TextColors.Shop("Is there anything else that you would like to buy? (Y)es/(N)o\n");
                            choice = Console.ReadLine().Trim().ToUpper();
                        }
                    }
                    if (choice.StartsWith("N"))
                    {
                        TextColors.Shop("Alright then. Have a nice day!\n");
                    }
                }
            }

        }
        
        /// <summary>
        /// Gets a random amount of party members from the amount that the player currently has and returns the list of affected members
        /// </summary>
        /// <returns></returns>
        internal List<PartyMember> GetRandMembers()
        {
            List<PartyMember> affectedMembers = new List<PartyMember>();
            List<PartyMember> partyMembers = inventory.CurrentPartyMembers;
            Random random = new Random();

            int members = random.Next(partyMembers.Count - 1);

            for (int i = 0; i < members; i++)
            {
                PartyMember member = partyMembers[random.Next(partyMembers.Count - 1)];
                affectedMembers.Add(member);
                partyMembers.Remove(member);
            }
            return affectedMembers;
        }

        /// <summary>
        /// Method to select encounter from list at random and determines if there is a delay
        /// </summary>
        /// <returns></returns>
        internal bool GetEncounter()
        {
            Random random = new Random();
            List<PartyMember> members = GetRandMembers();
            int index = random.Next(encounters.Count - 1);
            bool delay = encounters[index].FullEffect(members, inventory);
            if (delay)
            {
                Console.Write("You have been delayed and are stuck in "); TextColors.Towns(visitedTowns[^1].ToString()); Console.WriteLine(" for another day");
            }
            else
            {
                Console.WriteLine("You have not been delayed.");
            }
            return delay;
        }

        /// <summary>
        /// Loads the town order from a file and then selects from them randomly to create a new town order
        /// </summary>
        internal void LoadTownOrder()
        {
            StreamReader reader = null;

            try
            {
                reader = new StreamReader("../../../TownList.txt");

                //Ask about File.ReadAllText and File.ReadLines
                //string[] lines = null;
                //lines = File.ReadAllLines("../../../TownList.txt");

                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    string addTown = (line);
                    towns.Add(addTown);
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

            Random randTown = new Random();
            int townCount = towns.Count;
            while (townOrder.Count < townCount)
            {
                string nextTown = towns[randTown.Next(towns.Count)];
                Town addTown = new Town(nextTown);
                townOrder.Enqueue(addTown);
                towns.Remove(nextTown);
            }
            Town FinalTown = new Town();
            townOrder.Enqueue(FinalTown);
        }

        /// <summary>
        /// Checks if there is a file to load the save data of and does do accordingly, called at the start of the game
        /// </summary>
        internal void LoadGame()
        {
            TextColors.Normal("What is the name of the file you would like to load?\n");
            string fileName = Console.ReadLine().Trim() + ".txt";
            StreamReader reader = null;

            try
            {
                reader = new StreamReader("../../../" + fileName);

                townOrder.Clear();

                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("townOrder:"))
                    {
                        townOrder.Enqueue(new Town(line.Substring(line.IndexOf(':') + 2)));
                    }

                    if (line.Contains("visited:"))
                    {
                        visitedTowns.Add(new Town(line.Substring(line.IndexOf(':') + 2)));
                    }

                    if ((line.Contains(PartyRoles.Trapeze +"") || line.Contains(PartyRoles.Strongman + "") || line.Contains(PartyRoles.Clown + "") || line.Contains(PartyRoles.BeastTamer + "")) && inventory.GetPartyMembers().Count == 0)
                    {
                        inventory.ExternalPerformerLoad(fileName);
                    }

                    if (line.Contains("Item") && inventory.GetCurrentItems().Count == 0)
                    {
                        inventory.ItemLoad(fileName);
                    }

                    if (line.Contains("currentCash"))
                    {
                        inventory.currentCash = int.Parse(line.Substring(12));
                    }
                }
            }
            catch (Exception e)
            {
                TextColors.Error("Error loading file: " + e.Message + "\n\n");
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
        /// Overloaded method, does the same thing as other load method, but allows the file to be saved and then immediately pick back up, this is because in order to save proper orders, I need to remove data from the various data structures
        /// </summary>
        /// <param name="fileName"></param>
        internal void LoadGame(string fileName)
        {
            fileName = fileName + ".txt";
            StreamReader reader = null;

            try
            {
                reader = new StreamReader("../../../" + fileName);

                townOrder.Clear();

                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("townOrder:"))
                    {
                        townOrder.Enqueue(new Town(line.Substring(line.IndexOf(':') + 2)));
                    }

                    if (line.Contains("visited:"))
                    {
                        visitedTowns.Add(new Town(line.Substring(line.IndexOf(':') + 2)));
                    }

                    if ((line.Contains(PartyRoles.Trapeze + "") || line.Contains(PartyRoles.Strongman + "") || line.Contains(PartyRoles.Clown + "") || line.Contains(PartyRoles.BeastTamer + "")) && inventory.GetPartyMembers().Count == 0)
                    {
                        inventory.ExternalPerformerLoad(fileName);
                    }

                    if (line.Contains("Item") && inventory.GetCurrentItems().Count == 0)
                    {
                        inventory.ItemLoad(fileName);
                    }

                    if (line.Contains("currentCash"))
                    {
                        inventory.currentCash = int.Parse(line.Substring(12));
                    }
                }
            }
            catch (Exception e)
            {
                TextColors.Error("Error loading file: " + e.Message + "\n\n");
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
        /// Saves the data to a save file, checks if there is already a file with that name, and handles accordingly
        /// </summary>
        /// <returns></returns>
        private string SaveGame()
        {
            bool wantSave = true;
            Console.WriteLine("What would you like the file name to be?");
            string fileName = Console.ReadLine().Trim();
            StreamWriter writer = null;
            while (wantSave)
            {
                while (File.Exists($"../../../{fileName}.txt"))
                {
                    TextColors.Error("A file with this name already exists. Are you sure that you want to overwrite this file? (Y)es/(N)o\n");
                    string choice = Console.ReadLine().Trim().ToUpper();
                    if (!choice.StartsWith('Y'))
                    {
                        Console.WriteLine("Do you want to create a file with a different name?");
                        string option = Console.ReadLine().Trim().ToUpper();
                        if (option.StartsWith('Y'))
                        {
                            Console.WriteLine("What would you like the file name to be?");
                            fileName = Console.ReadLine().Trim();
                            continue;
                        }
                        else
                        {
                            wantSave = false;
                            break;
                        }
                    }
                    else if (choice.StartsWith('Y'))
                    {
                        File.Delete(fileName);
                        break;
                    }
                }

                try
                {
                    writer = new StreamWriter("../../../" + fileName + ".txt");

                    int townOrderCount = townOrder.Count;
                    for (int i = 0; i < townOrderCount; i++)
                    {
                        writer.WriteLine("townOrder: " + townOrder.First());
                        townOrder.Dequeue();
                    }

                    int visitedTownsCount = visitedTowns.Count;
                    for (int i = 0; i < visitedTownsCount; i++)
                    {
                        writer.WriteLine("visited: " + visitedTowns.First());
                        visitedTowns.Remove(visitedTowns.First());
                    }

                    foreach (PartyMember member in inventory.GetPartyMembers())
                    {
                        writer.WriteLine(member.Role + ": " + member.Name + "," + member.GetHappiness());
                    }

                    foreach (Item items in inventory.GetCurrentItems())
                    {
                        writer.WriteLine("Item: " + items.ItemID + "," + items.ItemPrice + "," + items.IsConsumable);
                    }

                    writer.WriteLine($"currentCash: {inventory.currentCash}");
                }
                catch (Exception e)
                {
                    TextColors.Error("Error loading file: " + e.Message + "\n\n");
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                }
                return fileName;
            }
            return null;
        }
    }
}
