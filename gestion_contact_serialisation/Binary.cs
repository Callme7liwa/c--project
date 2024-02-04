using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using gestion_contact_data;
using System.Security.Cryptography;

namespace gestion_contact_serialisation
{
    /*****************************************************
     * Class BinarySerializer                            *
     * Used to manage the serialization of a binary file *
     *****************************************************/
    public class BinarySerializer
    {
        private BinaryFormatter formatter = new BinaryFormatter();

        /// <summary>
        /// Method Save :
        /// Creates a binary file in which is written the given tree without encryption 
        /// </summary>
        /// <param name="root">Folder at the root of the tree to serialize</param>
        /// <param name="name">Name of the file in which to write the datas</param>
        public void Save(Dossier root, string name)
        {
            using (FileStream stream = new FileStream(AbsolutePath.DirectPath + name, FileMode.Create))
            {
                formatter.Serialize(stream, root);
            }

        }

        /// <summary>
        /// Method Get :
        /// Get a tree from a binary file that is not encrypted
        /// </summary>
        /// <param name="file_name">Name of the file to deserialize</param>
        /// <returns>Dossier which contains the root of the newly retreived tree</returns>
        public Dossier Get(string file_name)
        {
            Dossier root = null;

            try
            {
                using (FileStream stream = new FileStream(AbsolutePath.DirectPath + file_name, FileMode.Open))
                {
                    root = (Dossier)formatter.Deserialize(stream);
                }
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine("Le fichier " + file_name + " n'existe pas, vérifiez que vous avez bien renseigné le bon nom.");
            }

            return root;
        }

        /// <summary>
        /// Method Encrypt :
        /// Serialize a tree into a binary file and encrypting it
        /// </summary>
        /// <param name="root">Folder at the root of the tree to serialize</param>
        /// <param name="file_name">Name of the file in which to write the datas</param>
        /// <param name="key">Key with which to encrypt the datas</param>
        public void Encrypt(Dossier root, string file_name, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;

                using (FileStream fileStream = File.Open(AbsolutePath.DirectPath + file_name, FileMode.Create))
                {
                    fileStream.Write(aes.IV, 0, aes.IV.Length);
                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        formatter.Serialize(cryptoStream, root);
                    }
                }
            }
        }

        /// <summary>
        /// Method Decrypt :
        /// Decrypt datas written into a binary file to retreive a tree
        /// </summary>
        /// <param name="file_name">Name of the file to decrypt</param>
        /// <param name="key">Key with which to decrypt the datas</param>
        /// <returns>Dossier which contains the root of the retreived tree</returns>
        public Dossier Decrypt(string file_name, byte[] key)
        {
            Dossier root = null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;

                    using (FileStream fs = File.Open(AbsolutePath.DirectPath + file_name, FileMode.Open))
                    {
                        byte[] IV = new byte[aes.IV.Length];
                        fs.Read(IV, 0, aes.IV.Length);
                        aes.IV = IV;
                        using (CryptoStream cryptoStream = new CryptoStream(fs, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Read))
                        {
                            root = (Dossier)formatter.Deserialize(cryptoStream);
                        }
                    }
                }
            }
            catch(CryptographicException e)
            {
                Console.WriteLine("Erreur lors du déchiffrement, vérifier si la clé est la bonne : " + e.Message);
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("Le fichier n'existe pas : " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Impossible de déchiffrer le fichier " + e.Message);
            }
            return root;
        }
    }
}
