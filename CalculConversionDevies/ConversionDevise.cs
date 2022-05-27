using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculConversionDevies
{
    public class ConversionDevise
    {
        private static Dictionary<string, List<DeviseCibleAvecTaux>> _dicDevise = new Dictionary<string, List<DeviseCibleAvecTaux>>();

        private string _deviseDepart;
        private double _montantAConvertirInitial;
        private string _deviseCible;

        private double _montantAConvertirIntermediaire; // nécesaire pour mémoriser les calculs intermédiares
        private bool _trouve; // nécessaire pour arrêter la recherhce dans l'arbre, le but étant d'être le plus efficace

        public static Dictionary<string, List<DeviseCibleAvecTaux>> DicDevise { get => _dicDevise; set => _dicDevise = value; }

        

        /// <summary>
        /// Fonction de calcul appelée par l'utilisateur
        /// </summary>
        /// <param name="deviseDepart"></param>
        /// <param name="montantAConvertir"></param>
        /// <param name="deviseCible"></param>
        /// <returns>le montant de conversion calculé</returns>
        public int CalculerConversion(string deviseDepart, double montantAConvertir, string deviseCible)
        {
            _deviseDepart = deviseDepart;
            _montantAConvertirInitial = montantAConvertir;
            _deviseCible = deviseCible;

            // pour la fonction de calcul récursive
            _montantAConvertirIntermediaire = _montantAConvertirInitial;
            _trouve = false;

            Console.WriteLine($"Conversion de {montantAConvertir} {deviseDepart} en {deviseCible} ");

            // on teste que la devise de départ fasse bien partie du dico
            if (!_dicDevise.ContainsKey(_deviseDepart))
            {
                throw new ApplicationException($"la devise de départ {_deviseDepart} n'est pas connue");
            }

            // on teste que la devise cible fasse bien partie du dico
            if (!_dicDevise.ContainsKey(_deviseCible))
            {
                throw new ApplicationException($"la devise cible {_deviseCible} n'est pas connue");
            }

            int resulat = CalculerConversion(deviseIntermediaire: deviseDepart);


            Console.WriteLine($"Résultat => la conversion de {montantAConvertir} {deviseDepart} est égale à {resulat} {deviseCible} ");
            Console.WriteLine("-------------");

            return resulat;
        }

        /// <summary>
        /// Fonction principale récursive de calcul
        /// </summary>
        /// <param name="deviseIntermediaire"></param>
        /// <returns>le montant intermédiaire de conversion calculé</returns>
        private int CalculerConversion(string deviseIntermediaire)
        {
            // etape 1 => on cherche s'il existe une conversion d'arrivee simplement dans les valeurs du dico pour la clé correspondante à la devise de départ
            DeviseCibleAvecTaux deviseCibleAvecTauxDirect = _dicDevise[deviseIntermediaire].Where(l => l.DeviseCible.Equals(_deviseCible)).FirstOrDefault();
            if (deviseCibleAvecTauxDirect != null)
            {
                _montantAConvertirIntermediaire = Math.Round(_montantAConvertirIntermediaire * deviseCibleAvecTauxDirect.TauxChange, 4);
                Console.WriteLine($"Conversion directe trouvée de {deviseIntermediaire} vers {_deviseCible}, montant = {_montantAConvertirIntermediaire}");
                _trouve = true;
            }
            else
            {
                // etape 2 => on n'a pas trouvé tout de suite de conversion, on doit balayer chaque valeur du dico pour la devise recherchée et relancer la recherche de façon récursive
                // en prenant la valeur et en la considérant comme la devise de départ de façon à explorer tout l'arbre
                // de façon à optimiser, il n'y a pas besoin de balyer tout l'arbe dès qu'une branche a permis de trouver la conversion en direct (étape1),
                // d'où le test avec le booléen trouve
                foreach (DeviseCibleAvecTaux deviseCibleIntermediaireAvecTaux in _dicDevise[deviseIntermediaire])
                {
                    if (!_trouve)
                    {
                        // inutile de chercher une devise cible qui correspond à la devise de départ
                        if (!deviseCibleIntermediaireAvecTaux.DeviseCible.Equals(_deviseDepart))
                        {
                            _montantAConvertirIntermediaire = Math.Round(_montantAConvertirIntermediaire * deviseCibleIntermediaireAvecTaux.TauxChange, 4);
                            Console.WriteLine($"Conversion intermédiaire trouvée de {deviseIntermediaire} vers {deviseCibleIntermediaireAvecTaux.DeviseCible}, montant = {_montantAConvertirIntermediaire}");
                            CalculerConversion(deviseCibleIntermediaireAvecTaux.DeviseCible);
                        }
                        // c'est un chemin sans issue, on doit poursuivre mais il faut ré-affecter le montant intermédiaire au montant initial
                        else
                        {
                            Console.WriteLine($"Voie sans issue, la devise cible {deviseCibleIntermediaireAvecTaux.DeviseCible} est égale à la devise de départ");
                            _montantAConvertirIntermediaire = _montantAConvertirInitial;
                        }
                    }
                }
            }

            // règle de gestion => le résulat doit être arrondi à l'entier supérieur.
            return (int)Math.Ceiling(_montantAConvertirIntermediaire);
        }
    }
}
