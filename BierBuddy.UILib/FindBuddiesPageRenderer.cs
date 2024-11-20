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
        

        public WrapPanel GetFindBuddiesPage(double NavBarWidth, double ScreenWidth)
        {
            WrapPanel FindBuddiesPanel = new();
            FindBuddiesPanel.Width = ScreenWidth - NavBarWidth;

            FindBuddiesPanel.Children.Add(GetDislikeButton());
            FindBuddiesPanel.Children.Add(GetProfilePanel());
            FindBuddiesPanel.Children.Add(GetlikeButton());

            return FindBuddiesPanel;
        }


        private UIElement GetDislikeButton() 
        {
            Button disLikeButton = new();
            disLikeButton.Content = "Dislike";
            disLikeButton.VerticalAlignment = VerticalAlignment.Center;
            disLikeButton.HorizontalAlignment = HorizontalAlignment.Center;

            return disLikeButton;
        }


        private UIElement GetProfilePanel()
        {
            // Create the Grid
            Grid profileGrid = new Grid();
            profileGrid.Width = 400;
            profileGrid.Height = 250;
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

            profileGrid.Background = UIUtils.BabyPoeder;
            

            return profileGrid;
        }
        private UIElement GetProfileBanner()
        {
            WrapPanel bannerPanel = new WrapPanel();


            bannerPanel.Children.Add(GetNameLabel());
            bannerPanel.Children.Add(GetProfileBannerDot());
            bannerPanel.Children.Add(GetAgeLabel());

            bannerPanel.HorizontalAlignment = HorizontalAlignment.Center;
            bannerPanel.VerticalAlignment = VerticalAlignment.Center;

            ProfileContentBorder profileContentBorder = new ProfileContentBorder();
            profileContentBorder.Child = bannerPanel;

            return bannerPanel;
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



        private UIElement GetlikeButton()
        {
            Button likeButton = new();
            likeButton.Content = "LIKE";
            likeButton.VerticalAlignment = VerticalAlignment.Center;
            likeButton.HorizontalAlignment = HorizontalAlignment.Center;
            return likeButton;
        }

        public void UpdatePageSize(double NavBarWidth, double ScreenWidth)
        {
            
        }
    }

    internal class ProfileContentBorder : Border
    {
        public Label? ProfileContentLabel { get; set; }

        public ProfileContentBorder(string content) : this()
        {
            ProfileContentLabel = new ProfileContentLabel(content);
        }
        public ProfileContentBorder()
        {
            this.Child = ProfileContentLabel;
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
