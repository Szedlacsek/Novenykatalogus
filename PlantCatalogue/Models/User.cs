using System.ComponentModel.DataAnnotations;

namespace PlantCatalogue.Models
{
    public class User
    {
        ///<summary>
        ///Tábla készítésekor létrejött id
        ///</summary
        [Key]
        public int Id { get; set; }
        ///<summary>
        ///E-mail tábla
        ///</summary
        [Required]
        public string Email { get; set; }
        ///<summary>
        ///Jelszó tábla
        ///</summary
        [Required]
        public string Password { get; set; }
        ///<summary>
        ///Felhasználó hitelesítés
        ///</summary
        [Required]
        public bool Valid { get; set; }
    }
} 