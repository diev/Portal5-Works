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

using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Interfaces;

using Microsoft.Extensions.Options;

namespace Portal5;

public partial class MessagesForm : Form
{
    private readonly bool _autoRefresh = false;
    private readonly IPortalService _portal;
    private readonly MessagesFilter _filter;
    private List<Diev.Portal5.API.Messages.Message> _messages = [];
    private string _downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public MessagesForm() : this(null!, null!) { }

    public MessagesForm(
        IPortalService portal,
        IOptions<MessagesFilterSettings> messagesFilterSettings
        )
    {
        _portal = portal;

        InitializeComponent();
        messagesGrid.ContextMenuStrip = contextMenu;

        var s = messagesFilterSettings.Value;
        _filter = new MessagesFilter(
            task: s.Task,
            tasks: s.Tasks,
            notasks: s.NoTasks,
            before: s.Before,
            days: s.Days,
            day: s.Day,
            minDateTime: s.MinDateTime,
            maxDateTime: s.MaxDateTime,
            minSize: s.MinSize,
            maxSize: s.MaxSize,
            inbox: s.Inbox,
            outbox: s.Outbox,
            status: s.Status,
            page: s.Page
            );

        inBox.Checked = s.Inbox;
        outBox.Checked = s.Outbox;

        if (_filter.MinDateTime.HasValue)
        {
            minDateBox.Checked = true;
            minDateBox.Value = (DateTime)_filter.MinDateTime;
        }

        if (_filter.MaxDateTime.HasValue)
        {
            maxDateBox.Checked = true;
            maxDateBox.Value = (DateTime)_filter.MaxDateTime;
        }
        else
        {
            maxDateBox.Value = DateTime.Today;
        }

        string text = "Все задачи";
        taskBox.Items.Add(text);
        taskBox.SelectedItem = text;

        _autoRefresh = true;
        RefreshGrid();

        //messagesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        FitColumnsToWidthNoScroll(messagesGrid);
    }

    private void RefreshGrid(object sender, EventArgs e)
    {
        RefreshGrid();
    }

