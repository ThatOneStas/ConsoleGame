using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CSexam.Exceptions;
using CSexam.MsgHandlers;

namespace CSexam.Models
{
    public class Player : IEntity
    {
        //
        // variables
        //
        public string _name { get; set; }
        public int _hp { get; set; }
        public int _max_hp { get;  set; }
        public int _lvl { get; set; }
        public int _lvl_points { get; set; }
        public int _gold { get; set; }
        public Weapon _current_weapon { get; set; }
        public Armor _current_armor { get; set; }
        public List<Weapon> _weapon_inventory { get; set; } = new List<Weapon>();
        public List<Armor> _armor_inventory { get; set; } = new List<Armor>();
        public List<Food> _food { get; set; } = new List<Food>();
        //
        // private methods
        //
        private bool checkIfPlayerHasItem(Food food)
        {
            for (int i = 0; i < _food.Count; i++)
            {
                if (_food[i]._id == food._id)
                {
                    return true;
                }
            }
            return false;
        }
        private int findItem(Food food)
        {
            for (int i = 0; i < _food.Count; i++)
            {
                if (_food[i]._id == food._id)
                {
                    return i;
                }
            }
            return -1;
        }
        private void checkHP()
        {
            if(_hp > _max_hp)
            {
                _hp = _max_hp;
            }
        }
        private void updateLVL()
        {
            if(_lvl_points >= 100)
            {
                int lvl_ups = 0;
                while(_lvl_points >= 100)
                {
                    _lvl_points -= 100;
                    _lvl += 1;
                    lvl_ups++;
                }
                Handler.Special3_Print($"Level UP! +{lvl_ups} lvl(s). Current LVL: {_lvl}");
            }
        }
        private int getTotalCritChance() // тут ми збираємо усі можливі шанси на критичний удар в один 
        {
            return _current_weapon._crit_chance + _current_armor._crit_chance;
        }
        //
        // public methods
        //
        public Player(string name, Weapon weapon, Armor armor) // constructor
        {
            _name = name;
            _gold = 100;
            _lvl = 1;
            _max_hp = 100 + armor._max_hp_bonus;
            _hp = _max_hp;
            _lvl_points = 0;
            _current_weapon = weapon;
            _current_armor = armor;
        }
        public Player(string name, Weapon weapon, Armor armor, List<Food> food) // constructor
        {
            _name = name;
            _gold = 100;
            _lvl = 1;
            _max_hp = 100 + armor._max_hp_bonus;
            _hp = _max_hp;
            _lvl_points = 0;
            _current_weapon = weapon;
            _current_armor = armor;
            _food = food;
        }
        public Player() { }
        public void PrintInfo()
        {
            Handler.Default_Print($"Name: '{_name}', GOLD: {_gold};" +
                $"\nLVL: {_lvl}, LVL points: {_lvl_points};" +
                $"\nHP: {_hp}, Max HP: {_max_hp}, Total crit. chance: {getTotalCritChance()}%;" +
                $"\nWeapon: {_current_weapon.GetInfo()};" +
                $"\nArmor: {_current_armor.GetInfo()}.");
        }
        public string GetInfo()
        {
            return ($"Name: '{_name}', GOLD: {_gold};" +
                $"\nLVL: {_lvl}, LVL points: {_lvl_points};" +
                $"\nHP: {_hp}, Max HP: {_max_hp}, Total crit. chance: {getTotalCritChance()}%;" +
                $"\nWeapon: {_current_weapon.GetInfo()};" +
                $"\nArmor: {_current_armor.GetInfo()}.");
        }
        public void PrintWeapons() // прінтимо усю зброю
        {
            Handler.Default_Print("Weapon inventory:");
            if (_weapon_inventory.Count > 0)
            {
                for (int i = 0; i < _weapon_inventory.Count; i++)
                {
                    Handler.Default_Print($"{i + 1}) {_weapon_inventory[i].GetInfo()};");
                }
            }
            else
            {
                Handler.Default_Print("Empty :(");
            }
        }
        public void PrintArmors() // прінтимо усі обладунки
        {
            Handler.Default_Print("Armor inventory:");
            if(_armor_inventory.Count > 0)
            {
                for (int i = 0; i < _armor_inventory.Count; i++)
                {
                    Handler.Default_Print($"{i + 1}) {_armor_inventory[i].GetInfo()};");
                }
            }
            else
            {
                Handler.Default_Print("Empty :(");
            }
        }
        public void PrintFood() // прінтимо весь хавчик
        {
            Handler.Default_Print("Food inventory:");
            if (_food.Count > 0)
            {
                for (int i = 0; i < _food.Count; i++)
                {
                    Handler.Default_Print($"{i + 1}) {_food[i].GetInfo()};");
                }
            }
            else
            {
                Handler.Default_Print("Empty :(");
            }
        }
        public void PrintCurrentWeaponAndArmor() // прінтим наш теперішній лут
        {
            Handler.Default_Print($"Current Weapon:\n" +
                $"{_current_weapon.GetInfo()}\n" +
                $"Current Armor:\n" +
                $"{_current_armor.GetInfo()}");
        }

