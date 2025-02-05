# AutoGen Multi-Agent Example

This project demonstrates a multi-agent system using Microsoft AutoGen with the OpenAI API in .NET. The system includes three agents: a Writer Agent, a Critic Agent, and a Reviewer Agent.

## Prerequisites

- .NET SDK
- OpenAI API Key

## Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Dharma-DX/AutoGenMultiAgentExample.git
   cd AutoGenMultiAgentExample
Install the necessary packages:

dotnet new console -o AutoGenMultiAgentExample; and cd AutoGenMultiAgentExample; and dotnet add package AutoGen; and dotnet add package AutoGen.OpenAI; and set -x OPENAI_API_KEY "your_openai_api_key"
Set your OpenAI API key:

set -x OPENAI_API_KEY "your_openai_api_key"
Code Overview
The project consists of a simple multi-agent system where:

The Writer Agent generates content based on user input.
The Critic Agent provides feedback on the writer's content.
The Reviewer Agent finalizes the content after receiving feedback from the critic.
Main Program
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
        // Set your OpenAI API key
        var openAIKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new Exception("Please set OPENAI_API_KEY environment variable.");
        var openAIClient = new OpenAIClient(openAIKey);

        // Define the model to use
        var model = "gpt-4";

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
        var initialMessage = "Hey writer, please generate a short story.";

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
Running the Example
Build the project:

dotnet build
Run the application:

dotnet run
License
This project is licensed under the MIT License. See the LICENSE file for details.

Contributing
Contributions are welcome! Please open an issue or submit a pull request.


Feel free to update the repository with this content. 
