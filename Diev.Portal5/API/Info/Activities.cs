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

namespace Diev.Portal5.API.Info;

/// <summary>
/// Краткое наименование вида деятельности.
/// </summary>
public record Activities
(
    /// <summary>
    /// Полное наименование вида деятельности (необязательное поле).
    /// </summary>
    string? FullName,

    /// <summary>
    /// Краткое наименование вида деятельности (необязательное поле).
    /// </summary>
    string? ShortName,

    /// <summary>
    /// Поднадзорное подразделение:
    /// Name – наименование поднадзорного подразделения Банка России (необязательное поле).
    /// </summary>
    SupervisionDevision? SupervisionDevision // Division?
);

/*
[
    {
        "FullName": "Депозитарии",
        "ShortName": "Депозитарии",
        "SupervisionDevision": {
            "Name": "Руководство"
        }
    },
    {
        "FullName": "Банки",
        "ShortName": "Кредитные организации",
        "SupervisionDevision": {
            "Name": "Руководство"
        }
    },
    {
        "FullName": "Брокеры",
        "ShortName": "Брокеры",
        "SupervisionDevision": {
            "Name": "Руководство"
        }
    },
    {
        "FullName": "Доверительные управляющие",
        "ShortName": "Доверительные управляющие",
        "SupervisionDevision": {
            "Name": "Руководство"
        }
    }
]
*/
