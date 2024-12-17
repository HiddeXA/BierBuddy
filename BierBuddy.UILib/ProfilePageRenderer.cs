using BierBuddy.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BierBuddy.UILib
{
    public class ProfilePageRenderer : IPageRenderer
    {
        private Grid _ProfilePanel;
        private Size _MainWindowSize;
        private double _NavBarWidth;
        private Visitor _Visitor { get; set; }

        private int BigFontSize = 28;
        private int GeneralFontSize = 16;

        public ProfilePageRenderer()
        {
            _ProfilePanel = new ();
            _Visitor = new(0, "temp", "temp", 0);
        }

        public Grid GetProfilePage(Visitor visitor, bool readOnly)
        {
            _ProfilePanel = new();
            _ProfilePanel.Margin = new Thickness(20);
            _ProfilePanel.Width = _MainWindowSize.Width - _NavBarWidth;
            _ProfilePanel.Height = _MainWindowSize.Height - 130;
            //kolommen voor automatische layout en resizen
            _ProfilePanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            _ProfilePanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            _Visitor = visitor;

            StackPanel profile = GetProfile(readOnly);
            profile.Margin = new Thickness(0, 0, 20, 0);
            Grid.SetColumn(profile, 0);
            _ProfilePanel.Children.Add(profile);

            Border photos = GetPhotos(_Visitor.Photos, readOnly);
            photos.Margin = new Thickness(0, 0, 60, 0);
            Grid.SetColumn(photos, 1);
            _ProfilePanel.Children.Add(photos);

            return _ProfilePanel;
        }

        private StackPanel GetProfile(bool readOnly)
        {
            StackPanel stackPanel = new StackPanel();
            TextBlock pageTitle = new TextBlock { Text = "ACCOUNT", FontSize = BigFontSize, Margin = new Thickness(10, 0, 0, 20), Foreground = UIUtils.BabyPoeder, FontWeight = FontWeights.Bold };
            stackPanel.Children.Add(pageTitle);

            //naam en leeftijd
            Grid nameAndAge = GetNameAndAgeLabels(readOnly);
            stackPanel.Children.Add(nameAndAge);

            //bio
            Border bio = GetBioLabel(readOnly);
            stackPanel.Children.Add(bio);

            //drank
            Border drinks = GetDrinks(readOnly);
            stackPanel.Children.Add(drinks);

            //activiteit
            Border activities = GetActivities(readOnly);
            stackPanel.Children.Add(activities);

            //interesses
            Border interests = GetInterests(readOnly);
            stackPanel.Children.Add(interests);

            return stackPanel;
        }

        private Grid GetNameAndAgeLabels(bool readOnly)
        {
            Grid grid = new ();
            //kolommen en rijen voor layout en automatisch resizen
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            ProfileContentBorder name = new ProfileContentBorder(_Visitor.Name, UIUtils.BabyPoeder, GeneralFontSize);
            name.ProfileContentLabel.Foreground = Brushes.Black;
            name.Margin = new Thickness(0, 0, 20, 0);
            name.ProfileContentLabel.HorizontalAlignment = HorizontalAlignment.Left;
            name.ProfileContentLabel.Padding = new Thickness(20, 10, 0, 10);
            name.VerticalAlignment = VerticalAlignment.Center;

            ProfileContentBorder age = new ProfileContentBorder(_Visitor.Age.ToString(), new SolidColorBrush(Color.FromRgb(190, 194, 188)), GeneralFontSize);
            age.ProfileContentLabel.Foreground = Brushes.Black;
            age.ProfileContentLabel.Padding = new Thickness(0, 10, 0, 10);
            age.VerticalAlignment = VerticalAlignment.Center;

            Grid.SetColumn(name, 0);
            Grid.SetColumn(age, 1);
            grid.Children.Add(name);
            grid.Children.Add(age);

            grid.Margin = new Thickness(10, 0, 0, 10);

            return grid;
        }

        private ProfileContentBorder GetBioLabel(bool readOnly)
        {
            ProfileContentBorder bio = new(UIUtils.BabyPoeder);
            bio.Height = _MainWindowSize.Height / 6;
            bio.Child = new TextBlock { Text = _Visitor.Bio, TextWrapping = TextWrapping.Wrap, FontSize = GeneralFontSize, Foreground = Brushes.Black, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left, Padding = new Thickness(20, 10, 0, 10) };

            return bio;
        }

        private Border GetDrinks(bool readOnly)
        {
            if (readOnly)
            {
                return GetItemList(_Visitor.DrinkPreference.ToArray());
            }
            else
            {
                return GetCustomItemList(_Visitor.DrinkPreference.ToArray(), new Dictionary<long, string>());
            }
        }

        private Border GetActivities(bool readOnly)
        {
            if (readOnly)
            {
                return GetItemList(_Visitor.ActivityPreference.ToArray());
            }
            else
            {
                return GetCustomItemList(_Visitor.ActivityPreference.ToArray(), new Dictionary<long, string>());
            }
        }

        private Border GetInterests(bool readOnly)
        {
            if (readOnly)
            {
                return GetItemList(_Visitor.Interests.ToArray());
            }
            else
            {
                return GetCustomItemList(_Visitor.Interests.ToArray(), new Dictionary<long, string>());
            }
        }

        private Border GetItemList(string[] items)
        {
            ProfileContentBorder border = new ProfileContentBorder(UIUtils.BabyPoeder);
            WrapPanel wrapPanel = new ();
            wrapPanel.Orientation = Orientation.Horizontal;
            wrapPanel.VerticalAlignment = VerticalAlignment.Center;

            //voeg alle items toe aan de wrapPanel
            foreach (string item in items)
            {
                ProfileContentBorder profileContentBorder = new ProfileContentBorder(item, new SolidColorBrush(Color.FromRgb(108, 114, 109)), GeneralFontSize);
                profileContentBorder.VerticalAlignment = VerticalAlignment.Center;
                profileContentBorder.ProfileContentLabel.Padding = new Thickness(20, 10, 20, 10);
                wrapPanel.Children.Add(profileContentBorder);
            }

            border.Child = wrapPanel;
            return border;
        }

        private Border GetCustomItemList(string[] items, Dictionary<long, string> options)
        {
            throw new NotImplementedException();
        }

        private Border GetPhotos(List<string> photos, bool readOnly)
        {
            Border BGborder = new ProfileContentBorder(UIUtils.Outer_Space);
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < photos.Count; i++) {
                string photo = photos[i];
                if (i < 2) // 2 foto's per rij
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }
                else if (i == 2) // 2 kolommen
                {
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                }
                if (!photo.Equals("Geen URL gevonden")) // als er echt een foto is
                {
                    Border border = GetPhoto(photo);
                    Grid.SetColumn(border, i % 2);
                    Grid.SetRow(border, i / 2);
                    grid.Children.Add(border);
                }
            }

            BGborder.Child = grid;

            return BGborder;
        }

        private Border GetPhoto(string photo)
        {
            ProfileContentBorder profileContentBorder = new ProfileContentBorder(UIUtils.Onyx70);
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(photo));
            //afbeelding centreren in de border
            image.HorizontalAlignment = HorizontalAlignment.Center;
            image.VerticalAlignment = VerticalAlignment.Center;

            //afbeelding clippen zodat het in de border past, zelfs na het resizen
            profileContentBorder.SizeChanged += (s, e) =>
            {
                double borderWidth = profileContentBorder.ActualWidth;
                double borderHeight = profileContentBorder.ActualHeight;

                profileContentBorder.Clip = new RectangleGeometry(
                    new Rect(0, 0, borderWidth, borderHeight),
                    profileContentBorder.CornerRadius.TopLeft,
                    profileContentBorder.CornerRadius.TopLeft
                );
            };

            profileContentBorder.Child = image;
            return profileContentBorder;
        }

        public void UpdatePageSize(double newNavBarWidth, Size newScreenSize)
        {
            _NavBarWidth = newNavBarWidth;
            _MainWindowSize = newScreenSize;
            _ProfilePanel.Width = _MainWindowSize.Width - _NavBarWidth;
            _ProfilePanel.Height = _MainWindowSize.Height - 80;
        }
    }
}
