using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSexam.MsgHandlers;

namespace CSexam.Models
{
    public class Food : IItem
    {
        //
        // variables
        //
        public string _name { get; set; }
        public int _id { get; set; }
        public int _recovering_points { get; set; }
        public int _price { get; set; }
        public int _quantity { get; set; }
        public int _lvl { get; set; } // ! лвл тут не потрібен ! (але інтерфейс вимагає)
        public Food _inner_food { get; set; } // їжа може містити їжу. пояснення з прикладом:
                               // ми створюємо корзину яблук, яка містить 5 яблук. Корзина яблук має різні айді,..
                               // порівняно з простим яблуком, тому хоч ми і будем при покупці звертатись до айді корзини..
                               // ми будем просто витягувати яблука з самої корзини.
        //
        // public methods
        //
        public Food(string name, int id, int recovering_points, int price, int quantity) // constructor 1
        {
            _name = name;
            _id = id;
            _recovering_points = recovering_points;
            _price = price;
            _quantity = quantity;
            _inner_food = null;
            _lvl = 1; // для їжі лвл не потрібен (якщо буде потрібен то зроблю інший конструктор)
        }
        public Food(string name, int id, int recovering_points, int price) // constructor 2
        {
            _name = name;
            _id = id;
            _recovering_points = recovering_points;
            _price = price;
            _inner_food = null;
            _quantity = 1;
            _lvl = 1; // для їжі лвл не потрібен (якщо буде потрібен то зроблю інший конструктор)
        }
        public Food(string name, int id, Food inner_food, int price) // constructor 3
        {
            _name = name;
            _id = id;
            _inner_food = inner_food;
            _recovering_points = inner_food._recovering_points;
            // we can setup price with discount or any other we want
            _price = price;
            _quantity = 1;
            _lvl = 1; // для їжі лвл не потрібен (якщо буде потрібен то зроблю інший конструктор)
        }
        public Food(string name, int id, Food inner_food) // constructor 4
        {
            _name = name;
            _id = id;
            _inner_food = inner_food;
            _recovering_points = inner_food._recovering_points;
            _price = inner_food._price * inner_food._quantity;
            _quantity = 1;
            _lvl = 1; // для їжі лвл не потрібен (якщо буде потрібен то зроблю інший конструктор)
        }
        public Food() { }
        public string GetInfo()
        {
            return $"'{_name}', ID: {_id}, recovering HP: {_recovering_points}, Price: {_price} GOLD{(_quantity > 1 ? $", quantity: {_quantity}":"")}";
        }
        public void PrintInfo()
        {
            Handler.Default_Print($"'{_name}', ID: {_id}, recovering HP: {_recovering_points}, Price: {_price} GOLD{(_quantity > 1 ? $", quantity: {_quantity}" : "")}");
        }
        public object Clone()
        {
            return new Food(_name, _id, _recovering_points, _price, _quantity);
        }
    }
}
