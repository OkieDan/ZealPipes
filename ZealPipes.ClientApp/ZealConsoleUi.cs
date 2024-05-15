using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using ZealPipes.Common.Models;
using ZealPipes.Services;
using ZealPipes.Services.Helpers;
namespace ZealPipes.ClientApp
{
    public class ZealConsoleUi
    {
        private static Dictionary<int, Gauge> gauges = new Dictionary<int, Gauge>();
        class Gauge
        {
            public int Value { get; set; }
            public int MaxValue { get; set; }
            public int Length { get; set; }
            public int TopPosition { get; set; }
        }

        public static void DrawGauge(int id, int value, int maxValue, int length)
        {
            if (gauges.ContainsKey(id))
            {
                Gauge gauge = gauges[id];
                Console.SetCursorPosition(0, gauge.TopPosition);
                int filledLength = (int)Math.Round((double)value / maxValue * length);
                int emptyLength = length - filledLength;
                Console.Write("[");
                for (int i = 0; i < filledLength; i++)
                {
                    Console.Write("█"); // Filled block character
                }
                for (int i = 0; i < emptyLength; i++)
                {
                    Console.Write(" "); // Empty block character
                }
                Console.Write($"] {value}/{maxValue}");

                gauge.Value = value;
                gauge.MaxValue = maxValue;
                gauge.Length = length;
            }
            else
            {
                int topPosition = gauges.Count == 0 ? Console.CursorTop : gauges.Last().Value.TopPosition + 1;
                Gauge newGauge = new Gauge
                {
                    Value = value,
                    MaxValue = maxValue,
                    Length = length,
                    TopPosition = topPosition
                };

                int filledLength = (int)Math.Round((double)value / maxValue * length);
                int emptyLength = length - filledLength;
                Console.SetCursorPosition(0, topPosition);
                Console.Write("[");
                for (int i = 0; i < filledLength; i++)
                {
                    Console.Write("█"); // Filled block character
                }
                for (int i = 0; i < emptyLength; i++)
                {
                    Console.Write(" "); // Empty block character
                }
                Console.Write($"] {value}/{maxValue}");

                gauges.Add(id, newGauge);
            }
        }
    }

}
