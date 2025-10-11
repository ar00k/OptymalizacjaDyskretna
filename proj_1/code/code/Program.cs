#nullable disable
// using System.IO;

Console.WriteLine("Enter the size of the matrix:");
int size = int.Parse(Console.ReadLine());
int[,] matrix = new int[size, size];
Random rand = new Random();

int option;

for (;;)
{
    Console.WriteLine("1. Load matrix with your own values\n2. Load matrix with random values\n3. Display the matrix\n4. Load matrix from file\n5. Save matrix to file\n6. Find min value from each column\n9. Exit");
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
                matrix[i, j] = rand.Next(1, 16);
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
        try
        {
            Console.Write("Enter filename (e.g., p1.txt): ");
            string filename = Console.ReadLine();
            StreamReader sr = new StreamReader(filename);
            
            int fileSize = int.Parse(sr.ReadLine());
            
            if (fileSize != size)
            {
                size = fileSize;
                matrix = new int[size, size];
            }
            
            for (int i = 0; i < size; i++)
            {
                string[] values = sr.ReadLine().Split(' ');
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = int.Parse(values[j]);
                }
            }
            
            sr.Close();
            Console.WriteLine("Matrix loaded successfully from file.");
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        break;
    case 5:
        try
        {
            StreamWriter sw = new StreamWriter("p1.txt");
            
            sw.WriteLine(size);
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    sw.Write(matrix[i, j]);
                    if (j < size - 1) sw.Write(" ");
                }
                sw.WriteLine();
            }
            
            sw.Close();
            Console.WriteLine("Matrix saved successfully to p1.txt");
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        break;

    case  6: 
    Console.WriteLine("Finding min value from each column:");
    for (int j = 0; j < size; j++)
    {
        int min = matrix[0, j];
        for (int i = 1; i < size; i++)
        {
            if (matrix[i, j] < min)
            {
                min = matrix[i, j];
            }
        }
        Console.WriteLine($"Min value in column {j}: {min}");
    }
    break;

    case 9:
        Console.WriteLine("Exiting...");
        return;
    default:
        Console.WriteLine("Invalid option.");
        break;
}
}