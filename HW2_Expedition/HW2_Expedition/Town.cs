using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HW2_Expedition
{
    internal class Town
    {
        //name of town
        private string townName;

        //List of items available in the shop 
        private List<Item> shopItems;

        public string TownName { get; set; }

        //Constructor
        public Town(string townName)
        {
            shopItems = new List<Item>();
            this.TownName = townName;
        }

        //Overloaded constructor for the final town because it doesn't need to have information
        public Town()
        {

        }

        /// <summary>
        /// Made before properties were understood, didn't have time to fix
        /// </summary>
        /// <returns></returns>
        internal List<Item> GetShopItems()
        {
            return shopItems;
        }

        /// <summary>
        /// Store interaction method which prints the available wares in the specific town
        /// </summary>
        /// <param name="iteration"></param>
        /// <param name="inventory"></param>
        /// <returns></returns>
        internal bool StoreInteraction(int iteration, Inventory inventory)
        {
            if (iteration == 0)
            {
                LoadStoreItems($"{TownName}Store.txt");
            }
            Console.WriteLine("Would you like to look at the store? (Y)es/(N)o");
            string choice = Console.ReadLine().Trim().ToUpper();
            while (string.IsNullOrEmpty(choice))
            {
                TextColors.Error("Please enter (Y)es or (N)o");
                choice = Console.ReadLine().Trim().ToUpper();
            }

            int i = 1;
            if (choice.StartsWith("Y"))
            {
                TextColors.Shop("Our available wares are:\n");
                foreach (Item item in shopItems)
                {
                    if (inventory.currentCash >= item.ItemPrice)
                    {
                        TextColors.Normal($"{i}: {item.ItemID} - ${item.ItemPrice}\n");
                    }
                    else
                    {
                        TextColors.Expensive($"{i}: {item.ItemID} - ${item.ItemPrice}\n");
                    }

                    i++;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loads the store items for each town from a file
        /// </summary>
        /// <param name="fileName"></param>
        internal void LoadStoreItems(string fileName)
        {
            StreamReader reader = null;

            try
            {
                reader = new StreamReader("./" + fileName);

                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] strings = line.Split(',');

                    shopItems.Add(new Item(strings[0], int.Parse(strings[1]), bool.Parse(strings[2])));
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

        }

        /// <summary>
        /// Gets and returns the town descriptions for each town
        /// </summary>
        /// <returns></returns>
        internal string TownDescription()
        {
            switch (TownName)
            {
                case "Troutbeck":
                    return $"{TownName} is a wonderful shore town with a large market in fishing and seafood. Many have passed through this very town on the way to success.";

                case "Lookout Point":
                    return $"{TownName} has a high elevation making way for incredible views of the local landscape. Rumor has it that this very town was once a stronghold during a long forgotten war.";

                case "Woodborough":
                    return $"{TownName} is a logging town that takes great pride in maintaining the forest. A lot of the town is themed around the woods.";

                case "East Hollow":
                    return $"{TownName} is known as the \"Gateway to the East\", and you can see why. There is a very large open door statue that has been constructed and you can assume that it probably faces east.";

                case "New Grim":
                    return $"{TownName} is famous for it's year-round celebration of Halloween and everything spooky. The people here are so passionate about it that they name everything that they have something special to fit the theme.";

                case "Dayton":
                    return $"{TownName} is famous for the annual footraces it hosts, named after the number of participants allowed each year, the Dayton 200 is a sight to see.";

                case "Greenville":
                    return $"{TownName} is famously the invention point of green crayons";

                case "Pippinpaddleopsicopolis":
                    return $"{TownName} is named after it's great founder Kuzon. Yes the people do actually know to pronounce it.";

                case "Rainbow End":
                    return $"{TownName} is falsely named in the same way that Greenland is, nobody really wants to live here, but they hope that people come looking for the metaphorical \"pot of gold\".";

                case "Tarrey Town":
                    return $"{TownName} is a new town that has been built from the ground up. It is a mix of people from all kinds of places and walks of life.";

                default:
                    return null;
            }
        }

        /// <summary>
        /// For debugger ease of use
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return TownName;
        }
    }
}
