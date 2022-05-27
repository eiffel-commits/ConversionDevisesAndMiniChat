using System.Collections.Generic;

namespace CalculConversionDevies
{
    public class ConfigDevise
    {
        private Dictionary<string, List<DeviseCibleAvecTaux>> _dicDevise = new Dictionary<string, List<DeviseCibleAvecTaux>>();
        private string _deviseDepart;
        private double _montantAConvertirInitial;
        private string _deviseCible;

        public string DeviseDepart { get => _deviseDepart; set => _deviseDepart = value; }
        public double MontantAConvertirInitial { get => _montantAConvertirInitial; set => _montantAConvertirInitial = value; }
        public string DeviseCible { get => _deviseCible; set => _deviseCible = value; }
        public Dictionary<string, List<DeviseCibleAvecTaux>> DicDevise { get => _dicDevise; set => _dicDevise = value; }
    }
}
