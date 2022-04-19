namespace PlantCatalogue.ViewModels
{
    public class Change
    {
        ///<summary>
        ///Az ID amelyhez módosul a jelszó
        ///</summary
        public int Id { get; set; }
        ///<summary>
        ///Jelszó adatbázisba vitele
        ///</summary
        public string Password { get; set; }
        ///<summary>
        ///Újbóli jelszó adatbázisba vitele
        ///</summary
        public string ConfirmPassword { get; set; }
    }
}
