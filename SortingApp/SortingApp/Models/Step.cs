namespace SortingApp.Models
{
    public class Step
    {
        // Index by correct order 
        public int ID { get; set; }
        public string Text { get; set; }
        // Mixed order number
        public int Index { get; set; }

        public Step(int iD, string text, int index)
        {
            ID = iD;
            Text = text;
            Index = index;
        }

        public override string ToString()
        {
            return Index + ":" + Text;
        }
    }
}
