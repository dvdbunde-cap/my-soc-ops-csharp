namespace SocOps.Data;

public static class Questions
{
    public const string FREE_SPACE = "FREE SPACE";

    public static readonly List<string> QuestionsList = new()
    {
        // Chaotic Gimmes (easy but unexpected)
        "got sunscreen in their eyes at the beach",
        "has eaten an entire meal in a moving vehicle",
        "wore mismatched shoes while traveling",
        "has taken a selfie with a weird statue",
        "packed way too many shoes",
        "has fallen asleep in an airport",
        "bought a souvenir they immediately regretted",
        "has gotten lost in a parking lot",
        "ate fast food in a fancy location",
        "has worn the same outfit for 2+ days",
        "took a photo of something boring",
        "has used a hotel towel as a blanket",

        // Medium Chaos
        "got lost in a foreign city with no data",
        "has accidentally waved at someone who wasn't waving at them",
        "ordered food without knowing what it was",
        "has been chased by a bird or animal",
        "fell asleep on public transportation and missed their stop",
        "has accidentally packed something weird (flip-flops in winter)",
        "got sunburned in an unexpected place",
        "has made friends with a stranger in a bathroom line",

        // Bold Chaos
        "has traveled with a broken suitcase",
        "got food poisoning while on vacation",
        "has been on TV in another country",
        "fell asleep in a weird position on a plane",

        // Wildcard Chaos (action-based)
        "do your best impression of a tired traveler",
        "show us the weirdest travel item you own",
        "tell us about your most chaotic travel story in 30 seconds"
    };
}
