using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gestion_contact_data
{
    /*****************************************************
     * Class AbsolutePath                                *
     * Class used to store and use the path of the       *
     * directory used for serialization/deserialization  *
     *****************************************************/
    public static class AbsolutePath
    {
        public static string DirectPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\contacts\\";


        /// <summary>
        /// Creates a directory in the system if it is not yet created.
        /// This directory is used to store all files generated or read by the application
        /// </summary>
        static AbsolutePath()
        {
            try
            {
                /* If the directory doesn't already exist */
                if (!Directory.Exists(AbsolutePath.DirectPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(AbsolutePath.DirectPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Impossible de créer le dossier : " + e.ToString());
            }
        }
    }
}
