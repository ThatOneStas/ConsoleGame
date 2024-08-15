using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSexam.MsgHandlers;

namespace CSexam.Models
{
    public class Weapon : IItem
    {
        //
        // variables
        //
        public string _name { get; set; }
        public int _id { get; set; }
        public Tuple<int, int> _dmg { get; set; }
        public int _crit_chance { get; set; } // special
        public int _lvl { get; set; }
        public int _price { get; set; }
        public int _sell_price { get; set; }
        //
        // public methods
        //
        public Weapon(string name, int id, Tuple<int,int> dmg, int crit_chance, int lvl, int price) // constructor
        {
            _name = name;
            _id = id;
            _dmg = dmg;
            _crit_chance = crit_chance;
            _lvl = lvl;
            _price = price;
            _sell_price = (price / 100) * 30;
        }
        public Weapon() { }
        public string GetInfo()
        {
            return $"'{_name}', ID: {_id}, DMG: {_dmg.Item1} - {_dmg.Item2}, Crit chance: {_crit_chance}%, Price: {_price} GOLD";
        }
        public void PrintInfo()
        {
            Handler.Default_Print($"'{_name}', ID: {_id}, DMG: {_dmg.Item1} - {_dmg.Item2}, Crit chance: {_crit_chance}%, Price: {_price} GOLD");
        }
        public object Clone()
        {
            return new Weapon(_name, _id, new Tuple<int,int>(_dmg.Item1, _dmg.Item2), _crit_chance, _lvl, _price);
        }
    }
}
