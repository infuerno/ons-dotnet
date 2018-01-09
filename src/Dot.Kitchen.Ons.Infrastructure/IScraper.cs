namespace Dot.Kitchen.Ons.Infrastructure
{
    public interface IScraper
    {
        ScrapeResults Execute(string surname);
    }
}