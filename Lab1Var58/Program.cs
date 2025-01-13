using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1Var58
{
    internal class Program
    {
        // 58 mod 20 == 18
        // Вариант 18

        static void Main()
        {
            //  получаем список строк таблицы
            var Rows = GetFileData();

            //  шапка таблицы
            Console.WriteLine($"№\tX\tY\tРезультат");

            //  построчный вывод таблицы
            foreach (string[] row in Rows.Item1)
                Console.WriteLine($"{row[0]}\t{row[1]}\t{row[2]}\t{row[3]}");

            if (Rows.Item2 != "")
            { 
                Console.WriteLine("\nВ ходе выполнения возникли следующие ошибки:");
                Console.WriteLine(Rows.Item2);
            }
        }

        static (List<string[]>, string) GetFileData()
        {
            //  return RowsList - список строк таблицы
            //  return ErrorBuilder - строка ошибок выполнения

            //  путь к файлу
            const string file_name = "..\\..\\Coordinates.txt";

            //  список массивов с координатами из файла
            var RowsList = new List<string[]> { };

            //  переменная под разделитель, если разделитель вдруг изменится
            char splitter = ' ';

            //  билдер ошибок
            StringBuilder ErrorBuilder = new StringBuilder();

            //  счетчик выстрелов
            //  также используем для отслеживания строки с ошибкой в файле
            int ShotCounter = 1;

            try
            {
                //  через менеджер открываем файл на чтение
                using (StreamReader sr = new StreamReader(file_name))
                {
                    string line;

                    //  читаем строки файла
                    while ((line = sr.ReadLine()) != null)
                    {
                        //  получаем массив цифр str[]
                        string[] line_split;
                        line_split = line.Split(splitter);

                        //  конвертируем координаты в int
                        int coordX = Convert.ToInt32(line_split[0]);
                        int coordY = Convert.ToInt32(line_split[1]);

                        //  создаем строку таблицы
                        string[] line_xy = {
                            ShotCounter.ToString(),
                            coordX.ToString(),
                            coordY.ToString(),

                            //  зовем функцию попадания
                            ShotProcessing(coordX, coordY)
                        };

                        //  добавляем массив в список всех координат
                        RowsList.Add(line_xy);

                        //  обновляем счетчик
                        ShotCounter++;
                    }
                }
            }
            //  обрабатываем выкидыши
            catch (Exception e) 
            {
                ErrorBuilder.AppendLine($"Строка {ShotCounter}: {e.Message}");
            }

            return (RowsList, ErrorBuilder.ToString());
        }

        static string ShotProcessing(int coordX, int coordY)
        {
            //  проверка попадания

            //  param:
            //  int coordX - координата X
            //  int coordY - координата Y
            //  return:
            //  str - результат попадания

            double radius = 10;

            //  формула попадания в вехний круг
            var distance_from_center_upper = Math.Sqrt(Math.Pow(coordX - radius, 2) + Math.Pow(coordY - radius, 2));

            //  формула попадания в нижиний круг
            var distance_from_center_lower = Math.Sqrt(Math.Pow(coordX + radius, 2) + Math.Pow(coordY + radius, 2));

            //  проверка на попадание в круг в I четверти
            //  если x и y пололжительные
            //  и точка ниже y = x
            //  и радиус больше либо равен дистанции до центра
            if (coordX > 0 && coordY > 0)
                if (distance_from_center_upper <= radius && coordY - coordX <= 0)
                    return "Попал";

            //  проверка на попадание в круг в III четверти
            //  если x и y отрицательные
            //  и точка выше y = x
            //  и радиус больше либо равен дистанции до центра
            if (coordX < 0 && coordY < 0)
                if (distance_from_center_lower <= radius && coordY - coordX >= 0)
                    return "Попал";

            //  если раньше не вернули попадание - значит не судьба
            return "Мимо";
        }
    }
}
