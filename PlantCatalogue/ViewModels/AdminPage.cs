namespace PlantCatalogue.Models
{
    public class AdminPage
    {
        ///<summary>
        ///Az admin oldalon lévő növények tábla
        ///</summary
        public IEnumerable<Plants> plants { get; set; }
        ///<summary>
        ///Az admin oldalon lévő kapcsolás tábla
        ///</summary
        public IEnumerable<Switch> switchi { get; set; }
        ///<summary>
        ///Az admin oldalon lévő eszközök tábla
        ///</summary
        public IEnumerable<Tools> tools { get; set; }
        ///<summary>
        ///Az admin oldalon lévő felhasználók tábla
        ///</summary
        public IEnumerable<User> users { get; set; }
        ///<summary>
        ///Az admin oldalon lévő kommentek tábla
        ///</summary
        public IEnumerable<Comment> comments { get; set; }
        ///<summary>
        ///Hibaüzenet az oldal betöltése során
        ///</summary
        public string error { get; set; }
    }
}