using Microsoft.AspNetCore.Mvc;
using PlantCatalogue.Data;
using PlantCatalogue.Models;
using PlantCatalogue.ViewModels;
using System.Diagnostics;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;


namespace PlantCatalogue.Controllers
{
    public class HomeController : Controller
    {
        public Services servi = new Services();
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHost;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IWebHostEnvironment webhost)
        {
            _logger = logger;
            _db = db;
            this.webHost = webhost;
        }

        ///<summary>
        ///Főoldal amely össze van kötve adatbázissal
        ///</summary>
        ///<param name="searchi">A kereső felületért felelős paraméter</param>
        public IActionResult Index(IndexSearch searchi)
        {
            Tarolas val = new Tarolas();

            if (searchi.Search == val.Id)
            {
                return RedirectToAction("Admin");
            }
            else if (searchi.Search != "" && searchi != null && searchi.Search != null)
            {
                searchi.plent = _db.Plants.Where(x => x.PlantName.Contains(searchi.Search));
            }
            else
            {
                searchi.plent = _db.Plants;  
            }
            return View(searchi);
        }
        ///<summary>
        ///A kereső felület által kiadott kártyák
        ///</summary>
        ///<param name="sear">A kereső felületért felelős paraméter mely a beirt adat alapján keresi ki a növény ártyákat és jeleníti meg</param>
        [HttpPost]
        public IActionResult OutSearch([Bind(include: "Search,UserId")] IndexSearch sear)
        {
            return RedirectToAction("Index", sear);
        }

