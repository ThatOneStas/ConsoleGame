using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CSexam.Exceptions;
using CSexam.MsgHandlers;

namespace CSexam.Models
{
    public class Mob : IEntity
    {
        //
        // variables
        //
        public string _name { get; set; }
        public int _hp { get; set; }
        public Tuple<int,int> _dmg { get; set; }
        public int _lvl { get; set; }
        public Tuple<int, int> _gold_reward { get; set; } // нагорода в діапазоні (золото)
        public int _lvl_points_reward { get; set; } // нагорода в діапазоні (золото)

        //
        // private methods
        //
        private void updateStats() // бонус до складності моба за кожний його лвл
        {
            if(_lvl != 1)
            {
                // !!перший лвл не береться до уваги так як він базовий!!
                int buff_quant = _lvl - 1;
                // якщо в моба наприклад 5 лвл то..
                _hp += (int)((float)_hp / 100) * (buff_quant * 40); // .. він отримає баф в розмірі 160%
                // а тут бонус до урону, але за тих самих 160% до хп, тут він отримає лише 80%
                _dmg = new Tuple<int, int>((int)(_dmg.Item1 + (((float)_dmg.Item1 / 100) * (buff_quant * 20))), (int)(_dmg.Item2 + (((float)_dmg.Item2 / 100) * (buff_quant * 20))));
                // тут так само
                _gold_reward = new Tuple<int, int>((int)(_gold_reward.Item1 + (((float)_gold_reward.Item1 / 100) * (buff_quant * 20))), (int)(_gold_reward.Item2 + (((float)_gold_reward.Item2 / 100) * (buff_quant * 20))));
            }
        }
        //
        // public methods
        //
        public Mob(string name, int hp, Tuple<int,int> dmg, int lvl, Tuple<int, int> gold_reward,int lvl_points_reward) // constructor
        {
            _name = name;
            _hp = hp;
            _dmg = dmg;
            _lvl = lvl;
            _gold_reward = gold_reward;
            _lvl_points_reward = lvl_points_reward;
            updateStats();
        }
        public void PrintInfo()
        {
            Handler.Default_Print($"Name: '{_name}', LVL: {_lvl};" +
                $"\n(STATS) DMG: {_dmg.Item1} - {_dmg.Item2}, HP: {_hp};" +
                $"\n(REWARDS) Gold: {_gold_reward.Item1} - {_gold_reward.Item2}, LVL points: {_lvl_points_reward}.");
        }
        public string GetInfo()
        {
            return ($"Name: '{_name}', LVL: {_lvl};" +
                $"\n(STATS) DMG: {_dmg.Item1} - {_dmg.Item2}, HP: {_hp};" +
                $"\n(REWARDS) Gold: {_gold_reward.Item1} - {_gold_reward.Item2}, LVL points: {_lvl_points_reward}.");
        }
        public void Attack(ref Player player) // атака моба
        {
            // перевіряєм чи те що ми отримали не є нулл
            if (player == null)
            {
                throw new NullEntity("Entity is null.");
            }
            if (_hp <= 0)
            {
                throw new EntityIsDeadAlready("Mob is dead (can't attack).");
            }
            if (player._hp > 0)
            {
                int dmg_to_deal = new Random().Next(_dmg.Item1, _dmg.Item2);
                // у гравця може бути броня, яка захищає його у процентах від нанесеного мобами урону
                // тому цей момент треба обробити
                dmg_to_deal = dmg_to_deal - (dmg_to_deal / 100 * player._current_armor._defence);
                player._hp -= dmg_to_deal;
                Handler.Special2_Print($"'{_name}' attacked you with {dmg_to_deal} DMG. Your HP: {player._hp}");
                if (player._hp < 0)
                {
                    // player.Lose(this); // програш гравця (program.main)
                }
            }
            else
            {
                throw new EntityIsDeadAlready("Player is dead already.");
            }
        }
        public void Lose() // програш моба
        {
            // deconstruct
        }
        public object Clone()
        {
            return new Mob(_name, _hp, new Tuple<int, int>(_dmg.Item1, _dmg.Item2), _lvl, new Tuple<int, int>(_gold_reward.Item1, _gold_reward.Item2), _lvl_points_reward);
        }
    }
}
