using gestion_contact_data;
using gestion_contact_serialisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace gestion_contact
{
    /*****************************************************
     * Class Console_components                          *
     * Regroups all the methodes used by the console     *
     * application                                       *
     *****************************************************/
    public static class Console_components
    {

        private static readonly Serialisation_manager SerializeManager = new Serialisation_manager();

        /// <summary>
        /// Retrieves a tree from a file, replacing the current user's tree.
        /// </summary>
        /// <param name="root">The current root of the user's tree.</param>
        /// <param name="current">The folder in the user's tree that is currently active.</param>
        public static void GetTree(ref Dossier root, ref Dossier current)
        {
            string fileName;
            bool isEncrypted;
            string decryptionKey = "";
            Dossier deserializedTree = null;
            string confirmation;

            Console.WriteLine("Your current folder tree will be replaced, please ensure it's backed up.");
            Console.WriteLine("Continue? (y/n)");
            confirmation = Console.ReadLine();

            /* If the user wishes to proceed with loading a new tree */
            if (confirmation.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("The standard path is {0})", AbsolutePath.DirectPath);
                Console.WriteLine("Enter the file name:");
                fileName = Console.ReadLine();
                isEncrypted = fileName.Contains("_encrypted");

                /* If the file is encrypted, prompt for the decryption key */
                if (isEncrypted)
                {
                    Console.WriteLine("The file is encrypted, please enter the decryption key: ");
                    decryptionKey = Console.ReadLine();
                    deserializedTree = SerializeManager.Deserialize(fileName, true, decryptionKey);
                }
                else
                {
                    deserializedTree = SerializeManager.Deserialize(fileName, false, decryptionKey);
                }

                /* If the deserialization was successful */
                if (deserializedTree != null)
                {
                    root = deserializedTree;
                    current = deserializedTree;
                    Console.WriteLine("Recovery completed!\n");
                }
                else
                {
                    Console.WriteLine("Loading aborted...\n");
                }
            }
            else
            {
                Console.WriteLine("Loading aborted...\n");
            }
        }


        /// <summary>
        /// Saves a tree of folders in a file.
        /// </summary>
        /// <param name="root">The root folder of the tree to serialize.</param>
        public static void SaveTree(Dossier root)
        {
            string fileName;
            string[] fileNameParts;
            string encryptionChoice;
            string encryptionKey = "";

            Console.WriteLine("Folders will be saved in the directory {0}.", AbsolutePath.DirectPath);
            Console.WriteLine("Enter a name for the file: (.bin or .xml)");
            fileName = Console.ReadLine();
            fileNameParts = fileName.Split('.');
            Console.WriteLine("Would you like to encrypt the file? (y/n)");
            encryptionChoice = Console.ReadLine();

            /* If the user chooses to encrypt the file */
            if (encryptionChoice.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                fileName = string.Join(".", fileNameParts.Take(fileNameParts.Length - 1)) + "_encrypted." + fileNameParts.Last();
                Console.WriteLine("Enter a password for encryption (it will be required when loading the file):");
                encryptionKey = Console.ReadLine();
                SerializeManager.Serialize(root, fileName, true, encryptionKey);
            }
            else
            {
                /* The pattern '_encrypted' is only for encrypted files */
                if (!fileName.Contains("_encrypted"))
                {
                    SerializeManager.Serialize(root, fileName, false, encryptionKey);
                }
            }
        }


        /// <summary>
        /// Adds an individual contact to the children of the provided folder.
        /// </summary>
        /// <param name="currentFolder">The folder in which to add the contact.</param>
        public static void AddContact(Dossier currentFolder)
        {
            string lastName;
            string firstName;
            string email;
            string company;
            string relationship;
            Relation link = Relation.Ami;
            bool validEmailFormat = false;
            Contact newContact = null;

            /* Collect all required information */
            Console.WriteLine($"\n\nThe contact will be added to the current folder ({currentFolder.GetPath("")})");
            Console.WriteLine("Enter the last name of the contact: ");
            lastName = Console.ReadLine();
            Console.WriteLine("Enter the first name of the contact: ");
            firstName = Console.ReadLine();
            Console.WriteLine("Enter the email of the contact (example@example.example): ");

            /* Ensure the email is in the correct format */
            do
            {
                email = Console.ReadLine();
                validEmailFormat = Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                if (!validEmailFormat)
                {
                    Console.WriteLine("Incorrect email format. Please use the format example@example.example");
                }
            } while (!validEmailFormat);

            Console.WriteLine("Enter the company of the contact: ");
            company = Console.ReadLine();

            /* User can choose from one of the four options for relationship */
            do
            {
                Console.WriteLine("Relationship with the contact: \n\n1- Friend\n2- Colleague\n3- Acquaintance\n4- Network");
                relationship = Console.ReadLine();
                switch (relationship.ToLower())
                {
                    case "1":
                    case "friend":
                        break;
                    case "2":
                    case "colleague":
                        link = Relation.Collegue;
                        break;
                    case "3":
                    case "acquaintance":
                        link = Relation.Relation;
                        break;
                    case "4":
                    case "network":
                        link = Relation.Reseau;
                        break;
                    default:
                        Console.WriteLine("Invalid option!\n");
                        break;
                }
            } while (relationship != "1" && relationship != "friend" && relationship != "2" &&
                     relationship != "colleague" && relationship != "3" && relationship != "acquaintance" &&
                     relationship != "4" && relationship != "network");

            Console.WriteLine("\nCreating the contact... \n");
            newContact = new Contact(lastName, firstName, email, company, link);
            currentFolder.AddFichier(newContact);

            Console.WriteLine("Done!\n");
        }


        /// <summary>
        /// Adds a new folder to the list of children of the provided folder.
        /// </summary>
        /// <param name="currentFolder">The folder in which to add the new folder.</param>
        public static void AddFolder(ref Dossier currentFolder)
        {
            string folderName;
            string confirmation;
            Dossier newFolder = null;
            Fichier existingItem;

            Console.WriteLine($"\n\nThe folder will be added to the current folder ({currentFolder.GetPath("")})");
            Console.WriteLine("Enter a name for the new folder: ");
            folderName = Console.ReadLine();
            Console.WriteLine($"\nThe folder will be named {folderName}. Confirm? (y/n)");
            confirmation = Console.ReadLine();

            // If the user confirms the folder name
            if (confirmation.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                existingItem = currentFolder.Find(folderName);

                // If there isn't already a folder or contact with the same name in the children list
                if (existingItem == null || existingItem is Contact)
                {
                    Console.WriteLine("\nCreating the folder...\n");
                    newFolder = new Dossier(folderName, currentFolder);
                    currentFolder.AddFichier(newFolder);
                    currentFolder = newFolder;
                }
                else
                {
                    Console.WriteLine($"The folder {folderName} already exists.\nCreation canceled...\n");
                }
            }
            else
            {
                Console.WriteLine("\nCreation canceled...\n");
            }

            Console.WriteLine("Done!\n");
        }



        /// <summary>
        /// Displays a hierarchical tree structure starting from the given file.
        /// </summary>
        /// <param name="currentFile">The file to display in the tree.</param>
        /// <param name="level">The depth level of the file in the tree.</param>
        public static void DisplayTree(Fichier currentFile, int level)
        {
            // Display the name of the file with proper indentation
            for (int i = 0; i < level; i++)
            {
                Console.Write("  ");
            }
            Console.WriteLine("|");

            for (int i = 0; i < level; i++)
            {
                Console.Write("  ");
            }
            Console.Write(" -- ");

            Console.WriteLine(currentFile.GetNom());

            // If the file is a directory, display its contents
            if (currentFile is Dossier)
            {
                // Display all the files in the directory
                foreach (Fichier file in ((Dossier)currentFile).Fichiers)
                {
                    DisplayTree(file, level + 1);
                }
            }
        }


        /// <summary>
        /// Changes the current directory by moving up or down one level in the tree.
        /// </summary>
        /// <param name="currentFolder">The current folder in the file system.</param>
        public static void ChangeDirectory(ref Dossier currentFolder)
        {
            string folderName;
            Fichier targetFolder;

            Console.WriteLine("\nCurrent Tree Structure:\n" + currentFolder.getDossiers() + "\n");
            Console.WriteLine("Enter a folder name within the current directory or '..' to move to the parent directory\n");
            folderName = Console.ReadLine();

            // If the user wants to move up one level
            if (folderName == "..")
            {
                // Check if the current folder is not the root
                if (currentFolder.Parent != null)
                {
                    currentFolder = currentFolder.Parent;
                }
                else
                {
                    Console.WriteLine("This folder is the root directory\n");
                }
            }

            // If the user wants to move down one level
            else
            {
                targetFolder = currentFolder.Find(folderName);

                // If a folder with the given name is found
                if (targetFolder != null && targetFolder is Dossier)
                {
                    currentFolder = (Dossier)targetFolder;
                    Console.WriteLine("Folder found, moving into the directory.\n");
                }
                else
                {
                    Console.WriteLine("Unknown folder\n");
                }
            }
        }



        /// <summary>
        /// Displays a user-friendly menu with various options based on the current directory.
        /// </summary>
        /// <param name="currentDirectory">The directory in which the user is currently located.</param>
        public static void DisplayMenu(Dossier currentDirectory)
        {
            // Obtain the current directory path
            string currentPath = currentDirectory.GetPath("");

            // Display an aesthetically pleasing menu header
            Console.WriteLine("\t╔══════════════════════════╗");
            Console.WriteLine("\t║      Choose an Action:   ║");
            Console.WriteLine("\t╚══════════════════════════╝\n");

            // Present the current directory path
            Console.WriteLine($"\t*Current Directory: {currentPath}*\n\n");

            // List available options in the menu
            Console.WriteLine("\t 1. Display Directory Tree => (tree)");
            Console.WriteLine("\t 2. Change Directory => (cd)");
            Console.WriteLine("\t 3. Load => (load)");
            Console.WriteLine("\t 4. Save => (save)");
            Console.WriteLine("\t 5. Add a Directory => (mkdir)");
            Console.WriteLine("\t 6. Add a Contact => (mkcon)");
            Console.WriteLine("\t 7. Exit => (exit)\n");

            // Prompt for user input
            Console.Write("Enter your choice: ");
        }
    }
}
