namespace PlantCatalogue.ViewModels
{
    public class UserRegister
    {
        ///<summary>
        ///E-mail címet tárol az adatbázis táblában
        ///</summary
        public string Email { get; set; }
        ///<summary>
        ///Jelszavakat tárol az adatbázis táblában
        ///</summary
        public string Password { get; set; }
        ///<summary>
        ///Jelszó megerősítéseket tárol az adatbázis táblában
        ///</summary
        public string ConfirmPassword { get; set; }
    }
}
