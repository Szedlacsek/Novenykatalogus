using PlantCatalogue.Models;

namespace PlantCatalogue.ViewModels
{
    public class IndexSearch
    {
        ///<summary>
        ///A növények kikereshetőségéért felel
        ///</summary
        public IEnumerable<Plants> plent {get; set;}
        ///<summary>
        ///Kereső rendszer a főoldalon
        ///</summary
        public string Search { get; set;}
        ///<summary>
        ///Hibaüzenet a keresés során
        ///</summary
        public string error { get; set;}
        ///<summary>
        ///Felhasználók ID-ja, amelyeket megkapnak az oldalak
        ///</summary
        public int UserId { get; set;}
    }
}
