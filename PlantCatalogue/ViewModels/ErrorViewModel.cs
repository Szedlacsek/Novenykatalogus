namespace PlantCatalogue.Models
{
    public class ErrorViewModel
    {
        ///<summary>
        ///Hiba az odlall t�rt�n� kommunik�ci� sor�n
        ///</summary
        public string? RequestId { get; set; }
        ///<summary>
        ///Hiba az odlall t�rt�n� kommunik�ci� sor�n
        ///</summary
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}