using BenchmarkDotNet.Running;

namespace AccountProcessorBenchmarks
{
    /// <summary>
    /// Результаты бенчмарка в файле BenchmarkResults/Results.txt в этом проекте
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<AccountProcessor>();
            Console.WriteLine("Done benchmarking AccountProcessor");
            Console.ReadLine();
        }
    }
}