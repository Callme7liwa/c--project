using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace gestion_contact_data
{
    /*****************************************************
     * Class Dossier                                     *
     * Class simulating a folder in which files can be   *
     * stored                                            *
     *****************************************************/
    [Serializable]
    [XmlRoot("Dossier"), XmlInclude(typeof(Dossier)), XmlInclude(typeof(Contact))]
    public class Dossier : Fichier
    {
        /// <summary>
        /// A folder has files as children
        /// </summary>
        public List<Fichier> Fichiers { get; private set; }

        /// <summary>
        /// A folder has a folder that is its parent
        /// </summary>
        [XmlIgnore]
        public Dossier Parent { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nom">Name of the folder</param>
        /// <param name="parent">Parent of the new folder</param>
        public Dossier(string nom, Dossier parent) : base(nom, DateTime.Now)
        {
            this.Fichiers = new List<Fichier>();
            Parent = parent;
        }

        /// <summary>
        /// Constructor for the root folder
        /// </summary>
        /// <param name="nom">Name of the folder</param>
        public Dossier(string nom) : this(nom, null)
        {

        }

        /// <summary>
        /// Constructor for the serialization
        /// </summary>
        public Dossier() : this("home", null)
        {

        }


        /// <summary>
        /// Method getDossiers :
        /// Get the folders in the children as a string to display them
        /// </summary>
        /// <returns>A string which displays the children as a tree</returns>
        public string getDossiers()
        {
            string res = Nom;
            /* For each child */
            foreach(Fichier f in Fichiers)
            {
                /* If it is a folder */
                if(f is Dossier)
                {
                    res = res + "\n  |\n   --" + f.Nom;
                }
            }

            return res;
        }

        /// <summary>
        /// Method Find :
        /// Look for a file with a specific name in the children of the folder
        /// </summary>
        /// <param name="nom">Name of the file to find</param>
        /// <returns>The file if found or null</returns>
        public Fichier Find(string nom)
        {
            Fichier res = null;
            int i = 0;

            /* While the file is not found */
            while(i < Fichiers.Count && Fichiers[i].Nom != nom)
            {
                i++;
            }

            /* If a file matching the name was found */
            if(i < Fichiers.Count)
            {
                res = Fichiers[i];
            }

            return res;
        }

        /// <summary>
        /// Method GetNom :
        /// Returns a string which is the full name of the folder
        /// </summary>
        /// <returns>The full name of the folder</returns>
        public override string GetNom()
        {
            return "/" + Nom;
        }

        /// <summary>
        /// Method GetTree :
        /// Get the whole path from this folder as a string in order to display it
        /// </summary>
        /// <param name="res">String in which to store the tree</param>
        /// <returns>The tree as a string</returns>
        public string GetPath(string res)
        {
            /* If it is not the root folder */
            if(Parent != null)
            {
                res = Parent.GetPath(res);
            }

            return res + GetNom();
        }


        /// <summary>
        /// Method AddFichier :
        /// Adds a file in the children of the folder
        /// </summary>
        /// <param name="f">The file to add</param>
        public void AddFichier(Fichier f)
        {
            Fichiers.Add(f);
        }

    }
}
