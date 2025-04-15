//dotnet add package ConsoleTables
using ConsoleTables;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("format 1:\n");
        var table = new ConsoleTable("ID", "Name", "Age", "ID", "Name", "Age", "ID", "Name", "Age");
        table.AddRow(1, "name1", 25, 1, "name1", 25, 1, "name1", 25)
             .AddRow(2, "name2", 30, 2, "name2", 30, 2, "name2", 30)
             .AddRow(3, "name3", 28, 3, "name3", 28, 3, "name3", 28);

        table.Write();

        Console.WriteLine("\n");
        Console.WriteLine(new string('-', 100));
        Console.WriteLine("format 2:\n\n");
        
        Console.ForegroundColor = ConsoleColor.White;
        //Console.ResetColor();
        Console.WriteLine(table.ToMinimalString());


        Console.WriteLine("\n");
        Console.WriteLine(new string('-', 100));
        Console.WriteLine("format 3:\n\n");

        Console.WriteLine(table.ToStringAlternative());

        Console.WriteLine("\n");
        Console.WriteLine(new string('-', 100));
        Console.WriteLine("format 4:\n\n");

        var people = new List<Person>
    {
        new Person { ID = 1, Name = "name1", Age = 25, number = 1, lastName = "name1", Age2 = 25 },
        new Person { ID = 2, Name = "name2", Age = 30, number = 2, lastName = "name2", Age2 = 30 },
    };

        ConsoleTable
            .From(people)
        .Write();

        Console.ReadLine();
    }

    class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int number { get; set; }
        public string lastName { get; set; }
        public int Age2 { get; set; }
    }
}