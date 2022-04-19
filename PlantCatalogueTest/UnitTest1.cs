using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PlantCatalogue;
using PlantCatalogue.Controllers;
using PlantCatalogue.ViewModels;

namespace PlantCatalogueTest
{
    public class Tests
    {
        public Services servi = new Services();
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void Test1()
        {
            string val = "alma"; //Jelszó titkosítás nélkül
            Assert.AreEqual("qLXSPBo5ajX6ZYr1B5vWiw==", servi.Encription(val)); //Jelszó a titkosítás után
        }
        [Test]
        public void Test2()
        {
            string val = "qLXSPBo5ajX6ZYr1B5vWiw=="; 
            Assert.AreEqual("alma", servi.Decription(val));

        }
        [Test]
        public void Test3() //Fájlbeolvasás
        {
            Assert.AreNotEqual("", servi.FromFile(@"G:\Vizsga_Reloaded\PlantCatalogue\PlantCatalogue\wwwroot\forgot.html"));
        }

       
    }
}