#region License
/*
Copyright 2022-2025 Dmitrii Evdokimov
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

using Microsoft.Extensions.Configuration;

namespace CryptoBot.Helpers;

public static class JsonSection
{
    public static string[] Values(IConfigurationSection config, string key)
    {
        var section = config.GetSection(key);
        List<string> list = [];

        foreach (var item in section.GetChildren())
        {
            list.Add(item.Value!);
        }
        
        return [.. list];
    }

    public static string[] MyOld(IConfigurationSection config)
    {
        return Values(config, nameof(MyOld));
    }

    public static string[] Subscribers(IConfigurationSection config)
    {
        return Values(config, nameof(Subscribers));
    }

    //public static string[] DoverXml(IConfigurationSection config)
    //{
    //    return Values(config, nameof(DoverXml));
    //}
}
