// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.18.1

using Azure;
using Azure.AI.OpenAI;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot1.Bots
{

    public class EchoBot : ActivityHandler
    {

        public static string key = "23565e934b4e4e5a951fff22bfb785f2";
        public static string endpoint = "https://gfsautomation.openai.azure.com/";


        OpenAIClient client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
   
        public static string deploymentName = "gpt35";


            ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages =
            {
                new ChatMessage(ChatRole.System, "You are a helpful assistant. You will talk like a pirate."),
                new ChatMessage(ChatRole.User, "Can you help me?"),
                new ChatMessage(ChatRole.Assistant, "Arrrr! Of course, me hearty! What can I do for ye?"),
                new ChatMessage(ChatRole.User, "What's the best way to train a parrot?"),
            }
            };



        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {

                CompletionsOptions completionsOptions = new CompletionsOptions();
                completionsOptions.Prompts.Add(turnContext.Activity.Text);

                Response<ChatCompletions> completionsResponse = client.GetChatCompletions(deploymentName, new ChatCompletionsOptions()
                    {
                        Messages =
                {
                    new ChatMessage(ChatRole.User, turnContext.Activity.Text)

                }
                    });

                foreach(var resp in completionsResponse.Value.Choices)
                {
                    string completion = resp.Message.Content;
                    //Console.WriteLine($"Chatbot: {completion}");
                    var replyText = completion;
                    await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                }
                

            
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
