namespace BlankWorder.Models
{
    public class NavigationMenuItem
    {
        public string Text { get; }
        public string Icon { get; }
        public string NavigateTo { get; }

        public NavigationMenuItem(string text, string navToPageTag, string icon)
        {
            Text = text;
            Icon = icon;
            NavigateTo = navToPageTag;
        }
    }
}
