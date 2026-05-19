using System.Text;

namespace Services.Tools;

public static class CyrillicToLatinConverter
{
    private static readonly Dictionary<string, string> Map = new()
    {
        {"А","A"},{"а","a"},{"Б","B"},{"б","b"},{"В","V"},{"в","v"},
        {"Г","G"},{"г","g"},{"Д","D"},{"д","d"},{"Е","E"},{"е","e"},
        {"Ё","Yo"},{"ё","yo"},{"Ж","J"},{"ж","j"},{"З","Z"},{"з","z"},
        {"И","I"},{"и","i"},{"Й","Y"},{"й","y"},{"К","K"},{"к","k"},
        {"Л","L"},{"л","l"},{"М","M"},{"м","m"},{"Н","N"},{"н","n"},
        {"О","O"},{"о","o"},{"П","P"},{"п","p"},{"Р","R"},{"р","r"},
        {"С","S"},{"с","s"},{"Т","T"},{"т","t"},{"У","U"},{"у","u"},
        {"Ф","F"},{"ф","f"},{"Х","X"},{"х","x"},
        {"Ц","Ts"},{"ц","ts"},{"Ч","Ch"},{"ч","ch"},
        {"Ш","Sh"},{"ш","sh"},{"Щ","Sh"},{"щ","sh"},
        {"Ъ","'"},{"ъ","'"},{"Ы","I"},{"ы","i"},
        {"Ь",""},{"ь",""},{"Э","E"},{"э","e"},
        {"Ю","Yu"},{"ю","yu"},{"Я","Ya"},{"я","ya"},
        // O'zbek maxsus harflari
        {"Ў","O'"},{"ў","o'"},{"Қ","Q"},{"қ","q"},
        {"Ғ","G'"},{"ғ","g'"},{"Ҳ","H"},{"ҳ","h"},
        {"Ҷ","J"},{"ҷ","j"},
    };

    public static string Convert(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        // 1. Kirill → Lotin
        var sb = new StringBuilder();
        foreach (char c in input)
            sb.Append(Map.TryGetValue(c.ToString(), out var val) ? val : c.ToString());

        // 2. Har bir so'zning birinchi harfini katta, qolganini kichik qilish
        var result = sb.ToString().ToLower();
        var words = result.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < words.Length; i++)
            words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);

        return string.Join(" ", words);
    }
}