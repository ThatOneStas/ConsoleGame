using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSexam.Exceptions;
using CSexam.MsgHandlers;

namespace CSexam.Models
{
    public class Shop
    {
        public string _name {  get; set; }
        //
        // lists of items to buy
        //
        public List<Weapon> _weapons { get; private set; } = new List<Weapon>();
        public List<Armor> _armors { get; private set; } = new List<Armor>();
        public List<Food> _food { get; private set; } = new List<Food>();
        //
        // public methods
        //
        private void CheckIfPlayerCanBuyItem(Player player, Weapon item)
        {
            if(player._gold < item._price)
            {
                throw new OutOfGold("\nPlayer can't afford this item." +
                    $"\nPlayer's {player._gold} GOLD isn't enough for '{item._name}' price of {item._price} GOLD.");
            }
            else
            {
                return;
            }
        }
        private void CheckIfPlayerCanBuyItem(Player player, Armor item)
        {
            if (player._gold < item._price)
            {
                throw new OutOfGold("\nPlayer can't afford this item." +
                    $"\nPlayer's {player._gold} GOLD isn't enough for '{item._name}' price of {item._price} GOLD.");
            }
            else
            {
                return;
            }
        }
        private void CheckIfPlayerCanBuyItem(Player player, Food item)
        {
            if (player._gold < (item._price * item._quantity))
            {
                throw new OutOfGold("\nPlayer can't afford this/these item(s)." +
                    $"\nPlayer's {player._gold} GOLD isn't enough for x{item._quantity} '{item._name}' price of {item._price} GOLD each.");
            }
            else
            {
                return;
            }
        }
        private bool CheckIfFoodHasReferance(Food item)
        {
            for (int i = 0; i < _food.Count; i++)
            {
                if (item._recovering_points == _food[i]._recovering_points && item._price == _food[i]._price && item._id != _food[i]._id)
                {
                    return true;
                }
            }
            return false;
        } // not in usage now.
        //
        // public methods
        //
        public Shop(string name, List<Weapon> weapons, List<Armor> armors, List<Food> food) // constructor, 3 lists
        {
            _name = name;
            _weapons = weapons;
            _armors = armors;
            _food = food;
        }
        public Shop(string name, List<Weapon> weapons) // constructor, 1 list
        {
            _name = name;
            _weapons = weapons;
            _armors = new List<Armor>();
            _food = new List<Food>();
        }
        public Shop(string name, List<Armor> armors) // constructor, 1 list
        {
            _name = name;
            _armors = armors;
            _weapons = new List<Weapon>();
            _food = new List<Food>();
        }
        public Shop(string name, List<Food> food) // constructor, 1 list
        {
            _name = name;
            _food = food;
            _weapons = new List<Weapon>();
            _armors = new List<Armor>();
        }
        public void PrintInfo() // інфа про магаз та як він працює
        {
            Handler.Default_Print($"'{_name}' (name)." +
                $"\nWeapons quantity: {_weapons.Count};" +
                $"\nArmors quantity: {_armors.Count};" +
                $"\nFood quantity: {_food.Count};" +
                $"\nTo buy an item find its ID and use buy item method, after that" +
                $"\nyou will be asked to input the item's ID that you want to buy." +
                $"\nInput it and it will be bought and moved to your inventory.");
        }
        public void PrintWeapons() // прінтимо усю зброю
        {
            Handler.Default_Print("~~ Weapons:");
            for (int i = 0; i < _weapons.Count; i++)
            {
                Handler.Default_Print($"{i+1}) {_weapons[i].GetInfo()};");
            }
        }
        public void PrintArmors() // прінтимо усі обладунки
        {
            Handler.Default_Print("~~ Armors:");
            for (int i = 0; i < _armors.Count; i++)
            {
                Handler.Default_Print($"{i + 1}) {_armors[i].GetInfo()};");
            }
        }
        public void PrintFood() // прінтимо весь хавчик
        {
            Handler.Default_Print("~~ Food:");
            for (int i = 0; i < _food.Count; i++)
            {
                Handler.Default_Print($"{i + 1}) {_food[i].GetInfo()};");
            }
        }
        public void BuyItem(ref Player player, int item_id) // покупки айтемів
        {
            // перевыряєи чи плеєр не є нулл
            if (player == null)
            {
                throw new NullEntity("Entity (Player) is null.");
            }
            // шукаємо айтем за айді вказуване користувачем (у зброї)
            for (int i = 0; i < _weapons.Count; i++)
            {
                if (item_id == _weapons[i]._id)
                {
                    try
                    {
                        CheckIfPlayerCanBuyItem(player, _weapons[i]);
                        player._gold -= _weapons[i]._price;
                        Handler.Special3_Print($"\nItem '{_weapons[i]._name}' was bought: -{_weapons[i]._price} GOLD");
                        player.AddItem(_weapons[i]);
                        return;
                    }
                    catch (OutOfGold error)
                    {
                        Handler.Special2_Print(error.Message);
                        return;
                    }
                }
            }
            // шукаємо айтем за айді вказуване користувачем (в обладунках)
            for (int i = 0; i < _armors.Count; i++)
            {
                if (item_id == _armors[i]._id)
                {
                    try
                    {
                        CheckIfPlayerCanBuyItem(player, _armors[i]);
                        player._gold -= _armors[i]._price;
                        Handler.Special3_Print($"\nItem '{_armors[i]._name}' was bought: -{_armors[i]._price} GOLD");
                        player.AddItem(_armors[i]);
                        return;
                    }
                    catch (OutOfGold error)
                    {
                        Handler.Special2_Print(error.Message);
                        return;
                    }
                }
            }
            // шукаємо айтем за айді вказуване користувачем (у їжі)
            for (int i = 0; i < _food.Count;i++)
            {
                if(item_id == _food[i]._id)
                {
                    try
                    {
                        CheckIfPlayerCanBuyItem(player, _food[i]);
                        player._gold -= (_food[i]._price * _food[i]._quantity);
                        // якщо їжа яку ми хочем купити має к-сть..
                        // ..то мусим допомогти знайти
                        if (_food[i]._inner_food != null)
                        {
                            Handler.Special3_Print($"\nItem '{_food[i]._name}' was bought: -{_food[i]._price} GOLD");
                            player.AddItem(_food[i]._inner_food);
                        }
                        else
                        {
                            Handler.Special3_Print($"\nItem/s x{_food[i]._quantity} '{_food[i]._name}' was/were bought: -{_food[i]._price * _food[i]._quantity} GOLD");
                            player.AddItem(_food[i]);
                        }
                        return;
                    }
                    catch (OutOfGold error)
                    {
                        Handler.Special2_Print(error.Message);
                        return;
                    }
                }
            }
            // якщо ніде немає
            Handler.Special2_Print($"\nItem with {item_id} ID wasn't found. Nothing was bought.");
        }
    }
}

