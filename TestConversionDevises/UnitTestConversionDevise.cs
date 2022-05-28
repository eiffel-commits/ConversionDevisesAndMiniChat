using System;

using CalculConversionDevise;

using NUnit.Framework;

namespace TestConversionDevises
{
    public class Tests
    {
        ConversionDevise conversionDevise = new ConversionDevise();

        [OneTimeSetUp] // une seule fois pour tous les tests ici
        public void Setup()
        {

            ConversionDevise.DicDevise = InitConfigDevise.InitDicInMemoryForTest();
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(59033, conversionDevise.CalculerConversion(montantAConvertir: 550, deviseDepart: "EUR", deviseCible: "JPY"));
        }

        [Test]
        public void Test2()
        {
            Assert.AreEqual(37186, conversionDevise.CalculerConversion(montantAConvertir: 450, deviseDepart: "USD", deviseCible: "JPY"));
        }

        [Test]
        public void Test3()
        {
            Assert.AreEqual(418, conversionDevise.CalculerConversion(montantAConvertir: 450, deviseDepart: "USD", deviseCible: "CHF"));
        }

        public void Test4()
        {
            //test4 (cas qui implique de gérer les boucles infinies)
            Assert.AreEqual(12438, conversionDevise.CalculerConversion(deviseDepart: "AUD", montantAConvertir: 220, deviseCible: "INR"));
        }

        [Test]
        public void Test5()
        {
            // test avec une devise de départ qui n'existe pas 
            string deviseDepart = "URGGGGS";
            var ex = Assert.Throws<ApplicationException>(() => conversionDevise.CalculerConversion(montantAConvertir: 450, deviseDepart: deviseDepart, deviseCible: "CHF"));
            Assert.AreEqual($"la devise de départ {deviseDepart} n'est pas connue", ex.Message);
        }

        [Test]
        public void Test6()
        {
            // test avec une devise cible qui n'existe pas 
            string deviseCible = "URGYYYYGGGS";
            var ex = Assert.Throws<ApplicationException>(() => conversionDevise.CalculerConversion(montantAConvertir: 450, deviseDepart: "EUR", deviseCible: deviseCible));
            Assert.AreEqual($"la devise cible {deviseCible} n'est pas connue", ex.Message);
        }
    }
}