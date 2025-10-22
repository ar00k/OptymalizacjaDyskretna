// Algorytm plecakowy z powtórzeniami przedmiotów
#nullable disable
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

void InputItems(int[,] table, int size)
{
    for (int i = 0; i < size; i++)
    {
        Console.WriteLine($"Podaj wagę przedmiotu {i + 1}:");
        table[0, i] = int.Parse(Console.ReadLine());
        Console.WriteLine($"Podaj wartość przedmiotu {i + 1}:");
        table[1, i] = int.Parse(Console.ReadLine());
    }
}
void DisplayItems(int[,] table, int size)
{
    Console.WriteLine("Wagi przedmiotów:");
    for (int i = 0; i < size; i++)
    {
        Console.WriteLine($"Przedmiot {i + 1}: Waga = {table[0, i]}, Wartość = {table[1, i]}");
    }
}

void InputRandomItems(int[,] table, int size)
{
    Random rand = new Random();
    for (int i = 0; i < size; i++)
    {
        table[0, i] = rand.Next(1, 20); // Waga od 1 do 20
        table[1, i] = rand.Next(10, 100); // Wartość od 10 do 100
    }
    Console.WriteLine("Losowe przedmioty zostały wygenerowane.");
}

void SaveToFile(int[,] table, int size)
{
    using (StreamWriter writer = new StreamWriter("items.txt"))
    {
        writer.WriteLine(size);
        for (int i = 0; i < size; i++)
        {
            writer.WriteLine($"{table[0, i]},{table[1, i]}");
        }
    }
    Console.WriteLine("Przedmioty zostały zapisane do pliku items.txt.");
}

void LoadFromFile(int[,] table, int size)
{
    using (StreamReader reader = new StreamReader("items.txt"))
    {
        int fileSize = int.Parse(reader.ReadLine());
        for (int i = 0; i < size; i++)
        {
            string line = reader.ReadLine();
            string[] parts = line.Split(',');
            table[0, i] = int.Parse(parts[0]);
            table[1, i] = int.Parse(parts[1]);
        }
    }
    Console.WriteLine("Przedmioty zostały wczytane z pliku items.txt.");
}

void SolveGreedy(int[,] table, int size, int capacity)
{
    float[] ratios = new float[size];
    for (int i = 0; i < size; i++)
    {
        ratios[i] = (float)table[1, i] / table[0, i];
    }
    Console.WriteLine("Współczynniki wartości do wagi przedmiotów:");
    for (int i = 0; i < size; i++)
    {
        Console.WriteLine($"Przedmiot {i + 1}: Współczynnik = {ratios[i]}");
    }
    int RemainingCapacity = capacity;
    int TotalValue = 0;
    int bestIndex = ratios.ToList().IndexOf(ratios.Max());
    while (RemainingCapacity > bestIndex)
    {
        if (RemainingCapacity >= table[0, bestIndex])
        {
            RemainingCapacity -= table[0, bestIndex];
            TotalValue += table[1, bestIndex];
            Console.WriteLine(
                $"Dodano przedmiot {bestIndex + 1} do plecaka. Pozostała pojemność: {RemainingCapacity}, a łączna wartość: {TotalValue}"
            );
        }
        else if (RemainingCapacity < table[0, bestIndex])
        {
            int newBestIndex = -1;
            float newBestRatio = 0;
            for (int i = 0; i < size; i++)
            {
                if (table[0, i] <= RemainingCapacity && ratios[i] > newBestRatio)
                {
                    newBestRatio = ratios[i];
                    newBestIndex = i;
                }
            }
            if (newBestIndex != -1)
            {
                bestIndex = newBestIndex;
            }
            else
            {
                break;
            }
        }
    }
    Console.WriteLine($"Całkowita wartość przedmiotów w plecaku: {TotalValue}");
}

void solveOptimal(int[,] table, int size, int capacity)
{
    int[] dp = new int[capacity + 1];
    int[] choice = Enumerable.Repeat(-1, capacity + 1).ToArray();

    for (int i = 0; i < size; i++)
    {
        for (int w = table[0, i]; w <= capacity; w++)
        {
            int candidate = dp[w - table[0, i]] + table[1, i];
            if (candidate > dp[w])
            {
                Console.WriteLine(
                    $"Dla pojemności {w}: ulepszenie przez przedmiot {i + 1} (waga {table[0, i]}, wartość {table[1, i]}). Poprzednio {dp[w]}, teraz {candidate}."
                );
                dp[w] = candidate;
                choice[w] = i;
            }
        }
    }

    // Odtworzenie wyboru przedmiotów (ilości każdego przedmiotu)
    int remaining = capacity;
    int[] counts = new int[size];
    Console.WriteLine("Odtwarzanie wyboru przedmiotów:");
    while (remaining > 0 && choice[remaining] != -1)
    {
        int idx = choice[remaining];
        counts[idx]++;
        remaining -= table[0, idx];
        Console.WriteLine(
            $"Wybrano przedmiot {idx + 1} (waga {table[0, idx]}, wartość {table[1, idx]}). Pozostała pojemność: {remaining}"
        );
    }

    Console.WriteLine("Podsumowanie wybranych przedmiotów:");
    for (int i = 0; i < size; i++)
    {
        if (counts[i] > 0)
            Console.WriteLine(
                $"Przedmiot {i + 1}: ilość = {counts[i]}, waga = {table[0, i]}, wartość = {table[1, i]}"
            );
    }

    Console.WriteLine(
        $"Maksymalna wartość, którą można uzyskać w plecaku o pojemności {capacity}, to: {dp[capacity]}"
    );
}

Console.WriteLine("Podaj liczbę przedmiotów:");
int size = int.Parse(Console.ReadLine());
int[,] tableOfItems = new int[2, size];

for (; ; )
{
    int Menu()
    {
        Console.WriteLine("Wybierz opcję:");
        Console.WriteLine("1. Wprowadź wagi i wartości przedmiotów");
        Console.WriteLine("2. Wprowadź losowe przedmioty");
        Console.WriteLine("3. Wyświetl przedmioty");
        Console.WriteLine("4. Zapisz do pliku");
        Console.WriteLine("5. Odczytaj z pliku");
        Console.WriteLine("6. Rozwiąż problem plecakowy zachłannie");
        Console.WriteLine("7. Rozwiąż problem plecakowy optymalnie");
        Console.WriteLine("0. Zakończ program");
        return int.Parse(Console.ReadLine());
    }
    switch (Menu())
    {
        case 1:
            InputItems(tableOfItems, size);
            break;

        case 2:
            InputRandomItems(tableOfItems, size);
            break;
        case 3:
            DisplayItems(tableOfItems, size);
            break;

        case 4:
            SaveToFile(tableOfItems, size);
            break;

        case 5:
            LoadFromFile(tableOfItems, size);
            break;

        case 6:
            Console.WriteLine("Podaj pojemność plecaka:");
            int capacity = int.Parse(Console.ReadLine());

            Console.WriteLine("Rozwiązuje problem plecakowy zachłannie...");
            SolveGreedy(tableOfItems, size, capacity);

            break;

        case 7:
            Console.WriteLine("Podaj pojemność plecaka:");
            int cap = int.Parse(Console.ReadLine());

            Console.WriteLine("Rozwiązuje problem plecakowy optymalnie...");
            solveOptimal(tableOfItems, size, cap);

            break;
        case 0:
            Console.WriteLine("Koniec programu.");
            return;
        default:
            Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
            break;
    }
}
