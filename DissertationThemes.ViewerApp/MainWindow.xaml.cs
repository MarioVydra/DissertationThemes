using DissertationThemes.WebApi.Dtos;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace DissertationThemes.ViewerApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ThemeDto> themes { get; set; } = new ObservableCollection<ThemeDto>();
        public ObservableCollection<StProgramDto> stPrograms { get; set; } = new ObservableCollection<StProgramDto>();
        public ObservableCollection<int> years { get; set; } = new ObservableCollection<int>();
        public int? selectedStProgramId { get; set; }
        public int? selectedYear { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            this.LoadThemes();
            this.LoadStPrograms();
            this.LoadYears();
        }

        private async Task LoadThemes()
        {
            try
            {
                using HttpClient client = new HttpClient();
                string apiUrl = "https://localhost:7016/themes";

                var queryParams = new List<string>();

                if (selectedStProgramId.HasValue)
                {
                    queryParams.Add($"stProgramId={selectedStProgramId.Value}");
                }
                if (selectedYear.HasValue)
                {
                    queryParams.Add($"year={selectedYear.Value}");
                }

                if (queryParams.Count > 0)
                {
                    apiUrl = $"{apiUrl}?{string.Join("&", queryParams)}";
                }

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var themes = JsonSerializer.Deserialize<List<ThemeDto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    if (themes != null)
                    {
                        this.themes.Clear();
                        foreach (var theme in themes)
                        {
                            this.themes.Add(theme);
                        }
                        themesCount.Text = this.themes.Count.ToString();
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching themes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private async Task LoadStPrograms()
        {
            try
            {
                using HttpClient client = new HttpClient();
                string apiUrl = "https://localhost:7016/stprograms";
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var programs = JsonSerializer.Deserialize<List<StProgramDto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    if (programs != null)
                    {
                        this.stPrograms.Clear();
                        foreach (var program in programs)
                        {
                            this.stPrograms.Add(program);
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching programs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private async Task LoadYears()
        {
            try
            {
                using HttpClient client = new HttpClient();
                string apiUrl = "https://localhost:7016/themesyears";
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var years = JsonSerializer.Deserialize<List<int>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    this.years.Clear();
                    if (years != null)
                    {
                        foreach (var year in years)
                        {
                            this.years.Add(year);
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching years: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            selectedStProgramId = null;
            selectedYear = null;
            StProgramComboBox.SelectedItem = null;
            YearComboBox.SelectedItem = null;

            this.LoadThemes();
        }

        private void StProgramChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LoadThemes();
        }

        private void YearChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LoadThemes();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        { 
            Application.Current.Shutdown();
        }

        private async void ExportToCsv_Click(object sender, RoutedEventArgs e)
        { 
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = ".csv";
            string fileName = "phd_temy";

            if (selectedYear.HasValue)
            {
                fileName += $"-{selectedYear}";
            }

            if (StProgramComboBox.SelectedItem != null)
            { 
                var selectedProgram = StProgramComboBox.SelectedItem as StProgramDto;
                if (selectedProgram != null)
                {
                    fileName += $"-{selectedProgram.Name}";
                }
            }
            fileName += ".csv";

            saveFileDialog.FileName = fileName;


            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                try
                {
                    using HttpClient client = new HttpClient();
                    string apiUrl = "https://localhost:7016/themes2csv";

                    var queryParams = new List<string>();

                    if (selectedStProgramId.HasValue)
                    {
                        queryParams.Add($"stProgramId={selectedStProgramId.Value}");
                    }
                    if (selectedYear.HasValue)
                    {
                        queryParams.Add($"year={selectedYear.Value}");
                    }

                    if (queryParams.Count > 0)
                    {
                        apiUrl = $"{apiUrl}?{string.Join("&", queryParams)}";
                    }

                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] csvData = await response.Content.ReadAsByteArrayAsync();

                        File.WriteAllBytes(path, csvData);
                    }
                    else
                    {
                        MessageBox.Show($"Error: {response.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting to CSV: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        { 
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            aboutWindow.ShowDialog();
        }

        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            var selectedTheme = ThemesListView.SelectedItem as ThemeDto;
            var selectedProgram = stPrograms.FirstOrDefault(p => p.Id == selectedTheme.StProgramId);
            string programDetails = selectedProgram != null ? $"{selectedProgram.Name} ({selectedProgram.FieldOfStudy})" : "Unknown";

            var detailedTheme = new
            {
                selectedTheme.Id,
                selectedTheme.Name,
                selectedTheme.Supervisor,
                programDetails,
                selectedTheme.IsFullTimeStudy,
                selectedTheme.IsExternalStudy,
                selectedTheme.ResearchType,
                selectedTheme.Description,
                selectedTheme.Created
            };


            if (selectedTheme != null)
            {
                DetailsWindow detailsWindow = new DetailsWindow(detailedTheme);
                detailsWindow.Owner = this;
                detailsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                detailsWindow.ShowDialog();
            }
        }

        private void ThemesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTheme = ThemesListView.SelectedItem as ThemeDto;

            if (selectedTheme != null)
            {
                ShowDetailsButton.IsEnabled = true;
                GenerateDocButton.IsEnabled = true;
            }
            else {
                ShowDetailsButton.IsEnabled = false;
                GenerateDocButton.IsEnabled = false;
            }
        }

        private async void GenerateDoc_Click(object sender, RoutedEventArgs e)
        {
            var selectedTheme = ThemesListView.SelectedItem as ThemeDto;

            if (selectedTheme != null)
            {
                try
                {
                    using HttpClient client = new HttpClient();
                    string apiUrl = $"https://localhost:7016/theme2docx/{selectedTheme.Id}";

                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] docxContent = await response.Content.ReadAsByteArrayAsync();

                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "Word Documents (*.docx)|*.docx|All files (*.*)|*.*";
                        saveFileDialog.DefaultExt = ".docx";
                        saveFileDialog.FileName = $"{selectedTheme.Name}.docx";

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string path = saveFileDialog.FileName;
                            File.WriteAllBytes(path, docxContent);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Error: {response.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating document: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}