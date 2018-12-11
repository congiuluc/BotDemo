// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using XmasDevGiftAssistantDemo.Shopping;

namespace XmasDevGiftAssistantDemo
{
    public class ChatState
    {
        public int TurnCount { get; set; } = 0;

        public string Username { get; set; }

        public SearchFilter SearchFilter{ get; set; }
    }
}
