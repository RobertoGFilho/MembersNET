using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace MembersNET
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Member> Members { get; set; }

        public MainPage()
        {
            Members = [];
            BindingContext = this;
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (!Members.Any())
            {
                await LoadData();
            }
        }

        private async Task LoadData()
        {
            using var client = new HttpClient();
            string url = "https://api.github.com/orgs/dotnet/members";

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<List<Member>>(content);

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        Members.Add(item);
                    }
                }
            }
            else
            {
                await DisplayAlert("Alerta", "Houve um erro inesperado", "Ok");
            }
        }
    }

    public class Member
    {
        [JsonProperty("login")]
        public string? Login { get; set; }


        [JsonProperty("avatar_url")]
        public string? AvatarUrl { get; set; }
    }

}
