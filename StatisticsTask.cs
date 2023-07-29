using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
    public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
    {
        List<double> result = new List<double>();
        var c = visits.OrderBy(v => v.DateTime).ToList();
        for (int i = 0; i < c.Count; i++)
        {
            if (c[i].SlideType == slideType)
            {
                for (int j = i + 1; j < c.Count; j++)
                {
                    if (c[i].UserId == c[j].UserId)
                    {
                        var curr = c[j].DateTime.Subtract(c[i].DateTime).TotalMinutes;
                        if (curr >= 1 && curr <= 120) result.Add(curr);
                        break;
                    }
                }
            }
        }
        result.Sort();
        if (result.Count == 0) return 0;
        return ExtensionsTask.Median(result);
    }
}