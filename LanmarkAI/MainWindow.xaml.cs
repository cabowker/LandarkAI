using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LanmarkAI.Classes;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace LanmarkAI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Image files (*.png; *.jpg) | *.png; *.jpg; *.jpeg | All files (*.*)|*.*";
        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        
        if (openFileDialog.ShowDialog() == true)
        {
            string fileName = openFileDialog.FileName;
            SelectedImage.Source = new BitmapImage(new Uri(fileName));

            MakePredictionAsync(fileName);
        }
    }

    private async void MakePredictionAsync(string fileName)
    {
        string url =
            "https://southcentralus.api.cognitive.microsoft.com/customvision/v3.0/Prediction/555f978d-92e5-490a-9806-cd857a811647/classify/iterations/Iteration1/image";
        string prediction_key = "a97c37ed6b8f4807b62921137f87c037";
        string content_type  = "application/octet-stream";
        var file = File.ReadAllBytes(fileName);

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Prediction-Key", prediction_key);

            using (var content = new ByteArrayContent(file))
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(content_type);
                var response = await client.PostAsync(url, content);
                
                var responseString = await response.Content.ReadAsStringAsync();
                
                List<Prediction> predictions = JsonConvert.DeserializeObject<CustomVision>(responseString).Predictions;
                
                PredictionsListView.ItemsSource = predictions;
            }
        }
    }
}