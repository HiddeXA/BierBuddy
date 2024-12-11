using BierBuddy.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BierBuddy.UILib
{
    public class ProfilePageRenderer : IPageRenderer
    {
        private Canvas _ProfilePanel;
        private Size _MainWindowSize;
        private double _NavBarWidth;
        private Image _ProfilePicture;
        private Visitor _Visitor { get; set; }
        private FindBuddies _FindBuddies;

        private int BigFontSize = 28;
        private int GeneralFontSize = 16;

        private readonly int _MinFontSizeBig = 20;
        private readonly int _MinFontSizeGeneral = 12;

        public ProfilePageRenderer()
        {
            _ProfilePanel = new Canvas();
            _ProfilePicture = new Image();
            _Visitor = new(0, "temp", "temp", 0);
        }
        public Canvas GetProfilePage(Visitor visitor, bool readOnly)
        {
            _ProfilePanel = new();
            _ProfilePanel.Margin = new Thickness(20);
            _ProfilePanel.VerticalAlignment = VerticalAlignment.Center;
            _Visitor = visitor;

            _ProfilePanel = new();
            double panelWidth = _MainWindowSize.Width - _NavBarWidth;

            StackPanel profile = GetProfile(readOnly);
            _ProfilePanel.Children.Add(profile);

            _ProfilePanel.VerticalAlignment = VerticalAlignment.Center;
            _ProfilePanel.HorizontalAlignment = HorizontalAlignment.Center;

            return _ProfilePanel;
        }

        public StackPanel GetProfile(bool readOnly)
        {
            StackPanel stackPanel = new StackPanel();
            TextBlock pageTitle = new TextBlock { Text = "ACCOUNT", FontSize = BigFontSize, Margin = new Thickness(0, 0, 0, 20), Foreground = UIUtils.BabyPoeder };
            stackPanel.Children.Add(pageTitle);

            //naam en leeftijd
            StackPanel nameAndAge = GetNameAndAgeLabels(readOnly);
            stackPanel.Children.Add(nameAndAge);

            //bio
            UIElement bio = GetBioLabel(readOnly);
            stackPanel.Children.Add(bio);

            //drank
            StackPanel drinks = GetDrinks(readOnly);
            stackPanel.Children.Add(drinks);

            //activiteit
            StackPanel activities = GetActivities(readOnly);
            stackPanel.Children.Add(activities);

            //interesses
            StackPanel interests = GetInterests(readOnly);
            stackPanel.Children.Add(interests);

            return stackPanel;
        }

        public StackPanel GetNameAndAgeLabels(bool readOnly)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Margin = new Thickness(0, 0, 0, 20);

            ProfileContentBorder name = new ProfileContentBorder(_Visitor.Name, UIUtils.BabyPoeder, GeneralFontSize);
            ProfileContentBorder age = new ProfileContentBorder(_Visitor.Age.ToString(), new SolidColorBrush(Color.FromRgb(190, 194, 188)), GeneralFontSize);
            name.ProfileContentLabel.Foreground = Brushes.Black;
            age.ProfileContentLabel.Foreground = Brushes.Black;
            stackPanel.Children.Add(name);
            stackPanel.Children.Add(age);

            return stackPanel;
        }

        public UIElement GetBioLabel(bool readOnly)
        {
            ProfileContentBorder bio = new(_Visitor.Bio, UIUtils.BabyPoeder, GeneralFontSize);
            bio.ProfileContentLabel.Foreground = Brushes.Black;

            return bio;
        }

        public StackPanel GetDrinks(bool readOnly)
        {
            if (!readOnly)
            {
                return GetItemList(_Visitor.DrinkPreference.ToArray());
            }
            else
            {
                return GetCustomItemList(_Visitor.DrinkPreference.ToArray(), new Dictionary<long, string>());
            }
        }

        public StackPanel GetActivities(bool readOnly)
        {
            if (!readOnly)
            {
                return GetItemList(_Visitor.ActivityPreference.ToArray());
            }
            else
            {
                return GetCustomItemList(_Visitor.ActivityPreference.ToArray(), new Dictionary<long, string>());
            }
        }

        public StackPanel GetInterests(bool readOnly)
        {
            if (!readOnly)
            {
                return GetItemList(_Visitor.Interests.ToArray());
            }
            else
            {
                return GetCustomItemList(_Visitor.Interests.ToArray(), new Dictionary<long, string>());
            }
        }

        public StackPanel GetItemList(string[] items)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Margin = new Thickness(0, 0, 0, 20);
            stackPanel.Background = UIUtils.BabyPoeder;

            foreach (string item in items)
            {
                ProfileContentBorder profileContentBorder = new ProfileContentBorder(item, UIUtils.Onyx, GeneralFontSize);
                stackPanel.Children.Add(profileContentBorder);
            }

            return stackPanel;
        }

        public StackPanel GetCustomItemList(string[] items, Dictionary<long, string> options)
        {
            throw new NotImplementedException();
        }

        public void UpdatePageSize(double newNavBarWidth, Size newScreenSize)
        {
            _NavBarWidth = newNavBarWidth;
            _MainWindowSize = newScreenSize;

            // Fontsize aanpassen
            if (_MainWindowSize.Width < 1500)
            {
                BigFontSize = _MinFontSizeBig;
                GeneralFontSize = _MinFontSizeGeneral;
            }
            else
            {
                BigFontSize = 28;
                GeneralFontSize = 16;
            }
        }
    }
}
