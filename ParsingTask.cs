// Вставьте сюда финальное содержимое файла ParsingTask.cs
// Вставьте сюда финальное содержимое файла ParsingTask.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
    /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
    /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {
        Dictionary<string, SlideType> types = new Dictionary<string, SlideType>();
        types.Add("theory", SlideType.Theory);
        types.Add("quiz", SlideType.Quiz);
        types.Add("exercise", SlideType.Exercise);
        return lines.Select(x => x.Split(';', '\n', '\r'))
                    .Where(c => {
                        try
                        {
                            new SlideRecord(int.Parse(c[0]), types[c[1]], c[2]);
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    })
                    .Select(v => new SlideRecord(int.Parse(v[0]), types[v[1]], v[2]))
                    .ToDictionary(c => c.SlideId, c => c);

    }

    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
    /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
    /// Такой словарь можно получить методом ParseSlideRecords</param>
    /// <returns>Список информации о посещениях</returns>
    /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
    {
        return lines.Skip(1).Select(x => {
            try
            {
                return x.Split(';', '\n', '\r');

            }
            catch (Exception e)
            {
                throw new FormatException("Wrong line" + " [" + x + "]");
            }
        })
                                .Select(g =>
                                {
                                    try
                                    {
                                        return new VisitRecord(int.Parse(g[0]), int.Parse(g[1]), Convert.ToDateTime(g[2] + " " + g[3]), slides[int.Parse(g[1])].SlideType);

                                    }
                                    catch (Exception e)
                                    {
                                        throw new FormatException("Wrong line" + " [" + string.Join(";", g) + "]");
                                    }
                                }

            );
    }
}