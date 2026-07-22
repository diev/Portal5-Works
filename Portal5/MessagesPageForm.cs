#region License
/*
Copyright 2022-2026 Dmitrii Evdokimov
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

using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Interfaces;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Portal5;

public partial class MessagesPageForm : Form
{
    private readonly IApiService _api;
    private MessagesPage _page = null!;
    private string _downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true
    };

    public MessagesPageForm() : this(null!) { }

    public MessagesPageForm(IApiService api)
    {
        _api = api;

        InitializeComponent();
        messagesGrid.ContextMenuStrip = contextMenu;

        string text = "Все задачи";
        taskBox.Items.Add(text);
        taskBox.SelectedItem = text; //RefreshGrid();
    }

    private void RefreshGrid(object sender, EventArgs e)
    {
        RefreshGrid();
    }

    private async void RefreshGrid()
    {
        StatusLabel.Text = "Загрузка...";
        Cursor = Cursors.WaitCursor;

        var filter = new MessagesFilter(
            task: taskBox.Text == "Все задачи" ? null : taskBox.Text,
            before: null,
            days: null,
            day: null,
            minDateTime: minDateBox.Checked ? minDateBox.Value.Date : null,
            maxDateTime: maxDateBox.Checked ? maxDateBox.Value.Date.AddDays(1) : null,
            minSize: null,
            maxSize: null,
            inbox: inBox.Checked,
            outbox: outBox.Checked,
            status: null,
            page: (uint?)pageBox.Value);

        var result = await _api.GetMessagesPageAsync(filter);

        if (result.OK)
        {
            _page = result.Data!;
            var pages = _page.Pages;
            StatusLabel.Text = $"Страница {pages.CurrentPage} из {pages.TotalPages}";

            int i = 0;
            var rows = _page.Messages.Select(x => new
            {
                N = ++i,
                Type = x.Type == "inbox" ? "Вх" : "Исх",
                x.TaskName,
                x.Title,
                x.Text,
                x.CreationDate,
                Status = $"{x.Status} {x.RegNumber}",
                Files = $"{x.Files.Length}: " +
                    string.Join(", ", x.Files.Where(f => f.SignedFile is null).Select(f => f.Name).Order()),
                x.TotalSize
                //x.Id,
                //x.CorrelationId
            }).ToArray();

            messagesGrid.DataSource = rows;
            messagesGrid.AutoResizeColumns();

            pageBox.Maximum = pages.TotalPages;
            pageBox.Enabled = pageBox.Maximum > 1;

            StatusLabel.Text = $"Страница {pages.CurrentPage} из {pages.TotalPages}";
        }
        else
        {
            StatusLabel.Text = "Ошибка загрузки";
        }

        Cursor = Cursors.Default;
    }

    private void messagesGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0)
            return;

        var clickedColumn = messagesGrid.Columns[e.ColumnIndex];
        var row = messagesGrid.Rows[e.RowIndex];

        string text = row.Cells[e.ColumnIndex].Value?.ToString() ?? string.Empty;

        if (clickedColumn.Name == "TaskName")
        {
            if (!taskBox.Items.Contains(text))
            {
                taskBox.Items.Add(text);
            }

            taskBox.SelectedItem = text; //RefreshGrid();
        }
    }

    private void contextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        // Получаем позицию курсора относительно грида
        var pt = messagesGrid.PointToClient(Cursor.Position);
        var hit = messagesGrid.HitTest(pt.X, pt.Y);

        // Если клик не по строке - отменяем открытие
        if (hit.RowIndex < 0)
        {
            e.Cancel = true;
            return;
        }

        int rowIndex = hit.RowIndex;
        var row = _page.Messages[rowIndex];

        // Сохраняем Message.Id в Tag меню, чтобы потом его использовать
        contextMenu.Tag = row.Id;
        filterMenuItem.Text = $"Фильтр по {row.TaskName}";
    }

    private void filterMenuItem_Click(object sender, EventArgs e)
    {
        if (contextMenu.Tag is not string id)
            return;

        // Находим объект по Id (на случай, если список отфильтрован и индексы не совпадают)
        var row = _page.Messages.FirstOrDefault(x => x.Id == id);
        if (row == null)
        {
            MessageBox.Show("Элемент не найден.",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string text = row.TaskName;

        if (!taskBox.Items.Contains(text))
        {
            taskBox.Items.Add(text);
        }

        taskBox.SelectedItem = text; //RefreshGrid();
    }

    private async void savePageMenuItem_Click(object sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Куда сохранить файл?",
            InitialDirectory = _downloadPath,
            FileName = $"Portal5-{DateTime.Now:yyyyMMdd-HHmm}.json",
            DefaultExt = "json"
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        string json = JsonSerializer.Serialize(_page, _jsonOptions);

        string path = dialog.FileName;
        _downloadPath = Path.GetDirectoryName(path) ?? string.Empty;
        await File.WriteAllTextAsync(path, json);

        if (Path.Exists(path))
        {
            MessageBox.Show($"Json сохранён в {path}",
                "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show($"Json не сохранён в {path}",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void saveJsonMenuItem_Click(object sender, EventArgs e)
    {
        if (contextMenu.Tag is not string id)
            return;

        // Находим объект по Id (на случай, если список отфильтрован и индексы не совпадают)
        var row = _page.Messages.FirstOrDefault(x => x.Id == id);
        if (row == null)
        {
            MessageBox.Show("Элемент не найден.",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string json = JsonSerializer.Serialize(row, _jsonOptions);

        using var dialog = new SaveFileDialog
        {
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Куда сохранить файл?",
            InitialDirectory = _downloadPath,
            FileName = $"{id}.json",
            DefaultExt = "json"
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        string path = dialog.FileName;
        _downloadPath = Path.GetDirectoryName(path) ?? string.Empty;
        await File.WriteAllTextAsync(path, json);

        if (Path.Exists(path))
        {
            MessageBox.Show($"Json сохранён в {path}",
                "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show($"Json не сохранён в {path}",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void saveZipMenuItem_Click(object sender, EventArgs e)
    {
        if (contextMenu.Tag is not string id)
            return;

        // Находим объект по Id (на случай, если список отфильтрован и индексы не совпадают)
        var row = _page.Messages.FirstOrDefault(x => x.Id == id);
        if (row == null)
        {
            MessageBox.Show("Элемент не найден.",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*",
            Title = "Куда сохранить файл?",
            InitialDirectory = _downloadPath,
            FileName = $"{id}.zip",
            DefaultExt = "zip"
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        string path = dialog.FileName;
        _downloadPath = Path.GetDirectoryName(path) ?? string.Empty;
        var result = await _api.DownloadMessageZipAsync(id, path);

        if (result.OK)
        {
            MessageBox.Show($"Zip сохранён в {path}",
                "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show($"Zip не сохранён в {path}",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void saveFilesMenuItem_Click(object sender, EventArgs e)
    {
        if (contextMenu.Tag is not string id)
            return;

        // Находим объект по Id (на случай, если список отфильтрован и индексы не совпадают)
        var row = _page.Messages.FirstOrDefault(x => x.Id == id);
        if (row == null)
        {
            MessageBox.Show("Элемент не найден.",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var dialog = new FolderBrowserDialog
        {
            Description = "Выберите папку для сохранения",
            SelectedPath = _downloadPath,
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        string folder = dialog.SelectedPath;
        _downloadPath = folder;
        bool ok = true;

        foreach (var file in row.Files)
        {
            string path = Path.Combine(folder, file.Name);
            var result = await _api.DownloadMessageFileAsync(id, file.Id, path);
            ok = ok && result.OK;
        }

        if (ok)
        {
            MessageBox.Show($"Файлы сохранены в {folder}",
                "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show($"Файлы не сохранены в {folder}",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void viewLkMenuItem_Click(object sender, EventArgs e)
    {
        if (contextMenu.Tag is not string id)
            return;

        // Находим объект по Id (на случай, если список отфильтрован и индексы не совпадают)
        var row = _page.Messages.FirstOrDefault(x => x.Id == id);
        if (row == null)
        {
            MessageBox.Show("Элемент не найден.",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var baseUri = _api.GetBaseAddress();
        string path = $"/messages/view-message/{id}/";
        var uri = new Uri(baseUri, path);

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = uri.ToString(),
                UseShellExecute = true
            };

            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось открыть ссылку: {ex.Message}",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
