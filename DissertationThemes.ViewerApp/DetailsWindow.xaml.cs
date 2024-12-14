using System.Windows;


namespace DissertationThemes.ViewerApp
{
    public partial class DetailsWindow : Window
    {
        public DetailsWindow(dynamic theme)
        {
            InitializeComponent();
            IdTextBlock.Text = theme.Id.ToString();
            NameTextBlock.Text = theme.Name.ToString();
            SupervisorTextBlock.Text = theme.Supervisor.ToString();
            StudyProgramTextBlock.Text = theme.programDetails;
            FullTimeStudyTextBlock.Text = theme.IsFullTimeStudy ? "Yes" : "No";
            ExternalStudyTextBlock.Text = theme.IsExternalStudy ? "Yes" : "No";
            ResearchTypeTextBlock.Text = theme.ResearchType;
            DescriptionTextBox.Text = theme.Description;
            CreatedTextBlock.Text = theme.Created.ToString("M/dd/yyyy H:mm:ss tt");
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        { 
            this.Close();
        }
    }
}
