namespace Models;

public class Game
{
    public enum Mode
    {
        SinglePlayer,
        Multiplayer,
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Style { get; set; }

    public DateOnly DateRelease { get; set; }

    public Mode GameplayMode { get; set; }

    public int? NumberSold { get; set; }

    public Studio? GameStudio { get; set; }

    public int? StudioId { get; set; }
}