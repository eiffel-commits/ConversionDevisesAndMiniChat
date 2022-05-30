using System;
using System.Collections.Generic;

namespace CalculConversionDevise
{
    public class InitConfigDevise
    {
        /// <summary>
        /// Initialisation en mémoire pour test (sans lecture du fichier)
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, List<DeviseCibleAvecTaux>> InitDicInMemoryForTest()
        {
            Dictionary<string, List<DeviseCibleAvecTaux>> dicDevise = new Dictionary<string, List<DeviseCibleAvecTaux>>();

            dicDevise.Add("AUD", new List<DeviseCibleAvecTaux>());
            dicDevise.Add("JPY", new List<DeviseCibleAvecTaux>());
            dicDevise.Add("EUR", new List<DeviseCibleAvecTaux>());
            dicDevise.Add("CHF", new List<DeviseCibleAvecTaux>());
            dicDevise.Add("KWU", new List<DeviseCibleAvecTaux>());
            dicDevise.Add("USD", new List<DeviseCibleAvecTaux>());
            dicDevise.Add("INR", new List<DeviseCibleAvecTaux>());

            //avec taux direct
            dicDevise["AUD"].Add(new DeviseCibleAvecTaux("CHF", 0.9661)); //ligne 1
            dicDevise["JPY"].Add(new DeviseCibleAvecTaux("KWU", 13.1151)); //ligne 2
            dicDevise["EUR"].Add(new DeviseCibleAvecTaux("CHF", 1.2053)); //ligne 3
            dicDevise["AUD"].Add(new DeviseCibleAvecTaux("JPY", 86.0305)); //ligne 4
            dicDevise["EUR"].Add(new DeviseCibleAvecTaux("USD", 1.2989)); //ligne 5
            dicDevise["JPY"].Add(new DeviseCibleAvecTaux("INR", 0.6571)); //ligne 6

            //avec taux inversé
            dicDevise["CHF"].Add(new DeviseCibleAvecTaux("AUD", 1 / 0.9661)); //ligne 1
            dicDevise["KWU"].Add(new DeviseCibleAvecTaux("JPY", 1 / 13.1151)); //ligne 2
            dicDevise["CHF"].Add(new DeviseCibleAvecTaux("EUR", 1 / 1.2053)); //ligne 3
            dicDevise["JPY"].Add(new DeviseCibleAvecTaux("AUD", 1 / 86.0305)); //ligne 4
            dicDevise["USD"].Add(new DeviseCibleAvecTaux("EUR", 1 / 1.2989)); //ligne 5
            dicDevise["INR"].Add(new DeviseCibleAvecTaux("JPY", 1 / 0.6571)); //ligne 6

            return dicDevise;
        }

        /// <summary>
        /// lecture du fichier passé en paramètre
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static ConfigDevise LectureFichier(string fileName)
        {
            ConfigDevise configFileDevise = new ConfigDevise();

            // lecture fichier dans un tableau de string.
            // chaque élément du tableau correspond à une ligne du fichier
            string[] lines = System.IO.File.ReadAllLines(fileName);

            // Display the file contents by using a foreach loop.
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("\t" + line);
            }

            // lecture ligne n°1 (deviseDepart;Montant;deviseCible)
            int numLigne = 0;
            char separator = ';';
            configFileDevise.DeviseDepart = lines[numLigne].Split(separator)[0];
            configFileDevise.MontantAConvertirInitial = double.Parse(lines[numLigne].Split(separator)[1]);
            configFileDevise.DeviseCible = lines[numLigne].Split(separator)[2];
            numLigne++;

            // lecture ligne n°2 (nbTauxChange)
            int nbTauxChange = int.Parse(lines[numLigne]);
            numLigne++;

            //on lit le tableau de change à la fois dans le sens DeviseDepart => DeviseCible

            // Lecture ligne n°3 à n°3+nbDevises => Lecture dans le sens DeviseDepart => DeviseCible 
            // exemple de ce que l'on doit avoir dans le dico
            // clé, values
            // [AUD], (CHF, 0.9661);(JPY,86.0305) => ligne 1 et 4 du fichier
            // [JPY], (KWU, 13.1151);(INR,0.6571) => ligne 2 et 6 du fichier
            // [EUR], (CHF,1.2053);(USD;1.2989) => ligne 3 et 5 du fichier
            for (int numLigneTaux = numLigne; numLigneTaux < (nbTauxChange+ numLigne); numLigneTaux++)
            {
                string deviseDepart = lines[numLigneTaux].Split(separator)[0];
                string deviseCible = lines[numLigneTaux].Split(separator)[1];
                string taux = lines[numLigneTaux].Split(separator)[2];
                double tauxChange = double.Parse(taux.Replace('.',','));

                if (!configFileDevise.DicDevise.ContainsKey(deviseDepart))
                {
                    configFileDevise.DicDevise.Add(deviseDepart, new List<DeviseCibleAvecTaux>());
                }
                configFileDevise.DicDevise[deviseDepart].Add(new DeviseCibleAvecTaux(deviseCible, tauxChange));
            }

            return configFileDevise;
        }
    }
}
