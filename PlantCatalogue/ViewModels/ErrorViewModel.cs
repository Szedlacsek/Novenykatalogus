namespace PlantCatalogue.Models
{
    public class ErrorViewModel
    {
        ///<summary>
        ///Hiba az odlall történõ kommunikáció során
        ///</summary
        public string? RequestId { get; set; }
        ///<summary>
        ///Hiba az odlall történõ kommunikáció során
        ///</summary
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}