using System;

namespace T5DecoratorPattern
{
    public abstract class DeliverySystem //абстрактный класс для всех систем доставки
    {
        public abstract decimal CalculateCost();
        public abstract string GetDescription();
        public abstract int GetDeliveryTime();
    }

    public class ExpressDeliverySystem : DeliverySystem //класс экспресс-доставки
    {
        private readonly DeliverySystem _baseSystem;

        public ExpressDeliverySystem(DeliverySystem baseSystem)
        {
            _baseSystem = baseSystem;
        }

        public override decimal CalculateCost()
        {
            decimal baseCost = _baseSystem.CalculateCost();
            decimal expressSurcharge = CalculateExpressSurcharge();
            return baseCost + expressSurcharge;
        }

        public override string GetDescription()
        {
            return _baseSystem.GetDescription() + " (Экспресс)";
        }

        public override int GetDeliveryTime()
        {
            int baseTime = _baseSystem.GetDeliveryTime();
            return Math.Max(1, baseTime / 2);
        }

        public string TrackDelivery(string trackingNumber)
        {
            return $"Статус экспресс-доставки {trackingNumber}: В пути, прибытие через {GetDeliveryTime()} дней";
        }

        public decimal CalculateExpressCost()
        {
            return CalculateCost();
        }

        private decimal CalculateExpressSurcharge()
        {
            return 10.0m;
        }
    }

    public class ExpressDeliveryDecorator : DeliverySystem //класс-обёртка для экспресс-доставки
    {
        private readonly ExpressDeliverySystem _expressSystem;

        public ExpressDeliveryDecorator(DeliverySystem baseSystem)
        {
            _expressSystem = new ExpressDeliverySystem(baseSystem);
        }

        public override decimal CalculateCost()
        {
            return _expressSystem.CalculateCost();
        }

        public override string GetDescription()
        {
            return _expressSystem.GetDescription();
        }

        public override int GetDeliveryTime()
        {
            return _expressSystem.GetDeliveryTime();
        }

        public string TrackDelivery(string trackingNumber)
        {
            return _expressSystem.TrackDelivery(trackingNumber);
        }

        public decimal CalculateExpressCost()
        {
            return _expressSystem.CalculateExpressCost();
        }
    }

    public class CourierDelivery : DeliverySystem //курьерская доставка
    {
        public override decimal CalculateCost() => 10m;
        public override string GetDescription() => "Курьерская доставка";
        public override int GetDeliveryTime() => 3;
    }

    public class PostalDelivery : DeliverySystem //почтовая доставка
    {
        public override decimal CalculateCost() => 5m;
        public override string GetDescription() => "Почтовая доставка";
        public override int GetDeliveryTime() => 7;
    }

    public class PickupDelivery : DeliverySystem //самовывоз
    {
        public override decimal CalculateCost() => 0m;
        public override string GetDescription() => "Самовывоз";
        public override int GetDeliveryTime() => 1;
    }

    internal class T5DecoratorPattern
    {
        static void Main(string[] args)
        {
            DeliverySystem courier = new CourierDelivery(); //создание экземпляров базовых способов
            DeliverySystem postal = new PostalDelivery();
            DeliverySystem pickup = new PickupDelivery();

            Console.WriteLine("=== Базовые способы доставки ==="); //тест базовых способов
            TestDeliverySystem(courier);
            TestDeliverySystem(postal);
            TestDeliverySystem(pickup);

            Console.WriteLine("\n=== Экспресс-доставка через декоратор ===");

            DeliverySystem expressCourier = new ExpressDeliveryDecorator(new CourierDelivery()); //создание декорированных версий с экспресс-доставкой
            DeliverySystem expressPostal = new ExpressDeliveryDecorator(new PostalDelivery());
            DeliverySystem expressPickup = new ExpressDeliveryDecorator(new PickupDelivery());

            TestExpressDeliverySystem(expressCourier); //тест декорированных систем
            TestExpressDeliverySystem(expressPostal); 
            TestExpressDeliverySystem(expressPickup);
        }

        static void TestDeliverySystem(DeliverySystem system) //метод тестирования базовой доставки
        {
            Console.WriteLine($"Способ: {system.GetDescription()}");
            Console.WriteLine($"Стоимость: {system.CalculateCost()} руб.");
            Console.WriteLine($"Срок доставки: {system.GetDeliveryTime()} дней");
            Console.WriteLine();
        }

        static void TestExpressDeliverySystem(DeliverySystem system) //метод тестирования экспресс-доставки
        {
            if (system is ExpressDeliveryDecorator express)
            {
                Console.WriteLine($"Способ: {system.GetDescription()}");
                Console.WriteLine($"Стоимость: {system.CalculateCost()} руб.");
                Console.WriteLine($"Срок доставки: {system.GetDeliveryTime()} дней");

                string trackingNumber = "4K-" + DateTime.Now.Ticks;
                Console.WriteLine($"Отслеживание: {express.TrackDelivery(trackingNumber)}");
                Console.WriteLine($"Стоимость экспресс-доставки: {express.CalculateExpressCost()} руб.");
                Console.WriteLine();
            }
        }
    }

}