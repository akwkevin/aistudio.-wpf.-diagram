namespace AIStudio.Wpf.Flowchart
{
    /// <summary>
    /// 前端SelectOption
    /// </summary>
    public class SelectOption
    {
        public string value { get; set; }
        public string text { get; set; }

        public override string ToString()
        {
            return text;
        }
    }
}
