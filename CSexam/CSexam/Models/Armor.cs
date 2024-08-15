using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSexam.MsgHandlers;

namespace CSexam.Models
{
    public class Armor : IItem
    {
        //
        // variables
        //
        public string _name { get; set; }
        public int _id { get; set; }
        public int _defence { get; set; }
        public int _max_hp_bonus { get; set; } // special
        public int _crit_chance { get; set; } // special
        public int _lvl { get; set; }
        public int _price { get; set; }
        public int _sell_price { get; set; }
        //
        // public methods
        //
        public Armor(string name, int id, int defence, int max_hp_bonus, int crit_chance, int lvl, int price) // constructor
        {
            _name = name;
            _id = id;
            _defence = defence;
            _max_hp_bonus = max_hp_bonus;
            _crit_chance = crit_chance;
            _lvl = lvl;
            _price = price;
            _sell_price = (price / 100) * 30;
        }
        public Armor() { }
        public string GetInfo()
        {
            return $"'{_name}', ID: {_id}, Defence: {_defence}%, Max HP bonus: +{_max_hp_bonus}, Crit chance: {_crit_chance}%, Price: {_price} GOLD";
        }
        public void PrintInfo()
        {
            Handler.Default_Print($"'{_name}', ID: {_id}, Defence: {_defence}%, Max HP bonus: +{_max_hp_bonus}, Crit chance: {_crit_chance}%, Price: {_price} GOLD");
        }
        public object Clone()
        {
            return new Armor(_name, _id, _defence, _max_hp_bonus, _crit_chance, _lvl, _price);
        }
    }
}
