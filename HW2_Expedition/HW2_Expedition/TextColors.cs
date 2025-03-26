using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HW2_Expedition
{
    /// <summary>
    /// Collection of Colors for different uses
    /// </summary>
    internal class TextColors
    {
        internal static void Shop(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Expensive(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red; 
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Normal(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(message);
        }

        internal static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Inventory(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Title(string messsage)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(messsage);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Towns(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Emphasis(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Encounter(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Role(string message, PartyRoles role)
        {
            if (role == PartyRoles.Trapeze)
            {
                Trapeze(message);
            }
            if (role == PartyRoles.Strongman)
            {
                Strongman(message);
            }
            if (role == PartyRoles.Clown)
            {
                Clown(message);
            }
            if (role == PartyRoles.BeastTamer)
            {
                BeastTamer(message);
            }
        }

        internal static void Role(string message, PartyMember member)
        {
            if (member.Role == PartyRoles.Trapeze)
            {
                Trapeze(message);
            }
            if (member.Role == PartyRoles.Strongman)
            {
                Strongman(message);
            }
            if (member.Role == PartyRoles.Clown)
            {
                Clown(message);
            }
            if (member.Role == PartyRoles.BeastTamer)
            {
                BeastTamer(message);
            }
        }

        internal static void Trapeze(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Strongman(string message)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Clown(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void BeastTamer(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
