// See https://aka.ms/new-console-template for more information
using System.Text;

Console.WriteLine("Linq Example Application!");


var list = new List<int>();

for (int i = 0; i < 20; i++)
{
    list.Add(i);
}

printListWithForLoop(list);
printListWithLinqFor(list);
filterMultyPrint(list);
filterMultyPrintOptimized(list);
filterMultyPrintLinq(list);

linqMethodsExample(list);

void printListWithForLoop<T>(List<T> list) 
{
    var builder = new StringBuilder();
    for (int i = 0; i < list.Count; i++)
    {
        builder.Append(list[i]);
        builder.Append("  ");
    }
    Console.WriteLine($"ForLoop \nlist => {builder.ToString()}");
}

void printListWithLinqFor<T>(List<T> list) 
{
    var builder = new StringBuilder();
    list.ForEach(item => 
    {
        builder.Append(item);
        builder.Append("  ");
    });
    Console.WriteLine($"Linq.ForEach \nlist => {builder.ToString()}");
}

void filterMultyPrint(List<int> list) 
{
    var filteredList = new List<int>();
    foreach (var item in list)
    {
        if (item % 2 == 0)
            filteredList.Add(item);
    }

    var multipleList = new List<int>();
    foreach (var item in filteredList)
    {
        multipleList.Add(2 * item);
    }

    printListWithForLoop(multipleList);
}

void filterMultyPrintOptimized(List<int> list) 
{
    var resultList = new List<int>();
    foreach (var item in list)
    {
        if (item % 2 != 0) 
            continue;

        resultList.Add(2 * item);
    }

    printListWithForLoop(resultList);
}

void filterMultyPrintLinq(List<int> list) 
{
    var resultList = list
        .Where(item => item % 2 == 0)
        .Select(item => 2 * item)
        .ToList();

    printListWithLinqFor(resultList);
}

void linqMethodsExample(List<int> list) {

    var hasNumberGreater5 = list.Any(item => item > 5);
    var hasNumberGreater60 = list.Any(item => item > 60);
    var allNumberLower40 = list.All(item => item < 40);
    var allNumberLower10 = list.All(item => item < 10);
    var number5 = list.First(item => item == 5);
    var number45 = list.FirstOrDefault(item => item == 45, -1);

    var secondList = list.Select(item => $"<{item}>");
    var thirdList = list.Select(item => -item);
    
    var resultList = list.Zip(secondList, thirdList)
        .Select(item => 
        {
            return new TrippleFelds<int, string, int>() {
                Field1 = item.First,
                Field2 = item.Second,
                Field3 = item.Third
            };
        })
        .Where(item => item.Field1 % 3 != 0)
        .OrderBy(item => item.Field3);

    resultList
        .GroupBy(item => item.Field1 % 4)
        .OrderBy(item => item.Key)
        .ToList()
        .ForEach(group => 
        {
            Console.WriteLine($"Group");
            printListWithLinqFor(group.ToList());
        });
}

class TrippleFelds<T1, T2, T3>
{
    public T1 Field1 { get; set; }
    public T2 Field2 { get; set; }
    public T3 Field3 { get; set; }

    public override string ToString()
    {
        return $"TrippleFelds<{Field1} {Field2} {Field3}>";
    }
}