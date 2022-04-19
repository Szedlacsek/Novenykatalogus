using System.ComponentModel.DataAnnotations;

namespace PlantCatalogue.Models
{
    public class Switch
    {
        ///<summary>
        ///Összeköttetések Id-je
        ///</summary
        [Key]
        public int SwitchId { get; set; }
        ///<summary>
        ///Összeköttetéskor felhasznált növény id-je
        ///</summary
        [Required]
        public int PlantsId { get; set; }
        ///<summary>
        ///Összeköttetéskor felhasznált eszközök id-je
        ///</summary
        [Required]
        public int ToolsId { get; set; }

    }
}
