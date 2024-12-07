using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace news_feed_app
{
    public partial class _default : Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await GetNewsFeed();
            }
        }

        public async Task GetNewsFeed()
        {
            string API_URL = "http://feeds.bbci.co.uk/news/world/rss.xml";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(API_URL);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        XDocument feed = XDocument.Parse(content);

                        // Seleccionar elementos del feed
                        var items = feed.Descendants("item").Select(i => new
                        {
                            Title = (string)i.Element("title"),
                            Link = (string)i.Element("link"),
                            Description = (string)i.Element("description") ?? "No description available",
                            PubDate = (string)i.Element("pubDate"),
                            Thumbnail = (string)i.Element("{http://search.yahoo.com/mrss/}thumbnail")?.Attribute("url")
                        });

                        // Crear elementos HTML dinámicamente
                        foreach (var item in items)
                        {
                            // Card
                            HtmlGenericControl div = new HtmlGenericControl("div");
                            div.Attributes.Add("class", "news-item card");

                            // Agregar imagen si existe
                            if (!string.IsNullOrEmpty(item.Thumbnail))
                            {
                                HtmlGenericControl img = new HtmlGenericControl("img");
                                img.Attributes.Add("class", "card-img-top");
                                img.Attributes.Add("src", item.Thumbnail);
                                img.Attributes.Add("alt", item.Title);
                                div.Controls.Add(img);
                            }

                            // Fecha de publicación
                            DateTime pubDate = DateTime.Parse(item.PubDate);
                            TimeSpan timeSincePub = DateTime.Now - pubDate;
                            double totalMinutes = timeSincePub.TotalMinutes;
                            string unit;
                            double value;

                            if (totalMinutes < 1)
                            {
                                value = timeSincePub.TotalSeconds;
                                unit = "seconds";
                            }
                            else if (totalMinutes < 60)
                            {
                                value = totalMinutes;
                                unit = "minutes";
                            }
                            else if (totalMinutes < 1440) // 24 * 60
                            {
                                value = timeSincePub.TotalHours;
                                unit = "hours";
                            }
                            else
                            {
                                value = timeSincePub.TotalDays;
                                unit = "days";
                            }

                            // Redondear el valor y determinar la unidad
                            double roundedValue = Math.Round(value);
                            unit = roundedValue == 1 ? unit.TrimEnd('s') : unit;

                            HtmlGenericControl date = new HtmlGenericControl("p");
                            date.Attributes.Add("class", "text-sm text-secondary");
                            date.InnerText = roundedValue + " " + unit + " ago";

                            // Titulo
                            HtmlGenericControl h2 = new HtmlGenericControl("h2");
                            h2.Attributes.Add("class", "card-title");
                            h2.InnerText = item.Title;

                            // Descripcion
                            HtmlGenericControl p = new HtmlGenericControl("p");
                            p.Attributes.Add("class", "card-text");
                            p.InnerText = item.Description;

                            // Link
                            HtmlGenericControl a = new HtmlGenericControl("a");
                            a.Attributes.Add("class", "btn btn-primary");
                            a.Attributes.Add("href", item.Link);
                            a.Attributes.Add("target", "_blank");
                            a.InnerText = "Read more";

                            // Agregar los componentes
                            div.Controls.Add(date);
                            div.Controls.Add(h2);
                            div.Controls.Add(p);
                            div.Controls.Add(a);

                            // Agregar el contenedor al control newsFeed
                            newsFeed.Controls.Add(div);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejar errores y mostrar un mensaje en el feed
                    HtmlGenericControl errorDiv = new HtmlGenericControl("div");
                    errorDiv.Attributes.Add("class", "error alert alert-danger");
                    errorDiv.InnerText = "Error fetching the news feed: " + ex.Message;
                    newsFeed.Controls.Add(errorDiv);
                }
            }
        }
    }
}