        public void AddItem(Food food) // оверлоад для їжі
        {
            if(food == null)
            {
                throw new NullItem("Food is null.");
            }
            if (checkIfPlayerHasItem(food)) // якщо вже є така їжа то..
            {
                _food[findItem(food)]._quantity += food._quantity; // ..збільшуєм її кількість
            }
            else
            {
                _food.Add((Food)food.Clone()); // якщо ні то добавляєм новий екземпляр айтему
            }
            // повідомляєм про дію
            Handler.Special3_Print($"x{food._quantity} '{food._name}' was/were added to your inventory.");
        }
        public void AddItem(Armor armor) // оверлоад для обладунків
        {
            if (armor == null)
            {
                throw new NullItem("Armor is null.");
            }
            _armor_inventory.Add((Armor)armor.Clone()); // додаєм до інших обладунків
            // повідомляєм про дію
            Handler.Special3_Print($"'{armor._name}' was added to your inventory.");
        }
        public void AddItem(Weapon weapon) // оверлоад для зброї
        {
            if (weapon == null)
            {
                throw new NullItem("Weapon is null.");
            }
            _weapon_inventory.Add((Weapon)weapon.Clone()); // додаєм до інших озброєнь
            // повідомляєм про дію
            Handler.Special3_Print($"'{weapon._name}' was added to your inventory.");
        }
        // перше знаходин айтем, який потрібно використати
        public void UseItem(int id)
        {
            for(int i = 0; i < _food.Count; i++)
            {
                if(id == _food[i]._id)
                {
                    UseItem(_food[i]);
                    return;
                }
            }
            Handler.Special2_Print($"\nItem with {id} ID, wasn't found in your inventory.");
        }
        // а тепер кидаєм його сюди
        public void UseItem(Food food) // оверлоад для їжі
        {
            if(_hp < _max_hp) // перевіряєм чи потрібно відновлювати здоров'я і чи є їжа для відновлення його
            {
                // якщо так, то перше відновлюємо його
                _hp += food._recovering_points;
                checkHP(); // якщо хп більше макс хп, то прирівнюємо хп до макс хп
                // тепер виводимо повідомлення, так як у якісь момент, того айтему може не стати..
                // ..і ми не зможемо до нього звернутись для виведення інформації за допомогою _food[i] (чуть нижче після видалення)
                Handler.Special3_Print($"\nThe '{food._name}' was used. +{food._recovering_points} HP recovered. " +
                    $"Current HP: {_hp}{(_hp == _max_hp ? " (MAXXED)" : "")}");
                // після використання їжі..
                if (_food[findItem(food)]._quantity == 1)
                {
                    _food.RemoveAt(findItem(food)); // ..видаляємо її або..
                    // ось чому перше ми вивели повідомлення, так як тут ми видалили цей айтем з списку їжі
                }
                else
                {
                    _food[findItem(food)]._quantity -= 1; // ..зменшуємо її к-сть на 1
                }
            }
            else if(_hp == _max_hp) // якщо ХП на максимумі
            {
                Handler.Default_Print($"\nYour HP is full.");
            }
            else // якщо їжа не була знайдена у інвентарі (це заглушка якщо виникне помилка)
            {
                Handler.Special2_Print($"\n'{food._name}' wasn't found in your inventory.");
            }
        }
        public void Lose(Mob mob) // програш гравця
        {
            Handler.Special2_Print($"\n'{mob._name}' killed you. -20% of your gold.");
            // -80% голди
            _gold = (_gold / 100) * 80;
            updateLVL();
            if(_hp <= 0)
            {
                _hp = _max_hp;
            }
        }
        public void RunAway() // втеча гравця
        {
            Handler.Special2_Print($"You've run away. -20% of your gold.");
            _gold = (_gold / 100) * 80;
            updateLVL();
        }
        public void Attack(ref Mob mob) // атака гравця
        {
            // перевіряєм чи те що ми отримали не є нулл
            if (mob == null)
            {
                throw new NullEntity("Entity is null.");
            }
            if (_hp <= 0)
            {
                throw new EntityIsDeadAlready("Player is dead (can't attack).");
            }
            // перевіряєм чи в нього достатньо хп
            if (mob._hp > 0)
            {
                // якщо є то:
                int dmg_to_deal = new Random().Next(_current_weapon._dmg.Item1, _current_weapon._dmg.Item2); // урон який ми завдаєм
                if(getTotalCritChance() > 0) // якщо є шанс на критичний удар то..
                {
                    if (getTotalCritChance() >= new Random().Next(1, 100)) // ..пробуємо його здобути
                    {
                        // якщо фортануло то подвоюємо урон
                        dmg_to_deal *= 2;
                        Handler.Special3_Print("!!! CRITICAL HIT !!! (x2 DMG)");
                    }
                }
                mob._hp -= dmg_to_deal; // знімаємо у моба хп
                // якщо після нашогу удару в моба 0 хп то гравець переміг
                if (mob._hp <= 0)
                {
                    // mob.Lose(); // програш моба (program.main)
                    int lvl_points = mob._lvl_points_reward; // лвл поінти за перемогу
                    int gold = new Random().Next(mob._gold_reward.Item1, mob._gold_reward.Item2); // голда за перемогу
                    // добавляємо наше здобуте золото та рівень до загальних
                    _gold += gold;
                    _lvl_points += lvl_points;
                    Handler.Special1_Print($"You killed '{mob._name}'. +{gold} gold, +{lvl_points} lvl points.");
                    // апдейтим лвл
                    updateLVL(); // <-- умова усередині методу
                }
                // якщо моба не перемогли то просто виводим повідомлення з уроном по мобі
                else
                {
                    Handler.Special3_Print($"You attacked '{mob._name}' with {dmg_to_deal} DMG. Mob's HP: {mob._hp}");
                }
            }
            // якщо в моба менше 0 і він не був видалений.
            else
            {
                throw new EntityIsDeadAlready("Mob is dead already.");
            }
        }
        // перше знаходин айтем, який потрібно вдіти
        public void EquipItem(int id)
        {
            // перевіряємо, чи речі які ми хочемо вдіти, не є вже вдітими
            if (id == _current_weapon._id)
            {
                Handler.Default_Print($"\n'{_current_weapon._name}' is already equiped.");
                return;
            }
            if (id == _current_armor._id)
            {
                Handler.Default_Print($"\n'{_current_armor._name}' is already equiped.");
                return;
            }
            else
            {
                for(int i = 0; i < _weapon_inventory.Count; i++)
                {
                    if(id == _weapon_inventory[i]._id)
                    {
                        EquipItem(_weapon_inventory[i]);
                        return;
                    }
                }
                for (int i = 0; i < _armor_inventory.Count; i++)
                {
                    if (id == _armor_inventory[i]._id)
                    {
                        EquipItem(_armor_inventory[i]);
                        return;
                    }
                }
                Handler.Special2_Print($"\nItem with {id} ID wasn't found in your inventory.");
            }
        }
        // а тепер кидаєм його сюди
        public void EquipItem(Weapon weapon)
        {
            if (weapon == null)
            {
                throw new NullItem("Item to equip is null.");
            }
            else if (_current_weapon._id == weapon._id)
            {
                Handler.Default_Print($"\n'{weapon._name}' is already equiped.");
            }
            else
            {
                for (int i = 0; i < _weapon_inventory.Count; i++)
                {
                    if (weapon._id == _weapon_inventory[i]._id)
                    {
                        Handler.Special3_Print($"\n'{weapon._name}' was equiped. '{_current_weapon._name}' was moved to your inventory.");
                        _weapon_inventory.Add(_current_weapon);
                        _current_weapon = weapon;
                        _weapon_inventory.Remove(weapon);
                        return;
                    }
                }
                Handler.Special2_Print($"\n'{weapon._name}' wasn't found in your inventory.");
            }

        }
        public void EquipItem(Armor armor)
        {
            if (armor == null)
            {
                throw new NullItem("Item to equip is null.");
            }
            else if (_current_armor._id == armor._id)
            {
                Handler.Default_Print($"\n'{armor._name}' is already equiped.");
            }
            else
            {
                for (int i = 0; i < _armor_inventory.Count; i++)
                {
                    if (armor._id == _armor_inventory[i]._id)
                    {
                        _max_hp -= _current_armor._max_hp_bonus;
                        _max_hp += armor._max_hp_bonus;
                        Handler.Special3_Print($"\n'{armor._name}' was equiped. '{_current_armor._name}' was moved to your inventory.");
                        _armor_inventory.Add(_current_armor);
                        _current_armor = armor;
                        _armor_inventory.Remove(armor);
                        return;
                    }
                }
                Handler.Special2_Print($"\n'{armor._name}' wasn't found in your inventory.");
            }
        }
        public void SellItem(int id)
        {
            for (int i = 0; i < _weapon_inventory.Count; i++)
            {
                if (id == _weapon_inventory[i]._id)
                {
                    _gold += _weapon_inventory[i]._sell_price;
                    Handler.Special3_Print($"\n'{_weapon_inventory[i]._name}' was sold, +{_weapon_inventory[i]._sell_price} GOLD (30% of its original price)");
                    _weapon_inventory.RemoveAt(i);
                    return;
                }
            }
            for (int i = 0; i < _armor_inventory.Count; i++)
            {
                if (id == _armor_inventory[i]._id)
                {
                    _gold += _armor_inventory[i]._sell_price;
                    Handler.Special3_Print($"\n'{_armor_inventory[i]._name}' was sold, +{_armor_inventory[i]._sell_price} GOLD (30% of its original price)");
                    _armor_inventory.RemoveAt(i);
                    return;
                }
            }
            for (int i = 0; i < _food.Count; i++)
            {
                if(id == _food[i]._id)
                {
                    Handler.Default_Print($"\nYou can't sell FOOD.");
                    return;
                }
            }
            if (id == _current_weapon._id)
            {
                Handler.Default_Print($"\nYou can't sell the CURRENT Weapon.");
                return;
            }
            if (id == _current_armor._id)
            {
                Handler.Default_Print($"\nYou can't sell the CURRENT Armor");
                return;
            }
            else
            {
                Handler.Special2_Print($"\nItem with {id} ID wasn't found in your inventory.");
                return;
            }
        }
        // по факту він не потрібен гравцю (Clone)
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
