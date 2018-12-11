using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XmasDevGiftAssistantDemo.Shopping;

namespace XmasDevGiftAssistantDemo
{
    public class XmasDevGiftAssistantDemoBot : IBot
    {
        private readonly XmasDevGiftAssistantDemoAccessors _accessors;
        private readonly ILogger _logger;
        private readonly IShopping _shopping;
        private readonly IShoppingHelper _shoppingHelper;
        private const string WelcomeText = "Questo bot ti permetterà di trovare dei prodoti interessanti da regalare per natale";
        private DialogSet _dialogSet;

        private const string CustomSearchDialog = "customSearchDialog";
        private const string AgePrompt = "agePrompt";
        private const string GenderPrompt = "genderPrompt";
        private const string PricePrompt = "pricePrompt";



        // Services configured from the ".bot" file.
        private static LuisRecognizer _luisRecognizer;
        public static readonly string LuisKey = "XmasDevGiftAssistantDemo";

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="accessors">A class containing <see cref="IStatePropertyAccessor{T}"/> used to manage state.</param>
        /// <param name="loggerFactory">A <see cref="ILoggerFactory"/> that is hooked to the Azure App Service provider.</param>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1#windows-eventlog-provider"/>
        public XmasDevGiftAssistantDemoBot(XmasDevGiftAssistantDemoAccessors accessors, ILoggerFactory loggerFactory, IShopping shopping, IShoppingHelper shoppingHelper)
        {
            if (loggerFactory == null)
            {
                throw new System.ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<XmasDevGiftAssistantDemoBot>();
            _logger.LogTrace("Turn start.");
            _accessors = accessors ?? throw new System.ArgumentNullException(nameof(accessors));

            // Create the dialog set and add the prompts, including custom validation.
            _dialogSet = new DialogSet(_accessors.DialogStateAccessor);
            _dialogSet.Add(new ChoicePrompt(GenderPrompt));
            _dialogSet.Add(new NumberPrompt<int>(AgePrompt, AgeValidatorAsync));
            _dialogSet.Add(new NumberPrompt<decimal>(PricePrompt, PriceValidatorAsync));

            // Define the steps of the waterfall dialog and add it to the set.
            WaterfallStep[] steps = new WaterfallStep[]
            {
                PromptForGenderAsync,
                PromptForAgeAsync,
                PromptForPriceAsync,
                CustomSearchResultAsync,
            };
            _dialogSet.Add(new WaterfallDialog(CustomSearchDialog, steps));

            _shopping = shopping;
            _shoppingHelper = shoppingHelper;
        }



        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {

                // Get the current reservation info from state.
                CustomSearch customSearch = await _accessors.CustomSearchAccessor.GetAsync(
                    turnContext, () => null, cancellationToken);

                // Generate a dialog context for our dialog set.
                DialogContext dc = await _dialogSet.CreateContextAsync(turnContext, cancellationToken);

                if (turnContext.Activity.Text == "Start") {
                    await dc.CancelAllDialogsAsync(cancellationToken);
                    customSearch = null;
                    await _accessors.CustomSearchAccessor.SetAsync(
                           turnContext,
                           customSearch,
                           cancellationToken);

                    await dc.CancelAllDialogsAsync(cancellationToken);
                }

                if (dc.ActiveDialog is null)
                {
                    if (customSearch is null)
                    {
                        // If not, start the dialog.
                        await dc.BeginDialogAsync(CustomSearchDialog, null, cancellationToken);
                    }
                    else
                    {
                        // Otherwise, send a status message.
                        customSearch = null;
                        await _accessors.CustomSearchAccessor.SetAsync(
                            turnContext,
                            customSearch,
                            cancellationToken);

                        await dc.CancelAllDialogsAsync(cancellationToken);
                        await ShowStartAsync(turnContext, cancellationToken);
                    }
                }
                else
                {
                    // Continue the dialog.
                    DialogTurnResult dialogTurnResult = await dc.ContinueDialogAsync(cancellationToken);

                    // If the dialog completed this turn, record the reservation info.
                    if (dialogTurnResult.Status is DialogTurnStatus.Complete)
                    {
                        customSearch = (CustomSearch)dialogTurnResult.Result;
                        await _accessors.CustomSearchAccessor.SetAsync(
                            turnContext,
                            customSearch,
                            cancellationToken);

                        await ShowAmazonProductsAsync(turnContext, customSearch, cancellationToken);
                        await ShowStartAsync(turnContext, cancellationToken);
                    }
                }

                // Save the updated dialog state into the conversation state.
                await _accessors.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            }
            else if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate)
            {
                if (turnContext.Activity.MembersAdded != null)
                {
                    await SendWelcomeMessageAsync(turnContext, cancellationToken);
                }
            }
            //else
            //{
            //    await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected");
            //}
        }

