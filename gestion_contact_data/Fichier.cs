using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace gestion_contact_data
{
    /*****************************************************
     * Abstract class Fichier                            *
     * Abstract class for the files                      *
     *****************************************************/
    [Serializable]
    public abstract class Fichier
    {
        public string Nom { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateModif { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nom">Name of the file</param>
        /// <param name="time">Date of creation of the file</param>
        protected Fichier(string nom, DateTime time)
        {
            Nom = nom;
            DateCreation = time;
            DateModif = time;
        }

        public abstract string GetNom();
    }
}
