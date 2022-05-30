using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculConversionDevise
{
    public class ConversionDevise
    {
        private static Dictionary<string, List<DeviseCibleAvecTaux>> _dicDevise = new Dictionary<string, List<DeviseCibleAvecTaux>>();

        private string _deviseDepart;
        private double _montantAConvertirInitial;
        private string _deviseCible;

        private double _montantAConvertirIntermediaire; // nécesaire pour mémoriser les calculs intermédiares
        private bool _trouve; // nécessaire pour arrêter la recherhce dans l'arbre, le but étant d'être le plus efficace
        private List<string> _lstAlreadyDeviseParcourue;

        public static Dictionary<string, List<DeviseCibleAvecTaux>> DicDevise { get => _dicDevise; set => _dicDevise = value; }

        public ConversionDevise()
        {
            _lstAlreadyDeviseParcourue = new List<string>();
        }

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
            _lstAlreadyDeviseParcourue.Clear(); // indispensable quand on lance plusieurs calculs à la suite

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

            int resulat = CalculerConversion(deviseIntermediaire: deviseDepart, _lstAlreadyDeviseParcourue);

            if (resulat != 0)
            {
                Console.WriteLine($"Résultat => la conversion de {montantAConvertir} {deviseDepart} est égale à {resulat} {deviseCible} ");
            }
            else
            {
                Console.WriteLine($"Résultat => la conversion de {montantAConvertir} {deviseDepart} en {deviseCible} ne peut pas aboutir");
            }
            Console.WriteLine("-------------");

            return resulat;
        }

        /// <summary>
        /// Fonction principale récursive de calcul
        /// </summary>
        /// <param name="deviseIntermediaire"></param>
        /// <param name="lstAlreadyDeviseParcourue">indispensable pour les chemins qui tournent en boucle</param>
        /// <returns>le montant intermédiaire de conversion calculé</returns>
        private int CalculerConversion(string deviseIntermediaire, List<string> lstAlreadyDeviseParcourue)
        {
            lstAlreadyDeviseParcourue.Add(deviseIntermediaire);
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
                List<DeviseCibleAvecTaux> lstDeviseIntermediaireLaPlusCourte = SearchDeviseInterLaPlusCourte(_dicDevise[deviseIntermediaire]);

                //foreach (DeviseCibleAvecTaux deviseCibleIntermediaireAvecTaux in _dicDevise[deviseIntermediaire])
                foreach (DeviseCibleAvecTaux deviseCibleIntermediaireAvecTaux in lstDeviseIntermediaireLaPlusCourte)
                {
                    if (!_trouve)
                    {
                        // inutile de chercher une devise cible qui correspond à la devise de départ
                        if (!deviseCibleIntermediaireAvecTaux.DeviseCible.Equals(_deviseDepart))
                        {
                            _montantAConvertirIntermediaire = Math.Round(_montantAConvertirIntermediaire * deviseCibleIntermediaireAvecTaux.TauxChange, 4);
                            Console.WriteLine($"Conversion intermédiaire trouvée de {deviseIntermediaire} vers {deviseCibleIntermediaireAvecTaux.DeviseCible}, montant = {_montantAConvertirIntermediaire}");

                            if (!lstAlreadyDeviseParcourue.Contains(deviseCibleIntermediaireAvecTaux.DeviseCible))
                            {
                                CalculerConversion(deviseCibleIntermediaireAvecTaux.DeviseCible, lstAlreadyDeviseParcourue);
                            }
                            else
                            {
                                Console.WriteLine($"Devise {deviseIntermediaire} déjà parcourue, on sort de l'exploration récursive");
                            }
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

        /// <summary>
        /// recherche de la conversion la plus courte
        /// </summary>
        /// <param name="lstDeviseCibleAvecTaux"></param>
        /// <returns></returns>
        private List<DeviseCibleAvecTaux> SearchDeviseInterLaPlusCourte(List<DeviseCibleAvecTaux> lstDeviseCibleAvecTaux)
        {
            // ex : pour la conversion de AUD vers INR, on a AUD => CHF, JPY mais comme il existe une conversion directe (JPY => KWU, INR) il faut la balayer en premier lieu
            // on va donc inverser l'ordre de balayage pour explorer JPY en premier
            List<DeviseCibleAvecTaux> lstDeviseIntermediaireDansOrdreLeplusRapide = new List<DeviseCibleAvecTaux>();

            // on cherche parmi les taux de conversion intermédaire que l'on veut explorer si on a une conversion directe vers la cible
            // et si c'est le cas, alors on la prend en premier
            foreach (DeviseCibleAvecTaux deviseCibleAvecTauxIntermediaire in lstDeviseCibleAvecTaux) // AUD => CHF, JPY
            {
                DeviseCibleAvecTaux deviseCibleAvecTauxDirect = _dicDevise[deviseCibleAvecTauxIntermediaire.DeviseCible].Where(l => l.DeviseCible.Equals(_deviseCible)).FirstOrDefault();
                if (deviseCibleAvecTauxDirect != null)
                {
                    lstDeviseIntermediaireDansOrdreLeplusRapide.Add(deviseCibleAvecTauxIntermediaire);
                }
            }

            // on ajoute les autres conversions à la suite dans la liste
            foreach (DeviseCibleAvecTaux deviseCibleAvecTauxIntermediaire in lstDeviseCibleAvecTaux) 
            {
                if (!lstDeviseIntermediaireDansOrdreLeplusRapide.Contains(deviseCibleAvecTauxIntermediaire))
                {
                    lstDeviseIntermediaireDansOrdreLeplusRapide.Add(deviseCibleAvecTauxIntermediaire);
                }
            }

            return lstDeviseIntermediaireDansOrdreLeplusRapide;
        }
    }
}
