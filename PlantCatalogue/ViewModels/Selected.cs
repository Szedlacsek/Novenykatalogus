namespace PlantCatalogue.ViewModels
{
    public class Selected
    {
        ///<summary>
        ///A kiválasztott kártyán lévő növény ID-jének átvitele
        ///</summary
        public int PlantId { get; set; }
        ///<summary>
        ///A kiválasztott kártyán lévő növény nevének átvitele
        ///</summary
        public string PlantName { get; set; }
        ///<summary>
        ///A főoldalról tovább menő felhasználói ID
        ///</summary
        public int UserId { get; set; }
    }
}
