using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using Material.Icons;
using Material.Icons.WPF;

namespace BierBuddy.UILib
{
    public class FindBuddiesPageRenderer : IPageRenderer
    {

        public WrapPanel GetFindBuddiesPage(double navBarWidth, double screenWidth, double screenHeight)
        {



            WrapPanel FindBuddiesPanel = new();
            double panelWidth = screenWidth - navBarWidth;

            FindBuddiesPanel.Children.Add(GetDislikeButton(panelWidth / 4 , screenHeight));
            FindBuddiesPanel.Children.Add(GetProfilePanel(panelWidth / 2 , screenHeight));
            FindBuddiesPanel.Children.Add(GetlikeButton(panelWidth / 4 , screenHeight));

            FindBuddiesPanel.VerticalAlignment = VerticalAlignment.Center;
            FindBuddiesPanel.HorizontalAlignment = HorizontalAlignment.Center;

            return FindBuddiesPanel;
        }

        private UIElement GetDislikeButton(double width, double height) 
        {
            DockPanel dislikeButtonPanel = new();
            dislikeButtonPanel.Width = width;
            dislikeButtonPanel.Height = height;

            Button dislikeButton = new();
            dislikeButton.Template = GetLikeButtonTemplate(new SolidColorBrush(Color.FromArgb(0xFF, 0xBE, 0x37, 0x32)));
            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.GlassMugOff;
            icon.Foreground = UIUtils.BabyPoeder;
            dislikeButton.Content = icon;

            dislikeButton.Width = width / 3;
            dislikeButton.VerticalAlignment = VerticalAlignment.Center;
            dislikeButton.HorizontalAlignment = HorizontalAlignment.Center;
            dislikeButton.Background = UIUtils.Transparent;

            dislikeButtonPanel.Children.Add(dislikeButton);

            return dislikeButtonPanel;
        }
        private UIElement GetlikeButton(double width, double height)
        {
            DockPanel likeButtonPanel = new();
            likeButtonPanel.Width = width;
            likeButtonPanel.Height = height;

            Button likeButton = new();
            likeButton.Template = GetLikeButtonTemplate(new SolidColorBrush(Color.FromArgb(0xFF, 0x7E, 0xA1, 0x72)));
           
            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.GlassMug;
            icon.Foreground = UIUtils.BabyPoeder;
            likeButton.Content = icon;
            likeButton.Width = width / 3;
            likeButton.VerticalAlignment = VerticalAlignment.Center;
            likeButton.HorizontalAlignment = HorizontalAlignment.Center;
            likeButton.Background = UIUtils.Transparent;

            likeButtonPanel.Children.Add(likeButton);
            return likeButtonPanel;
        }
        private ControlTemplate GetLikeButtonTemplate(Brush brush)
        {
            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));

            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            //set background color
            borderFactory.SetValue(Border.BackgroundProperty, brush);
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(90));
            gridFactory.AppendChild(borderFactory);

            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            gridFactory.AppendChild(contentPresenterFactory);

            template.VisualTree = gridFactory;

            return template;
        }
        private UIElement GetProfilePanel(double width, double height)
        {
            height -= 150;
            Canvas profilePanel = new();
            profilePanel.Width = width;
            profilePanel.Height = height;

            UIElement profilePicture = GetProfilePicture(width);
            profilePanel.Children.Add(profilePicture);

            
            UIElement profileContent = GetProfileContentPanel(width);
            Canvas.SetTop(profileContent, height - UIUtils.ProfileConentHeight);
            profilePanel.Children.Add(profileContent);

            profilePanel.Margin = new Thickness(20);

            Border profileBorder = new();
            profileBorder.Background = UIUtils.Outer_Space;
            profileBorder.Child = profilePanel;
            profileBorder.CornerRadius = UIUtils.UniversalCornerRadius;
            profilePanel.VerticalAlignment = VerticalAlignment.Center;
            
            return profileBorder;
        }
        
        private UIElement GetProfilePicture(double width)
        {
            Label tempFoto = new Label();
            tempFoto.Content = "FOTO!";
            tempFoto.Foreground = UIUtils.testMarking;
            return  tempFoto;
        }

        private UIElement GetProfileContentPanel(double width)
        {
            // Create the Grid
            Grid profileGrid = new Grid();
            profileGrid.Width = width;
            profileGrid.Height = UIUtils.ProfileConentHeight;
            profileGrid.HorizontalAlignment = HorizontalAlignment.Left;
            profileGrid.VerticalAlignment = VerticalAlignment.Top;
            profileGrid.ShowGridLines = true;

            // Define the Columns
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            profileGrid.ColumnDefinitions.Add(colDef1);
            profileGrid.ColumnDefinitions.Add(colDef2);
            profileGrid.ColumnDefinitions.Add(colDef3);

            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            profileGrid.RowDefinitions.Add(rowDef1);
            profileGrid.RowDefinitions.Add(rowDef2);
            profileGrid.RowDefinitions.Add(rowDef3);

            UIElement ProfileBanner = GetProfileBanner();
            Grid.SetColumnSpan(ProfileBanner, 3);
            Grid.SetRow(ProfileBanner, 1);

            UIElement drinkLabel = GetDrinkLabel();
            Grid.SetColumn(drinkLabel, 0);
            Grid.SetRow(drinkLabel, 2);

            UIElement activityLabel = GetActivityLabel();
            Grid.SetColumn(activityLabel, 1);
            Grid.SetRow(activityLabel, 2);

            UIElement interestLabel = GetInterestLabel();
            Grid.SetColumn(interestLabel, 2);
            Grid.SetRow(interestLabel, 2);

            profileGrid.Children.Add(ProfileBanner);
            profileGrid.Children.Add(drinkLabel);
            profileGrid.Children.Add(activityLabel);
            profileGrid.Children.Add(interestLabel);

            Border profileContentBorder = new Border();
            //gradient toevoegen
            LinearGradientBrush gradientBrush = new LinearGradientBrush
            {
                StartPoint = new System.Windows.Point(0, 0), // Bovenkant
                EndPoint = new System.Windows.Point(0, 1)   // Onderkant
            };
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFC, 0xFF, 0xF7), 1));
            profileContentBorder.Background = gradientBrush;
            profileContentBorder.CornerRadius = new CornerRadius(0, 0, 40, 40);

            profileContentBorder.Child = profileGrid;

            return profileContentBorder;
        }
        private UIElement GetProfileBanner()
        {
            WrapPanel bannerPanel = new WrapPanel();

            bannerPanel.Children.Add(GetNameLabel());
            bannerPanel.Children.Add(GetProfileBannerDot());
            bannerPanel.Children.Add(GetAgeLabel());

            bannerPanel.HorizontalAlignment = HorizontalAlignment.Center;
            bannerPanel.VerticalAlignment = VerticalAlignment.Center;

            ProfileContentBorder contentBannerBorder = new ProfileContentBorder();
            contentBannerBorder.Child = bannerPanel;
            return contentBannerBorder;
        }
        private UIElement GetNameLabel()
        {
            ProfileContentLabel nameLabel = new ProfileContentLabel("Rick");
            return nameLabel;
        }
        private UIElement GetProfileBannerDot()
        {
            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.Dot;
            ProfileContentLabel nameLabel = new ProfileContentLabel();
            nameLabel.Content = icon;
            nameLabel.FontSize = 56;
            return nameLabel;
        }
        private UIElement GetAgeLabel()
        {
            ProfileContentLabel ageLabel = new ProfileContentLabel("19");
            return ageLabel;
        }
        private UIElement GetDrinkLabel()
        {
            ProfileContentBorder profileContentBorder = new ProfileContentBorder("sambuca");
            return profileContentBorder;
        }
        private UIElement GetActivityLabel()
        {
            ProfileContentBorder profileContentBorder = new ProfileContentBorder("darten");
            return profileContentBorder;
        }
        private UIElement GetInterestLabel()
        {
            ProfileContentBorder profileContentBorder = new ProfileContentBorder("OIKOS");
            return profileContentBorder;
        }

        public void UpdatePageSize(double newNavBarWidth, double newScreenWidth)
        {
            
        }
    }

    internal class ProfileContentBorder : Border
    {
        public Label? ProfileContentLabel { get; set; }

        public ProfileContentBorder(string content) : this()
        {
            ProfileContentLabel = new ProfileContentLabel(content);
            this.Child = ProfileContentLabel;
        }
        public ProfileContentBorder()
        {
            this.Background = UIUtils.Onyx70;
            this.CornerRadius = UIUtils.UniversalCornerRadius;
            this.Margin = new Thickness(10);
        }
    }
    internal class ProfileContentLabel : Label
    {
        public ProfileContentLabel(string content) : this()
        {
            this.Content = content;
        }
        public ProfileContentLabel()
        {
            this.Foreground = UIUtils.BabyPoeder;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.FontFamily = UIUtils.ÜniversalFontFamily;
            this.FontSize = 30;
        }
    }
}
