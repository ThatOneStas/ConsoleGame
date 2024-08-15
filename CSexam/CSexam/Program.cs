using System;
using System.Reflection.Metadata;
using System.Text.Json;
using CSexam.Exceptions;
using CSexam.Models;
using CSexam.MsgHandlers;

// Методи для вставки у хендлери
public class Prints
{
    // просто консолька
    public static void DefaultPrint(string msg)
    {
        Console.WriteLine(msg);
    }
    // з зеленим кольором
    public static void GreenPrint(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
    // з червоним кольором
    public static void RedPrint(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
    // з бюрюзовим кольором
    public static void CyanPrint(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}

internal partial class Program
{
    private static async Task Main(string[] args)
    {
        // вставляю вище створені методи у хендлери
        {
            Handler.msgHandler += Prints.DefaultPrint;
            Handler.specified1_msgHandler += Prints.GreenPrint;
            Handler.specified2_msgHandler += Prints.RedPrint;
            Handler.specified3_msgHandler += Prints.CyanPrint;
        }

        // JsonSeralizer
        string jsonFile = "../../../Extra/PreviousSessions.json"; // path
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true
        }; // exta options (tuple included)

        // завантажуэмо попередні сесіЇ назад до програми
        List<Player> sessions = new List<Player>(); // список з попередніми сесіями
        using (FileStream fs = new FileStream(jsonFile, FileMode.OpenOrCreate))
        {
            List<Player> previous_sessions = await JsonSerializer.DeserializeAsync<List<Player>>(fs, options);
            sessions = previous_sessions;
        }

        // створення АБО завантаження гравця з попередніх сесій
        Player player = null; // гравець (поки що нулловий)
        bool loaded = false; // флажок який позначає чи був завантажений гравець чи був створений новий
        {
            List<string> sessions_keys = new List<string>();
            for(int i = 0; i < sessions.Count; i++)
            {
                sessions_keys.Add(sessions[i]._name);
            }
            {
                string player_name = null;
                bool flag = true;
                Handler.Special1_Print("To load previous session: input the sessions' player name." +
                    "\nTo create new session: input a new player's name (4 letters required):");
                while (flag)
                {
                    player_name = Console.ReadLine();

                    if (player_name == null)
                    {
                        Handler.Special2_Print("\nName can't be null. Try again:\n");
                        flag = true;
                    }
                    else if (player_name.Length < 4)
                    {
                        Handler.Special2_Print("\nName is too short. Try again:\n");
                        flag = true;
                    }
                    else if (sessions_keys.Contains(player_name))
                    {
                        for (int i = 0; i < sessions.Count; i++)
                        {
                            if (sessions[i]._name == player_name)
                            {
                                player = sessions[i];
                                loaded = true;
                                flag = false;
                            }
                        }
                        // повідомлення при завантеженні попереньої сесії
                        Handler.Special3_Print("\nGood luck with continuing your journey..");
                    }
                    else
                    {
                        // початковий сет
                        Weapon starter_weapon = new Weapon("Wooden Stick", 1, new Tuple<int, int>(15, 25), 5, 1, 100);
                        Armor starter_armor = new Armor("Rookie Outfit", 2, 10, 0, 0, 1, 100);
                        List<Food> starter_food = [new Food("Apple", 30, 15, 35, 10)];
                        player = new Player(player_name, starter_weapon, starter_armor, starter_food);
                        // повідомлення при створюванні нової сесії
                        Handler.Special3_Print("\nGood luck with your new journey..");
                        flag = false;
                    }
                }
            }
        }

        // створюємо магазин
        Shop main_shop = null;
        {
            List<Weapon> weapons = [
                new Weapon("Stone Axe", 10, new Tuple<int, int>(20, 35), 5, 2, 280),
                new Weapon("Archer's Bow", 11, new Tuple<int, int>(30, 50), 5, 3, 500),
                new Weapon("Crossbow", 12, new Tuple<int, int>(45, 75), 8, 4, 900),
                new Weapon("Iron sword", 13, new Tuple<int, int>(55, 95), 7, 5, 1400),
                new Weapon("Viking's Axe", 14, new Tuple<int, int>(80, 125), 10, 6, 2000),
                new Weapon("An Old Revolver", 15, new Tuple<int, int>(125, 195), 10, 7, 2800),
                new Weapon("Mage Wand", 16, new Tuple<int, int>(190, 280), 5, 8, 3800),
                new Weapon("A Dark Scythe", 17, new Tuple<int, int>(290, 455), 15, 9, 5800),
                new Weapon("Excalibur", 18, new Tuple<int, int>(500, 800), 15, 10, 10000),
                new Weapon("Joker's Card", 19, new Tuple<int, int>(9, 999), 7, 10, 7777),
            ];
            List<Armor> armors = [
                new Armor("Leather Armor", 20, 15, 10, 0, 2, 250),
                new Armor("Archer's Outfit", 21, 10, 15, 5, 3, 400),
                new Armor("Demaged Iron Armor", 22, 20, 20, 0, 4, 650),
                new Armor("Iron Armor", 23, 25, 30, 5, 5, 1000),
                new Armor("HP Outfit", 24, 5, 150, 5, 6, 1500),
                new Armor("Magician Outfit", 25, 30, 50, 10, 7, 2000),
                new Armor("Critical Striker Outfit", 26, 20, 50, 25, 8, 3000),
                new Armor("Dark Armor", 27, 40, 100, 10, 9, 5000),
                new Armor("A Legendary Armor", 28, 50, 200, 15, 10, 7000),
            ];
            List<Food> food = [
                new Food("Apple", 30, 15, 35),
                new Food("Cookie", 31, 30, 60),
                new Food("A piece of Pie", 32, 60, 100),
                new Food("Pie", 33, 100, 150),
                new Food("Grapes", 34, 5, 10, 20),
                new Food("A bunch of Apples (x5)", 35, new Food("Apple", 30, 15, 35, 5)), // basic price
                new Food("A package of Cookies (x5)", 36, new Food("Cookie", 31, 30, 60, 5), 260), // price with discount
            ];
            main_shop = new Shop("The Only Shop", weapons, armors, food);
        }

        //
        // starting
        //
        bool MAIN_FLAG = true;
        while (MAIN_FLAG)
        {
            int MAIN_ACTION = ChooseAction();
            switch (MAIN_ACTION)
            {
                // бойовка
                case 1:
                    Mob mob = GetRandomMob(player);
                    Handler.Special3_Print($"You came across '{mob._name}'.\n" +
                        $"He doesn't seem to let you go away..");
                    bool FIGHT_FLAG = true;
                    while (FIGHT_FLAG)
                    {
                        int FIGHT_ACTION = ChooseFightAction();
                        switch (FIGHT_ACTION)
                        {
                            case 1:
                                try
                                {
                                    try
                                    {
                                        player.Attack(ref mob);
                                        mob.Attack(ref player);
                                    }
                                    catch(EntityIsDeadAlready err)
                                    {
                                        // do smth
                                    }
                                    catch(NullEntity err)
                                    {
                                        // do smth
                                    }
                                    // перевіряємо чи хтось з них не був вбитий
                                    if (mob._hp <= 0)
                                    {
                                        mob.Lose();
                                        FIGHT_FLAG = false;
                                    }
                                    if (player._hp <= 0)
                                    {
                                        player.Lose(mob);
                                        FIGHT_FLAG = false;
                                    }
                                }
                                catch(EntityIsDeadAlready err)
                                {
                                    Handler.Special2_Print(err.Message);
                                    FIGHT_FLAG = false;
                                }
                                break;
                            case 2:
                                mob.PrintInfo();
                                break;
                            case 3:
                                Handler.Default_Print("Enter the item's ID: ");
                                int item_id = Convert.ToInt32(Console.ReadLine());
                                player.UseItem(item_id);
                                break;
                            case 4:
                                player.PrintFood();
                                break;
                            case 5:
                                player.RunAway();
                                FIGHT_FLAG = false;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                // магази
                case 2:
                    Handler.Special3_Print($"You came to the '{main_shop._name}'.\n" +
                        $"Take a look on stuff, you'll probably need something..");
                    bool SHOP_FLAG = true;
                    while (SHOP_FLAG)
                    {
                        int SHOP_ACTION = ChooseShopAction();
                        switch (SHOP_ACTION)
                        {
                            case 1:
                                main_shop.PrintInfo();
                                break;
                            case 2:
                                main_shop.PrintWeapons();
                                break;
                            case 3:
                                main_shop.PrintArmors();
                                break;
                            case 4:
                                main_shop.PrintFood();
                                break;
                            case 5:
                                Handler.Default_Print("Enter the item's ID: ");
                                int item_id_to_buy = Convert.ToInt32(Console.ReadLine());
                                main_shop.BuyItem(ref player, item_id_to_buy);
                                break;
                            case 6:
                                Handler.Default_Print("Enter the item's ID: ");
                                int item_id_to_sell = Convert.ToInt32(Console.ReadLine());
                                player.SellItem(item_id_to_sell);
                                break;
                            case 7:
                                Handler.Special3_Print($"You left the '{main_shop._name}'..");
                                SHOP_FLAG = false;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                // інвентар
                case 3:
                    Handler.Special3_Print($"You've unpacked your items..");
                    bool INVENTORY_FLAG = true;
                    while (INVENTORY_FLAG)
                    {
                        int INVENTORY_ACTION = ChooseInventoryAction();
                        switch (INVENTORY_ACTION)
                        {
                            case 1:
                                player.PrintWeapons();
                                break;
                            case 2:
                                player.PrintArmors();
                                break;
                            case 3:
                                player.PrintFood();
                                break;
                            case 4:
                                player.PrintCurrentWeaponAndArmor();
                                break;
                            case 5:
                                Handler.Default_Print("Enter the item's ID: ");
                                int item_id_to_use = Convert.ToInt32(Console.ReadLine());
                                player.UseItem(item_id_to_use);
                                break;
                            case 6:
                                Handler.Default_Print("Enter the item's ID: ");
                                int item_id_to_equip = Convert.ToInt32(Console.ReadLine());
                                player.EquipItem(item_id_to_equip);
                                break;
                            case 7:
                                Handler.Special3_Print($"You've packed all your items back..");
                                INVENTORY_FLAG = false;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                // профіль
                case 4:
                    player.PrintInfo();
                    break;
                // end
                case 5:
                    MAIN_FLAG = false;
                    break;
                default:
                    break;
            }
        }

        //
        // ending
        //
        // пропонуємо гравцю зберегти його сесію
        {
            Handler.Special1_Print("Would you like to re/save your session? (Y/N):");
            string choice_of_saving = null;
            bool flag = true;
            while (flag)
            {
                choice_of_saving = Console.ReadLine();
                // якщо так то зберігаємо
                if (choice_of_saving.ToUpper() == "Y")
                {
                    // зберігаєм нашого гравця до попередніх сесій або оновлюємо стати попереднього
                    if (loaded) // якщо був завантажений з попередніх то..
                    {
                        // ..шукаєм де саме, і оновлюємо стати
                        for (int i = 0; i < sessions.Count; i++)
                        {
                            if (sessions[i]._name == player._name)
                            {
                                sessions[i] = player;
                            }
                        }
                    }
                    else // якщо ні, то добавляюм нову сесію до інших
                    {
                        sessions.Add(player);
                    }
                    // завантаження сесій назад до json файлу для подальшого їх завантаження назад до програми
                    using (FileStream fs = new FileStream(jsonFile, FileMode.OpenOrCreate))
                    {
                        await JsonSerializer.SerializeAsync(fs, sessions, options);
                    }
                    flag = false;
                }
                // якщо ні, так і закінчуємо
                else if (choice_of_saving.ToUpper() == "N")
                {
                    flag = false;
                }
                // якщо вибір не правильний, продовжуємо запитувати
                else
                {
                    Handler.Special2_Print("\nWrong choice. Try again:\n");
                }
            }
        }
    }
    //
    // METHODS TO RUN THE GAME
    //
    private static int ChooseAction()
    {
        Handler.Default_Print($"\nChoose the action:" +
            $"\n1) Fight a RANDOM Mob;" +
            $"\n2) Go to the Shop;" +
            $"\n3) Inventory;" +
            $"\n4) Profile;"+
            $"\n5) End the session.");
        int action = 0;
        bool flag = true;
        while(flag){
            action = Convert.ToInt32(Console.ReadLine());
            if(action >= 1 && action <= 5)
            {
                flag = false;
            }
            else
            {
                Handler.Special2_Print("\nWrong action choice. Try again:\n");
                flag = true;
            }
        }
        Handler.Default_Print(""); // slash
        return action;
    }
    private static int ChooseInventoryAction()
    {
        Handler.Default_Print($"\nChoose the action:" +
            $"\n1) Print your Weapons;" +
            $"\n2) Print your Armors;" +
            $"\n3) Print your Food;" +
            $"\n4) Print current Armor and Weapon;" +
            $"\n5) Use an item;" +
            $"\n6) Equip an item (Armor/Weapon);" +
            $"\n7) Return.");
        int action = 0;
        bool flag = true;
        while (flag)
        {
            action = Convert.ToInt32(Console.ReadLine());
            if (action >= 1 && action <= 7)
            {
                flag = false;
            }
            else
            {
                Handler.Special2_Print("\nWrong action choice. Try again:\n");
                flag = true;
            }
        }
        Handler.Default_Print(""); // slash
        return action;
    }
    private static int ChooseShopAction()
    {
        Handler.Default_Print($"\nChoose the action:" +
            $"\n1) Print the Shop's info;" +
            $"\n2) Print Weapons;" +
            $"\n3) Print Armors;" +
            $"\n4) Print Food;" +
            $"\n5) Buy item;" +
            $"\n6) Sell an item (Armor/Weapon only);" +
            $"\n7) Return.");
        int action = 0;
        bool flag = true;
        while (flag)
        {
            action = Convert.ToInt32(Console.ReadLine());
            if (action >= 1 && action <= 7)
            {
                flag = false;
            }
            else
            {
                Handler.Special2_Print("\nWrong action choice. Try again:\n");
                flag = true;
            }
        }
        Handler.Default_Print(""); // slash
        return action;
    }
    private static int ChooseFightAction()
    {
        Handler.Default_Print($"\nChoose the action:" +
            $"\n1) Attack (mob attacks next);" +
            $"\n2) Get mob info;" +
            $"\n3) Use an item;" +
            $"\n4) Print Food;" +
            $"\n5) Run away (Lose).");
        int action = 0;
        bool flag = true;
        while (flag)
        {
            action = Convert.ToInt32(Console.ReadLine());
            if (action >= 1 && action <= 5)
            {
                flag = false;
            }
            else
            {
                Handler.Special2_Print("\nWrong action choice. Try again:\n");
                flag = true;
            }
        }
        Handler.Default_Print(""); // slash
        return action;
    }
    private static Mob GetRandomMob(Player player)
    {
        List<Tuple<Mob, int>> mob_list = [
            new Tuple<Mob, int>(new Mob("Skeleton", 70, new Tuple<int, int>(5, 15), player._lvl, new Tuple<int, int>(150, 300), 50), 99),
            new Tuple<Mob, int>(new Mob("Armored Bandit", 100, new Tuple<int, int>(10, 20), player._lvl, new Tuple<int, int>(250, 400), 75),0),
            new Tuple<Mob, int>(new Mob("Dark Knight", 130, new Tuple<int, int>(20, 30), player._lvl, new Tuple<int, int>(350, 500), 100), 0),
            new Tuple<Mob, int>(new Mob("Minotaur", 200, new Tuple<int, int>(30, 50), player._lvl, new Tuple<int, int>(700, 1000), 200), 0),
        ];
        int random_num = new Random().Next(0,100);
        int counter = 0;
        // Вибираємо моба, при чому чим сильніший він тим рідше він буде попадатись
        for(int i = 0; i < mob_list.Count; i++)
        {
            counter += mob_list[i].Item2;
            if(counter <= random_num)
            {
                return (Mob)mob_list[i].Item1.Clone();
            }
        }
        // Якщо виникне якась помилка то завжди буде вибиратись моб незалежно від його сили.
        return (Mob)mob_list[new Random().Next(0, mob_list.Count-1)].Item1.Clone();
    }
}