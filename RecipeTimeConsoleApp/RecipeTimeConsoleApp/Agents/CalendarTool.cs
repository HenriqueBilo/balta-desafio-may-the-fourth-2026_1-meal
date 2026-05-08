using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RecipeTimeConsoleApp.Agents;

public static class CalendarTool
{
    [Description("Retorna os slots de tempo livre na agenda do usuário para hoje.")]
    public static string GetFreeTimeSlots()
    {
        var json = File.ReadAllText("Data/agenda.json");
        var agenda = JsonSerializer.Deserialize<Agenda>(json);

        var slots = CalculateFreeSlots(agenda.Events);

        if (slots.Count == 0)
            return "Não há tempo livre na agenda hoje.";

        var descriptions = slots.Select(s =>
            $"{s.DurationMinutes} minutos livres entre {s.Start:hh\\:mm} e {s.End:hh\\:mm}");

        return $"Slots livres hoje: {string.Join("; ", descriptions)}.";
    }

    private static List<FreeSlot> CalculateFreeSlots(List<CalendarEvent> events)
    {
        var workStart = TimeSpan.FromHours(8);
        var workEnd = TimeSpan.FromHours(18);
        var sorted = events.OrderBy(e => e.StartTime).ToList();
        var slots = new List<FreeSlot>();
        var current = workStart;

        foreach (var ev in sorted)
        {
            if (ev.StartTime > current)
                slots.Add(new FreeSlot(current, ev.StartTime));
            if (ev.EndTime > current)
                current = ev.EndTime;
        }

        if (current < workEnd)
            slots.Add(new FreeSlot(current, workEnd));

        return slots.Where(s => s.DurationMinutes >= 15).ToList();
    }

    private record FreeSlot(TimeSpan Start, TimeSpan End)
    {
        public int DurationMinutes => (int)(End - Start).TotalMinutes;
    }
}

public class Agenda
{
    [JsonPropertyName("events")]
    public List<CalendarEvent> Events { get; set; } = [];
}

public class CalendarEvent
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("start")]
    public string Start { get; set; } = string.Empty;

    [JsonPropertyName("end")]
    public string End { get; set; } = string.Empty;

    public TimeSpan StartTime => TimeSpan.Parse(Start);
    public TimeSpan EndTime => TimeSpan.Parse(End);
}
