﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace ContosoHelpdeskChatBot.Dialogs
{
    public class MainDialog : WaterfallDialog
    {
        private const string InstallAppOption = "Install Application (install)";
        private const string GreetMessage = "Welcome to **Contoso Helpdesk Chat Bot**.\n\nI am designed to use with mobile email app, make sure your replies do not contain signatures. \n\nFollowing is what I can help you with, just reply with word in parenthesis:";
        private const string ErrorMessage = "Not a valid option.  Please select one of the possible choices:";
        private static List<Choice> HelpdeskOptions = new List<Choice>
        {
            new Choice { Value = InstallAppOption, Synonyms = new List<string>{ "install", "application", "install application" } },
        };
        public static string dialogId = "MainDialog";


        public MainDialog(string dialogId) : base(dialogId)
        {
            AddStep(GreetingStepAsync);
            AddStep(ChoiceSelectedStepAsync);
        }

        private static async Task<DialogTurnResult> GreetingStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync("promptChoice", new PromptOptions
            {
                Prompt = MessageFactory.Text(GreetMessage),
                Choices = HelpdeskOptions,
                RetryPrompt = MessageFactory.Text(ErrorMessage)
            }, cancellationToken);
        }

        private static async Task<DialogTurnResult> ChoiceSelectedStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var optionSelected = (stepContext.Result as FoundChoice)?.Value;

            switch (optionSelected)
            {
                case InstallAppOption:
                    return await stepContext.BeginDialogAsync(InstallAppDialog.dialogId);
            }

            return await stepContext.NextAsync();
        }
    }
}