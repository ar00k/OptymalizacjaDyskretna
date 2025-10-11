#nullable disable
// using System.IO;

Console.WriteLine("Enter the size of the matrix:");
int size = int.Parse(Console.ReadLine());
int[,] matrix = new int[size, size];
Random rand = new Random();

int option;

for (;;)
{
    Console.WriteLine("1. Load matrix with your own values\n2. Load matrix with random values\n3. Display the matrix\n4. Load matrix from file\n5. Save matrix to file\n6. Find min value from each column\n7. Simplify matrix by removing smallest elements\n9. Exit");
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
            Console.Write("Enter filename (e.g., p2.txt): ");
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
            StreamWriter sw = new StreamWriter("p2.txt");
            
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
            Console.WriteLine("Matrix saved successfully to p2.txt");
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

    case 7:
    Console.WriteLine("Hungarian Algorithm - solving assignment problem:");
    
    // Create a copy of the matrix for the algorithm
    int[,] workMatrix = new int[size, size];
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            workMatrix[i, j] = matrix[i, j];
        }
    }
    
    Console.WriteLine("Original matrix:");
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            Console.Write(workMatrix[i, j] + "\t");
        }
        Console.WriteLine();
    }
    
    // Step 1: Subtract row minimums
    Console.WriteLine("\nStep 1: Subtract row minimums");
    for (int i = 0; i < size; i++)
    {
        int min = workMatrix[i, 0];
        for (int j = 1; j < size; j++)
        {
            if (workMatrix[i, j] < min)
            {
                min = workMatrix[i, j];
            }
        }
        
        Console.WriteLine($"Row {i} minimum: {min}");
        for (int j = 0; j < size; j++)
        {
            workMatrix[i, j] -= min;
        }
    }
    
    Console.WriteLine("After step 1:");
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            Console.Write(workMatrix[i, j] + "\t");
        }
        Console.WriteLine();
    }

    // Step 2: Subtract column minimums
    Console.WriteLine("\nStep 2: Subtract column minimums");
    for (int j = 0; j < size; j++)
    {
        int min = workMatrix[0, j];
        for (int i = 1; i < size; i++)
        {
            if (workMatrix[i, j] < min)
            {
                min = workMatrix[i, j];
            }
        }
        
        if (min > 0)
        {
            Console.WriteLine($"Column {j} minimum: {min}");
            for (int i = 0; i < size; i++)
            {
                workMatrix[i, j] -= min;
            }
        }
    }
    
    Console.WriteLine("After step 2:");
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            Console.Write(workMatrix[i, j] + "\t");
        }
        Console.WriteLine();
    }

    // Main algorithm loop
    int iteration = 0;
    while (true)
    {
        iteration++;
        Console.WriteLine($"\nIteration {iteration}:");
        
        // Find maximum matching
        int[] assignment = new int[size];
        for (int i = 0; i < size; i++) assignment[i] = -1;
        
        bool[] rowMatched = new bool[size];
        bool[] colMatched = new bool[size];
        int matchCount = 0;
        
        // Try to find maximum matching using zeros
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (workMatrix[i, j] == 0 && !rowMatched[i] && !colMatched[j])
                {
                    assignment[i] = j;
                    rowMatched[i] = true;
                    colMatched[j] = true;
                    matchCount++;
                    break;
                }
            }
        }
        
        Console.WriteLine($"Found {matchCount} matches");
        
        if (matchCount == size)
        {
            // Found optimal assignment
            int totalCost = 0;
            Console.WriteLine("\nOptimal assignment found:");
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine($"Worker {i+1} -> Job {assignment[i]+1}, Cost: {matrix[i, assignment[i]]}");
                totalCost += matrix[i, assignment[i]];
            }
            Console.WriteLine($"Total assignment cost: {totalCost}");
            break;
        }
        
        // Step 3: Find minimum vertex cover using König's theorem
        bool[] rowCovered = new bool[size];
        bool[] colCovered = new bool[size];
        
        // Start with unmatched rows
        bool[] visited = new bool[size];
        for (int i = 0; i < size; i++)
        {
            if (!rowMatched[i])
            {
                DfsAlternatingPath(workMatrix, assignment, i, visited, rowCovered, colCovered, size);
            }
        }
        
        // Apply König's theorem: cover unvisited rows and visited columns
        for (int i = 0; i < size; i++)
        {
            if (!visited[i]) rowCovered[i] = true;
        }
        
        for (int j = 0; j < size; j++)
        {
            for (int i = 0; i < size; i++)
            {
                if (visited[i] && workMatrix[i, j] == 0)
                {
                    colCovered[j] = true;
                    break;
                }
            }
        }
        
        // Count covering lines
        int lineCount = 0;
        for (int i = 0; i < size; i++) if (rowCovered[i]) lineCount++;
        for (int j = 0; j < size; j++) if (colCovered[j]) lineCount++;
        
        Console.WriteLine($"Number of covering lines: {lineCount}");
        
        // Step 4: Create additional zeros
        Console.WriteLine("Step 4: Create additional zeros");
        
        // Find minimum uncovered element
        int minUncovered = int.MaxValue;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (!rowCovered[i] && !colCovered[j] && workMatrix[i, j] < minUncovered)
                {
                    minUncovered = workMatrix[i, j];
                }
            }
        }
        
        Console.WriteLine($"Smallest uncovered element: {minUncovered}");

        // Adjust matrix
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (!rowCovered[i] && !colCovered[j])
                {
                    workMatrix[i, j] -= minUncovered;
                }
                else if (rowCovered[i] && colCovered[j])
                {
                    workMatrix[i, j] += minUncovered;
                }
            }
        }
        
        Console.WriteLine("Matrix after adjustment:");
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Console.Write(workMatrix[i, j] + "\t");
            }
            Console.WriteLine();
        }
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

// Add this helper method outside the switch statement (after the for loop)
static void DfsAlternatingPath(int[,] matrix, int[] assignment, int row, bool[] visited, 
                              bool[] rowCovered, bool[] colCovered, int size)
{
    if (visited[row]) return;
    visited[row] = true;
    
    for (int j = 0; j < size; j++)
    {
        if (matrix[row, j] == 0)
        {
            // Find which row is matched to column j
            int matchedRow = -1;
            for (int i = 0; i < size; i++)
            {
                if (assignment[i] == j)
                {
                    matchedRow = i;
                    break;
                }
            }
            
            if (matchedRow != -1)
            {
                DfsAlternatingPath(matrix, assignment, matchedRow, visited, rowCovered, colCovered, size);
            }
        }
    }
}