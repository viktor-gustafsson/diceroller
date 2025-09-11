namespace DiscordBot.ResponseMessages;

public static class ErrorMessages
{
    public static string GetInvalidRollCommandMessage()
        => "Invalid roll command. Please reference /help for examples and guidance.";

    public static string GetFallbackErrorMessage()
        => "I'm sorry, I don't know what to do.";

}