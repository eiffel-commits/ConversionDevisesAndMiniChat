using System;

using CalculConversionDevies;

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

        [Test]
        public void Test4()
        {
            // test avec une devise de départ qui n'existe pas 
            string deviseDepart = "URGGGGS";
            var ex = Assert.Throws<ApplicationException>(() => conversionDevise.CalculerConversion(montantAConvertir: 450, deviseDepart: deviseDepart, deviseCible: "CHF"));
            Assert.AreEqual($"la devise de départ {deviseDepart} n'est pas connue", ex.Message);
        }

        [Test]
        public void Test5()
        {
            // test avec une devise cible qui n'existe pas 
            string deviseCible = "URGYYYYGGGS";
            var ex = Assert.Throws<ApplicationException>(() => conversionDevise.CalculerConversion(montantAConvertir: 450, deviseDepart: "EUR", deviseCible: deviseCible));
            Assert.AreEqual($"la devise cible {deviseCible} n'est pas connue", ex.Message);
        }
    }
}