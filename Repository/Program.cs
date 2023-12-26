using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Globalization;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using Data;
using Models;
using Repository.Controls;
using static Models.Studio;
using static System.Net.Mime.MediaTypeNames;
using Repository.Controls;
using DataUnitOfWork;
using System.Xml.Linq;

internal class Program
{
    private static UnitOfWork unitOfWork = new UnitOfWork();

    private enum MenuItems
    {
        Додавання,
        Редагування,
        Видалення,
        Вихід,
    }

    private enum Mode
    {
        Однокористувацький,
        Багатокористувацький,
    }

    private enum PropertyGame
    {
        Назва,
        Студія,
        Стиль,
        Дата,
        Режим,
        Кількість,
    }

    private enum PropertyStudio
    {
        Назва,
        Країни,
        Міста,
        Ігри,
        Вихід,
    }

    private enum MainMenu
    {
        Гра,
        Студія,
        Вихід,
    }

    public static void PrintInfo(string text, List<Game> items)
    {
        Console.WriteLine();
        if (items.Count > 0)
        {
            Console.WriteLine(text);
            Console.WriteLine();
            Console.WriteLine("{0,-30} {1,-25} {2,-20} {3,-15} {4,-15} {5,-15}", "Назва гри", "Студія", "Стиль", "Дата релізу", "Режим", "Кількість проданих копій");
            Console.WriteLine();

            foreach (var item in items)
            {
                Console.WriteLine("{0,-30} {1,-25} {2,-20} {3,-15} {4,-15} {5,-15}", item.Name, item.GameStudio.Name, item.Style, item.DateRelease, item.GameplayMode, item.NumberSold);
            }
        }
        else
        {
            Console.WriteLine("Інформацію не знайдено.");
        }

        Console.ReadKey();
        Console.Clear();
    }

    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        using (DataContex dc = new DataContex())
        {
            var valorant = new Game { Name = "Valorant", Style = "Shooter", DateRelease = new DateOnly(2020, 6, 2, new JulianCalendar()), GameplayMode = Game.Mode.Multiplayer, NumberSold = 15000000 };
            var dota2 = new Game { Name = "Dota 2", Style = "MOBA", DateRelease = new DateOnly(2013, 7, 9, new JulianCalendar()), GameplayMode = Game.Mode.Multiplayer, NumberSold = 500000 };
            var cyberpunk = new Game { Name = "Cyberpunk 2077", Style = "Action RPG", DateRelease = new DateOnly(2020, 9, 28, new JulianCalendar()), GameplayMode = Game.Mode.SinglePlayer, NumberSold = 15000000 };
            var cs2 = new Game { Name = "Counter-Strike 2", Style = "Shooter", DateRelease = new DateOnly(2023, 9, 27, new JulianCalendar()), GameplayMode = Game.Mode.Multiplayer, NumberSold = 1400000 };
            var theWitcher3 = new Game { Name = "The Witcher 3: Wild Hunt", Style = "Action RPG", DateRelease = new DateOnly(2015, 5, 19, new JulianCalendar()), GameplayMode = Game.Mode.SinglePlayer, NumberSold = 30000000 };
            var amongUs = new Game { Name = "Among Us", Style = "Social Deduction", DateRelease = new DateOnly(2018, 6, 15, new JulianCalendar()), GameplayMode = Game.Mode.Multiplayer, NumberSold = 50000000 };
            var minecraft = new Game { Name = "Minecraft", Style = "Sandbox", DateRelease = new DateOnly(2011, 11, 18, new JulianCalendar()), GameplayMode = Game.Mode.Multiplayer, NumberSold = 238000000 };
            var skyrim = new Game { Name = "The Elder Scrolls V: Skyrim", Style = "Action RPG", DateRelease = new DateOnly(2011, 11, 11, new JulianCalendar()), GameplayMode = Game.Mode.SinglePlayer, NumberSold = 30000000 };
            var falloutShelter = new Game { Name = "Fallout Shelter", Style = "Simulation", DateRelease = new DateOnly(2015, 6, 14, new JulianCalendar()), GameplayMode = Game.Mode.SinglePlayer, NumberSold = 12000000 };
            var leagueOfLegends = new Game { Name = "League of Legends", Style = "MOBA", DateRelease = new DateOnly(2009, 10, 27, new JulianCalendar()), GameplayMode = Game.Mode.Multiplayer, NumberSold = 600000 };

            var valve = new Studio { Name = "Valve Corporation", Countries = "USA", Cities = "Bellevue", Games = { dota2, cs2 } };
            var cdProjektRed = new Studio { Name = "CD Projekt Red", Countries = "Poland", Cities = "Warsaw", Games = { cyberpunk, theWitcher3 } };
            var riotGames = new Studio { Name = "Riot Games", Countries = "USA", Cities = "Los Angeles", Games = { valorant, leagueOfLegends } };
            var innersloth = new Studio { Name = "Innersloth", Countries = "USA", Cities = "Redmond", Games = { amongUs } };
            var mojang = new Studio { Name = "Mojang Studios", Countries = "Sweden", Cities = "Stockholm", Games = { minecraft } };
            var bethesda = new Studio { Name = "Bethesda Game Studios", Countries = "USA", Cities = "Rockville", Games = { skyrim, falloutShelter } };
            unitOfWork.GameRepository.Insert(valorant);
            unitOfWork.GameRepository.Insert(dota2);
            unitOfWork.GameRepository.Insert(cyberpunk);
            unitOfWork.GameRepository.Insert(cs2);
            unitOfWork.GameRepository.Insert(theWitcher3);
            unitOfWork.GameRepository.Insert(amongUs);
            unitOfWork.GameRepository.Insert(minecraft);
            unitOfWork.GameRepository.Insert(skyrim);
            unitOfWork.GameRepository.Insert(falloutShelter);

            unitOfWork.StudioRepository.Insert(valve);
            unitOfWork.StudioRepository.Insert(cdProjektRed);
            unitOfWork.StudioRepository.Insert(riotGames);
            unitOfWork.StudioRepository.Insert(innersloth);
            unitOfWork.StudioRepository.Insert(mojang);
            unitOfWork.StudioRepository.Insert(bethesda);
            unitOfWork.Save();

            var games = unitOfWork.GameRepository.GetAll();
            PrintInfo("Всі гри: ", games);
            Console.WriteLine("Тут і в подальшому для продовження натисніть будь-яку клавішу.");

            Console.WriteLine("Пошук за назвою гри \nВведіть назву гри: ");
            string inputName = Convert.ToString(Console.ReadLine());
            var gameSearchByName = unitOfWork.GameRepository.GetAll().Where(g => g.Name == inputName).ToList();
            PrintInfo("Інформація про цю гру: ", gameSearchByName);

            Console.WriteLine("Пошук за назвою студії \nВведіть назву студії: ");
            string inputStudio = Convert.ToString(Console.ReadLine());
            var gamesSearchByStudio = unitOfWork.GameRepository.GetAll().Where(g => g.GameStudio.Name == inputStudio).ToList();
            PrintInfo("Ігри цієї студії: ", gamesSearchByStudio);

            Console.WriteLine("Пошук за стилем гри \nВведіть стиль гри: ");
            string inputStyle = Convert.ToString(Console.ReadLine());
            var gamesSearchByStyle = unitOfWork.GameRepository.GetAll().Where(g => g.Style == inputStyle).ToList();
            PrintInfo("Ігри цього стилю: ", gamesSearchByStyle);

            Console.WriteLine("Пошук за роком випуску гри \nВведіть рік випуску гри: ");
            int inputYear = Convert.ToInt32(Console.ReadLine());
            var gamesSearchByYear = unitOfWork.GameRepository.GetAll().Where(g => g.DateRelease >= new DateOnly(inputYear, 1, 1, new JulianCalendar()) && g.DateRelease <= new DateOnly(inputYear, 12, 31, new JulianCalendar())).ToList();
            PrintInfo("Ігри цього року: ", gamesSearchByYear);

            var gamesSingle = unitOfWork.GameRepository.GetAll().Where(g => g.GameplayMode == Game.Mode.SinglePlayer).ToList();
            PrintInfo("Однокористувацькі ігри: ", gamesSingle);

            var gamesMulti = unitOfWork.GameRepository.GetAll().Where(g => g.GameplayMode == Game.Mode.Multiplayer).ToList();
            PrintInfo("Багатокористувацькі ігри: ", gamesMulti);

            var gamesMostPopular = unitOfWork.GameRepository.GetAll().OrderByDescending(g => g.NumberSold).Take(3).ToList();
            PrintInfo("Топ-3 найпопулярніших ігор (за кількістю проданих копій): ", gamesMostPopular);

            var gamesLeastPopular = unitOfWork.GameRepository.GetAll().OrderBy(g => g.NumberSold).Take(3).ToList();
            PrintInfo("Топ-3 найнепопулярніших ігор (за кількістю проданих копій): ", gamesLeastPopular);

            Console.WriteLine("Кількість однокористувацьких ігор: {0}", gamesSingle.Count);
            Console.WriteLine("Кількість багатокористувацьких ігор: {0}", gamesMulti.Count);
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Введіть стиль гри: ");
            inputStyle = Convert.ToString(Console.ReadLine());
            var gamesTop5MostPopular = unitOfWork.GameRepository.GetAll().Where(g => g.Style == inputStyle).OrderByDescending(g => g.NumberSold).ToList();
            PrintInfo("Топ-5 ігор за найбільшою кількістю продажів цього стилю: ", gamesTop5MostPopular);
            var gamesTop5LessPopular = unitOfWork.GameRepository.GetAll().Where(g => g.Style == inputStyle).OrderBy(g => g.NumberSold).ToList();
            PrintInfo("Топ-5 ігор за найменшою кількістю продажів цього стилю: ", gamesTop5LessPopular);

            Console.WriteLine("Назви студій і стилі ігор, які вони випускають: ");
            Console.WriteLine();
            var studioNames = unitOfWork.StudioRepository.GetAll().Select(studio => studio.Name);
            foreach (var studioName in studioNames)
            {
                var gameStyles = unitOfWork.GameRepository.GetAll()
                .Where(game => game.GameStudio.Name == studioName)
                .Select(game => game.Style)
                .Distinct();

                Console.WriteLine($"Студія: {studioName}");
                Console.Write($"Стилі: ");

                foreach (var style in gameStyles)
                {
                    var gamesInStyle = unitOfWork.StudioRepository.GetAll()
                        .Where(studio => studio.Name == studioName && studio.Games.Any(game => game.Style == style))
                        .SelectMany(studio => studio.Games.Where(game => game.Style == style).Select(game => game.Name));

                    Console.WriteLine($"{style} ({string.Join(", ", gamesInStyle)})");
                }

                Console.WriteLine();
            }

            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Виберіть що ви хочете додати/змінити/видалити *управління за допомогою стрілочок*");
            while (true)
            {
                int input = Menu.MultipleChoice(true, new MainMenu());
                switch ((MainMenu)input)
                {
                    case MainMenu.Гра:
                        Console.Clear();
                        ACDGames(dc);
                        break;

                    case MainMenu.Студія:
                        Console.Clear();
                        ACDStudio(dc);
                        break;

                    case MainMenu.Вихід:
                        Console.Clear();
                        Environment.Exit(0);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private static void ACDGames(DataContex dc)
    {
        while (true)
        {
            int input = Menu.MultipleChoice(true, new MenuItems());
            switch ((MenuItems)input)
            {
                case MenuItems.Додавання:
                    MenuAddGame(dc);
                    break;

                case MenuItems.Редагування:
                    MenuChangeGame(dc);
                    break;

                case MenuItems.Видалення:
                    MenuDeleteGame(dc);
                    break;

                case MenuItems.Вихід:
                    Environment.Exit(0);
                    break;

                default:
                    break;
            }

            var games = unitOfWork.GameRepository.GetAll();
            PrintInfo("Всі гри: ", games);
        }
    }

    private static void MenuAddGame(DataContex dc)
    {
        Console.Clear();
        Console.WriteLine("Введіть назву гри: ");
        string name = Convert.ToString(Console.ReadLine());
        Console.Clear();

        string studio;
        while (true)
        {
            Console.WriteLine("Введіть назву студії гри: ");
            studio = Convert.ToString(Console.ReadLine());
            if (dc.Studios.Any(s => s.Name == studio))
            {
                break;
            }
            else
            {
                Console.WriteLine("Цієї студії немає в БД.");
            }
        }

        Console.Clear();

        Console.WriteLine("Введіть стиль гри: ");
        string style = Convert.ToString(Console.ReadLine());
        Console.Clear();

        DateOnly dateRelease;
        while (true)
        {
            Console.WriteLine("Введіть дату релізу гри (у форматі yyyy-MM-dd): ");
            string dateInput = Console.ReadLine();

            if (DateOnly.TryParse(dateInput, out dateRelease))
            {
                break;
            }
            else
            {
                Console.WriteLine("Некоректний формат дати!");
            }
        }

        Console.Clear();

        Console.WriteLine("Виберіть режим гри");
        Game.Mode mode = Game.Mode.Multiplayer;
        int inputMode = Menu.MultipleChoice(true, new Mode());

        switch ((Mode)inputMode)
        {
            case Mode.Однокористувацький:
                mode = Game.Mode.SinglePlayer;
                break;
            case Mode.Багатокористувацький:
                mode = Game.Mode.Multiplayer;
                break;
            default:
                break;
        }

        Console.Clear();

        Console.WriteLine("Введіть кількість проданих копій гри: ");
        int count = Convert.ToInt32(Console.ReadLine());

        Game newGame = new Game { Name = name, GameStudio = unitOfWork.StudioRepository.GetAll().FirstOrDefault(s => s.Name == studio), Style = style, DateRelease = dateRelease, GameplayMode = mode, NumberSold = count };
        unitOfWork.GameRepository.Insert(newGame);
        unitOfWork.Save();
        Console.Clear();
    }

    private static void MenuChangeGame(DataContex dc)
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("Введіть назву гри, дані якої хочете змінити: ");
            string name = Convert.ToString(Console.ReadLine());
            Console.Clear();

            if (unitOfWork.GameRepository.GetByName(name) != null)
            {
                int inputProperty = Menu.MultipleChoice(true, new PropertyGame());
                Game gameToUpdate = unitOfWork.GameRepository.GetByName(name);

                switch ((PropertyGame)inputProperty)
                {
                    case PropertyGame.Назва:
                        Console.Clear();
                        Console.WriteLine("Введіть нову назву гри: ");
                        string newName = Convert.ToString(Console.ReadLine());
                        gameToUpdate.Name = newName;
                        break;

                    case PropertyGame.Студія:
                        Console.Clear();
                        string studio;
                        while (true)
                        {
                            Console.WriteLine("Введіть нову назву студії гри: ");
                            studio = Convert.ToString(Console.ReadLine());
                            if (unitOfWork.StudioRepository.GetByName(studio)!=null)
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Цієї студії немає в БД.");
                            }
                        }

                        gameToUpdate.GameStudio = unitOfWork.StudioRepository.GetByName(studio);
                        break;

                    case PropertyGame.Стиль:
                        Console.Clear();
                        Console.WriteLine("Введіть новий стиль гри: ");
                        string newStyle = Convert.ToString(Console.ReadLine());
                        gameToUpdate.Style = newStyle;
                        break;

                    case PropertyGame.Дата:
                        Console.Clear();
                        DateOnly newdateRelease;
                        while (true)
                        {
                            Console.WriteLine("Введіть нову дату релізу гри (у форматі yyyy-MM-dd): ");
                            string dateInput = Console.ReadLine();

                            if (DateOnly.TryParse(dateInput, out newdateRelease))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Некоректний формат дати!");
                            }
                        }

                        gameToUpdate.DateRelease = newdateRelease;
                        break;

                    case PropertyGame.Режим:
                        Console.Clear();
                        Console.WriteLine("Виберіть новий режим гри: ");
                        int inputGameMode = Menu.MultipleChoice(true, new Mode());

                        switch ((Mode)inputGameMode)
                        {
                            case Mode.Однокористувацький:
                                gameToUpdate.GameplayMode = Game.Mode.SinglePlayer;
                                break;
                            case Mode.Багатокористувацький:
                                gameToUpdate.GameplayMode = Game.Mode.Multiplayer;
                                break;
                            default:
                                break;
                        }

                        break;

                    case PropertyGame.Кількість:
                        Console.Clear();
                        Console.WriteLine("Введіть нову кількість проданих копій гри: ");
                        int newNumber = Convert.ToInt32(Console.ReadLine());
                        gameToUpdate.NumberSold = newNumber;
                        break;
                    default:
                        break;
                }

                unitOfWork.GameRepository.Update(gameToUpdate);
                unitOfWork.Save();
                break;
            }
            else
            {
                Console.WriteLine("Гру не знайдено.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        Console.Clear();
    }

    private static void MenuDeleteGame(DataContex dc)
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("Введіть назву гри, яку хочете видалити: ");
            string name = Convert.ToString(Console.ReadLine());
            Console.Clear();

            if (unitOfWork.GameRepository.GetByName(name) != null)
            {
                unitOfWork.GameRepository.Delete(name);
                unitOfWork.Save();
                break;
            }
            else
            {
                Console.WriteLine("Гру не знайдено.");
                Console.ReadKey();
                Console.Clear();
            }

            break;
        }

        Console.Clear();
    }

