using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2_Expedition
{
    internal class Item
    {
        //Name of item
        private string itemID;
        
        //Price of item in shop
        private int itemPrice;

        //Description of item
        private string itemDescription;

        //Whether the item can be used or not
        private bool isConsumable;

        public string ItemID { get; set; }
        public string ItemDescription { get; set; }
        public int ItemPrice { get; set; }

        public bool IsConsumable { get; set; }
        
        //Constructor
        public Item(string itemID, int itemPrice, bool isConsumable)
        {
            this.ItemID = itemID;
            this.ItemPrice = itemPrice;
            this.IsConsumable = isConsumable;
        }

        /// <summary>
        /// Returns the amount item affects member happiness
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        internal int AffectHappiness(PartyMember member)
        {
            switch (ItemID)
            {
                case "Apple":
                    return 5;
                    break;
                case "Chicken Parm":
                    return 9;
                    break;
                case "Steel Drum":
                    return 30;
                    break;
                case "Pumpkin Pie":
                    return 15;
                    break;
                case "Chicken Alfredo":
                    return 7;
                    break;
                case "Ticket To Magic Show":
                    return 35;
                    break;
                case "Poison Apple":
                    return 6;
                    break;
                case "Congealed Blood Pie":
                    return 17;
                    break;
                case "Ticket For Haunted Maze":
                    return 40;
                    break;
                case "Smoked Salmon":
                    return 10;
                    break;
                case "Sushi":
                    return 6;
                    break;
                case "Reed Flute":
                    return 25;
                    break;
                case "Bongos":
                    return 28;
                    break;
                case "Guitar":
                    return 40;
                    break;
                case "Kazoo":
                    return 30;
                    break;
                case "Carrot Cake":
                    return 17;
                    break;
                default:
                    return 5000;
            }
        }

        /// <summary>
        /// Returns the description of the item
        /// </summary>
        /// <returns></returns>
        internal string GetItemDesc()
        {
            string description = null;
            switch (ItemID)
            {
                case "Apple":
                    description = "A common fruit that is found almost everywhere. It raises happiness by 5";
                    break;
                case "Chicken Parm":
                    description = "A delicious dish made with chicken and toasted cheese forming a crust. It raises happiness by 9.";
                    break;
                case "Steel Drum":
                    description = "A beautiful instrument that can be mezmorizing if played well. It raises happiness by 30.";
                    break;
                case "Pumpkin Pie":
                    description = "A sweet treat made with care. It raises happiness by 15.";
                    break;
                case "Chicken Alfredo":
                    description = "A delicious meal made with chicken and pasta. It raises happiness by 7.";
                    break;
                case "Ticket To Magic Show":
                    description = "A ticket to see a magician perform. Raises happiness by 35.";
                    break;
                case "Poison Apple":
                    description = "Seemingly a regular apple, but you can never know for sure. Raises happiness by 6.";
                    break;
                case "Congealed Blood Pie":
                    description = "The filling of this pie seems like pumpkin, at least you hope it is. Raises happiness by 17.";
                    break;
                case "Ticket For Haunted Maze":
                    description = "Is the maze really haunted? Probably not, but maybe they forgot to put up their bottle trees. Raises happiness by 40.";
                    break;
                case "Smoked Salmon":
                    description = "Salmon that has been smoked for hours, it tastes delicious. Raises happiness by 10.";
                    break;
                case "Sushi":
                    description = "A treat offered by more shoreline towns. Raises happiness by 6.";
                    break;
                case "Reed Flute":
                    description = "An instrument that isn't as popular, but still sounds pretty. Raises happiness by 25.";
                    break;
                case "Bongos":
                    description = "A great way to get out any frustrations. Raises happiness by 28.";
                    break;
                case "Guitar":
                    description = "Great for sitting around a campfire and singing. Raises happiness by 40.";
                    break;
                case "Kazoo":
                    description = "Buzzzzzz. Raises happiness by 30.";
                    break;
                case "Carrot Cake":
                    description = "A warm cakey deliciousness, made with love (and many carrots). Raises happiness by 17.";
                    break;
                default:
                    return null;
            }
            if (description == null)
            {
                return "The dev obviously has no idea what he's doing. Why am I even appearing?";
            }
            return description;
        }

        public override string ToString()
        {
            return ItemID;
        }
    }
}
