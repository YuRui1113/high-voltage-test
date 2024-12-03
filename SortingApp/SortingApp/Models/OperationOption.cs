namespace SortingApp.Models
{
    public class OperationOption
    {
        public OperationType Type  { get; set; }
        public string Label { get; set; }

        public override string ToString()
        {
            return Label;
        }
    }
}
