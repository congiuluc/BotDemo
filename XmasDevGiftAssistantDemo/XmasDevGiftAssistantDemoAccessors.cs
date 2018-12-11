using System;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using XmasDevGiftAssistantDemo.Shopping;
using static XmasDevGiftAssistantDemo.Shopping.Enums;

namespace XmasDevGiftAssistantDemo
{
    public class XmasDevGiftAssistantDemoAccessors
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogPromptBotAccessors"/> class.
        /// Contains the <see cref="ConversationState"/> and associated <see cref="IStatePropertyAccessor{T}"/>.
        /// </summary>
        /// <param name="conversationState">The state object that stores the counter.</param>
        public XmasDevGiftAssistantDemoAccessors(ConversationState conversationState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
        }

        public static string DialogStateAccessorKey { get; } = "XmasDevGiftAssistantDemoAccessors.DialogState";

        public static string CustomSearchAccessorKey { get; } = "XmasDevGiftAssistantDemoAccessors.CustomSearch";

        /// <summary>
        /// Gets or sets the <see cref="IStatePropertyAccessor{T}"/> for CounterState.
        /// </summary>
        /// <value>
        /// The accessor stores the turn count for the conversation.
        /// </value>
        public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; set; }

        public IStatePropertyAccessor<CustomSearch> CustomSearchAccessor { get; set; }

        /// <summary>
        /// Gets the <see cref="ConversationState"/> object for the conversation.
        /// </summary>
        /// <value>The <see cref="ConversationState"/> object.</value>
        public ConversationState ConversationState { get; }
    }

    public class CustomSearch
    {
        public Gender   Gender{ get; set; }

        public int Age { get; set; }

        public decimal Price{ get; set; }
    }
}
