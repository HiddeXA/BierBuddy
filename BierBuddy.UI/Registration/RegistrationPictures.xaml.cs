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
    IDataAccess DataAccess { get; set; }
    Visitor registrationVisitor;
    Byte[] photo1;
    Byte[] photo2;
    Byte[] photo3;
    Byte[] photo4;
    public RegistrationPictures(Visitor registrationVisitor, IDataAccess dataAccess)
    {
        this.registrationVisitor = registrationVisitor;
        DataAccess = dataAccess;
        InitializeComponent();
    }

    private void Next_OnClick(object sender, RoutedEventArgs e)
    {
        if (photo1 == null && photo2 == null && photo3 == null && photo4 == null)
        {
            MessageBox.Show("Voeg minstens 1 foto toe");
            return;
        }
        
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
        
        RegistrationComplete registrationComplete = new RegistrationComplete(registrationVisitor, DataAccess);
        registrationComplete.Show();
        this.Close();
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
            case "Img1":
               image = Image1;
                break;
            case "Img2":
                image = Image2;
                break;
            case "Img3":
                image = Image3;
                break;
            default:
                image = Image4;
                break;
        }
       

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                byte[] photo = File.ReadAllBytes(openFileDialog.FileName);

                switch (name)
                {
                    case "Img1":
                        photo1 = photo;
                        break;
                    case "Img2":
                        photo2 = photo;
                        break;
                    case "Img3":
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

    private void Back_OnClick(object sender, RoutedEventArgs e)
    {
        RegistrationActivityPrefrence registrationActivityPrefrence = new RegistrationActivityPrefrence(registrationVisitor, DataAccess);
        registrationActivityPrefrence.Show();
        this.Close();
    }
}