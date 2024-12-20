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
    Option<uint?> before, Option<uint?> days, Option<uint?> day,
    Option<DateTime?> minDateTime, Option<DateTime?> maxDateTime,
    Option<uint?> minSize, Option<uint?> maxSize,
    Option<bool> inbox, Option<bool> outbox,
    Option<string?> status,
    Option<uint?> page)
    : BinderBase<MessagesFilter>
{
    protected override MessagesFilter GetBoundValue(BindingContext ctx)
    {
        var today = DateTime.Today; // next 00:00

        uint? b = ctx.ParseResult.GetValueForOption(before);
        uint? d = ctx.ParseResult.GetValueForOption(days);
        uint? n = ctx.ParseResult.GetValueForOption(day);

        DateTime? day1 = n is null
            ? null
            : today.AddDays((double)-n);
        DateTime? from = d is null
            ? ctx.ParseResult.GetValueForOption(minDateTime)
            : today.AddDays((double)-d);
        DateTime? to = b is null
            ? ctx.ParseResult.GetValueForOption(maxDateTime)
            : today.AddDays((double)-b);

        bool ibx = ctx.ParseResult.GetValueForOption(inbox);
        bool obx = ctx.ParseResult.GetValueForOption(outbox);

        return new()
        {
            Task = ctx.ParseResult.GetValueForOption(task),
            MinDateTime = n is null ? from : day1,
            MaxDateTime = n is null ? to : day1!.Value.AddDays(1),
            MinSize = ctx.ParseResult.GetValueForOption(minSize),
            MaxSize = ctx.ParseResult.GetValueForOption(maxSize),
            Type = ibx == obx ? null : ibx ? "inbox" : "outbox",
            Status = ctx.ParseResult.GetValueForOption(status),
            Page = ctx.ParseResult.GetValueForOption(page)
        };
    }
}
