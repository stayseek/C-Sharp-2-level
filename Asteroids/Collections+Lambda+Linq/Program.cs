using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections_Lambda_Linq
{
    class Program
    {
        private static Dictionary<T,int> CountElements<T> (List<T> list)
        {
            Dictionary<T, int> found = new Dictionary<T, int>();
            foreach (T val in list)
            {
                if (found.ContainsKey(val))
                {
                    found[val] ++;
                }
                else
                {
                    found.Add(val, 1);
                }
            }
            return found;
        }

        //private static Dictionary<int,int> CountElements (List<int> list)
        //{
        //    Dictionary<int, int> found = new Dictionary<int, int>();
        //    foreach (int val in list)
        //    {
        //        if (found.ContainsKey(val))
        //        {
        //            found[val] ++;
        //        }
        //        else
        //        {
        //            found.Add(val,1);
        //        }
        //    }
        //    return found;
        //}

        static void Main(string[] args)
        {
            Console.WriteLine("Подсчёт количества вхождений элементов в List<int>.");
            List<int> intList = new List<int>(){0,3,2,7,3,6,12,3,24,6,8};
            Dictionary<int, int> intDict = CountElements(intList);
            foreach (KeyValuePair<int,int> pair in intDict)
            {
                Console.WriteLine($"Element: {pair.Key} - Count: {pair.Value}");
            }
            Console.ReadKey();

            Console.WriteLine("Подсчёт количества вхождений элементов в List<T> (на примере string).");
            List<string> stringList = new List<string>() { "a", "ada", "adda", "a", "dda", "dada", "dda" };
            Dictionary<string, int> stringDict = CountElements(stringList);
            foreach (KeyValuePair<string, int> pair in stringDict)
            {
                Console.WriteLine($"Element: {pair.Key} - Count: {pair.Value}");
            }
            Console.ReadKey();



            Console.WriteLine("Подсчёт количества вхождений элементов с помощью linq");
            List<string> linqList = new List<string>() { "a", "ada", "adda", "a", "dda", "dada", "dda"};

            var elements = linqList
                            .Select(str => new {                                //создаём анонимный тип
                                Name = str,                                     //где в поле Name наше значение,
                                Count = linqList.Count(s => s == str)           //а в поле Count количество вхождений нашего значения в List
                            }) 
                            .Distinct()                                         //убираем повторы
                            .ToDictionary(obj => obj.Name, obj => obj.Count);   //создаём словарь значение - количество совпадений.

            foreach (var e in elements)
            {
                Console.WriteLine($"Element: {e.Key} - Count: {e.Value}");
            }
            Console.ReadKey();

            // Сортировка словаря.
            Dictionary<string, int> dict = new Dictionary<string, int>()
            {
                {"four",4 },
                {"two",2 },
                {"one",1 },
                {"three",3 },
            };

            Console.WriteLine("Сортировка словаря с использованием анономного метода.");
            var d = dict.OrderBy(delegate (KeyValuePair<string, int> pair) { return pair.Value; });
            foreach (var pair in d)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }
            Console.WriteLine("Сортировка словаря с использованием лямбда-выражением.");
            var d1 = dict.OrderBy(pair => pair.Value);
            foreach (var pair in d1)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }
            Console.WriteLine("Сортировка словаря с использованием обобщённого делегата.");
            Func<KeyValuePair<string, int>, int> a = pair => pair.Value;
            var d2 = dict.OrderBy(a);
            foreach (var pair in d2)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }
            Console.ReadKey();

        }
    }
}