        ///<summary>
        ///A regisztrációs oldal megjelenítéséért felelős metódus
        ///</summary>
        public IActionResult RegLogin(Reglogin re)
        {
            return View(re);
        }
        ///<summary>
        ///A bejelentkező folyamat lezajlása
        ///</summary>
        ///<param name="user">A beírt adatok alapján bejelentkeztet az oldal</param>
        [HttpPost]
        public IActionResult Login([Bind(include: "Email,Password")] User user)
        {
            string error = "";
            IndexSearch ind = new IndexSearch();
            if (user != null)
            {
                try
                {
                    if (_db.User.Any(x => x.Email == user.Email && x.Password == servi.Encription(user.Password) && x.Valid == true))
                    { 
                        ind.UserId = _db.User.Where(x => x.Email == user.Email && x.Password == servi.Encription(user.Password) && x.Valid == true).FirstOrDefault().Id;
                        string path = ind.UserId.ToString();
                        return RedirectToAction("Index", ind);
                    }
                    else
                    {
                        Reglogin re = new Reglogin();
                        re.error = "Az email cím vagy a jelszó hibás";
                        return RedirectToAction("RegLogin", re);
                    }
                }
                catch
                {
                    Reglogin re = new Reglogin();
                    re.error = "Hiba a beléptetés közben";
                    return RedirectToAction("RegLogin", re);
                }
            }
            else
            {
                Reglogin re = new Reglogin();
                re.error = "Kitöltetlen mező";
                return RedirectToAction("RegLogin", re);
            }
        }
        ///<summary>
        ///Az elfelejtett jelszóra vonatkozó metódus
        ///</summary>
        ///<param name="user">Ha az E-mail mező ki van töltve, akkor kérhető a jelszó emlékeztető</param>
        [HttpPost]
        public IActionResult Forgott([Bind(include: "Email")] UserRegister user)
        {
            Reglogin re = new Reglogin();
            if (user == null)
            {
                re.error = "Nincs kitöltve az email mező";
            }
            else
            {
                try
                {
                    if (_db.User.Any(x => x.Email == user.Email))
                    {
                        string MailBody = "";
                        string email = servi.FromFile(webHost.WebRootPath + @"\forgot.html");
                        MailBody = "<a href = 'https://" + Url.ActionContext.HttpContext.Request.Host.ToString() +"/Home/Forgotpw/" + _db.User.Where(x => x.Email == user.Email).FirstOrDefault().Id + "' > Jelszó csere </a>";
                        email = email.Replace("{bod}", MailBody);
                        string to = user.Email;
                        string from = "test.teszt123123@gmail.com";
                        MailMessage message = new MailMessage(from, to);
                        message.Subject = "Elfelejtett jelszó";
                        message.Body = email;
                        message.BodyEncoding = Encoding.UTF8;
                        message.IsBodyHtml = true;
                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                        System.Net.NetworkCredential basicCredential1 = new
                        System.Net.NetworkCredential("test.teszt123123@gmail.com", "AbC123456");
                        client.EnableSsl = true;
                        client.UseDefaultCredentials = false;
                        client.Credentials = basicCredential1;
                        client.Send(message);
                    }
                    re.error = "Nem létező email cím";
                }
                catch
                {
                    re.error = "Az email küldése sikertelen volt";
                }
            }
            return RedirectToAction("RegLogin", re);
        }
        ///<summary>
        ///Az elfelejtett jelszó oldal megjelenítéséért felelős metódus
        ///</summary>
        public IActionResult Forgotpw(Confirm forgot)
        {
            return View(forgot);
        }
        ///<summary>
        ///Az elfelejtett jelszóra vonatkozó, jelszó megváltoztató metódus
        ///</summary>
        ///<param name="change">A jelszó emlékeztető kiküldésére szolgáló paraméter</param>
        [HttpPost]
        public IActionResult Forgotpws([Bind(include: "Id,Password,ConfirmPassword")] Change change)
        {
            Reglogin re = new Reglogin();
            if (change != null)
            {
                if (change.Password == change.ConfirmPassword)
                {
                    try
                    {
                        User us = _db.User.Find(change.Id);
                        us.Password = servi.Encription(change.Password);
                        _db.SaveChanges();
                    }
                    catch
                    {
                        re.error = "Hiba a jelszó megváltoztatása közben";
                    }
                }
                {
                    re.error = "A jelszavak nem egyeznek";
                }
            }
            else
            {
                re.error = "Üres mező";
            }
            return RedirectToAction("RegLogin", re);
        }
        ///<summary>
        ///A regisztrációt lebonyolító metódus
        ///</summary>
        ///<param name="user">Regisztráció során kiküldött megerősítő e-mail</param>
        [HttpPost]
        public IActionResult Register([Bind(include: "Email,Password,ConfirmPassword")] UserRegister user)
        {
            string error = "";
            if (user != null)
            {
                if (user.Password == user.ConfirmPassword)
                {
                    if (_db.User.Any(x => x.Email == user.Email))
                    {
                        error = "Az email cím már létezik";
                    }
                    else
                    {
                        try
                        {
                            User us = new User();
                            us.Valid = false;
                            us.Email = user.Email;
                            us.Password = servi.Encription(user.Password);
                            _db.User.Add(us);
                            _db.SaveChanges();
                            error = "Kérjük hitelesítse regisztrációját";
                        }
                        catch
                        {
                            error = "A felhasználó létrehozása sikertelen volt";
                        }
                        try
                        {
                            if (_db.User.Any(x => x.Email == user.Email))
                            {
                                string MailBody = "";
                                string email = servi.FromFile(webHost.WebRootPath + @"\confirm.html");

                                MailBody = "<a href = 'https://" 
                                    + Url.ActionContext.HttpContext.Request.Host.ToString() 
                                    + "/Home/Confirm/" + _db.User.Where(x => x.Email == user.Email).FirstOrDefault().Id 
                                    + "' > megerősítő link </a>";

                                email = email.Replace("{bod}", MailBody);
                                string to = user.Email;
                                string from = "test.teszt123123@gmail.com";
                                MailMessage message = new MailMessage(from, to);
                                message.Subject = "Megerősítő email";
                                message.Body = email;
                                message.BodyEncoding = Encoding.UTF8;
                                message.IsBodyHtml = true;
                                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                                System.Net.NetworkCredential basicCredential1 = new
                                System.Net.NetworkCredential("test.teszt123123@gmail.com", "AbC123456");
                                client.EnableSsl = true;
                                client.UseDefaultCredentials = false;
                                client.Credentials = basicCredential1;
                                client.Send(message);
                            }
                        }
                        catch
                        {
                            error = "Az email küldése sikertelen volt";
                        }
                    }
                }
                else
                {
                    error = "A két jelszó nem egyezik";
                }
            }
            else
            {
                error = "Kitöltetlen mező";
            }
            Reglogin re = new Reglogin();
            re.error = error;
            return RedirectToAction("RegLogin", re);
        }

