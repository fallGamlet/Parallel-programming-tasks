// See https://aka.ms/new-console-template for more information
using System.Text;

Console.WriteLine("Linq Example Application!");

// create list with filled numbers from 0 to 20
var list = Enumerable.Range(0, 20).ToList();

printListWithForLoop(list);
printListWithLinqFor(list);
filterMultyPrint(list);
filterMultyPrintOptimized(list);
filterMultyPrintLinq(list);
linqMethodsExample(list);

// print into console all items of list with usage FOR loop
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

// print into console all items of list with usage Linq.ForEach
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

// example of filtering source list and transform each of filtered items with usage FOR loops
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

// example of filtering source list and transform each of filtered items with usage one FOR loop
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

// example of filtering source list and transform each of filtered items with usage Linq methods
void filterMultyPrintLinq(List<int> list) 
{
    var resultList = list
        // filter items of list
        .Where(item => item % 2 == 0)
        // convert each item of list
        .Select(item => 2 * item)
        .ToList();

    printListWithLinqFor(resultList);
}

void linqMethodsExample(List<int> list) {
    // check for some of list bigger then 5
    var hasNumberGreater5 = list.Any(item => item > 5);
    // check for some of list bigger then 60
    var hasNumberGreater60 = list.Any(item => item > 60);
    // check for all of list lower then 40
    var allNumberLower40 = list.All(item => item < 40);
    // check for all of list lower then 10
    var allNumberLower10 = list.All(item => item < 10);
    // get first item of list equals to 5 or throw exception
    var number5 = list.First(item => item == 5);
    // get first item of list equals to 45 or get default value (-1)
    var number45 = list.FirstOrDefault(item => item == 45, -1);

    // create secontList with translate each items of list into string by template "<{item}>"
    var secondList = list.Select(item => $"<{item}>");
    // create thirdList with translate each items of list into negative numbers
    var thirdList = list.Select(item => -item);
    
    // zip list with secondList and thirdList
    var resultList = list.Zip(secondList, thirdList)
        // translate each zipped item into TrippleFelds class
        .Select(item => 
            new TrippleFelds<int, string, int>() {
                Field1 = item.First,
                Field2 = item.Second,
                Field3 = item.Third
            }
        )
        // filter items of list
        .Where(item => item.Field1 % 3 != 0)
        // sort items of list by Field3
        .OrderBy(item => item.Field3);

    resultList
        // group items of resultList
        .GroupBy(item => item.Field1 % 4)
        // sort items of groups by key
        .OrderBy(item => item.Key)
        .ToList()
        .ForEach(group => 
        {
            Console.WriteLine($"Group");
            printListWithLinqFor(group.ToList());
        });
}

// Simple class for contain 3 fields
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