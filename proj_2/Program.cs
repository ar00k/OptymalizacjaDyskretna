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
                matrix[i, j] = rand.Next(100, 1000);
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

    // Main algorithm loop with iteration limit
    int iteration = 0;
    int maxIterations = size * size; // Prevent infinite loops
    
    while (iteration < maxIterations)
    {
        iteration++;
        Console.WriteLine($"\nIteration {iteration}:");
        
        // Find maximum matching using proper bipartite matching
        int[] rowMatch = new int[size];
        int[] colMatch = new int[size];
        for (int i = 0; i < size; i++) { rowMatch[i] = -1; colMatch[i] = -1; }
        
        int matchCount = 0;
        
        // Use augmenting path algorithm for maximum matching
        for (int i = 0; i < size; i++)
        {
            bool[] visited = new bool[size];
            if (FindAugmentingPath(workMatrix, i, visited, rowMatch, colMatch, size))
            {
                matchCount++;
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
                Console.WriteLine($"Worker {i+1} -> Job {rowMatch[i]+1}, Cost: {matrix[i, rowMatch[i]]}");
                totalCost += matrix[i, rowMatch[i]];
            }
            Console.WriteLine($"Total assignment cost: {totalCost}");
            
            // Display assignment as 0,1 matrix
            Console.WriteLine("\nAssignment matrix (1 = assigned, 0 = not assigned):");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (rowMatch[i] == j)
                    {
                        Console.Write("1\t");
                    }
                    else
                    {
                        Console.Write("0\t");
                    }
                }
                Console.WriteLine();
            }
            break;
        }
        
        // Find vertex cover using König's theorem
        bool[] rowCovered = new bool[size];
        bool[] colCovered = new bool[size];
        bool[] rowVisited = new bool[size];
        bool[] colVisited = new bool[size];
        
        // Start DFS from unmatched rows
        for (int i = 0; i < size; i++)
        {
            if (rowMatch[i] == -1)
            {
                DfsKonig(workMatrix, i, rowMatch, colMatch, rowVisited, colVisited, size);
            }
        }
        
        // Apply König's theorem: cover unvisited rows and visited columns
        for (int i = 0; i < size; i++)
        {
            if (!rowVisited[i]) rowCovered[i] = true;
        }
        for (int j = 0; j < size; j++)
        {
            if (colVisited[j]) colCovered[j] = true;
        }
        
        // Count covering lines
        int lineCount = 0;
        for (int i = 0; i < size; i++) if (rowCovered[i]) lineCount++;
        for (int j = 0; j < size; j++) if (colCovered[j]) lineCount++;
        
        Console.WriteLine($"Number of covering lines: {lineCount}");
        
        // If we have exactly 'matchCount' lines, we need to create more zeros
        if (lineCount == matchCount)
        {
            // Find minimum uncovered element
            int minUncovered = int.MaxValue;
            bool foundUncovered = false;
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (!rowCovered[i] && !colCovered[j])
                    {
                        foundUncovered = true;
                        if (workMatrix[i, j] < minUncovered)
                        {
                            minUncovered = workMatrix[i, j];
                        }
                    }
                }
            }
            
            if (!foundUncovered)
            {
                Console.WriteLine("Error: No uncovered elements found but matching is not complete");
                break;
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
    }
    
    if (iteration >= maxIterations)
    {
        Console.WriteLine($"Algorithm stopped after {maxIterations} iterations to prevent infinite loop.");
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

// Helper method for finding augmenting paths in bipartite matching
static bool FindAugmentingPath(int[,] matrix, int row, bool[] visited, int[] rowMatch, int[] colMatch, int size)
{
    for (int col = 0; col < size; col++)
    {
        if (matrix[row, col] == 0 && !visited[col])
        {
            visited[col] = true;
            
            if (colMatch[col] == -1 || FindAugmentingPath(matrix, colMatch[col], visited, rowMatch, colMatch, size))
            {
                rowMatch[row] = col;
                colMatch[col] = row;
                return true;
            }
        }
    }
    return false;
}

// Helper method for König's theorem DFS
static void DfsKonig(int[,] matrix, int row, int[] rowMatch, int[] colMatch, bool[] rowVisited, bool[] colVisited, int size)
{
    if (rowVisited[row]) return;
    rowVisited[row] = true;
    
    for (int col = 0; col < size; col++)
    {
        if (matrix[row, col] == 0 && !colVisited[col])
        {
            colVisited[col] = true;
            if (colMatch[col] != -1)
            {
                DfsKonig(matrix, colMatch[col], rowMatch, colMatch, rowVisited, colVisited, size);
            }
        }
    }
}