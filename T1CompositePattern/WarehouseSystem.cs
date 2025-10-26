using System;
using System.Collections.Generic;

namespace T1CompositePattern
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public decimal GetPrice()
        {
            return Price;
        }

        public void Display(int depth = 0)
        {
            string indent = new string(' ', depth * 4);
            Console.WriteLine($"{indent}Продукт: {Name} - {Price} руб.");
        }
    }

    public class Box
    {
        public string Name { get; set; }
        public decimal PackagingCost { get; set; }
        public List<object> Contents { get; set; }

        //Конструктор коробки
        public Box(string name, decimal packagingCost = 0)
        {
            Name = name;
            PackagingCost = packagingCost;
            Contents = new List<object>();
        }

        public void AddItem(object item)
        {
            Contents.Add(item);
        }

        //Расчёт общей стоимости коробки
        public decimal GetPrice()
        {
            decimal totalPrice = PackagingCost;
            foreach (var item in Contents)
            {
                if (item is Product product)
                {
                    totalPrice += product.GetPrice();
                }
                else if (item is Box box)
                {
                    totalPrice += box.GetPrice();
                }
            }
            return totalPrice;
        }

        public void Display(int depth = 0)
        {
            string indent = new string(' ', depth * 4);
            Console.WriteLine($"{indent}Коробка: {Name} (упаковка: {PackagingCost} руб.)");
            foreach (var item in Contents)
            {
                if (item is Product product)
                {
                    product.Display(depth + 1);
                }
                else if (item is Box box)
                {
                    box.Display(depth + 1);
                }
            }
        }
    }

    public class Order
    {
        public List<object> Items { get; set; }

        public Order()
        {
            Items = new List<object>();
        }

        public void AddItem(object item)
        {
            Items.Add(item);
        }

        public decimal CalculateTotalPrice()
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                if (item is Product product)
                {
                    total += product.GetPrice();
                }
                else if (item is Box box)
                {
                    total += box.GetPrice();
                }
            }
            return total;
        }

        public void DisplayOrderContents()
        {
            Console.WriteLine("Состав заказа:");
            Console.WriteLine("==================");
            foreach (var item in Items)
            {
                if (item is Product product)
                {
                    product.Display();
                }
                else if (item is Box box)
                {
                    box.Display();
                }
            }
            Console.WriteLine("==================");
            Console.WriteLine($"Общая стоимость заказа: {CalculateTotalPrice()} руб.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Создаем продукты
            var laptop = new Product("Ноутбук", 120000);
            var mouse = new Product("Компьютерная мышь", 2500);
            var keyboard = new Product("Механическая клавиатура", 8000);
            var headphones = new Product("Наушники", 15000);
            var charger = new Product("Зарядное устройство", 3000);
            var usbFlash = new Product("USB-флешка", 1200);

            //Создаем коробки разных уровней
            var smallBox = new Box("Маленькая коробка", 200);
            var mediumBox = new Box("Средняя коробка", 500);
            var largeBox = new Box("Большая коробка", 1000);

            //Комплектуем маленькую коробку
            smallBox.AddItem(mouse);
            smallBox.AddItem(usbFlash);

            //Комплектуем среднюю коробку
            mediumBox.AddItem(keyboard);
            mediumBox.AddItem(headphones);
            mediumBox.AddItem(smallBox);

            //Комплектуем большую коробку
            largeBox.AddItem(laptop);
            largeBox.AddItem(mediumBox);
            largeBox.AddItem(charger);

            //Создаем заказ
            var order = new Order();

            //Добавляем компоненты в заказ
            order.AddItem(largeBox);
            order.AddItem(new Product("Дополнительная гарантия", 5000)); //Простой продукт без упаковки

            //Отображаем состав заказа и общую стоимость
            order.DisplayOrderContents();
        }
    }
}