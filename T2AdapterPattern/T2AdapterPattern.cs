using System;

public interface IRoad
{
    string Type { get; }
    void DisplayRoadInfo();
}

public class Highway : IRoad //конкретика дороги, шоссе
{
    public string Type => "шоссе";

    public void DisplayRoadInfo()
    {
        Console.WriteLine($"Дорога типа '{Type}' идеально подходит для быстрой езды");
    }
}

public class CountryRoad : IRoad //конкретика дороги, просёлочная дорога
{
    public string Type => "просёлочная дорога";

    public void DisplayRoadInfo()
    {
        Console.WriteLine($"Дорога типа '{Type}' подходит для неспешной езды");
    }
}

public interface ITransport //интерфейс транспорта
{
    void Ride(IRoad road);
}


public class Car : ITransport //класс машины (уже транспорт)
{
    private readonly string _model;

    public Car(string model)
    {
        _model = model;
    }

    public void Ride(IRoad road)
    {
        Console.WriteLine($"Машина {_model} едет по дороге типа '{road.Type}'");
        road.DisplayRoadInfo();
    }
}


public class Donkey //класс осла (не является транспортом)
{
    public string Name { get; }

    public Donkey(string name)
    {
        Name = name;
    }

    public void Eat()
    {
        Console.WriteLine($"Осёл {Name} кушает сено");
    }
}


public class Saddle : ITransport //адаптер - седло, превращающее осла в транспорт
{
    private readonly Donkey _donkey;

    public Saddle(Donkey donkey)
    {
        _donkey = donkey;
    }

    public void Ride(IRoad road)
    {
        Console.WriteLine($"Осёл {_donkey.Name} с седлом идёт по дороге типа '{road.Type}'");
        if (road.Type == "шоссе")
        {
            Console.WriteLine($"Седло позволяет всаднику комфортно ехать на осле по ровной дороге");
        }
        else
        {
            Console.WriteLine($"Седло обеспечивает устойчивость при езде на осле по неровной дороге");
        }
        road.DisplayRoadInfo();
    }
}

class T2AdapterPattern
{
    static void Main(string[] args)
    {
        var highway = new Highway();
        var countryRoad = new CountryRoad();

        var car = new Car("Toyota Camry");
        car.Ride(highway);
        Console.WriteLine();

        var donkey = new Donkey("Муня");
        donkey.Eat();
        Console.WriteLine();

        var donkeywithSaddle = new Saddle(donkey); //осёл  с седлом - транспорт (адаптер)
        donkeywithSaddle.Ride(countryRoad);
        Console.WriteLine();

        car.Ride(countryRoad);
        Console.WriteLine();
        donkeywithSaddle.Ride(highway);
    }
}