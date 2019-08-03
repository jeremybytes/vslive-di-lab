namespace PeopleLibrary
{
    public class RatingPersonFormatter : IPersonFormatter
    {
        public string DisplayName => "Rating Formatter";

        public string Format(Person person)
        {
            return $"{person.GivenName} has {person.Rating} Stars";
        }
    }
}
