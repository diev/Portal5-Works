#region License
/*
Copyright 2022-2024 Dmitrii Evdokimov
Open source software

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System.CommandLine;
using System.CommandLine.Binding;

using Diev.Portal5.API.Tools;

namespace CryptoBot;

public class MessagesFilterBinder(
    Option<string?> task,
    Option<int?> days, Option<string?> minDate, Option<string?> maxDate,
    Option<int?> minSize, Option<int?> maxSize,
    Option<bool> inbox, Option<bool> outbox,
    Option<string?> status,
    Option<int?> page)
    : BinderBase<MessagesFilter>
{
    protected override MessagesFilter GetBoundValue(BindingContext ctx)
    {
        int? d = ctx.ParseResult.GetValueForOption(days);
        string? day = d is null ? null : $"{DateTime.Now.AddDays((double)-d):yyyy-MM-dd}";
        string? from = ctx.ParseResult.GetValueForOption(minDate);
        string? to = ctx.ParseResult.GetValueForOption(maxDate);
        bool ibx = ctx.ParseResult.GetValueForOption(inbox);
        bool obx = ctx.ParseResult.GetValueForOption(outbox);

        return new()
        {
            Task = ctx.ParseResult.GetValueForOption(task),
            MinDate = d is null ? from : day,
            MaxDate = d is null ? to : day,
            MinSize = ctx.ParseResult.GetValueForOption(minSize),
            MaxSize = ctx.ParseResult.GetValueForOption(maxSize),
            Type = ibx == obx ? null : ibx ? "inbox" : "outbox",
            Status = ctx.ParseResult.GetValueForOption(status),
            Page = ctx.ParseResult.GetValueForOption(page)
        };
    }
}
