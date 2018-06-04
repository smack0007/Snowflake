namespace Snowflake.Tests.Types
{
    public class Person
	{
		public string FirstName
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public int Age
		{
			get;
			set;
		}

        public Person()
        {
        }

        public Person(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }
	}
}
