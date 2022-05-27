using System;

namespace CalculConversionDevies
{
    public class DeviseCibleAvecTaux
    {
        private string _deviseCible;
        private double _tauxChange;

        public DeviseCibleAvecTaux(string deviseCible, double tauxChange)
        {
            _deviseCible = deviseCible;
            TauxChange = tauxChange; // attention, on passe par l'accesseur ici pour l'arrondi à 4c.
        }

        public string DeviseCible { get => _deviseCible; set => _deviseCible = value; }
        public double TauxChange 
        {
            get => _tauxChange; // Math.Round(_tauxChange,4); 
            set => _tauxChange = Math.Round(value, 4); // Règle de gestion => arrondi du taux à 4 chiffres qui sera nécessaire dans le cas où le taux est inversé
        }
    }
}
