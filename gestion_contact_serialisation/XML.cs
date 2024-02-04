using gestion_contact_data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace gestion_contact_serialisation
{
    /***************************************************
     * Class XMLSerializer                             *
     * Used to manage the serialization of an XML file *
     ***************************************************/
    public class XMLSerializer
    {

        private readonly XmlSerializer serializer = new XmlSerializer(typeof(Dossier));

        /// <summary>
        /// Method Save :
        /// Creates an XML file in which is written the given tree without encryption 
        /// </summary>
        /// <param name="root">Folder at the root of the tree to serialize</param>
        /// <param name="name">Name of the file in which to write the datas</param>
        public void Save(Dossier root, string name)
        {
            using (FileStream stream = new FileStream(AbsolutePath.DirectPath + name, FileMode.Create))
            {
                serializer.Serialize(stream, root);
            }
        }

        /// <summary>
        /// Method Link :
        /// Links a parent-folder to its children
        /// </summary>
        /// <param name="parent">Folder to link with its children</param>
        private void Link(ref Dossier parent)
        {
            /* For every child */
            foreach (Fichier f in parent.Fichiers)
            {
                /* If the child is a Dossier which needs to be linked */
                if (f is Dossier current)
                {
                    current.Parent = parent;
                    Link(ref current);
                }
            }
        }


        /// <summary>
        /// Method Get :
        /// Get a tree from an XML file that is not encrypted
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
                    root = (Dossier)serializer.Deserialize(stream);
                }
                root.Parent = null;
                Link(ref root);
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine("Le fichier " + file_name + " n'existe pas, vérifiez que vous avez bien renseigné le bon chemin d'accès.");
            }
            catch(Exception e)
            {
                Console.WriteLine("Impossible de lire le fichier " + e.Message);
            }

            return root;
        }


        /// <summary>
        /// Method Encrypt :
        /// Serialize a tree into a file and encrypting it
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
                        serializer.Serialize(cryptoStream, root);
                    }
                }
            }
        }

        /// <summary>
        /// Method Decrypt :
        /// Decrypt datas written into an XML file to retreive a tree
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
                            root = (Dossier)serializer.Deserialize(cryptoStream);
                            Link(ref root);
                        }
                    }
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Erreur lors du déchiffrement, vérifier si la clé est la bonne : " + e.Message);
            }
            catch (FileNotFoundException e)
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
