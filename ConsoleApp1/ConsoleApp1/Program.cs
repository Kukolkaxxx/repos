using System.Threading.Channels;

while (true)
{
    Console.Clear();
    Console.WriteLine("определения");
    Console.WriteLine("выберите что-то");
    Console.WriteLine("1-Переменная");
    Console.WriteLine("2-Типы данных");
    Console.WriteLine("3-Условия");
    Console.WriteLine("4-Модернизированное условие(конструкция)");
    Console.WriteLine("5-Циклы");
    Console.WriteLine("6-Коллекции");
    Console.WriteLine("7-Синхронные и асинхронные функции");
    Console.WriteLine("8-выход");
    switch (Console.ReadLine())
    {
        case "1":

            Console.Clear();
            Console.WriteLine("Переменная-именнованная область памяти в которой хранится n-ная информация");
            Console.WriteLine("нажмите что бы вернутся");
            Console.Read();
            break;

        case "2":
            Console.Clear();
            Console.WriteLine("Типы данных:");
            Console.WriteLine("1-Логический тип данных");
            Console.WriteLine("2-Строковый(словесный) тип данных");
            Console.WriteLine("3-Вещественный тип данных");
            Console.WriteLine("4-Целочисленный тип данных");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("Логический тип данных");
                    Console.WriteLine("Команада bool, имеет значения: true, folse");
                    Console.Read();
                    break;

                case "2":
                    Console.Clear();
                    Console.WriteLine("Строковый(словесный) тип данных");
                    Console.WriteLine("Команада string, сохраняет всю строку");
                    Console.Read();
                    break;

                case "3":
                    Console.Clear();
                    Console.WriteLine("Вещественный тип данных");
                    Console.WriteLine("Команада float, сохраняет 6 символов после запятой");
                    Console.WriteLine("Команада double, сохраняет 15 символов после запятой");
                    Console.Read();
                    break;

                case "4":
                    Console.Clear();
                    Console.WriteLine("Целочисленный тип данных");
                    Console.WriteLine("Команады byte(0;255), short(-32000;32000), int(-2.1млрд;,2.1млрд.), long(-10^18;10^18).");
                    Console.Read();
                    break;

                default:
                    Console.WriteLine("нет выбранного варианта");
                    Console.WriteLine("возврат к началному меню");
                    Console.Read();
                    break;

            }
            break;

        case "3":
            Console.Clear();
            Console.WriteLine("Условия: \nПозволяют выполнять разный код в зависимости от логических проверок\nКоманды:if; else");
            Console.Read();
            break;

        case "4":
            Console.Clear();
            Console.WriteLine("Модернизированное условие(конструкция)" +
                "\nКонструкция if;else позволяет выполнять блок кода только при соблюдении определённого условия." +
                "\nЕсли условие истинно, выполняется код внутри блока if, если ложно — код внутри блока else." +
                "\nКонструкция switch позволяет выполнять разные блоки кода на основе значения переменной или выражения." +
                "\nОператор переходит к коду под меткой с вариантом, который совпадает со значением." +
                "\nЕсли такого варианта нет — выполнится код под меткой default. ");
            Console.Read();
            break;

        case "5":
            Console.Clear();
            Console.WriteLine("Циклы бывают 4-х типов" +
                "\n1-Цикл с предусловием \n2-Цикл с постсловием \n3-Цикл с счётчиком \n4-Цикл перебора");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("Цикл с предусловием" +
                        "\n выполняется пока условие истенно");
                    Console.Read();
                    break;

                case "2":
                    Console.Clear();
                    Console.WriteLine("Цикл с постсловием" +
                        "\n");
                    Console.Read();
                    break;

                case "3":
                    Console.Clear();
                    Console.WriteLine("Цикл с счётчиком" +
                        "\n");
                    Console.Read();
                    break;

                case "4":
                    Console.Clear();
                    Console.WriteLine("Цикл перебора" +
                        "\n");
                    Console.Read();
                    break;

            }
            break;

        case "6":
            Console.Clear();
            Console.WriteLine("Коллекция- обобщённое понятие для структур данных которые хронят набор элементов" +
                "\nМассив- простейшая структура данных котрая представляет собой фиксированное кол-во элементов 1-го типа, \n" +
                "расположенных в пмяти друг за другом. \n=====" +
                "\nСписок- его можно представить как \"умный\", динамический массив. " +
                "\nКогда места не хватает список автоматически внутренний массив и котирует в неговсе элементы. Обозначается: List<T>");
            Console.Read();
            break;

        case "7":
            Console.Clear();
            Console.WriteLine("Синхронные и асинхронные функции\n");
            Console.Read();
            break;

        case "8":
            return;

            //default: Console.WriteLine();
    }
}