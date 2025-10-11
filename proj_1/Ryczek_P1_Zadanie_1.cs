// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
#nullable disable

Console.WriteLine("Enter the size of the matrix:");
int size = int.Parse(Console.ReadLine());
int[,] matrix = new int[size, size];
Random rand = new Random();

int option;

for (; ; )
{
    Console.WriteLine(
        "1. Load matrix with your own values\n2. Load matrix with random values\n3. Display the matrix\n4. Exit"
    );
    option = int.Parse(Console.ReadLine());

    switch (option)
    {
        case 1:
            Console.WriteLine("Input your own values...");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write($"Enter value for position [{i},{j}]: ");
                    matrix[i, j] = int.Parse(Console.ReadLine());
                }
            }
            break;
        case 2:
            Console.WriteLine("Loading random values.");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = rand.Next(1, 101);
                }
            }
            break;

        case 3:
            Console.WriteLine("Displaying the matrix:");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
            break;
        case 4:
            Console.WriteLine("Exiting...");
            return;
        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}
