using System;
using System.Collections.Generic;
using System.Linq;

namespace T4FacadePattern
{
    public class Customer //класс клиента
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }

    public class Order //класс заказа
    {
        public int id { get; set; }
        public int custid { get; set; }
        public decimal total { get; set; }
    }

    public class CustomerDatabase //класс для работы с базой данных клиентов
    {
        private List<Customer> _custs = new List<Customer>
    {
        new Customer { id = 1, name = "cust1", email = "cust1@mail.com" },
        new Customer { id = 2, name = "cust2", email = "cust2@mail.com" }
    };

        public Customer GetCustomer(int id)
        {
            Console.WriteLine($"[CustomerDB] Получение пользователя {id}");
            return _custs.FirstOrDefault(u => u.id == id);
        }

        public void SaveCustomer(Customer cust)
        {
            Console.WriteLine($"[CustomerDB] Сохранение пользователя {cust.name}");
            var existing = _custs.FirstOrDefault(u => u.id == cust.id);
            if (existing != null)
                _custs.Remove(existing);
            _custs.Add(cust);
        }
    }

    public class OrderDatabase //класс для работы с базой даннных заказов
    {
        private List<Order> _orders = new List<Order>
    {
        new Order { id = 1, custid = 1, total = 100.50m },
        new Order { id = 2, custid = 1, total = 75.25m }
    };
        private int _nextid = 3;

        public Order GetOrder(int id)
        {
            Console.WriteLine($"[OrderDB] Получение заказа {id}");
            return _orders.FirstOrDefault(o => o.id == id);
        }

        public void SaveOrder(Order order)
        {
            Console.WriteLine($"[OrderDB] Сохранение заказа {order.id}");
            if (order.id == 0)
            {
                order.id = _nextid++;
                _orders.Add(order);
            }
            else
            {
                var existing = _orders.FirstOrDefault(o => o.id == order.id);
                if (existing != null)
                {
                    existing.total = order.total;
                    existing.custid = order.custid;
                }
            }
        }

        public List<Order> GetCustomerOrders(int custid)
        {
            Console.WriteLine($"[OrderDB] Получение заказов пользователя {custid}");
            return _orders.Where(o => o.custid == custid).ToList();
        }
    }

    public class DatabaseFacade //фасад,скрывающий сложность работы с базами данных
    {
        private readonly CustomerDatabase _customerDb;
        private readonly OrderDatabase _orderDb;

        public DatabaseFacade()
        {
            _customerDb = new CustomerDatabase();
            _orderDb = new OrderDatabase();
        }

        public Customer GetCustomerWithOrders(int custid)
        {
            Console.WriteLine($"\n=== Получение пользователя {custid} с заказами ===");
            var cust = _customerDb.GetCustomer(custid);
            var orders = _orderDb.GetCustomerOrders(custid);

            Console.WriteLine($"Пользователь: {cust.name}, Заказов: {orders.Count}");
            return cust;
        }

        public void CreateOrder(int custid, decimal total)
        {
            Console.WriteLine($"\n=== Создание заказа для пользователя {custid} ===");
            var cust = _customerDb.GetCustomer(custid);
            var order = new Order { custid = custid, total = total };

            _orderDb.SaveOrder(order);
            Console.WriteLine($"Создан заказ на сумму {total} для {cust.name}");
        }

        public decimal GetCustomerTotalSpent(int custid)
        {
            Console.WriteLine($"\n=== Расчет общей суммы покупок пользователя {custid} ===");
            var orders = _orderDb.GetCustomerOrders(custid);
            decimal total = 0;

            foreach (var order in orders)
            {
                total += order.total;
            }

            Console.WriteLine($"Общая сумма: {total}");
            return total;
        }
    }

    internal class FacadePattern
    {
        static void Main(string[] args)
        {
            var database = new DatabaseFacade(); //создание объекта фасада для работы с системами

            database.GetCustomerTotalSpent(1); //показ начальной суммы покупок клиента

            database.GetCustomerWithOrders(1); //получение информации о клиенте и его заказах
            database.CreateOrder(1, 952m); //создание нового заказа

            database.GetCustomerTotalSpent(1); //показ обновленной суммы покупок
        }
    }
}