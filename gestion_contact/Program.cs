using System;
using gestion_contact_data;

namespace gestion_contact
{
    /***********************************
     * Main Program                    *
     ***********************************/
    public class Program
    {
        static void Main(string[] args)
        {
            // Initialize the root directory
            Dossier root = new Dossier("home");
            Dossier current = root;
            bool exitRequested = false;

            // Main program loop
            while (!exitRequested)
            {
                // Display the user-friendly menu and gather user input
                Console_components.DisplayMenu(current);
                string userChoice = Console.ReadLine()?.ToLower();

                // Process user's selection
                switch (userChoice)
                {
                    case "display directory tree":
                    case "tree":
                        Console_components.DisplayTree(current, 0);
                        Console.WriteLine("\n");
                        break;
                    case "load":
                        Console_components.GetTree(ref root, ref current);
                        break;
                    case "save":
                        Console_components.SaveTree(root);
                        break;
                    case "add a directory":
                    case "mkdir":
                        Console_components.AddFolder(ref current);
                        break;
                    case "add a contact":
                    case "mkcon":
                        Console_components.AddContact(current);
                        break;
                    case "change directory":
                    case "cd":
                        Console_components.ChangeDirectory(ref current);
                        break;
                    case "exit":
                        exitRequested = true;
                        break;
                    default:
                        Console.WriteLine("Unknown operation!\n");
                        break;
                }
            }
        }
    }
}