    private async void RefreshGrid()
    {
        if (!_autoRefresh)
            return;

        StatusLabel.Text = "Загрузка...";
        Cursor = Cursors.WaitCursor;

        var filter = new MessagesFilter(_filter)
        {
            Task = taskBox.Text == "Все задачи" ? null : taskBox.Text,
            MinDateTime = minDateBox.Checked ? minDateBox.Value.Date : null,
            MaxDateTime = maxDateBox.Checked ? maxDateBox.Value.Date.AddDays(1) : null,
            Type = inBox.Checked == outBox.Checked
                ? null
                : inBox.Checked
                    ? MessageType.Inbox
                    : MessageType.Outbox
        };

        var result = await _portal.GetMessagesAsync(filter);

        if (result.OK)
        {
            _messages = result.Data!;
            StatusLabel.Text = $"Всего: {_messages.Count}";

            //int i = 0;
            var rows = _messages.Select(x => new
            {
                //N = ++i,
                Type = x.Type == "inbox" ? "Вх" : "Исх",
                x.TaskName,
                x.Title,
                x.Text,
                x.CreationDate,
                Status = $"{x.Status} {x.RegNumber}",
                Files = $"{x.Files.Count}: " +
                    string.Join(", ", x.Files.Where(f => f.SignedFile is null).Select(f => f.Name).Order()),
                x.TotalSize
                //x.Id,
                //x.CorrelationId
            }).ToArray();

            messagesGrid.DataSource = null;
            messagesGrid.DataSource = rows;
            SetupSortableColumns(messagesGrid);
            messagesGrid.AutoResizeColumns();
        }
        else
        {
            StatusLabel.Text = $"Ошибка загрузки: {result.Error?.ErrorMessage}";
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
        var message = _messages[rowIndex];

        contextMenu.Tag = message;
        filterMenuItem.Text = $"Фильтр по {message.TaskName}";
    }

    private void filterMenuItem_Click(object sender, EventArgs e)
    {
        var message = (Diev.Portal5.API.Messages.Message)contextMenu.Tag!;
        string text = message.TaskName;

        if (!taskBox.Items.Contains(text))
        {
            taskBox.Items.Add(text);
        }

        taskBox.SelectedItem = text; //RefreshGrid();
    }

    private async void saveMessagesMenuItem_Click(object sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Куда сохранить файл?",
            InitialDirectory = _downloadPath,
            FileName = $"Portal5 {DateTime.Now:yyyyMMdd-HHmm}.json",
            DefaultExt = "json"
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        string path = dialog.FileName;
        _downloadPath = Path.GetDirectoryName(path) ?? string.Empty;
        var result = await _portal.SaveMessagesJsonAsync(_messages, path);

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

    private async void saveMessageMenuItem_Click(object sender, EventArgs e)
    {
        var message = (Diev.Portal5.API.Messages.Message)contextMenu.Tag!;
        var msgId = message.Id;

        using var dialog = new SaveFileDialog
        {
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Куда сохранить файл?",
            InitialDirectory = _downloadPath,
            FileName = $"{msgId}.json",
            DefaultExt = "json"
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        string path = dialog.FileName;
        _downloadPath = Path.GetDirectoryName(path) ?? string.Empty;
        var result = await _portal.SaveMessageJsonAsync(message, path);

        if (result.OK)
        {
            MessageBox.Show($"Json сохранён в {path}",
                "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show(result.Error?.ErrorMessage,
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void saveZipMenuItem_Click(object sender, EventArgs e)
    {
        var message = (Diev.Portal5.API.Messages.Message)contextMenu.Tag!;
        var msgId = message.Id;

        using var dialog = new SaveFileDialog
        {
            Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*",
            Title = "Куда сохранить файл?",
            InitialDirectory = _downloadPath,
            FileName = $"{msgId}.zip",
            DefaultExt = "zip"
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        string path = dialog.FileName;
        _downloadPath = Path.GetDirectoryName(path) ?? string.Empty;
        var result = await _portal.DownloadMessageZipAsync(msgId, path);

        if (result.OK)
        {
            MessageBox.Show($"Zip сохранён в {path}",
                "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show(result.Error?.ErrorMessage,
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void saveFilesMenuItem_Click(object sender, EventArgs e)
    {
        var message = (Diev.Portal5.API.Messages.Message)contextMenu.Tag!;

        using var dialog = new FolderBrowserDialog
        {
            Description = "Выберите папку для сохранения",
            SelectedPath = _downloadPath,
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        string path = dialog.SelectedPath;
        _downloadPath = path;
        var result = await _portal.DownloadMessageFilesAsync(message, path);

        if (result.OK)
        {
            MessageBox.Show($"Файлы сохранены в {path}",
                "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show(result.Error?.ErrorMessage,
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void decryptFilesMenuItem_Click(object sender, EventArgs e)
    {
        var message = (Diev.Portal5.API.Messages.Message)contextMenu.Tag!;

        using var dialog = new FolderBrowserDialog
        {
            Description = "Выберите папку для сохранения",
            SelectedPath = _downloadPath,
        };

        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        string path = dialog.SelectedPath;
        _downloadPath = path;
        var result = await _portal.DecryptMessageFilesAsync(message, path);

        if (result.OK)
        {
            MessageBox.Show($"Файлы расшифрованы в {path}",
                "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show(result.Error?.ErrorMessage,
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void viewLkMenuItem_Click(object sender, EventArgs e)
    {
        var message = (Diev.Portal5.API.Messages.Message)contextMenu.Tag!;
        string url = _portal.GetMessageUrl(message.Id);

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };

            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось открыть ссылку {url}{Environment.NewLine}{ex.Message}",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private static void FitColumnsToWidthNoScroll(DataGridView dgv)
    {
        // Сначала подгоняем под содержимое
        dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

        int totalWidth = 0;
        foreach (DataGridViewColumn col in dgv.Columns)
        {
            if (!col.Visible) continue;
            totalWidth += col.Width;
        }

        int clientWidth = dgv.ClientSize.Width - (dgv.RowHeadersVisible ? dgv.RowHeadersWidth : 0);

        if (totalWidth > clientWidth)
        {
            // Пропорционально уменьшаем все колонки
            double ratio = (double)clientWidth / totalWidth;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (!col.Visible) continue;
                col.Width = (int)(col.Width * ratio);
            }
        }
    }

    private static void SetupSortableColumns(DataGridView dgv)
    {
        dgv.AllowUserToOrderColumns = false;
        dgv.AllowUserToResizeColumns = true; // по желанию

        foreach (DataGridViewColumn col in dgv.Columns)
        {
            // Для простых типов (string, int, DateTime) достаточно Programmatic
            col.SortMode = DataGridViewColumnSortMode.Automatic;
        }
    }

    private void messagesGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        // Пропускаем заголовки и пустые ячейки
        if (e.RowIndex < 0 || e.Value == null)
            return;

        string status = _messages[e.RowIndex].Status;

        Color foreColor = status switch
        {
            //"OK" => Color.LightGreen,
            //"Warning" => Color.LightYellow,
            //"Error" => Color.LightCoral,
            //_ => Color.White

            "draft" => Color.Gray,
            "sent" => Color.Gray,
            "delivered" => Color.Blue,
            "processing" => Color.Blue,
            "registered" => Color.Green,
            "success" => Color.Green,
            "rejected" => Color.Brown,
            "error" => Color.Red,
            _ => Color.Black
        };

        // Применяем стиль к строке целиком
        messagesGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = foreColor;
    }
}
