namespace MyApp.Program;

public class Student(string name, int age, int grade)
{
    public string Name { get; set; } = name;
    public int Age { get; set; } = age;
    public int Grade { get; set; } = grade;

    public void DisplayInfo()
    {
        Console.WriteLine($"Name: {Name}, Age: {Age}, Grade: {Grade}");
    }
}

public class Program
{
    public static void Main()
    {
        // Task 1
        FizzBuzz();

        // Task 2
        List<string> words = StringAnlyzer();
        Console.WriteLine($"Following are the words in the sentences");
        foreach (string word in words)
        {
            Console.WriteLine(word);
        }

        // Task 3
        Calculator();

        // Task 4
        List<string> top3 = top3Words();
        Console.WriteLine("The Top 3 Longest Words are:");
        foreach (string word in top3)
        {
            Console.WriteLine(word);
        }

        // Task 5
        StudentFilter();

        // Task 6
        var strs = palindromeFinder();
        foreach (string s in strs)
        {
            Console.WriteLine(s);
        }
    }

    /*
        Task 1. FizzBuzz 
    .WriteLine($"The Longest Word in the Sentence is {longestWord}); a loop from 1 to 100:
            Print "Fizz" if divisible by 3,
            "Buzz" if divisible by 5,
            "FizzBuzz" if divisible by both,
            Otherwise print the number.
            Count how many times "FizzBuzz" appears.
    */
    public static void FizzBuzz()
    {
        int count = 0;

        for (int i = 1; i <= 100; i++)
        {
            if (i % 3 == 0 && i % 5 == 0)
            {
                count++;
                Console.WriteLine("FizzBuzz");
            }
            else if (i % 3 == 0)
            {
                Console.WriteLine("Fizz");
            }
            else if (i % 5 == 0)
            {
                Console.WriteLine("Buzz");
            }
            else
            {
                Console.WriteLine(i);
            }
        }

        Console.WriteLine($"the Phrase FizzBuzz occus {count} times.");
    }

    /*
        Task 2: String Analyzer
            Take a sentence as input and:
            Count the number of words.
            Find the longest word.
            Return a list of words longer than 5 characters.
    */
    public static List<string> StringAnlyzer()
    {
        Console.WriteLine("Enter a Sentence");
        string sentence = Console.ReadLine() ?? string.Empty;

        string[] words = sentence.Split(" ");

        int workCount = words.Length;

        Console.WriteLine($"The Number of Words in the Sentence are {workCount}");

        string longestWord = words[0];

        List<string> bigwords = [];

        foreach (string word in words)
        {
            int wordLength = word.Length;
            if (wordLength > longestWord.Length)
            {
                longestWord = word;
            }

            if (wordLength > 5)
            {
                bigwords.Add(word);
            }
        }

        Console.WriteLine($"The Longest Word in the Sentence is {longestWord}");

        return bigwords;
    }

    /*
        Task 3: Create a loop-driven menu:
            1. Add
            2. Subtract
            3. Multiply
            4. Divide
            5. Exit
    */
    public static void Calculator()
    {
        bool exit = false;

        string menu = @"
        Following Operations are Permitted:
            1. Add (+).
            2. Sub (-).
            3. Mul (*).
            4. Div (/).
            x. Exit (x).
        ";

        while (!exit)
        {
            Console.WriteLine(menu);

            Console.Write("Select an Operation: ");
            char op = char.Parse(Console.ReadLine());

            Console.Write("Enter numbers x}");
            int x = int.Parse(Console.ReadLine());

            Console.Write("Enter numbers y}");
            int y = int.Parse(Console.ReadLine());



            switch (op)
            {
                case '+':
                    Console.WriteLine($"The Sum of {x} and {y} is {x + y}");
                    break;
                case '-':
                    Console.WriteLine($"The Difference of {x} and {y} is {x - y}");
                    break;
                case '*':
                    Console.WriteLine($"The Product of {x} and {y} is {x * y}");
                    break;
                case '/':
                    if (y == 0)
                    {
                        Console.WriteLine("Division by zero is not allowed.");
                    }
                    else
                    {
                        Console.WriteLine($"The Quotient of {x} and {y} is {x / y}");
                    }
                    break;
                case 'x':
                    exit = true;
                    Console.WriteLine("Exiting the Calculator.");
                    break;
                default:
                    Console.WriteLine("Invalid operation. Please try again.");
                    break;
            }

            if (!exit)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }


    /*
        Task 4: Top 3 Longest Words
            Given a list of strings.
            Use LINQ to find the top 3 longest words.
            Return them sorted by length descending.
    */
    public static List<string> top3Words()
    {
        List<string> words = ["apple", "banana", "grapefruit", "kiwi", "blueberry", "strawberry"];

        IEnumerable<string> query =
            from word in words
            orderby word.Length descending
            select word;

        List<string> top3 = query.Take(3).ToList();

        return top3;
    }

    /*
        Task 5: Student Filter
            Define a Student class with:
                Name, Age, Grade
                Create a list of students and:
                Find all students with grade > 75.
                Sort them by grade descending.
                Group them by age and print how many in each age group.
    */
    public static void StudentFilter()
    {
        List<Student> students =
        [
            new Student("Rushikesh", 21, 76 ),
            new Student("Aaditi", 22, 98),
            new Student("Pravin", 22, 73),
            new Student("Anushka", 21, 99 )
        ];


        var toppers = students
        .Where(s => s.Grade > 75)
        .OrderByDescending(s => s.Grade)
        .GroupBy(s => s.Age)
        .ToList();

        foreach (var group in toppers)
        {
            Console.WriteLine($"Age: {group.Key}, Count: {group.Count()}");
            foreach (var student in group)
            {
                student.DisplayInfo();
            }
        }
    }

    /* 
        Task 6: Palindrom Finder 
            From a list of strings, return all palindromes (e.g., "madam", "level"), ignoring case.
    */

    public static List<string> palindromeFinder()
    {
        string[] words = ["madam", "nitin", "level", "mama", "abccba", "Rushi", "xyz"];

        List<string> palindromes = [];

        foreach (string word in words)
        {
            if (isPalindrome(word))
            {
                palindromes.Add(word);
            }
        }

        return palindromes;
    }

    private static bool isPalindrome(string word)
    {
        for (int i = 0; i < word.Length / 2; i++)
        {
            if (word[i] != word[^(i + 1)])
            {
                return false;
            }
        }

        return true;
    }
}