    private static void ACDStudio(DataContex dc)
    {
        while (true)
        {
            int input = Menu.MultipleChoice(true, new MenuItems());
            switch ((MenuItems)input)
            {
                case MenuItems.Додавання:
                    MenuAddStudio(dc);
                    break;

                case MenuItems.Редагування:
                    MenuChangeStudio(dc);
                    break;

                case MenuItems.Видалення:
                    MenuDeleteStudio(dc);
                    break;

                case MenuItems.Вихід:
                    Environment.Exit(0);
                    break;

                default:
                    break;
            }

            var studios = unitOfWork.StudioRepository.GetAll();
            Console.WriteLine("Всі студії: ");
            Console.WriteLine();
            foreach (var studio in studios)
            {
                Console.WriteLine("Назва студії: {0}", studio.Name);
                Console.WriteLine("Країни, в яких є філії: {0}", studio.Countries);
                Console.WriteLine("Міста, в яких є філії: {0}", studio.Cities);
                Console.WriteLine("Ігри: {0}", string.Join(", ", studio.Games.Select(game => game.Name)));
                Console.WriteLine();
            }

            Console.ReadLine();
            Console.Clear();
        }
    }

    private static void MenuAddStudio(DataContex dc)
    {
        Console.Clear();
        Console.WriteLine("Введіть назву студії: ");
        string nameStudio = Convert.ToString(Console.ReadLine());
        Console.Clear();

        if (unitOfWork.StudioRepository.GetByName(nameStudio) == null)
        {
            Console.WriteLine("Введіть країни, в яких є філії: ");
            string countries = Convert.ToString(Console.ReadLine());
            Console.Clear();

            Console.WriteLine("Введіть міста, в яких є філії: ");
            string cities = Convert.ToString(Console.ReadLine());
            Console.Clear();

            Console.WriteLine("Введіть назви ігор через кому і без пробілів: ");
            string gamesInput = Console.ReadLine();
            string[] gameNames = gamesInput.Split(',');

            List<Game> games = new List<Game>();
            foreach (var name in gameNames)
            {
                string trimmedName = name.Trim();
                Game game = unitOfWork.GameRepository.GetByName(trimmedName);

                if (game != null)
                {
                    games.Add(game);
                }
            }

            Studio newStudio = new Studio { Name = nameStudio, Countries = countries, Cities = cities, Games = games };
            unitOfWork.StudioRepository.Insert(newStudio);
            unitOfWork.Save();
            Console.Clear();
        }
        else
        {
            Console.WriteLine("Така студія в БД вже є.");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private static void MenuChangeStudio(DataContex dc)
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("Введіть назву студії, дані якої хочете змінити: ");
            string nameStudio = Convert.ToString(Console.ReadLine());
            Console.Clear();

            if (unitOfWork.StudioRepository.GetByName(nameStudio) != null)
            {
                int inputProperty = Menu.MultipleChoice(true, new PropertyStudio());
                Studio studioToUpdate = unitOfWork.StudioRepository.GetByName(nameStudio);

                switch ((PropertyStudio)inputProperty)
                {
                    case PropertyStudio.Назва:
                        Console.Clear();
                        Console.WriteLine("Введіть нову назву студії: ");
                        string newName = Convert.ToString(Console.ReadLine());
                        studioToUpdate.Name = newName;
                        break;

                    case PropertyStudio.Країни:
                        Console.Clear();
                        Console.WriteLine("Введіть нові країни: ");
                        string newCoutries = Convert.ToString(Console.ReadLine());
                        studioToUpdate.Countries = newCoutries;
                        break;

                    case PropertyStudio.Міста:
                        Console.Clear();
                        Console.WriteLine("Введіть нові міста: ");
                        string newCities = Convert.ToString(Console.ReadLine());
                        studioToUpdate.Cities = newCities;
                        break;

                    case PropertyStudio.Ігри:
                        Console.Clear();
                        Console.WriteLine("Введіть назви ігор через кому і без пробілів: ");
                        string gamesInput = Console.ReadLine();
                        string[] gameNames = gamesInput.Split(',');
                        List<Game> games = new List<Game>();
                        foreach (var name in gameNames)
                        {
                            string trimmedName = name.Trim();
                            Game game = unitOfWork.GameRepository.GetByName(trimmedName);

                            if (game != null)
                            {
                                games.Add(game);
                            }
                        }

                        studioToUpdate.Games = games;
                        break;

                    default:
                        break;
                }

                unitOfWork.StudioRepository.Update(studioToUpdate);
                unitOfWork.Save();
                break;
            }
            else
            {
                Console.WriteLine("Студію не знайдено.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        Console.Clear();
    }

    private static void MenuDeleteStudio(DataContex dc)
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("Введіть назву студії, яку хочете видалити: ");
            string name = Convert.ToString(Console.ReadLine());
            Console.Clear();

            if (unitOfWork.StudioRepository.GetByName(name) != null)
            {
                unitOfWork.StudioRepository.Delete(name);
                unitOfWork.Save();
                break;
            }
            else
            {
                Console.WriteLine("Студію не знайдено.");
                Console.ReadKey();
                Console.Clear();
            }

            break;
        }

        Console.Clear();
    }
}