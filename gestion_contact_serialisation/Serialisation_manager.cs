using gestion_contact_data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace gestion_contact_serialisation
{

    /**************************************************
     * Class Serialisation_manager                    *
     * Used to manage the whole serialization process *
     **************************************************/
    public class Serialisation_manager
    {
        public BinarySerializer binary = new BinarySerializer();
        public XMLSerializer xml = new XMLSerializer();

        /// <summary>
        /// Method Serialize :
        /// Manages the serialization of a tree of files
        /// </summary>
        /// <param name="root">Folder at the root of the tree to serialize</param>
        /// <param name="file_name">Name of the file in which to serialize</param>
        /// <param name="encrypt">Whether or not to encrypt the file</param>
        /// <param name="key">Password to hash if the tree is to be encrypted</param>
        public void Serialize(Dossier root, string file_name, bool encrypt, string key)
        {
            if (encrypt)
            {
                EncryptedSerialize(root, file_name, key);
            }
            else
            {
                NormalSerialize(root, file_name);
            }
        }

        /// <summary>
        /// Method NormalSerialize :
        /// Manages the serialization of a tree of files without encrypting the datas
        /// </summary>
        /// <param name="root">Folder at the root of the tree to serialize</param>
        /// <param name="file_name">Name of the file in which to serialize</param>
        public void NormalSerialize(Dossier root, string file_name)
        {
            string ext = file_name.Split('.').Last();

            try
            {
                /* If the file should be a binary one */
                if (ext == "bin")
                {
                    binary.Save(root, file_name);
                }
                /* If the file should be an xml file */
                else if (ext == "xml")
                {
                    xml.Save(root, file_name);
                }
                else
                {
                    Console.WriteLine("Extension non reconnue. Abandon...");
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la sérialisation " + e.Message);
            }
        }

        /// <summary>
        /// Method EncryptedSerialize :
        /// Manages the serialization of a tree of files and encrypting the datas
        /// </summary>
        /// <param name="root">Folder at the root of the tree to serialize</param>
        /// <param name="file_name">Name of the file in which to serialize</param>
        /// <param name="key">Password to hash and create the key to encrypt the datas</param>
        public void EncryptedSerialize(Dossier root, string file_name, string key)
        {
            string ext = file_name.Split('.').Last();

            try
            {
                byte[] new_key = CreateKey(key);

                /* If the file should be a binary one */
                if (ext == "bin")
                {
                    binary.Encrypt(root, file_name, new_key);
                }
                /* If the file should be an xml one */
                else if(ext == "xml")
                {
                    xml.Encrypt(root, file_name, new_key);
                }
                else
                {
                    Console.WriteLine("Extension non reconnue. Abandon...");
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la sérialisation " + e.Message);
            }
        }

        /// <summary>
        /// Method Deserialize :
        /// Manages the deserialization of a file into a tree of files
        /// </summary>
        /// <param name="file_name">Name of the file in which to serialize</param>
        /// <param name="encrypt">Whether or not the file is encrypted</param>
        /// <param name="key">Password of the encrypted file</param>
        /// <returns>Dossier which is the root of the newly retreived tree</returns>
        public Dossier Deserialize(string file_name, bool encrypt, string key)
        {
            Dossier root_out = null;

            if (encrypt)
            {
                root_out = EncryptedDeserialize(file_name, key);
            }
            else
            {
                root_out = NormalDeserialize(file_name);
            }

            return root_out;
        }


        /// <summary>
        /// Method NormalDeserialize :
        /// Manages the deserialization of a file that is not encrypted
        /// </summary>
        /// <param name="file_name">Name of the file to deserialize</param>
        /// <returns>Dossier which is the root of the newly retreived tree</returns>
        public Dossier NormalDeserialize(string file_name)
        {
            string ext = file_name.Split('.').Last();
            Dossier root_out = null;

            try
            {
                /* If the file is a binary one */
                if (ext == "bin")
                {
                    root_out = binary.Get(file_name);
                }
                /* If the file is an xml one */
                else if (ext == "xml")
                {
                    root_out = xml.Get(file_name);
                }
                else
                {
                    Console.WriteLine("Extension non reconnue. Abandon...");
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la sérialisation " + e.Message);
            }

            return root_out;
        }

        /// <summary>
        /// Method EncryptedDeserialize :
        /// Manages the deserialization of a file that is encrypted
        /// </summary>
        /// <param name="file_name">Name of the file to deserialize</param>
        /// <returns>Dossier which is the root of the newly retreived tree</returns>
        public Dossier EncryptedDeserialize(string file_name, string key)
        {
            string ext = file_name.Split('.').Last();
            Dossier root_out = null;

            try
            {
                byte[] new_key = CreateKey(key);

                /* If the file is a binary one */
                if (ext == "bin")
                {
                    root_out = binary.Decrypt(file_name, new_key);
                }
                /* If the file is an xml one */
                else if (ext == "xml")
                {
                    root_out = xml.Decrypt(file_name, new_key);
                }
                else
                {
                    Console.WriteLine("Extension non reconnue. Abandon...");
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la désérialisation " + e.Message);
            }

            return root_out;
        }

        /// <summary>
        /// Method CreateKey :
        /// Creates a key with sha256 which will be used to encrypt or decrypt a file
        /// </summary>
        /// <param name="in_key">Password to hash in order to create the key</param>
        /// <returns>A 16-byte-key</returns>
        public byte[] CreateKey(string in_key)
        {
            byte[] out_key;
            using (SHA256 sha256 = SHA256.Create())
            {
                out_key = sha256.ComputeHash(Encoding.UTF8.GetBytes(in_key));
                Array.Resize(ref out_key, 16); // Resize to get the correct size for the key
                return out_key;
            }
        }
    }
}
