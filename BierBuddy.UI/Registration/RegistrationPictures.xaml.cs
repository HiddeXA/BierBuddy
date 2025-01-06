using System.IO;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using BierBuddy.Core;
using Microsoft.Win32;

namespace BierBuddy.UI.Registration;

public partial class RegistrationPictures : Window
{
    Visitor registrationVisitor;
    Byte[] photo1;
    Byte[] photo2;
    Byte[] photo3;
    Byte[] photo4;
    public RegistrationPictures(Visitor registrationVisitor)
    {
        this.registrationVisitor = registrationVisitor;
        
        InitializeComponent();
    }

    private void Next_OnClick(object sender, RoutedEventArgs e)
    {
        if (photo1 != null)
        {
            registrationVisitor.AddToPhotos(photo1);
        }
        if (photo2 != null)
        {
            registrationVisitor.AddToPhotos(photo2);
        }
        if (photo3 != null)
        {
            registrationVisitor.AddToPhotos(photo3);
        }
        if (photo4 != null)
        {
            registrationVisitor.AddToPhotos(photo4);
        }
        
        RegistrationComplete registrationInterests = new RegistrationComplete(registrationVisitor);
    }

    private void ImageClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "Image Files|*.jpg;*.jpeg;*.png;"
        };
        
        string name = ((Button)sender).Name;
        Image image;
        switch (name)
        {
            case "Image1":
               image = image1;
                break;
            case "Image2":
                image = image2;
                break;
            case "Image3":
                image = image3;
                break;
            default:
                image = image4;
                break;
        }
       

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                byte[] photo = File.ReadAllBytes(openFileDialog.FileName);

                switch (name)
                {
                    case "Image1":
                        photo1 = photo;
                        break;
                    case "Image2":
                        photo2 = photo;
                        break;
                    case "Image3":
                        photo3 = photo;
                        break;
                    default:
                        photo4 = photo;
                        break;
                }
                
                
                image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}