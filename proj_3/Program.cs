// Algorytm plecakowy z powtórzeniami przedmiotów
#nullable disable
using System;
using System.ComponentModel.DataAnnotations;

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
            Console.WriteLine($"Dodano przedmiot {bestIndex + 1} do plecaka. Pozostała pojemność: {RemainingCapacity}, a łączna wartość: {TotalValue}");
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




Console.WriteLine("Podaj liczbę przedmiotów:");
int size = int.Parse(Console.ReadLine());
int[,] tableOfItems = new int[2, size];

for(;;)
{
    int Menu()
    {
        Console.WriteLine("Wybierz opcję:");
        Console.WriteLine("1. Wprowadź wagi i wartości przedmiotów");
        Console.WriteLine("2. Wyświetl przedmioty");
        Console.WriteLine("3. Rozwiąż problem plecakowy zachłannie");
        Console.WriteLine("0. Zakończ program");
        return int.Parse(Console.ReadLine());
    }
    switch(Menu())
    {
        case 1:
            InputItems(tableOfItems, size);
            break;
        case 2:
            DisplayItems(tableOfItems, size);
            break;
        case 3:
            Console.WriteLine("Podaj pojemność plecaka:");
            int capacity = int.Parse(Console.ReadLine());

            Console.WriteLine("Rozwiązuje problem plecakowy zachłannie...");
            SolveGreedy(tableOfItems, size, capacity);

            break;
        case 0:
            Console.WriteLine("Koniec programu.");
            return;
        default:
            Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
            break;
    }
    
}

