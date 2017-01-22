
/// <summary>
/// Game state
/// </summary>
public enum GameState
{
    NONE = -1,
    Menu,
    Intro,
    Play,
    Outro,
    Results,
}

public enum ControllerSide
{
    NONE = -1,
    Left,
    Right,
}

public enum ButtonStateEvent
{
    Idle = -2,
    Release,
    NONE = 0,
    Press,
    Hold,   
}

/// <summary>
/// events tracked for the player
/// </summary>
public enum TrackingEvent
{
    NONE = -1,
    LastManStanding,
}

public enum RumbleEvent
{
    Death,
    ForceHit,
    Win, 
    Thump,
}