using ChatGPT.Net;

namespace DotnetWorkshop;

/// <summary>
/// Class that executes the ChatGpt conversation system.
/// </summary>
public static class PromptRunner
{
    /// <summary>
    /// Executes the ChatGpt conversation system.
    /// </summary>
    /// <param name="prompt">The prompt message to pass to the ChatGpt conversation system.</param>
    /// <param name="question">The question to ask the ChatGpt conversation system.</param>
    /// <param name="apiKey">The API key to authenticate the ChatGpt instance.</param>
    /// <returns>The response from the ChatGpt conversation system.</returns>
    public static async Task<string> Run(string prompt, string question, string apiKey)
    {
        var bot = new ChatGpt(apiKey);
        var conversationId = Guid.NewGuid().ToString();
        
        bot.SetConversationSystemMessage(conversationId, prompt);
        
        return await bot.Ask(question, conversationId);
    }
}