        public IActionResult Confirm(Confirm confirm)
        {
            return View(confirm);
        }
        ///<summary>
        ///Validálásért felelős metódus
        ///</summary>
        ///<param name="confirm">A regisztráció hitelesítéséért felel</param>
        [HttpPost]
        public IActionResult Validate([Bind(include: "Id")] Confirm confirm)
        {
            Reglogin re = new Reglogin();
            try
            {
                User us = _db.User.Find(confirm.Id);
                us.Valid = true;
                _db.SaveChanges();
                re.error = "Sikeres hitelesítés";
            }
            catch
            {
                re.error = "Hiba lépett fel a hitelesítés közben";
            }
            return RedirectToAction("RegLogin", re);
        }

        ///<summary>
        ///Privacy-ért felelős metódus
        ///</summary>
        public IActionResult Privacy()
        {
            return View();
        }

        ///<summary>
        ///Az Admin page adatainak létezéséért felelős metódus
        ///</summary>
        public IActionResult Admin(AdminPage error)
        {
            AdminPage admin = new AdminPage();
            admin.plants = _db.Plants;
            admin.tools = _db.Tools;
            admin.switchi = _db.Switch;
            admin.comments = _db.Comment;
            admin.users = _db.User;
            if (error.error != null)
            {
                admin.error = error.error;
            }
            return View(admin);
        }

