using System.ComponentModel.DataAnnotations;

namespace PlantCatalogue.Models
{
    public class Plants
    {
        ///<summary>
        ///Növények ID-je
        ///</summary
        [Key]
        public int PlantId { get; set; }
        ///<summary>
        ///Növények neve
        ///</summary
        [Required]
        public string PlantName { get; set; }
        ///<summary>
        ///Növények latin neve
        ///</summary
        public string LatinName { get; set; }
        ///<summary>
        ///Növény elérhetősége (látható-e vagy sem)
        ///</summary
        public bool Available { get; set; }   

    }
}
