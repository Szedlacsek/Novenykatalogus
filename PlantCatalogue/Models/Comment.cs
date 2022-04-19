using System.ComponentModel.DataAnnotations;

namespace PlantCatalogue.Models
{
    public class Comment
    {
        ///<summary>
        ///Tábla létrehozásakor létrejövő ID
        ///</summary
        [Key]
        public int Id { get; set; }
        ///<summary>
        ///Felhasználók ID-ja ami regisztráció során jön létre
        ///</summary
        [Required]
        public int UserId { get; set; }
        ///<summary>
        ///Növények ID-je
        ///</summary
        [Required]
        public int PlantId { get; set; }
        ///<summary>
        ///A növényrek alatt megjelenő kommentek
        ///</summary
        [Required]
        public string Comm { get; set; }
        
        
    }
}