        ///<summary>
        ///A képek <c>lementéséért</c> illetve az adatbázisban <c>új adat felviteléért</c> felelős metódus
        ///</summary>
        ///<param name="pic">A Plants Modelből származó értékek illetve PlantName alapján lementendő képekért felelős paraméter</param>
        [HttpPost]
        public async Task<IActionResult> PlantPage([Bind(include: "PlantName, LatinName, Available, Picture")] Plantpictures pic)
        {
            string error = "";
            if (pic != null)
            {
                Plants plant = new Plants();
                plant.PlantName = pic.PlantName;
                plant.LatinName = pic.LatinName;
                plant.Available = pic.Available;
                try
                {
                    if (pic.Picture != null)
                    {
                        string wwwR = webHost.WebRootPath + "/pictures/";
                        string filename = pic.PlantName + "-.jpg";
                        string path = Path.Combine(wwwR, filename);
                        using (FileStream fileStream = new FileStream(path, FileMode.Create))
                        {
                            await pic.Picture.CopyToAsync(fileStream);
                        }
                    }
                }
                catch
                {
                    error = "A kép feltöltése sikertelen";
                }
                try
                {
                    _db.Plants.Add(plant);
                    _db.SaveChanges();
                }
                catch
                {
                    error = "A növény mentése sikertelen";
                }
            }
            else
            {
                error = "Kitöltetlen mező";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }
        ///<summary>
        ///Az adatbázisban lévő növények <c>adatainak módostásáért</c> felelős metódus
        ///</summary>
        ///<param name="plants">Az AdminPage-en történő, adatbázis Update-ért felelős paraméter </param>
        [HttpPost]
        public IActionResult PlantUpdate([Bind(include: "PlantName, LatinName, Available, PlantId")] Plants plants)
        {
            string error = "";
            if (plants != null)
            {
                var obj = _db.Plants.Find(plants.PlantId);
                try
                {
                    _db.Plants.Remove(obj);
                    _db.Plants.Add(plants);
                    _db.SaveChanges();
                }
                catch
                {
                    error = "Hiba a növény válttoztatásakor";
                }
                try
                {
                    if (obj.PlantName != plants.PlantName)
                    {
                        //Kép átnevezése
                        FileInfo file = new FileInfo(Path.Combine(webHost.WebRootPath + "/pictures/", obj.PlantName + "-.jpg"));
                        file.MoveTo(Path.Combine(webHost.WebRootPath + "/pictures/", plants.PlantName + "-.jpg"));
                    }
                }
                catch
                {
                    error = "Hiba a kép módosításakor";
                }
            }
            else
            {
                error = "Kitöltetlen mező";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }

        ///<summary>
        ///Az adatbázisban lévő növények <c>törléséért</c> felelős metódus
        ///</summary>
        ///<param name="plants">Az AdminPage-en történő, adatbázis Delete-ért felelős paraméter </param>
        [HttpPost]
        public IActionResult DeletePlants([Bind(include: "PlantId, PlantName")] Plants plants)
        {
            string error = "";
            try
            {
                _db.Plants.Remove(plants);
            }
            catch
            {
                error = "A növény törlése sikertelen volt";
            }
            try
            {
                IEnumerable<Models.Switch> switches = _db.Switch.Where(x => x.PlantsId == plants.PlantId);
                if (switches.Count() > 0)
                {
                    foreach (var item in switches)
                    {
                        _db.Switch.Remove(item);
                    }
                }
                _db.SaveChanges();
            }
            catch
            {
                error = "A növény kapcsolatainak törlése sikertelen volt";
            }
            try
            {
                //kép törlése a szerverről
                FileInfo file = new FileInfo(Path.Combine(webHost.WebRootPath + "/pictures/", plants.PlantName + "-.jpg"));
                file.Delete();
            }
            catch
            {
                error = "A kép törlése sikertelen volt";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }
        ///<summary>
        ///Az adatbázisban az eszközöknél <c>új adat felviteléért</c> felelős metódus
        ///</summary>
        ///<param name="tools">Az AdminPage-en történő, adatbázis Create-ért felelős paraméter </param>
        [HttpPost]
        public IActionResult ToolsCreate([Bind(include: "ToolsName, ToolsType, ToolsDesc")] Tools tools)
        {
            string error = "";
            try
            {
                if (tools != null)
                {
                    _db.Tools.Add(tools);
                    _db.SaveChanges();
                }
                else
                {
                    error = "Kitöltetlen mező";
                }
            }
            catch
            {
                error = "A szerszám mentése sikertelen volt";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }

        ///<summary>
        ///Az adatbázisban az eszközök <c>adatainak módostásáért</c> felelős metódus
        ///</summary>
        ///<param name="tools">Az AdminPage-en történő, adatbázis Update-ért felelős paraméter </param>
        [HttpPost]
        public IActionResult ToolsUpdate([Bind(include: "ToolsName, ToolsType, ToolsDesc, ToolsId")] Tools tools)
        {
            string error = "";
            try
            {
                if (tools != null)
                {
                    _db.Tools.Update(tools);
                    _db.SaveChanges();
                }
                else
                {
                    error = "Kitöltetlen mező";
                }
            }
            catch
            {
                error = "A szerszám mentése sikertelen volt";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }

        ///<summary>
        ///Az adatbázisban lévő eszközök <c>törléséért</c> felelős metódus
        ///</summary>
        ///<param name="tools">Az AdminPage-en történő, adatbázis Delete-ért felelős paraméter </param>
        [HttpPost]
        public IActionResult ToolsDelete([Bind(include: "ToolsId")] Tools tools)
        {
            string error = "";
            try
            {
                if (tools != null)
                {
                    try
                    {
                        IEnumerable<Models.Switch> switches = _db.Switch.Where(x => x.ToolsId == tools.ToolsId);
                        if (switches.Count() > 0)
                        {
                            foreach (var item in switches)
                            {
                                _db.Switch.Remove(item);
                            }
                        }
                    }
                    catch
                    {
                        error = "A szerszám kapcsolatainak törlése sikertelen volt";
                    }
                    _db.Tools.Remove(tools);
                    _db.SaveChanges();
                }
                else
                {
                    error = "Kitöltetlen mező";
                }
            }
            catch
            {
                error = "A szerszám törlése sikertelen volt";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }

        ///<summary>
        ///Az adatbázisban a növények és eszközök <c>ID-jainak összekapcsolásának létrehozásáért</c> felelős metódus
        ///</summary>
        ///<param name="switchi">Az AdminPage-en történő, adatbázis <c>adat(ID) összekapcsolásért</c> felelős paraméter </param>
        [HttpPost]
        public IActionResult SwitchCreate([Bind(include: "PlantsId, ToolsId")] Models.Switch switchi)
        {
            string error = "";
            if (switchi != null)
            {
                try
                {
                    _db.Switch.Add(switchi);
                    _db.SaveChanges();
                }
                catch
                {
                    error = "A kapcsolat mentése sikertelen volt";
                }
            }
            else
            {
                error = "Kitöltetlen mező";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }

        ///<summary>
        ///Az adatbázisban a növények és eszközök <c>ID-jainak módostásáért</c> felelős metódus
        ///</summary>
        ///<param name="switchi">Az AdminPage-en történő, adatbázis <c>ID-k módosításáért</c> felelős paraméter </param>
        [HttpPost]
        public IActionResult SwitchUpdate([Bind(include: "PlantsId, ToolsId, SwitchId")] Models.Switch switchi)
        {
            string error = "";
            if (switchi != null)
            {
                try
                {
                    _db.Switch.Update(switchi);
                    _db.SaveChanges();
                }
                catch
                {
                    error = "A kapcsolat átírása sikertelen volt";
                }
            }
            else
            {
                error = "Kitöltetlen mező";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }

        ///<summary>
        ///Az adatbázisban lévő növények és eszközök <c>ID összekapcsolásának törléséért</c> felelős metódus
        ///</summary>
        ///<param name="switchi">Az AdminPage-en történő, adatbázis <c>ID-k törléséért</c> felelős paraméter </param>
        [HttpPost]
        public IActionResult SwitchDelete([Bind(include: "SwitchId")] Models.Switch switchi)
        {
            string error = "";
            if (switchi != null)
            {
                try
                {
                    _db.Switch.Remove(switchi);
                    _db.SaveChanges();
                }
                catch
                {
                    error = "A kapcsolat törlése sikertelen volt";
                }
            }
            else
            {
                error = "Kitöltetlen mező";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }
        ///<summary>
        ///Az adatbázisban felhasználók beírásáért felelős metódus
        ///</summary>
        ///<param name="user">Az AdminPage-en történő, adatbázisba <c>felhasználók</c> beírásáért felelős paraméter </param>
        [HttpPost]
        public IActionResult UserCreate([Bind(include: "Email,Password,Valid")] User user)
        {
            string error = "";
            if (user != null)
            {
                try
                {
                    user.Password = servi.Encription(user.Password);
                    _db.User.Add(user);
                    _db.SaveChanges();
                }
                catch
                {
                    error = "A felhasználó létrehozása sikertelen volt";
                }
            }
            else
            {
                error = "Kitöltetlen mező";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }
        ///<summary>
        ///Az adatbázisban felhasználók adatainak átírásáért felelős metódus
        ///</summary>
        ///<param name="user">Az AdminPage-en történő, adatbázisba <c>felhasználók adatainak</c> frissítéséért felelős paraméter </param>
        [HttpPost]
        public IActionResult UserUpdate([Bind(include: "Id, Email,Password, Valid")] User user)
        {
            string error = "";
            if (user != null)
            {
                try
                {
                    _db.User.Update(user);
                    _db.SaveChanges();
                }
                catch
                {
                    error = "A felhasználó adatainak változtatása sikertelen volt";
                }
            }
            else
            {
                error = "Kitöltetlen mező";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }
        ///<summary>
        ///Az adatbázisban felhasználók adatainak törléséért felelős metódus
        ///</summary>
        ///<param name="user">Az AdminPage-en történő, adatbázisba <c>felhasználók adatainak</c> törléséért felelős paraméter </param>
        [HttpPost]
        public IActionResult DeleteUser([Bind(include: "Id")] User user)
        {
            string error = "";
            if (user != null)
            {
                try
                {
                    _db.User.Remove(user);
                    _db.SaveChanges();
                }
                catch
                {
                    error = "A felhasználó törlése sikertelen volt" + user;
                }
            }
            else
            {
                error = "Kitöltetlen mező";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }
        ///<summary>
        ///Az adatbázisban kommentek írásáért felelős metódus
        ///</summary>
        ///<param name="comment">Az AdminPage-en történő, adatbázisba <c>felhasználói kommentek</c> írásáért felelős paraméter </param>
        [HttpPost]
        public IActionResult CommentCreate([Bind(include: "UserId,PlantId,Comm")] Comment comment)
        {
            string error = "";
            if (comment != null)
            {
                try
                {
                    _db.Comment.Add(comment);
                    _db.SaveChanges();
                }
                catch
                {
                    error = "A Comment mentése sikertelen volt";
                }
            }
            else
            {
                error = "Kitöltetlen mező";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }
        ///<summary>
        ///Az adatbázisban kommentek átírásáért felelős metódus
        ///</summary>
        ///<param name="comment">Az AdminPage-en történő, adatbázisba <c>felhasználói kommentek</c> frissítéséért felelős paraméter </param>
        [HttpPost]
        public IActionResult CommentUpdate([Bind(include: "Id, UserId, PlantId, Comm")] Comment comment)
        {
            string error = "";
            if (comment != null)
            {
                try
                {
                    _db.Comment.Update(comment);
                    _db.SaveChanges();
                }
                catch
                {
                    error = "A Comment változtatása sikertelen volt";
                }
            }
            else
            {
                error = "Kitöltetlen mező";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }
        ///<summary>
        ///Az adatbázisban kommentek törléséért felelős metódus
        ///</summary>
        ///<param name="comment">Az AdminPage-en történő, adatbázisba <c>felhasználói kommentek</c> törléséért felelős paraméter </param>
        [HttpPost]
        public IActionResult DeleteComment([Bind(include: "Id")] Comment comment)
        {
            string error = "";
            if (comment != null)
            {
                try
                {
                    _db.Comment.Remove(comment);
                    _db.SaveChanges();
                }
                catch
                {
                    error = "A Comment törlése sikertelen volt";
                }
            }
            else
            {
                error = "Kitöltetlen mező";
            }
            AdminPage page = new AdminPage();
            page.error = error;
            return RedirectToAction("Admin", page);
        }
        ///<summary>
        ///A Selected metódusba küldi tovább az adatokat
        ///</summary>
        ///<param name="valami">Tovább küldi a <c>növények egyes adatait</c> ez a paraméter </param>
        [HttpPost]
        public IActionResult Selected2([Bind(include: "PlantId,PlantName,UserId")] Selected valami)
        {
            return RedirectToAction("Selected", valami);
        }
        ///<summary>
        ///A kiválasztott kártyákban lévő adatok kimutatásáért felelős metódus
        ///</summary>
        ///<param name="valami"> Az egyes növények adatit, képeit, leírásait, felhasználókat és általuk írt kommentek <c>kimutatásáért</c> felelős paraméter </param>
        public IActionResult Selected(Selected valami)
        {
            List<Models.Switch> val = _db.Switch.Where(x => x.PlantsId == valami.PlantId).ToList();
            List<Tools> tools = new List<Tools>();
            foreach (var t in val)
            {
                tools.Add(_db.Tools.Where(x => x.ToolsId == t.ToolsId).FirstOrDefault());
            }
            IEnumerable<Comment> com = _db.Comment.Where(x => x.PlantId == valami.PlantId);
            IEnumerable<User> use = _db.User.ToList();
            List<string> emails = new List<string>();
            List<string> comms = new List<string>();
            foreach (var comments in com)
            {
                comms.Add(comments.Comm);
                bool isEmail = true;
                foreach(User item in use)
                {
                    if(item.Id==comments.UserId)
                    {
                        isEmail = false;
                        emails.Add(item.Email);
                    }
                }
                if(isEmail)
                {
                    emails.Add("Deleted_User");
                }
            }
            ToolsPic tool = new ToolsPic();
            tool.PlantName = valami.PlantName;
            tool.Tool = tools;
            tool.Emails= emails;
            tool.Comments = comms;
            tool.Id = valami.UserId;
            tool.PlantId = valami.PlantId;
            return View(tool);
        }
        ///<summary>
        ///Az adatbázisba írandó adatok mentéséért felelős metódus
        ///</summary>
        ///<param name="comment"> Az egyes növények alatt kommentelő felhasználók és általuk írt kommentek <c>kimentéséért</c> felelős paraméter </param>
        [HttpPost]
        public IActionResult CommentCreate2([Bind(include: "Id,PlantId,Comm,PlantName")] ToolsPic comment)
        {
            string error = "";
            if (comment != null)
            {
                try
                {
                    Comment comm = new Comment();
                    comm.UserId = comment.Id;
                    comm.PlantId = comment.PlantId;
                    comm.Comm = comment.Comm;
                    _db.Comment.Add(comm);
                    _db.SaveChanges();
                }
                catch
                {
                    error = "A Comment mentése sikertelen volt";
                }
            }
            else
            {
                error = "Kitöltetlen mező";
            }
            Selected s = new Selected();
            s.PlantName = comment.PlantName;
            s.PlantId = comment.PlantId;
            s.UserId = comment.Id;
            return RedirectToAction("Selected", s);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}