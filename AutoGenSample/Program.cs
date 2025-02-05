using System;
using System.Threading.Tasks;
using AutoGen;
using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using OpenAI;

class Program
{
    static async Task Main(string[] args)
    {
        // Get your OpenAI API key and model from environment variables
        var openAIKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        var model = Environment.GetEnvironmentVariable("OPENAI_MODEL");

        var openAIClient = new OpenAIClient(openAIKey);

        // Create a writer agent
        var writerAgent = new OpenAIChatAgent(
            name: "writer",
            systemMessage: "You are a writer that generates content based on user input.",
            chatClient: openAIClient.GetChatClient(model)
        )
        .RegisterMessageConnector()
        .RegisterPrintMessage(); // Register a hook to print messages nicely to the console

        // Create a critic agent
        var criticAgent = new OpenAIChatAgent(
            name: "critic",
            systemMessage: "You are a critic that provides feedback on the writer's content.",
            chatClient: openAIClient.GetChatClient(model)
        )
        .RegisterMessageConnector()
        .RegisterPrintMessage();

        // Create a reviewer agent
        var reviewerAgent = new OpenAIChatAgent(
            name: "reviewer",
            systemMessage: "You are a reviewer that finalizes the content after receiving feedback from the critic.",
            chatClient: openAIClient.GetChatClient(model)
        )
        .RegisterMessageConnector()
        .RegisterPrintMessage();

        // Automated conversation flow
        var initialMessage = "Hey writer, please generate a short story within max 20 sentences.";

        // Writer Agent generates content
        await writerAgent.InitiateChatAsync(
            receiver: criticAgent,
            message: initialMessage,
            maxRound: 10
        );

        // Critic Agent provides feedback
        var criticFeedback = "Please review the writer's story.";
        await criticAgent.InitiateChatAsync(
            receiver: writerAgent,
            message: criticFeedback,
            maxRound: 5
        );

        // Reviewer Agent finalizes the content
        var reviewMessage = "Please finalize the story after receiving feedback.";
        await reviewerAgent.InitiateChatAsync(
            receiver: criticAgent,
            message: reviewMessage,
            maxRound: 5
        );
    }
}