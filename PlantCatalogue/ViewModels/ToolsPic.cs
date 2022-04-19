namespace PlantCatalogue.Models
{
    public class ToolsPic
    {
        ///<summary>
        ///Adatok létrehozása során, automatikusan megjelenő id
        ///</summary
        public int Id { get; set; }
        ///<summary>
        ///Növény ID-je, amit meg kell jeleníteni az adott időben
        ///</summary
        public int PlantId { get; set; }
        ///<summary>
        ///Növény neve, amit meg kell jeleníteni az adott időben
        ///</summary
        public string PlantName { get; set; }
        ///<summary>
        ///Növényhez tartozó kommentek, amit meg kell jeleníteni az adott időben
        ///</summary
        public string Comm { get; set; }
        ///<summary>
        ///Növényhez tartozó eszközök, amit meg kell jeleníteni az adott időben
        ///</summary
        public IEnumerable<Tools> Tool { get; set; }
        ///<summary>
        ///Növények alatti komment szekcióhoztartozó e-mail címek, amit meg kell jeleníteni az adott időbenl
        ///</summary
        public IEnumerable<string> Emails { get; set; }
        ///<summary>
        ///Növények alatti komment szekcióhoztartozó kommentek címek, amit meg kell jeleníteni az adott időbenl
        ///</summary
        public IEnumerable<string> Comments { get; set; }
    }
}

