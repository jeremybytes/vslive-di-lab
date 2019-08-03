namespace PeopleLibrary
{
    public class StartMonthPersonFormatter : IPersonFormatter
    {
        public string DisplayName => "Include Start Month";

        public string Format(Person person)
        {
            return $"{person.FamilyName} in {person.StartDate:MMM}";
        }
    }
}
