namespace Entity
{
    public class CharacterName
    {
        public string First;
        public string Middle;
        public string Last;
        public string Full => $"{First} {Last}";
        
        public enum ImpliedGender
        {
            None,
            Neutral,
            Masc,
            Femme,
        }

        public CharacterName(string firstName, string lastName)
        {
            First = firstName;
            Last = lastName;
        }

        public CharacterName(string firstName, string lastName, string middleName)
        {
            First = firstName;
            Last = lastName;
            Middle = middleName;
        }
    }
}