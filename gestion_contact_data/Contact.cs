using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace gestion_contact_data
{
    /*****************************************************
     * Class Contact                                     *
     * Class in which to regroup the information on an   *
     * individual                                        *
     *****************************************************/
    [Serializable]
    [XmlInclude(typeof(Dossier))]
    public class Contact : Fichier
    {
        private string Prenom;
        private string Courriel;
        private string Societe;
        private Relation Lien;

        /// <summary>
        /// Constructor with all the information needed
        /// </summary>
        /// <param name="nom">Last name of the individual</param>
        /// <param name="prenom">First name of the individual</param>
        /// <param name="courriel">E-mail of the individual</param>
        /// <param name="societe">Company of the individual</param>
        /// <param name="lien">Social link between the individual and the user</param>
        public Contact(string nom, string prenom, string courriel, string societe, Relation lien) : base(nom, DateTime.Now)
        {
            this.Prenom = prenom;
            this.Courriel = courriel;
            this.Societe = societe;
            this.Lien = lien;
        }


        public Contact() : this("", "", "", "", Relation.Relation)
        {

        }

        /// <summary>
        /// Method GetNom :
        /// Returns a string which contains all the information on the individual
        /// </summary>
        /// <returns>The information on the individual</returns>
        public override string GetNom()
        {
            return Nom + " " + Prenom + ", " + Courriel + ", " + Societe + " " + Lien;
        }
    }
}
