using System.ComponentModel.DataAnnotations;

namespace PlantCatalogue.Models
{
    public class Tools
    {
        ///<summary>
        ///Eszközök id-je
        ///</summary
        [Key]
        public int ToolsId { get; set; }
        ///<summary>
        ///Eszközök nevei
        ///</summary
        [Required]
        public string ToolsName { get; set; }
        ///<summary>
        ///Eszközök típusai
        ///</summary
        public string ToolsType { get; set; }
        ///<summary>
        ///Növények leírása
        ///</summary
        public string ToolsDesc { get; set; }
    }
}
