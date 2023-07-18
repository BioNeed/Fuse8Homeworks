using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<StringInternBenchmark>();
Console.WriteLine("Done benchmarking");
Console.ReadLine();

/// <summary>
/// Подробные результаты в файле BenchmarkResults/Results.txt
/// 
/// Результаты для слов:
/// "Эйри" - слово из начала словаря через StringBuilder
/// WordIsExistsIntern("Эйри") - 1.022 us
/// WordIsExists("Эйри") - 1.174 us
/// 
/// "Эйде" - слово из начала словаря константное
/// WordIsExistsIntern("Эйде") - ????? us
/// WordIsExists("Эйде") - ????? us
/// 
/// "потеплеет" - слово из середины словаря через StringBuilder
/// WordIsExistsIntern("потеплеет") - 637.698 us
/// WordIsExists("потеплеет") - 794.809 us
/// 
/// "подернется" - слово из середины словаря константное
/// WordIsExistsIntern("подернется") - 642.639 us
/// WordIsExists("подернется") - 868.846 us
/// 
/// "ёмче" - слово из конца словаря через StringBuilder
/// WordIsExistsIntern("ёмче") - 1,201.952 us
/// WordIsExists("ёмче") - 1,671.600 us
/// 
/// "ёрш" - слово из конца словаря константное
/// WordIsExistsIntern("ёрш") - 1,251.781 us
/// WordIsExists("ёрш") - 1,612.884 us
/// 
/// "лопнутый" - слово НЕ из словаря через StringBuilder
/// WordIsExistsIntern("лопнутый") - 1,233.824 us
/// WordIsExists("лопнутый") - 1,639.830 us
/// 
/// "шуфлядка" - слово НЕ из словаря константное
/// WordIsExistsIntern("шуфлядка") - 1,174.752 us
/// WordIsExists("шуфлядка") - 1,716.977 us
/// 
/// </summary>
[MemoryDiagnoser(displayGenColumns: true)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class StringInternBenchmark
{
    private readonly List<string> _words = new();

    public StringInternBenchmark()
    {
       foreach (var word in File.ReadLines(@".\SpellingDictionaries\ru_RU.dic"))
           _words.Add(string.Intern(word));
    }

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(SampleData))]
    public bool WordIsExists(string word)
        => _words.Any(item => word.Equals(item, StringComparison.Ordinal));

    [Benchmark]
    [ArgumentsSource(nameof(SampleData))]
    public bool WordIsExistsIntern(string word)
    {
        var internedWord = string.Intern(word);
        return _words.Any(item => ReferenceEquals(internedWord, item));
    }

    public IEnumerable<string> SampleData()
    {
        // Слова из начала
        yield return new StringBuilder().Append("Эй").Append("ри").ToString();
        yield return "Эйде";

        // Слова из середины
        yield return new StringBuilder().Append("потеп").Append("леет").ToString();
        yield return "подернется";

        // Слова из конца
        yield return new StringBuilder().Append("ём").Append("че").ToString();
        yield return "ёрш";

        // Этих слов нет в словаре
        yield return new StringBuilder().Append("лопну").Append("тый").ToString();
        yield return "шуфлядка";
    }
}