        private async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    var reply = turnContext.Activity.CreateReply();
                    var card = new HeroCard
                    {
                        Title = $"Benvenuto su XMASDEV Gift Assistant.",
                        Images = new List<CardImage>() {
                            new CardImage(url: "http://xmasdevgiftassistantdemo.azurewebsites.net/images/logo_full.png")
                        }
                    };

                    reply.Attachments.Add(card.ToAttachment());

                    await turnContext.SendActivityAsync(reply, cancellationToken);
                    await ShowStartAsync(turnContext, cancellationToken);

                }
            }
        }

        private async Task SendAnotherTestMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var replyQuestion = turnContext.Activity.CreateReply();

            replyQuestion.Text = "Vuoi fare un nuovo test? ";
            replyQuestion.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                        {
                            new CardAction() { Title = "Senza LUIS", Type = ActionTypes.ImBack, Value = "NOLUIS" },
                            new CardAction() { Title = "Con LUIS", Type = ActionTypes.ImBack, Value = "LUIS" },
                             new CardAction() { Title = "Amazon Test", Type = ActionTypes.ImBack, Value = "AMAZONTEST" },
                        },
            };
            await turnContext.SendActivityAsync(replyQuestion, cancellationToken);

        }

        private async Task ShowStartAsync(ITurnContext turnContext,  CancellationToken cancellationToken)
        {


            var replyQuestion = turnContext.Activity.CreateReply();
            replyQuestion.Text = "Premi o scrivi Start per incominciare";
            replyQuestion.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                        {
                            new CardAction() { Title = "Start", Type = ActionTypes.ImBack, Value = "Start" },
                         },
            };
            await turnContext.SendActivityAsync(replyQuestion, cancellationToken);
        }


            private async Task ShowAmazonProductsAsync(ITurnContext turnContext, CustomSearch customSearch, CancellationToken cancellationToken)
        {
           var searchIndexs = _shoppingHelper.GetSearchIndexDictionary(customSearch.Age, customSearch.Gender);
            Random random = new Random();
            int randomNumber = random.Next(0, searchIndexs.Keys.Count);

            int index = searchIndexs.ElementAt(randomNumber).Key;
            var items1 = _shopping.SearchItems("", (Enums.SearchIndex)index, 0, true, 0, Convert.ToInt32(customSearch.Price), 1, Enums.SearchSort.Salesrank, Enums.SearchSortOrder.Ascending);

            randomNumber = random.Next(0, searchIndexs.Keys.Count);
            index = searchIndexs.ElementAt(randomNumber).Key;

            var items2 = _shopping.SearchItems("", (Enums.SearchIndex)index, 0, true, 0, Convert.ToInt32(customSearch.Price), 1, Enums.SearchSort.Salesrank, Enums.SearchSortOrder.Ascending);

            randomNumber = random.Next(0, searchIndexs.Keys.Count);
            index = searchIndexs.ElementAt(randomNumber).Key;
            var items3 = _shopping.SearchItems("", (Enums.SearchIndex)index, 0, true, 0, Convert.ToInt32(customSearch.Price), 1, Enums.SearchSort.Salesrank, Enums.SearchSortOrder.Ascending);

            
            List<Item> items = new List<Item>();
            items.AddRange(items1.Items.Where(x=>x.Price<customSearch.Price).OrderBy(x => random.Next()).Take(5));
            items.AddRange(items2.Items.Where(x => x.Price < customSearch.Price).OrderBy(x => random.Next()).Take(5));
            items.AddRange(items3.Items.Where(x => x.Price < customSearch.Price).OrderBy(x => random.Next()).Take(5));

           

            List<Attachment> attachments = new List<Attachment>();

            foreach (var item in items)
            {
                attachments.Add(
                     new HeroCard(
                    title: item.Title,
                    subtitle: $"Prezzo € {item.Price}",
                    images: new CardImage[] { new CardImage(url: item.ImageUrl, alt: item.Title) },
                    buttons: new CardAction[]
                    {
                            new CardAction(title: "Compra", type: ActionTypes.MessageBack, value: item.ItemId)
                    })
                .ToAttachment()
                                );
            }
            var activity = MessageFactory.Carousel(attachments);

            // Send the activity as a reply to the user.
            await turnContext.SendActivityAsync(activity);

        }

        private async Task<DialogTurnResult> PromptForAgeAsync(
           WaterfallStepContext stepContext,
           CancellationToken cancellationToken = default(CancellationToken))
        {

            var gender = (FoundChoice)(stepContext.Result);
            stepContext.Values["gender"] = gender.Value;

            return await stepContext.PromptAsync(
                AgePrompt,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Quanti anni ha?"),
                    RetryPrompt = MessageFactory.Text("Inserisci un età corretta"),
                },
                cancellationToken);
        }

        private async Task<DialogTurnResult> PromptForGenderAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
           
            return await stepContext.PromptAsync(
                GenderPrompt,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Vuoi fare un regalo ad una donna o un uomo?"),
                    RetryPrompt = MessageFactory.Text("Mi dispiace, inserisci solo uomo o donna."),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "Donna", "Uomo" }),
                },
                cancellationToken);
        }

        private async Task<DialogTurnResult> PromptForPriceAsync(
         WaterfallStepContext stepContext,
         CancellationToken cancellationToken = default(CancellationToken))
        {

            var age = stepContext.Result;
            stepContext.Values["age"] = age;

            return await stepContext.PromptAsync(
                PricePrompt,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Quanto pensavi di spendere?"),
                    RetryPrompt = MessageFactory.Text("Mi dispiace, inserisci un prezzo valido"),
                },
                cancellationToken);
        }
        private async Task<DialogTurnResult> CustomSearchResultAsync(
              WaterfallStepContext stepContext,
              CancellationToken cancellationToken = default(CancellationToken))
        {

            var price = (decimal)stepContext.Result;
            stepContext.Values["price"] = price;

            // Return the collected information to the parent context.
            CustomSearch customSearch = new CustomSearch
            {
                Price = price,
                Age= (int)stepContext.Values["age"],
                Gender= (stepContext.Values["gender"].ToString().ToLower()=="donna") ? Enums.Gender.female: Enums.Gender.male 
            };
           
            return await stepContext.EndDialogAsync(customSearch, cancellationToken);
        }

        private async Task<bool> AgeValidatorAsync(
            PromptValidatorContext<int> promptContext,
            CancellationToken cancellationToken)
        {
            // Check whether the input could be recognized as an integer.
            if (!promptContext.Recognized.Succeeded)
            {
                await promptContext.Context.SendActivityAsync(
                    "Mi dispiace, non ho capito. Inserisci l'età.",
                    cancellationToken: cancellationToken);
                return false;
            }

          
            int age = promptContext.Recognized.Value;
            if (age < 1 || age > 100)
            {
                await promptContext.Context.SendActivityAsync(
                    "Inserisci un età valida",
                    cancellationToken: cancellationToken);
                return false;
            }

            return true;
        }

        private async Task<bool> PriceValidatorAsync(
        PromptValidatorContext<decimal> promptContext,
        CancellationToken cancellationToken = default(CancellationToken))
        {
            // Check whether the input could be recognized as an integer.
            if (!promptContext.Recognized.Succeeded)
            {
                await promptContext.Context.SendActivityAsync(
                    "Mi dispiace, non ho capito. Inserisci un prezzo valido.",
                    cancellationToken: cancellationToken);
                return false;
            }

            decimal price = promptContext.Recognized.Value;
            if (price < 1 || price > 5000)
            {
                await promptContext.Context.SendActivityAsync(
                    "Inserisci un prezzo valido",
                    cancellationToken: cancellationToken);
                return false;
            }

            return true;
        }

      
    }
}