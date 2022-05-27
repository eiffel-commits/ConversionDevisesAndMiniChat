using System;

namespace CalculConversionDevies
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Programme de conversion des devises");

            ConversionDevise conversionDevise = new ConversionDevise();

            // mode sans argument => on utilise un jeu avec des devises de test
            if (args.Length == 0)
            {
                // pour les premiers tests, cela a permis de mettre au point l'algo sans dévélopper la lecture fichier dans un premier temps
                ConversionDevise.DicDevise = InitConfigDevise.InitDicInMemoryForTest();

                try
                {
                    //test1
                    conversionDevise.CalculerConversion(deviseDepart: "EUR", montantAConvertir: 550, deviseCible: "JPY");

                    //test2
                    conversionDevise.CalculerConversion(deviseDepart: "USD", montantAConvertir: 450, deviseCible: "JPY");

                    //test3
                    conversionDevise.CalculerConversion(deviseDepart: "USD", montantAConvertir: 150, deviseCible: "CHF");

                    //test4
                    //conversionDevise.CalculerConversion(deviseDepart: "AUD", montantAConvertir: 220, deviseCible: "INR");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            // mode File si un argument est transmis
            else
            {
                string fileName = args[0];
                if (!string.IsNullOrEmpty(fileName))
                {
                    // @"D:\Dev\ConversionDevises\CalculConversionDevies\Input.txt"
                    ConfigDevise configFileDevise = InitConfigDevise.LectureFichier(fileName);

                    ConversionDevise.DicDevise = configFileDevise.DicDevise;

                    //Appel de la conversion avec les valeurs du fichier
                    conversionDevise.CalculerConversion(configFileDevise.DeviseDepart, configFileDevise.MontantAConvertirInitial, configFileDevise.DeviseCible);
                }
            }
        }
    }
}
