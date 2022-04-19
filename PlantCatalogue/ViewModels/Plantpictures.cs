namespace PlantCatalogue.Models
{
    public class Plantpictures
    {
        ///<summary>
        ///A főoldallon lévő növény neve
        ///</summary
        public string PlantName { get; set; }
        ///<summary>
        ///A főoldallon lévő növény latin neve
        ///</summary
        public string LatinName { get; set; }
        ///<summary>
        ///A főoldallon lévő növény eléretősége (látható vagy nem látható)
        ///</summary
        public bool Available { get; set; }
        ///<summary>
        ///A főoldallon lévő növény képe
        ///</summary
        public IFormFile Picture { get; set; }
    }
}
